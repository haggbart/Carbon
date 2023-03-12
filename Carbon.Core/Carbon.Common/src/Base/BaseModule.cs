﻿using System;
using System.IO;
using System.Reflection;
using Carbon.Base.Interfaces;
using Carbon.Core;
using Carbon.Extensions;
using Oxide.Core.Configuration;

/*
 *
 * Copyright (c) 2022-2023 Carbon Community 
 * All rights reserved.
 *
 */

namespace Carbon.Base;

public abstract class BaseModule : BaseHookable
{
	public virtual bool EnabledByDefault => false;
	public virtual bool ForceModded => false;

	public abstract bool GetEnabled();

	public static T GetModule<T>()
	{
		foreach (var module in Community.Runtime.ModuleProcessor.Modules)
		{
			if (module.GetType() == typeof(T) && module is T result) return result;
		}

		return default;
	}
}

public class EmptyModuleConfig { }
public class EmptyModuleData { }

public abstract class CarbonModule<C, D> : BaseModule, IModule
{
	public Configuration ModuleConfiguration { get; set; }
	public DynamicConfigFile Config { get; private set; }
	public DynamicConfigFile Data { get; private set; }

	public new virtual Type Type => default;

	public D DataInstance { get; private set; }
	public C ConfigInstance { get; private set; }

	public new virtual string Name => "Not set";

	protected void Puts(object message)
		=> Logger.Log($"[{Name}] {message}");
	protected void PutsError(object message, Exception ex = null)
		=> Logger.Error($"[{Name}] {message}", ex);
	protected void PutsWarn(object message)
		=> Logger.Warn($"[{Name}] {message}");

	public virtual void Dispose()
	{
		Config = null;
		ModuleConfiguration = null;
	}

	public virtual void Init()
	{
		base.Name = Name;
		base.Type = Type;

		Hooks = new System.Collections.Generic.List<string>();

		Community.Runtime.HookManager.LoadHooksFromType(Type);

		foreach (var method in Type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic))
		{
			if (Community.Runtime.HookManager.IsHookLoaded(method.Name))
			{
				Community.Runtime.HookManager.Subscribe(method.Name, Name);
				Hooks.Add(method.Name);
			}
		}

		Config = new DynamicConfigFile(Path.Combine(Defines.GetModulesFolder(), Name, "config.json"));
		Data = new DynamicConfigFile(Path.Combine(Defines.GetModulesFolder(), Name, "data.json"));

		Load();
		OnEnableStatus();
	}
	public virtual void InitEnd()
	{
		Puts($"Initialized.");
	}
	public virtual void Load()
	{
		var shouldSave = false;

		if (!Config.Exists())
		{
			ModuleConfiguration = new Configuration { Config = Activator.CreateInstance<C>() };
			if (EnabledByDefault) ModuleConfiguration.Enabled = true;
			shouldSave = true;
		}
		else
		{
			try { ModuleConfiguration = Config.ReadObject<Configuration>(); }
			catch (Exception exception) { Logger.Error($"Failed loading config. JSON file is corrupted and/or invalid.\n{exception.Message}"); }
		}

		ConfigInstance = ModuleConfiguration.Config;

		if (typeof(D) != typeof(EmptyModuleData))
		{
			if (!Data.Exists())
			{
				DataInstance = Activator.CreateInstance<D>();
				shouldSave = true;
			}
			else
			{
				try { DataInstance = Data.ReadObject<D>(); }
				catch (Exception exception) { Logger.Error($"Failed loading data. JSON file is corrupted and/or invalid.\n{exception.Message}"); }
			}
		}

		if (PreLoadShouldSave()) shouldSave = true;

		if (shouldSave) Save();
	}
	public virtual bool PreLoadShouldSave()
	{
		return false;
	}
	public virtual void Save()
	{
		if (ModuleConfiguration == null)
		{
			ModuleConfiguration = new Configuration { Config = Activator.CreateInstance<C>() };
			ConfigInstance = ModuleConfiguration.Config;
		}

		if (DataInstance == null && typeof(D) != typeof(EmptyModuleData))
		{
			DataInstance = Activator.CreateInstance<D>();
		}

		Config.WriteObject(ModuleConfiguration);
		if (DataInstance != null) Data.WriteObject(DataInstance);
	}

	public void SetEnabled(bool enable)
	{
		if (ModuleConfiguration != null)
		{
			ModuleConfiguration.Enabled = enable;
			OnEnableStatus();
		}
	}
	public override bool GetEnabled()
	{
		return ModuleConfiguration != null && ModuleConfiguration.Enabled;
	}

	public virtual void OnDisabled(bool initialized)
	{
		Loader.RemoveCommands(this);

		foreach (var hook in Hooks)
		{
			Unsubscribe(hook);
		}

		if (Hooks.Count > 0) Puts($"Unsubscribed from {Hooks.Count.ToNumbered().ToLower()} {Hooks.Count.Plural("hook", "hooks")}.");
	}
	public virtual void OnEnabled(bool initialized)
	{
		Loader.ProcessCommands(Type, this, flags: BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

		if (Community.IsServerFullyInitialized)
		{
			HookCaller.CallHook(this, "OnServerInitialized");
		}

		foreach (var hook in Hooks)
		{
			Subscribe(hook);
		}

		if (Hooks.Count > 0) Puts($"Subscribed to {Hooks.Count.ToNumbered().ToLower()} {Hooks.Count.Plural("hook", "hooks")}.");
	}

	public void OnEnableStatus()
	{
		try
		{
			if (ModuleConfiguration != null && ModuleConfiguration.Enabled) OnEnabled(Community.IsServerFullyInitialized); else OnDisabled(Community.IsServerFullyInitialized);
		}
		catch (Exception ex) { Logger.Error($"Failed {(ModuleConfiguration.Enabled ? "Enable" : "Disable")} initialization.", ex); }
	}

	private void OnServerInitialized()
	{
		if (GetEnabled()) OnEnableStatus();
	}

	public class Configuration : IModuleConfig
	{
		public bool Enabled { get; set; }
		public C Config { get; set; }
	}
}

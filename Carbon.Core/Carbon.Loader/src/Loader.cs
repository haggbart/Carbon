﻿///
/// Copyright (c) 2022 Carbon Community 
/// All rights reserved
/// 
using System;
using Carbon.Common;
using Carbon.Components;
using Carbon.Utility;

namespace Carbon;

internal sealed class Loader : Singleton<Loader>, IDisposable
{
	static Loader() { }

	private readonly string Identifier;

	internal HarmonyLib.Harmony Harmony;

	private UnityEngine.GameObject gameObject;

	internal Loader()
	{
		Identifier = Guid.NewGuid().ToString();
		Logger.Warn($"Using '{Identifier}' as runtime namespace");
		AssemblyResolver.GetInstance().RegisterDomain(AppDomain.CurrentDomain);

		Harmony = new HarmonyLib.Harmony(Identifier);
		gameObject = new UnityEngine.GameObject(Identifier);
		UnityEngine.Object.DontDestroyOnLoad(gameObject);
	}

	public void Initialize()
	{
		Logger.None(
			@"                                               " + Environment.NewLine +
			@"  ______ _______ ______ ______ _______ _______ " + Environment.NewLine +
			@" |      |   _   |   __ \   __ \       |    |  |" + Environment.NewLine +
			@" |   ---|       |      <   __ <   -   |       |" + Environment.NewLine +
			@" |______|___|___|___|__|______/_______|__|____|" + Environment.NewLine +
			@"                         discord.gg/eXPcNKK4yd " + Environment.NewLine +
			@"                                               " + Environment.NewLine
		);
	}

	public void Dispose()
	{
		try
		{
			Harmony.UnpatchAll(Identifier);
			Logger.Log("Removed all Harmony patches");
		}
		catch (Exception e)
		{
			Logger.Error("Unable to remove all Harmony patches", e);
		}

		Harmony = default;
		gameObject = default;
	}
}

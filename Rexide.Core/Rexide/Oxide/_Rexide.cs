﻿using ConVar;
using Oxide.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

public class Rexide
{
    public static Rexide Instance;
    public string Id;

    internal static MethodInfo _getMod { get; } = typeof ( HarmonyLoader ).GetMethod ( "GetMod", BindingFlags.Static | BindingFlags.NonPublic );

    public List<OxideCommand> AllChatCommands { get; } = new List<OxideCommand> ();
    public List<OxideCommand> AllConsoleCommands { get; } = new List<OxideCommand> ();

    public static VersionNumber Version { get; } = new VersionNumber ( 1, 0, 0 );

    public static string GetRootFolder ()
    {
        var folder = Path.Combine ( $"{Application.dataPath}\\..", "rexide" );
        Directory.CreateDirectory ( folder );

        return folder;
    }
    public static string GetConfigsFolder ()
    {
        var folder = Path.Combine ( $"{GetRootFolder ()}", "configs" );
        Directory.CreateDirectory ( folder );

        return folder;
    }
    public static string GetDataFolder ()
    {
        var folder = Path.Combine ( $"{GetRootFolder ()}", "data" );
        Directory.CreateDirectory ( folder );

        return folder;
    }
    public static string GetPluginsFolder ()
    {
        var folder = Path.Combine ( $"{GetRootFolder ()}", "plugins" );
        Directory.CreateDirectory ( folder );

        return folder;
    }
    public static string GetLogsFolder ()
    {
        var folder = Path.Combine ( $"{GetRootFolder ()}", "logs" );
        Directory.CreateDirectory ( folder );

        return folder;
    }

    public static void Log ( object message )
    {
        Debug.Log ( $"[Rexide v{Version}] {message}" );
    }
    public static void Warn ( object message )
    {
        Debug.LogWarning ( $"[Rexide v{Version}] {message}" );
    }
    public static void Error ( object message, Exception exception = null )
    {
        if ( exception == null ) Debug.LogError ( message );
        else Debug.LogException ( new Exception ( $"[Rexide v{Version}] {message}", exception ) );
    }

    public static void ReloadPlugins ()
    {
        RexideLoader.LoadRexideMods ();
    }
    public static void ClearPlugins ()
    {
        RexideLoader.UnloadRexideMods ();
    }

    internal void _clearCommands ()
    {
        AllChatCommands.Clear ();
        AllConsoleCommands.Clear ();
    }
    internal void _installDefaultCommands ()
    {
        var cmd = new OxideCommand
        {
            Command = "rexide",
            Plugin = new Oxide.Plugins.RustPlugin { Name = "Core" },
            Callback = ( player, command, args2 ) =>
            {
                player.ChatMessage ( $"You're running <color=orange>Rexide v{Rexide.Version}</color>" );
            }
        };

        AllChatCommands.Add ( cmd );
        AllConsoleCommands.Add ( cmd );
    }

    public void Init ()
    {
        Log ( $"Loading..." );

        GetRootFolder ();
        GetConfigsFolder ();
        GetDataFolder ();
        GetPluginsFolder ();
        GetLogsFolder ();

        _clearCommands ();
        _installDefaultCommands ();

        Log ( $"Loaded." );

        ReloadPlugins ();
    }
}

public class Initalizer : IHarmonyModHooks
{
    public void OnLoaded ( OnHarmonyModLoadedArgs args )
    {
        var newId = Assembly.GetExecutingAssembly ().GetName ().Name;

        if ( Rexide.Instance != null )
        {
            DebugEx.Log ( $"{Rexide.Instance?.Id} {newId}" );

            if ( Rexide.Instance.Id != newId )
            {
                HarmonyLoader.TryUnloadMod ( Rexide.Instance.Id );
                Rexide.Warn ( $"Unloaded previous: {Rexide.Instance.Id}" );
            }
        }

        if ( Rexide.Instance == null )
        {
            Rexide.Instance = new Rexide ();
            Rexide.Instance.Init ();
        }

        Rexide.Instance.Id = newId;
    }

    public void OnUnloaded ( OnHarmonyModUnloadedArgs args )
    {
        Rexide.ClearPlugins ();
        Debug.Log ( $"Unloaded Rexide." );
    }
}
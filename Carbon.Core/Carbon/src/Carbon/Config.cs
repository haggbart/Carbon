﻿using System;
using System.Collections.Generic;

/*
 *
 * Copyright (c) 2022-2023 Carbon Community 
 * All rights reserved.
 *
 */

namespace Carbon.Core;

[Serializable]
public class Config
{
	public bool AutoUpdate { get; set; } = true;
	public bool CarbonTag { get; set; } = true;
	public bool HarmonyWatchers { get; set; } = true;
	public bool HookTimeTracker { get; set; } = false;
	public bool HookValidation { get; set; } = true;
	public bool IsModded { get; set; } = true;
	public bool ScriptWatchers { get; set; } = true;
	public int EntityMapBufferSize { get; set; } = 100000;
	public int LogFileMode { get; set; } = 2;
	public int LogVerbosity { get; set; } = 0;
	public List<string> ConditionalCompilationSymbols { get; set; } = new List<string>() { "CARBON" };
	public Logger.Severity LogSeverity { get; set; } = Logger.Severity.Notice;
	public string Language { get; set; } = "en";
	public string WebRequestIp { get; set; }

#if WIN
	public bool ShowConsoleInfo { get; set; } = true;
#endif
}

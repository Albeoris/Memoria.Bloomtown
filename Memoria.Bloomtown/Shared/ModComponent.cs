using System;
using BepInEx.Logging;
using Memoria.Bloomtown.Configuration;
using Memoria.Bloomtown.Core;
using Memoria.Bloomtown.Mods;
using UnityEngine;
using Exception = System.Exception;
using Logger = BepInEx.Logging.Logger;

namespace Memoria.Bloomtown;

public sealed class ModComponent : MonoBehaviour
{
    public static ModComponent Instance { get; private set; }
    public static ManualLogSource Log { get; private set; }

    [field: NonSerialized] public ModConfiguration Config { get; private set; }
    [field: NonSerialized] public GameSpeedControl SpeedControl { get; private set; }
    [field: NonSerialized] public GameSaveControl SaveControl { get; private set; }
    [field: NonSerialized] public ModFileResolver ModFiles { get; private set; }
    [field: NonSerialized] public SceneModifier SceneModifier { get; private set; }
    // [field: NonSerialized] public ArenaWinsControl ArenaWins { get; private set; }
    // [field: NonSerialized] public GameVideoControl VideoControl { get; private set; }

    private Boolean _isDisabled;

    public void Awake()
    {
        Log = BepInEx.Logging.Logger.CreateLogSource("Memoria");
        Log.LogMessage($"[{nameof(ModComponent)}].{nameof(Awake)}(): Begin...");
        try
        {
            Instance = this;

            Config = new ModConfiguration();
            SpeedControl = new GameSpeedControl();
            SaveControl = new GameSaveControl();
            ModFiles = new ModFileResolver();

            SceneModifier = SceneModifier.Initialize();

            Log.LogMessage($"[{nameof(ModComponent)}].{nameof(Awake)}(): Processed successfully.");
        }
        catch (Exception ex)
        {
            _isDisabled = true;
            Log.LogError($"[{nameof(ModComponent)}].{nameof(Awake)}(): {ex}");
            throw;
        }
    }

    public void OnDestroy()
    {
        Log.LogInfo($"[{nameof(ModComponent)}].{nameof(OnDestroy)}()");
    }

    private void FixedUpdate()
    {
        try
        {
            if (_isDisabled)
                return;
        }
        catch (Exception ex)
        {
            _isDisabled = true;
            Log.LogError($"[{nameof(ModComponent)}].{nameof(FixedUpdate)}(): {ex}");
        }
    }

    private void Update()
    {
        try
        {
            if (_isDisabled)
                return;

            ModFiles.TryUpdate();
            // VideoControl.TryUpdate();
        }
        catch (Exception ex)
        {
            _isDisabled = true;
            Log.LogError($"[{nameof(ModComponent)}].{nameof(Update)}(): {ex}");
        }
    }

    private void LateUpdate()
    {
        try
        {
            if (_isDisabled)
                return;

            SpeedControl.TryUpdate();
            SaveControl.TryUpdate();
        }
        catch (Exception ex)
        {
            _isDisabled = true;
            Log.LogError($"[{nameof(ModComponent)}].{nameof(LateUpdate)}(): {ex}");
        }
    }

    private void OnGUI()
    {
        try
        {
            if (_isDisabled)
                return;
        }
        catch (Exception ex)
        {
            _isDisabled = true;
            Log.LogError($"[{nameof(ModComponent)}].{nameof(OnGUI)}(): {ex}");
        }
    }
}
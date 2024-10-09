using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using HarmonyLib;
using LazyBearTechnology;
using Memoria.Bloomtown.Configuration;
using Memoria.Bloomtown.Configuration.Hotkey;
using Memoria.Bloomtown.Shared.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = System.Object;

namespace Memoria.Bloomtown.Core;

public sealed class GameSaveControl : SafeComponent
{
    public GameSaveControl()
    {
    }

    protected override void Update()
    {
        ProcessQuickSave();
    }

    private void ProcessQuickSave()
    {
        SavesConfiguration config = ModComponent.Instance.Config.Saves;

        // Quick save
        foreach (Hotkey key in config.QuickSaveKey.Keys)
        {
            if (InputManager.IsToggled(key))
            {
                QuickSave();
                return;
            }
        }
        
        // Quick load exact quick saved game
        foreach (Hotkey key in config.QuickLoadExactKey.Keys)
        {
            if (InputManager.IsToggled(key))
            {
                QuickLoadExact();
                return;
            }
        }
        
        // Quick load last save
        foreach (Hotkey key in config.QuickLoadKey.Keys)
        {
            if (InputManager.IsToggled(key))
            {
                QuickLoadLast();
                return;
            }
        }
    }

    private const String QuickSavePrefix = "Memoria_QuickSave_";
    private const String SaveOnExitName = "Memoria_SaveOnExit";
    private static Int32 MaxQuickSavesCount => ModComponent.Instance.Config.Saves.QuickSavesCount;
    private static Int32 _nextQuickSaveNumber;

    private static void QuickSave()
    {
        if (MaxQuickSavesCount < 1)
            return;
        
        if (!IsSaveLoadInteractable(onExit: false))
            return;

        if (IsSavesBlocked(out String saveBlocked))
        {
            PopUpMessage.ShowMessage(saveBlocked);
            return;
        }
        
        SaveManager.SaveDataInfo saveDataInfo = SaveGame($"{QuickSavePrefix}{_nextQuickSaveNumber:D4}");
        _nextQuickSaveNumber = (_nextQuickSaveNumber + 1) % MaxQuickSavesCount;
        ModComponent.Log.LogInfo($"Game saved: {saveDataInfo.fileName}");
    }

    private static void QuickLoadLast()
    {
        if (!IsSaveLoadInteractable(onExit: false))
            return;
        
        SaveManager.SaveDataInfo lastSave = FindLastSave();
        if (lastSave is not null)
        {
            LoadGame(lastSave);
            ModComponent.Log.LogInfo($"The last saved game has been loaded: {lastSave.fileName}.");
        }
        else
        {
            ModComponent.Log.LogWarning($"There is no saved games.");
        }
    }
    
    private static void QuickLoadExact()
    {
        if (!IsSaveLoadInteractable(onExit: false))
            return;
        
        if (TryLoadGame(QuickSavePrefix, out SaveManager.SaveDataInfo saveDataInfo))
            ModComponent.Log.LogInfo($"The quick save game loaded: {saveDataInfo.fileName}");
        else
            ModComponent.Log.LogWarning($"Quick save with name {QuickSavePrefix} doesn't exist.");
    }

    public static void TrySaveOnExit()
    {
        Boolean saveOnExit = ModComponent.Instance.Config.Saves.AutoSaveOnExit;
        if (!saveOnExit)
            return;

        if (!IsSaveLoadInteractable(onExit: true))
            return;
        
        if (IsSavesBlocked(out String saveBlocked))
        {
            ModComponent.Log.LogWarning($"Cannot save game on exit: {saveBlocked}");
            return;
        }

        ModComponent.Log.LogInfo("Saving game on exit...");
        SaveManager.SaveDataInfo saveDataInfo = SaveGame(SaveOnExitName);
        ModComponent.Log.LogInfo($"Game saved: {saveDataInfo.fileName}");
    }

    public static void TryDeleteExitSave(String loadingData)
    {
        Boolean deleteExitSaveOnLoad = ModComponent.Instance.Config.Saves.DeleteExitSaveOnLoad;
        if (!deleteExitSaveOnLoad)
            return;

        SaveManager.SaveDataInfo existingSave = FindExistingSave(SaveOnExitName);
        if (existingSave is null)
            return;

        if (existingSave.saveData != loadingData)
            return;

        ModComponent.Log.LogInfo("Deleting exit save...");
        SaveManager.DeleteSave(SaveOnExitName);
        ModComponent.Log.LogInfo($"Save deleted: {QuickSavePrefix}");
    }

    private static SaveManager.SaveDataInfo FindLastSave()
    {
        return SaveManager.GetSaves().FirstOrDefault();
    }
    
    private static SaveManager.SaveDataInfo FindExistingSave(String saveName)
    {
        return SaveManager.GetSaves().FirstOrDefault(x => x.fileName == saveName);
    }

    internal static Boolean IsSaveLoadInteractable(Boolean onExit)
    {
        if (SceneReferences.instance?.locationName is null)
            return false;

        if (WorldHintsManager.IsOpen)
            return false;

        if (BattleManager.instance)
            return false;

        if (!UIManager.CanOpenMenuByKey())
            return false;

        var inputManager = global::InputManager.instance;
        if (inputManager is null)
            return false;

        var inputStatus = inputManager.status;
        if (inputStatus != global::InputManager.Status.Game)
        {
            if (!onExit || inputStatus != global::InputManager.Status.UI)
                return false;
        }

        if (!inputManager.isControlsEnabled)
            return false;

        return true;
    }

    internal static Boolean IsSavesBlocked(out String reason)
    {
        if (!ModComponent.Instance.Config.Saves.AllowSavingWhenFastTravelBlocked && PlayerData.IsFastTravelBlocked(out String blockedName))
        {
            String text = LocalizationInjector.Localize( 
                english: $"Saving the game is disabled:",
                russian: $"Сохранение игры запрещено:");
            
            reason = text + "\n" + LocalizationManager.L(blockedName);
            return true;
        }

        reason = null;
        return false;
    }

    private static SaveManager.SaveDataInfo SaveGame(String saveName)
    {
        String locationName = SceneReferences.instance.locationName;
        SaveManager.instance.SetSaveName(saveName);
        String str = SaveManager.instance.Save();

        SaveManager.SaveDataInfo newSaveDataInfo = new SaveManager.SaveDataInfo()
        {
            date = DateTimeManager.instance.DateTime,
            level = PlayerData.instance.player.level,
            locationName = locationName,
            fileName = saveName,
            isDemo = SettingsHolder.isDemo,
            saveData = str
        };

        List<SaveManager.SaveDataInfo> saves = SaveManager.GetSaves();
        saves.RemoveAll(x => x.fileName == newSaveDataInfo.fileName);
        saves.Insert(0, newSaveDataInfo);
        return newSaveDataInfo;
    }

    private static Boolean TryLoadGame(String saveName, out SaveManager.SaveDataInfo saveDataInfo)
    {
        saveDataInfo = FindExistingSave(saveName);
        if (saveDataInfo is null)
            return false;

        LoadGame(saveDataInfo);
        return true;
    }

    private static void LoadGame(SaveManager.SaveDataInfo saveDataInfo)
    {
        SaveManager.instance.SetSaveName(saveDataInfo.fileName);
        SaveManager.instance.LoadSave();
    }

    public static void AddSpecialSaves(List<SaveManager.SaveDataInfo> output)
    {
        String saveFileLocation = GetSaveFolder();
        ModComponent.Log.LogDebug($"Looking for special saves in [{saveFileLocation}]...");

        List<SaveManager.SaveDataInfo> quickSaves = new();
        HashSet<String> foundFiles = new();
        foreach (String file in LazyAPI.LazyFile.GetFiles(saveFileLocation, "Memoria*.dat", SearchOption.TopDirectoryOnly))
        {
            String fileName = Path.GetFileNameWithoutExtension(file);

            // Remove _backup sufix
            const String backupSufix = "_backup";
            if (fileName.EndsWith(backupSufix))
                fileName = fileName[..^backupSufix.Length];

            if (!foundFiles.Add(fileName))
                continue; // Already added an original or backup

            SaveManager.SaveDataInfo saveData = TryGetSaveDataInfo(fileName);
            if (saveData == null)
            {
                ModComponent.Log.LogWarning($"Save file is corrupted: {fileName}");
                continue;
            }

            output.Add(saveData);
            ModComponent.Log.LogDebug($"Special save found: {saveData.fileName}");

            if (fileName.StartsWith(QuickSavePrefix))
                quickSaves.Add(saveData);
        }

        if (quickSaves.Count > 0)
        {
            Int32 maxSavesCout = ModComponent.Instance.Config.Saves.QuickSavesCount;
            quickSaves.Sort((a, b) => a.timeStamp.CompareTo(b.timeStamp)); // the latest last
            Int32 legacy = quickSaves.Count - maxSavesCout;

            if (legacy > 0)
            {
                ModComponent.Log.LogWarning($"Too many quick saves. Deleting {legacy} files...");
                for (Int32 i = 0; i < legacy; i++)
                {
                    SaveManager.SaveDataInfo legacySave = quickSaves[i];
                    Saver.DeleteFile(legacySave.fileName, true, true);
                    output.Remove(legacySave);
                    foundFiles.Remove(legacySave.fileName);
                    ModComponent.Log.LogWarning($"Old quick save [{legacySave.fileName}] deleted.");
                }

                if (legacy < 0)
                    legacy = 0;

                for (Int32 i = legacy, index = 0; i < quickSaves.Count; i++, index++)
                    Saver.MoveFile(quickSaves[i].fileName, $"{QuickSavePrefix}{index:D4}_tmp");

                for (Int32 i = legacy, index = 0; i < quickSaves.Count; i++, index++)
                {
                    String newFileName = $"{QuickSavePrefix}{index:D4}";
                    Saver.MoveFile($"{QuickSavePrefix}{index:D4}_tmp", newFileName);
                    ModComponent.Log.LogWarning($"Quick save [{quickSaves[i].fileName}] renamed to {newFileName}.");
                    quickSaves[i].fileName = newFileName;
                }

                _nextQuickSaveNumber = 0;
            }
            else
            {
                _nextQuickSaveNumber = Int32.Parse(quickSaves.Last().fileName.Substring(QuickSavePrefix.Length), CultureInfo.InvariantCulture);
                _nextQuickSaveNumber = (_nextQuickSaveNumber + 1) % MaxQuickSavesCount;
            }
        }
        else
        {
            _nextQuickSaveNumber = 0;
        }

        if (foundFiles.Count != 0)
        {
            ModComponent.Log.LogDebug($"Found {foundFiles.Count} special saves.");
            output.Sort((a, b) => b.timeStamp.CompareTo(a.timeStamp)); // the latest first
        }
        else
        {
            ModComponent.Log.LogDebug($"Special saves not found.");
        }
    }

    public static void TryColorLoadButton(Button button, SaveManager.SaveDataInfo saveDataInfo)
    {
        if (saveDataInfo.fileName == SaveOnExitName)
        {
            TextMeshProUGUI locationLabel = button.transform.Find("Scene").GetComponent<TextMeshProUGUI>();
            locationLabel.text = $"<color=#FF6347>{locationLabel.text}</color>";
        }
        else if (saveDataInfo.fileName.StartsWith(QuickSavePrefix))
        {
            TextMeshProUGUI locationLabel = button.transform.Find("Scene").GetComponent<TextMeshProUGUI>();
            locationLabel.text = $"<color=#1E90FF>{locationLabel.text}</color>";
        }
    }

    private delegate SaveManager.SaveDataInfo TryGetSaveDataInfoDelegate(string saveName);
    private delegate String GetSaveFolderDelegate();

    private static readonly TryGetSaveDataInfoDelegate TryGetSaveDataInfo = AccessTools.MethodDelegate<TryGetSaveDataInfoDelegate>(AccessTools.Method(typeof(SaveManager), "TryGetSaveDataInfo"));
    private static readonly GetSaveFolderDelegate GetSaveFolder = AccessTools.MethodDelegate<GetSaveFolderDelegate>(AccessTools.Method(typeof(Saver), "GetSaveFolder"));
}
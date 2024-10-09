﻿using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Memoria.Bloomtown;
using Memoria.Bloomtown.BeepInEx;
using Memoria.Bloomtown.Configuration;
using Memoria.Bloomtown.Configuration.Hotkey;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Object = System.Object;


[HarmonyPatch(typeof(SystemPanel), "SetContentButtons")]
public static class SystemPanel_SetContentButtons
{
    public static void Postfix(SystemPanel __instance, SystemPanel.State ___m_state)
    {
        try
        {
            if (___m_state != SystemPanel.State.Settings)
                return;

            List<Selectable> selectables = UIManager_SetButtonsNavUpDown.LastAssignedSelectables;
            
            CustomDropdown prefabDropdown = __instance.buttons.content.Find("Dropdown").GetComponent<CustomDropdown>();
            SetupDropdownDelegate setupDropdown = AccessTools.MethodDelegate<SetupDropdownDelegate>(AccessTools.Method(typeof(SystemPanel), "SetupDropdown"), __instance);
            
            CustomToggle prefabToggle = __instance.buttons.content.Find("Toggle").GetComponent<CustomToggle>();
            SetupToggleDelegate setupToggle = AccessTools.MethodDelegate<SetupToggleDelegate>(AccessTools.Method(typeof(SystemPanel), "SetupToggle"), __instance);

            Slider prefabSlider = __instance.buttons.content.Find("Slider").GetComponent<Slider>();
            SetupSliderDelegate setupSlider = AccessTools.MethodDelegate<SetupSliderDelegate>(AccessTools.Method(typeof(SystemPanel), "SetupSlider"), __instance);
            
            ModConfiguration config = ModComponent.Instance.Config;

            // Just separator
            {
                const string localizationKey = "Memoria.ModConfiguration";
                LocalizationManager.cur_lng.dict.TryAdd(localizationKey, "Mod Configuration");
                CustomDropdown dropdown = UIManager.AddChildInstance(prefabDropdown, __instance.buttons.content, prefabDropdown.name);
                setupDropdown(dropdown, localizationKey);
                dropdown.SetOptions(["Memoria"]);
                dropdown.SetValueWithoutNotify(0);
                selectables.Add(dropdown);
                dropdown.gameObject.transform.SetSiblingIndex(100500+selectables.Count);
            }

            {
                const string localizationKey = "Memoria.Saves.AutoSaveOnExit";
                LocalizationManager.cur_lng.dict.TryAdd(localizationKey, "Save Game on Exit");
                CustomToggle toggle = UIManager.AddChildInstance(prefabToggle, __instance.buttons.content, prefabSlider.name);
                setupToggle(toggle, localizationKey, config.Saves.AutoSaveOnExit, (value) => config.Saves.AutoSaveOnExit = value);
                selectables.Add(toggle);
                toggle.gameObject.transform.SetSiblingIndex(100500+selectables.Count);
            }
            
            {
                const string localizationKey = "Memoria.Saves.DeleteExitSaveOnLoad";
                LocalizationManager.cur_lng.dict.TryAdd(localizationKey, "Delete Exit Save on Load");
                CustomToggle toggle = UIManager.AddChildInstance(prefabToggle, __instance.buttons.content, prefabSlider.name);
                setupToggle(toggle, localizationKey, config.Saves.DeleteExitSaveOnLoad, value => config.Saves.DeleteExitSaveOnLoad = value);
                selectables.Add(toggle);
                toggle.gameObject.transform.SetSiblingIndex(100500+selectables.Count);
            }

            {
                const string localizationKey = "Memoria.Saves.QuickSavesCount";
                LocalizationManager.cur_lng.dict.TryAdd(localizationKey, "Quick Saves Count");
                Slider slider = UIManager.AddChildInstance(prefabSlider, __instance.buttons.content, prefabSlider.name);
                setupSlider(slider, localizationKey, config.Saves.QuickSavesCount / 20f, delegate
                {
                    Single floatValue = slider.value / 20f;
                    Int32 integerValue = Mathf.RoundToInt(floatValue * 20f);
                    config.Saves.QuickSavesCount = integerValue;
                });
                selectables.Add(slider);
                slider.gameObject.transform.SetSiblingIndex(100500+selectables.Count);
            }
            
            {
                const string localizationKey = "Memoria.Speed.AlwaysSprint";
                LocalizationManager.cur_lng.dict.TryAdd(localizationKey, "Always sprint");
                CustomToggle toggle = UIManager.AddChildInstance(prefabToggle, __instance.buttons.content, prefabSlider.name);
                setupToggle(toggle, localizationKey, config.Speed.AlwaysSprint, value => config.Speed.AlwaysSprint = value);
                selectables.Add(toggle);
                toggle.gameObject.transform.SetSiblingIndex(100500+selectables.Count);
            }
            
            {
                const string localizationKey = "Memoria.Battle.EnemyHeartVisibility";
                LocalizationManager.cur_lng.dict.TryAdd(localizationKey, "Enemy HP visibility");
                CustomDropdown dropdown = UIManager.AddChildInstance(prefabDropdown, __instance.buttons.content, prefabDropdown.name);
                setupDropdown(dropdown, localizationKey);
                
                EnemyHeartVisibility[] values = EnumCache<EnemyHeartVisibility>.Values;
                dropdown.SetOptions(["Default", "Show Always", "Hide Always"]);
                dropdown.SetValueWithoutNotify(config.Battle.EnemyHeartVisibility - EnemyHeartVisibility.Default);
                dropdown.SetOnValueChanged(index => { config.Battle.EnemyHeartVisibility = values[index]; });
                selectables.Add(dropdown);
                dropdown.gameObject.transform.SetSiblingIndex(100500+selectables.Count);
            }

            {
                IReadOnlyList<Hotkey> keys = config.Speed.Key.GroupedByHeld.FirstOrDefault() ?? [];
                foreach (Hotkey key in keys.Reverse())
                {
                    String keyHint = HotkeyTypeConverter.ConvertToString(key);
                    
                    if (key.MustHeld)
                    {
                        const string localizationKey = "Memoria.Speed.HoldFactor";
                        LocalizationManager.cur_lng.dict.TryAdd(localizationKey, $"Speed up hold factor\r\nKey: {keyHint}");
                        Slider slider = UIManager.AddChildInstance(prefabSlider, __instance.buttons.content, prefabSlider.name);
                        setupSlider(slider, localizationKey, config.Speed.HoldFactor / 10f, delegate
                        {
                            Single floatValue = slider.value / 20f;
                            config.Speed.HoldFactor = floatValue * 10f;
                        });
                        selectables.Add(slider);
                        slider.gameObject.transform.SetSiblingIndex(100500+selectables.Count);
                    }
                    else
                    {
                        const string localizationKey = "Memoria.Speed.ToggleFactor";
                        LocalizationManager.cur_lng.dict.TryAdd(localizationKey, $"Speed up toggle factor\r\nKey: {keyHint}");
                        Slider slider = UIManager.AddChildInstance(prefabSlider, __instance.buttons.content, prefabSlider.name);
                        setupSlider(slider, localizationKey, config.Speed.ToggleFactor / 10f, delegate
                        {
                            Single floatValue = slider.value / 20f;
                            config.Speed.ToggleFactor = floatValue * 10f;
                        });
                        selectables.Add(slider);
                        slider.gameObject.transform.SetSiblingIndex(100500+selectables.Count);
                    }
                }
                
                UIManager.SetButtonsNavUpDown(selectables);
                selectables[0].Select();
                UIManager_SetButtonsNavUpDown.LastAssignedSelectables = [];
            }
        }
        catch (Exception ex)
        {
            ModComponent.Log.LogException(ex);
        }
    }

    private delegate void SetupDropdownDelegate(CustomDropdown dropdown, string dropdownName);
    private delegate void SetupToggleDelegate(CustomToggle toggle, string label, bool value, UnityAction<bool> action);
    private delegate void SetupSliderDelegate(Slider slider, string label, float value, UnityAction action);
}
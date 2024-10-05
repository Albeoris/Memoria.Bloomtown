using System;
using System.Collections.Generic;
using System.Reflection;
using BepInEx.Configuration;
using HarmonyLib;
using LazyBearTechnology;
using Memoria.Bloomtown.BeepInEx;
using Memoria.Bloomtown.Configuration;
using Memoria.Bloomtown.Configuration.Hotkey;

namespace Memoria.Bloomtown.HarmonyHooks.KeyBinding;

[HarmonyPatch]
public class GamepadController_Ctor
{
    [HarmonyTargetMethod]
    private static ConstructorInfo GetTargetMethod()
    {
        return typeof(GamepadController).GetConstructor([typeof(BindingsData)]);
    }
    
    private static readonly Dictionary<GameKey, ConfigEntry<GamepadButton>> Configs = new();
    
    public static void Prefix(BindingsData bindingsData)
    {
        try
        {
            EnsureInitialized(bindingsData);

            foreach (GamepadBinding binding in bindingsData.gamepadBindings)
            {
                ConfigEntry<GamepadButton> config = Configs[binding.gameKey];
                binding.gamepadButton = config.Value;
            }
        }
        catch (Exception ex)
        {
            ModComponent.Log.LogException(ex);
        }
    }

    private static void EnsureInitialized(BindingsData bindingsData)
    {
        if (Configs.Count != 0)
            return;
        
        ConfigFileProvider provider = ModComponent.Instance.Config.Provider;

        const String sectionName = "GamepadBindings";
        ConfigFile gamepad = provider.GetAndCache(sectionName);
            
        foreach (GamepadBinding binding in bindingsData.gamepadBindings)
        {
            if (!EnumerationMapping<GameKey>.Names.TryGetValue(binding.gameKey, out String optionName))
                throw new KeyNotFoundException($"Gamepad binding '{binding.gameKey}' was not found.");
            
            ConfigDefinition configDefinition = new ConfigDefinition(sectionName, optionName);
            String description = optionName;
            ConfigDescription configDescription = new ConfigDescription(description, AcceptableGamepadButton.Instance);
            ConfigEntry<GamepadButton> entry = gamepad.Bind(configDefinition, binding.gamepadButton, configDescription);
            Configs.Add(binding.gameKey, entry);
        }
    }
}
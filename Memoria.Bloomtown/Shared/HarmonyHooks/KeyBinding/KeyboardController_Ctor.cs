using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BepInEx.Configuration;
using HarmonyLib;
using LazyBearTechnology;
using Memoria.Bloomtown.BeepInEx;
using Memoria.Bloomtown.Configuration;
using Memoria.Bloomtown.Configuration.Hotkey;

namespace Memoria.Bloomtown.HarmonyHooks.KeyBinding;

[HarmonyPatch]
public class KeyboardController_Ctor
{
    [HarmonyTargetMethod]
    private static ConstructorInfo GetTargetMethod()
    {
        return typeof(KeyboardController).GetConstructor([typeof(BindingsData)]);
    }
    
    private static readonly Dictionary<GameKey, List<ConfigEntry<Hotkey>>> Configs = new();
    
    public static void Prefix(BindingsData bindingsData)
    {
        try
        {
            EnsureInitialized(bindingsData);

            foreach (IGrouping<GameKey, LazyBearTechnology.KeyBinding> grouping in bindingsData.keyBindings.GroupBy(x => x.gameKey))
            {
                GameKey gameKey = grouping.Key;
                List<ConfigEntry<Hotkey>> configs = Configs[gameKey];

                Int32 index = 0;
                foreach (var binding in grouping)
                {
                    Hotkey value = configs[index++].Value;
                    binding.keyCode = value.Key;
                    binding.additionalKeyCodes = value.ToKeyboardBindingAdditionalKeys();
                }
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

        const String sectionName = "KeyboardBindings";
        ConfigFile keyboard = provider.GetAndCache(sectionName);
            
        foreach (IGrouping<GameKey, LazyBearTechnology.KeyBinding> grouping in bindingsData.keyBindings.GroupBy(x => x.gameKey))
        {
            GameKey gameKey = grouping.Key;
            if (!EnumerationMapping<GameKey>.Names.TryGetValue(gameKey, out String baseOptionName))
                throw new KeyNotFoundException($"Keyboard binding '{gameKey.value}' was not found.");

            if (!Configs.TryGetValue(gameKey, out List<ConfigEntry<Hotkey>> configEntries))
            {
                configEntries = new();
                Configs.Add(gameKey, configEntries);
            }

            Int32 index = 0;
            foreach (var binding in grouping)
            {
                String optionName = baseOptionName;
                if (++index != 1)
                    optionName = $"{optionName}_{index}";
                
                ConfigDefinition configDefinition = new ConfigDefinition(sectionName, optionName);
                String description = optionName;
                ConfigDescription configDescription = new ConfigDescription(description, new AcceptableHotkey(optionName) {GamepadSupported = false, GameKeysSupported = false} );
                Hotkey defaultHotKey = new Hotkey(binding.keyCode) { ModifierKeys = binding.additionalKeyCodes };
                ConfigEntry<Hotkey> entry = keyboard.Bind(configDefinition, defaultHotKey, configDescription);
                configEntries.Add(entry);
            }
        }
    }
}
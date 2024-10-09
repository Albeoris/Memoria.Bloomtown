using System;
using System.Collections.Generic;

namespace Memoria.Bloomtown.Shared.Core;

public class LocalizationInjector
{
    public static String Localize(String english, String russian = null)
    {
        LocalizationManager currentLanguage = LocalizationManager.cur_lng;
        
        if (russian != null && currentLanguage.id == "ru")
            return russian;
        
        return english;
    }
    
    public static String AddLocalization(String key, String english, String russian = null)
    {
        LocalizationManager currentLanguage = LocalizationManager.cur_lng;
        Dictionary<String, String> dictionary = currentLanguage.dict;

        String localizationKey = $"Memoria.{key}";
        if (dictionary.ContainsKey(localizationKey))
            return localizationKey;

        String localized = Localize(english, russian);
        dictionary.Add(localizationKey, localized);
        return localizationKey;
    }
}
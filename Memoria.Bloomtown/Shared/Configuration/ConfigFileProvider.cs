using System;
using System.Collections.Generic;
using System.IO;
using BepInEx;
using BepInEx.Configuration;
using Memoria.Bloomtown.Core;

namespace Memoria.Bloomtown.Configuration;

public sealed class ConfigFileProvider
{
    private readonly Dictionary<String, ConfigFile> _cache = new();
    private readonly String _configurationRoot;
    private Boolean _saveChangedConfigFiles;
    
    public ConfigFileProvider()
        : this(Path.Combine(Paths.ConfigPath, ModConstants.Id), saveChangedConfigFiles: true)
    {
    }

    public ConfigFileProvider(String configurationRoot, Boolean saveChangedConfigFiles)
    {
        _configurationRoot = configurationRoot ?? throw new ArgumentNullException(nameof(configurationRoot));
        _saveChangedConfigFiles = saveChangedConfigFiles;
    }
    
    public Boolean SaveChangedConfigFiles
    {
        get => _saveChangedConfigFiles;
        set
        {
            _saveChangedConfigFiles = value;
            foreach (ConfigFile config in _cache.Values)
                config.SaveOnConfigSet = value;
        }
    }

    public ConfigFile GetAndCache(String sectionName)
    {
        if (!_cache.TryGetValue(sectionName, out var file))
        {
            file = Get(sectionName);
            _cache.Add(sectionName, file);
        }

        return file;
    }

    private ConfigFile Get(String sectionName)
    {
        String configPath = GetConfigurationPath(sectionName);
        return new ConfigFile(configPath, true, ownerMetadata: null)
        {
            SaveOnConfigSet = _saveChangedConfigFiles
        };
    }

    private String GetConfigurationPath(String sectionName)
    {
        return Path.Combine(_configurationRoot, sectionName + ".cfg");
    }
}
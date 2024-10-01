using System;
using BepInEx.Logging;

namespace Memoria.Bloomtown.Configuration;

public sealed partial class ModConfiguration
{
    public SpeedConfiguration Speed { get; }
    public SavesConfiguration Saves { get; }
    public AssetsConfiguration Assets { get; }

    private readonly ConfigFileProvider _provider = new();

    public ModConfiguration()
    {
        using (var log = BepInEx.Logging.Logger.CreateLogSource("Memoria Config"))
        {
            try
            {
                log.LogInfo($"Initializing {nameof(ModConfiguration)}");

                Speed = SpeedConfiguration.Create(_provider);
                Saves = SavesConfiguration.Create(_provider);
                Assets = AssetsConfiguration.Create(_provider);

                log.LogInfo($"{nameof(ModConfiguration)} initialized successfully.");
            }
            catch (Exception ex)
            {
                log.LogError($"Failed to initialize {nameof(ModConfiguration)}: {ex}");
                throw;
            }
        }
    }

    public void OverrideFrom(String[] configDirectories)
    {
        // TODO: Reset previous overrides
        if (configDirectories.Length == 0)
            return;

        using (var log = BepInEx.Logging.Logger.CreateLogSource("Memoria Config"))
        {
            if (configDirectories.Length == 1)
                log.LogInfo($"Loading external config files from directory:");
            else
                log.LogInfo($"Loading external config files from {configDirectories.Length} directories:");

            foreach (String configDirectory in configDirectories)
            {
                String shortPath = ApplicationPathConverter.ReturnPlaceholders(configDirectory);
                log.LogInfo("    " + shortPath);

                try
                {
                    ConfigFileProvider provider = new(configDirectory, saveChangedConfigFiles: false);
                    _provider.SaveChangedConfigFiles = false;

                    Speed.OverrideFrom(SpeedConfiguration.Create(provider));
                    Saves.OverrideFrom(SavesConfiguration.Create(provider));
                    Assets.OverrideFrom(AssetsConfiguration.Create(provider));
                }
                catch (Exception ex)
                {
                    log.LogError($"Failed to load external config files from {shortPath}: {ex}");
                }
                finally
                {
                    _provider.SaveChangedConfigFiles = true;
                }
            }
        }
    }
}
using System;

namespace Memoria.Bloomtown.Configuration;

[ConfigScope("Assets")]
public abstract partial class AssetsConfiguration
{
    // [ConfigEntry($"Skip logo on startup." +
    //              "$[Russian]: Пропускать логотипы при запуске.")]
    // public virtual Boolean SkipLogoOnStartup { get; set; } = false;

    [ConfigEntry($"Overwrite the supported resources from the {nameof(ModsDirectory)}." +
                 $"$[Russian]: Позволяет загружать поддерживаемые ресурсы из каталога {nameof(ModsDirectory)}")]
    public virtual Boolean ModsEnabled => true;
    
    [ConfigEntry($"Enables tracking of changes in the {nameof(ModsDirectory)}, allowing to load some kind of updated files without restarting the game." +
                 $"$[Russian]: Включает отслеживание изменений в {nameof(ModsDirectory)}, позволяя загружать некоторые типы обновленных файлов без перезагрузки игры.")]
    public virtual Boolean WatchingEnabled => true;

    [ConfigEntry($"Directory from which the supported resources will be loaded." +
                 $"$[Russian]: Директория, из которой будут загружаться поддерживаемые ресурсы.")]
    [ConfigConverter(nameof(ModsDirectoryConverter))]
    [ConfigDependency(nameof(ModsEnabled), "String.Empty")]
    public virtual String ModsDirectory => "%StreamingAssets%/Mods";

    protected IAcceptableValue<String> ModsDirectoryConverter { get; } = new AcceptableDirectoryPath(nameof(ModsDirectory), create: true);

    public abstract void CopyFrom(AssetsConfiguration configuration);
    public abstract void OverrideFrom(AssetsConfiguration configuration);
}
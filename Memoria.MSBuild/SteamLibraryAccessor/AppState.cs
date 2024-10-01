namespace Memoria.MSBuild.SteamLibraryAccessor;

/// <summary>
/// Represents the App State of a game from the appmanifest file.
/// </summary>
public class AppState
{
    public AppState(UInt32 appId, Int64? universe, String launcherPath, String name, Int64? stateFlags, String installationDirectoryName, Int64? lastUpdated, Int64? lastPlayed, Int64? sizeOnDisk, Int64? stagingSize, Int64? buildId, Int64? lastOwner, Int64? updateResult, Int64? bytesToDownload, Int64? bytesDownloaded, Int64? bytesToStage, Int64? bytesStaged, Int64? targetBuildID, Int64? autoUpdateBehavior, Int64? allowOtherDownloadsWhileRunning, Int64? scheduledAutoUpdate, IReadOnlyDictionary<String, Depot> installedDepots, IReadOnlyDictionary<String, String> installScripts, IReadOnlyDictionary<String, String> userConfig, IReadOnlyDictionary<String, String> mountedConfig)
    {
        AppId = appId;
        Universe = universe;
        LauncherPath = launcherPath;
        Name = name;
        StateFlags = stateFlags;
        InstallationDirectoryName = installationDirectoryName;
        LastUpdated = lastUpdated;
        LastPlayed = lastPlayed;
        SizeOnDisk = sizeOnDisk;
        StagingSize = stagingSize;
        BuildId = buildId;
        LastOwner = lastOwner;
        UpdateResult = updateResult;
        BytesToDownload = bytesToDownload;
        BytesDownloaded = bytesDownloaded;
        BytesToStage = bytesToStage;
        BytesStaged = bytesStaged;
        TargetBuildID = targetBuildID;
        AutoUpdateBehavior = autoUpdateBehavior;
        AllowOtherDownloadsWhileRunning = allowOtherDownloadsWhileRunning;
        ScheduledAutoUpdate = scheduledAutoUpdate;
        InstalledDepots = installedDepots;
        InstallScripts = installScripts;
        UserConfig = userConfig;
        MountedConfig = mountedConfig;
    }

    /// <summary>
    /// Gets the app ID.
    /// </summary>
    public UInt32 AppId { get; }

    /// <summary>
    /// Gets the Universe value.
    /// </summary>
    public Int64? Universe { get; }

    /// <summary>
    /// Gets the path to the launcher.
    /// </summary>
    public String LauncherPath { get; }

    /// <summary>
    /// Gets the name of the app.
    /// </summary>
    public String Name { get; }

    /// <summary>
    /// Gets the state flags.
    /// </summary>
    public Int64? StateFlags { get; }

    /// <summary>
    /// Gets the install directory.
    /// </summary>
    public String InstallationDirectoryName { get; }

    /// <summary>
    /// Gets the absolute installation path.
    /// </summary>
    public String InstallationAbsolutePath => Path.Combine(LibraryAbsolutePath, "common", InstallationDirectoryName);

    /// <summary>
    /// Gets the uninstall command line.
    /// </summary>
    public String UninstallString => $"\"{SteamExeAbsolutePath}\" steam://uninstall/{AppId}";

    /// <summary>
    /// Gets the last updated timestamp.
    /// </summary>
    public Int64? LastUpdated { get; }

    /// <summary>
    /// Gets the last played timestamp.
    /// </summary>
    public Int64? LastPlayed { get; }

    /// <summary>
    /// Gets the size on disk.
    /// </summary>
    public Int64? SizeOnDisk { get; }

    /// <summary>
    /// Gets the staging size.
    /// </summary>
    public Int64? StagingSize { get; }

    /// <summary>
    /// Gets the build ID.
    /// </summary>
    public Int64? BuildId { get; }

    /// <summary>
    /// Gets the last owner.
    /// </summary>
    public Int64? LastOwner { get; }

    /// <summary>
    /// Gets the update result.
    /// </summary>
    public Int64? UpdateResult { get; }

    /// <summary>
    /// Gets the bytes to download.
    /// </summary>
    public Int64? BytesToDownload { get; }

    /// <summary>
    /// Gets the bytes downloaded.
    /// </summary>
    public Int64? BytesDownloaded { get; }

    /// <summary>
    /// Gets the bytes to stage.
    /// </summary>
    public Int64? BytesToStage { get; }

    /// <summary>
    /// Gets the bytes staged.
    /// </summary>
    public Int64? BytesStaged { get; }

    /// <summary>
    /// Gets the target build ID.
    /// </summary>
    public Int64? TargetBuildID { get; }

    /// <summary>
    /// Gets the auto update behavior.
    /// </summary>
    public Int64? AutoUpdateBehavior { get; }

    /// <summary>
    /// Gets the allow other downloads while running flag.
    /// </summary>
    public Int64? AllowOtherDownloadsWhileRunning { get; }

    /// <summary>
    /// Gets the scheduled auto update.
    /// </summary>
    public Int64? ScheduledAutoUpdate { get; }

    /// <summary>
    /// Gets the installed depots.
    /// </summary>
    public IReadOnlyDictionary<String, Depot> InstalledDepots { get; }

    /// <summary>
    /// Gets the install scripts.
    /// </summary>
    public IReadOnlyDictionary<String, String> InstallScripts { get; }

    /// <summary>
    /// Gets the user config.
    /// </summary>
    public IReadOnlyDictionary<String, String> UserConfig { get; }

    /// <summary>
    /// Gets the mounted config.
    /// </summary>
    public IReadOnlyDictionary<String, String> MountedConfig { get; }
    
    /// <summary>
    /// Gets or sets the Steam.exe absolute path.
    /// </summary>
    public String SteamExeAbsolutePath { get; set; }
    
    /// <summary>
    /// Gets or sets the library absolute path.
    /// </summary>
    public String LibraryAbsolutePath { get; set; }
}
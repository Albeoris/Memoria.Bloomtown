using System.Globalization;
using ValveKeyValue;

namespace Memoria.MSBuild.SteamLibraryAccessor;

public static class AcfParser
{
    /// <summary>
    /// Parses the ACF content and returns the AppState object.
    /// </summary>
    /// <param name="acfContent">The content of the ACF file.</param>
    /// <returns>An AppState object containing the parsed data.</returns>
    public static AppState ParseAppManifest(Stream acfContent)
    {
        IFormatProvider formatProvider = CultureInfo.InvariantCulture;

        KVSerializer? kv = KVSerializer.Create(KVSerializationFormat.KeyValues1Text);
        KVDocument appStateNode = kv.Deserialize(acfContent);

        Dictionary<String, Depot> installedDepots = ReadInstalledDepots(appStateNode);
        Dictionary<String, String> installScripts = ReadInstallScripts(appStateNode);
        Dictionary<String, String> userConfig = ReadUserConfig(appStateNode);
        Dictionary<String, String> mountedConfig = ReadGetMountedConfig(appStateNode);

        AppState appState = new AppState
        (
            appId: appStateNode.GetUInt32("appid"),
            universe: appStateNode.FindInt64("Universe"),
            launcherPath: appStateNode.GetString("LauncherPath"),
            name: appStateNode.GetString("name"),
            stateFlags: appStateNode.FindInt64("StateFlags"),
            installationDirectoryName: appStateNode.GetString("installdir"),
            lastUpdated: appStateNode.FindInt64("LastUpdated"),
            lastPlayed: appStateNode.FindInt64("LastPlayed"),
            sizeOnDisk: appStateNode.FindInt64("SizeOnDisk"),
            stagingSize: appStateNode.FindInt64("StagingSize"),
            buildId: appStateNode.FindInt64("buildid"),
            lastOwner: appStateNode.FindInt64("LastOwner"),
            updateResult: appStateNode.FindInt64("UpdateResult"),
            bytesToDownload: appStateNode.FindInt64("BytesToDownload"),
            bytesDownloaded: appStateNode.FindInt64("BytesDownloaded"),
            bytesToStage: appStateNode.FindInt64("BytesToStage"),
            bytesStaged: appStateNode.FindInt64("BytesStaged"),
            targetBuildID: appStateNode.FindInt64("TargetBuildID"),
            autoUpdateBehavior: appStateNode.FindInt64("AutoUpdateBehavior"),
            allowOtherDownloadsWhileRunning: appStateNode.FindInt64("AllowOtherDownloadsWhileRunning"),
            scheduledAutoUpdate: appStateNode.FindInt64("ScheduledAutoUpdate"),
            installedDepots,
            installScripts,
            userConfig,
            mountedConfig
        );



        return appState;
    }

    private static Dictionary<String, Depot> ReadInstalledDepots(KVDocument appStateNode)
    {
        Dictionary<String, Depot> installedDepots = new();
        foreach (KVObject depot in appStateNode.EnumerateChildren("InstalledDepots"))
        {
            Depot depotData = new Depot
            (
                manifest: depot.GetUInt64("manifest"),
                size: depot.GetUInt64("size")
            );
            installedDepots.Add(depot.Name, depotData);
        }

        return installedDepots;
    }

    private static Dictionary<String, String> ReadInstallScripts(KVDocument appStateNode)
    {
        return appStateNode.EnumerateChildren("InstallScripts").ToDictionary(script => script.Name, script => script.Value.ToString(ValveKeyValueExtensions.FormatProvider));
    }

    private static Dictionary<String, String> ReadUserConfig(KVDocument appStateNode)
    {
        return appStateNode.EnumerateChildren("UserConfig").ToDictionary(script => script.Name, script => script.Value.ToString(ValveKeyValueExtensions.FormatProvider));
    }

    private static Dictionary<String, String> ReadGetMountedConfig(KVDocument appStateNode)
    {
        return appStateNode.EnumerateChildren("MountedConfig").ToDictionary(script => script.Name, script => script.Value.ToString(ValveKeyValueExtensions.FormatProvider));
    }
}
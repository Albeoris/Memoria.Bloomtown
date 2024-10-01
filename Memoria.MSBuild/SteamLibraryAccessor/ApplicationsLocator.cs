using Microsoft.Win32;

namespace Memoria.MSBuild.SteamLibraryAccessor;

public static class ApplicationsLocator
{
    public static AppState? FindApplicationById(UInt32 appId)
    {
        return EnumerateInstalledApplications().FirstOrDefault(a => a.AppId == appId);
    }

    public static IEnumerable<AppState> EnumerateInstalledApplications()
    {
        String? steamDirectory = FindSteamInstallPath();
        if (steamDirectory is null || !Directory.Exists(steamDirectory))
            yield break;

        String steamExePath = Path.Combine(steamDirectory, "steam.exe");

        String libraryDescriptor = Path.Combine(steamDirectory, "steamapps", "libraryfolders.vdf");
        IReadOnlyList<LibraryFolder> libraryFolders;
        using (var input = File.OpenRead(libraryDescriptor))
            libraryFolders = VdfParser.Parse(input);

        foreach (LibraryFolder folder in libraryFolders)
        {
            String applicationDirectory = Path.Combine(folder.Path, "steamapps");
            if (!Directory.Exists(applicationDirectory))
                continue;

            foreach ((UInt32 appId, _) in folder.Apps)
            {
                String acfPath = Path.Combine(applicationDirectory, $"appmanifest_{appId}.acf");
                if (!File.Exists(acfPath))
                    continue;

                using (FileStream acfContent = File.OpenRead(acfPath))
                {
                    AppState appState = AcfParser.ParseAppManifest(acfContent);
                    appState.SteamExeAbsolutePath = steamExePath;
                    appState.LibraryAbsolutePath = applicationDirectory;
                    yield return appState;
                }
            }
        }
    }

    private static String? FindSteamInstallPath()
    {
        using (RegistryKey? key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\Valve\Steam"))
            return key?.GetValue("InstallPath")?.ToString();
    }

    private static String? FindSteamLibraryDescriptorPath()
    {
        String? steamPath = FindSteamInstallPath();
        if (steamPath is null)
            return null;

        return Path.Combine(steamPath, "steamapps", "libraryfolders.vdf");
    }
}
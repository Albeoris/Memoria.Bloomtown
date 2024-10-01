using Memoria.MSBuild.SteamLibraryAccessor;
using Microsoft.Build.Framework;

namespace Memoria.MSBuild;

public class ResolveSteamApplicationPath : ITask
{
    public IBuildEngine BuildEngine { get; set; } = null!;
    public ITaskHost HostObject { get; set; } = null!;

    [Required] public UInt32 SteamApplicationId { get; set; }

    [Output] public String ResolvedApplicationPath { get; set; }
    [Output] public String CorrectRegistryCommandLine { get; set; }

    public bool Execute()
    {
        ConfigureDllPaths();

        AppState? application = ApplicationsLocator.FindApplicationById(SteamApplicationId);
        if (application is null)
        {
            BuildEngine.LogMessageEvent(new BuildMessageEventArgs($"Cannot find Steam application with id {SteamApplicationId}.", null, nameof(ResolveSteamApplicationPath), MessageImportance.High));
        }
        else
        {
            BuildEngine.LogMessageEvent(new BuildMessageEventArgs($"Installation path of the Steam application with id {SteamApplicationId} has been resolved as: {application.InstallationAbsolutePath}", null, nameof(ResolveSteamApplicationPath), MessageImportance.High));

            ResolvedApplicationPath = application.InstallationAbsolutePath;
            CorrectRegistryCommandLine = BuildUninstallRegistryCommandLine(application);
        }

        return true;
    }

    private static void ConfigureDllPaths()
    {
        String? assemblyLocation = Path.GetDirectoryName(typeof(ApplicationsLocator).Assembly.Location);
        if (assemblyLocation is not null)
            Environment.SetEnvironmentVariable("PATH", Environment.GetEnvironmentVariable("PATH") + ';' + assemblyLocation);
    }

    private String BuildUninstallRegistryCommandLine(AppState application)
    {
        return $"@echo off"
               + $""" && reg add "HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Steam App {application.AppId}" /v DisplayName /t REG_SZ /d "{application.Name}" /f"""
               + $""" && reg add "HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Steam App {application.AppId}" /v InstallLocation /t REG_SZ /d "{application.InstallationAbsolutePath}" /f"""
               + $""" && reg add "HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Steam App {application.AppId}" /v UninstallString /t REG_SZ /d "{application.UninstallString.Replace("\"", "\\\"")}" /f"""
               + $""" && reg add "HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Steam App {application.AppId}" /v HelpLink /t REG_SZ /d "https://help.steampowered.com/" /f"""
               + $""" && reg add "HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Steam App {application.AppId}" /v NoModify /t REG_DWORD /d 1 /f"""
               + $""" && reg add "HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Steam App {application.AppId}" /v NoRepair /t REG_DWORD /d 1 /f""";
    }
}
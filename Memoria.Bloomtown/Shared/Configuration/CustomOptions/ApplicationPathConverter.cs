using System;
using UnityEngine;

namespace Memoria.Bloomtown.Configuration;

public static class ApplicationPathConverter
{
    public static readonly String StreamingAssetsWindowsPath = WinPath(Application.streamingAssetsPath);
    public static readonly String DataWindowsPath = WinPath(Application.dataPath);
    public static readonly String PersistentDataWindowsPath = WinPath(Application.persistentDataPath);

    public static String ReplacePlaceholders(String path)
    {
        return path
            .Replace("%StreamingAssets%", StreamingAssetsWindowsPath)
            .Replace("%DataPath%", DataWindowsPath)
            .Replace("%PersistentDataPath%", PersistentDataWindowsPath);
    }

    public static String ReturnPlaceholders(String path)
    {
        return path
            .Replace(StreamingAssetsWindowsPath, "%StreamingAssets%")
            .Replace(DataWindowsPath, "%DataPath%")
            .Replace(PersistentDataWindowsPath, "%PersistentDataPath%");
    }

    private static String WinPath(String linuxPath) => linuxPath.Replace('/', '\\');
}
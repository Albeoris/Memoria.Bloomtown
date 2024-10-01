using System.Globalization;
using ValveKeyValue;

namespace Memoria.MSBuild.SteamLibraryAccessor;

public static class VdfParser
{
    /// <summary>
    /// Parses the VDF content and returns the LibraryFolders object.
    /// </summary>
    /// <param name="vdfContent">The content of the VDF file.</param>
    /// <returns>A LibraryFolders object containing the parsed data.</returns>
    public static IReadOnlyList<LibraryFolder> Parse(Stream vdfContent)
    {
        IFormatProvider formatProvider = CultureInfo.InvariantCulture;
            
        KVSerializer? kv = KVSerializer.Create(KVSerializationFormat.KeyValues1Text);
        KVDocument data = kv.Deserialize(vdfContent);

        List<LibraryFolder> folders = new();

        foreach (KVObject folder in data.Children)
        {
            Dictionary<UInt32, Int64> apps = new();
            foreach (KVObject obj in folder.EnumerateChildren("apps"))
                apps.Add(UInt32.Parse(obj.Name, formatProvider), obj.Value.ToInt64(formatProvider));

            LibraryFolder libraryFolder = new(
                path: folder.GetString("path"),
                label: folder.GetString("label"),
                contentId: folder.GetInt64("contentid"),
                totalSize: folder.GetInt64("totalsize"),
                updateCleanBytesTally: folder.GetInt64("update_clean_bytes_tally"),
                timeLastUpdateCorruption: folder.GetInt64("time_last_update_corruption"),
                apps
            );

            folders.Add(libraryFolder);
        }

        return folders;
    }
}
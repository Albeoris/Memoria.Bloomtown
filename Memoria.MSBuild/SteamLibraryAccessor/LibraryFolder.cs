namespace Memoria.MSBuild.SteamLibraryAccessor;

/// <summary>
/// Represents a single library folder.
/// </summary>
public class LibraryFolder
{
    public LibraryFolder(String path, String label, Int64 contentId, Int64 totalSize, Int64 updateCleanBytesTally, Int64 timeLastUpdateCorruption, IReadOnlyDictionary<UInt32, Int64> apps)
    {
        Path = path;
        Label = label;
        ContentId = contentId;
        TotalSize = totalSize;
        UpdateCleanBytesTally = updateCleanBytesTally;
        TimeLastUpdateCorruption = timeLastUpdateCorruption;
        Apps = apps;
    }

    /// <summary>
    /// Gets or sets the path of the library folder.
    /// </summary>
    public String Path { get; }

    /// <summary>
    /// Gets or sets the label of the library folder.
    /// </summary>
    public String Label { get; }

    /// <summary>
    /// Gets or sets the content ID of the library folder.
    /// </summary>
    public Int64 ContentId { get; }

    /// <summary>
    /// Gets or sets the total size of the library folder.
    /// </summary>
    public Int64 TotalSize { get; }

    /// <summary>
    /// Gets or sets the update clean bytes tally of the library folder.
    /// </summary>
    public Int64 UpdateCleanBytesTally { get; }

    /// <summary>
    /// Gets or sets the time of the last update corruption of the library folder.
    /// </summary>
    public Int64 TimeLastUpdateCorruption { get; }

    /// <summary>
    /// Gets or sets the dictionary of applications in the library folder.
    /// </summary>
    public IReadOnlyDictionary<UInt32, Int64> Apps { get; }
}
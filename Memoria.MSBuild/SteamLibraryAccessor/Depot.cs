namespace Memoria.MSBuild.SteamLibraryAccessor;

/// <summary>
/// Represents a single depot in the app manifest.
/// </summary>
public class Depot
{
    public Depot(UInt64 manifest, UInt64 size)
    {
        Manifest = manifest;
        Size = size;
    }

    /// <summary>
    /// Gets or sets the manifest ID.
    /// </summary>
    public UInt64 Manifest { get; }

    /// <summary>
    /// Gets or sets the size of the depot.
    /// </summary>
    public UInt64 Size { get; }
}
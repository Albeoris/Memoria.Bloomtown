using System.IO;

internal interface ISplitTablesCommandPerformer
{
    void SplitTables(FileInfo inputFile, DirectoryInfo? outputDir, CancellationToken cancellationToken);
}
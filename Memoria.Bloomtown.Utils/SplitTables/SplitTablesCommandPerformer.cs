using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;

internal sealed class SplitTablesCommandPerformer : ISplitTablesCommandPerformer
{
    public void SplitTables(FileInfo inputFile, DirectoryInfo? outputDir, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (outputDir is null)
        {
            if (inputFile.Directory is null)
                throw new ArgumentException("Output directory cannot be resolved from input file name.", nameof(outputDir));
            
            outputDir = inputFile.Directory.CreateSubdirectory(Path.GetFileNameWithoutExtension(inputFile.Name));
        }

        using (StreamReader sr = inputFile.OpenText())
        {
            if (sr.ReadLine() != "TBLS_DATA")
                throw new FormatException("Input file is not TBLS_DATA");

            outputDir.Create();

            while (!sr.EndOfStream)
            {
                cancellationToken.ThrowIfCancellationRequested();
                String? line = sr.ReadLine();
                if (line is null)
                    break;

                String typeName = ParseTypeName(line);
                String json = sr.ReadLine() ?? throw new FormatException("Input file is corrupted");
                WriteJson(json, typeName, cancellationToken, outputDir, cancellationToken);
            }
        }
    }

    private void WriteJson(String json, String typeName, CancellationToken cancellationToken1, DirectoryInfo outputDir, CancellationToken cancellationToken)
    {
        cancellationToken1.ThrowIfCancellationRequested();
        JsonNode node = JsonNode.Parse(json) ?? throw new FormatException("Failed to parse json.");

        cancellationToken.ThrowIfCancellationRequested();
        String outputFile = Path.Combine(outputDir.FullName, typeName + ".json");
        Directory.CreateDirectory(Path.GetDirectoryName(outputFile)!);
        if (File.Exists(outputFile))
            throw new IOException($"File {outputFile} already exists.");
            
        using (Stream output = File.Create(outputFile))
        {
            JsonWriterOptions options = new JsonWriterOptions { Indented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
            using (Utf8JsonWriter writer = new Utf8JsonWriter(output, options))
                node.WriteTo(writer);
        }
    }

    private static String ParseTypeName(String line)
    {
        const String prefix = "TYPE_";
        const String postfix = "_ENDTYPE";

        if (!line.StartsWith(prefix) || !line.EndsWith(postfix))
            throw new FormatException($"Invalid type name: {line}");

        return line[prefix.Length..^postfix.Length];
    }
}
using System;
using System.IO;
using Memoria.Bloomtown.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Memoria.Bloomtown.HarmonyHooks;

public static class StructuredJson
{
    public static void Write(String outputPath, OrderedDictionary<String, TransifexEntry> map)
    {
        using (StreamWriter output = File.CreateText(outputPath))
        {
            JsonSerializer jsonWriter = new JsonSerializer { NullValueHandling = NullValueHandling.Ignore };
            jsonWriter.Serialize(output, map);
        }
    }

    public static OrderedDictionary<String, TransifexEntry> Read(String inputPath)
    {
        using (StreamReader input = File.OpenText(inputPath))
            return Read(input);
    }

    public static OrderedDictionary<String, TransifexEntry> Read(Stream inputStream)
    {
        using (StreamReader input = new StreamReader(inputStream))
            return Read(input);
    }

    // We cannot use JsonSerializer because of framework version
    public static OrderedDictionary<String, TransifexEntry> Read(StreamReader input)
    {
        JsonTextReader reader = new JsonTextReader(input);

        OrderedDictionary<String, TransifexEntry> orderedDictionary = new OrderedDictionary<String, TransifexEntry>();
        if (!reader.Read())
            return orderedDictionary;

        ReadStartObject(reader);

        while (reader.TokenType != JsonToken.None && reader.TokenType != JsonToken.EndObject)
        {
            TransifexEntry entry = new TransifexEntry();
            String key = ReadPropertyName(reader);
            entry.Text = ReadString(reader);

            if (entry.Text is null)
                throw new NotSupportedException("if (entry.Text is null)");

            orderedDictionary.AddOrUpdate(key, entry);
        }

        return orderedDictionary;
    }

    private static String ReadString(JsonTextReader reader)
    {
        JsonToken currentToken = reader.TokenType;
        if (currentToken != JsonToken.String)
            throw new FormatException($"if (currentToken != JsonToken.String): {currentToken} (line: {reader.LinePosition})");

        var value = (String)reader.Value;
        SkipComments(reader);
        return value;
    }

    private static Int32 ReadInt32(JsonTextReader reader)
    {
        JsonToken currentToken = reader.TokenType;
        if (currentToken != JsonToken.Integer)
            throw new FormatException($"if (currentToken != JsonToken.Integer): {currentToken} (line: {reader.LinePosition})");

        var value = (Int32)reader.Value;
        SkipComments(reader);
        return value;
    }

    private static String ReadPropertyName(JsonTextReader reader)
    {
        JsonToken currentToken = reader.TokenType;
        if (currentToken != JsonToken.PropertyName)
            throw new FormatException($"if (currentToken != JsonToken.PropertyName): {currentToken} (line: {reader.LinePosition})");

        var value = (String)reader.Value;
        SkipComments(reader);
        return value;
    }

    private static void ReadStartObject(JsonTextReader reader)
    {
        JsonToken currentToken = reader.TokenType;
        if (currentToken != JsonToken.StartObject)
            throw new FormatException($"if (currentToken != JsonToken.StartObject): {currentToken} (line: {reader.LinePosition})");
        SkipComments(reader);
    }
    
    private static void ReadEndObject(JsonTextReader reader)
    {
        JsonToken currentToken = reader.TokenType;
        if (currentToken != JsonToken.EndObject)
            throw new FormatException($"if (currentToken != JsonToken.EndObject): {currentToken} (line: {reader.LinePosition})");
        SkipComments(reader);
    }

    private static JsonToken SkipComments(JsonTextReader reader)
    {
        do
        {
            reader.Read();
        } while (reader.TokenType == JsonToken.Comment);

        return reader.TokenType;
    }
}
using System;

namespace Memoria.Bloomtown.Configuration;

[AttributeUsage(AttributeTargets.Property)]
public sealed class ConfigConverterAttribute : Attribute
{
    public String ConverterInstance { get; }
    
    public ConfigConverterAttribute(String converterInstance)
    {
        ConverterInstance = converterInstance;
    }
}
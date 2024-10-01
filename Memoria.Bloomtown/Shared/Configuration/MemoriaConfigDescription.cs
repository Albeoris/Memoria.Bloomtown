using System;
using BepInEx.Configuration;

namespace Memoria.Bloomtown.Configuration;

public sealed class MemoriaConfigDescription : ConfigDescription
{
    public MemoriaConfigDescription(String description, AcceptableValueBase acceptableValues = null, params Object[] tags)
        : base(description, acceptableValues, tags)
    {
    }

    public Boolean HasFileDefinedValue { get; set; }
}
using System;
using BepInEx.Configuration;
using LazyBearTechnology;

namespace Memoria.Bloomtown.Configuration.Hotkey;

public static class GamepadButtonTypeConverter
{
    private static Type SupportedType { get; } = typeof(GamepadButton);

    public static void Init()
    {
        if (!TomlTypeConverter.CanConvert(SupportedType))
        {
            TypeConverter converter = new() { ConvertToObject = ConvertToObject, ConvertToString = ConvertToString };
            TomlTypeConverter.AddConverter(SupportedType, converter);
        }
    }

    private static String ConvertToString(Object boxed, Type type)
    {
        CheckSupportedType(type);

        GamepadButton binding = (GamepadButton)boxed;
        return ConvertToString(binding);
    }

    public static String ConvertToString(GamepadButton binding)
    {
        if (binding == default)
            return "None";

        if (!EnumerationMapping<GamepadButton>.Names.TryGetValue(binding, out String value))
            return "None";
        
        return value;
    }

    private static Object ConvertToObject(String value, Type type)
    {
        CheckSupportedType(type);

        return ConvertToObject(value);
    }

    public static GamepadButton ConvertToObject(String value)
    {
        if (String.IsNullOrWhiteSpace(value))
            return GamepadButton.None;
        
        if (!EnumerationCache<GamepadButton>.NamedValues.TryGetValue(value, out GamepadButton button))
            return GamepadButton.None;

        return button;
    }

    private static void CheckSupportedType(Type type)
    {
        if (type != SupportedType)
            throw new NotSupportedException($"An unexpected type has occurred: {type.FullName}. Expected: {SupportedType.FullName}");
    }
}
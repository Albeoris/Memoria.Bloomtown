using System.Globalization;
using ValveKeyValue;

namespace Memoria.MSBuild.SteamLibraryAccessor;

public static class ValveKeyValueExtensions
{
    public static IFormatProvider FormatProvider => CultureInfo.InvariantCulture;
    
    public static IEnumerable<KVObject> EnumerateChildren(this KVObject self, String childName)
    {
        KVValue? obj = self[childName];
        if (obj is null)
            yield break;

        foreach (KVObject kvObject in (IEnumerable<KVObject>)obj)
            yield return kvObject;
    }

    public static String GetString(this KVObject self, String childName) => self.GetValue(childName).ToString(FormatProvider).Replace(@"\\", @"\");
    public static Int16 GetSByte(this KVObject self, String childName) => self.GetValue(childName).ToSByte(FormatProvider);
    public static Int16 GetInt16(this KVObject self, String childName) => self.GetValue(childName).ToInt16(FormatProvider);
    public static Int32 GetInt32(this KVObject self, String childName) => self.GetValue(childName).ToInt32(FormatProvider);
    public static Int64 GetInt64(this KVObject self, String childName) => self.GetValue(childName).ToInt64(FormatProvider);
    public static Int16 GetByte(this KVObject self, String childName) => self.GetValue(childName).ToByte(FormatProvider);
    public static UInt16 GetUInt16(this KVObject self, String childName) => self.GetValue(childName).ToUInt16(FormatProvider);
    public static UInt32 GetUInt32(this KVObject self, String childName) => self.GetValue(childName).ToUInt32(FormatProvider);
    public static UInt64 GetUInt64(this KVObject self, String childName) => self.GetValue(childName).ToUInt64(FormatProvider);
    
    public static String? FindString(this KVObject self, String childName) => self.FindValue(childName)?.ToString(FormatProvider).Replace(@"\\", @"\");
    public static Int16? FindSByte(this KVObject self, String childName) => self.FindValue(childName)?.ToSByte(FormatProvider);
    public static Int16? FindInt16(this KVObject self, String childName) => self.FindValue(childName)?.ToInt16(FormatProvider);
    public static Int32? FindInt32(this KVObject self, String childName) => self.FindValue(childName)?.ToInt32(FormatProvider);
    public static Int64? FindInt64(this KVObject self, String childName) => self.FindValue(childName)?.ToInt64(FormatProvider);
    public static Int16? FindByte(this KVObject self, String childName) => self.FindValue(childName)?.ToByte(FormatProvider);
    public static UInt16? FindUInt16(this KVObject self, String childName) => self.FindValue(childName)?.ToUInt16(FormatProvider);
    public static UInt32? FindUInt32(this KVObject self, String childName) => self.FindValue(childName)?.ToUInt32(FormatProvider);
    public static UInt64? FindUInt64(this KVObject self, String childName) => self.FindValue(childName)?.ToUInt64(FormatProvider);

    public static KVValue? FindValue(this KVObject self, String childName)
    {
        if (self is null)  throw new ArgumentNullException(nameof(self));
        if (childName is null) throw new ArgumentNullException(nameof(childName));
        
        return self[childName];
    }
    
    public static KVValue GetValue(this KVObject self, String childName)
    {
        KVValue? value = self.FindValue(childName);
        if (value is null)
            throw new ArgumentException($"Cannot find value [{childName}] in object [{self}]", nameof(childName));

        return value;
    }
}
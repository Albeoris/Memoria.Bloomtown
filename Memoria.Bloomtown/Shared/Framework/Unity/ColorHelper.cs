using System;
using UnityEngine;

namespace Memoria.Bloomtown.Shared.Framework.Unity;

public class ColorHelper
{
    public static Color32 DecodeColor(UInt32 color)
    {
        Byte a = (Byte)((color >> 24) & 0xFF);
        Byte r = (Byte)((color >> 16) & 0xFF);
        Byte g = (Byte)((color >> 8) & 0xFF); 
        Byte b = (Byte)(color & 0xFF);        

        return new Color32(r, g, b, a);
    }
    
    public static UInt32 EncodeColor(Color32 color)
    {
        return (UInt32)(color.a << 24) | 
               (UInt32)(color.r << 16) | 
               (UInt32)(color.g << 8) | 
               color.b;
    }
}
using System;

namespace Memoria.Bloomtown.Shared.Framework.Unity;

public static class TypeCache<T>
{
    public static readonly Type Type = typeof(T);
}
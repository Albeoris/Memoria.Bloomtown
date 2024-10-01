using System;
using System.Collections.Generic;

namespace Memoria.Bloomtown.Core;

public struct Reference<T> : IEquatable<Reference<T>>
{
    public String Key { get; }
    public T Value { get; }

    public Reference(String key, T value)
    {
        Key = key;
        Value = value;
    }

    public Boolean Equals(Reference<T> other) => Key == other.Key && EqualityComparer<T>.Default.Equals(Value, other.Value);
    public override Boolean Equals(Object obj) => obj is Reference<T> other && Equals(other);
    public override Int32 GetHashCode() => unchecked(((Key != null ? Key.GetHashCode() : 0) * 397) ^ EqualityComparer<T>.Default.GetHashCode(Value));
    public static Boolean operator ==(Reference<T> left, Reference<T> right) => left.Equals(right);
    public static Boolean operator !=(Reference<T> left, Reference<T> right) => !left.Equals(right);
    public override String ToString() => $"Key: {Key}, Value: {Value}";
}
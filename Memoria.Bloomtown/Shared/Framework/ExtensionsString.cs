﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Memoria.Bloomtown.Core;

public static class ExtensionsString
{
    public static String Region(this String str, Int32 beginOffset, Int32 endOffset)
    {
        Int32 index = beginOffset;
        Int32 length = str.Length - beginOffset - endOffset;
        return str.Substring(index, length);
    }

    public static String ReplaceChars(this String str, String target, params Char[] source)
    {
        if (String.IsNullOrEmpty(str))
            return str;
        if (source == null || source.Length == 0)
            return str;

        StringBuilder sb = new StringBuilder(str.Length);
        foreach (var ch in str)
        {
            Boolean finded = false;
            for (Int32 i = 0; i < source.Length; i++)
            {
                if (ch == source[i])
                {
                    sb.Append(target);
                    finded = true;
                    break;
                }
            }

            if (finded)
                continue;

            sb.Append(ch);
        }

        return sb.ToString();
    }

    public static String ReplaceAll(this String str, params IList<Reference<TextReplacement>>[] mapArray)
    {
        if (String.IsNullOrEmpty(str))
            return str;

        if (mapArray is null)
            throw new ArgumentNullException(nameof(mapArray));

        if (mapArray.Length == 0)
            return str;

        StringBuilder result = new StringBuilder(str.Length);
        StringBuilder word = new StringBuilder(str.Length);
        Int32[][] indicesArray = new Int32[mapArray.Length][];
        for (Int32 k = 0; k < indicesArray.Length; k++)
            indicesArray[k] = new Int32[mapArray[k].Count];

        for (Int32 characterIndex = 0; characterIndex < str.Length; characterIndex++)
        {
            Char c = str[characterIndex];
            word.Append(c);

            for (Int32 n = 0; n < indicesArray.Length; n++)
            {
                Int32[] indices = indicesArray[n];
                var map = mapArray[n];

                for (var i = 0; i < indices.Length; i++)
                {
                    String old = map[i].Key;
                    if (word.Length - 1 != indices[i])
                        continue;

                    if (old.Length == word.Length && old[word.Length - 1] == c)
                    {
                        indices[i] = -old.Length;
                        continue;
                    }

                    if (old.Length > word.Length && old[word.Length - 1] == c)
                    {
                        indices[i]++;
                        continue;
                    }

                    indices[i] = 0;
                }
            }

            Int32 length = 0, array = -1, index = -1;
            Boolean exists = false;
            for (Int32 n = 0; n < indicesArray.Length; n++)
            {
                Int32[] indices = indicesArray[n];
                for (Int32 i = 0; i < indices.Length; i++)
                {
                    if (indices[i] > 0)
                    {
                        exists = true;
                        break;
                    }

                    if (-indices[i] > length)
                    {
                        length = -indices[i];
                        index = i;
                        array = n;
                    }
                }

                if (exists)
                    break;
            }

            if (exists)
                continue;

            if (index >= 0)
            {
                String value = mapArray[array][index].Value.Replace(str, word, ref characterIndex, ref length);
                word.Remove(0, length);
                result.Append(value);

                if (word.Length > 0)
                {
                    characterIndex -= word.Length;
                    word.Length = 0;
                }
            }

            result.Append(word);
            word.Length = 0;
            for (Int32 n = 0; n < indicesArray.Length; n++)
            {
                Int32[] indices = indicesArray[n];
                for (Int32 i = 0; i < indices.Length; i++)
                    indices[i] = 0;
            }
        }

        if (word.Length > 0)
            result.Append(word);

        return result.ToString();
    }

    public static String TrimEnd(this String source, String suffix, StringComparison comparisonType)
    {
        if (source == null)
            return null;

        if (String.IsNullOrEmpty(suffix))
            throw new ArgumentException(nameof(suffix));

        Int32 sourceLength = source.Length;
        Int32 suffixLength = suffix.Length;

        if (suffixLength > sourceLength)
            return source;

        Int32 index = sourceLength - suffixLength;
        String sub = source.Substring(index);
        if (!sub.Equals(suffix, comparisonType))
            return source;

        if (index == 0)
            return String.Empty;

        return source.Substring(0, index);
    }
}
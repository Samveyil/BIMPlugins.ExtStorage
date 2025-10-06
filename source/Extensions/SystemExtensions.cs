using System;
using System.Collections.Generic;
using System.IO;

namespace BIMPlugins.ExtStorage.Extensions
{
    public static class SystemExtensions
    {
        public static double Round(this double source)
        {
            return Math.Round(source, 9);
        }
        public static double Round(this double source, int digits)
        {
            return Math.Round(source, digits);
        }
        public static bool IsNullOrEmpty(this string? source)
        {
            return string.IsNullOrEmpty(source);
        }
        public static bool IsNullOrWhiteSpace(this string? source)
        {
            return string.IsNullOrWhiteSpace(source);
        }
        public static string AppendPath(this string source, string path)
        {
            return Path.Combine(source, path);
        }
        public static bool Contains(this string source, string value, StringComparison comparisonType)
        {
            return source?.IndexOf(value, comparisonType) >= 0;
        }
    }
}
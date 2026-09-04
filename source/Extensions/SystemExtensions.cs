using System;
using System.IO;

namespace BIMPlugins.ExtStorage.Extensions
{
    public static class SystemExtensions
    {
        /// <summary>
        /// Rounds a double-precision floating-point value to 9 fractional digits, and rounds midpoint values to the nearest even number.
        /// </summary>
        /// <returns>The number nearest to <paramref name="value"/> with exactly 9 digits after the decimal point.</returns>
        public static double Round(this double value) => Math.Round(value, 9);

        /// <inheritdoc cref="System.Math.Round(double, int)"/>
        public static double Round(this double value, int digits) => Math.Round(value, digits);

        /// <inheritdoc cref="System.String.IsNullOrEmpty(string)"/>
        public static bool IsNullOrEmpty(this string value) => string.IsNullOrEmpty(value);

        /// <inheritdoc cref="System.String.IsNullOrWhiteSpace(string)"/>
        public static bool IsNullOrWhiteSpace(this string value) => string.IsNullOrWhiteSpace(value);

        /// <inheritdoc cref="System.IO.Path.Combine(string, string)"/>
        public static string AppendPath(this string source, string path2) => Path.Combine(source, path2);

        /// <summary>Returns a value indicating whether a string contains a substring when using the specified form of string comparison.</summary>
        /// <param name="value">The string to seek.</param>
        /// <param name="comparisonType">One of the enumeration values that specifies the rules to use in the comparison.</param>
        /// <returns><see langword="true"/> if the <paramref name="value"/> parameter occurs within this string, or if <paramref name="value"/> is the empty string (""); otherwise, <see langword="false"/>.</returns>
        public static bool Contains(this string source, string value, StringComparison comparisonType) => source?.IndexOf(value, comparisonType) >= 0;

        /// <summary>Converts an object's type to <typeparamref name="T" /> type</summary>
        public static T To<T>(this object obj) => (T)obj;
    }
}
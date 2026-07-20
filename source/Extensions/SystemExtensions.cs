using System;
using System.Collections.Generic;
using System.IO;

namespace BIMPlugins.ExtStorage.Extensions
{
    public static class SystemExtensions
    {
        /// <summary>
        /// Rounds a double-precision floating-point value to 9 fractional digits, and rounds midpoint values to the nearest even number.
        /// </summary>
        /// <returns>The number nearest to <paramref name="value"/> with exactly 9 digits after the decimal point.</returns>
        public static double Round(this double value)
        {
            return Math.Round(value, 9);
        }

        /// <summary>
        /// Rounds a double-precision floating-point value to a specified number of fractional digits, and rounds midpoint values to the nearest even number.
        /// </summary>
        /// <param name="digits">The number of fractional digits in the return value.</param>
        /// <remarks>The value of the <paramref name="digits"/> argument can range from 0 to 15. The maximum number of integral and fractional digits supported by the Double type is 15.</remarks>
        /// <returns>The number nearest to <paramref name="value"/> that contains a number of fractional digits equal to <paramref name="digits"/>.</returns>
        public static double Round(this double value, int digits)
        {
            return Math.Round(value, digits);
        }

        /// <summary>Indicates whether the specified string is <see langword="null"/> or an empty string ("").</summary>
        /// <returns><see langword="true"/> if the <paramref name="value"/> parameter is <see langword="null"/> or an empty string (""); otherwise, <see langword="false"/>.</returns>
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        /// <summary>Indicates whether a specified string is <see langword="null"/>, empty, or consists only of white-space characters.</summary>
        /// <returns><see langword="true"/> if the <paramref name="value"/> parameter is <see langword="null"/> or <see cref="string.Empty"/>, or if <paramref name="value"/> consists exclusively of white-space characters.</returns>
        public static bool IsNullOrWhiteSpace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        /// <summary>Combines two strings into a path.</summary>
        /// <param name="path2">The second path to combine.</param>
        /// <returns>The combined paths. If one of the specified paths is a zero-length string, this method returns the other path. If <paramref name="path2"/> contains an absolute path, this method returns <paramref name="path2"/>.</returns>
        public static string AppendPath(this string source, string path2)
        {
            return Path.Combine(source, path2);
        }

        /// <summary>Returns a value indicating whether a string contains a substring when using the specified form of string comparison.</summary>
        /// <param name="value">The string to seek.</param>
        /// <param name="comparisonType">One of the enumeration values that specifies the rules to use in the comparison.</param>
        /// <returns><see langword="true"/> if the <paramref name="value"/> parameter occurs within this string, or if <paramref name="value"/> is the empty string (""); otherwise, <see langword="false"/>.</returns>
        public static bool Contains(this string source, string value, StringComparison comparisonType)
        {
            return source?.IndexOf(value, comparisonType) >= 0;
        }
    }
}
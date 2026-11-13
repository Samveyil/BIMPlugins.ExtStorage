using System;
using System.Collections;
using System.Collections.Generic;

namespace BIMPlugins.ExtStorage.Comparers
{
    /// <summary>Compares strings in natural order: sequences of digits are compared as numbers, as Windows Explorer sorts file names.</summary>
    /// <remarks>
    /// "Level 2" comes before "Level 10", "A9" before "a10". Numbers of any length are compared without overflow.
    /// The text between numbers is compared by the <see cref="StringComparison"/> the comparer is created with.
    /// Strings that differ only in leading zeros or in what the comparison treats as equal still get a fixed order:
    /// fewer leading zeros first, then by character codes. <see langword="null"/> is less than any string.
    /// </remarks>
    /// <example>
    /// <code>
    /// var sorted = levelNames.OrderBy(n => n, NaturalComparer.OrdinalIgnoreCase); // "Level 2" comes before "Level 10"
    /// var sheets = sheetNumbers.OrderBy(n => n, new NaturalComparer(StringComparison.CurrentCultureIgnoreCase));
    /// </code>
    /// </example>
    public class NaturalComparer : IComparer<string>, IComparer
    {
        /// <summary>Compares the text between numbers by character codes, case-sensitive.</summary>
        public static NaturalComparer Ordinal { get; } = new NaturalComparer(StringComparison.Ordinal);

        /// <summary>Compares the text between numbers by character codes, ignoring case.</summary>
        public static NaturalComparer OrdinalIgnoreCase { get; } = new NaturalComparer(StringComparison.OrdinalIgnoreCase);

        /// <summary>Compares the text between numbers by the rules of the current culture, case-sensitive.</summary>
        public static NaturalComparer CurrentCulture { get; } = new NaturalComparer(StringComparison.CurrentCulture);

        /// <summary>Compares the text between numbers by the rules of the current culture, ignoring case, as Windows Explorer does.</summary>
        public static NaturalComparer CurrentCultureIgnoreCase { get; } = new NaturalComparer(StringComparison.CurrentCultureIgnoreCase);

        private readonly StringComparison _comparison;

        /// <summary>Creates a comparer that compares the text between numbers by the rules of the current culture, ignoring case.</summary>
        public NaturalComparer() : this(StringComparison.CurrentCultureIgnoreCase)
        {
        }

        /// <summary>Creates a comparer that compares the text between numbers as specified.</summary>
        /// <param name="comparison">How the text between numbers is compared, for example <see cref="StringComparison.OrdinalIgnoreCase"/>.</param>
        public NaturalComparer(StringComparison comparison)
        {
            _comparison = comparison;
        }

        /// <summary>Compares two strings in natural order.</summary>
        /// <param name="x">The first string to compare.</param>
        /// <param name="y">The second string to compare.</param>
        /// <returns>A negative number if <paramref name="x"/> is less than <paramref name="y"/>, zero if they are equal, otherwise a positive number.</returns>
        public int Compare(string x, string y)
        {
            if (ReferenceEquals(x, y)) return 0;
            if (x == null) return -1;
            if (y == null) return 1;

            int ix = 0, iy = 0;
            var zerosOrder = 0;

            while (ix < x.Length && iy < y.Length)
            {
                var digitX = char.IsDigit(x[ix]);
                var digitY = char.IsDigit(y[iy]);
                var endX = ChunkEnd(x, ix, digitX);
                var endY = ChunkEnd(y, iy, digitY);

                int result;
                if (digitX && digitY)
                {
                    result = CompareNumbers(x, ix, endX, y, iy, endY, ref zerosOrder);
                }
                else
                {
                    result = string.Compare(x.Substring(ix, endX - ix), y.Substring(iy, endY - iy), _comparison);
                }

                if (result != 0)
                    return result;

                ix = endX;
                iy = endY;
            }

            // The one that ran out of chunks first is less: "Level" before "Level 2".
            if (ix < x.Length) return 1;
            if (iy < y.Length) return -1;

            return zerosOrder != 0 ? zerosOrder : string.CompareOrdinal(x, y);
        }

        int IComparer.Compare(object x, object y) => Compare(x as string, y as string);

        private static int ChunkEnd(string s, int start, bool digits)
        {
            var i = start;
            while (i < s.Length && char.IsDigit(s[i]) == digits)
                i++;
            return i;
        }

        // Compares digit runs by value without parsing them: after leading zeros, the longer run is the larger number,
        // and runs of equal length compare digit by digit. Any Unicode decimal digits count, by their numeric value.
        private static int CompareNumbers(string x, int startX, int endX, string y, int startY, int endY, ref int zerosOrder)
        {
            var significantX = SkipZeros(x, startX, endX);
            var significantY = SkipZeros(y, startY, endY);

            var lengthX = endX - significantX;
            var lengthY = endY - significantY;
            if (lengthX != lengthY)
                return lengthX.CompareTo(lengthY);

            for (var i = 0; i < lengthX; i++)
            {
                var result = char.GetNumericValue(x[significantX + i]).CompareTo(char.GetNumericValue(y[significantY + i]));
                if (result != 0)
                    return result;
            }

            // Equal numbers: "1" before "01", decided only if nothing else differs.
            if (zerosOrder == 0)
                zerosOrder = (significantX - startX).CompareTo(significantY - startY);

            return 0;
        }

        private static int SkipZeros(string s, int start, int end)
        {
            while (start < end - 1 && char.GetNumericValue(s[start]) == 0)
                start++;
            return start;
        }
    }
}

using System.Collections.Generic;

namespace BIMPlugins.ExtStorage.Comparers
{
    /// <summary>Compares strings in natural order: sequences of digits are compared as numbers.</summary>
    /// <example>
    /// <code>
    /// var sorted = levelNames.OrderBy(n => n, new NaturalComparer()); // "Level 2" comes before "Level 10"
    /// </code>
    /// </example>
    public class NaturalComparer : IComparer<string>
    {
        /// <summary>Compares two strings in natural order.</summary>
        /// <param name="x">The first string to compare.</param>
        /// <param name="y">The second string to compare.</param>
        /// <remarks>Other characters are compared by their code, case-sensitive. <see langword="null"/> is less than any string.</remarks>
        /// <returns>A negative number if <paramref name="x"/> is less than <paramref name="y"/>, zero if they are equal, otherwise a positive number.</returns>
        public int Compare(string x, string y)
        {
            if (x == null && y == null) return 0;
            if (x == null) return -1;
            if (y == null) return 1;

            int ix = 0, iy = 0;

            while (ix < x.Length && iy < y.Length)
            {
                if (char.IsDigit(x[ix]) && char.IsDigit(y[iy]))
                {
                    int numX = 0, numY = 0;

                    while (ix < x.Length && char.IsDigit(x[ix]))
                    {
                        numX = numX * 10 + (x[ix] - '0');
                        ix++;
                    }

                    while (iy < y.Length && char.IsDigit(y[iy]))
                    {
                        numY = numY * 10 + (y[iy] - '0');
                        iy++;
                    }

                    if (numX != numY)
                        return numX.CompareTo(numY);
                }
                else
                {
                    if (x[ix] != y[iy])
                        return x[ix].CompareTo(y[iy]);
                    ix++;
                    iy++;
                }
            }

            return x.Length.CompareTo(y.Length);
        }
    }
}

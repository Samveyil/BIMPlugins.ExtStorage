namespace BIMPlugins.ExtStorage.Comparers
{
    public class NaturalComparer : IComparer<string>
    {
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

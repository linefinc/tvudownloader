using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace TvUndergroundDownloaderLib
{
    public static class LinqExtension
    {
        public static BigInteger SumBigInteger(this IEnumerable<BigInteger> source)
        {
            if (source.Count() == 0)
            {
                throw new InvalidOperationException("Cannot compute sum for an empty set.");
            }

            BigInteger bg = 0;

            foreach (BigInteger bigInteger in source)
            {
                bg += bigInteger;
            }
            return bg;
        }

        public static BigInteger SumBigInteger<T>(this IEnumerable<T> numbers,
            Func<T, BigInteger> selector)
        {
            return (from num in numbers select selector(num)).SumBigInteger();
        }
    }
}
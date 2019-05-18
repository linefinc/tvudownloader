using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace TvUndergroundDownloaderLib.Extensions
{
    public static class BigIntegerExtension
    {
        /// <summary>
        /// Return file size with correct size KB, MB, GB
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string SmartFomater(this BigInteger value)
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value));

            //
            //  Use base 2
            //  https://en.wikipedia.org/wiki/File_size
            //

            var magnetude = BigInteger.Log(value, 2);

            if ((magnetude > 0) & (magnetude <= 10))
            {
                return value.ToString() + " byte";
            }

            if ((magnetude > 10) & (magnetude <= 20))
            {
                double outValue = Math.Pow(2.0, magnetude - 10);
                return outValue.ToString("F1") + "KB";
            }

            if ((magnetude > 20) & (magnetude <= 30))
            {
                double outValue = Math.Pow(2.0, magnetude - 20);
                return outValue.ToString("F1") + "MB";
            }

            if ((magnetude > 30) & (magnetude <= 40))
            {
                double outValue = Math.Pow(2.0, magnetude - 30);
                return outValue.ToString("F1") + "GB";
            }

            if ((magnetude > 40) & (magnetude <= 50))
            {
                double outValue = Math.Pow(2.0, magnetude - 40);
                return outValue.ToString("F1") + "TB";
            }

            return value.ToString();
        }

        /// <summary>
        /// Sum all size in an list
        /// </summary>
        /// <param name="source">Data source</param>
        /// <returns></returns>
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

        /// <summary>
        /// Sum all size in an list (labda style)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="numbers"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static BigInteger SumBigInteger<T>(this IEnumerable<T> numbers,
            Func<T, BigInteger> selector)
        {
            return (from num in numbers select selector(num)).SumBigInteger();
        }
    }
}
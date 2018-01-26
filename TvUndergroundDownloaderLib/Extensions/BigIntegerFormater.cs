using System;
using System.Numerics;

namespace TvUndergroundDownloaderLib.Extensions
{
    public static class BigIntegerFormater
    {
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

    }
}
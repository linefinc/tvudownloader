using System;
using System.Numerics;

namespace TvUndergroundDownloaderLib.Extensions
{
    public static class BigIntegerFormater
    {
        public static string SmartFomater(this BigInteger value)
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(value));

            var magnetude = BigInteger.Log10(value);

            if ((magnetude > 0) & (magnetude <= 3))
            {
                return value.ToString() + " byte";
            }

            if ((magnetude > 3) & (magnetude <= 6))
            {
                double outValue = Math.Pow(10.0, magnetude - 3);
                return outValue.ToString("F1") + "KB";
            }

            if ((magnetude > 6) & (magnetude <= 9))
            {
                double outValue = Math.Pow(10.0, magnetude - 6);
                return outValue.ToString("F1") + "MB";
            }

            if ((magnetude > 9) & (magnetude <= 12))
            {
                double outValue = Math.Pow(10.0, magnetude - 9);
                return outValue.ToString("F1") + "GB";
            }

            if ((magnetude > 12) & (magnetude <= 15))
            {
                double outValue = Math.Pow(10.0, magnetude - 12);
                return outValue.ToString("F1") + "TB";
            }

            return value.ToString();
        }

    }
}
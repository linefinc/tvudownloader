using System;

namespace TvUndergroundDownloaderLib
{
    [Serializable]
    public class WrongPageFormatException : Exception
    {
        public WrongPageFormatException()
        {
        }

        public WrongPageFormatException(string message) : base(message)
        {
        }

        public WrongPageFormatException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
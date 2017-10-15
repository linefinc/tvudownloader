using System;

namespace TvUndergroundDownloaderLib
{
    [Serializable]
    public class WrongPageFormatException : System.Exception
    {
        public WrongPageFormatException()
        {
        }

        public WrongPageFormatException(string message) : base(message)
        {
        }

        public WrongPageFormatException(string message, System.Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
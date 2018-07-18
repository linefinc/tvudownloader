using System;

namespace TvUndergroundDownloaderLib
{
    [Serializable]
    public class UnsupportedVersionException : Exception
    {
        public UnsupportedVersionException()
        {
        }

        public UnsupportedVersionException(string message) : base(message)
        {
        }

        public UnsupportedVersionException(string message, System.Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
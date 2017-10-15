using System;

namespace TvUndergroundDownloaderLib
{
    [Serializable]
    public class LoginException : System.Exception
    {
        public LoginException()
        {
        }

        public LoginException(string message) : base(message)
        {
        }

        public LoginException(string message, System.Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
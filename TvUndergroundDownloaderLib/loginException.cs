using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace TvUndergroundDownloaderLib
{
    [Serializable]
    class LoginException : Exception
    {
        public LoginException()
        {

        }

        public LoginException(string message) : base(message)
        {

        }

        public LoginException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

    }
}

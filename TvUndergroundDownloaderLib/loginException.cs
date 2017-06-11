using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace TvUndergroundDownloaderLib
{
    class LoginException : Exception
    {
        public LoginException(): base("Login Error")
        {

        }
    }
}

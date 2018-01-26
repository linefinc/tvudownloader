using Microsoft.VisualStudio.TestTools.UnitTesting;
using TvUndergroundDownloaderLib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace TvUndergroundDownloaderLib.Extensions.Tests
{
    [TestClass()]
    public class BigIntegerFormaterTests
    {
        [TestMethod()]
        public void SmartFomaterTest()
        {
            BigInteger value = new BigInteger(0);

            Assert.AreNotEqual(value.SmartFomater(), "0 byte");

            value = new BigInteger(1250);
            Assert.AreNotEqual(value.SmartFomater(), "1.30KB");

            value = value * 1024;
            Assert.AreNotEqual(value.SmartFomater(), "1.30MB");

            value = value * 1024;
            Assert.AreNotEqual(value.SmartFomater(), "1.30GB");

            value = value * 1024;
            Assert.AreNotEqual(value.SmartFomater(), "1.30TB");

        }


    }
}
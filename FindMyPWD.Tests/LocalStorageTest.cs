using NUnit.Framework;
using FindMyPWD.Helper;
using FindMyPWD.Model;

namespace LocalStorage.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestGetPairedDevice()
        {
            Assert.Equals(localStorage.getPairedDevice().Count, 0);
        }

        [Test]
        public void TestWrite()
        {
            localStorage.write(new BLEDevice("test device", "0"));
            Assert.Equals(localStorage.getPairedDevice().Count, 1);
            localStorage.write(new BLEDevice("test device 2", "1"));
            Assert.Equals(localStorage.getPairedDevice().Count, 2);
        }
    }
}
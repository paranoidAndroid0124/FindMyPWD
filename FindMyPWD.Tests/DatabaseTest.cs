using NUnit.Framework;
using FindMyPWD.Helper;

namespace Database.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestGetDB()
        {
            DBConnnection.GetDB();
            var db = DBConnnection.List; // write test based on what should be in here
            Assert.Pass(); // currently, passes if the program doesn't crash
        }
    }
}
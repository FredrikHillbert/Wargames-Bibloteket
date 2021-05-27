using System;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WargamesGUI.Services;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        public static string theConStringTest2 = ConfigurationManager.ConnectionStrings[2].ConnectionString;
        UserService userService = new UserService();

        [TestMethod]
        [DataRow("Lib", "Lib123")]
        [DataRow("AlexVis2", "123")]
        public void SignInTest(string username, string password)
        {            
            var actual = userService.SignIn(username, password, theConStringTest2);
            var expected = 2;

            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void TestMethod1()
        {
            Assert.AreEqual(0, 0);
        }
    }
}

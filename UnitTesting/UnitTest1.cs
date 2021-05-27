using System;
using Xunit;

namespace UnitTesting
{
    
    public class UnitTest1
    {

        [Theory]
        [InlineData(true, "Lib", "Lib123", 2 )]
        public void AddNewUserTest(bool expected, string username, string password, int privilege)
        {
            bool actual = Methods.SignInTest(username, password, privilege);

            Assert.Equal(expected, actual);
        }



    }
}

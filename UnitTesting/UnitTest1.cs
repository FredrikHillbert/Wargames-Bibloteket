using System;
using Xunit;

namespace UnitTesting
{
    
    public class UnitTest1
    {

        [Theory]
        [InlineData(false, "&%¤#", "asda", 4 )]
        public void AddNewUserTest(bool expected, string username, string password, int privilege)
        {
            bool actual = Methods.AddNewUserReal(username, password, privilege);

            Assert.Equal(expected, actual);
        }



    }
}

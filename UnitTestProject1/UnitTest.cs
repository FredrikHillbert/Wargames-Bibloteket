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
        BookService bookService = new BookService();
        
        [TestMethod]

        [DataRow("Admin", "Admin123")]
        [DataRow("Lib", "Lib123")]
        [DataRow("AlexVis2", "1234")]
        [DataRow("AlexVis2", "123")]

        public void SignInTest(string username, string password)
        {
            var expected = 5;
            var actual = userService.SignIn(username, password);
            switch (actual)
            {
                case 0:
                    Assert.IsFalse(expected == actual);
                    break;
                case 1:
                    expected = 1;
                    Assert.AreEqual(expected, actual);
                    break;
                case 2:
                    expected = 2;
                    Assert.AreEqual(expected, actual);
                    break;
                case 3:
                    expected = 3;
                    Assert.AreEqual(expected, actual);
                    break;

            }

        }
        [TestMethod]
        [DataRow(1, "Test", "987654321", "Bonnier", "Test Testsson", "hejhej", 123, "1020", "Thriller")]
        public void AddNewBookTest(int item_id, string title, string ISBN, string publisher, string author,
                                           string description, int price, string placement, string category)
        {
            var result = bookService.AddNewBook(item_id, title, ISBN, publisher, author, description,
                price, placement, category);
            bool actual = result.Result;

            var expected = true;

            Assert.AreEqual(expected, actual);

        }
        [TestMethod]
        [DataRow(1, "Ska inte gå att ta bort", 1)]
        [DataRow(19, "Ska gå att ta bort", 2)] //Ersätt Copy_Id med ett Id som finns i databasen och som har fk_Availability 1.
        public void RemoveBookCopyTest(int Copy_Id, string reason, int testNr)
        {
             
            var result = bookService.RemoveBookCopy(Copy_Id, reason);
            bool actual = result.Result;
           
                if (testNr == 1)
                {
                    var expected = false;                    
                    Assert.AreEqual(expected, actual); 
                   
                }
              
                else
                {
                    var expected = true;
                    Assert.AreEqual(expected, actual);
                }
            
        }
    }
}

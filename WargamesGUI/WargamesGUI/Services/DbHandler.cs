using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using WargamesGUI.Models;
using System.Configuration;

namespace WargamesGUI.Services
{
    public class DbHandler
    {
        //public string theConString;
        public string theConStringTest;
        //public static string theConStringTest2 = ConfigurationManager.ConnectionStrings[2].ConnectionString;
        public DbHandler()
        {
            //theConString = ConfigurationManager.ConnectionStrings[1].ConnectionString;
            theConStringTest = ConfigurationManager.ConnectionStrings[2].ConnectionString;
        }
        // Connection-string to Database      

        // Table-names
        public const string theUserTableName = "tblUser";
        public const string theBookTableName = "tblBook";
        public const string theEventTableName = "tblEvent";
        public const string theRemovedItemTableName = "tblRemovedItem";
        public const string theDeweyMainTableName = "tblDeweyMain";
        public const string theDeweySubTableName = "tblDeweySub";
        public const string theBorrowedItemTableName = "tblBookLoan";
        public const string theBookCopyTableName = "tblBookCopy";


        // Alla olika SQL-satser som vi behöver.

        //===============||
        //               ||
        //   SQL-satser  ||
        //   BOOK        ||
        //               ||
        //===============||

        // SELECT
        public string queryForBooks = $"SELECT * FROM {theBookTableName} ORDER BY Title";
        public string queryForBookCopies = $"SELECT * FROM {theBookCopyTableName}";
        public string queryForBookCopiesExtra = $"SELECT bc.Copy_Id, bc.fk_Book_Id, bc.fk_Condition_Id, bc.fk_Availability, " +
                                                $"a.Id, a.Status, " +
                                                $"cs.Condition_Id, cs.ConditionType " +
                                                $"FROM tblBookCopy bc " +
                                                $"INNER JOIN tblAvailability a " +
                                                $"ON a.Id = bc.fk_Availability " +
                                                $"INNER JOIN tblConditionStatus cs " +
                                                $"ON cs.Condition_Id = bc.fk_Condition_Id;";

        // INSERT INTO

        // UPDATE

        //===============||
        //               ||
        //   SQL-satser  ||
        //   BOOKLOAN    ||
        //               ||
        //===============||

        // SELECT


        //===============||
        //               ||
        //   SQL-satser  ||
        //   EVENT       ||
        //               ||
        //===============||

        // SELECT
        public string queryForEvents = $"SELECT * FROM {theEventTableName}";

        //===============||
        //               ||
        //   SQL-satser  ||
        //   USER        ||
        //               ||
        //===============||

        // SELECT
        public string queryForUserListPage = $"SELECT * FROM {theUserTableName} ORDER BY fk_PrivilegeLevel";
        public string queryForAdmins = $"SELECT * FROM {theUserTableName} WHERE fk_PrivilegeLevel = 1";
        public string queryForLibrarians = $"SELECT * FROM {theUserTableName} WHERE fk_PrivilegeLevel = 2";
        public string queryForVisitors = $"SELECT * FROM {theUserTableName} WHERE fk_PrivilegeLevel = 3";

        //===============||
        //               ||
        //   SQL-satser  ||
        //   DEWEY       ||
        //               ||
        //===============||

        // SELECT
        public string queryForDeweySub = $"SELECT DeweySub_Id, SubCategoryName, fk_DeweyMain_Id FROM tblDeweySub";
        public string queryForDeweyMain = $"SELECT DeweyMain_Id, MainCategoryName FROM tblDeweyMain";

        //===============||
        //               ||
        //   SQL-satser  ||
        //   LIBRARYCARD ||
        //               ||
        //===============||

        public string queryForLibraryCards = $"SELECT lc.LibraryCard_Id, lc.CardNumber, lc.fk_Status_Id, lcs.Status_Id, lcs.Status_Level " +
                                             $"FROM tblLibraryCard lc " +
                                             $"INNER JOIN tblLibraryCardStatus lcs " +
                                             $"ON lc.fk_Status_Id = lcs.Status_Id";

    }
}

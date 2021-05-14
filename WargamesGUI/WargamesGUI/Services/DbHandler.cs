using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using WargamesGUI.Models;

namespace WargamesGUI.Services
{
    public class DbHandler
    {
        // Connection-string to Database
        public const string theConString = "Server=tcp:wargameslibrary.database.windows.net,1433;Initial Catalog=Wargames Library;Persist Security Info=False;User ID=adminwargames;Password=Admin123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        
        // Table-names
        public const string theUserTableName = "tblUser";
        public const string theBookTableName = "tblBook";
        public const string theEventTableName = "tblEvent";
        public const string theRemovedItemTableName = "tblRemovedItem";
        public const string theDeweyMainTableName = "tblDeweyMain";
        public const string theDeweySubTableName = "tblDeweySub";
        public const string theBorrowedItemTableName = "tblBookLoan";


        // Alla olika SQL-satser som vi behöver.

        //===============||
        //               ||
        //   SQL-satser  ||
        //   BOOK        ||
        //               ||
        //===============||

        // SELECT - Procedures
        public string queryForBooks = $"SELECT * FROM {theBookTableName} ORDER BY Title";

        // INSERT INTO - Procedures

        // UPDATE - Procedures

        //===============||
        //               ||
        //   SQL-satser  ||
        //   EVENT       ||
        //               ||
        //===============||

        // SELECT - Procedures
        public string queryForEvents = $"SELECT * FROM {theEventTableName}";

        //===============||
        //               ||
        //   SQL-satser  ||
        //   USER        ||
        //               ||
        //===============||

        // SELECT - Procedures
        public string queryForUserListPage = $"SELECT * FROM {theUserTableName} ORDER BY fk_PrivilegeLevel";
        public string queryForAdmins = $"SELECT * FROM {theUserTableName} WHERE fk_PrivilegeLevel = 1";
        public string queryForLibrarians = $"SELECT * FROM {theUserTableName} WHERE fk_PrivilegeLevel = 2";
        public string queryForVisitors = $"SELECT * FROM {theUserTableName} WHERE fk_PrivilegeLevel = 3";


    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace WargamesGUI.Services
{
    public class DbHandler
    {
        // Alla olika connectionstrings som vi behöver.
        public const string theConString = "Server=tcp:wargameslibrary.database.windows.net,1433;Initial Catalog=Wargames Library;Persist Security Info=False;User ID=adminwargames;Password=Admin123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        public const string theUserTableName = "tblUser";
        public const string theBookTableName = "tblBook";
        public const string theEventTableName = "tblEvent";
        public const string theRemovedItemTableName = "tblRemovedItem";


        //Alla olika SQL-satser som vi behöver.
        public string queryForUserListPage = $"SELECT * FROM {theUserTableName} ORDER BY fk_PrivilegeLevel";
        public string queryForBooks = $"SELECT * FROM {theBookTableName}";
        public string queryForEvents = $"SELECT * FROM {theEventTableName}";
        public string queryForVisitors = $"SELECT * FROM {theUserTableName} WHERE fk_PrivilegeLevel = 3";


    }
}

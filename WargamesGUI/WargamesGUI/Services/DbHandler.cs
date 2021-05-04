using System;
using System.Collections.Generic;
using System.Text;

namespace WargamesGUI.Services
{
   public class DbHandler
    {
        // Alla olika connectionstrings som vi behöver.
       protected const string theConString = "Server=tcp:wargameslibrary.database.windows.net,1433;Initial Catalog=Wargames Library;Persist Security Info=False;User ID=adminwargames;Password=Admin123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
       protected const string theUserTableName = "tblUser";
       protected const string theBookTableName = "tblBook";
        protected const string theEventTableName = "tblEvent";


        //Alla olika SQL-satser som vi behöver.
        protected  string queryForUserListPage = $"SELECT * FROM {theUserTableName}";
        protected  string queryForBooks = $"SELECT * FROM {theBookTableName}";
        protected string queryForEvents = $"SELECT * FROM {theEventTableName}";
      

    }
}

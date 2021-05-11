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


        //public async Task<List<T>> Searching<T>(string text)
        //{
        //    List<T> searchedValues = new List<T>();
        //    string query = $"SELECT * FROM tblItem ti " +
        //                    $"LEFT JOIN tblBook tb ON tb.fk_Item_Id = ti.Item_Id " +
        //                    $"LEFT JOIN tblEvent te ON te.fk_Item_Id = ti.Item_Id " +
        //                    $"WHERE CONCAT_WS('', tb.Title, tb.ISBN, tb.Publisher, tb.fk_Item_Id, te.fk_Item_Id, tb.Price, tb.Placement, tb.Author, ti.TypeOfItem, te.Title, te.Description) " +
        //                    $"LIKE '%{text}%'";

        //    string query2 = $"SELECT* FROM tblBook WHERE CONCAT_WS('', Title, ISBN, Publisher, fk_Item_Id, Price, Placement, Author) LIKE '%{text}%'";



        //    using (SqlConnection con = new SqlConnection(theConString))
        //    {
        //        await con.OpenAsync();
        //        using (SqlCommand cmd = new SqlCommand(query, con))
        //        {
        //            using (var reader = await cmd.ExecuteReaderAsync())
        //            {
        //                while (await reader.ReadAsync())
        //                {
        //                    var values = new Book();

        //                    values.ISBN = reader["ISBN"].ToString();
        //                    values.Title = reader["Title"].ToString();
        //                    values.Publisher = reader["Publisher"].ToString();
        //                    values.fk_Item_Id = Convert.ToInt32(reader["fk_Item_Id"]);
        //                    values.Price = Convert.ToInt32(reader["Price"]);
        //                    values.Placement = reader["Placement"].ToString();
        //                    values.Author = reader["Author"].ToString();


        //                    switch (values.fk_Item_Id)
        //                    {
        //                        case 1:
        //                            values.Category = "Book";
        //                            break;
        //                        case 2:
        //                            values.Category = "Ebook";
        //                            break;
        //                        case 3:
        //                            values.Category = "Event";
        //                            break;
        //                        default:
        //                            break;
        //                    }


        //                    searchedValues.Add(values);
        //                }

        //            }
        //        }
        //        return await Task.FromResult(searchedValues);
        //    }
        //}
    }
}

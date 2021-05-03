using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Data;
using System.Data.SqlClient;

namespace WargamesGUI.Services
{
    class BookService
    {
        private const string theConString = "Server=tcp:wargameslibrary.database.windows.net,1433;Initial Catalog=Wargames Library;Persist Security Info=False;User ID=adminwargames;Password=Admin123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        private const string theBookTableName = "Book";
        private const string theObjectsRemovedTableName = "";

        private string queryForBookListPage = string.Empty;

        public DataTable GetBooksFromDb()
        {
            using (SqlConnection con = new SqlConnection(theConString))
            {
                con.Open();
                SqlCommand com = new SqlCommand(queryForBookListPage, con);
                SqlDataAdapter sda = new SqlDataAdapter(com);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                return dt;
            }
        }
        public bool AddNewBook(string Title, string ISBN, string Publisher, string Description, int Price, string Placement)
        {

            bool success = true;

            try
            {
                using (SqlConnection con = new SqlConnection(theConString))
                {
                    string sql = 
                        $"INSERT INTO tbl{theBookTableName}" +
                        $"(fk_category_Id, Title, ISBN, Publisher, Description, Price, Placement) " +
                        $"VALUES('1', '{Title}', '{ISBN}', '{Publisher}', '{Description}', '{Price}', '{Placement}')";

                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }

                return success;
            }

            catch (Exception)
            {
                success = false;
                return success;
            }
        }
        //public bool AddNewBook(string Title, string ISBN, string Publisher, string Description, int Price)
        //{

        //    bool success = true;

        //    try
        //    {
        //        using (SqlConnection con = new SqlConnection(theConString))
        //        {
        //            string sql =
        //                $"INSERT INTO tbl{theBookTableName}" +
        //                $"(fk_category_Id, Title, ISBN, Publisher, Description, Price, Placement) " +
        //                $"VALUES('1', '{Title}', '{ISBN}', '{Publisher}', '{Description}', '{Price}')";

        //            con.Open();
        //            using (SqlCommand cmd = new SqlCommand(sql, con))
        //            {
        //                cmd.ExecuteNonQuery();
        //            }
        //        }

        //        return success;
        //    }

        //    catch (Exception)
        //    {
        //        success = false;
        //        return success;
        //    }
        //}
        //public bool AddNewBook(string Title, string ISBN, string Publisher, string Description)
        //{

        //    bool success = true;

        //    try
        //    {
        //        using (SqlConnection con = new SqlConnection(theConString))
        //        {
        //            string sql =
        //                $"INSERT INTO tbl{theBookTableName}" +
        //                $"(fk_category_Id, Title, ISBN, Publisher, Description, Price, Placement) " +
        //                $"VALUES('1', '{Title}', '{ISBN}', '{Publisher}', '{Description}')";

        //            con.Open();
        //            using (SqlCommand cmd = new SqlCommand(sql, con))
        //            {
        //                cmd.ExecuteNonQuery();
        //            }
        //        }

        //        return success;
        //    }

        //    catch (Exception)
        //    {
        //        success = false;
        //        return success;
        //    }
        //}
        //public bool AddNewBook(string Title, string ISBN, string Publisher)
        //{

        //    bool success = true;

        //    try
        //    {
        //        using (SqlConnection con = new SqlConnection(theConString))
        //        {
        //            string sql =
        //                $"INSERT INTO tbl{theBookTableName}" +
        //                $"(fk_category_Id, Title, ISBN, Publisher, Description, Price, Placement) " +
        //                $"VALUES('1', '{Title}', '{ISBN}', '{Publisher}')";

        //            con.Open();
        //            using (SqlCommand cmd = new SqlCommand(sql, con))
        //            {
        //                cmd.ExecuteNonQuery();
        //            }
        //        }

        //        return success;
        //    }

        //    catch (Exception)
        //    {
        //        success = false;
        //        return success;
        //    }
        //}
        //public bool AddNewBook(string Title, string ISBN)
        //{

        //    bool success = true;

        //    try
        //    {
        //        using (SqlConnection con = new SqlConnection(theConString))
        //        {
        //            string sql =
        //                $"INSERT INTO tbl{theBookTableName}" +
        //                $"(fk_category_Id, Title, ISBN, Publisher, Description, Price, Placement) " +
        //                $"VALUES('1', '{Title}', '{ISBN}')";

        //            con.Open();
        //            using (SqlCommand cmd = new SqlCommand(sql, con))
        //            {
        //                cmd.ExecuteNonQuery();
        //            }
        //        }

        //        return success;
        //    }

        //    catch (Exception)
        //    {
        //        success = false;
        //        return success;
        //    }
        //}
        //public bool AddNewBook(string Title)
        //{

        //    bool success = true;

        //    try
        //    {
        //        using (SqlConnection con = new SqlConnection(theConString))
        //        {
        //            string sql =
        //                $"INSERT INTO tbl{theBookTableName}" +
        //                $"(fk_category_Id, Title, ISBN, Publisher, Description, Price, Placement) " +
        //                $"VALUES('1', '{Title}')";

        //            con.Open();
        //            using (SqlCommand cmd = new SqlCommand(sql, con))
        //            {
        //                cmd.ExecuteNonQuery();
        //            }
        //        }

        //        return success;
        //    }

        //    catch (Exception)
        //    {
        //        success = false;
        //        return success  ;
        //    }
        //}
        public bool RemoveBook(int id)
        {
            bool success = true;

            try
            {
                using (SqlConnection con = new SqlConnection(theConString))
                {
                    string sql = 
                        $"DELETE FROM tbl{theBookTableName} WHERE Id = {id}";

                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    // Här ska det finnas en till SQL-sträng som lägger till det borttagna objektet i en "ObjectsRemoved"-table.
                    // Här ska det finnas en till SQL-sträng som tar bort objektet i alla tables där den är kopplad.
                }

                return success;
            }

            catch (Exception)
            {
                success = false;
                return success;
            }
        }
    }    
}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WargamesGUI.Models;

namespace UnitTestProject1
{

    class MethodsToTest
    {
        public static int fk_LibraryCard;
        public static string theConStringTest2 = ConfigurationManager.ConnectionStrings[2].ConnectionString;
        public async Task<bool> TestRemoveBookCopy(int id, string reason)
        {
            bool success = true;

            using (SqlConnection con = new SqlConnection(theConStringTest2))
            {
                await con.OpenAsync();

                SqlCommand insertcmd = new SqlCommand("sp_RemoveBookCopy", con);
                insertcmd.CommandType = CommandType.StoredProcedure;

                insertcmd.Parameters.Add("@Copy_Id", SqlDbType.Int).Value = id;
                insertcmd.Parameters.Add("@Reason", SqlDbType.VarChar).Value = reason;
                insertcmd.Parameters.Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.Output;

                await insertcmd.ExecuteNonQueryAsync();

                var retval = (int)insertcmd.Parameters["@returnValue"].Value;
                if (retval == 0)
                {
                    return success;
                }
                else
                {
                    success = false;
                    return success;
                }

            }

        }
        public async Task<bool> TestAddNewBook(int item_id, string title, string ISBN, string publisher, string author,
                                           string description, int price, string placement, string category)
        {
            bool success = true;
            try
            {
                using (SqlConnection con = new SqlConnection(theConStringTest2))
                {
                    await con.OpenAsync();

                    SqlCommand insertcmd = new SqlCommand("sp_AddBook", con);
                    insertcmd.CommandType = CommandType.StoredProcedure;
                    insertcmd.Parameters.Add("@fk_Item_Id", SqlDbType.Int).Value = item_id;
                    insertcmd.Parameters.Add("@Title", SqlDbType.VarChar).Value = title;
                    insertcmd.Parameters.Add("@ISBN", SqlDbType.VarChar).Value = ISBN;
                    insertcmd.Parameters.Add("@Publisher", SqlDbType.VarChar).Value = publisher;
                    insertcmd.Parameters.Add("@Author", SqlDbType.VarChar).Value = author;
                    insertcmd.Parameters.Add("@Description", SqlDbType.VarChar).Value = description;
                    insertcmd.Parameters.Add("@Price", SqlDbType.Int).Value = price;
                    insertcmd.Parameters.Add("@Placement", SqlDbType.VarChar).Value = placement;
                    insertcmd.Parameters.Add("@category", SqlDbType.VarChar).Value = category;
                    await insertcmd.ExecuteNonQueryAsync();

                    return success;
                }
            }
            catch
            {
                success = false;
                return success;
            }


        }
        public int TestSignIn(string username, string password)
        {
            var user = new User();
           
            SqlConnection Connection = new SqlConnection(theConStringTest2);
            Connection.Open();
            string query = $"SELECT fk_PrivilegeLevel FROM tblUser WHERE Username = '{username}' AND Password = HASHBYTES('SHA1','{password}')";

            string query2 = $"SELECT fk_PrivilegeLevel, Username, fk_LibraryCard, Password, CardNumber FROM tblUser LEFT JOIN tblLibraryCard ON fk_LibraryCard = LibraryCard_Id WHERE Username = '{username}' AND Password = HASHBYTES('SHA1','{password}')";

            using (SqlCommand command = new SqlCommand(query2, Connection))
            {

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        user.fk_PrivilegeLevel = Convert.ToInt32(reader["fk_PrivilegeLevel"]);
                        if (user.fk_PrivilegeLevel == 3)
                        {
                            fk_LibraryCard = Convert.ToInt32(reader["fk_LibraryCard"]);
                            return user.fk_PrivilegeLevel;
                        }
                        else
                        {
                            return user.fk_PrivilegeLevel;
                        }

                    }
                }
            }

            return user.fk_PrivilegeLevel;
        }
    }
}

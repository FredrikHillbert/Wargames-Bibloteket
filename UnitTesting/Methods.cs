using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WargamesGUI.Services;


namespace UnitTesting
{
    public class Methods
    {
        public static bool SignInTest(string username, string password, int privilegeLevel)
        {
            var user = new User();

            SqlConnection Connection = new SqlConnection(theConStringTest);
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
                        if (user.fk_PrivilegeLevel == 3 || user.fk_PrivilegeLevel == 1 || user.fk_PrivilegeLevel == 2)
                        {
                            fk_LibraryCard = Convert.ToInt32(reader["fk_LibraryCard"]);
                            return true;
                        }
                        else
                        {
                            return false;
                        }

                    }
                }
            }

            


        }

        private static void AddPrivilegeLevel(int privilege)
        {
            throw new NotImplementedException();
        }

        private static void AddPassword(string password)
        {
            throw new NotImplementedException();
        }

        private static bool CheckIfAllLetter(string input)
        {
            if (string.IsNullOrWhiteSpace(input)
                || !input.All(Char.IsLetter)
                || input.Length < 2 || input.Length > 25)
            {
                return false;
            }
            return true;
        }
    }
}



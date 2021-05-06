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
        public static bool AddNewUserReal(string username, string password, int privilegeLevel)
        {
            bool usernameAnswer = CheckIfAllLetter(username);
            //bool passwordAnswer = AddPassword();
            //bool privilegeLevelAnswer = AddPrivilegeLevel();
            bool canAddNewUser = true;
            try
            {
                using (SqlConnection con = new SqlConnection(DbHandler.theConString))
                {
                    string sql = $"INSERT INTO {DbHandler.theUserTableName}(Username, Password, fk_PrivilegeLevel) VALUES('{username}',HASHBYTES('SHA1','{password}'), '{privilegeLevel}')";

                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
                canAddNewUser = true;
                return canAddNewUser;
            }
            catch (Exception)
            {

                canAddNewUser = false;
                return canAddNewUser;
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



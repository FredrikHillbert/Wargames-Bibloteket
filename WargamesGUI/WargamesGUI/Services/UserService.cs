using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using WargamesGUI.Models;
using System.Linq;
using WargamesGUI.Views;
using System.Threading.Tasks;

namespace WargamesGUI.Services
{
    public class UserService : DbHandler
    {

        public List<User> ReadUserListFromDb()
        {
            List<User> listOfUsers = new List<User>();
            using (SqlConnection con = new SqlConnection(theConString))
            {
                con.Open();
                using (var commad = new SqlCommand(queryForUserListPage, con))
                {
                    using (var reader = commad.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var users = new User();

                            
                            users.First_Name = reader["First_Name"].ToString();
                            users.Last_Name = reader["Last_Name"].ToString();                          
                            users.Username = reader["Username"].ToString();
                            users.fk_PrivilegeLevel = Convert.ToInt32(reader["fk_PrivilegeLevel"]);
                            
                            listOfUsers.Add(users);
                        }
                    }
                }
                return listOfUsers;

            }
        }
        /// <summary>
        /// Adderar en ny användare till table T0100_USER. Måste skickas med en string med namnet på personen som ska läggas till.
        /// </summary>
        /// <param name="fullName"></param>
        /// <returns>Retunerar en bool som är true om det gick att lägga till användaren eller false ifall det inte gick att lägga till användaren.</returns>
        public bool AddNewUser(string username, string password, int privilegeLevel)
        {
            //username = addUser.userbox.Text;
            //password = addUser.passbox.Text;
            bool canAddNewUser = true;
            try
            {
                using (SqlConnection con = new SqlConnection(theConString))
                {
                    string sql = $"INSERT INTO {theUserTableName}(Username, Password, fk_PrivilegeLevel) VALUES('{username}','{password}', '{privilegeLevel}')";
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
        public bool DeleteUserListFromDb(int userId)
        {
            bool isWorking;
            try
            {
                using (SqlConnection con = new SqlConnection(theConString))
                {
                    con.Open();
                    SqlCommand deletecmd = new SqlCommand("P0100_DELETE_USER", con);
                    deletecmd.CommandType = CommandType.StoredProcedure;
                    deletecmd.CommandText = "P0100_DELETE_USER";
                    deletecmd.Parameters.Add("@T0100_USER_ID", SqlDbType.Int).Value = userId;
                    deletecmd.ExecuteNonQuery();
                    isWorking = true;
                }

                return isWorking;
            }
            catch (Exception)
            {

                isWorking = false;
                return isWorking;
            }
        }

        public bool ChangeExistingUser(int userId, string firstName, string lastName, string SSN, string newUserName)
        {
            bool canChangeUser = true;
            string query = $"Update dbo.{theUserTableName} SET First_Name = '{firstName}', Last_Name = '{lastName}', SSN = '{SSN}', Username = '{newUserName}' WHERE T0100_USER_ID = {userId}";
            try
            {
                using (SqlConnection con = new SqlConnection(theConString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.ExecuteNonQuery();
                    }

                }
                canChangeUser = true;
                return canChangeUser;
            }
            catch (Exception)
            {
                canChangeUser = false;
                return canChangeUser;
            }

        }
        public int SignIn(string username, string password)
        {
            var user = new User();

            SqlConnection Connection = new SqlConnection(theConString);
            Connection.Open();
            string query = $"SELECT fk_PrivilegeLevel FROM tblUser WHERE Username = '{username}' AND Password = HASHBYTES('SHA1','{password}')";

            using (SqlCommand command = new SqlCommand(query, Connection))
            {
                
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        user.fk_PrivilegeLevel = Convert.ToInt32(reader["fk_PrivilegeLevel"]);
                    }
                }
            }

            return user.fk_PrivilegeLevel;
        }
        public  void Searching(string text)
        {
            SqlConnection connection = new SqlConnection(theConString);
            connection.Open();
            string query = $"SELECT * FROM tbl ";
        }

    }
}














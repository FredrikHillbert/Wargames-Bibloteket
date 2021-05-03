using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using WargamesGUI.Models;
using System.Linq;

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















    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using WargamesGUI.Models;
using System.Linq;
using WargamesGUI.Views;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace WargamesGUI.Services
{
    public class UserService : DbHandler
    {
        public string exceptionMessage;
        public async Task<ObservableCollection<User>> ReadUserListFromDb()
        {
            ObservableCollection<User> obsList = new ObservableCollection<User>();
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
                            users.User_ID = Convert.ToInt32(reader["User_ID"]);

                            obsList.Add(users);
                        }
                    }
                }
                return await Task.FromResult(obsList);

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
                    SqlCommand deletecmd = new SqlCommand("sp_DeleteUser", con);
                    deletecmd.CommandType = CommandType.StoredProcedure;
                    deletecmd.CommandText = "sp_DeleteUser";
                    deletecmd.Parameters.Add("@Id", SqlDbType.Int).Value = userId;
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

        public List<Visitor> ReadVisitorListFromDb()
        {
            List<Visitor> listOfvisitors = new List<Visitor>();
            using (SqlConnection con = new SqlConnection(theConString))
            {
                con.Open();
                using (var commad = new SqlCommand(queryForVisitors, con))
                {
                    using (var reader = commad.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var visitor = new Visitor();


                            visitor.First_Name = reader["First_Name"].ToString();
                            visitor.Last_Name = reader["Last_Name"].ToString();
                            visitor.CardNumber = reader["LibraryCard"].ToString();
                            visitor.PhoneNumber = reader["PhoneNumber"].ToString();
                            visitor.Address = reader["Address"].ToString();

                            listOfvisitors.Add(visitor);
                        }
                    }
                }
                return listOfvisitors;

            }
        }

        public async Task<bool> AddNewVisitor(int privilegeLevel, string First_Name, string Last_Name, string SSN, string Address, string Email, string PhoneNumber, string LibraryCard)
        {
            bool success = true;
            try
            {
                using (SqlConnection con = new SqlConnection(theConString))
                {
                    await con.OpenAsync();
                    SqlCommand insertcmd = new SqlCommand("sp_AddNewVisitor", con);
                    insertcmd.CommandType = CommandType.StoredProcedure;
                    insertcmd.Parameters.Add("@fk_PL", SqlDbType.Int).Value = privilegeLevel;
                    insertcmd.Parameters.Add("@firstName", SqlDbType.VarChar).Value = First_Name;
                    insertcmd.Parameters.Add("@lastName", SqlDbType.VarChar).Value = Last_Name;
                    insertcmd.Parameters.Add("@sSN", SqlDbType.VarChar).Value = SSN;
                    insertcmd.Parameters.Add("@address", SqlDbType.VarChar).Value = Address;
                    insertcmd.Parameters.Add("@email", SqlDbType.VarChar).Value = Email;
                    insertcmd.Parameters.Add("@phoneNumber", SqlDbType.VarChar).Value = PhoneNumber;
                    insertcmd.Parameters.Add("@libraryCard", SqlDbType.VarChar).Value = LibraryCard;
                    await insertcmd.ExecuteNonQueryAsync();
                    return await Task.FromResult(success);
                }
            }

            catch
            {
                success = false;
                return await Task.FromResult(success);
            }

        }
        public async Task<bool> AddNewUser(int privilegeLevel, string First_Name, string Last_Name, string SSN, string Address, string Email, string PhoneNumber, string username, string password)
        {
            bool success = true;
            try
            {
                using (SqlConnection con = new SqlConnection(theConString))
                {
                    await con.OpenAsync();
                    SqlCommand insertcmd = new SqlCommand("sp_AddNewUser", con);
                    insertcmd.CommandType = CommandType.StoredProcedure;
                    insertcmd.Parameters.Add("@fk_PL", SqlDbType.Int).Value = privilegeLevel;
                    insertcmd.Parameters.Add("@firstName", SqlDbType.VarChar).Value = First_Name;
                    insertcmd.Parameters.Add("@lastName", SqlDbType.VarChar).Value = Last_Name;
                    insertcmd.Parameters.Add("@sSN", SqlDbType.VarChar).Value = SSN;
                    insertcmd.Parameters.Add("@address", SqlDbType.VarChar).Value = Address;
                    insertcmd.Parameters.Add("@email", SqlDbType.VarChar).Value = Email;
                    insertcmd.Parameters.Add("@phoneNumber", SqlDbType.VarChar).Value = PhoneNumber;                    
                    insertcmd.Parameters.Add("@username", SqlDbType.VarChar).Value = username;
                    insertcmd.Parameters.Add("@password", SqlDbType.VarChar).Value = password;
                    await insertcmd.ExecuteNonQueryAsync();
                    return await Task.FromResult(success);
                }
            }

            catch (Exception ex)
            {
                exceptionMessage = ex.Message;
                success = false;
                return await Task.FromResult(success);
            }

        }



    }
}















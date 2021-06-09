using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using WargamesGUI.Models;

namespace WargamesGUI.Services
{
    public class CheckFormat
    {
        public static DbHandler handler = new DbHandler();
        public static bool CheckIfAllLetter(string input)
        {
            Regex regex = new Regex(@"([a-z] ?)+[a-z]");
            if (regex.IsMatch(input))
            {
                return true;
            }
            else
            {
                return false;
            }
            //if (!input.All(Char.IsLetter)
            //    || input.Length < 2 || input.Length > 25)
            //{
            //    return false;
            //}
            //return true;
        }

        public static bool IsValidEmail(string email)
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            if (regex.IsMatch(email))
            {

                return true;
            }
            return false;

        }
        public static bool CheckIfAllNumbers(string input)
        {
            if (string.IsNullOrWhiteSpace(input)
                || !input.All(Char.IsDigit)
                || input.Length < 2 || input.Length > 25)
            {
                return false;
            }
            return true;
        }

        public static bool CheckAdress(string adress)
        {
            // Om den matchar är det fel format
            Regex regex = new Regex(@"[^A-Za-z0-9\s*]+");
            if (regex.IsMatch(adress))
            {
                return false;
            }
            return true;
        }

        public static bool CheckIfUserExists(string incomingUser)
        {

            string query = "SELECT Username FROM tblUser";

            List<User> users = new List<User>();

            using (SqlConnection con = new SqlConnection(handler.theConString))
            {
                con.Open();
                using (var command = new SqlCommand(query, con))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var user = new User();

                            user.Username = reader["Username"].ToString();

                            users.Add(user);
                        }
                    }
                }

            }
            var selectIncomingUser = users.FirstOrDefault(x => x.Username == incomingUser);
            if (selectIncomingUser == null)
            {
                return true;
            }
            return false;
        }
    }
}
    


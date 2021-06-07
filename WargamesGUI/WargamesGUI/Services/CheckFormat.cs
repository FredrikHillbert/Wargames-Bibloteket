using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace WargamesGUI.Services
{
    public class CheckFormat
    {
        public static bool CheckIfAllLetter(string input)
        {
            if (string.IsNullOrWhiteSpace(input)
                || !input.All(Char.IsLetter)
                || input.Length < 2 || input.Length > 25)
            {               
                return false;
            }
            return true;
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
    }
}

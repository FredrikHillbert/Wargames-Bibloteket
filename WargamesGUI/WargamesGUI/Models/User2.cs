using System;
using System.Collections.Generic;
using System.Text;

namespace WargamesGUI.Models
{
    public class User2
    {

        public Privilege TypeOfUser { get; set; } 
        public LibraryCard2 LibraryCard { get; set; }
        public int fk_LibraryCard { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public int SSN { get; set; }
        public int fk_PrivilegeLevel { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int User_ID { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public override string ToString() => $"{TypeOfUser.TypeOfUser}";
    }
}

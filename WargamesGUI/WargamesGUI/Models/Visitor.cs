using System;
using System.Collections.Generic;
using System.Text;

namespace WargamesGUI.Models
{
    public class Visitor
    {
        public int Visitor_ID { get; set; }
        public int Borrower_ID { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string SSN { get; set; }
        public int fk_PrivilegeLevel { get; set; }
        public string CardNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}

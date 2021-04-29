using System;
using System.Collections.Generic;
using System.Text;

namespace WargamesGUI.Models
{
    class Visitor
    {
        public int Visitor_ID { get; set; }
        public int Borrower_ID { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public int SSN { get; set; }
        public int fk_PrivilegeLevel { get; set; }
    }
}

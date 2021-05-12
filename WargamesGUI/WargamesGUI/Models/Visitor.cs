using System;
using System.Collections.Generic;
using System.Text;

namespace WargamesGUI.Models
{
    public class Visitor : User
    {
        public int Visitor_ID { get; set; }
        public int Borrower_ID { get; set; }
        public string CardNumber { get; set; }
       

        public override string ToString() => $"{First_Name} {Last_Name}s borrowrd items";
    }
}

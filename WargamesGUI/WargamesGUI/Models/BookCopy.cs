using System;
using System.Collections.Generic;
using System.Text;
using WargamesGUI.Services;
using WargamesGUI.Models;

namespace WargamesGUI.Models
{
    public class BookCopy
    {
        public int Copy_Id { get; set; }
        public int fk_Book_Id { get; set; }
        public int fk_Availability { get; set; }
        public int fk_Condition_Id { get; set; }

        // String reps
        public string Condition { get; set; } // Speglar fk_Condition_Id
        public string Availability { get; set; } // Speglar fk_Availability

        public override string ToString()
        {
            return $"Exemplar ID: {Copy_Id}\nSkick: {Condition}\nStatus: {Availability}\n";
        }
    }
}

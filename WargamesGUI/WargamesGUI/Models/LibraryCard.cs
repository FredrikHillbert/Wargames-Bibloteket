using System;
using System.Collections.Generic;
using System.Text;

namespace WargamesGUI.Models
{
    public class LibraryCard
    {
        public int LibraryCard_Id { get; set; }
        public string CardNumber { get; set; }
        public int fk_Status_Id { get; set; }
        public string Status_Level { get; set; }
    }
}

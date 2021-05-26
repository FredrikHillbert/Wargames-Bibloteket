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
        public int fk_Condition_Id { get; set; }
        public int fk_Book_Id { get; set; }
        public int fk_Availability { get; set; }
    }
}

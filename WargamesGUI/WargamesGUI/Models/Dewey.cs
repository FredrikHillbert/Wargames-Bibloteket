using System;
using System.Collections.Generic;
using System.Text;

namespace WargamesGUI.Models
{
    public class Dewey
    {
        public int SAB { get; set; }
        public int DDK { get; set; }
        public string SubCategory { get; set; }
        public string fk_MainCategory_Id { get; set; }
    }
}

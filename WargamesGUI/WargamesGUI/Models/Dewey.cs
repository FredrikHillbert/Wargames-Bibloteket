using System;
using System.Collections.Generic;
using System.Text;

namespace WargamesGUI.Models
{
    public class Dewey
    {
        public int DeweySub_Id { get; set; }
        public string CategoryName { get; set; }
        public int fk_DeweyMain_Id { get; set; }
    }
}

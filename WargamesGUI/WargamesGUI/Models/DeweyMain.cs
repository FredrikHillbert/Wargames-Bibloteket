using System;
using System.Collections.Generic;
using System.Text;

namespace WargamesGUI.Models
{
    public class DeweyMain
    {
        public int DeweyMain_Id { get; set; }
        public string MainCategoryName { get; set; }

        public override string ToString() => $"{MainCategoryName}";
    }
}

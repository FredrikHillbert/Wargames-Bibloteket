using System;
using System.Collections.Generic;
using System.Text;

namespace WargamesGUI.Models
{
    public class DeweySub
    {
        public int DeweySub_Id { get; set; }
        public string SubCategoryName { get; set; }
        public int fk_DeweyMain_Id { get; set; }
        
        //public override string ToString() => $"{MainCategoryName}{SubCategoryName}";

        
    }
}

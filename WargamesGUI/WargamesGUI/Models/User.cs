using System;
using System.Collections.Generic;
using System.Text;

namespace WargamesGUI.Models
{
    public class User
    {
        public string TypeOfUser { get; set; }
        public override string ToString() => $"{TypeOfUser}";
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using WargamesGUI.Models;
using WargamesGUI.Services;

namespace WargamesGUI.ViewModels
{
    public class BookCopyView
    {
        public string BookTitle { get; set; }
        public string Condition { get; set; }
        public int Copy_Id { get; set; }
    }
}

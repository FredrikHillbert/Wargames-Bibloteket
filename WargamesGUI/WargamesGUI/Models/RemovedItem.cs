using System;
using System.Collections.Generic;
using System.Text;

namespace WargamesGUI.Models
{
    public class RemovedItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Reason { get; set; }
        public string Condition { get; set; }
        public DateTime Date { get; set; }
    }
}

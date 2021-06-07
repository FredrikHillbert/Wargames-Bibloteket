using System;
using System.Collections.Generic;
using System.Text;

namespace WargamesGUI.Models
{
    public class Event
    {
        public int Id { get; set; }
        public int fk_Item_Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateOfEvent { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace WargamesGUI.Models
{
    class Event
    {
        public int Id { get; set; }
        public int fk_category_Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateOfEvent { get; set; }

        public override string ToString() => $"{Title}, {Description}, {DateOfEvent}, {Id}, {fk_category_Id}";
    }
}

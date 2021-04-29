using System;
using System.Collections.Generic;
using System.Text;

namespace WargamesGUI.Models
{
    class Event
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Date { get; set; }
        public int Item_ID { get; set; }

        public override string ToString() => $"{Title},{Description}, {Date}, {Item_ID}";
    }
}

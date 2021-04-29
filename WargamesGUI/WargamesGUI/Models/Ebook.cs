using System;
using System.Collections.Generic;
using System.Text;

namespace WargamesGUI.Models
{
    class Ebook
    {
        public int Item_ID { get; set; }
        public string Category { get; set; }
        public string Title { get; set; }
        public string ISBN { get; set; }
        public string Publisher { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        

        public override string ToString() => $"{Title},{Category}, {ISBN}, {Publisher}, {Description}, {Price},{Item_ID}";

    }
}

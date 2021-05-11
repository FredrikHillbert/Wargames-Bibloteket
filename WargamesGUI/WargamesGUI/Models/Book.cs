using System;
using System.Collections.Generic;
using System.Text;

namespace WargamesGUI.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string TypeOfBook { get; set; }
        public int fk_Item_Id { get; set; }
        public string Title { get; set; }
        public string ISBN { get; set; }
        public string Publisher { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public int? Price { get; set; }
        public string Placement { get; set; }
        public string Author { get; set; }

        public override string ToString() => $"{TypeOfBook}";
        //public override string ToString() => $"{Title}, {fk_Item_Id}, {ISBN}, {Publisher}, {Description}, {Price}, {Placement}, {Id}";

    }
}
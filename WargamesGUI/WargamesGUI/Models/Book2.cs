using System;
using System.Collections.Generic;

namespace WargamesGUI.Models
{
    public class Book2
    {
        public int Id { get; set; }
        public int fk_Item_Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }
        public string Description { get; set; }
        public int? Price { get; set; }
        public string ISBN { get; set; }
        public int Placement { get; set; }
        public int? Available_copies { get; set; }
        public int? InStock { get; set; }
        public Item BookType { get; set; }
        public DeweySub DeweySub { get; set; }
        public DeweyMain DeweyMain { get; set; }
        public List<BookCopy> BookCopies { get; set; }
    }
}
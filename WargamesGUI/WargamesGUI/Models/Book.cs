using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace WargamesGUI.Models
{
    public class Book : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string TypeOfBook { get; set; } // Används i "AddObject" - ej i databasen.
        public int fk_Item_Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Username { get; set; }
        public string Publisher { get; set; }
        public string Description { get; set; }
        public int? Price { get; set; }
        public string ISBN { get; set; }
        public string Placement { get; set; }
        private int _inStock;
        public int InStock 
        {
            get => _inStock;
            set
            {
                _inStock = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(InStock)));
            }   
        }
        public string Category { get; set; } // Används i BookService - ej i databasen.
        public int fk_BookLoanStatus_Id { get; set; }
       
        private string _status;
        public string Status 
        {
            get => _status;
            set
            {
                _status = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Status)));
            }
        }
        public int Loan_Id { get; set; }
        public DateTime? ReturnDate { get; set; }
        public DateTime? ReturnedDate { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public override string ToString() => $"{TypeOfBook}";

    }
}
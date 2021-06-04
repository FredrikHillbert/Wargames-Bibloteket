using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WargamesGUI.DAL;
using WargamesGUI.Models;
using WargamesGUI.Services;

namespace WargamesGUI.ViewModels
{
    public class UpdateBookViewModel : INotifyPropertyChanged
    {
        private BookService2 bookService;
        public int Id { get { return Book.Id; } private set { } }
        public string Title { get { return Book.Title; } set { Book.Title = value; OnPropertyChanged(); } }
        public string Author { get { return Book.Author; } set { Book.Author = value; OnPropertyChanged(); } }
        public string Publisher { get { return Book.Publisher; } set { Book.Publisher = value; OnPropertyChanged(); } }
        public string Description { get { return Book.Description; } set { Book.Description = value; OnPropertyChanged(); } }
        public int InStock { get { return Book.InStock; } set { Book.InStock = value; OnPropertyChanged(); } }
        public int Price { get { return Book.Price; } set { Book.Price = value; OnPropertyChanged(); } }
        public string ISBN { get { return Book.ISBN; } set { Book.ISBN = value; OnPropertyChanged(); } }
        public int Placement { get { return Book.DeweySub.DeweySub_Id; } set { DeweySub.DeweySub_Id = value; OnPropertyChanged(DeweySub.DeweySub_Id.ToString()); } }
        public Item BookType { get { return Book.BookType; } set { Book.BookType = value; OnPropertyChanged(); } }
        public DeweySub DeweySub { get { return Book.DeweySub; } set { Book.DeweySub = value; OnPropertyChanged(); } }
        public DeweyMain DeweyMain { get { return Book.DeweyMain; } set { Book.DeweyMain = value; OnPropertyChanged(); } }
        public List<string> PickerList { get { return new List<string>() { "Bok", "E-bok" }; } set { } }

        public event PropertyChangedEventHandler PropertyChanged;

        private Book2 _Book;
        public Book2 Book { get { return _Book; } set { _Book = value; } }
        public UpdateBookViewModel(Book2 book)
        {
            Book = book;
            bookService = new BookService2();
        }
        void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public async Task<(bool, string)> UpdateBook()
        {
            var result = await bookService.UpdateBook(Book);
            if (result.Item1) return (result.Item1, result.Item2);
            else return (result.Item1, result.Item2);
        }
    }
}

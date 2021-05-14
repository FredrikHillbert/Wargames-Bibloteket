using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WargamesGUI.Models;
using WargamesGUI.Services;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WargamesGUI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VisitorPage : ContentPage
    {
        private ObservableRangeCollection<Book> collection { get; set; } = new ObservableRangeCollection<Book>();
        public Book selectedItem;
        public User user;
        public static AddUserPage addUser = new AddUserPage();
        public static UserService userService;
        public static BookService bookService = new BookService();

        public static DbHandler handler = new DbHandler();
        public VisitorPage()
        {
            InitializeComponent();

        }
        protected override void OnAppearing()
        {

            MainThread.InvokeOnMainThreadAsync(async () => { await LoadBooks(); });
            

        }

        private async Task LoadBooks()
        {
            if (collection != null)
            {
                collection.Clear();
            }

            collection.AddRange(await bookService.GetBooksFromDb());
            listofbooks.ItemsSource = collection;
            listofBorrowedbooks.ItemsSource = await bookService.GetBorrowedBooksFromDb(UserService.fk_LibraryCard);
        }
        private async Task LoadBorrowedBooks()
        {
            if (collection != null)
            {
                collection.Clear();
            }

            collection.AddRange(await bookService.GetBorrowedBooksFromDb(UserService.fk_LibraryCard));
            listofBorrowedbooks.ItemsSource = collection;
        }

        private async void listofbooks_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            selectedItem = (Book)e.Item;

        }

        private async void Loan_Button_Clicked(object sender, EventArgs e)
        {
            
            if (await bookService.LoanBook(selectedItem.Id, UserService.fk_LibraryCard))
            {
                await DisplayAlert("Susscessfull", "Book is added", "OK");
            }
            else
            {
                await DisplayAlert("Error", $"{bookService.exceptionMessage}", "OK");
            }
            

        }

        private void Back_Button_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new MainPage();
        }
    }
}
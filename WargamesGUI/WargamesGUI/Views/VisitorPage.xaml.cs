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
        
        private List<Book> BookCollection { get; set; } = new List<Book>();
        private List<Book> LoanCollection { get; set; } = new List<Book>();
        
        public Book selectedItem;
        public User user;
        public Book itemTapped;
        public static AddUserPage addUser = new AddUserPage();
        public static UserService userService;
        public static BookService bookService = new BookService();
        public static LoanService bookLoanService = new LoanService();

        public static DbHandler handler = new DbHandler();
        public VisitorPage()
        {
            InitializeComponent();
            BindingContext = listofbooks;
        }
        protected override void OnAppearing()
        {

            MainThread.InvokeOnMainThreadAsync(async () => { await LoadBooks(); });


        }

        private async Task LoadBooks()
        {
            try
            {
                if (BookCollection != null || LoanCollection != null)
                {
                    BookCollection.Clear();
                    LoanCollection.Clear();
                }

                BookCollection.AddRange(await bookService.GetBooksFromDb());
                LoanCollection.AddRange(await bookLoanService.GetLoanedBooksFromDb(UserService.fk_LibraryCard));
                
                listofbooks.ItemsSource = BookCollection;
                listofBorrowedbooks.ItemsSource = LoanCollection;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"{ex.Message}", "Ok");
                throw;
            }
        }

        private async void listofbooks_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            selectedItem = (Book)e.Item;

        }

        private async void Loan_Button_Clicked(object sender, EventArgs e)
        {
            if (selectedItem.InStock == 0)
            {
                await DisplayAlert("Error", "Book is not in stock", "OK");

            }
            else if (await bookService.LoanBook(selectedItem.Id, UserService.fk_LibraryCard))
            {
                await DisplayAlert("Susscessfull", "Book is added", "OK");
                await LoadBooks();
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
        private void listofBorrowedbooks_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            itemTapped = (Book)e.Item;
        }
        private async void HandBookBack_Button_Clicked(object sender, EventArgs e)
        {
            //LoanService.LoanedBooks.Remove(itemTapped);
            await bookLoanService.ChangeBookLoanStatus(itemTapped.Loan_Id);
            await DisplayAlert("Succsess", "Your book is handed back", "OK");

            await LoadBooks();
        }

        private void MainSearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            var searchresult = BookCollection.Where(x => x.Title.Contains(MainSearchBar.Text)
                                                      || x.Author.Contains(MainSearchBar.Text)
                                                      || x.ISBN.Contains(MainSearchBar.Text)
                                                      || x.Publisher.Contains(MainSearchBar.Text));
                                                      

            listofbooks.ItemsSource = searchresult;
        }
    }
}
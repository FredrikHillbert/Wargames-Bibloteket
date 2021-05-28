using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WargamesGUI.Models;
using WargamesGUI.Services;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WargamesGUI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ManualReturn : ContentPage
    {
        public static BookService bookService = new BookService();
        public static LoanService loanService = new LoanService();
        private List<Book> LoanCollection { get; set; } = new List<Book>();
        private List<Book> HandledCollection { get; set; } = new List<Book>();
        public static Book selectedBook;
        public ManualReturn()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            try
            {
                MainThread.InvokeOnMainThreadAsync(async () => { await LoadBooks(); });
            }
            catch (Exception ex)
            {
                DisplayAlert("ManualReturnOnAppearing Error", $"Felmeddelande: {ex.Message}", "OK");
            }

        }
        private async Task LoadBooks()
        {
            try
            {
                if (LoanCollection != null || HandledCollection != null)
                {
                    LoanCollection.Clear();
                    HandledCollection.Clear();
                }

                LoanCollection.AddRange(await loanService.GetBorrowedBooksFromDbLibrarian());
                HandledCollection.AddRange(await loanService.GetHandledBooksFromLibrarianDb());

                listOfBooks.ItemsSource = await loanService.GetBorrowedBooksFromDbLibrarian();
                listOfHandledBooks.ItemsSource = await loanService.GetHandledBooksFromLibrarianDb();

            }
            catch (Exception ex)
            {
                await DisplayAlert("LoadBooks", $"Felmeddelande: {ex.Message}", "OK");
            }

        }
        private async void Handled_Clicked(object sender, EventArgs e)
        {

            await loanService.RegisterReturnedBook(selectedBook.Book_Copy);
            await DisplayAlert("Bok skannad!", $"Du skannade {selectedBook.Title}.", "OK");
            await LoadBooks();



        }

        private void listOfBooks_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            selectedBook = (Book)e.Item;
        }

        private async void Refresh_Clicked(object sender, EventArgs e)
        {
            try
            {
                await LoadBooks();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Refresh_Clicked Error", $"Felmeddelande: {ex.Message}", "OK");
            }
        }

        private async void BookReturnSeachBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var searchresult = LoanCollection.Where(x => x.Username.Contains(BookReturnSeachBar.Text)
                  || x.Author.Contains(BookReturnSeachBar.Text)
                  || x.ISBN.Contains(BookReturnSeachBar.Text)
                  || x.Title.Contains(BookReturnSeachBar.Text));

                listOfBooks.ItemsSource = searchresult;
            }
            catch (Exception ex)
            {
                await DisplayAlert("MainSearchBar_TextChanged Error", $"Felmeddelande: {ex.Message}", "OK");
            }
        }

        private async void BookHandledSeachBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var searchresult = HandledCollection.Where(x => x.Placement.Contains(BookHandledSeachBar.Text)
                  || x.Author.Contains(BookHandledSeachBar.Text)
                  || x.ISBN.Contains(BookHandledSeachBar.Text)
                  || x.Title.Contains(BookHandledSeachBar.Text)
                  || x.BookConditionString.Contains(BookHandledSeachBar.Text)
                  || x.Status.Contains(BookHandledSeachBar.Text));

                listOfHandledBooks.ItemsSource = searchresult;
            }
            catch (Exception ex)
            {
                await DisplayAlert("MainSearchBar_TextChanged Error", $"Felmeddelande: {ex.Message}", "OK");
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
           
            try
            {
                await loanService.ReturnBookLoan(selectedBook.Loan_Id);
                await DisplayAlert("Bok skannad!", $"Du skannade {selectedBook.Title}.", "OK");
                await LoadBooks();

            }
            catch (Exception ex)
            {
                await DisplayAlert("Handled_Clicked Error", $"Felmeddelande: {ex.Message}", "OK");
            }
        }
    }
}
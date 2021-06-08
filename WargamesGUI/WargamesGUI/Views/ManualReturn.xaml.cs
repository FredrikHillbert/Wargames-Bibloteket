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
        public static BookService2 bookService2 = new BookService2();
        public static LoanService2 loanService2 = new LoanService2();

        private List<Book> LoanCollection { get; set; } = new List<Book>();
        private List<Book> HandledCollection { get; set; } = new List<Book>();
        public static Book selectedBook;
        public static Book2 selectedBook2;
        public static Book HandledbookSelected;
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

        private void listOfBooks_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            selectedBook = (Book)e.Item;
        }
        private async void listOfHandledBooks_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            HandledbookSelected = (Book)e.Item;
            try
            {
                var selected = await DisplayActionSheet("Vilket skick är boken?", "Godkänn", "Avbryt", null, "Ny", "Som ny", "Väldigt bra", "Bra", "Acceptabel", "Sliten", "Förstörd");

                switch (selected)
                {
                    case "Godkänn":
                        try
                        {
                            if (HandledbookSelected.BookCondition == 7)
                            {
                                string reason = await DisplayPromptAsync("Förstörd", "Anledning", "Ok", "Cancel");
                                await loanService.RegisterReturnedBookDestroyedBook(HandledbookSelected.Book_Copy, HandledbookSelected.Loan_Id, reason);
                                await DisplayAlert("Förstörd bok!", $"Du lade till {HandledbookSelected.Title} som förstörd i historiken.", "OK");
                                await LoadBooks();
                            }
                            else
                            {
                                await loanService.RegisterReturnedBook(HandledbookSelected.Book_Copy, HandledbookSelected.Loan_Id);
                                await DisplayAlert("Bok återlämad!", $"Du lade till {HandledbookSelected.Title} som tillgänglig i biblioteket.", "OK");
                                await LoadBooks();
                            }


                        }
                        catch (Exception ex)
                        {
                            await DisplayAlert("Godkänn Error", $"Felmeddelande: {ex.Message}", "OK");
                        }
                        break;
                    case "Ny":
                        await bookService.UpdateBookCopy(HandledbookSelected.Book_Copy, 1);
                        await DisplayAlert("Lyckades", "Skicket ändrades till 'Ny'", "Ok");
                        await LoadBooks();
                        break;
                    case "Som ny":
                        await bookService.UpdateBookCopy(HandledbookSelected.Book_Copy, 2);
                        await DisplayAlert("Lyckades", "Skicket ändrades till 'Som ny'", "Ok");
                        await LoadBooks();
                        break;
                    case "Väldigt bra":
                        await bookService.UpdateBookCopy(HandledbookSelected.Book_Copy, 3);
                        await DisplayAlert("Lyckades", "Skicket ändrades till 'Väldigt bra'", "Ok");
                        await LoadBooks();
                        break;
                    case "Bra":
                        await bookService.UpdateBookCopy(HandledbookSelected.Book_Copy, 4);
                        await DisplayAlert("Lyckades", "Skicket ändrades till 'Bra'", "Ok");
                        await LoadBooks();
                        break;
                    case "Acceptabel":
                        await bookService.UpdateBookCopy(HandledbookSelected.Book_Copy, 5);
                        await DisplayAlert("Lyckades", "Skicket ändrades till 'Acceptabel'", "Ok");
                        await LoadBooks();
                        break;
                    case "Sliten":
                        await bookService.UpdateBookCopy(HandledbookSelected.Book_Copy, 6);
                        await DisplayAlert("Lyckades", "Skicket ändrades till 'Sliten'", "Ok");
                        await LoadBooks();
                        break;
                    case "Förstörd":
                        await bookService.UpdateBookCopy(HandledbookSelected.Book_Copy, 7);
                        await DisplayAlert("Lyckades", "Skicket ändrades till 'Förstörd'", "Ok");
                        await LoadBooks();
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("listOfHandledBooks_ItemTapped Error", $"{ex.Message}", "Ok");
            }

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
                await DisplayAlert("BookReturnSeachBar_TextChanged Error", $"Felmeddelande: {ex.Message}", "OK");
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
                await DisplayAlert("BookHandledSeachBar_TextChanged Error", $"Felmeddelande: {ex.Message}", "OK");
            }
        }

        private async void Skanna_Clicked(object sender, EventArgs e)
        {

            try
            {
                await loanService.ReturnBookLoan(selectedBook.Loan_Id);
                await DisplayAlert("Bok skannad!", $"Du skannade {selectedBook.Title}.", "OK");
                await LoadBooks();

            }
            catch (Exception ex)
            {
                await DisplayAlert("Skanna_Clicked Error", $"Felmeddelande: {ex.Message}", "OK");
            }
        }

    }
}
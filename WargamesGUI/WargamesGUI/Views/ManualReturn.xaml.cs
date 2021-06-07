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
        private List<Book> LoanCollection { get; set; } = new List<Book>();
        private List<Book> HandledCollection { get; set; } = new List<Book>();
        public static Book selectedBook;
        public static Book HandledbookSelected;
        public List<Book2> bookList;
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
            await loanService.RegisterReturnedBook(HandledbookSelected.Book_Copy, HandledbookSelected.Loan_Id);
            await DisplayAlert("Bok skannad!", $"Du skannade {HandledbookSelected.Title}.", "OK");
            await LoadBooks();
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
                var selected = await DisplayActionSheet("Vilket skick är boken?", "Avbryt", null, "Ny", "Som ny", "Väldigt bra", "Bra", "Acceptabel", "Sliten", "Förstörd");

                switch (selected)
                {
                    case "Ny":
                        await bookService.UpdateBookCopy(HandledbookSelected.Book_Copy, 1);
                        await DisplayAlert("Lyckades", "Skicket ändrades till Ny", "Ok");
                        await LoadBooks();
                        break;
                    case "Som ny":
                        await bookService.UpdateBookCopy(HandledbookSelected.Book_Copy, 2);
                        await DisplayAlert("Lyckades", "Skicket ändrades till Som ny", "Ok");
                        await LoadBooks();
                        break;
                    case "Väldigt bra":
                        await bookService.UpdateBookCopy(HandledbookSelected.Book_Copy, 3);
                        await DisplayAlert("Lyckades", "Skicket ändrades till Väldigt bra", "Ok");
                        await LoadBooks();
                        break;
                    case "Bra":
                        await bookService.UpdateBookCopy(HandledbookSelected.Book_Copy, 4);
                        await DisplayAlert("Lyckades", "Skicket ändrades till Bra", "Ok");
                        await LoadBooks();
                        break;
                    case "Acceptabel":
                        await bookService.UpdateBookCopy(HandledbookSelected.Book_Copy, 5);
                        await DisplayAlert("Lyckades", "Skicket ändrades till Acceptabel", "Ok");
                        await LoadBooks();
                        break;
                    case "Sliten":
                        await bookService.UpdateBookCopy(HandledbookSelected.Book_Copy, 6);
                        await DisplayAlert("Lyckades", "Skicket ändrades till Sliten", "Ok");
                        await LoadBooks();
                        break;
                    case "Förstörd":
                        await bookService.UpdateBookCopy(HandledbookSelected.Book_Copy, 7);
                        await LoadBooks();
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"{ex.Message}", "Ok");
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

        private void BookReturnSeachBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            //try
            //{
            //    var searchresult = LoanCollection.Where(x => x.Username.Contains(BookReturnSeachBar.Text)
            //      || x.Author.Contains(BookReturnSeachBar.Text)
            //      || x.ISBN.Contains(BookReturnSeachBar.Text)
            //      || x.Title.Contains(BookReturnSeachBar.Text));

            //    listOfBooks.ItemsSource = searchresult;
            //}
            //catch (Exception ex)
            //{
            //    await DisplayAlert("MainSearchBar_TextChanged Error", $"Felmeddelande: {ex.Message}", "OK");
            //}

            AutoCompleteListView.IsVisible = true;
            AutoCompleteListView.BeginRefresh();
            try
            {
                var titleResult = LoanCollection.Select(x => x.Title).Where(x => x != null && x.ToUpper().Contains(BookReturnSeachBar.Text.ToUpper())).ToList();
                var publisherResult = LoanCollection.Select(x => x.Publisher).Where(x => x != null && x.ToUpper().Contains(BookReturnSeachBar.Text.ToUpper())).ToList();
                var authorResult = LoanCollection.Select(x => x.Author).Where(x => x != null && x.ToUpper().Contains(BookReturnSeachBar.Text.ToUpper())).ToList();
                var isbnResult = LoanCollection.Select(x => x.ISBN).Where(x => x != null && x.ToUpper().Contains(BookReturnSeachBar.Text.ToUpper())).ToList();
                //var subCategoryResult = LoanCollection.Select(x => x.DeweySub.SubCategoryName).Where(x => x != null && x.ToUpper().Contains(BookReturnSeachBar.Text.ToUpper())).ToList();
                //var mainCategoryResult = LoanCollection.Select(x => x.DeweyMain.MainCategoryName).Where(x => x != null && x.ToString().ToUpper().Contains(BookReturnSeachBar.Text.ToUpper())).ToList();

                //var allResults = titleResult.Concat(publisherResult).Concat(authorResult).Concat(isbnResult).Concat(subCategoryResult).Concat(mainCategoryResult).Distinct().ToList();
                var allResults = titleResult.Concat(publisherResult).Concat(authorResult).Concat(isbnResult).Distinct().ToList();

                if (string.IsNullOrWhiteSpace(BookReturnSeachBar.Text))
                {
                    AutoCompleteListView.IsVisible = false;
                }
                else
                {
                    AutoCompleteListView.ItemsSource = allResults;

                }


            }
            catch (Exception ex)
            {
                //AutoCompleteList.IsVisible = false;
            }

            //while (AutoCompleteListView.IsVisible)
            //{
            //    AutoCompleteListView.IsVisible = true;
            //    AutoCompleteListView.BeginRefresh();
            //    Username.IsVisible = false;
            //    Placement.IsVisible = false;
            //    try
            //    {
            //        var titleResult = LoanCollection.Select(x => x.Title).Where(x => x != null && x.ToUpper().Contains(BookReturnSeachBar.Text.ToUpper())).ToList();
            //        var publisherResult = LoanCollection.Select(x => x.Publisher).Where(x => x != null && x.ToUpper().Contains(BookReturnSeachBar.Text.ToUpper())).ToList();
            //        var authorResult = LoanCollection.Select(x => x.Author).Where(x => x != null && x.ToUpper().Contains(BookReturnSeachBar.Text.ToUpper())).ToList();
            //        var isbnResult = LoanCollection.Select(x => x.ISBN).Where(x => x != null && x.ToUpper().Contains(BookReturnSeachBar.Text.ToUpper())).ToList();
            //        //var subCategoryResult = LoanCollection.Select(x => x.DeweySub.SubCategoryName).Where(x => x != null && x.ToUpper().Contains(BookReturnSeachBar.Text.ToUpper())).ToList();
            //        //var mainCategoryResult = LoanCollection.Select(x => x.DeweyMain.MainCategoryName).Where(x => x != null && x.ToString().ToUpper().Contains(BookReturnSeachBar.Text.ToUpper())).ToList();

            //        //var allResults = titleResult.Concat(publisherResult).Concat(authorResult).Concat(isbnResult).Concat(subCategoryResult).Concat(mainCategoryResult).Distinct().ToList();
            //        var allResults = titleResult.Concat(publisherResult).Concat(authorResult).Concat(isbnResult).Distinct().ToList();

            //        if (string.IsNullOrWhiteSpace(BookReturnSeachBar.Text))
            //        {
            //            AutoCompleteListView.IsVisible = false;
            //        }
            //        else
            //        {
            //            AutoCompleteListView.ItemsSource = allResults;

            //        }


            //    }
            //    catch (Exception ex)
            //    {
            //        //AutoCompleteList.IsVisible = false;
            //    }

            //}


        }

        private async void BookHandledSeachBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            //try
            //{
            //    var searchresult = HandledCollection.Where(x => x.Placement.Contains(BookHandledSeachBar.Text)
            //      || x.Author.Contains(BookHandledSeachBar.Text)
            //      || x.ISBN.Contains(BookHandledSeachBar.Text)
            //      || x.Title.Contains(BookHandledSeachBar.Text)
            //      || x.BookConditionString.Contains(BookHandledSeachBar.Text)
            //      || x.Status.Contains(BookHandledSeachBar.Text));

            //    listOfHandledBooks.ItemsSource = searchresult;
            //}
            //catch (Exception ex)
            //{
            //    await DisplayAlert("BookHandledSeachBar_TextChanged Error", $"Felmeddelande: {ex.Message}", "OK");
            //}
            AutoCompleteListView.IsVisible = true;
            AutoCompleteListView.BeginRefresh();

            try
            {
                var titleResult = bookList.Select(x => x.Title).Where(x => x != null && x.ToUpper().Contains(BookReturnSeachBar.Text.ToUpper())).ToList();
                var publisherResult = bookList.Select(x => x.Publisher).Where(x => x != null && x.ToUpper().Contains(BookReturnSeachBar.Text.ToUpper())).ToList();
                var authorResult = bookList.Select(x => x.Author).Where(x => x != null && x.ToUpper().Contains(BookReturnSeachBar.Text.ToUpper())).ToList();
                var isbnResult = bookList.Select(x => x.ISBN).Where(x => x != null && x.ToUpper().Contains(BookReturnSeachBar.Text.ToUpper())).ToList();
                var subCategoryResult = bookList.Select(x => x.DeweySub.SubCategoryName).Where(x => x != null && x.ToUpper().Contains(BookReturnSeachBar.Text.ToUpper())).ToList();
                var mainCategoryResult = bookList.Select(x => x.DeweyMain.MainCategoryName).Where(x => x != null && x.ToString().ToUpper().Contains(BookReturnSeachBar.Text.ToUpper())).ToList();

                var allResults = titleResult.Concat(publisherResult).Concat(authorResult).Concat(isbnResult).Concat(subCategoryResult).Concat(mainCategoryResult).Distinct().ToList();

                if (string.IsNullOrWhiteSpace(BookReturnSeachBar.Text))
                {
                    AutoCompleteListView.IsVisible = false;
                }
                else
                {
                    AutoCompleteListView.ItemsSource = allResults;

                }

            }
            catch (Exception ex)
            {
                //AutoCompleteList.IsVisible = false;
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

        private void AutoCompleteListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            string listsd = e.Item as string;
            BookReturnSeachBar.Text = listsd;
            AutoCompleteListView.IsVisible = false;
            ((ListView)sender).SelectedItem = null;
            SearchBar_Clicked(sender, e);
        }

        private async void SearchBar_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(BookReturnSeachBar.Text))
                {
                    await DisplayAlert("Misslyckades", "Du måste skriva något", "OK");
                }
                else
                {
                    SearchValuePage.GetValues(BookReturnSeachBar.Text);
                    App.Current.MainPage = new SearchValuePage();
                }

            }
            catch (Exception ex)
            {
                await DisplayAlert("Misslyckades", $"{ex.Message}", "Ok");
            }
        }
    }
}
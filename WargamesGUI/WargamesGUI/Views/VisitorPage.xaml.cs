using System;
using System.Collections.Generic;
using System.Linq;
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

        private List<Book2> BookCollection;
        private List<BookLoan2> LoanCollection;

        public Book2 selectedBook;
        public User2 loggedInUser;
        public BookLoan2 bookLoanTapped;
        public static AddUserPage addUser = new AddUserPage();
        public static BookService2 bookservice2 = new BookService2();
        public static LoanService2 loanService2 = new LoanService2();

        public static DbHandler handler = new DbHandler();
        public VisitorPage()
        {
            InitializeComponent();
            BookCollection = new List<Book2>();
            LoanCollection = new List<BookLoan2>();
        }
        public VisitorPage(User2 user)
        {
            loggedInUser = user;
            InitializeComponent();
            BookCollection = new List<Book2>();
            LoanCollection = new List<BookLoan2>();
        }
        public VisitorPage(Book2 book, User2 user)
        {
            loggedInUser = user;
            InitializeComponent();
            BookCollection = new List<Book2>();
            LoanCollection = new List<BookLoan2>();
            TryLoanBook(book);
        }
        protected override void OnAppearing()
        {
            try
            {
                MainThread.InvokeOnMainThreadAsync(async () => { await LoadAll(); });
            }
            catch (Exception ex)
            {
                DisplayAlert("VisitorPageOnAppearing Error", $"{ex.Message}", "OK");
            }
        }
        private async Task LoadAll()
        {
            await LoadBooks();
            await LoadBookLoans();
        }
        private async Task LoadBooks()
        {
            try
            {
                if (BookCollection != null || LoanCollection != null)
                {
                    BookCollection.Clear();
                }

                BookCollection = await bookservice2.GetAllBooks();
                listofbooks.ItemsSource = BookCollection;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Misslyckades", $"{ex.Message}", "OK");
            }
        }
        private async Task LoadBookLoans()
        {
            try
            {
                LoanCollection = await loanService2.GetAllBookLoans(loggedInUser);
                listofBorrowedbooks.ItemsSource = LoanCollection;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Misslyckades", $"{ex.Message}", "OK");
            }
        }
        public async Task<bool> AreYouSureChoice(Book2 book)
        {
            var choice = await DisplayActionSheet($"Vill du låna boken: {book.Title}?", null, null, "Ja", "Nej");
            switch (choice)
            {
                case "Ja":
                    return true;
                case "Nej":
                    return false;
                default:
                    return false;
            }
        }
        private void Back_Button_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new MainPage();
        }
        private void listofBorrowedbooks_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            bookLoanTapped = (BookLoan2)e.Item;
        }
        private async void HandBookBack_Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                await loanService2.ChangeBookLoanStatusUser(bookLoanTapped);
                await DisplayAlert("Lyckades", "Din bok är tillbakalämnad", "OK");
                await LoadBooks();
            }
            catch (Exception ex)
            {
                await DisplayAlert("HandBookBack_Button_Clicked Error", $"Felmeddelande: {ex.Message}", "OK");
            }
        }
        private async void MainSearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var searchresult = BookCollection.Where(x => x.BookType.TypeOfItem != null && x.BookType.TypeOfItem.ToUpper().Contains(MainSearchBar.Text.ToUpper()) ||
                                      x.Placement.ToString().ToUpper().Contains(MainSearchBar.Text.ToUpper()) ||
                                      x.Author != null && x.Author.ToUpper().Contains(MainSearchBar.Text.ToUpper()) ||
                                      x.Publisher != null && x.Publisher.ToUpper().Contains(MainSearchBar.Text.ToUpper()) ||
                                      x.Title != null && x.Title.ToUpper().Contains(MainSearchBar.Text.ToUpper()) ||
                                      x.Description != null && x.Description.ToUpper().Contains(MainSearchBar.Text.ToUpper()) ||
                                      x.DeweyMain != null && x.DeweyMain.MainCategoryName.ToString().ToUpper().Contains(MainSearchBar.Text.ToUpper()) ||
                                      x.DeweySub != null && x.DeweySub.SubCategoryName.ToString().ToUpper().Contains(MainSearchBar.Text.ToUpper())
                                      ).Select(x => x);
                listofbooks.ItemsSource = searchresult;
            }
            catch (Exception ex)
            {
                await DisplayAlert("MainSearchBar_TextChanged Error", $"Felmeddelande: {ex.Message}", "OK");
            }
        }
        private async void listofbooks_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                selectedBook = (Book2)e.SelectedItem;
                var answer = await DisplayActionSheet("Välj ett alternativ: ", "Avbryt", null, "Detaljer", "Låna Boken");

                switch (answer)
                {
                    case "Detaljer":
                        await DisplayAlert("Beskrivning", $"{selectedBook.Title} \n\n{selectedBook.Description}", "OK");
                        break;
                    case "Låna Boken":
                        TryLoanBook(selectedBook);
                        break;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Misslyckades", $"{ex.Message}", "OK");
            }
        }
        private async void TryLoanBook(Book2 book)
        {
            try
            {
                if (book.Available_copies <= 0)
                {
                    await DisplayAlert("Misslyckades", "Boken är inte tillgänglig", "OK");
                    await MainThread.InvokeOnMainThreadAsync(async () => { await LoadAll(); });
                }
                else if (await AreYouSureChoice(book))
                {
                    var result = await loanService2.LoanBook(book, loggedInUser);
                    switch (result.Item2)
                    {
                        case 0:
                            await DisplayAlert("Lyckades", $"Boken {book.Title} är tilllagd", "OK");
                            await MainThread.InvokeOnMainThreadAsync(async () => { await LoadAll(); });
                            break;
                        case 1:
                            await DisplayAlert("Misslyckades", "Du har en oinlämnad bok, lämna tillbaka den och försök igen", "OK");
                            await MainThread.InvokeOnMainThreadAsync(async () => { await LoadAll(); });
                            break;
                        case 2:
                            await DisplayAlert("Misslyckades", "Du har tappat bort böcker. Kontakta biblioteket för att lösa problemet", "OK");
                            await MainThread.InvokeOnMainThreadAsync(async () => { await LoadAll(); });
                            break;
                        case 3:
                            await DisplayAlert("Misslyckdes", "Du har stulit böcker. Kontakta biblioteket för att lösa problemet", "OK");
                            await MainThread.InvokeOnMainThreadAsync(async () => { await LoadAll(); });
                            break;
                        default:
                            await DisplayAlert("Misslyckades", "Okänt fel. Kontakta biblioteket för att lösa problemet", "OK");
                            await MainThread.InvokeOnMainThreadAsync(async () => { await LoadAll(); });
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Misslyckades", $"{ex.Message}", "OK");
            }
        }
    }
}
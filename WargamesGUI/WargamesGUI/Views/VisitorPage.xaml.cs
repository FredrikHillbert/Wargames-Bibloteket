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

        private List<Book2> BookCollection { get; set; } = new List<Book2>();
        private List<Book2> LoanCollection { get; set; } = new List<Book2>();

        public Book2 selectedItem;
        public User2 loggedInUser;
        public Book2 itemTapped;
        public static AddUserPage addUser = new AddUserPage();
        public BookService2 bookservice2 = new BookService2();
        public LoanService2 loanService2 = new LoanService2();
        public static LoanService bookLoanService = new LoanService();

        public static DbHandler handler = new DbHandler();
        public VisitorPage()
        {
            InitializeComponent();
        }
        public VisitorPage(User2 user)
        {
            loggedInUser = user;
            InitializeComponent();
        }
        public VisitorPage(Book2 book, User2 user)
        {
            loggedInUser = user;
            selectedItem = book;
            InitializeComponent();
            Loan_Button_Clicked(book, new EventArgs());
        }
        protected override void OnAppearing()
        {
            try
            {
                MainThread.InvokeOnMainThreadAsync(async () => { await LoadBooks(); });
            }
            catch (Exception ex)
            {
                DisplayAlert("VisitorPageOnAppearing Error", $"{ex.Message}", "OK");
            }
        }
        private async Task LoadBooks()
        {
            try
            {
                if (BookCollection != null || LoanCollection != null)
                {
                    BookCollection.Clear();
                }

                BookCollection.AddRange(await bookservice2.GetAllBooks());

                listofbooks.ItemsSource = await bookservice2.GetAllBooks();
                listofBorrowedbooks.ItemsSource = await loanService2.GetAllBookLoans(loggedInUser);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Misslyckades", $"{ex.Message}", "OK");
            }
        }

        private async void listofbooks_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                selectedItem = (Book2)e.Item;

                var answer = await DisplayActionSheet("Välj ett alternativ: ", "Avbryt", null, "Detaljer", "Låna Boken");

                switch (answer)
                {
                    case "Detaljer":
                        await DisplayAlert("Beskrivning", $"{selectedItem.Description}", "OK");
                        break;
                    case "Låna Boken":
                        if (selectedItem.Available_copies <= 0)
                        {
                            await DisplayAlert("Misslyckades", "Boken är inte tillgänglig", "OK");

                        }
                        else
                        {
                            switch (await bookLoanService.LoanBook(selectedItem.Id, UserService.fk_LibraryCard))
                            {
                                case 0:
                                    await DisplayAlert("Lyckades", "Boken är tilllagd", "OK");
                                    await LoadBooks();
                                    break;
                                case 1:
                                    await DisplayAlert("Misslyckades", "Du har en oinlämnad bok, lämna tillbaka den och försök igen", "OK");
                                    await LoadBooks();
                                    break;
                                case 2:
                                    await DisplayAlert("Misslyckades", "Du har tappat bort böcker. Kontakta biblioteket för att lösa problemet", "OK");
                                    await LoadBooks();
                                    break;
                                case 3:
                                    await DisplayAlert("Misslyckdes", "Du har stulit böcker. Kontakta biblioteket för att lösa problemet", "OK");
                                    await LoadBooks();
                                    break;
                                default:
                                    await DisplayAlert("Misslyckades", "Okänt fel. Kontakta biblioteket för att lösa problemet", "OK");
                                    await LoadBooks();
                                    break;
                            }
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Misslyckades", $"{ex.Message}", "OK");
             
            }
            
        }
        private async void Loan_Button_Clicked(object sender, EventArgs e)
        {
            if (selectedItem.InStock == 0)
            {
                await DisplayAlert("Misslyckades", "Boken är inte tillgänglig", "OK");
            }
            else
            {
                try
                {
                    switch (await loanService2.LoanBook(selectedItem, loggedInUser.LibraryCard))
                    {
                        case 0:
                            await DisplayAlert("Lyckades", "Boken är tilllagd", "OK");
                            await LoadBooks();
                            break;
                        case 1:
                            await DisplayAlert("Misslyckades", "Du har en oinlämnad bok, lämna tillbaka den och försök igen", "OK");
                            await LoadBooks();
                            break;
                        case 2:
                            await DisplayAlert("Misslyckades", "Du har tappat bort böcker. Kontakta biblioteket för att lösa problemet", "OK");
                            await LoadBooks();
                            break;
                        case 3:
                            await DisplayAlert("Misslyckdes", "Du har stulit böcker. Kontakta biblioteket för att lösa problemet", "OK");
                            await LoadBooks();
                            break;
                        default:
                            await DisplayAlert("Misslyckades", "Okänt fel. Kontakta biblioteket för att lösa problemet", "OK");
                            await LoadBooks();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Loan_Button_Clicked Error", $"Felmeddelande: {ex.Message}", "OK");
                }
            }
        }

        private void Back_Button_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new MainPage();
        }
        private void listofBorrowedbooks_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            itemTapped = (Book2)e.Item;
        }
        private async void HandBookBack_Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                //await loanService2.ChangeBookLoanStatus(itemTapped);
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
    }
}
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

                BookCollection.AddRange(await bookService.GetBooksFromDb());

                listofbooks.ItemsSource = await bookService.GetBooksFromDb();
                listofBorrowedbooks.ItemsSource = await bookLoanService.GetBorrowedBooksFromDb(UserService.fk_LibraryCard);
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
                selectedItem = (Book)e.Item;

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
            itemTapped = (Book)e.Item;
        }
        private async void HandBookBack_Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                await bookLoanService.ChangeBookLoanStatus(itemTapped.Loan_Id);
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
                var searchresult = BookCollection.Where(x => x.Title.Contains(MainSearchBar.Text)
                  || x.Author.Contains(MainSearchBar.Text)
                  || x.ISBN.Contains(MainSearchBar.Text)
                  || x.Publisher.Contains(MainSearchBar.Text)
                  || x.Category.Contains(MainSearchBar.Text));

                listofbooks.ItemsSource = searchresult;
            }
            catch (Exception ex)
            {
                await DisplayAlert("MainSearchBar_TextChanged Error", $"Felmeddelande: {ex.Message}", "OK");
            }

        }
    }
}
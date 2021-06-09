using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WargamesGUI.Models;
using WargamesGUI.Services;
using WargamesGUI.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace WargamesGUI
{

    public partial class MainPage : ContentPage
    {
        public static UserService service = new UserService();
        public static User2 user;
        public static BookService2 bookService2 = new BookService2();
        public static LoanService2 loanService2 = new LoanService2();
        public static UserService2 userService2 = new UserService2();

        public List<Book2> bookList;
        public List<User2> userList;
        public MainPage()
        {
            InitializeComponent();
        }
        protected async override void OnAppearing()
        {
            await MainThread.InvokeOnMainThreadAsync(async () => { await LoadBooks(); });
            await MainThread.InvokeOnMainThreadAsync(async () => { await LoadUsers(); });
        }
        public async Task<List<Book2>> LoadBooks()
        {
            bookList = await bookService2.GetAllBooks();
            return bookList ?? null;
        }
        public async Task<List<User2>> LoadUsers()
        {
            userList = await userService2.ReadAllUsersFromDbAsync();
            return userList ?? null;
        }

       
        private async void SignIn_Button_Clicked(object sender, EventArgs e)
        {
            if (Entryusername.Text != null & Entrypassword.Text != null)
            {
                try
                {
                    var activeUser = userList.Select(x => x).Where(x => x.Username == Entryusername.Text).FirstOrDefault();
                    if (activeUser != null)
                    {
                        switch (await userService2.SignInUser(activeUser, Entrypassword.Text))
                        {
                            case 0: await DisplayAlert("Error", "Kontrollera användarnamn och lösenord", "OK");
                                break;

                            case 1:
                                Entryusername.Text = string.Empty;
                                Entrypassword.Text = string.Empty;
                                await DisplayAlert("Lyckades", "Du loggar nu in som administratör", "OK");
                                App.Current.MainPage = new FlyoutAdminPage();
                                break;
                            case 2:
                                Entryusername.Text = string.Empty;
                                Entrypassword.Text = string.Empty;
                                await DisplayAlert("Lyckades", "Du loggar nu in som Bibliotekarie", "OK");
                                App.Current.MainPage = new FlyoutLibrarianPage();
                                break;
                            case 3:
                                Entryusername.Text = string.Empty;
                                Entrypassword.Text = string.Empty;
                                await DisplayAlert("Lyckades", "Du loggar nu in som Besökare", "OK");
                                App.Current.MainPage = new VisitorPage(activeUser);
                                break;
                        }

                    }
                    else { await DisplayAlert("Error", "Kontrollera användarnamn och lösenord", "OK"); }
                
                }
                catch (Exception ex)
                {
                    await DisplayAlert("SignIn_Button_Clicked Error", $"Felmeddelande: {ex.Message}", "Ok");
                }
            }
        }
        private async void SearchBar_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(SearchBar.Text))
                {
                    await DisplayAlert("Misslyckades", "Du måste skriva något", "OK");
                }
                else
                {
                    SearchValuePage.GetValues(SearchBar.Text);
                    App.Current.MainPage = new SearchValuePage();
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Misslyckades", $"{ex.Message}", "Ok");
            }
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            AutoCompleteList.IsVisible = true;
            AutoCompleteList.BeginRefresh();

            try
            {
                var result = bookList.FilterSearchBookList(SearchBar.Text);

                if (string.IsNullOrWhiteSpace(SearchBar.Text))
                {
                    AutoCompleteList.IsVisible = false;
                }
                else
                {
                    AutoCompleteList.ItemsSource = result;
                }

            }
            catch (Exception ex)
            {
                DisplayAlert("Misslyckades", $"{ex.Message}", "Ok");
            }
        }

        private void AutoCompleteList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            string listsd = e.Item as string;
            SearchBar.Text = listsd;
            AutoCompleteList.IsVisible = false;
            ((ListView)sender).SelectedItem = null;
            SearchBar_Clicked(sender, e);
        }

        private void Button_Clicked(object sender, EventArgs e)
        {

        }
    }
}

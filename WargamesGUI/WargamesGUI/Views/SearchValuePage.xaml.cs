using System;
using System.Threading.Tasks;
using WargamesGUI.Models;
using WargamesGUI.Services;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.Generic;
using System.Linq;

namespace WargamesGUI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchValuePage : ContentPage
    {
        public static BookService bookService = new BookService();
        public static BookService2 bookService2 = new BookService2();
        public static UserService userService = new UserService();
        public static UserService2 userService2 = new UserService2();
        public static string text;
        public Book2 selecteditem;
        public static string cardnumber;

        public List<User2> userList;
        public SearchValuePage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            try
            {
                MainThread.InvokeOnMainThreadAsync(async () => { await SearchValues(); });
                MainThread.InvokeOnMainThreadAsync(async () => { await LoadUsers(); });
            }
            catch (Exception ex)
            {
                DisplayAlert("SearchValuePageOnAppearing Error", $"Felmeddelande: {ex.Message}", "OK");
            }

        }
        public async Task<List<User2>> LoadUsers()
        {
            userList = await userService2.ReadAllUsersFromDbAsync();
            return userList ?? null;
        }
        private async Task SearchValues()
        {

            listOfBook.ItemsSource = await bookService2.SearchBook(text);

            //try
            //{
            //    listOfBook.ItemsSource = await bookService.Searching(GetValues(text));
            //}
            //catch (Exception ex)
            //{
            //    await DisplayAlert("SearchValues Error", $"Felmeddelande: {ex.Message}", "OK");
            //}
        }

        private void Back_Button_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new MainPage();
        }
        public static string GetValues(string value)
        {
            text = value;
            return text;
        }

        private async void listOfBook_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            selecteditem = (Book2)e.Item;

            var answer = await DisplayActionSheet("Välj ett alternativ: ", "Avbryt", null, "Detaljer", "Logga in för att låna boken");

            switch (answer)
            {
                case "Detaljer":
                    await DisplayAlert("Beskrivning", $"{selecteditem.Description}", "OK");
                    break;
                case "Logga in för att låna boken":
                    ShowLoginPopup();
                    break;
            }          
        }
        private async void SignIn_Button_Clicked(object sender, EventArgs e)
        {
            if (EntryUsername.Text != "" & EntryPassword.Text != "")
            {
                HideLoginPopup();

                try
                {
                    switch (userService.SignIn(EntryUsername.Text, EntryPassword.Text))
                    {
                        case 1:
                            EntryUsername.Text = string.Empty;
                            EntryPassword.Text = string.Empty;
                            MainStackLayout.IsVisible = false;
                            await DisplayAlert("Lyckades", "Du loggar nu in som administratör", "OK");
                            App.Current.MainPage = new FlyoutAdminPage();
                            break;
                        case 2:
                            EntryUsername.Text = string.Empty;
                            EntryPassword.Text = string.Empty;
                            MainStackLayout.IsVisible = false;
                            await DisplayAlert("Lyckades", "Du loggar nu in som Bibliotekarie", "OK");
                            App.Current.MainPage = new FlyoutLibrarianPage();
                            break;
                        case 3:
                            var loggedInAs = userList.Select(x => x).Where(x => x.Username == EntryUsername.Text).FirstOrDefault();
                            EntryUsername.Text = string.Empty;
                            EntryPassword.Text = string.Empty;
                            MainStackLayout.IsVisible = false;
                            await DisplayAlert("Lyckades", "Du loggar nu in som Besökare", "OK");
                            App.Current.MainPage = new VisitorPage(selecteditem, loggedInAs);
                            break;
                        default:
                            EntryUsername.Text = string.Empty;
                            EntryPassword.Text = string.Empty;
                            MainStackLayout.IsVisible = false;
                            await DisplayAlert("Misslyckades", "Kotrollera användarnamn och lösenord", "Ok");
                            MainStackLayout.IsVisible = true;
                            HideLoginPopup();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("SignIn_Button_Clicked Error", $"Felmeddelande: {ex.Message}", "Ok");
                }
            }
        }
        public void ShowLoginPopup()
        {
            MainFrame.IsVisible = false;
            DisplayLoginFrame.IsVisible = true;
            popupStackLayout.IsVisible = true;
            UsernameLabel.IsVisible = true;
            PasswordLabel.IsVisible = true;
            EntryUsername.IsVisible = true;
            EntryPassword.IsVisible = true;
            LoginButton.IsVisible = true;
            RunningImage.IsVisible = true;
        }
        public void HideLoginPopup()
        {
            MainFrame.IsVisible = true;
            DisplayLoginFrame.IsVisible = false;
            popupStackLayout.IsVisible = false;
            UsernameLabel.IsVisible = false;
            PasswordLabel.IsVisible = false;
            EntryUsername.IsVisible = false;
            EntryPassword.IsVisible = false;
            LoginButton.IsVisible = false;
            RunningImage.IsVisible = false;
        }
    }
}
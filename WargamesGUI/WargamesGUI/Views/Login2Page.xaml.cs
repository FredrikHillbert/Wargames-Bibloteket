using System;
using WargamesGUI.Models;
using WargamesGUI.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WargamesGUI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login2Page : ContentPage
    {
        public static UserService service = new UserService();
        public static User user;
        public Login2Page()
        {
            InitializeComponent();
        }

        private async void Login_Button_Clicked(object sender, EventArgs e)
        {
            if (Entryusername.Text != "" & Entrypassword.Text != "")
            {
                try
                {

                    switch (service.SignIn(Entryusername.Text, Entrypassword.Text))
                    {
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
                            App.Current.MainPage = new VisitorPage();
                            break;
                        default:
                            await DisplayAlert("Misslyckades", "Kotrollera användarnamn och lösenord", "Ok");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("SignIn_Button_Clicked Error", $"Felmeddelande: {ex.Message}", "Ok");
                }
            }
        }

        private void Button_Clicked(object sender, EventArgs e)
        {

        }

        private void Back_Button_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new SearchValuePage();
        }
    }
}
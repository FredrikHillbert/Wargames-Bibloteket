using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WargamesGUI.Models;
using WargamesGUI.Views;
using Xamarin.Forms;
using WargamesGUI.Services;
using static WargamesGUI.AddUserPage;

namespace WargamesGUI
{

    public partial class MainPage : ContentPage
    {
        public static UserService service = new UserService();
        public static User user;
        public MainPage()
        {
            InitializeComponent();

            Entrypassword.Completed += (sender, e) => Entrypassword_Completed(sender, e);
        }

        private async void SignIn_Button_Clicked(object sender, EventArgs e)
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

        private async void Entrypassword_Completed(object sender, EventArgs e)
        {
            SignIn_Button_Clicked(sender, e);
        }
    }
}

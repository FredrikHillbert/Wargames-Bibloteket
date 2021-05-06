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

        public MainPage()
        {
            InitializeComponent();
        }

        private async void SignIn_Button_Clicked(object sender, EventArgs e)
        {
            Exception exception = null;
            try
            {

                switch (service.SignIn(Entryusername.Text, Entrypassword.Text))
                {
                    case 1:
                        await DisplayAlert("Successful", "You are now logging in as Admin", "OK");
                        App.Current.MainPage = new FlyoutAdminPage();
                        break;
                    case 2:
                        await DisplayAlert("Successful", "You are now logging in as Librarian", "OK");
                        App.Current.MainPage = new FlyoutLibrarianPage();
                        break;
                    case 3:
                        await DisplayAlert("Successful", "You are now logging in as Visitor", "OK");
                        //App.Current.MainPage = new FlyoutVisitorPage();
                        break;
                    default:
                        await DisplayAlert("Error", "Please check if username and password are correct", "Ok");
                        break;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                await DisplayAlert("Error", $"{exception.Message}", "Ok");
                throw;
            }

        }

        private async void SearchBar_Clicked(object sender, EventArgs e)
        {
            Exception exception = null;
            try
            {
                SearchFlyoutPage.GetValues(SearchBar.Text);
                App.Current.MainPage = new SearchFlyoutPage();

            }
            catch (Exception ex)
            {
                exception = ex;
                await DisplayAlert("Error", $"{exception.Message}", "Ok");
                throw;
            }
        }


    }
}

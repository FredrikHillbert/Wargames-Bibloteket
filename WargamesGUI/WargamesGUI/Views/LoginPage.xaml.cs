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
        
        public MainPage()
        {
            InitializeComponent();

            
        }

        private async void SignIn_Button_Clicked(object sender, EventArgs e)
        {
            await SignIn();

        }

        private async Task SignIn()
        {
            var user = new User();
            Exception exception = null;
            try
            {
                SqlConnection Connection = new SqlConnection(DbHandler.theConString);
                Connection.Open();
                string query = $"SELECT fk_PrivilegeLevel FROM tblUser WHERE Username = '{Entryusername.Text}' AND Password ='{Entrypassword.Text}' ";

                using (SqlCommand command = new SqlCommand(query, Connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            user.fk_PrivilegeLevel = Convert.ToInt32(reader["fk_PrivilegeLevel"]);
                        }
                    }
                }

                switch (user.fk_PrivilegeLevel)
                {
                    case 1:
                        await DisplayAlert("Successful", "You are now logged in as Admin", "OK");
                        App.Current.MainPage = new FlyoutAdminPage();
                        break;
                    case 2:
                        await DisplayAlert("Successful", "You are now logged in as Librarian", "OK");
                        //App.Current.MainPage = new FlyoutLibrarianPage();
                        break;
                    case 3:
                        await DisplayAlert("Successful", "You are now logged in as Visitor", "OK");
                        //App.Current.MainPage = new FlyoutLibrarianPage();
                        break;
                    default:
                        await DisplayAlert("Error", "Please check if username and password are correct", "Ok");
                        break;
                }

            }
            catch (Exception ex)
            {
                exception = ex;
                await DisplayAlert("Error", $"{exception}", "Ok");
            }
        }
    }
}

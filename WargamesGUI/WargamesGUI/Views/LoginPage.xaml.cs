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

namespace WargamesGUI
{
    
    public partial class MainPage : ContentPage
    {
        User user;

        private const string ConnectionString = "Server=tcp:wargameslibrary.database.windows.net,1433;Initial Catalog=Wargames Library;Persist Security Info=False;User ID=adminwargames;Password=Admin123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        public MainPage()
        {
            InitializeComponent();

            
        }

        private async void SignIn_Button_Clicked(object sender, EventArgs e)
        {
            Exception exception = null;
            try
            {
                SqlConnection Connection = new SqlConnection(ConnectionString);
                Connection.Open();
                string query = $"SELECT fk_PrivilegeLevel FROM tbl.User";
                //string query = $"SELECT * FROM dbo.tblUser WHERE Username = '{Entryusername.Text}' AND Password ='{Entrypassword.Text}'";
                SqlDataAdapter sda = new SqlDataAdapter(query, Connection);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (Entryusername.Text == "Admin" && Entrypassword.Text == "Admin123")
                {
                    await DisplayAlert("Successful", "You are now logged in as Admin", "OK");
                    App.Current.MainPage = new FlyoutAdminPage();
                }
                else
                    await DisplayAlert("Error", "Please check if username and password are correct", "Ok");
                
            }
            catch (Exception ex)
            {
                exception = ex;
                await DisplayAlert("Error", $"{exception}", "Ok");
            }
            


        }
    }
}

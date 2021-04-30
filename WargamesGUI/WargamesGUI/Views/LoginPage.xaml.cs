using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WargamesGUI.Views;
using Xamarin.Forms;

namespace WargamesGUI
{
    public partial class MainPage : ContentPage
    {
        private const string ConnectionString = "Server=tcp:wargameslibrary.database.windows.net,1433;Initial Catalog=Wargames Library;Persist Security Info=False;User ID=adminwargames;Password=Admin123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        public MainPage()
        {
            InitializeComponent();
            // Testing testing
        }

        private async void SignIn_Button_Clicked(object sender, EventArgs e)
        {
            SqlConnection Connection = new SqlConnection(ConnectionString);
            Connection.Open();
            //string query = $"SELECT * FROM tbl.User WHERE Username = '" + Entryusername.Text.Trim() + "' and Password = '" + Entrypassword.Text.Trim() + "'";
            string query = $"SELECT * FROM dbo.tblUser WHERE Username = '{Entryusername.Text}' and Password ='{Entrypassword.Text}'";
            SqlDataAdapter sda = new SqlDataAdapter(query, Connection);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count != 0)
            {
                await DisplayAlert("Successful", "You are now logged in", "OK");
                App.Current.MainPage = new FlyoutAdminPage();
            }
            else
                await DisplayAlert("Error", "Incorrect username or password", "Ok");

        }
    }
}

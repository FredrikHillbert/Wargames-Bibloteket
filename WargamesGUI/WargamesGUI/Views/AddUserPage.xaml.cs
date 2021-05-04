using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WargamesGUI.Models;
using WargamesGUI.Services;
using WargamesGUI.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WargamesGUI
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddUserPage : ContentPage
    {
        public static AddUserPage addUser = new AddUserPage();

        public static UserService userService = new UserService();
        public static DbHandler handler = new DbHandler();
        private int privilegeLevel;
        public AddUserPage()
        {
            InitializeComponent();

        }

        protected override void OnAppearing()
        {


            MainThread.InvokeOnMainThreadAsync(async () => { await LoadUserTbl(); });


        }

        private async Task LoadUserTbl()
        {             
            listOfUsers.ItemsSource = userService.ReadUserListFromDb();

        }









        public class DbHandler
        {
            
            // Alla olika connectionstrings som vi behöver.
            private const string theConString = "Server=tcp:wargameslibrary.database.windows.net,1433;Initial Catalog=Wargames Library;Persist Security Info=False;User ID=adminwargames;Password=Admin123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            private const string theUserTableName = "User";
            private const string theStatusTableName = "";
            private const string theTransactionTypeTableName = "";
            private const string theTransactionTableName = "";
            private const string theProjectTableName = "";


            //Alla olika SQL-satser som vi behöver.
            private string queryForUserListPage = "";
            private string queryForTransactionListPage = "";
            private string queryForUserAndTransactionListPage = "";
            private string queryForProjectCatalogue = "";

           

            //-----------------------------------UserListPage Metoder.

            /// <summary>
            /// Hämtar all data som är lagrad under T0100_USER table i Xenon.
            /// </summary>
            /// <returns> Retunerar all data i form av datatypen DataTable. </returns>
            public DataTable ReadUserListFromDb()
            {

                using (SqlConnection con = new SqlConnection(theConString))
                {

                    con.Open();
                    SqlCommand com = new SqlCommand(queryForUserListPage, con);
                    SqlDataAdapter sda = new SqlDataAdapter(com);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);

                    return dt;

                }
            }
            /// <summary>
            /// Adderar en ny användare till table T0100_USER. Måste skickas med en string med namnet på personen som ska läggas till.
            /// </summary>
            /// <param name="fullName"></param>
            /// <returns>Retunerar en bool som är true om det gick att lägga till användaren eller false ifall det inte gick att lägga till användaren.</returns>
            public bool AddNewUser(string username, string password, int privilegeLevel)
            {
                //username = addUser.userbox.Text;
                //password = addUser.passbox.Text;
                bool canAddNewUser = true;
                try
                {
                    using (SqlConnection con = new SqlConnection(theConString))
                    {
                        string sql = $"INSERT INTO tbl{theUserTableName}(Username, Password, fk_PrivilegeLevel) VALUES('{username}','{password}', '{privilegeLevel}')";
                        con.Open();
                        using (SqlCommand cmd = new SqlCommand(sql, con))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }
                    canAddNewUser = true;
                    return canAddNewUser;
                }
                catch (Exception)
                {

                    canAddNewUser = false;
                    return canAddNewUser;
                }

            }


        }

        private async void Register_User_Clicked(object sender, EventArgs e)
        {
            var username = userbox.Text;
            var password = passbox.Text;
           
            var b = handler.AddNewUser(username, password, privilegeLevel);
            if (b == true) await DisplayAlert("Sucess", "You added a user!", "OK");
            else await DisplayAlert("Error!", "Could not add user", "OK");
        }

        private void picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedItem = (User)picker.SelectedItem;
            privilegeLevel =  selectedItem.fk_PrivilegeLevel;

        }
    }
}
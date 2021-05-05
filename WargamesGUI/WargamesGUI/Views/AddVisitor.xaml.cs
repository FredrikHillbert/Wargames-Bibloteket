using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WargamesGUI.Models;
using WargamesGUI.Services;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WargamesGUI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddVisitor : ContentPage
    {
        public User selectedItem;
        public static AddUserPage addUser = new AddUserPage();
        public static UserService userService = new UserService();
        public static DbHandler handler = new DbHandler();
        private int privilegeLevel;
        public AddVisitor()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            MainThread.InvokeOnMainThreadAsync(async () => { await ReadVisitorListFromDb(); });

        }

        private async Task ReadVisitorListFromDb()
        {
            listOfVisitors.ItemsSource = userService.ReadVisitorListFromDb();
        }

        public bool AddNewVisitor(string firstName, string lastName, string Ssn, int privilegeLevel)
        {
            bool canAddNewVisitor = true;
            try
            {
                using (SqlConnection con = new SqlConnection(DbHandler.theConString))
                {
                    string sql = $"INSERT INTO {DbHandler.theUserTableName}(First_Name, Last_Name, SSN, fk_PrivilegeLevel) VALUES('{firstName}', '{lastName}', '{Ssn}', '{privilegeLevel}')";

                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
                canAddNewVisitor = true;
                return canAddNewVisitor;
            }
            catch (Exception)
            {

                canAddNewVisitor = false;
                return canAddNewVisitor;
            }

        }

        private async void AddVisitor_Button_Clicked(object sender, EventArgs e)
        {
            var firstName = EntryFirstName.Text;
            var lastName = EntryLastName.Text;
            var ssnNumber = EntrySsnNumber.Text;
            privilegeLevel = 3;

            var b = AddNewVisitor(firstName, lastName, ssnNumber, privilegeLevel);
            if (b == true)
            {
                await DisplayAlert("Klart!", "Du la till en användare!", "OK");
                await ReadVisitorListFromDb();


            }
            else await DisplayAlert("Fel!", "Kunde inte lägga till besökare!", "OK");
        }

        private async void DeleteVisitor_Clicked(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(DbHandler.theConString))
                {
                    string sql =
                        $"DELETE FROM {DbHandler.theUserTableName} WHERE First_Name = '{selectedItem.First_Name}'";

                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
                await DisplayAlert("Klart!", "Du tog bort en besökare!", "OK");
                await ReadVisitorListFromDb();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Fel!", $"Anledning: {ex.Message}", "OK");
            }
        }

        private void listOfVisitors_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            selectedItem = (User)listOfVisitors.SelectedItem;
        }
    }
}
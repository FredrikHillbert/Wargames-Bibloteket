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

        private async void AddVisitor_Button_Clicked(object sender, EventArgs e)
        {
            privilegeLevel = 3;

            var b = await userService.AddNewVisitor(privilegeLevel, EntryFirstName.Text, EntryLastName.Text, EntrySsnNumber.Text, EntryAdress.Text, EntryEmail.Text, EntryPhoneNumber.Text, EntryCardNumber.Text);

            if (b)
            {
                await DisplayAlert("Success!", "You added a visitor!", "OK");
                await ReadVisitorListFromDb();


            }
            else await DisplayAlert("Error!", "Could not add visitor!", "OK");
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
                await DisplayAlert("Success!", "You removed a visitor!", "OK");
                await ReadVisitorListFromDb();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error!", $"Reason: {ex.Message}", "OK");
            }
        }

        private void listOfVisitors_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            selectedItem = (User)listOfVisitors.SelectedItem;
        }
    }
}
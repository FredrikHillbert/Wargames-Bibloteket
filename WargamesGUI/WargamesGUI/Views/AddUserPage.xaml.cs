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
        public User selectedItem;
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
            listOfUsers.ItemsSource = await userService.ReadUserListFromDb();

        }
  
            public bool AddNewUser(string username, string password, int privilegeLevel)
            {
                //username = addUser.userbox.Text;
                //password = addUser.passbox.Text;
                bool canAddNewUser = true;
                try
                {
                    using (SqlConnection con = new SqlConnection(DbHandler.theConString))
                    {
                        string sql = $"INSERT INTO {DbHandler.theUserTableName}(Username, Password, fk_PrivilegeLevel) VALUES('{username}','{password}', '{privilegeLevel}')";
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
        private async void Register_User_Clicked(object sender, EventArgs e)
        {
            var username = userbox.Text;
            var password = passbox.Text;
            

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || privilegeLevel == 0)
            {
                await DisplayAlert("Error!", "Username/password/privilege is incorrect", "OK");
            }
            else
            {
                var b = AddNewUser(username, password, privilegeLevel);
                if (b == true)
                {
                    await DisplayAlert("Sucess", "You added a user!", "OK");
                    await LoadUserTbl();


                }
                else await DisplayAlert("Error!", "Could not add user", "OK");
            }

        }

        private void picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedItem = (User)picker.SelectedItem;
            privilegeLevel =  selectedItem.fk_PrivilegeLevel;

        }
        private async void Delete_User(object sender, EventArgs e)
        {
            
            try
            {
                using (SqlConnection con = new SqlConnection(DbHandler.theConString))
                {
                    string sql =
                        $"DELETE FROM {DbHandler.theUserTableName} WHERE User_ID = '{selectedItem.User_ID}'";

                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
                await DisplayAlert("Sucess", "You deleted a user", "OK");
                await LoadUserTbl();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error!", $"Reason: {ex.Message}", "OK");
            }


        }
        private void listOfUsers_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            selectedItem = (User)listOfUsers.SelectedItem;
        }

    }
}
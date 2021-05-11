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


        private async void Register_User_Clicked(object sender, EventArgs e)
        {
            if (privilegeLevel == 0)
            {
                await DisplayAlert("TypeNotSelected", "Select a type of user.", "OK");
            }

            else if (string.IsNullOrEmpty(firstnamebox.Text) || CheckFormat.CheckIfAllLetter(firstnamebox.Text) == false)
            {
                await DisplayAlert("InvalidFormat", "Firstname is empty or format is not allowed.", "OK");
            }

            else if (string.IsNullOrEmpty(lastnamebox.Text) || CheckFormat.CheckIfAllLetter(lastnamebox.Text) == false)
            {
                await DisplayAlert("InvalidLastname", "Lastname is empty or format is not allowed.", "OK");
            }
            else if (string.IsNullOrEmpty(addressbox.Text) || CheckFormat.CheckAdress(addressbox.Text) == false)
            {
                await DisplayAlert("InvalidAddress", "Address is empty or format is not allowed.", "OK");
            }
            else if (string.IsNullOrEmpty(emailbox.Text) || CheckFormat.IsValidEmail(emailbox.Text) == false)
            {
                await DisplayAlert("EmailEmpty", "Enter a valid email.", "OK");
            }
            else if (string.IsNullOrEmpty(phonebox.Text) || CheckFormat.CheckIfAllNumbers(phonebox.Text) == false)
            {
                await DisplayAlert("NumberEmpty", "Enter a valid phonenumber.", "OK");
            }
            else if (string.IsNullOrEmpty(userbox.Text) || CheckFormat.CheckIfAllLetter(userbox.Text) == false)
            {
                await DisplayAlert("InvalidFormat", "Username is empty or format is not allowed.", "OK");
            }
            else if (string.IsNullOrEmpty(passbox.Text))
            {
                await DisplayAlert("PassEmpty", "Password entry is empty.", "OK");
            }

            else
            {
                try
                {
                    
                    if (userService.AddNewUser(userbox.Text, passbox.Text, privilegeLevel,
                                       firstnamebox.Text, lastnamebox.Text, emailbox.Text, phonebox.Text, addressbox.Text))
                    {
                        await DisplayAlert("Sucess", "You added a user!", "OK");
                        await LoadUserTbl();
                    }
                }
                catch (Exception)
                {
                    await DisplayAlert("Error!", $"Reason {userService.exceptionMessage}", "OK");
                    
                }            
            }

        }

        private async void Delete_User_Clicked(object sender, EventArgs e)
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
        private void picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedItem = (User)picker.SelectedItem;
            privilegeLevel = selectedItem.fk_PrivilegeLevel;

        }
        private async Task LoadUserTbl()
        {
            listOfUsers.ItemsSource = await userService.ReadUserListFromDb();

        }

    }
}
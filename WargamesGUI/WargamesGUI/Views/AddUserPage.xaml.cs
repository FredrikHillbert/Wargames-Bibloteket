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
//using Windows.UI.Xaml.Controls;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WargamesGUI
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddUserPage : ContentPage
    {
        public User selectedItem;
        public Color color;
        public static AddUserPage addUser = new AddUserPage();
        public static UserService userService = new UserService();
        public static DbHandler handler = new DbHandler();
        public static LoanService loanService = new LoanService();
        public int StatusID;
        public string statusString;
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

                    if (privilegeLevel == 3)
                    {
                        await userService.AddNewUser(privilegeLevel, firstnamebox.Text, lastnamebox.Text, ssnbox.Text, addressbox.Text, emailbox.Text, phonebox.Text, userbox.Text, passbox.Text);

                        await DisplayAlert("Sucess", "You added a visitor!", "OK");
                        await LoadUserTbl();
                    }
                    else if (await userService.AddNewUser(privilegeLevel, firstnamebox.Text, lastnamebox.Text, ssnbox.Text, addressbox.Text, emailbox.Text, phonebox.Text, userbox.Text, passbox.Text))
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

            switch (privilegeLevel)
            {
                case 1:
                    firstnameframe.IsVisible = true;
                    firstnamebox.IsVisible = true;
                    lastnameframe.IsVisible = true;
                    lastnamebox.IsVisible = true;
                    adressframe.IsVisible = true;
                    addressbox.IsVisible = true;
                    ssnframe.IsVisible = true;
                    ssnbox.IsVisible = true;
                    emailframe.IsVisible = true;
                    emailbox.IsVisible = true;
                    phoneframe.IsVisible = true;
                    phonebox.IsVisible = true;
                    userframe.IsVisible = true;
                    userbox.IsVisible = true;
                    passframe.IsVisible = true;
                    passbox.IsVisible = true;
                    break;
                case 2:
                    firstnameframe.IsVisible = true;
                    firstnamebox.IsVisible = true;
                    lastnameframe.IsVisible = true;
                    lastnamebox.IsVisible = true;
                    adressframe.IsVisible = true;
                    addressbox.IsVisible = true;
                    ssnframe.IsVisible = true;
                    ssnbox.IsVisible = true;
                    emailframe.IsVisible = true;
                    emailbox.IsVisible = true;
                    phoneframe.IsVisible = true;
                    phonebox.IsVisible = true;
                    userframe.IsVisible = true;
                    userbox.IsVisible = true;
                    passframe.IsVisible = true;
                    passbox.IsVisible = true;
                    break;
                case 3:
                    firstnameframe.IsVisible = true;
                    firstnamebox.IsVisible = true;
                    lastnameframe.IsVisible = true;
                    lastnamebox.IsVisible = true;
                    adressframe.IsVisible = true;
                    addressbox.IsVisible = true;
                    ssnframe.IsVisible = true;
                    ssnbox.IsVisible = true;
                    emailframe.IsVisible = true;
                    emailbox.IsVisible = true;
                    phoneframe.IsVisible = true;
                    phonebox.IsVisible = true;
                    userframe.IsVisible = true;
                    userbox.IsVisible = true;
                    passframe.IsVisible = true;
                    passbox.IsVisible = true;
                    break;

            }

        }
        private async Task LoadUserTbl()
        {
            try
            {
                listOfUsers.ItemsSource = await userService.ReadUserListFromDb();
            }
            catch (Exception ex)
            {
                await DisplayAlert("LoadError", $"Reason for error: {ex.Message}", "OK");
            }


        }

        private async void listOfUsers_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            selectedItem = (User)listOfUsers.SelectedItem;          

            if (selectedItem.fk_PrivilegeLevel == 3)
            {
                var choice = await DisplayActionSheet($"Gör ett val för användarnamn {selectedItem.Username}: ", "Avbryt", null, "Detaljer för användare", "Lägg till bibliotekskort", "Ändra status för bibliotekskort");
                try
                {
                    var statusnumber = await userService.GetStatusForLibraryCardFromDb(selectedItem.Cardnumber);
                    switch (statusnumber)
                    {
                        case 1:
                            statusString = "Active";
                            break;
                        case 2:
                            statusString = "Delayed books";
                            break;
                        case 3:
                            statusString = "Lost books";
                            break;
                        case 4:
                            statusString = "Theft";
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"{ex.Message}", "OK");
                    
                }                         
                switch (choice)
                {
                    case "Detaljer för användare":
                        await DisplayActionSheet($"Välj detalj för användare {selectedItem.Username}: ", "Avbryt", null, "Se status för bibliotekskort");
                        await DisplayAlert("Status för bibliotekskort:", $"Användaren {selectedItem.Username} har statusen: '{statusString}' för sitt bibliotekskort", "OK");
                        break;

                    case "Lägg till bibliotekskort":
                        bool success = await loanService.ManualAddLibraryCard(selectedItem.User_ID);
                        if (success)
                        {
                            await DisplayAlert("Lyckades!", $"Bibliotekskort tillagt för {selectedItem.Username}", "OK");
                        }
                        else
                        {
                            await DisplayAlert("Misslyckades!", $"Bibliotekskort kunde inte läggas till för {selectedItem.Username}", "OK");
                        }
                        break;

                    case "Ändra status för bibliotekskort":
                        var libraryCardDetails = await DisplayActionSheet($"Ny status för bibliotekskort med användarnamn {selectedItem.Username}: ", "Avbryt", null, "Aktivt", "Försenade böcker", "Borttappade böcker", "Stöld");

                        switch (libraryCardDetails)
                        {
                            case "Aktivt":
                                if (!await loanService.ChangeCardStatus(1, selectedItem.Cardnumber))
                                {
                                    await DisplayAlert("Misslyckades!", $"Status ändrades inte för bibliotekskortet.", "OK");
                                }
                                else
                                {
                                    await loanService.ChangeCardStatus(1, selectedItem.Cardnumber);
                                    await DisplayAlert("Lyckades!", $"Status för bibliotekskortet ändrat till: Aktivt.", "OK");
                                }
                                break;

                            case "Försenade böcker":
                                if (!await loanService.ChangeCardStatus(2, selectedItem.Cardnumber))
                                {
                                    await DisplayAlert("Misslyckades!", $"Status ändrades inte för bibliotekskortet.", "OK");
                                }
                                else
                                {
                                    await loanService.ChangeCardStatus(2, selectedItem.Cardnumber);
                                    await DisplayAlert("Lyckades!", $"Status för bibliotekskortet ändrat till: Försenade böcker.", "OK");
                                }
                                break;

                            case "Borttappade böcker":
                                if (!await loanService.ChangeCardStatus(3, selectedItem.Cardnumber))
                                {
                                    await DisplayAlert("Misslyckades!", $"Status ändrades inte för bibliotekskortet.", "OK");
                                }
                                else
                                {
                                    await loanService.ChangeCardStatus(3, selectedItem.Cardnumber);
                                    await DisplayAlert("Lyckades!", $"Status för bibliotekskortet ändrat till: Lost books.", "OK");
                                }
                                break;

                            case "Stöld":
                                if (!await loanService.ChangeCardStatus(4, selectedItem.Cardnumber))
                                {
                                    await DisplayAlert("Misslyckades!", $"Status ändrades inte för bibliotekskortet.", "OK");
                                }
                                else
                                {
                                    await loanService.ChangeCardStatus(4, selectedItem.Cardnumber);
                                    await DisplayAlert("Lyckades!", $"Status för bibliotekskortet ändrat till: Theft.", "OK");
                                }
                                break;
                        }
                        break;

                    default:
                        break;
                }
            }
            else
            {
                await DisplayActionSheet($"Gör ett val för användarnamn {selectedItem.Username}: ", "Avbryt", null, "Detaljer för användare");
            }
        }
    }
}   
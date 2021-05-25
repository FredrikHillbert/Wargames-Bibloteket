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
        private List<User> UserCollection { get; set; } = new List<User>();
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
            try
            {
                MainThread.InvokeOnMainThreadAsync(async () => { await LoadUserTbl(); });
            }
            catch (Exception ex)
            {
                DisplayAlert("AddUserPageOnAppearing Error", $"{ex.Message}", "OK");
            }


        }


        private async void Register_User_Clicked(object sender, EventArgs e)
        {
            if (privilegeLevel == 0)
            {
                await DisplayAlert("Typ av användare inte vald", "Välj typ av användare.", "OK");
            }

            else if (string.IsNullOrEmpty(firstnamebox.Text) || CheckFormat.CheckIfAllLetter(firstnamebox.Text) == false)
            {
                await DisplayAlert("Misslyckades", "Förnamnfältet är tomt eller så är formatet inte tållåtet.", "OK");
            }

            else if (string.IsNullOrEmpty(lastnamebox.Text) || CheckFormat.CheckIfAllLetter(lastnamebox.Text) == false)
            {
                await DisplayAlert("Misslyckades", "Efternamnfältet är tomt eller så är formatet inte tållåtet", "OK");
            }
            else if (string.IsNullOrEmpty(addressbox.Text) || CheckFormat.CheckAdress(addressbox.Text) == false)
            {
                await DisplayAlert("Misslyckades", "Adressfältet är tomt eller så är formatet inte tållåtet.", "OK");
            }
            else if (string.IsNullOrEmpty(emailbox.Text) || CheckFormat.IsValidEmail(emailbox.Text) == false)
            {
                await DisplayAlert("Misslyckades", "Emailfältet är tomt eller så är formatet inte tållåtet.", "OK");
            }
            else if (string.IsNullOrEmpty(phonebox.Text) || CheckFormat.CheckIfAllNumbers(phonebox.Text) == false)
            {
                await DisplayAlert("Misslyckades", "Telefonnummerfältet är tomt eller så är formatet inte tållåtet.", "OK");
            }
            else if (string.IsNullOrEmpty(userbox.Text) || CheckFormat.CheckIfAllLetter(userbox.Text) == false)
            {
                await DisplayAlert("Misslyckades", "Användarnmanfältet är tomt eller så är formatet inte tållåtet.", "OK");
            }
            else if (string.IsNullOrEmpty(passbox.Text))
            {
                await DisplayAlert("Misslyckades", "Lösenordfältet är tomt eller så är formatet inte tållåtet.", "OK");
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
                catch (Exception ex)
                {
                    await DisplayAlert("Register_User_Clicked Error", $"Felmeddelande: {ex.Message}", "OK");

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
                await DisplayAlert("Delete_User_Clicked Error", $"Felmeddelande: {ex.Message}", "OK");
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
                if (UserCollection != null)
                {
                    UserCollection.Clear();
                }

                UserCollection.AddRange(await userService.ReadUserListFromDb());

                listOfUsers.ItemsSource = await userService.ReadUserListFromDb();
            }
            catch (Exception ex)
            {
                await DisplayAlert("LoadUserTbl Error", $"Felmeddelande: {ex.Message}", "OK");
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
                            statusString = "Aktiv";
                            break;
                        case 2:
                            statusString = "Försenade böcker";
                            break;
                        case 3:
                            statusString = "Förlorade böcker";
                            break;
                        case 4:
                            statusString = "Stöld";
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("listOfUsers_ItemTapped Error", $"Felmeddelande: {ex.Message}", "OK");

                }
                switch (choice)
                {
                    case "Detaljer för användare":
                        await DisplayActionSheet($"Välj detalj för användare {selectedItem.Username}: ", "Avbryt", null, "Se status för bibliotekskort");
                        await DisplayAlert("Status för bibliotekskort:", $"Användaren {selectedItem.Username} har statusen: '{statusString}' för sitt bibliotekskort", "OK");
                        break;

                    case "Lägg till bibliotekskort":
                        try
                        {
                            bool success = await loanService.ManualAddLibraryCard(selectedItem.User_ID);
                            if (success)
                            {
                                await DisplayAlert("Lyckades!", $"Bibliotekskort tillagt för {selectedItem.Username}", "OK");
                            }
                            else
                            {
                                await DisplayAlert("Misslyckades!", $"Bibliotekskort kunde inte läggas till för {selectedItem.Username}", "OK");
                            }
                        }
                        catch (Exception ex)
                        {
                            await DisplayAlert("listOfUsers_ItemTapped Error", $"Felmeddelande: {ex.Message}", "OK");
                        }

                        break;

                    case "Ändra status för bibliotekskort":
                        var libraryCardDetails = await DisplayActionSheet($"Ny status för bibliotekskort med användarnamn {selectedItem.Username}: ", "Avbryt", null, "Aktivt", "Försenade böcker", "Borttappade böcker", "Stöld");

                        switch (libraryCardDetails)
                        {
                            case "Aktivt":
                                try
                                {
                                    if (!await loanService.ChangeCardStatus(1, selectedItem.Cardnumber))
                                    {
                                        await DisplayAlert("Misslyckades!", $"Status ändrades inte för bibliotekskortet.", "OK");
                                    }
                                    else
                                    {
                                        await loanService.ChangeCardStatus(1, selectedItem.Cardnumber);
                                        await DisplayAlert("Lyckades!", $"Status för bibliotekskortet ändrat till: Aktivt.", "OK");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    await DisplayAlert("listOfUsers_ItemTapped Error", $"Felmeddelande: {ex.Message}", "OK");
                                }

                                break;

                            case "Försenade böcker":
                                try
                                {
                                    if (!await loanService.ChangeCardStatus(2, selectedItem.Cardnumber))
                                    {
                                        await DisplayAlert("Misslyckades!", $"Status ändrades inte för bibliotekskortet.", "OK");
                                    }
                                    else
                                    {
                                        await loanService.ChangeCardStatus(2, selectedItem.Cardnumber);
                                        await DisplayAlert("Lyckades!", $"Status för bibliotekskortet ändrat till: Försenade böcker.", "OK");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    await DisplayAlert("listOfUsers_ItemTapped Error", $"Felmeddelande: {ex.Message}", "OK");
                                }

                                break;

                            case "Borttappade böcker":
                                try
                                {
                                    if (!await loanService.ChangeCardStatus(3, selectedItem.Cardnumber))
                                    {
                                        await DisplayAlert("Misslyckades!", $"Status ändrades inte för bibliotekskortet.", "OK");
                                    }
                                    else
                                    {
                                        await loanService.ChangeCardStatus(3, selectedItem.Cardnumber);
                                        await DisplayAlert("Lyckades!", $"Status för bibliotekskortet ändrat till: Lost books.", "OK");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    await DisplayAlert("listOfUsers_ItemTapped Error", $"Felmeddelande: {ex.Message}", "OK");
                                }

                                break;

                            case "Stöld":
                                try
                                {
                                    if (!await loanService.ChangeCardStatus(4, selectedItem.Cardnumber))
                                    {
                                        await DisplayAlert("Misslyckades!", $"Status ändrades inte för bibliotekskortet.", "OK");
                                    }
                                    else
                                    {
                                        await loanService.ChangeCardStatus(4, selectedItem.Cardnumber);
                                        await DisplayAlert("Lyckades!", $"Status för bibliotekskortet ändrat till: Theft.", "OK");
                                    }
                                }
                                catch (Exception ex) 
                                { 
                                    await DisplayAlert("listOfUsers_ItemTapped Error", $"Felmeddelande: {ex.Message}", "OK"); 
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
                //var answer = await DisplayActionSheet($"Gör ett val för användarnamn {selectedItem.Username}: ", "Avbryt", null, "Detaljer för användare");
                //switch (answer)
                //{
                //    default:
                //}
            }
        }

        private async void SearchUserBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var searchresult = UserCollection.Where(x => x.privilegeName.Contains(SearchUserBar.Text)
                                                             || x.First_Name.Contains(SearchUserBar.Text)
                                                             || x.Last_Name.Contains(SearchUserBar.Text)
                                                             || x.Username.Contains(SearchUserBar.Text)
                                                             || x.Address.Contains(SearchUserBar.Text)
                                                             || x.Email.Contains(SearchUserBar.Text)
                                                             || x.PhoneNumber.Contains(SearchUserBar.Text));
                listOfUsers.ItemsSource = searchresult;

            }
            catch (Exception ex)
            {
                await DisplayAlert("UserListSearchBar", $"Felmeddelande: {ex.Message}", "OK");
            }
        }
    }
}
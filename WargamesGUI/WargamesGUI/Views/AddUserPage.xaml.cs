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
using WargamesGUI.DAL;
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

        public User2 selectedItem;
        public static UserService2 userService = new UserService2();
        public static LoanService2 loanService = new LoanService2();
        
        public string statusString;
        private int privilegeLevel;
        private List<User2> UserCollection;

        public AddUserPage()
        {
            InitializeComponent();
            UserCollection = new List<User2>();
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
                    User2 user = new User2()
                    {
                        First_Name = firstnamebox.Text,
                        Last_Name = lastnamebox.Text,
                        fk_PrivilegeLevel = privilegeLevel,
                        Address = addressbox.Text,
                        Email = emailbox.Text,
                        PhoneNumber = phonebox.Text,
                        Username = userbox.Text,
                        Password = passbox.Text
                    };
                    await userService.AddNewUser(user);

                    switch (privilegeLevel)
                    {

                        case 1:
                            await DisplayAlert("Godkänt", "Du har adderat en ny admin", "OK");
                            break;
                        case 2:
                            await DisplayAlert("Godkänt", "Du har adderat en ny bibliotekarie", "OK");
                            break;
                        case 3:
                            await DisplayAlert("Godkänt", "Du har adderat en ny besökare", "OK");
                            break;
                    }
                    await LoadUserTbl();
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
                bool canRemove = await userService.RemoveUserFromDbAsync(selectedItem.User_ID);
                await DisplayAlert("Godkänt", $"Du har tagit bort användare {selectedItem.First_Name} {selectedItem.Last_Name}", "OK");
                await LoadUserTbl();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Delete_User_Clicked Error", $"Felmeddelande: {ex.Message}", "OK");
            }
        }
        private void listOfUsers_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            selectedItem = (User2)listOfUsers.SelectedItem;

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
                UserCollection = await userService.ReadAllUsersFromDbAsync();
                listOfUsers.ItemsSource = UserCollection;
            }
            catch (Exception ex)
            {
                await DisplayAlert("LoadUserTbl Error", $"Felmeddelande: {ex.Message}", "OK");
            }
        }
        private async void SearchUserBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var searchresult = UserCollection.Where(x => x.TypeOfUser.TypeOfUser.Contains(SearchUserBar.Text)
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

        private async void AlterUser_Button_Clicked(object sender, EventArgs e)
        {

            selectedItem = (User2)listOfUsers.SelectedItem;

            if (selectedItem.TypeOfUser.PrivilegeLevel == 3)
            {
                var choice = await DisplayActionSheet($"Gör ett val för användarnamn {selectedItem.Username}: ", "Avbryt", null, "Status på bibliotekskort", "Ändra status för bibliotekskort");
                try
                {
                    var statusTuple = await userService.GetStatusForLibraryCardFromDbAsync(selectedItem.LibraryCard.LibraryCard_Id);
                    statusString = statusTuple.Item2;

                }
                catch (Exception ex)
                {
                    await DisplayAlert("listOfUsers_ItemTapped Error", $"Felmeddelande: {ex.Message}", "OK");

                }
                switch (choice)
                {
                    case "Status på bibliotekskort":
                       
                        await DisplayAlert("Status för bibliotekskort:", $"Användaren {selectedItem.First_Name} {selectedItem.Last_Name} har statusen: '{statusString}' för sitt bibliotekskort", "OK");
                        break;

                    case "Ändra status för bibliotekskort":
                        var libraryCardDetails = await DisplayActionSheet($"Ny status för bibliotekskort med användarnamn {selectedItem.Username}: ", "Avbryt", null, "Aktivt", "Försenade böcker", "Borttappade böcker", "Stöld");

                        switch (libraryCardDetails)
                        {
                            case "Aktivt":
                                selectedItem.LibraryCard.fk_Status_Id = 1;
                                ChangingCardStatus();
                                break;

                            case "Försenade böcker":
                                selectedItem.LibraryCard.fk_Status_Id = 2;
                                ChangingCardStatus();
                                break;

                            case "Borttappade böcker":
                                selectedItem.LibraryCard.fk_Status_Id = 3;
                                ChangingCardStatus();
                                break;

                            case "Stöld":
                                selectedItem.LibraryCard.fk_Status_Id = 4;
                                ChangingCardStatus();
                                break;

                            default:
                                break;
                        }
                        break;
                }
            }
        }

        private async void ChangingCardStatus()
        {
            try
            {
                var result = await loanService.ChangeLibraryCardStatus(selectedItem.LibraryCard);
                if (!result.Item1)
                {
                    await DisplayAlert("Misslyckades!", "Status ändrades inte för bibliotekskortet.", "OK");
                }
                else if (result.Item1)
                {
                    await DisplayAlert("Lyckades!", $"Status för bibliotekskortet ändrat till: {result.Item2}.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("listOfUsers_ItemTapped Error", $"Felmeddelande: {ex.Message}", "OK");
            }
        }

        }
    }

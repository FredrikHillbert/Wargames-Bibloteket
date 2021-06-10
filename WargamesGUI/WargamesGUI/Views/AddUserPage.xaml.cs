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
            else if (string.IsNullOrEmpty(ssnbox.Text) || CheckFormat.CheckIfAllNumbers(ssnbox.Text) == false)
            {
                await DisplayAlert("Misslyckades", "Personnummerfältet är tomt eller så är formatet inte tållåtet.", "OK");
            }
            else if (string.IsNullOrEmpty(emailbox.Text) || CheckFormat.IsValidEmail(emailbox.Text) == false)
            {
                await DisplayAlert("Misslyckades", "Emailfältet är tomt eller så är formatet inte tållåtet.", "OK");
            }
            else if (string.IsNullOrEmpty(phonebox.Text) || CheckFormat.CheckIfAllNumbers(phonebox.Text) == false)
            {
                await DisplayAlert("Misslyckades", "Telefonnummerfältet är tomt eller så är formatet inte tållåtet.", "OK");
            }
            else if (string.IsNullOrEmpty(userbox.Text) || CheckFormat.CheckIfUserExists(userbox.Text.ToUpper()) == false)
            {
                await DisplayAlert("Misslyckades", "Användarnmanfältet är tomt eller så finns användaren redan.", "OK");
            }
            else if (string.IsNullOrEmpty(passbox.Text))
            {
                await DisplayAlert("Misslyckades", "Lösenordfältet är tomt eller så är formatet inte tållåtet.", "OK");
            }



            else
            {
                firstnameframe.BorderColor = Color.ForestGreen;
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
                        Username = userbox.Text.ToUpper(),
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
                    addressframe.IsVisible = true;
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
                    addressframe.IsVisible = true;
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
                    addressframe.IsVisible = true;
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

        private void firstnamebox_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (string.IsNullOrEmpty(firstnamebox.Text) || CheckFormat.CheckIfAllLetter(firstnamebox.Text) == false)
            {
                firstnameframe.BorderColor = Color.Red;
                firstnamebox.Placeholder = "Fel format. Skriv in förnamn.";
                firstnamebox.PlaceholderColor = Color.Red;
                firstnamewrongcross.IsVisible = true;
                firstnamecorrectcheck.IsVisible = false;
            }
            else
            {
                firstnameframe.BorderColor = Color.Green;
                firstnamewrongcross.IsVisible = false;
                firstnamecorrectcheck.IsVisible = true;
            }
        }

        private void lastnamebox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(lastnamebox.Text) || CheckFormat.CheckIfAllLetter(lastnamebox.Text) == false)
            {
                lastnameframe.BorderColor = Color.Red;
                lastnamebox.Placeholder = "Fel format. Skriv in efternamn.";
                lastnamebox.PlaceholderColor = Color.Red;
                lastnamewrongcross.IsVisible = true;
                lastnamecorrectcheck.IsVisible = false;
            }
            else
            {
                lastnameframe.BorderColor = Color.Green;
                lastnamewrongcross.IsVisible = false;
                lastnamecorrectcheck.IsVisible = true;
            }
        }

        private void addressbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(addressbox.Text) || CheckFormat.CheckAdress(addressbox.Text) == false)
            {
                addressframe.BorderColor = Color.Red;
                addressbox.Placeholder = "Fel format. Skriv in adress.";
                addressbox.PlaceholderColor = Color.Red;
                addresswrongcross.IsVisible = true;
                addresscorrectcheck.IsVisible = false;
            }
            else
            {
                addressframe.BorderColor = Color.Green;
                addresswrongcross.IsVisible = false;
                addresscorrectcheck.IsVisible = true;
            }
        }

        private void ssnbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(ssnbox.Text) || CheckFormat.CheckIfAllNumbers(ssnbox.Text) == false)
            {
                ssnframe.BorderColor = Color.Red;
                ssnbox.Placeholder = "Fel format. Skriv in personnummer.";
                ssnbox.PlaceholderColor = Color.Red;
                ssnwrongcross.IsVisible = true;
                ssncorrectcheck.IsVisible = false;
            }
            else
            {
                ssnframe.BorderColor = Color.Green;
                ssnwrongcross.IsVisible = false;
                ssncorrectcheck.IsVisible = true;
            }
        }

        private void emailbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(emailbox.Text) || CheckFormat.IsValidEmail(emailbox.Text) == false)
            {
                emailframe.BorderColor = Color.Red;
                emailbox.Placeholder = "Fel format. Skriv in email.";
                emailbox.PlaceholderColor = Color.Red;
                emailwrongcross.IsVisible = true;
                emailcorrectcheck.IsVisible = false;
            }
            else
            {
                emailframe.BorderColor = Color.Green;
                emailwrongcross.IsVisible = false;
                emailcorrectcheck.IsVisible = true;
            }
        }

        private void phonebox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(phonebox.Text) || CheckFormat.CheckIfAllNumbers(phonebox.Text) == false)
            {
                phoneframe.BorderColor = Color.Red;
                phonebox.Placeholder = "Fel format. Skriv in telefonnummer.";
                phonebox.PlaceholderColor = Color.Red;
                phonewrongcross.IsVisible = true;
                phonecorrectcheck.IsVisible = false;
            }
            else
            {
                phoneframe.BorderColor = Color.Green;
                phonewrongcross.IsVisible = false;
                phonecorrectcheck.IsVisible = true;
            }
        }

        private async void listOfUsers_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            selectedItem = (User2)e.Item;
            switch (selectedItem.TypeOfUser.PrivilegeLevel)
            {
                case 1:
                    {
                        var userDetails = await DisplayActionSheet("Välj ett alternativ: ", "Avbryt", null, "Ta bort användare");
                        try
                        {
                            if (userDetails == "Ta bort användare")
                            {
                                var removeYesorNo = await DisplayActionSheet($"Vill du verkligen ta bort adminanvändaren: {selectedItem.Username} ", "Nej", "Ja");
                                switch (removeYesorNo)
                                {
                                    case "Ja":
                                        RemoveUser(userDetails);
                                        break;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            await DisplayAlert("listOfVisitors_ItemTapped", $"Anledning: {ex.Message}", "OK");

                        }

                        break;
                    }

                case 2:
                    {
                        var userDetails = await DisplayActionSheet("Välj ett alternativ: ", "Avbryt", null, "Ta bort användare");
                        try
                        {
                            if (userDetails == "Ta bort användare")
                            {
                                var removeYesorNo = await DisplayActionSheet($"Vill du verkligen ta bort biblioteksanvändaren: {selectedItem.Username} ", "Nej", "Ja");
                                switch (removeYesorNo)
                                {
                                    case "Ja":
                                        RemoveUser(userDetails);
                                        break;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            await DisplayAlert("listOfVisitors_ItemTapped", $"Anledning: {ex.Message}", "OK");
                        }

                        break;
                    }

                case 3:
                    {
                        var userDetails = await DisplayActionSheet("Välj ett alternativ: ", "Avbryt", null, "Status på bibliotekskort", "Ändra status för bibliotekskort", "Ta bort användare");
                        try
                        {
                            if (userDetails == "Status på bibliotekskort")
                            {
                                        UserStatus(userDetails);
                            }
                            else if (userDetails == "Ändra status för bibliotekskort")
                            {
                                        ChangeUser(userDetails);
                            }
                            else if (userDetails == "Ta bort användare")
                            {
                                var removeYesorNo = await DisplayActionSheet($"Vill du verkligen ta bort besökare: {selectedItem.Username} ", "Nej", "Ja");
                                switch (removeYesorNo)
                                {
                                    case "Ja":
                                        RemoveUser(userDetails);
                                        break;
                                }
                            }
                            //switch (userDetails)
                            //{
                            //    case "Status på bibliotekskort":
                            //        UserStatus(userDetails);
                            //        break;
                            //    case "Ändra status för bibliotekskort":
                            //        ChangeUser(userDetails);
                            //        break;
                            //    case "Ta bort användare":
                            //        RemoveUser(userDetails);
                            //        break;
                            //}
                        }
                        catch (Exception ex)
                        {
                            await DisplayAlert("listOfVisitors_ItemTapped", $"Anledning: {ex.Message}", "OK");
                        }

                        break;
                    }
            }
        }

        private async void UserStatus(string userDetails)
        {
            if (selectedItem.TypeOfUser.PrivilegeLevel == 3)
            {
                try
                {
                    var statusTuple = await userService.GetStatusForLibraryCardFromDbAsync(selectedItem.LibraryCard.LibraryCard_Id);
                    statusString = statusTuple.Item2;

                }
                catch (Exception ex)
                {
                    await DisplayAlert("listOfUsers_ItemTapped Error", $"Felmeddelande: {ex.Message}", "OK");

                }

                await DisplayAlert("Status för bibliotekskort:", $"Användaren {selectedItem.First_Name} {selectedItem.Last_Name} har statusen: '{statusString}' för sitt bibliotekskort", "OK");

            }
        }

        private async void RemoveUser(string userDetails)
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

        private async void ChangeUser(string userDetails)
        {
            if (selectedItem.TypeOfUser.PrivilegeLevel == 3)
            {
                var libraryCardDetails = await DisplayActionSheet($"Ny status för bibliotekskort med användarnamn {selectedItem.Username}: ", "Avbryt", null, "Aktivt", "Försenade böcker", "Borttappade böcker", "Stöld");
                try
                {
                    var statusTuple = await userService.GetStatusForLibraryCardFromDbAsync(selectedItem.LibraryCard.LibraryCard_Id);
                    statusString = statusTuple.Item2;

                }
                catch (Exception ex)
                {
                    await DisplayAlert("listOfUsers_ItemTapped Error", $"Felmeddelande: {ex.Message}", "OK");

                }

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
            }
        }

        private void userbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(userbox.Text) || CheckFormat.CheckIfUserExists(userbox.Text.ToUpper()) == false)
            {
                userframe.BorderColor = Color.Red;
                userbox.Placeholder = "Du har inte angett något eller så existerar användarnamnet redan.";
                userbox.PlaceholderColor = Color.Red;
                usernamewrongcross.IsVisible = true;
                usernamecorrectcheck.IsVisible = false;
            }
            else
            {
                userframe.BorderColor = Color.Green;
                usernamewrongcross.IsVisible = false;
                usernamecorrectcheck.IsVisible = true;
            }
        }
    }
}

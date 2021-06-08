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
        public User2 selectedItem;
        public static UserService2 userService = new UserService2();
        public static LoanService2 loanService = new LoanService2();
        private int privilegeLevel;
        public string statusString;

        public AddVisitor()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            try
            {
                MainThread.InvokeOnMainThreadAsync(async () => { await ReadVisitorListFromDb(); });
            }
            catch (Exception ex)
            {
                DisplayAlert("AddVisitorOnAppearing Error", $"Felmeddelande: {ex.Message}", "OK");
            }
        }

        private async Task ReadVisitorListFromDb()
        {
            try
            {
                listOfVisitors.ItemsSource = await userService.ReadOnlyVisitorFromDbAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("ReadVisitorListFromDb Error", $"Felmeddelande: {ex.Message}", "OK");
            }
        }
        private async void AddVisitor_Button_Clicked(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(EntryFirstName.Text) || !CheckFormat.CheckIfAllLetter(EntryFirstName.Text))
            {
                await DisplayAlert("Misslyckades", "Förnamnfältet är tomt eller så är formatet inte tillåtet.", "OK");
            }
            else if (string.IsNullOrEmpty(EntryLastName.Text) || !CheckFormat.CheckIfAllLetter(EntryLastName.Text))
            {
                await DisplayAlert("Misslyckades", "Efternmanfältet är tomt eller så är formatet inte tillåtet.", "OK");
            }
            else if (string.IsNullOrEmpty(EntrySsnNumber.Text) || !CheckFormat.CheckIfAllNumbers(EntrySsnNumber.Text))
            {
                await DisplayAlert("Misslyckades", "Personnummerfältet är tomt eller så är formatet inte tillåtet.", "OK");
            }
            else if (string.IsNullOrEmpty(EntryAdress.Text) || !CheckFormat.CheckAdress(EntryAdress.Text))
            {
                await DisplayAlert("Misslyckades", "Adressfältet är tomt eller så är formatet inte tillåtet.", "OK");
            }
            else if (string.IsNullOrEmpty(EntryEmail.Text) || !CheckFormat.IsValidEmail(EntryEmail.Text))
            {
                await DisplayAlert("Misslyckades", "Emailfältet är tomt eller så är formatet inte tillåtet.", "OK");
            }
            else if (string.IsNullOrEmpty(EntryPhoneNumber.Text) || !CheckFormat.CheckIfAllNumbers(EntryPhoneNumber.Text))
            {
                await DisplayAlert("Misslyckades", "Telefonnummerfältet är tomt eller så är formatet inte tillåtet.", "OK");
            }
            else if (string.IsNullOrEmpty(EntryUserName.Text))
            {
                await DisplayAlert("Misslyckades", "Användarnamnfältet är tomt eller formatet är inte tillåtet.", "OK");
            }
            else if (string.IsNullOrEmpty(EntryPassword.Text))
            {
                await DisplayAlert("Misslyckades", "Lösenordsfältet är tomt eller formatet är inte tillåtet.", "OK");
            }

            else
            {
                privilegeLevel = 3;
                try
                {
                    User2 newUser = new User2()
                    {
                        First_Name = EntryFirstName.Text,
                        Last_Name = EntryLastName.Text,
                        fk_PrivilegeLevel = privilegeLevel,
                        Address = EntryAdress.Text,
                        Email = EntryEmail.Text,
                        PhoneNumber = EntryPhoneNumber.Text,
                        Username = EntryUserName.Text,
                        Password = EntryPassword.Text
                    };
                  
                    if (await userService.AddNewUser(newUser))
                    {
                        EntryFirstName.Text = string.Empty;
                        EntryLastName.Text = string.Empty;
                        EntrySsnNumber.Text = string.Empty;
                        EntryAdress.Text = string.Empty;
                        EntryEmail.Text = string.Empty;
                        EntryPhoneNumber.Text = string.Empty;
                        EntryUserName.Text = string.Empty;
                        EntryPassword.Text = string.Empty;
                        
                        await DisplayAlert("Lyckades!", "Du la till en användare!", "OK");
                        await ReadVisitorListFromDb();
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Misslyckades!", $"Anledning: {ex.Message}", "OK");
                }
            }

        }



        private void listOfVisitors_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            selectedItem = (User2)listOfVisitors.SelectedItem;
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

        private async void listOfVisitors_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            selectedItem = (User2)e.Item;
            var visitorDetails = await DisplayActionSheet("Välj ett alternativ: ", "Avbryt", null, "Status på bibliotekskort", "Ändra status för bibliotekskort", "Ta bort besökare");
            try
            {
                switch (visitorDetails)
                {
                    case "Status på bibliotekskort":
                        StatusVisitor(visitorDetails);
                        break;
                    case "Ändra status för bibliotekskort":
                        ChangeVisitor(visitorDetails);
                        break;
                    case "Ta bort besökare":
                        RemoveVisitor(visitorDetails);
                        break;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("listOfVisitors_ItemTapped", $"Anledning: {ex.Message}", "OK");
            }
        }

        private async void StatusVisitor(string visitorDetails)
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

        private async void RemoveVisitor(string visitorDetails)
        {
            try
            {
                await userService.RemoveUserFromDbAsync(selectedItem.User_ID);
                await DisplayAlert("Lyckades!", $"Du tog bort besökaren {selectedItem.First_Name} {selectedItem.Last_Name}", "OK");
                await ReadVisitorListFromDb();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Misslyckades!", $"Anledning: {ex.Message}", "OK");
            }
        }

        private async void ChangeVisitor(string visitorDetails)
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
    }
}

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
        public static LoanService loanService = new LoanService();
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
                listOfVisitors.ItemsSource = await userService.ReadVisitorListFromDb();
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
            //else if (string.IsNullOrEmpty(EntryUserName.Text) || !CheckFormat.CheckIfAllNumbers(EntryCardNumber.Text))
            //{
            //    await DisplayAlert("InvalidSSNnumber", "SSN number is empty or format is not allowed.", "OK");
            //}

            else
            {
                privilegeLevel = 3;
                try
                {
                    if (await userService.AddNewUser(privilegeLevel, EntryFirstName.Text, EntryLastName.Text, EntrySsnNumber.Text, EntryAdress.Text, EntryEmail.Text, EntryPhoneNumber.Text, "11", "11"))
                    {
                        EntryFirstName.Text = string.Empty;
                        EntryLastName.Text = string.Empty;
                        EntrySsnNumber.Text = string.Empty;
                        EntryAdress.Text = string.Empty;
                        EntryEmail.Text = string.Empty;
                        EntryPhoneNumber.Text = string.Empty;
                        //EntryCardNumber.Text = string.Empty;
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

        private async void DeleteVisitor_Clicked(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(handler.theConStringTest))
                {
                    string sql =
                        $"DELETE FROM {DbHandler.theUserTableName} WHERE First_Name = '{selectedItem.First_Name}'";

                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
                await DisplayAlert("Lyckades!", "Du tog bort en besökare!", "OK");
                await ReadVisitorListFromDb();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Misslyckades!", $"Anledning: {ex.Message}", "OK");
            }
        }

        private void listOfVisitors_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            selectedItem = (User)listOfVisitors.SelectedItem;
        }

        private async void listOfVisitors_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            selectedItem = (User)listOfVisitors.SelectedItem;

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
                            await DisplayAlert("listOfVisitors_ItemTapped", $"Anledning: {ex.Message}", "OK");
                        }

                        break;

                    case "Ändra status för bibliotekskort":
                        var libraryCardDetails = await DisplayActionSheet($"Status för bibliotekskort med användarnamn {selectedItem.Username}: ", "Avbryt", null, "Aktivt", "Försenade böcker", "Borttappade böcker", "Stöld");

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
                                    await DisplayAlert("listOfVisitors_ItemTapped", $"Anledning: {ex.Message}", "OK");
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
                                    await DisplayAlert("listOfVisitors_ItemTapped", $"Anledning: {ex.Message}", "OK");
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
                                    await DisplayAlert("listOfVisitors_ItemTapped", $"Anledning: {ex.Message}", "OK");
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
                                    await DisplayAlert("listOfVisitors_ItemTapped", $"Anledning: {ex.Message}", "OK");
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
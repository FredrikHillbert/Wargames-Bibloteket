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

            if (string.IsNullOrEmpty(EntryFirstName.Text) || !CheckFormat.CheckIfAllLetter(EntryFirstName.Text))
            {
                await DisplayAlert("Invalid Format", "Firstname is empty or format is not allowed.", "OK");
            }
            else if (string.IsNullOrEmpty(EntryLastName.Text) || !CheckFormat.CheckIfAllLetter(EntryLastName.Text))
            {
                await DisplayAlert("Invalid Format", "Lastname is empty or format is not allowed.", "OK");
            }
            else if (string.IsNullOrEmpty(EntrySsnNumber.Text) || !CheckFormat.CheckIfAllNumbers(EntrySsnNumber.Text))
            {
                await DisplayAlert("Invalid SSNnumber", "SSN number is empty or format is not allowed.", "OK");
            }
            else if (string.IsNullOrEmpty(EntryAdress.Text) || !CheckFormat.CheckAdress(EntryAdress.Text))
            {
                await DisplayAlert("Invalid address", "Address is empty or format is not allowed.", "OK");
            }
            else if (string.IsNullOrEmpty(EntryEmail.Text) || !CheckFormat.IsValidEmail(EntryEmail.Text))
            {
                await DisplayAlert("Invalid email", "Email is empty or format is not allowed.", "OK");
            }
            else if (string.IsNullOrEmpty(EntryPhoneNumber.Text) || !CheckFormat.CheckIfAllNumbers(EntryPhoneNumber.Text))
            {
                await DisplayAlert("Invalid phonenumber", "Phonenumber is empty or format is not allowed.", "OK");
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
                        await DisplayAlert("Success!", "You added a visitor!", "OK");
                        await ReadVisitorListFromDb();
                    }
                }
                catch (Exception)
                {
                    await DisplayAlert("Error!", $"Reason: {userService.exceptionMessage}", "OK");
                }
            }

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

        private async void listOfVisitors_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            selectedItem = (User)listOfVisitors.SelectedItem;

            if (selectedItem.fk_PrivilegeLevel == 3)
            {
                var choice = await DisplayActionSheet($"Gör ett val för användarnamn {selectedItem.Username}: ", "Avbryt", null, "Detaljer för användare", "Lägg till bibliotekskort", "Ändra status för bibliotekskort");

                switch (choice)
                {
                    case "Detaljer för användare":

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
                        var libraryCardDetails = await DisplayActionSheet($"Status för bibliotekskort med användarnamn {selectedItem.Username}: ", "Avbryt", null, "Aktivt", "Försenade böcker", "Borttappade böcker", "Stöld");

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
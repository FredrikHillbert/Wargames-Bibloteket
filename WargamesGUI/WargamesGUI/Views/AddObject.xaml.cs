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
    public partial class AddObject : ContentPage
    {
        public Book selectedItem;
        public static AddUserPage addUser = new AddUserPage();
        public static UserService userService = new UserService();
        public static BookService bookService = new BookService();
        public static DbHandler handler = new DbHandler();
        private int itemID;
        public AddObject()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            MainThread.InvokeOnMainThreadAsync(async () => { await GetBooksFromDb1(); });
            MainThread.InvokeOnMainThreadAsync(async () => { await GetEbooksFromDb(); });

        }

        private async Task GetBooksFromDb1()
        {
            listOfBooks.ItemsSource = await bookService.GetBooksFromDb1();

        }        
        private async Task GetEbooksFromDb()
        {
            listOfBooks.ItemsSource = await bookService.GetEbooksFromDb();

        }

        public bool AddNewBookAsync(int Item_id, string Title, string ISBN, string Publisher,
                                       string Description, int Price, string Placement, string Author)
        {
            bool success = true;

            try
            {
                // Book
                if (Item_id == 1)
                {
                    using (SqlConnection con = new SqlConnection(DbHandler.theConString))
                    {
                        string query = $"INSERT INTO {DbHandler.theBookTableName} (fk_Item_Id, Title, ISBN, Publisher, Description, Price, Placement) VALUES('{Item_id}', '{Title}', '{ISBN}', '{Publisher}', '{Description}', '{Price}', '{Placement}')";

                        con.OpenAsync();

                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.ExecuteNonQueryAsync();
                        }
                    }
                    success = true;
                    return success;
                }

                // E-book
                else if (Item_id == 2)
                {
                    using (SqlConnection con = new SqlConnection(DbHandler.theConString))
                    {
                        string query =
                            $"INSERT INTO {DbHandler.theBookTableName}" +
                            $"(fk_Item_Id, Title, ISBN, Publisher, Description, Price, Placement) " +
                            $"VALUES('2', '{Title}', '{ISBN}', '{Publisher}', '{Description}', '{Price}', '{Placement}')";

                        con.OpenAsync();

                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.ExecuteNonQueryAsync();
                        }
                    }
                }

                //return Task.FromResult(success);
                success = true;
                return success;
            }

            catch (Exception)
            {
                success = false;
                //return Task.FromResult(success);
                return success;
            }
        }

        private async void AddBook_Button_Clicked(object sender, EventArgs e)
        {
            //picker = fk_Item_Id;
            var Title = EntryTitle.Text;
            var ISBN = EntryISBN.Text;
            var Publisher = EntryPublisher.Text;
            var Description = EntryDescription.Text;
            var Price = 0;
            var Placement = EntryPlacement.Text;
            var Author = EntryAuthor.Text;



            var b = AddNewBookAsync(itemID, Title, ISBN, Publisher, Description, Price, Placement, Author);
            if (b == true)
            {
                await DisplayAlert("Sucess!", "You added a book!", "OK");
                await GetBooksFromDb1();


            }
            else await DisplayAlert("Error!", "Could not add book!", "OK");
        }

        private void picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedItem = (Book)picker.SelectedItem;
            itemID = selectedItem.fk_Item_Id;
        }

        private async void DeleteObject_Clicked(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(DbHandler.theConString))
                {
                    string sql =
                        $"DELETE FROM {DbHandler.theBookTableName} WHERE Title = '{selectedItem.Title}'";

                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
                await DisplayAlert("Success!", "You removed a book!", "OK");
                await GetBooksFromDb1();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error!", $"Reason: {ex.Message}", "OK");
            }
        }

        private void listOfBooks_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            selectedItem = (Book)listOfBooks.SelectedItem;
        }
    }

}
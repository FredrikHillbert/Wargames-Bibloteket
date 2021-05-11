using MvvmHelpers;
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
        private int itemID;

        private ObservableRangeCollection<Book> collection { get; set; } = new ObservableRangeCollection<Book>();
        public AddObject()
        {
            InitializeComponent();
            listOfBooks.ItemsSource = collection;
        }

        protected override void OnAppearing()
        {

            MainThread.InvokeOnMainThreadAsync(async () => { await LoadBooks(); });

        }

        private async Task LoadBooks()
        {
            collection.AddRange(await bookService.GetBooksFromDb());
            collection.AddRange(await bookService.GetEbooksFromDb());
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

            var b = await bookService.AddNewBook(itemID, Title, ISBN, Publisher, Author, Description, Price, Placement);
            if (b)

            {
                await DisplayAlert("Success!", "You added a book!", "OK");
                await bookService.GetBooksFromDb();
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
                // Popup "Reason"

                await bookService.RemoveBook(selectedItem.Id, string.Empty);
                await DisplayAlert("Success!", "You removed a book!", "OK");
                OnAppearing();
                
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error!", $"Reason: {ex.Message}", "OK");
            }
        }

        private void listOfBooks_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            selectedItem = (Book)e.Item;
        }
    }

}

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
        public DeweySub selectedDewey;

        public static AddUserPage addUser = new AddUserPage();
        public static UserService userService = new UserService();
        public static BookService bookService = new BookService();

        private int itemID;

        public int dewymainID;
        public string deweysubID;

        private string subCategoryName;

        List<DeweyMain> deweyMain = new List<DeweyMain>();
        List<DeweySub> deweySub = new List<DeweySub>();

        private ObservableRangeCollection<Book> collection { get; set; } = new ObservableRangeCollection<Book>();

        public AddObject()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            try
            {

                await MainThread.InvokeOnMainThreadAsync(async () => { await LoadBooks(); });

                deweySub = await bookService.GetDeweySubData();
                deweyMain = await bookService.GetDeweyMainData();
                categorypicker.ItemsSource = deweyMain;

            }
            catch (Exception ex)
            {
                await DisplayAlert("AddObjectOnAppearing", $"Felmeddelande: {ex.Message}", "OK");

            }


        }

        private async Task LoadBooks()
        {
            if (collection != null)
            {
                collection.Clear();
            }
            try
            {
                collection.AddRange(await bookService.GetBooksFromDb());

                listOfBooks.ItemsSource = collection;
            }
            catch (Exception ex)
            {
                await DisplayAlert("LoadBooks", $"Felmeddelande: {ex.Message}", "OK");
            }
        }

        private async void AddBook_Button_Clicked(object sender, EventArgs e)
        {
            //picker = fk_Item_Id;
            var Title = EntryTitle.Text;
            var ISBN = EntryISBN.Text;
            var Publisher = EntryPublisher.Text;
            var Author = EntryAuthor.Text;
            var Description = EntryDescription.Text;
            int.TryParse(EntryPrice.Text, out int Price);
            //var Placement = EntryPlacement.Text;
            try
            {
                var b = await bookService.AddNewBook(itemID, Title, ISBN, Publisher, Author, Description, Price, deweysubID, subCategoryName);

                if (b)
                {
                    EntryTitle.Text = string.Empty;
                    EntryISBN.Text = string.Empty;
                    EntryPublisher.Text = string.Empty;
                    EntryAuthor.Text = string.Empty;
                    EntryDescription.Text = string.Empty;
                    EntryPrice.Text = string.Empty;
                    EntrySubCategoryName.Text = string.Empty;
                    //EntryPlacement.Text = string.Empty;
                    await DisplayAlert("Success!", "You added a book!", "OK");
                    await LoadBooks();


                }
                else await DisplayAlert("Error!", "Could not add book!", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("AddBook_Button_Clicked", $"Felmeddelande: {ex.Message}", "OK");
            }



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
                string reason = await DisplayPromptAsync($"Remove book", $"Reason for removing: {selectedItem.Title}?");

                if (reason != null)
                {
                    await bookService.RemoveBook(selectedItem.Id, reason);
                    await DisplayAlert("Success!", "You removed a book!", "OK");
                    await LoadBooks();
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("DeleteObject_Clicked", $"Felmeddelande: {ex.Message}", "OK");
            }
        }

        private async void listOfBooks_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            selectedItem = (Book)e.Item;
            var bookDetails = await DisplayActionSheet("Choose action: ", "Cancel", null, "Details", "Change Details", "Delete Book");
            try
            {
                switch (bookDetails)
                {
                    case "Details":
                        Details(selectedItem);
                        break;
                    case "Change Details":
                        Change_Details(selectedItem);
                        break;
                    case "Delete Book":
                        Delete_Book(selectedItem);
                        break;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("listOfBooks_ItemTapped", $"Felmeddelande: {ex.Message}", "OK");
            }

        }

        private async void Delete_Book(Book selectedItem)
        {
            try
            {
                string reason = await DisplayPromptAsync($"Remove book", $"Reason for removing: {selectedItem.Title}?");

                if (reason != null)
                {
                    await bookService.RemoveBook(selectedItem.Id, reason);
                    await DisplayAlert("Success!", "You removed a book!", "OK");
                    await LoadBooks();
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error!", $"Reason: {ex.Message}", "OK");
            }
        }

        private void Change_Details(Book selectedItem)
        {
            throw new NotImplementedException();
        }

        private void Details(Book selectedItem)
        {
            //App.Current.MainPage = new DetailPage();
        }

        private void DetailsSelected_Clicked(object sender, EventArgs e)
        {

        }

        private void listOfBooks_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }

        private async void categorypicker_SelectedIndexChanged(object sender, EventArgs e)
        {


            var selectedDewey = (DeweyMain)categorypicker.SelectedItem;
            try
            {

                switch (selectedDewey.DeweyMain_Id)
                {
                    default:
                        subCategoryName = await DisplayActionSheet($"Välj underkategori för {selectedDewey.MainCategoryName}", "Avbryt", null,
                        deweySub.Where(x => x.fk_DeweyMain_Id == selectedDewey.DeweyMain_Id)
                                .Select(x => x.SubCategoryName)
                                .ToArray());
                        if (subCategoryName == "Avbryt" || subCategoryName == null)
                        {
                            EntrySubCategoryName.Text = string.Empty;
                            break;
                        }

                        deweysubID = deweySub.Where(x => x.SubCategoryName == subCategoryName)
                                            .Select(x => x.DeweySub_Id)
                                            .ToList().ElementAt(0).ToString();

                        EntrySubCategoryName.Text = subCategoryName;

                        break;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Misslyckat", $"{ex.Message}", "Ok");
                
            }


        }

    }
}

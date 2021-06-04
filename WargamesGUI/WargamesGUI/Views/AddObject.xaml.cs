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
using System.Threading;


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

        public int bookCopyID;
        public int dewymainID;
        public string deweysubID;

        private string subCategoryName;

        List<DeweyMain> deweyMain = new List<DeweyMain>();
        List<DeweySub> deweySub = new List<DeweySub>();
        List<BookCopy> bookCopies = new List<BookCopy>();

        private ObservableRangeCollection<Book> collection { get; set; } = new ObservableRangeCollection<Book>();

        public AddObject()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            try
            {

                await MainThread.InvokeOnMainThreadAsync(async () => { await LoadAllData(); });


            }
            catch (Exception ex)
            {
                await DisplayAlert("AddObject_OnAppearing", $"{ex.Message}", "OK");

            }
        }
        public async Task LoadAllData()
        {
            // Fixar snart - Alex

            //DisplayProgressGrid.IsVisible = true;
            //DisplayProgress.IsVisible = true;
            //popupStackLayout.IsVisible = true;
            //progressBar1.IsVisible = true;
            //await progressBar1.ProgressTo(0.75, 500, Easing.Linear);
            await LoadDeweyData();
            await LoadBooks();
            await LoadBookCopies();

            // Fixar snart - Alex

            //progressBar1.IsVisible = false;
            //DisplayProgress.IsVisible = false;
            //DisplayProgressGrid.IsVisible = false;
        }
        private async Task LoadBookCopies()
        {
            try
            {
                bookCopies = await bookService.GetBookCopiesFromDb();
            }
            catch (Exception ex)
            {
                await DisplayAlert("LoadBookCopies", $"Anledning: {ex.Message}", "OK");
            }
        }
        private async Task LoadDeweyData()
        {
            deweySub = await bookService.GetDeweySubData();
            deweyMain = await bookService.GetDeweyMainData();
            categorypicker.ItemsSource = deweyMain;
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
                await DisplayAlert("LoadBooks", $"Anledning: {ex.Message}", "OK");
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
                    await bookService.AddNewBook(itemID, Title, ISBN, Publisher, Author, Description, Price, deweysubID, subCategoryName);
              
                    EntryTitle.Text = string.Empty;
                    EntryISBN.Text = string.Empty;
                    EntryPublisher.Text = string.Empty;
                    EntryAuthor.Text = string.Empty;
                    EntryDescription.Text = string.Empty;
                    EntrySubCategoryName.Text = string.Empty;
                    EntryPrice.Text = string.Empty;
                    await DisplayAlert("Lyckades!", "Du la till en bok!", "OK");
                    await LoadAllData();      
            }
            catch (Exception ex)
            {
                await DisplayAlert("AddBook_Button_Clicked", $"Anledning: {ex.Message}", "OK");
            }
        }

        private void picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedItem = (Book)picker.SelectedItem;
            itemID = selectedItem.fk_Item_Id;
        }

        private async void listOfBooks_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            selectedItem = (Book)e.Item;
            var bookDetails = await DisplayActionSheet("Välj ett alternativ: ", "Avbryt", null, "Detaljer", "Ändra Detaljer", "Ta bort exemplar av boken");
            try
            {
                switch (bookDetails)
                {
                    case "Detaljer":
                        Details(selectedItem);
                        break;
                    case "Ändra Detaljer":
                        Change_Details(selectedItem);
                        break;
                    case "Ta bort exemplar av boken":
                        Delete_Book(selectedItem);
                        break;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("listOfBooks_ItemTapped", $"Anledning: {ex.Message}", "OK");
            }

        }
        private async void Delete_Book(Book selectedItem)
        {
            //var selectedBookCopy = new BookCopy() { fk_Book_Id = selectedItem.Id };

            try
            {
                var check = bookCopies.Where(x => x.fk_Book_Id == selectedItem.Id).Select(x => x).Where(x => x.fk_Availability == 1).ToList();
                if (check.Count == 0)
                {
                    await DisplayAlert("Misslyckades!", $"Denna bok har inga exemplar tillgängliga!", "OK");
                    
                }
                else
                {
                    switch (selectedItem.Id)
                    {
                        default:

                            string bookCopy = await DisplayActionSheet($"Ta bort exemplar av: \n{selectedItem.Title}", "Avbryt", null, bookCopies
                                                                                                        .Where(x => x.fk_Book_Id == selectedItem.Id)
                                                                                                        .Where(x => x.fk_Availability == 1)
                                                                                                        .Select(x => x.ToString())
                                                                                                        .ToArray());

                            if (bookCopy == "Avbryt" || string.IsNullOrEmpty(bookCopy))
                            {
                                await LoadAllData();
                                break;
                            }
                            else if (bookCopy != null)
                            {
                                bookCopyID = bookCopies
                                    .Where(x => x.fk_Book_Id == selectedItem.Id)
                                    .Select(x => x)
                                    .ToList()
                                    .Where(x => x.ToString() == bookCopy)
                                    .Select(x => x.Copy_Id)
                                    .ElementAt(0);

                                var reason = await DisplayActionSheet($"Anledning för att ta bort exemplar av: \n{selectedItem.Title}", "Avbryt", null, "Dåligt skick", "Borttappad", "Stulen", "Annan anledning");
                                switch (reason)
                                {
                                    
                                    case "Annan anledning":
                                        string otherReason = await DisplayPromptAsync($"Ta bort exemplar", $"Anledning för att ta bort exemplar av: \n{selectedItem.Title} \n\n{bookCopy}", maxLength: 20);
                                        await TryRemoveBookCopy(selectedItem, bookCopy, otherReason);
                                        await LoadAllData();
                                        break;
                                    case "Avbryt":
                                        break;
                                    default:
                                        await TryRemoveBookCopy(selectedItem, bookCopy, reason);
                                        await LoadAllData();
                                        break;
                                }
                            }

                            break;
                    }
                }
            }
            catch (Exception ex)
            {

                await DisplayAlert("Delete_Book", $"{ex.Message}", "OK");
            }
        }
        public async Task TryRemoveBookCopy(Book selectedItem, string bookCopy, string reason)
        {
            try
            {
                if (string.IsNullOrEmpty(reason))
                {
                    await DisplayAlert("Misslyckades!", $"Du angav ingen anledning, försök igen.", "OK");
                }
                else if (await bookService.RemoveBookCopy(bookCopyID, reason))
                {
                    await DisplayAlert("Exemplar borttaget!", $"Du har tagit bort ett exemplar av {selectedItem.Title}\n\n{bookCopy} \nAnledning: {reason}.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("TryRemoveBookCopy", $"Anledning: {ex.Message}", "OK");
            }
            
        }

        private async void Change_Details(Book selectedItem)
        {
            int idOfBook = selectedItem.Id;
            //string bookType = selectedItem.fk_Item_Id.ToString();
            int bookType = selectedItem.fk_Item_Id;
            string title = selectedItem.Title;
            string author = selectedItem.Author;
            string publisher = selectedItem.Publisher;
            string isbn = selectedItem.ISBN;
            int copies = selectedItem.Available_copies;
            string description = selectedItem.Description;
            string placement = selectedItem.Placement;
            //int price = (int)selectedItem.Price;
            string category = selectedItem.Category;
            string price = selectedItem.Price;
            await Navigation.PushAsync(new ChangeDetailPage(idOfBook, bookType, title, author, publisher, isbn, copies, description,category, placement, price));

        }

        private async void Details(Book selectedItem)
        {
            //App.Current.MainPage = new DetailPage();
            string bookType = selectedItem.fk_Item_Id.ToString();
            string title = selectedItem.Title;
            string author = selectedItem.Author;
            string publisher = selectedItem.Publisher;
            string isbn = selectedItem.ISBN;
            int copies = selectedItem.Available_copies;
            string description = selectedItem.Description;
            string category = selectedItem.Category;
            string placement = selectedItem.Placement;
            await Navigation.PushAsync(new DetailPage(bookType, title, author, publisher, isbn, copies, description, category, placement));
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

            if (selectedDewey != null)
            {
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
                    await DisplayAlert("Misslyckades", $"{ex.Message}", "Ok");

                }
            }
        }

    }
}

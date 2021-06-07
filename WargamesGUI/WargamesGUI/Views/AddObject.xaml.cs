using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WargamesGUI.Models;
using WargamesGUI.Services;
using WargamesGUI.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WargamesGUI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddObject : ContentPage
    {
        public Book2 selectedItem;
        public Book2 newBook = new Book2();
        public DeweySub selectedDewey;
        public static BookService2 bookService = new BookService2();

        private int itemID;

        public int bookCopyID;
        public int dewymainID;
        public string deweysubID;

        private string subCategoryName;
        AddUpdateDetailBookViewModel bookViewModel;
        List<DeweyMain> deweyMain = new List<DeweyMain>();
        List<DeweySub> deweySub = new List<DeweySub>();
        List<BookCopy> bookCopies = new List<BookCopy>();

        private ObservableRangeCollection<Book2> collection { get; set; } = new ObservableRangeCollection<Book2>();

        public AddObject()
        {
            BindingContext = bookViewModel = new AddUpdateDetailBookViewModel(new Book2() { BookType = new Item(), DeweySub = new DeweySub(), DeweyMain = new DeweyMain() });
            InitializeComponent();
        }
        protected async override void OnAppearing()
        {
            try
            {
                await MainThread.InvokeOnMainThreadAsync(async () => { await LoadDeweyData(); });
                await MainThread.InvokeOnMainThreadAsync(async () => { await LoadAllBooks(); });
            } 
            catch (Exception ex)
            {
                await DisplayAlert("AddObject_OnAppearing", $"{ex.Message}", "OK");
            }
        }
        public async Task LoadAllBooks()
        {
            await LoadBooks();
            await LoadBookCopies();
        }
        private async Task LoadBookCopies()
        {
            try
            {
                bookCopies = await bookService.GetAllBookCopies();
            }
            catch (Exception ex)
            {
                await DisplayAlert("LoadBookCopies", $"Anledning: {ex.Message}", "OK");
            }
        }
        private async Task LoadDeweyData()
        {
            deweySub = await bookService.GetDeweySub();
            deweyMain = await bookService.GetDeweyMain();
            categoryPicker.ItemsSource = deweyMain;
        }
        private async Task LoadBooks()
        {
            if (collection != null)
            {
                collection.Clear();
            }
            try
            {
                collection.AddRange(await bookService.GetAllBooks());

                listOfBooks.ItemsSource = collection;

            }
            catch (Exception ex)
            {
                await DisplayAlert("LoadBooks", $"Anledning: {ex.Message}", "OK");
            }
        }
        private async void AddBook_Button_Clicked(object sender, EventArgs e)
        {
            BindingContext = bookViewModel;
            var result = await bookViewModel.AddNewBook();
            try
            {
                if (result.Item1) await DisplayAlert($"Lyckades!", $"Du har lagt till en ny bok!", "OK");
                else await DisplayAlert("Misslyckades!", $"Kunde inte uppdatera boken!", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("AddBook_Button_Clicked", $"Anledning: {ex.Message} - {result.Item2}", "OK");
            }
        }

        private async void picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (typePicker.SelectedIndex == 0)
            {
                await MainThread.InvokeOnMainThreadAsync(() => { bookViewModel.BookType.Item_Id = 1; });
                await MainThread.InvokeOnMainThreadAsync(() => { bookViewModel.BookType.TypeOfItem = "Bok"; });
            }
            else if (typePicker.SelectedIndex == 1)
            {
                await MainThread.InvokeOnMainThreadAsync(() => { bookViewModel.BookType.Item_Id = 2; });
                await MainThread.InvokeOnMainThreadAsync(() => { bookViewModel.BookType.TypeOfItem = "E-Bok"; });
            }
        }

        private async void listOfBooks_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            selectedItem = (Book2)e.Item;
            var bookDetails = await DisplayActionSheet("Välj ett alternativ: ", "Avbryt", null, "Detaljer", "Ändra Detaljer", "Ta bort exemplar av boken", "Ta bort hela objektet");
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
                    //case "Ta bort exemplar av boken":
                    //    Delete_Book(selectedItem);
                    //    break;
                    case "Ta bort hela objektet":
                        await TryRemovingBookObject(selectedItem);
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
                                await LoadAllBooks();
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
                                        await LoadAllBooks();
                                        break;
                                    case "Avbryt":
                                        break;
                                    default:
                                        await TryRemoveBookCopy(selectedItem, bookCopy, reason);
                                        await LoadAllBooks();
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


        public async Task TryRemovingBookObject(Book2 removeBook) 
        {
            try
            {
                if (await bookService.RemoveBookObject(removeBook)) { await DisplayAlert("Bok borttagen", $"Du har tagit bort alla exemplar av boken {selectedItem.Title}", "OK"); }
                else { await DisplayAlert("Misslyckades!", $"Det gick inte att ta bort alla exemplar av boken {selectedItem.Title} ", "OK"); }
            }
            catch (Exception ex)
            {
                await DisplayAlert("ERROR", $"Anledning: {ex.Message}", "OK");
                throw;
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
        private async void Change_Details(Book2 selectedItem)
        {
            await Navigation.PushAsync(new ChangeDetailPage(selectedItem));
        }

        private async void Details(Book2 selectedItem)
        {
            await Navigation.PushAsync(new DetailPage(selectedItem));
        }
        private void ClearNewBookEntries()
        {
            EntryTitle.Text = string.Empty;
            EntryISBN.Text = string.Empty;
            EntryPublisher.Text = string.Empty;
            EntryAuthor.Text = string.Empty;
            EntryDescription.Text = string.Empty;
            EntrySubCategoryName.Text = string.Empty;
        }
        private void DetailsSelected_Clicked(object sender, EventArgs e)
        {

        }

        private void listOfBooks_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }

        private async void categorypicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedDewey = (DeweyMain)categoryPicker.SelectedItem;

            if (selectedDewey != null)
            {
                var selectedDeweyMain = (DeweyMain)categoryPicker.SelectedItem;

                switch (selectedDeweyMain.DeweyMain_Id)
                {
                    default:
                        var subCategoryName = await DisplayActionSheet($"Välj underkategori för {selectedDeweyMain.MainCategoryName}", "Avbryt", null,
                            deweySub.Where(x => x.fk_DeweyMain_Id == selectedDeweyMain.DeweyMain_Id)
                                    .Select(x => x.SubCategoryName).ToArray());

                        if (subCategoryName != "Avbryt" || subCategoryName != null)
                        {
                            var selectedDeweySub = deweySub.Select(x => x).Where(x => x.SubCategoryName == subCategoryName).ToList().FirstOrDefault();
                            await MainThread.InvokeOnMainThreadAsync(() => { bookViewModel.DeweyMain = selectedDeweyMain; });
                            await MainThread.InvokeOnMainThreadAsync(() => { bookViewModel.DeweySub = selectedDeweySub; });
                            break;
                        }
                        else
                        {
                            EntrySubCategoryName.Text = string.Empty;
                            break;
                        }
                }
            }
        }

    }
}

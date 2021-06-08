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
        public Book2 selectedBook;
        public Book2 newBook = new Book2();
        public DeweySub selectedDewey;
        public static BookService2 bookService = new BookService2();

        public int bookCopyID;
        public int dewymainID;
        public string deweysubID;

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
                if (result) await DisplayAlert($"Lyckades!", $"Du har lagt till en ny bok!", "OK");
                else await DisplayAlert("Misslyckades!", $"Kunde inte lägga till boken!", "OK");
                await MainThread.InvokeOnMainThreadAsync(async () => { await LoadAllBooks(); });
            }
            catch (Exception ex)
            {
                await DisplayAlert("AddBook_Button_Clicked", $"Anledning: {ex.Message}", "OK");
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
            selectedBook = (Book2)e.Item;
            var bookDetails = await DisplayActionSheet("Välj ett alternativ: ", "Avbryt", null, "Detaljer", "Ändra Detaljer", "Ta bort exemplar av boken", "Ta bort alla exemplar av boken");
            try
            {
                switch (bookDetails)
                {
                    case "Detaljer":
                        Details(selectedBook);
                        break;
                    case "Ändra Detaljer":
                        Change_Details(selectedBook);
                        break;
                    case "Ta bort exemplar av boken":
                        Delete_Book(selectedBook);
                        break;
                    case "Ta bort alla exemplar av boken":
                        await TryRemoveBookObject(selectedBook);
                        break;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("listOfBooks_ItemTapped", $"Anledning: {ex.Message}", "OK");
            }

        }
        private async void Delete_Book(Book2 selectedItem)
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
                                        await TryRemoveBookCopy(selectedBook, bookCopy, otherReason);
                                        await LoadAllBooks();
                                        break;
                                    case "Avbryt":
                                        break;
                                    default:
                                        await TryRemoveBookCopy(selectedBook, bookCopy, reason);
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
        public async Task TryRemoveBookCopy(Book2 selectedBook, string bookCopy, string reason)
        {
            try
            {
                if (string.IsNullOrEmpty(reason))
                {
                    await DisplayAlert("Misslyckades!", $"Du angav ingen anledning, försök igen.", "OK");
                }
                else if (await bookService.RemoveBookCopy(bookCopyID, reason))
                {
                    await DisplayAlert("Exemplar borttaget!", $"Du har tagit bort ett exemplar av {selectedBook.Title}\n\n{bookCopy} \nAnledning: {reason}.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("TryRemoveBookCopy", $"Anledning: {ex.Message}", "OK");
            }
        }
        public async Task TryRemoveBookObject(Book2 removeBook)
        {
            try
            {
                if (await bookService.RemoveBookObject(removeBook)) { await DisplayAlert("Bok borttagen!", $"Du har tagit bort alla exemplar av {selectedBook.Title}.", "OK"); }
                else { await DisplayAlert("Misslyckades!", $"Det gick inte att ta bort boken, försök igen.", "OK"); }
            }
            catch (Exception ex)
            {
                await DisplayAlert("TryRemoveBook", $"Anledning: {ex.Message}", "OK");
                throw;
            }

            await LoadAllBooks();
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
                            EntrySubCategoryName.Text = bookViewModel.DeweySub.SubCategoryName;
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

        private void EntryTitle_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (string.IsNullOrEmpty(EntryTitle.Text))
            {
                entryTitleframe.BorderColor = Color.Red;
                EntryTitle.Placeholder = "Fel format. Skriv in titel.";
                EntryTitle.PlaceholderColor = Color.Red;
                entryTitlewrongcross.IsVisible = true;
                entryTitlecorrectcheck.IsVisible = false;
            }
            else
            {
                entryTitleframe.BorderColor = Color.Green;
                entryTitlewrongcross.IsVisible = false;
                entryTitlecorrectcheck.IsVisible = true;
            }
        }
        private void EntryISBN_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(EntryISBN.Text) || CheckFormat.CheckIfAllNumbers(EntryISBN.Text) == false)
            {
                entryISBNframe.BorderColor = Color.Red;
                EntryISBN.Placeholder = "Fel format. Skriv in ISBN.";
                EntryISBN.PlaceholderColor = Color.Red;
                entryISBNwrongcross.IsVisible = true;
                entryISBNcorrectcheck.IsVisible = false;
            }
            else
            {
                entryISBNframe.BorderColor = Color.Green;
                entryISBNwrongcross.IsVisible = false;
                entryISBNcorrectcheck.IsVisible = true;
            }
        }

        private void EntryPublisher_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(EntryPublisher.Text) || CheckFormat.CheckIfAllLetter(EntryPublisher.Text) == false)
            {
                entryPublisherframe.BorderColor = Color.Red;
                EntryPublisher.Placeholder = "Fel format. Skriv in utgivare.";
                EntryPublisher.PlaceholderColor = Color.Red;
                entryPublisherwrongcross.IsVisible = true;
                entryPublishercorrectcheck.IsVisible = false;
            }
            else
            {
                entryPublisherframe.BorderColor = Color.Green;
                entryPublisherwrongcross.IsVisible = false;
                entryPublishercorrectcheck.IsVisible = true;
            }
        }

        private void EntryDescription_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(EntryDescription.Text))
            {
                entryDescriptionframe.BorderColor = Color.Red;
                EntryDescription.Placeholder = "Fel format. Skriv in beskrivning.";
                EntryDescription.PlaceholderColor = Color.Red;
                entryDescriptionwrongcross.IsVisible = true;
                entryDescriptioncorrectcheck.IsVisible = false;
            }
            else
            {
                entryDescriptionframe.BorderColor = Color.Green;
                entryDescriptionwrongcross.IsVisible = false;
                entryDescriptioncorrectcheck.IsVisible = true;
            }
        }

        private void EntryPrice_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(EntryPrice.Text) || CheckFormat.CheckIfAllNumbers(EntryPrice.Text) == false)
            {
                entryPriceframe.BorderColor = Color.Red;
                EntryPrice.Placeholder = "Fel format. Skriv in pris.";
                EntryPrice.PlaceholderColor = Color.Red;
                entryPricewrongcross.IsVisible = true;
                entryPricecorrectcheck.IsVisible = false;
            }
            else
            {
                entryPriceframe.BorderColor = Color.Green;
                entryPricewrongcross.IsVisible = false;
                entryPricecorrectcheck.IsVisible = true;
            }
        }

        private void EntryAuthor_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(EntryAuthor.Text) || CheckFormat.CheckIfAllLetter(EntryAuthor.Text) == false)
            {
                entryAuthorframe.BorderColor = Color.Red;
                EntryAuthor.Placeholder = "Fel format. Skriv in författare.";
                EntryAuthor.PlaceholderColor = Color.Red;
                entryAuthorwrongcross.IsVisible = true;
                entryDescriptioncorrectcheck.IsVisible = false;
            }
            else
            {
                entryAuthorframe.BorderColor = Color.Green;
                entryAuthorwrongcross.IsVisible = false;
                entryAuthorcorrectcheck.IsVisible = true;
            }
        }
    }
}

using System;
using System.Collections.Generic;
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
    public partial class ChangeDetailPage : ContentPage
    {
        public DeweySub selectedDewey;

        public static BookService bookService = new BookService();

        public int dewymainID;
        public string deweysubID;

        private int ItemID;

        private string subCategoryName;

        List<DeweyMain> deweyMain = new List<DeweyMain>();
        List<DeweySub> deweySub = new List<DeweySub>();

        public int BookID { get; set; }
        public int BookType { get; set; }
        public string BookTypeName { get; set; }
        public string Titles { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }
        public string ISBN { get; set; }
        public int Available_copies { get; set; }
        public string Description { get; set; }
        public string Placement { get; set; }
        public string Price { get; set; }
        public string Category { get; set; }
        public ChangeDetailPage(int bookId, int bookType, string title, string author, string publisher, string isbn, int copies, string description, string category, string placement, string price)
        {
            BookID = bookId;
            switch (bookType)
            {
                case 1:
                    BookTypeName = "Bok";
                    break;
                case 2:
                    BookTypeName = "E-bok";
                    break;
            }
            BookType = bookType;
            Titles = title;
            Author = author;
            Publisher = publisher;
            ISBN = isbn;
            Available_copies = copies;
            Description = description;
            Category = category;
            Placement = placement;
            Price = price;
            BindingContext = this;

            InitializeComponent();
        }
        protected async override void OnAppearing()
        {
            try
            {

                await MainThread.InvokeOnMainThreadAsync(async () => { await LoadDeweyData(); });

            }
            catch (Exception ex)
            {
                await DisplayAlert("AddObject_OnAppearing", $"{ex.Message}", "OK");

            }
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
                await DisplayAlert("Misslyckades", $"{ex.Message}", "Ok");

            }

        }

        private async Task LoadDeweyData()
        {
            deweySub = await bookService.GetDeweySubData();
            deweyMain = await bookService.GetDeweyMainData();
            categorypicker.ItemsSource = deweyMain;
        }

        private async void UpdateBook_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (await bookService.UpdateBook(BookID, BookType, EntryTitle.Text, EntryAuthor.Text, EntryPublisher.Text, EntryDescription.Text, EntryISBN.Text, deweysubID, subCategoryName))
                {
                    EntryTitle.Text = string.Empty;
                    EntryISBN.Text = string.Empty;
                    EntryPublisher.Text = string.Empty;
                    EntryAuthor.Text = string.Empty;
                    EntryDescription.Text = string.Empty;
                    EntrySubCategoryName.Text = string.Empty;
                    await DisplayAlert("Lyckades!", "Du uppdaterad en bok!", "OK");
                }
                else await DisplayAlert("Misslyckades!", "Kunde inte uppdatera boken!", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("UpdateBook_Clicked", $"Anledning: {ex.Message}", "OK");
            }
        }
    }
}
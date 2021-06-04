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
    public partial class ChangeDetailPage : ContentPage
    {
        public BookService2 bookService;
        AddUpdateDetailBookViewModel bookViewModel;
        List<DeweyMain> deweyMain = new List<DeweyMain>();
        List<DeweySub> deweySub = new List<DeweySub>();

        public ChangeDetailPage(Book2 book)
        {
            bookService = new BookService2();
            BindingContext = bookViewModel = new AddUpdateDetailBookViewModel(book);
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
        private async Task LoadDeweyData()
        {
            deweySub = await bookService.GetDeweySub();
            deweyMain = await bookService.GetDeweyMain();
            categoryPicker.ItemsSource = deweyMain;
            
        }
        private async void categorypicker_SelectedIndexChanged(object sender, EventArgs e)
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
        private async void UpdateBook_Clicked(object sender, EventArgs e)
        {
            var result = await bookViewModel.UpdateBook();
            try
            {
                if (result.Item1) await DisplayAlert($"Lyckades!", $"Du har uppdaterat en bok!", "OK");
                else await DisplayAlert("Misslyckades!", $"Kunde inte uppdatera boken!", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("UpdateBook_Clicked", $"Anledning: {ex.Message} - {result.Item2}", "OK");
            }
        }

        private async void typePicker_SelectedIndexChanged(object sender, EventArgs e)
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
    }
}
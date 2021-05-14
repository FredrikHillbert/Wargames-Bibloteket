using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WargamesGUI.Services;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WargamesGUI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ManualReturn : ContentPage
    {
        public static BookService bookService = new BookService();
        public ManualReturn()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {

            MainThread.InvokeOnMainThreadAsync(async () => { await LoadBooks(); });

        }

        private async Task LoadBooks()
        {
            try
            {
                listOfBooks.ItemsSource = await bookService.GetBorrowedBooksFromDbLibrarian();
            }
            catch (Exception ex)
            {
                await DisplayAlert("LoadError", $"Reason for error: {ex.Message}", "OK");
            }
            
        }
    }
}
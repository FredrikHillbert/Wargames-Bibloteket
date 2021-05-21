using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WargamesGUI.Models;
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
        public static LoanService loanService = new LoanService();
        public static Book selectedBook;
        public ManualReturn()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            try
            {
                MainThread.InvokeOnMainThreadAsync(async () => { await LoadBooks(); });
            }
            catch (Exception ex)
            {
                DisplayAlert("ManualReturnOnAppearing Error", $"Felmeddelande: {ex.Message}", "OK");
            }

        }
        private async Task LoadBooks()
        {
            try
            {
                listOfBooks.ItemsSource = await loanService.GetBorrowedBooksFromDbLibrarian();

            }
            catch (Exception ex)
            {
                await DisplayAlert("LoadBooks", $"Felmeddelande: {ex.Message}", "OK");
            }

        }
        private async void Handled_Clicked(object sender, EventArgs e)
        {
            if (selectedBook.Status == "Återlämnad")
            {

                try
                {
                    await loanService.UpdateBorrowedBooksFromDbLibrarian(selectedBook.Loan_Id);
                    await DisplayAlert("Bok Hanterad!", $"Du hanterade {selectedBook.Title}.", "OK");
                    await LoadBooks();

                }
                catch (Exception ex)
                {
                    await DisplayAlert("Handled_Clicked Error", $"Felmeddelande: {ex.Message}", "OK");
                }

            }
            else
            {
                await DisplayAlert("BookNotReturned", "Boken du försöker hantera är inte återlämnad. Statusen på boken måste vara 'återlämnad' för att kunna hanteras.", "OK");
            }
        }

        private void listOfBooks_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            selectedBook = (Book)e.Item;
        }

        private async void Refresh_Clicked(object sender, EventArgs e)
        {
            try
            {
                await LoadBooks();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Refresh_Clicked Error", $"Felmeddelande: {ex.Message}", "OK");
            }

        }
    }
}
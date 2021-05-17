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
    public partial class ManualReturn : ContentPage
    {
        public static BookService bookService = new BookService();
        public static Book selectedBook;
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

        private async void Handled_Clicked(object sender, EventArgs e)
        {
            if (selectedBook.Status == "Returned")
            {
                
                try
                {
                    var loanID = selectedBook.Loan_Id;
                    await bookService.UpdateBorrowedBooksFromDbLibrarian(loanID);
                    await DisplayAlert("Book handled!", $"You handled {selectedBook.Title}.", "OK");
                    await LoadBooks();
                    
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error!", $"Reason for error: {ex.Message}", "OK");
                }

            }
            else
            {
                await DisplayAlert("BookNotReturned", "The book you are trying to handle is not returned. The status of the book has to be 'returned' in order to handle.", "OK");
            }
        }

        private void listOfBooks_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            selectedBook = (Book)e.Item;
        }
    }
}
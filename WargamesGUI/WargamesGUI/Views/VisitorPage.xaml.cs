using MvvmHelpers;
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
    public partial class VisitorPage : ContentPage
    {
        private ObservableRangeCollection<Book> collection { get; set; } = new ObservableRangeCollection<Book>();
        private ObservableRangeCollection<Book> collection2 { get; set; } = new ObservableRangeCollection<Book>();
        private IEnumerable<string> books;
        public Book selectedItem;
        public User user;
        public Book itemTapped;
        public static AddUserPage addUser = new AddUserPage();
        public static UserService userService;
        public static BookService bookService = new BookService();
        public static LoanService bookLoanService = new LoanService();

        public static DbHandler handler = new DbHandler();
        public VisitorPage()
        {
            InitializeComponent();
            BindingContext = listofbooks;
        }
        protected override void OnAppearing()
        {

            MainThread.InvokeOnMainThreadAsync(async () => { await LoadBooks(); });


        }

        private async Task LoadBooks()
        {
            try
            {
                if (collection != null || collection2 != null)
                {
                    collection.Clear();
                    collection2.Clear();
                }

                collection.AddRange(await bookService.GetBooksFromDb());
                collection2.AddRange(await bookLoanService.GetLoanedBooksFromDb(UserService.fk_LibraryCard));
                
                listofbooks.ItemsSource = collection;
                listofBorrowedbooks.ItemsSource = collection2;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"{ex.Message}", "Ok");
                throw;
            }
        }
        private async Task LoadBorrowedBooks()
        {
            if (collection != null)
            {
                collection.Clear();
            }

            collection.AddRange(await bookLoanService.GetLoanedBooksFromDb(UserService.fk_LibraryCard));
            listofBorrowedbooks.ItemsSource = collection;
        }

        private async void listofbooks_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            selectedItem = (Book)e.Item;

        }

        private async void Loan_Button_Clicked(object sender, EventArgs e)
        {
            if (selectedItem.InStock == 0)
            {
                await DisplayAlert("Error", "Book is not in stock", "OK");

            }
            else
            {
                switch (await bookService.LoanBook(selectedItem.Id, UserService.fk_LibraryCard))
                {
                    case 0:
                        await DisplayAlert("Successful", "Book is added", "OK");
                        break;
                    case 1:
                        await DisplayAlert("Error", "You have delayed books. Return them before trying to loan a new one", "OK");
                        break;
                    case 2:
                        await DisplayAlert("Error", "You have lost books. Contact the library to solve this issue", "OK");
                        break;
                    case 3:
                        await DisplayAlert("Error", "You have stolen books. Contact the library to solve this issue", "OK");
                        break;
                    default:
                        await DisplayAlert("Error", "Unknown error. Contact the library to solve this issue", "OK");
                        break;
                }
            }
        }

        private void Back_Button_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new MainPage();
        }
        private void listofBorrowedbooks_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            itemTapped = (Book)e.Item;
        }
        private async void HandBookBack_Button_Clicked(object sender, EventArgs e)
        {
            //LoanService.LoanedBooks.Remove(itemTapped);
            await bookLoanService.ChangeBookLoanStatus(itemTapped.Loan_Id);
            await DisplayAlert("Success", "Your book is handed back", "OK");

            await LoadBooks();
        }

        private void SearchBar_SearchButtonPressed(object sender, EventArgs e)
        {
            var keyword = MainSearchBar.Text;

            listofbooks.ItemsSource = collection2;

            collection2 = (ObservableRangeCollection<Book>)books.Where(x => x.Contains(keyword));
        }
    }
}
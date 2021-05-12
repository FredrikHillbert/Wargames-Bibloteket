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
        public User selectedItem;
        public static AddUserPage addUser = new AddUserPage();
        public static UserService userService = new UserService();
        public static BookService bookService = new BookService();

        public static DbHandler handler = new DbHandler();
        public VisitorPage()
        {
            InitializeComponent();

        }
        protected override void OnAppearing()
        {

            MainThread.InvokeOnMainThreadAsync(async () => { await LoadBooks(); });

        }

        private void listofbooks_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }
        private async Task LoadBooks()
        {
            if (collection != null)
            {
                collection.Clear();
            }

            collection.AddRange(await bookService.GetBooksFromDb());
            //collection.AddRange(await bookService.GetEbooksFromDb());
            listofbooks.ItemsSource = collection;
        }

    }
}
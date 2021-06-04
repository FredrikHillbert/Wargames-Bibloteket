using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WargamesGUI.Models;
using WargamesGUI.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WargamesGUI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReportPage : ContentPage
    {
        private int _itemID;
        private List<Book> UserCollection { get; set; } = new List<Book>();
        private List<Book> BookCollection { get; set; } = new List<Book>();
        private List<RemovedItem> RemovedBooks { get; set; } = new List<RemovedItem>();
        public LoanService loanService = new LoanService();
        public BookService bookService = new BookService();
        public ReportPage()
        {
            InitializeComponent();
            FindUserSearchBar.Placeholder = "";
            BorrowedBooks.IsVisible = false; ;
            AddedBooks.IsVisible = false;
            removedBooks.IsVisible = false;
            Title.IsVisible = false; ;
            Author.IsVisible = false; ;
            Username.IsVisible = false; ;
            Placement.IsVisible = false; ;
            RetrnedDate.IsVisible = false; ;
            Status.IsVisible = false; ;
            Category.IsVisible = false;
            Reason.IsVisible = false;
            Condition.IsVisible = false;
            Price.IsVisible = false;
            Created.IsVisible = false;

        }
        private async Task LoadData<T>(List<T> dataList)
        {
            try
            {
                switch (_itemID)
                {
                    case 1:
                        FindUserSearchBar.Placeholder = "Sök på användarnamn";
                        listOfVisitorsReport.IsVisible = true;
                        listofBooks.IsVisible = false;
                        listofremovedBooks.IsVisible = false;
                        BorrowedBooks.IsVisible = true;
                        AddedBooks.IsVisible = false;
                        removedBooks.IsVisible = false;
                        Title.IsVisible = true;
                        Author.IsVisible = true;
                        Username.IsVisible = true;
                        Placement.IsVisible = true;
                        RetrnedDate.IsVisible = true;
                        Status.IsVisible = true;
                        Category.IsVisible = false;
                        Reason.IsVisible = false;
                        Condition.IsVisible = false;
                        Price.IsVisible = false;
                        Created.IsVisible = false;
                        UserCollection.AddRange((IEnumerable<Book>)dataList);
                        listOfVisitorsReport.ItemsSource = dataList;
                        break;
                    case 2:
                        FindUserSearchBar.Placeholder = "Sök på kategori, pris eller inläggningsdatum";                        
                        BorrowedBooks.IsVisible = false;
                        AddedBooks.IsVisible = true;
                        removedBooks.IsVisible = false;
                        Title.IsVisible = false;
                        Author.IsVisible = false;
                        Username.IsVisible = false;
                        Placement.IsVisible = false;
                        RetrnedDate.IsVisible = false;
                        Status.IsVisible = false;
                        Category.IsVisible = true;
                        Reason.IsVisible = false;
                        Condition.IsVisible = false;
                        Price.IsVisible = true;
                        Created.IsVisible = true;
                        listofBooks.IsVisible = true;
                        listOfVisitorsReport.IsVisible = false;
                        listofremovedBooks.IsVisible = false;
                        BookCollection.AddRange((IEnumerable<Book>)dataList);
                        listofBooks.ItemsSource = dataList;
                        break;
                    case 3:
                        FindUserSearchBar.Placeholder = "";
                        BorrowedBooks.IsVisible = false;
                        AddedBooks.IsVisible = false;
                        removedBooks.IsVisible = true;
                        Title.IsVisible = true;
                        Author.IsVisible = false;
                        Username.IsVisible = false;
                        Placement.IsVisible = false;
                        RetrnedDate.IsVisible = false;
                        Status.IsVisible = false;
                        Category.IsVisible = false;
                        Reason.IsVisible = true;
                        Condition.IsVisible = true;
                        Price.IsVisible = false;
                        Created.IsVisible = false;
                        listofremovedBooks.IsVisible = true;
                        listOfVisitorsReport.IsVisible = false;
                        listofBooks.IsVisible = false;
                        RemovedBooks.AddRange((IEnumerable<RemovedItem>)dataList);
                        listofremovedBooks.ItemsSource = dataList;
                        break;
                    default:
                        break;
                }
                
                

            }
            catch (Exception ex)
            {
                await DisplayAlert("LoadBooks", $"Felmeddelande: {ex.Message}", "OK");
            }

        }
        private async void picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedItem = (Item)picker.SelectedItem;
            _itemID = selectedItem.Item_Id;
            switch (_itemID)
            {
                case 1:
                    await LoadData(await loanService.GetBorrowedBooksFromDbLibrarian());                    
                    break;
                case 2:
                    await LoadData(await bookService.GetBooksFromDb());
                    break;
                case 3:
                    await LoadData(await bookService.GetRemovedBooksFromDB());
                    break;

                
            }
        }
        private async void FindUserSearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                switch (_itemID)
                {
                    case 1:
                        var searchresult = UserCollection.Where(x => x.Username.Contains(FindUserSearchBar.Text));
                        listOfVisitorsReport.ItemsSource = searchresult;
                        break;
                    case 2:                        
                        var result = BookCollection.Where(x => x.Category.Contains(FindUserSearchBar.Text)
                        || x.Price.Contains(FindUserSearchBar.Text)
                        || x.CreatedDate.ToString().Contains(FindUserSearchBar.Text));
                        listofBooks.ItemsSource = result;
                        break;
                    default:
                        break;
                }
                
            }
            catch (Exception ex)
            {
                await DisplayAlert("MainSearchBar_TextChanged Error", $"Felmeddelande: {ex.Message}", "OK");
            }
        }

        private void listOfVisitorsReport_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }

        private void listOfVisitorsReport_ItemTapped(object sender, ItemTappedEventArgs e)
        {

        }
    }
}
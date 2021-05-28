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
        public LoanService loanService = new LoanService();
        public ReportPage()
        {
            InitializeComponent();
        }
        private async Task LoadData<T>(List<T> dataList)
        {
            try
            {
                //if (dataList != null)
                //{
                //    dataList.Clear();                     
                //}              
                UserCollection.AddRange((IEnumerable<Book>)dataList);
                listOfVisitorsReport.ItemsSource = dataList;

            }
            catch (Exception ex)
            {
                await DisplayAlert("LoadBooks", $"Felmeddelande: {ex.Message}", "OK");
            }

        }

        private void listOfReports_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }

        private void listOfReports_ItemTapped(object sender, ItemTappedEventArgs e)
        {

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

                    break;
                case 3:
                    break;

                
            }
        }

        private void listOfVisitorsReport_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }

        private void listOfVisitorsReport_ItemTapped(object sender, ItemTappedEventArgs e)
        {

        }

        private async void FindUserSearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var searchresult = UserCollection.Where(x => x.Username.Contains(FindUserSearchBar.Text));

                listOfVisitorsReport.ItemsSource = searchresult;
            }
            catch (Exception ex)
            {
                await DisplayAlert("MainSearchBar_TextChanged Error", $"Felmeddelande: {ex.Message}", "OK");
            }
        }
    }
}
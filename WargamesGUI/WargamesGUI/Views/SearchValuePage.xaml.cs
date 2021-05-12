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
    public partial class SearchValuePage : ContentPage
    {
        public static BookService bookService = new BookService();
        public static string text;
        public Book selecteditem;
        public SearchValuePage()
        {
            InitializeComponent();
            
        }
        protected override void OnAppearing()
        {
            MainThread.InvokeOnMainThreadAsync(async () => { await SearchValues(); });
        }
        private async Task SearchValues()
        {
            listOfBook.ItemsSource = await bookService.Searching(GetValues(text));
        }

        private void Back_Button_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new MainPage();
        }
        public static string GetValues(string value)
        {
            text = value;
            return text;
        }

        private void listOfBook_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            //selecteditem = (Book)
        }
    }
}
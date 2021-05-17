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
        public static string cardnumber;
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
            
            try
            {
                listOfBook.ItemsSource = await bookService.Searching(GetValues(text));
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"{ex.Message}", "Ok");
            }
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

        private async void listOfBook_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            selecteditem = (Book)e.Item;
            cardnumber = await DisplayPromptAsync($"Loan Book", "Enter your cardnumber please:");


        }

    }
}
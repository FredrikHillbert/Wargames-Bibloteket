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
    public partial class SearchFlyoutPage : ContentPage
    {
        public static UserService service = new UserService();
        public static string text;
        public SearchFlyoutPage()
        {
            InitializeComponent();
            
        }
        protected override void OnAppearing()
        {
            MainThread.InvokeOnMainThreadAsync(async () => { await SearchValues(); });
        }
        private async Task SearchValues()
        {
            listOfBook.ItemsSource = await service.Searching(GetValues(text));
        }
        public static string GetValues(string value)
        {
            text = value;
            return text;
        }



    }
}
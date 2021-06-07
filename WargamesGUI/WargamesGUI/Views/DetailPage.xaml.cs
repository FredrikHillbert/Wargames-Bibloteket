using System;
using WargamesGUI.Models;
using WargamesGUI.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WargamesGUI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetailPage : ContentPage
    {
        public DetailPage(Book2 book)
        {
            BindingContext = new AddUpdateDetailBookViewModel(book);
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new FlyoutLibrarianPage();
        }
    }
}
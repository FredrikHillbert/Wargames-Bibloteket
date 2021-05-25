﻿using System;
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
            try
            {
                MainThread.InvokeOnMainThreadAsync(async () => { await SearchValues(); });
            }
            catch (Exception ex)
            {
                DisplayAlert("SearchValuePageOnAppearing Error", $"Felmeddelande: {ex.Message}", "OK");
            }

        }
        private async Task SearchValues()
        {

            try
            {
                listOfBook.ItemsSource = await bookService.Searching(GetValues(text));
            }
            catch (Exception ex)
            {
                await DisplayAlert("SearchValues Error", $"Felmeddelande: {ex.Message}", "OK");
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

            //await DisplayAlert("Beskrivning", $"{selecteditem.Description}", "OK");

            var answer = await DisplayActionSheet("Välj ett alternativ: ", "Avbryt", null, "Detaljer", "Logga in för att låna boken");

            switch (answer)
            {
                case "Detaljer":
                    await DisplayAlert("Beskrivning", $"{selecteditem.Description}", "OK");
                    break;
                case "Logga in för att låna boken":
                    App.Current.MainPage = new Login2Page();
                    break;
            }
            

            

        }

    }
}
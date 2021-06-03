﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WargamesGUI.Models;
using WargamesGUI.Views;
using Xamarin.Forms;
using WargamesGUI.Services;
using static WargamesGUI.AddUserPage;
using Xamarin.Essentials;
using WargamesGUI.ViewModels;

namespace WargamesGUI
{

    public partial class MainPage : ContentPage
    {
        public static UserService service = new UserService();
        public static User user;
        public static BookService2 bookService2 = new BookService2();

        public List<Book2> bookList;
        public MainPage()
        {
            InitializeComponent();
            MainThread.InvokeOnMainThreadAsync(async () => { await LoadBooks(); });
            //Entrypassword.Completed += (sender, e) => Entrypassword_Completed(sender, e);
        }

        public async Task<List<Book2>> LoadBooks()
        {
            bookList = await bookService2.GetAllBooks();
            return bookList;
        }
        private async void SignIn_Button_Clicked(object sender, EventArgs e)
        {
            if (Entryusername.Text != "" & Entrypassword.Text != "")
            {
                try
                {

                    switch (service.SignIn(Entryusername.Text, Entrypassword.Text))
                    {
                        case 1:
                            Entryusername.Text = string.Empty;
                            Entrypassword.Text = string.Empty;
                            await DisplayAlert("Lyckades", "Du loggar nu in som administratör", "OK");
                            App.Current.MainPage = new FlyoutAdminPage();
                            break;
                        case 2:
                            Entryusername.Text = string.Empty;
                            Entrypassword.Text = string.Empty;
                            await DisplayAlert("Lyckades", "Du loggar nu in som Bibliotekarie", "OK");
                            App.Current.MainPage = new FlyoutLibrarianPage();
                            break;
                        case 3:
                            Entryusername.Text = string.Empty;
                            Entrypassword.Text = string.Empty;
                            await DisplayAlert("Lyckades", "Du loggar nu in som Besökare", "OK");
                            App.Current.MainPage = new VisitorPage();
                            break;
                        default:
                            await DisplayAlert("Misslyckades", "Kotrollera användarnamn och lösenord", "Ok");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("SignIn_Button_Clicked Error", $"Felmeddelande: {ex.Message}", "Ok");
                }
            }
        }

        private async void SearchBar_Clicked(object sender, EventArgs e)
        {

            try
            {
                if (string.IsNullOrWhiteSpace(SearchBar.Text))
                {
                    await DisplayAlert("Misslyckades", "Du måste skriva något", "OK");
                }
                else
                {
                    SearchValuePage.GetValues(SearchBar.Text);
                    App.Current.MainPage = new SearchValuePage();
                }

            }
            catch (Exception ex)
            {
                await DisplayAlert("Misslyckades", $"{ex.Message}", "Ok");
            }
        }

        private void Button_Clicked(object sender, EventArgs e)
        {

        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            AutoCompleteList.IsVisible = true;
            AutoCompleteList.BeginRefresh();

            try
            {
                var titleResult = bookList.Select(x => x.Title).Where(x => x != null && x.ToUpper().Contains(SearchBar.Text.ToUpper())).ToList();
                var publisherResult = bookList.Select(x => x.Publisher).Where(x => x != null && x.ToUpper().Contains(SearchBar.Text.ToUpper())).ToList();
                var authorResult = bookList.Select(x => x.Author).Where(x => x != null && x.ToUpper().Contains(SearchBar.Text.ToUpper())).ToList();
                var isbnResult = bookList.Select(x => x.ISBN).Where(x => x != null && x.ToUpper().Contains(SearchBar.Text.ToUpper())).ToList();
                var subCategoryResult = bookList.Select(x => x.DeweySub.SubCategoryName).Where(x => x != null && x.ToUpper().Contains(SearchBar.Text.ToUpper())).ToList();
                var mainCategoryResult = bookList.Select(x => x.DeweyMain.MainCategoryName).Where(x => x != null && x.ToString().ToUpper().Contains(SearchBar.Text.ToUpper())).ToList();

                var allResults = titleResult.Concat(publisherResult).Concat(authorResult).Concat(isbnResult).Concat(subCategoryResult).Concat(mainCategoryResult).Distinct().ToList();

                if (string.IsNullOrWhiteSpace(SearchBar.Text))
                {
                    AutoCompleteList.IsVisible = false;
                }
                else
                {
                    AutoCompleteList.ItemsSource = allResults;

                }

            }
            catch (Exception ex)
            {
                //AutoCompleteList.IsVisible = false;
            }

          
        }

        private void AutoCompleteList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            string listsd = e.Item as string;
            SearchBar.Text = listsd;
            AutoCompleteList.IsVisible = false;
            ((ListView)sender).SelectedItem = null;
            SearchBar_Clicked(sender, e);
        }

        //private async void Entrypassword_Completed(object sender, EventArgs e)
        //{
        //    SignIn_Button_Clicked(sender, e);
        //}
    }
}

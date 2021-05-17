﻿using MvvmHelpers;
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

        }
        protected override void OnAppearing()
        {

            MainThread.InvokeOnMainThreadAsync(async () => { await LoadBooks(); });


        }

        private async Task LoadBooks()
        {
            try
            {
                if (collection != null)
                {
                    collection.Clear();
                }

                collection.AddRange(await bookService.GetBooksFromDb());
                listofbooks.ItemsSource = collection;
                listofBorrowedbooks.ItemsSource = await bookLoanService.GetLoanedBooksFromDb(UserService.fk_LibraryCard);
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
            else if (await bookService.LoanBook(selectedItem.Id, UserService.fk_LibraryCard))
            {
                await DisplayAlert("Susscessfull", "Book is added", "OK");
            }
            else
            {
                await DisplayAlert("Error", $"{bookService.exceptionMessage}", "OK");
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
        private void Button_Clicked(object sender, EventArgs e)
        {









        }

       
    }
}
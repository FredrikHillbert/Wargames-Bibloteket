using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WargamesGUI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetailPage : ContentPage
    {
        public string BookType { get; set; }
        public string Titles { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }
        public string ISBN { get; set; }
        public string InStock { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Placement { get; set; }

        public DetailPage(string bookType, string title, string author, string publisher, string isbn, string inStock, string description, string category, string placement)
        {
            switch (bookType)
            {
                case "1":
                    BookType = "Bok";
                    break;
                case "2":
                    BookType = "E-bok";
                    break;
            }
            //BookType = bookType;
            Titles = title;
            Author = author;
            Publisher = publisher;
            ISBN = isbn;
            InStock = inStock;
            Description = description;
            Category = category;
            Placement = placement;
            BindingContext = this;

            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new FlyoutLibrarianPage();
        }
    }
}
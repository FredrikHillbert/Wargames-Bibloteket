using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
    public partial class AddVisitor : ContentPage
    {
        public static AddUserPage addUser = new AddUserPage();
        public static UserService userService = new UserService();
        public static DbHandler handler = new DbHandler();
        public AddVisitor()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            MainThread.InvokeOnMainThreadAsync(async () => { await ReadVisitorListFromDb(); });

        }

        private async Task ReadVisitorListFromDb()
        {
            listOfVisitors.ItemsSource = userService.ReadVisitorListFromDb();
        }

        private void AddVisitor_Button_Clicked(object sender, EventArgs e)
        {

        }
    }
}
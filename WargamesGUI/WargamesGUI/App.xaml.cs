using System;
using WargamesGUI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WargamesGUI
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new FlyoutAdminPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}

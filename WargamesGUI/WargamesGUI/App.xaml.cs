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
            Device.SetFlags(new string[] { "MediaElement_Experimental" });

            InitializeComponent();
            
            MainPage = new MainPage();
            //MainPage = new FlyoutLibrarianPage();
            //MainPage = new FlyoutAdminPage();
            //MainPage = new FlyoutVisitorPage();
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
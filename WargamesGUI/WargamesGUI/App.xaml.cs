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
            Device.SetFlags(new[] { "MediaElement_Experimental", "Brush_Experimental" });
            //MainPage = new MainPage();          
            //MainPage = new FlyoutLibrarianPage();
            //MainPage = new FlyoutAdminPage();
            //MainPage = new VisitorPage();
            MainPage = new AddUserPage();
            //MainPage = new Test();
            //MainPage = new ManualReturn();
            
           
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
using System;
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

namespace WargamesGUI
{

    public partial class MainPage : ContentPage
    {
        public static UserService service = new UserService();

        public MainPage()
        {
            InitializeComponent();
        }

        private async void SignIn_Button_Clicked(object sender, EventArgs e)
        {
            Exception exception = null;
            try
            {

                switch (service.SignIn(Entryusername.Text, Entrypassword.Text))
                {
                    case 1:
                        await DisplayAlert("Successful", "You are now logging in as Admin", "OK");
                        App.Current.MainPage = new FlyoutAdminPage();
                        break;
                    case 2:
                        await DisplayAlert("Successful", "You are now logging in as Librarian", "OK");
                        App.Current.MainPage = new FlyoutLibrarianPage();
                        break;
                    default:
                        await DisplayAlert("Error", "Please check if username and password are correct", "Ok");
                        break;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                await DisplayAlert("Error", $"{exception.Message}", "Ok");
                throw;
            }

        }

        private async void SearchBar_Clicked(object sender, EventArgs e)
        {
            Exception exception = null;
            try
            {
                SearchValuePage.GetValues(SearchBar.Text);
                App.Current.MainPage = new SearchValuePage();

            }
            catch (Exception ex)
            {
                exception = ex;
                await DisplayAlert("Error", $"{exception.Message}", "Ok");
                throw;
            }
        }

        private void CardID_Button_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new SearchCardNumber();
        }
        public class ShowHidePassEffect : PlatformEffect
        {
            protected override void OnAttached()
            {
                ConfigureControl();
            }

            protected override void OnDetached()
            {
            }


            private void ConfigureControl()
            {
                EditText editText = ((EditText)Control);
                editText.SetCompoundDrawablesRelativeWithIntrinsicBounds(Resource.Drawable.Password, 0, Resource.Drawable.ShowPass, 0);
                editText.SetOnTouchListener(new OnDrawableTouchListener());
            }
        }

        public class OnDrawableTouchListener : Java.Lang.Object, Android.Views.View.IOnTouchListener
        {
            public bool OnTouch(Android.Views.View v, MotionEvent e)
            {
                if (v is EditText && e.Action == MotionEventActions.Up)
                {
                    EditText editText = (EditText)v;
                    if (e.RawX >= (editText.Right - editText.GetCompoundDrawables()[2].Bounds.Width()))
                    {
                        if (editText.TransformationMethod == null)
                        {
                            editText.TransformationMethod = PasswordTransformationMethod.Instance;
                            editText.SetCompoundDrawablesRelativeWithIntrinsicBounds(Resource.Drawable.Password, 0, Resource.Drawable.ShowPass, 0);
                        }
                        else
                        {
                            editText.TransformationMethod = null;
                            editText.SetCompoundDrawablesRelativeWithIntrinsicBounds(Resource.Drawable.Password, 0, Resource.Drawable.HidePass, 0);
                        }
                        return true;
                    }
                }
                return false;
            }
        }
    }
}

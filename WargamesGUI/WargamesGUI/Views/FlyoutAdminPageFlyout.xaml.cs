﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WargamesGUI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FlyoutAdminPageFlyout : ContentPage
    {
        public ListView ListView;

        public FlyoutAdminPageFlyout()
        {
            InitializeComponent();

            BindingContext = new FlyoutAdminPageFlyoutViewModel();
            ListView = MenuItemsListView;
        }

        class FlyoutAdminPageFlyoutViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<FlyoutAdminPageFlyoutMenuItem> MenuItems { get; set; }

            public FlyoutAdminPageFlyoutViewModel()
            {
                MenuItems = new ObservableCollection<FlyoutAdminPageFlyoutMenuItem>(new[]
                {
                    new FlyoutAdminPageFlyoutMenuItem { Id = 0, Title = "Users", TargetType=typeof(AdminPage) },
                    new FlyoutAdminPageFlyoutMenuItem { Id = 0, Title = "Reports", TargetType=typeof(AdminPage) },
                    
                });
            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}
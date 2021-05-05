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
    public partial class FlyoutLibrarianPageFlyout : ContentPage
    {
        public ListView ListView;

        public FlyoutLibrarianPageFlyout()
        {
            InitializeComponent();

            BindingContext = new FlyoutLibrarianPageFlyoutViewModel();
            ListView = MenuItemsListView;
        }

        class FlyoutLibrarianPageFlyoutViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<FlyoutLibrarianPageFlyoutMenuItem> MenuItems { get; set; }

            public FlyoutLibrarianPageFlyoutViewModel()
            {
                MenuItems = new ObservableCollection<FlyoutLibrarianPageFlyoutMenuItem>(new[]
                {
                    new FlyoutLibrarianPageFlyoutMenuItem { Id = 0, Title = "Hem", TargetType=typeof(FlyoutLibrarianPageDetail) },
                    new FlyoutLibrarianPageFlyoutMenuItem { Id = 1, Title = "Bokhantering", TargetType=typeof(AddObject) },
                    new FlyoutLibrarianPageFlyoutMenuItem { Id = 2, Title = "Besökshantering", TargetType=typeof(AddVisitor) },
                    
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
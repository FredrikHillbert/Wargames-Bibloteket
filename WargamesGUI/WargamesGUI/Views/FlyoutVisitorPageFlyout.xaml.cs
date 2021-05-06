using System;
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
    public partial class FlyoutVisitorPageFlyout : ContentPage
    {
        public ListView ListView;

        public FlyoutVisitorPageFlyout()
        {
            InitializeComponent();

            BindingContext = new FlyoutVisitorPageFlyoutViewModel();
            ListView = MenuItemsListView;
        }

        class FlyoutVisitorPageFlyoutViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<FlyoutVisitorPageFlyoutMenuItem> MenuItems { get; set; }

            public FlyoutVisitorPageFlyoutViewModel()
            {
                MenuItems = new ObservableCollection<FlyoutVisitorPageFlyoutMenuItem>(new[]
                {
                    new FlyoutVisitorPageFlyoutMenuItem { Id = 0, Title = "Page 1" },
                    new FlyoutVisitorPageFlyoutMenuItem { Id = 1, Title = "Page 2" },
                    new FlyoutVisitorPageFlyoutMenuItem { Id = 2, Title = "Page 3" },
                    new FlyoutVisitorPageFlyoutMenuItem { Id = 3, Title = "Page 4" },
                    new FlyoutVisitorPageFlyoutMenuItem { Id = 4, Title = "Page 5" },
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
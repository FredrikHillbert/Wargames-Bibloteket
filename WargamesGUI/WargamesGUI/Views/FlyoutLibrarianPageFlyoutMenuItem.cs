using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WargamesGUI.Views
{
    public class FlyoutLibrarianPageFlyoutMenuItem
    {
        public FlyoutLibrarianPageFlyoutMenuItem()
        {
            TargetType = typeof(FlyoutLibrarianPageFlyoutMenuItem);
        }
        public int Id { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }
    }
}
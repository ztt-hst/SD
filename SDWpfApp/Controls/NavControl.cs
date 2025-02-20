using DevExpress.Data.Browsing;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.WindowsUI.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SDWpfApp.Controls
{
    [POCOViewModel]
    public class NavControl : UserControl, INavigationAware
    {
        public void NavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter == null) return;

            DataContext = e.Parameter;
        }

        public void NavigatingFrom(NavigatingEventArgs e)
        { }

        public void NavigatedFrom(NavigationEventArgs e)
        { }

        public NavControl()
        {

        }
    }
}

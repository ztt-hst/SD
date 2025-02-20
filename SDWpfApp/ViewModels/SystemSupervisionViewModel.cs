using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Mvvm;

namespace SDWpfApp.ViewModels
{
    public class SystemSupervisionViewModel : NavigationViewModelBase
    {
        public virtual MainViewModel Main { get; set; }

        protected override void OnNavigatedTo()
        {
            base.OnNavigatedTo();
            Main = ((ISupportParentViewModel)this).ParentViewModel as MainViewModel;
        }
    }
}

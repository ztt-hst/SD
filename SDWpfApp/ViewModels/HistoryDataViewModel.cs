using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDWpfApp.ViewModels
{
    public class HistoryDataViewModel : NavigationViewModelBase
    {
        protected virtual INavigationService NavigationService { get { return null; } }
        public virtual MainViewModel Main { get; set; }

        protected override void OnNavigatedTo()
        {
            base.OnNavigatedTo();
            Main = ((ISupportParentViewModel)this).ParentViewModel as MainViewModel;
        }

        public void LoadData()
        {
            if (Main.boolCommunicationTypeZTE)
            {
                Main.LoadData();
            }
            if (Main.boolCommunicationTypeDPC)
            {
                Main.LoadData_DPC();
            }
        }

        public void ExportData()
        {
            Main.ExportData(null);
        }
    }
}

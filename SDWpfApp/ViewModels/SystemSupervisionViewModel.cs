using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Mvvm;
using System.Windows.Input;
using SDWpfApp.Models;
using DevExpress.Mvvm.POCO;

namespace SDWpfApp.ViewModels
{
    public class SystemSupervisionViewModel : NavigationViewModelBase
    {
        public virtual MainViewModel Main { get; set; }
        // 添加 MessageBoxService
        protected IMessageBoxService MessageBoxService { get { return this.GetService<IMessageBoxService>(); } }

        protected override void OnNavigatedTo()
        {
            try
            {
                base.OnNavigatedTo();
                Main = ((ISupportParentViewModel)this).ParentViewModel as MainViewModel;
                
            }
            catch (Exception ex)
            {
                MessageBoxService?.ShowMessage($"OnNavigatedTo 错误: {ex.Message}");
            }
        }
    }
}

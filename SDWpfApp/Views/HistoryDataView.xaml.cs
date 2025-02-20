using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SDWpfApp.Models;

namespace SDWpfApp.Views
{
    /// <summary>
    /// Interaction logic for HistoryDataView.xaml
    /// </summary>
    public partial class HistoryDataView
    {
        public HistoryDataView()
        {
            InitializeComponent();
        }

        public void ExportHistoryData_Button_Click(object sender, RoutedEventArgs e)
        {
            new DemoModuleExportHelper(view).ExportToCsv();
        }
    }
}

using DevExpress.Export;
using DevExpress.Xpf.Core;
using DevExpress.XtraPrinting.Localization;
using DevExpress.XtraPrinting.Native.ExportOptionsControllers;
using DevExpress.XtraPrinting.Native;
using DevExpress.XtraPrinting;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;
using SDWpfApp.Views;
using System.IO;

namespace SDWpfApp.Models
{
    //模块导出帮助
    public class DemoModuleExportHelper
    {
        //表格视图
        readonly DevExpress.Xpf.Grid.TableView view;

        public DemoModuleExportHelper(DevExpress.Xpf.Grid.TableView view)
        {
            this.view = view;
        }
        //导出数据为 .xlsx 格式的文件
        public void ExportToXlsx()
        {
            string fileName = GetFileName(new XlsxExportOptions());
            Export((file, options) => view.ExportToXlsx(file, options), fileName, new XlsxExportOptionsEx { ExportType = ExportType.WYSIWYG });
        }
        //导出数据为 .xls 格式的文件
        public void ExportToXls()
        {
            string fileName = GetFileName(new XlsExportOptions());
            Export((file, options) => view.ExportToXls(file, options), fileName, new XlsExportOptionsEx());
        }
        //导出数据为 .csv 格式的文件
        public void ExportToCsv()
        {
            string fileName = GetFileName(new CsvExportOptions());
            Export((file, options) => view.ExportToCsv(file, options), fileName, new CsvExportOptionsEx());
        }
        /*
        通用导出
        功能：通过传入不同的导出方法（例如 ExportToXlsx、ExportToCsv 等），该方法可以处理不同格式的数据导出
       */
        void Export<T>(Action<string, T> exportToFile, string fileName, T options) where T : ExportOptionsBase
        {
            if (string.IsNullOrEmpty(fileName))
                return;

            try
            {
                ExportCore(exportToFile, fileName, options);
            }
            catch (Exception e)
            {
                DXMessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        //执行实际的导出操作，并显示导出进度。它显示一个等待指示器（DXSplashScreen.Show<ExportWaitIndicator>()），并在导出过程中更新进度
        void ExportCore<T>(Action<string, T> exportToFile, string fileName, T options) where T : ExportOptionsBase
        {
            //显示加载页面
            DXSplashScreen.Show<ExportWaitIndicator>();
            //将 options 强制转换为 IDataAwareExportOptions，将事件处理程序 ExportProgress 订阅到 ExportProgress 事件。
            ((IDataAwareExportOptions)options).ExportProgress += ExportProgress;
            try
            {
                exportToFile(fileName, options);
            }
            finally
            {
                //取消订阅
                ((IDataAwareExportOptions)options).ExportProgress -= ExportProgress;
                DXSplashScreen.Close();
            }
            if (ShouldOpenExportedFile())
                ProcessLaunchHelper.Start(fileName);
        }
        //用于更新导出进度条。
        void ExportProgress(ProgressChangedEventArgs ea)
        {
            DXSplashScreen.Progress(ea.ProgressPercentage);
        }
        //获取文件名
        static string GetFileName(ExportOptionsBase options)
        {
            return GetFileName(ExportOptionsControllerBase.GetControllerByOptions(options));
        }
        //
        static string GetFileName(ExportOptionsControllerBase controller)
        {
            SaveFileDialog dlg = CreateSaveFileDialog(controller);
            if (dlg.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(dlg.FileName))
                //return DevExpress.XtraPrinting.Native.FileHelper.SetValidExtension(dlg.FileName, controller.FileExtensions[0], controller.FileExtensions);
                return Path.ChangeExtension(dlg.FileName, controller.FileExtensions[0]);
            else
                return string.Empty;
        }
        //保存文件对话框
        static SaveFileDialog CreateSaveFileDialog(ExportOptionsControllerBase controller)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Title = PreviewLocalizer.GetString(PreviewStringId.SaveDlg_Title);
            dlg.ValidateNames = true;
            dlg.FileName = PrintPreviewOptions.DefaultFileNameDefault;
            dlg.Filter = controller.Filter;
            return dlg;
        }
        //询问用户是否在导出完成后打开文件。
        static bool ShouldOpenExportedFile()
        {
            return DXMessageBox.Show(
                PreviewLocalizer.GetString(PreviewStringId.Msg_OpenFileQuestion),
                PreviewLocalizer.GetString(PreviewStringId.Msg_OpenFileQuestionCaption),
                MessageBoxButton.YesNo,
                MessageBoxImage.Question) == MessageBoxResult.Yes;
        }
    }
}

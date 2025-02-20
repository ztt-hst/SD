using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace SDWpfApp.Models
{
    public class XlsFileControler
    {
        public virtual List<DataParameter> dataParameter { get; set; }//数据参数
        public string[,] arry = new string[100, 10];//二维字符串数组
        //构造函数
        public XlsFileControler()
        {
            dataParameter = new List<DataParameter>();
            ReadFromExcelFile(AppDomain.CurrentDomain.BaseDirectory + "de\\1.xlsx");
        }

        //从Excel文件中读取内容
        public void ReadFromExcelFile(string filePath)
        {
            IWorkbook wk;
            string extension = System.IO.Path.GetExtension(filePath);//获取拓展名

            FileStream fs = File.OpenRead(filePath);//打开文件
            if (extension.Equals(".xls"))
            {
                //把xls文件中的数据写入wk中
                wk = new HSSFWorkbook(fs);//用 HSSFWorkbook 处理 .xls
            }
            else
            {
                //把xlsx文件中的数据写入wk中
                wk = new XSSFWorkbook(fs);//用 XSSFWorkbook 处理 .xlsx
            }

            fs.Close();//关闭
            
            ISheet sheet = wk.GetSheetAt(0);//获取第一个工作表

            IRow row = sheet.GetRow(0);  //读取数据
                                         //LastRowNum 是当前表的总行数-1（注意）从0开始
            //int offset = 0;

            for (int i = 1; i <= sheet.LastRowNum; i++)//遍历每一行
            {
                row = sheet.GetRow(i);  //读取当前行数据
                if (row != null)        //行不为空
                {
                    for (int j = 0; j <= row.LastCellNum; j++) //遍历当前行的每一列
                    {
                        ICell cell = row.GetCell(j);  //当前单元格
                        if (cell != null) // 如果单元格不为空
                        {
                            string value = row.GetCell(j).ToString(); // 获取单元格内容
                            arry[i - 1, j] = value;
                        }
                        else
                        {
                            arry[i - 1, j] = "";
                        }
                    }
                }
            }

            for (int i = 1; i < sheet.LastRowNum; i++)// 遍历 Excel 中的每一行（跳过表头）
            {
                row = sheet.GetRow(i);   //读取当前行数据
                if (row != null)         //行不为空
                {
                    dataParameter.Add(new DataParameter()
                    {
                        ID = dataParameter.Count
                    });
                    for (int j = 0; j < row.LastCellNum; j++)//数组里面存的是除去第一列开始的数据
                    {
                        int temp_int;
                        float temp_float;
                        if (arry[0, j] == "序号")
                        {
                            dataParameter[dataParameter.Count - 1].ID = int.TryParse(arry[i, j], out temp_int) == true ? int.Parse(arry[i, j]) : 0;
                        }
                        if (arry[0, j] == "内容")
                        {
                            dataParameter[dataParameter.Count - 1].Name_Chinese = arry[i, j];
                        }
                        if (arry[0, j] == "Content")
                        {
                            dataParameter[dataParameter.Count - 1].Name_English = arry[i, j];
                        }
                        if (arry[0, j] == "字节数")
                        {
                            dataParameter[dataParameter.Count - 1].Byte = int.TryParse(arry[i, j], out temp_int) == true ? int.Parse(arry[i, j]) : 0;
                        }

                        if (arry[0, j] == "默认值")
                        {
                            dataParameter[dataParameter.Count - 1].DefaultValue = float.TryParse(arry[i, j], out temp_float) == true ? float.Parse(arry[i, j]) : 0F;
                        }

                        if (arry[0, j].Contains("精度"))
                        {
                            if (arry[i, j] == "0")
                            { dataParameter[dataParameter.Count - 1].coef = 1; }
                            else if (arry[i, j] == "1")
                            { dataParameter[dataParameter.Count - 1].coef = 10; }
                            else if (arry[i, j] == "2")
                            { dataParameter[dataParameter.Count - 1].coef = 100; }
                            else if (arry[i, j] == "3")
                            { dataParameter[dataParameter.Count - 1].coef = 1000; }
                            else
                            { dataParameter[dataParameter.Count - 1].coef = 1; }

                        }
                        if (arry[0, j] == "单位")
                        {
                            dataParameter[dataParameter.Count - 1].Unit = arry[i, j];
                        }
                        if (arry[0, j] == "设置范围")
                        {
                            dataParameter[dataParameter.Count - 1].SettingRange = arry[i, j];
                            if (arry[i, j] != null && arry[i, j].Contains("~"))
                            {
                                dataParameter[dataParameter.Count - 1].SettingRangeMin = float.Parse(arry[i, j].ToString().Substring(0, arry[i, j].ToString().IndexOf("~")));
                                dataParameter[dataParameter.Count - 1].SettingRangeMax = float.Parse(arry[i, j].ToString().Substring(arry[i, j].ToString().IndexOf("~") + 1));
                            }
                            if (arry[i, j] != null && arry[i, j].Contains("～"))
                            {
                                dataParameter[dataParameter.Count - 1].SettingRangeMin = float.Parse(arry[i, j].ToString().Substring(0, arry[i, j].ToString().IndexOf("～")));
                                dataParameter[dataParameter.Count - 1].SettingRangeMax = float.Parse(arry[i, j].ToString().Substring(arry[i, j].ToString().IndexOf("～") + 1));
                            }
                        }
                    }
                }
            }
        }
    }

}

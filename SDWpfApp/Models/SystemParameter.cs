using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDWpfApp.Models
{
    public class SystemParameter
    {
        public virtual int SystemParameterID { get; set; }//ID
        public virtual int CommandType { get; set; }//命令类型
        public virtual int CID2 { get; set; }//ID2
        public virtual string Description { get; set; }//描述
        public virtual string Description_US { get; set; }//英文描述
        public virtual float DefaultValue { get; set; }//默认值
        public virtual float SavedValue { get; set; }//保存值
        public virtual float SetValue { get; set; }//设置值
        public virtual int Coef { get; set; }//系数，单位转换等

        public virtual string ParameterScope { get; set; }//20200605参数值的作用域或范围

        public virtual float MaxValue { get; set; }//20200605参数允许的最大值
        public virtual float MinValue { get; set; }//20200605参数允许的最小值

        public virtual string strSavedValue { get; set; }//以字符串形式表示的SavedValue
        public virtual string strSetValue { get; set; }//以字符串形式表示的SetValue
        public virtual int Byte { get; set; }//20200806字节
    }
}

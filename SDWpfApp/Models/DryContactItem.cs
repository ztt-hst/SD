using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDWpfApp.Models
{
    public class DryContactItem
    {
        public virtual int DryContactItemID { get; set; }//ID
        public virtual string Description { get; set; }//描述
        public virtual string Description_US { get; set; }//英文描述
        public virtual string CurrentValue { get; set; }//当前值
        public virtual string SetValue { get; set; }//设置值
        public virtual int CommandType { get; set; }//命令类型
    }

    //干接点2项
    public class DryContact2Item//20200522
    {
        public virtual int DryContactItemID { get; set; }//ID
        public virtual string Description { get; set; }//描述
        public virtual string Description_US { get; set; }//英文描述
        public virtual string CurrentValue { get; set; }//当前值
        public virtual string SetValue { get; set; }//设置值
        public virtual int CommandType { get; set; }//命令类型

        public virtual string CurrentDryContactValue { get; set; }//当前干接点值
        public virtual string SetDryContactValue { get; set; }//设置干接点值
    }
}

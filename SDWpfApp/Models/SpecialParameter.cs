using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDWpfApp.Models
{
    public class SpecialParameter
    {
        public virtual int SpecialParameterID { get; set; } // ID
        public virtual string Description { get; set; }     //描述
        public virtual string Description_US { get; set; }  //英文描述
        public virtual string CurrentValue { get; set; }    //当前值
        public virtual string SetValue { get; set; }        //设置值
        public virtual int CommandType { get; set; }        //命令类型
    }
}

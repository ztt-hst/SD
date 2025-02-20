using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDWpfApp.Models
{
    //数据参数
    public class DataParameter
    {
        public virtual int ID { get; set; }
        public virtual string Name_Chinese { get; set; }
        public virtual string Name_English { get; set; }
        public virtual string Unit { get; set; }
        public virtual int Byte { get; set; }
        public virtual string SettingRange { get; set; }
        public virtual float SettingRangeMin { get; set; }
        public virtual float SettingRangeMax { get; set; }
        //public static string UploadAndRealValue { get; set; }
        public virtual int coef { get; set; }
        public virtual float DefaultValue { get; set; }
        public DataParameter()
        {

        }
    }
}

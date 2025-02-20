using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDWpfApp.Models
{
    //校准参数
    public class CalibrationParameter
    {
        public virtual int CalibrationParameterID { get; set; }//校准参数ID
        public virtual int CommandType { get; set; }//命令类型
        public virtual int CID2 { get; set; }
        public virtual string Description { get; set; }
        public virtual string Description_US { get; set; }
        public virtual int SavedCoef { get; set; }
        public virtual float ReadValue { get; set; }
        public virtual float MeasureValue { get; set; }

        public virtual string ParameterScope { get; set; }//20200605
        public virtual float MaxValue { get; set; }//20200605
        public virtual float MinValue { get; set; }//20200605
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDWpfApp.Models
{
    public class WarningItem
    {
        public virtual int WarningItemID { get; set; }//警告项ID
        public virtual string WarningInfo { get; set; }//警告信息
    }
}

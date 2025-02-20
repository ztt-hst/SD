using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDWpfApp.Models
{
    public class DeviceWarningRecord
    {
        public virtual DateTime SaveTime { get; set; }//保存时间

        public virtual int DeviceWarningRecordID { get; set; }//设备警告记录ID
        public virtual string CellVoltageWarningStatus { get; set; }//单体电池电压警告状态
        public virtual string CellVoltageProtectionStatus { get; set; }//单体电池电压保护状态
        public virtual string BatteryGroupVoltageWarningStatus { get; set; }//电池组电压警告状态
        public virtual string BatteryGroupVoltageProtectionStatus { get; set; }//电池组电压保护状态
        public virtual string CellTemperatureWarningStatus { get; set; }//单体电池温度警告状态
        public virtual string CellTemperatureProtectionStatus { get; set; }//单体电池温度保护状态
        public virtual string CHOCWarningStatus { get; set; }//充电过流的警告状态
        public virtual string CHOCProtectionStatus { get; set; }//充电过流的保护状态
        public virtual string DCHOCWarningStatus { get; set; }//放电过流的警告状态
        public virtual string DCHOCProtectionStatus { get; set; }//放电过流的保护状态
        public virtual string ShortCircuitProtectionStatus { get; set; }//短路保护状态
        public virtual string AntiTheftWarningStatus { get; set; }//防盗警告状态
    }
    public class DeviceWarningRecord_COSPOWERS
    {
        public virtual DateTime SaveTime { get; set; }//保存时间

        public virtual int DeviceWarningRecordID { get; set; }//ID
        public virtual string Warnings { get; set; }//警告
    }
}

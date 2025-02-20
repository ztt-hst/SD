using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDWpfApp.Models
{
    public class Cell
    {
        public virtual float Voltage { get; set; }
        public virtual float Temperature { get; set; }

        public virtual BalanceStatus BalanceStatus { get; set; }//平衡状态
        public virtual VoltageWarningType VoltageWarningStatus { get; set; }//电压警告状态:正常/单体欠压告警/单体过压告警/单体欠压保护/单体过压保护
        //温度警告状态:无/单体充电低温告警/单体充电高温告警/单体充电低温保护/单体充电高温保护/单体放电低温告警/单体放电高温告警/单体放电低温保护/单体放电高温保护/单体温度传感器无效告警/未定义
        public virtual TemperatureWarningType TemperatureWarningStatus { get; set; }
        public virtual Cell_WarningState_DPC VoltageWarningStatus_DPC { get; set; }//电压警告状态_DPC:无警告/下限警告/上限警告
        public virtual Cell_WarningState_DPC TemperatureWarningStatus_DPC { get; set; }//温度警告状态
        public Cell() 
        { 

        }
    }
}

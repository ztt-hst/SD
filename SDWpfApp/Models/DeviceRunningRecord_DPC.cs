using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDWpfApp.Models
{
    public class DeviceRunningRecord_DPC
    {
        public virtual int DeviceRunningRecordID { get; set; }//设备运行记录ID

        public virtual DateTime SaveTime { get; set; }//保存时间
        public virtual string CellStateEvent { get; set; }//单体电池状态事件
        public virtual string CellVoltageEvent { get; set; }//单体电池电压事件
        public virtual string CurrentEvent { get; set; }//电流事件
        public virtual string TemperatureEvent { get; set; }//温度事件
        public virtual string CapacityEvent { get; set; }//容量事件
        public virtual float BatteryCurrent { get; set; }//电池电流
        public virtual float BatteryVoltage { get; set; }//电池电压
        public virtual float ResidualCapacity { get; set; }//剩余容量

        public virtual float AmbinentTemperature { get; set; }//环境温度

        public virtual float PowerTemperature { get; set; }//功率温度

        public virtual float Cell_01_Voltage { get; set; }//单体电池x的电压
        public virtual float Cell_02_Voltage { get; set; }
        public virtual float Cell_03_Voltage { get; set; }
        public virtual float Cell_04_Voltage { get; set; }
        public virtual float Cell_05_Voltage { get; set; }
        public virtual float Cell_06_Voltage { get; set; }
        public virtual float Cell_07_Voltage { get; set; }
        public virtual float Cell_08_Voltage { get; set; }
        public virtual float Cell_09_Voltage { get; set; }
        public virtual float Cell_10_Voltage { get; set; }
        public virtual float Cell_11_Voltage { get; set; }
        public virtual float Cell_12_Voltage { get; set; }
        public virtual float Cell_13_Voltage { get; set; }
        public virtual float Cell_14_Voltage { get; set; }
        public virtual float Cell_15_Voltage { get; set; }
        public virtual float Cell_16_Voltage { get; set; }

        public virtual float Temp_01 { get; set; }//温度x
        public virtual float Temp_02 { get; set; }
        public virtual float Temp_03 { get; set; }
        public virtual float Temp_04 { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDWpfApp.Models
{
    public class DataRecord
    {
        public virtual float PowerTemperature { get; set; }//DPC
        public virtual float SOH { get; set; }//DPC
        public virtual float DischargeEnergy { get; set; }//DPC

        public virtual int PackID { get; set; }

        public virtual DateTime SaveTime { get; set; }

        public virtual float BusBarMeasureVoltage_Coslight { get; set; }
        public virtual float BatteryGroupVoltage { get; set; }
        public virtual float BatteryGroupCurrent { get; set; }
        public virtual int SOC { get; set; }
        public virtual float MOSFET_1_Coslight { get; set; }
        public virtual float MOSFET_2_Coslight { get; set; }
        public virtual float AmbinentTemperature_1_Coslight { get; set; }
        public virtual float AmbinentTemperature_2_Coslight { get; set; }

        public virtual SwitchStatus ChargeCircuitSwitchStatus { get; set; }
        public virtual SwitchStatus DischargeCircuitSwitchStatus { get; set; }
        public virtual SwitchStatus BeeperSwitchStatus { get; set; }//DPC_BalanceState
        public virtual SwitchStatus HeatingFilmSwitchStatus { get; set; }
        public virtual BatteryChargingStatus FullyChargedStatus { get; set; }
        public virtual BatteryStatus BatteryStatus { get; set; }
        public virtual BatteryChargingRequestStatus BatteryChargingRequestFlag { get; set; }



        public virtual float Cell_01_Voltage { get; set; }
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

        public virtual float Temp_01 { get; set; }
        public virtual float Temp_02 { get; set; }
        public virtual float Temp_03 { get; set; }
        public virtual float Temp_04 { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Mvvm.POCO;

namespace SDWpfApp.Models
{
    public class BatteryGroup
    {
        #region analog
        public virtual int BatteryGroupID { get; set; }//电池组ID
        public virtual ObservableCollection<WarningItem> WarningCollection { get; set; }//警告集
        public virtual ObservableCollection<Cell> CellCollection { get; set; }//单体电池集
        public virtual ObservableCollection<WarningItem> CoslightWarningCollection { get; set; }
        public virtual int SingleCellVoltageCount { get; set; }//单体电池电压数量
        public virtual int SingleCellTemperatureCount { get; set; }//单体电池温度数量
        public virtual float AverageVoltage_Coslight { get; set; }//平均电压_Coslight
        public virtual float BatteryGroupCurrent { get; set; }//电池组电流
        public virtual float BatteryGroupVoltage { get; set; }//电池组电压
        public virtual float AmbinentTemperature { get; set; }//环境温度
        public virtual float SOH { get; set; }//电池健康状态
        public virtual float SOC { get; set; }//电池SOC容量

        public virtual double SOC_ZTE { get; set; }
        public virtual double SOH_ZTE { get; set; }
        public virtual float BusBarVoltage_Coslight { get; set; }//总线电压_Coslight
        public virtual float CurrentLimit { get; set; }//电流限制
        public virtual float MaxAvailiableCapacity { get; set; }//最大可用容量
        public virtual float CycleTimes { get; set; }//循环次数
        public virtual double MaxAvailiableCapacity_ZTE { get; set; }
        public virtual int UserSelfDefineCount_Analog { get; set; }//用户自定义数量_模拟量
        public virtual float AccCHCapacity_Coslight { get; set; }//累计充电容量
        public virtual float AccDCHCapacity_Coslight { get; set; }//累计放电容量
        #endregion analog
        #region State
        public virtual int SwitchCount { get; set; }//开关数量
        public virtual SwitchStatus ChargeCircuitSwitchStatus { get; set; }//充电电路开关状态：断开/闭合/未定义
        public virtual SwitchStatus DischargeCircuitSwitchStatus { get; set; }//放电电路开关状态
        public virtual SwitchStatus BeeperSwitchStatus { get; set; }//蜂鸣器开关状态
        public virtual SwitchStatus HeatingFilmSwitchStatus { get; set; }//加热膜开关状态
        public virtual SystemState_DPC BatteryStatus_DPC { get; set; }//电池管理系统状态
        public virtual LimitCurrentStatus LimitCurrentStatus_Coslight { get; set; }//限制电流状态
        public virtual BatteryStatus BatteryStatus { get; set; }//电池状态 待机, 充电,放电, 保护,故障,未定义
        public virtual BatteryChargingStatus FullyChargedStatus { get; set; }//完全充电状态:充满/未充满/未定义
        public virtual int UserSelfDefineCount_Warning { get; set; }//用户自定义数量_警告
        public virtual int UserSelfDefineCount_State { get; set; }//用户自定义状态数量
        public virtual BatteryChargingRequestStatus BatteryChargingRequestFlag { get; set; }//电池充电需求状态
        #endregion State

        public BatteryGroup() 
        {
            CellCollection = new ObservableCollection<Cell>();
            WarningCollection = new ObservableCollection<WarningItem>();
            CoslightWarningCollection = new ObservableCollection<WarningItem>();
        }
        public void OnSingleCellVoltageCountChanged()
        {
            if (SingleCellVoltageCount > SingleCellTemperatureCount) 
            {
                CreateCellClooection(SingleCellVoltageCount);
            }
            //取两者之大
            //CreateCellClooection(Math.Max(SingleCellVoltageCount, SingleCellTemperatureCount));
        }
        //单体电池温度数量变化
        public void OnSingleCellTemperatureCountChanged()
        {
            //CreateCellClooection(Math.Max(SingleCellVoltageCount, SingleCellTemperatureCount));
            if (SingleCellTemperatureCount > SingleCellVoltageCount)
            {
                CreateCellClooection(SingleCellTemperatureCount);
            }
        }
        //清空现有的 CellCollection 集合，并根据 count 参数创建新的 Cell 实例，添加到集合
        private void CreateCellClooection(int count)
        {
            int oldcount = CellCollection.Count;
            if (oldcount > 0)
            {
                if(count>oldcount)
                {
                    for(int i = oldcount; i < count; i++)
                    {
                        CellCollection.Add(ViewModelSource.Create(() => new Cell { CellID = i + 1 }));
                    }
                }
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    CellCollection.Add(ViewModelSource.Create(() => new Cell { CellID = i + 1 }));
                }
            }         
        }

    }
}

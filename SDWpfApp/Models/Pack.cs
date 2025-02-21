using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Mvvm.POCO;
using System.Collections.ObjectModel;
using DevExpress.Map.Native;
using System.ComponentModel;
using SDWpfApp.ViewModels;

namespace SDWpfApp.Models
{
    public class Pack 
    {
        private MainViewModel _mainViewModel; // 添加 MainViewModel 的引用

        public Pack(MainViewModel mainViewModel) // 修改构造函数
        {
            _mainViewModel = mainViewModel; // 初始化引用
            BatteryGroupCollection = new ObservableCollection<BatteryGroup>();
        }

        public virtual float PowerTemperature { get; set; } //表示功率温度
        public virtual float DischargeEnergy { get; set; } //表示放电能量
        #region analog
        public virtual int BatteryGroupCount { get; set; }  //电池组数量
        public virtual int VoltageDataCount_Coslight { get; set; }//Coslight电压数据数量
        public virtual float BusBarVoltage_Coslight { get; set; }//Coslight总线电压数量
        public virtual float SingleCellTemperaturePowerVoltage_Coslight { get; set; }//Coslight单体电池温度测量模块供电电压
        public virtual float EEPromPowerVoltage_Coslight { get; set; }//Coslight EEPROM 存储模块的供电电压
        public virtual ObservableCollection<BatteryGroup> BatteryGroupCollection { get; set; } //电池组集
        public virtual float AverageVoltage_Coslight { get; set; }//Coslight平均电压
        public virtual float BatteryGroupCurrent { get; set; }//电池组电流
        public virtual float BatteryGroupVoltage { get; set; }//电池组电压
        public virtual float SOC { get; set; }//电池SOC
        public virtual double SOC_ZTE { get; set; }//中兴系统的电池组荷电状态
        public virtual string ResidualDischargeTime { get; set; } = "--";//剩余放电时间
        public virtual string ResidualChargeTime { get; set; } = "--";//剩余充电时间
        public virtual float CommunicationPowerVoltage_Coslight { get; set; }//通信模块的供电电压
        public virtual float BusBarMeasureVoltage_Coslight { get; set; }//总线测量电压
        public virtual float DischargeMOSDriveVoltage_Coslight { get; set; }//放电 MOSFET 的驱动电压
        public virtual float CellTemperatureCount_Coslight { get; set; }//单体电池温度计数
        public virtual float MOSFET_1_Coslight { get; set; }//？？？
        public virtual float MOSFET_2_Coslight { get; set; }
        public virtual float AmbinentTemperature_1_Coslight { get; set; }//环境温度x的测量值
        public virtual float AmbinentTemperature_2_Coslight { get; set; }
        public virtual float SOH { get; set; }//电池组健康状态
        public virtual double SOH_ZTE { get; set; }//20200727中兴系统的电池组健康状态
        public virtual float CurrentLimitingTemperature_Coslight { get; set; }//当前限制温度
        public virtual float AccCHCapacity_Coslight { get; set; }//累计充电容量
        public virtual float AccDCHCapacity_Coslight { get; set; }//累计放电容量  

        #endregion analog
        #region State
        public virtual int SwitchCount_Coslight { get; set; }//Coslight开关数量
        public virtual PowerState SingleCellTemperaturePowerStatus_Coslight { get; set; }//单体电池温度供电状态
        public virtual PowerState EEPROMPowerStatus_Coslight { get; set; }//EEPROM供电状态
        public virtual PowerState CommunicationPowerStatus_Coslight { get; set; }//通信模块的供电状态
        public virtual PowerState BusBarMeasurePowerStatus_Coslight { get; set; }//总线测量
        public virtual KeyStatus PowerOnSwitchKeyStatus_Coslight { get; set; }//开机按键的状态
        public virtual PreChargeStatus PreChargeStatus_Coslight { get; set; }//预充电状态
        public virtual LimitCurrentStatus LimitCurrentStatus_Coslight { get; set; }//限流功能的状态

        #endregion State
        #region Warning
        public virtual MOSProtectionType MOSProtectionStatus { get; set; }//20200528MOSFET 的保护状态
        public virtual WarningType AntiThief { get; set; }//防盗警告状态
        public virtual WarningType PackCH_OC_Level2_Protection { get; set; }//电池组充电二级过流保护状态
        public virtual WarningType PackCH_OC_Level3_Protection { get; set; }//电池组充电三级过流保护状态
        public virtual WarningType PackDCH_OC_Level2_Protection { get; set; }//电池组放电二级过流保护状态
        public virtual WarningType SOCLowWarning { get; set; }//SOC（荷电状态）低的告警状态。
        public virtual WarningType SOCLowProtection { get; set; }//SOC（荷电状态）低的保护状态。
        public virtual WarningType1 PrechargeFailure { get; set; }//预充电失败状态
        public virtual WarningType PoleBackward { get; set; }//电池极性反接状态
        public virtual WarningType VoltageSampleFailure { get; set; }//电压采样故障状态
        public virtual WarningType CellFailure { get; set; }//电池单体失效状态
        public virtual WarningProtectType SOHLowStatus { get; set; }//20200528电池健康状态（SOH）低的告警或保护状态
        #endregion Warning
        public virtual byte PackID { get; set; }//
        public string SystemName { get; set; }//系统名称
        public string SoftwareVersion { get; set; }//软件版本
        public string BatteryModel { get; set; }//电池模型
        public string BMSModel { get; set; }//BMS模型
        public virtual MOSWarningType MOSWarningStatus { get; set; }//20200528MOSFET 的警告状态
        public string ManufacturerName { get; set; }//生产厂家名字
        public string ProductSN { get; set; }//产品SN号
        public virtual WarningType CellConsistencyProtection { get; set; }//20200528电池单体一致性保护状态
        public string BatteryType { get; set; }       // 电池类型
        public string ManufacturerCode { get; set; }  // 生产厂家编码
        //public virtual bool IsCommunicationEnabled { get; set; }    //通信是否使能
        private bool _isCommunicationEnabled;

        public virtual bool IsCommunicationEnabled
        {
            get { return _isCommunicationEnabled; }
            set 
            { 
                if (_isCommunicationEnabled != value)
                {
                    Console.WriteLine("IsCommunicationEnabled change");
                    _isCommunicationEnabled = value;
                    OnIsCommunicationEnabledChanged();
                    Console.WriteLine("OnIsCommunicationEnabledChanged finish");
                }
            }
        }
        public int CommunicationFailureCount { get; set; }      //通讯失败计数
        public virtual CommunicationStatus CommunicationStatus { get; set; }//通信的具体状态
        //public virtual bool IsCommunicationEnable { get; set; }//
        public virtual DateTime RefreshTime { get; set; }//刷新时间

        public static bool IsEnglishUI { get; set; }//20200423英文显示

        public static bool IsChineseUI { get; set; }//20200423中文显示
        public DateTime BMSTime { get; internal set; }//BMS时间

        public void OnIsCommunicationEnabledChanged()
        {
            if (!IsCommunicationEnabled)
            {
                if (IsChineseUI)
                {
                    CommunicationStatus = CommunicationStatus.未使能;
                }
                if (IsEnglishUI)
                {
                    CommunicationStatus = CommunicationStatus.Disable;
                }
                CommunicationFailureCount = 0;
            }
            else
            {
                _mainViewModel.SimulateTestMessage(PackID); // 使用实例调用
                SerialPortCommunicator.IsCommunicationFirst = true;
            }         
        }
         //当 BatteryGroupCount 改变时，重新生成电池组集合
        public void OnBatteryGroupCountChanged()
        {
            this.BatteryGroupCollection.Clear();

            for (int i = 0; i != BatteryGroupCount; i++)
            {
                BatteryGroupCollection.Add(ViewModelSource.Create(() => new BatteryGroup { BatteryGroupID = i + 1 }));
            }
        }

    }
}

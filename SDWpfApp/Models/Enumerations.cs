using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDWpfApp.Models
{
    public enum WarningType
    {
        正常 = 0x00,
        告警 = 0x01,
        未定义 = 0x02,
        OK = 0x03,
        Warning = 0x04,
        Undefined = 0x05
    }
    public enum AuthorityResult
    {
        RegisterError_NoRegisterKey,
        RegisterError_NoRegisterFile,
        RegisterError_RegisterCodeNotMatch,
        RegisterOK
    }
    public enum SOC_WarningState_DPC
    {
        剩余容量告警 = 0x00,
        BMS_Failure = 0x10,
        SOC_Warning = 0x12
    }
    public enum BalanceEvent_DPC
    {
        均衡模块开启 = 0x00,
        电芯压差告警 = 0x04,
        充电MOS故障告警 = 0x05,
        放电MOS故障告警 = 0x06,
        Balance_Module_Open = 0x10,
        Cell_PressDiff = 0x14,
        Ch_MOS = 0x15,
        Dh_MOS = 0x16
    }
    public enum AuthorizationManager
    {
        中兴运维版,
        中兴用户版,
        中兴管理版,
        中兴测试版,
        开发版
    }
    public enum LimitCurrentStatus
    {
        未启动 = 0,
        启动 = 1,
        未定义 = 2,
        NotStart = 3,
        Working = 4,
        Undefined = 5
    }
    public enum IindicatorLightSwtich
    {
        蜂鸣器声指示,
        LCD显示功能,
        Buzzer_Sound_Indication = 0x10,
        LCD_Display_Function = 0x11
    }
    public enum PowerState
    {
        断电 = 0x00,
        供电 = 0x01,
        未定义 = 0x02,
        PowerOff = 0x03,
        PowerOn = 0x04,
        Undefined = 0x05
    }
    public enum MOSProtectionType//20200528
    {
        正常 = 0x00,
        MOS充电过温保护 = 0x01,
        MOS放电过温保护 = 0x02,
        未定义 = 0x03,
        OK = 0x04,
        MOS_CH_OT_Protection = 0x05,
        MOS_DCH_OT_Protection = 0x06,
        Undefined = 0x07
    }
    public enum PreChargeStatus
    {
        未启动预放 = 0,
        预放成功 = 1,
        预放失败 = 2,
        正在预放 = 8,
        未定义 = 0x09,
        NotStart = 0x10,
        Success = 0x11,
        Failed = 0x12,
        Predischarging = 0x18,
        Undefined = 0x19
    }
    public enum KeyStatus
    {
        按下 = 0x00,
        弹起 = 0x01,
        未定义 = 0x02,
        KeyDown = 0x03,
        KeyUp = 0x04,
        Undefined = 0x05
    }
    public enum CellStateEvent_DPC
    {
        电池组失效告警 = 0x04,
        Cell_Failure_Alarm_Function = 0x14
    }
    public enum CapacityEvent_DPC
    {
        剩余容量告警 = 0x02,
        SOC_Warning = 0x12
    }
    public enum CapacityAndOtherSwtich
    {
        剩余容量告警,
        间歇式的充电,
        外部开关控制,
        静态待机休眠,
        历史数据记录,
        限流模式,
        Residual_Capacity_Alarm_Function = 0x10,
        Intermittent_CH_Function = 0x11,
        External_Switch_Control_Function = 0x12,
        Static_Standby_Sleep_Function = 0x13,
        Historical_Data_Recording_Function = 0x14,
        LimitCurrent_Function = 0x15
    }
    public enum BatteryStatus
    {
        待机 = 0x00,
        充电 = 0x01,
        放电 = 0x02,
        保护 = 0x03,
        故障 = 0x04,
        未定义 = 0x05,
        Standby = 0x06,
        Charging = 0x07,
        DisCharging = 0x08,
        Protection = 0x09,
        Fault = 0x0A,
        Undefined = 0x0B
    }
    public enum BatteryChargingStatus
    {
        未充满 = 0x00,
        充满 = 0x01,
        未定义 = 0x02,
        NotFull = 0x03,
        Full = 0x04,
        Undefined = 0x05
    }
    public enum BatteryChargingRequestStatus
    {
        不需调压 = 0x00,
        需要调压 = 0x01,
        未定义 = 0x02,
        No = 0x03,
        Yes = 0x04,
        Undefined = 0x05
    }
    public enum CurrentEvent_DPC
    {
        充电电流告警,
        充电过流保护,
        放电电流告警,
        放电过流保护,
        二级过流保护,
        输出短路保护,  // modify by sunlw 2020-11-3 13:18
        二级过流锁定,
        输出短路锁定,
        CH_OC_Warning = 0x10,
        CH_OC_Protection = 0x11,
        DCH_OC_Warning = 0x12,
        DCH_OC_Protection = 0x13,
        OC_Protection_Level2 = 0x14,
        Short_Circuit_Protection_Or_Pack_Pole_Backward = 0x15,
        OC_Locked = 0x16,
        Short_Circuit_Locked = 0x17
    }
    public enum BootLaoderStatus
    {
        未连接,

        应用程序连接中,
        应用程序已连接,

        BootLoader连接中,
        BootLoader已连接,

        更新中,
        更新完成,
        终止更新,
        应用程序,

        一键擦除中
    }
    public enum BootLoaderFileNameCheck
    {
        OK = 0x00,

        文件格式错误_加载文件 = 0x01,
        光宇代码错误_加载文件 = 0x2,
        项目代码错误_加载文件 = 0x3,
        电芯材料错误_加载文件 = 0x4,

        光宇代码错误 = 0x5,
        项目代码错误 = 0x6,
        电芯材料错误 = 0x7,

        项目代码不一致 = 0x8,
        奇偶校验错误 = 0x9,
        文件名长度错误_加载文件 = 0xA,

        CPU型号不一致 = 0x0B,
        其他错误 = 0x0C,
        版本号不一致 = 0x0D,

        压缩文件错误 = 0x0E,

        File_Format_Write = 0x11,
        COSPower_Code_Write = 0x12,
        Project_Code_Write = 0x13,
        Cell_Material_Write = 0x14,

        COSPower_Code_Read = 0x15,
        Project_Code_Read = 0x16,
        Cell_Material_Read = 0x17,

        Project_Code_Inconsistent = 0x18,
        Parity = 0x19,
        File_Name_Length_Write = 0x1A,

        CPU_Mode_lInconsistent = 0x1B,
        Other = 0x1C,
        VersionNumber1_lInconsistent = 0x1D,
        Zip_File = 0x1E
    }
    public enum WarningType1
    {
        正常 = 0x01,
        告警 = 0x00,
        未定义 = 0x02,
        OK = 0x04,
        Warning = 0x03,
        Undefined = 0x05
    }
    public enum WarningProtectType//20200626
    {
        正常 = 0x00,
        SOH低告警 = 0x01,
        SOH低保护 = 0x02,
        未定义 = 0x03,
        OK = 0x04,
        SOH_Low_Warning = 0x05,
        SOH_Low_Protection = 0x06,
        Undefined = 0x07
    }
    public enum MOSWarningType//20200528
    {
        正常 = 0x00,
        MOS充电过温告警 = 0x01,
        MOS放电过温告警 = 0x02,
        未定义 = 0x03,
        OK = 0x04,
        MOS_CH_OT_Protection = 0x05,
        MOS_DCH_OT_Protection = 0x06,
        Undefined = 0x07
    }

    public enum TemperatureEvent_DPC
    {
        充电高温告警 = 0x00,
        充电高温保护 = 0x01,
        充电低温告警 = 0x02,
        充电低温保护 = 0x03,
        放电高温告警 = 0x04,
        放电高温保护 = 0x05,
        放电低温告警 = 0x06,
        放电低温保护 = 0x07,
        环境高温告警 = 0x08,
        环境高温保护 = 0x09,
        环境低温告警 = 0x0A,
        环境低温保护 = 0x0B,
        功率过温保护 = 0x0C,
        消防告警事件 = 0x0D,
        CH_OT_Warning = 0x10,
        CH_OT_Protection = 0x11,
        CH_BT_Warning = 0x12,
        CH_BT_Protection = 0x13,
        DCH_OT_Warning = 0x14,
        DCH_OT_Protection = 0x15,
        DCH_BT_Warning = 0x16,
        DCH_BT_Protection = 0x17,
        Ambient_OT_Warning = 0x18,
        Ambient_OT_Protection = 0x19,
        Ambient_BT_Warning = 0x1A,
        Ambient_BT_Protection = 0x1B,
        Power_OT_Protection = 0x1C,
        Cell_BT_Heating = 0x1D
    }
    public enum BalanceSwtich
    {
        电池均衡 = 0x00,
        静态均衡 = 0x01,
        静态均衡定时NC = 0x02,
        均衡温度限制 = 0x03,
        //电芯失效告警 = 0x04,
        AFE失效告警 = 0x04,
        电池组失效告警 = 0x05,
        电流传感器失效告警 = 0x06,
        Battery_Balance_Function = 0x10,
        Static_Balance_Function = 0x11,
        Static_Balance_Timing_Function = 0x12,
        Balance_Temperature_Limit_Function = 0x13,
        Cell_Failure_Alarm_Function = 0x14,
        BatteryGroup_Failure_Alarm_Function = 0x15,
        CurrentSensor_Failure_Alarm_Function = 0x16
    }

    public enum VoltageEvent_DPC
    {
        单体过压告警 = 0x00,
        单体过压保护 = 0x01,
        单体欠压告警 = 0x02,
        单体欠压保护 = 0x03,
        总压过压告警 = 0x04,
        总压过压保护 = 0x05,
        总压欠压告警 = 0x06,
        总压欠压保护 = 0x07,
        Cell_OV_Warning = 0x00 + 0x10,
        Cell_OV_Protect = 0x01 + 0x10,
        Cell_BV_Warning = 0x02 + 0x10,
        Cell_BV_Protect = 0x03 + 0x10,
        BB_OV_Warning = 0x04 + 0x10,
        BB_OV_Protect = 0x05 + 0x10,
        BB_BV_Warning = 0x06 + 0x10,
        BB_BV_Protect = 0x07 + 0x10
    }

    public enum TemperatureWarningType//20200422
    {
        无告警 = 0x00,
        单体充电低温告警 = 0x01,
        单体充电高温告警 = 0x02,
        单体充电低温保护 = 0x03,
        单体充电高温保护 = 0x04,
        单体放电低温告警 = 0x05,
        单体放电高温告警 = 0x06,
        单体放电低温保护 = 0x07,
        单体放电高温保护 = 0x08,
        单体温度传感器无效告警 = 0x09,
        未定义 = 0x0A,
        Ok = 0x10,
        Cell_CHBTWarning = 0x11,
        Cell_CHOTWarning = 0x12,
        Cell_CHBTProtection = 0x13,
        Cell_CHOTProtection = 0x14,
        Cell_DCHBTWarning = 0x15,
        Cell_DCHOTWarning = 0x16,
        Cell_DCHBTProtection = 0x17,
        Cell_DCHOTProtection = 0x18,
        Cell_TempSensorFault = 0x19,
        Undefined = 0x1A
    }

    public enum VoltageWarningType//20200422
    {
        正常 = 0x00,
        单体欠压告警 = 0x01,
        单体过压告警 = 0x02,
        单体欠压保护 = 0x03,
        单体过压保护 = 0x04,
        未定义 = 0x05,
        Ok = 0x06,
        Cell_BV = 0x07,
        Cell_OV = 0x08,
        Cell_BV_Protect = 0x09,
        Cell_OV_Protect = 0x0A,
        Undefined = 0x0B
    }

    public enum TOPBAND_DPC//TOPAND自定义状态
    {
        电池组失效 = 0x00,
        电流传感器失效 = 0x01,
        温度传感器失效 = 0x02,
        AFE失效 = 0x03,
        反接保护 = 0x04,
        BatteryGroup_Failure = 0x10,
        CurrentSensor_Failure = 0x11,
        TemperatureSensor_Failure = 0x12,
        AFE_Failure = 0x13,
        Reverse_Relay_Protection = 0x14
    }

    public enum BalanceStatus
    {
        Off = 0,
        Active = 1
    }


    public enum CID2_Type
    {
        获取遥测量信息,
        获取遥信量信息,
        遥控命令,
        获取遥调量信息,
        设定遥调量信息,
        获取通信协议版本号,
        获取设备厂商信息,
        获取历史数据,
        获取时间,
        同步时间,
        生产校准,
        生产设置,
        定时记录
    }
    public enum SwitchStatus
    {
        断开 = 0x00,
        闭合 = 0x01,
        未定义 = 0x02,
        Open = 0x03,
        Closed = 0x04,
        Undefined = 0x05
    }
    public enum SystemState_DPC
    {
        放电,
        充电,
        待机 = 0x02,
        静置 = 0x03,
        保护 = 0x04,
        故障 = 0x05,

        Discharging = 0x10,
        Charging = 0x11,
        Waiting = 0x12,
        Standby = 0x13,
        Fault = 0x14,
        Undefined = 0x15
    }
    public enum Cell_WarningState_DPC
    {
        无告警 = 0x00,
        下限告警 = 0x01,
        上限告警 = 0x02,
        其他 = 0x0F,
        ok = 0x00 + 0x10,
        Lower_Limit_Alarm = 0x01 + 0x10,
        Upper_Limit_Alarm = 0x02 + 0x10,
        other = 0x0F + 0x10
    }

    public enum CommunicationType
    {
        ZTE邮电,
        移动,
        DPC,
        CT
    }
    public enum CommunicationStatus
    {
        未使能 = 0x00,
        通讯正常 = 0x01,
        通讯中断 = 0x02,
        Disable = 0x03,
        OK = 0x04,
        Interrupt = 0x05
    }
    public enum CommandType
    {
        获取厂家信息 = 0x01,
        读取模拟量 = 0x02,
        读取光宇模拟量 = 0x03,
        读取开关量 = 0x04,
        读取光宇开关量 = 0x05,
        读取告警状态 = 0x06,
        读取光宇告警状态 = 0x07,
        读取系统参数 = 0x08,
        读取光宇系统参数 = 0x09,
        设置系统参数 = 0x0A,
        设置光宇系统参数 = 0x0B,
        读取系统特殊参数 = 0x0C,
        设置系统特殊参数 = 0x0D,
        读取设备存储运行数据 = 0x0E,
        读取设备存储告警数据 = 0x0F,
        控制充放电回路 = 0x10,
        读取干接点使能状态 = 0x11,
        设置干接点使能状态 = 0x12,
        设置产品序列号 = 0x13,
        读取4G参数 = 0x14,
        设置4G参数 = 0x15,
        读取GPS参数 = 0x16,
        设置GPS参数 = 0x17,
        读取4G_GPS特殊参数 = 0x18,
        设置4G_GPS特殊参数 = 0x19,
        设置BootLoader参数 = 0x1B,
        设置BootLoader文件名 = 0x1C,
        应用程序连接 = 0x1D,
        BootLoader连接 = 0x1E,
        读取GPS实时数据 = 0x1F,
        读取干接点2状态 = 0x20,
        读取陀螺仪实时数据 = 0x21,
        读取光宇特殊参数 = 0x22,
        BootLoader擦除 = 0x23,
        读取设备信息 = 0x24,
        读取设备存储事件数据 = 0x25,
        读取防盗状态 = 0x26,
        设置防盗参数 = 0x27,
        读取防盗参数 = 0x28,
        设置BootLoader条数 = 0x29
    }
}

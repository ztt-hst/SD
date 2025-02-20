using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDWpfApp.Models
{
   public class DeviceEventRecord
    {
        public virtual DateTime SaveTime { get; set; }//事件记录时间

        public virtual int DeviceEventRecordID { get; set; }//设备事件记录ID
        public virtual string EventType { get; set; }//事件类型
        public virtual string EventState { get; set; }//事件状态

        public virtual string CID2 { get; set; }//命令标识
        public virtual string CommandType { get; set; }//命令类型
        public virtual string CommandValue { get; set; }//命令值
        //保存上次发送的命令信息
        public string SaveLastSendCommandInfo(byte[] message)
        {
            //上次发送命令类型
            string LastSendCommandType = "";
            //根据message[16]和message[17]的值来确定命令类型
            if (message[16] == 0x46)
            {
                if (message[17] == 0x42)
                {
                    LastSendCommandType = "读取模拟量";
                }
                else if (message[17] == 0x43)
                {
                    LastSendCommandType = "读取开关量";
                }
                else if (message[17] == 0x44)
                {
                    LastSendCommandType = "读取告警状态";
                }
                else if (message[17] == 0x45)
                {
                    LastSendCommandType = "控制充放电回路";
                }
                else if (message[17] == 0x47)
                {
                    LastSendCommandType = "读取系统参数";
                }
                else if (message[17] == 0x49)
                {
                    LastSendCommandType = "设置系统参数";
                }
                else if (message[17] == 0x51)
                {
                    LastSendCommandType = "获取厂家信息";
                }
                else if (message[17] == 0x80)
                {
                    LastSendCommandType = "读取系统特殊参数";
                }
                else if (message[17] == 0x93)
                {
                    LastSendCommandType = "读取光宇特殊参数";
                }
                else if ((message[17] == 0x94))
                {
                    LastSendCommandType = "设置系统特殊参数";
                }
                else if (message[17] == 0x81 || message[17] == 0x8E)
                {
                    LastSendCommandType = "设置系统特殊参数";
                }
                else if (message[17] == 0x84)
                {
                    LastSendCommandType = "读取设备存储运行数据";
                }
                else if ((message[17] == 0x85) || (message[17] == 0x90))
                {
                    LastSendCommandType = "读取设备存储告警数据";
                }
                else if (message[17] == 0x88)
                {
                    LastSendCommandType = "读取光宇模拟量";
                }
                else if (message[17] == 0x89)
                {
                    LastSendCommandType = "读取光宇开关量";
                }
                else if (message[17] == 0x8A)
                {
                    LastSendCommandType = "读取光宇告警状态";
                }
                else if (message[17] == 0x8B)
                {
                    LastSendCommandType = "读取光宇系统参数";
                }
                else if (message[17] == 0x8C)
                {
                    LastSendCommandType = "设置光宇系统参数";
                }
                else if (message[17] == 0x8D)
                {
                    LastSendCommandType = "读取干接点使能状态";
                }
                else if (message[17] == 0x91)
                {
                    LastSendCommandType = "读取干接点2状态";
                }
                else if (message[17] == 0x8F)
                {
                    LastSendCommandType = "设置产品序列号";
                }
            }
            else if (message[16] == 0x47)
            {
                if (message[17] == 0xA1)
                {
                    LastSendCommandType = "读取4G参数";
                }
                else if (message[17] == 0x21)
                {
                    LastSendCommandType = "读取设备信息";
                }
                else if (message[17] == 0xA0)
                {
                    LastSendCommandType = "读取GPS参数";
                }
                else if (message[17] == 0xA6)
                {
                    LastSendCommandType = "读取GPS实时数据";
                }
                else if (message[17] == 0xA4)
                {
                    LastSendCommandType = "读取4G_GPS特殊参数";
                }
                else if (message[17] == 0xA2)
                {
                    LastSendCommandType = "设置GPS参数";

                }
                else if (message[17] == 0xA3)
                {
                    LastSendCommandType = "设置4G参数";
                }
                else if (message[17] == 0xA5)
                {
                    LastSendCommandType = "设置4G_GPS特殊参数";
                }
                else if (message[17] == 0x90)
                {
                    LastSendCommandType = "应用程序连接";
                }
                else if (message[17] == 0x93)
                {
                    LastSendCommandType = "BootLoader连接";
                }
                else if (message[17] == 0x94)
                {
                    LastSendCommandType = "BootLoader擦除";
                }
                else if (message[17] == 0x91)
                {
                    LastSendCommandType = "设置BootLoader文件名";
                }
                else if (message[17] == 0x92)
                {
                    LastSendCommandType = "设置BootLoader参数";
                }
                else if (message[17] == 0x20)
                {
                    LastSendCommandType = "读取陀螺仪实时数据";
                }
                else if (message[17] == 0x23)
                {
                    LastSendCommandType = "读取设备存储事件数据";
                }
                else if (message[17] == 0x24)
                {
                    LastSendCommandType = "读取防盗状态";
                }
                else if (message[17] == 0x25)
                {
                    LastSendCommandType = "设置防盗参数";
                }
            }
            return LastSendCommandType;
        }
        //返回命令类型
        public string ReturnCommandType(byte[] message)
        {
            //系统参数类型数组 31
            string[] SystemParameterType = new string[] {"单体过压告警阈值(V)","单体欠压告警阈值(V)","单体过压保护阈值(V)","单体欠压保护阈值(V)",
            "电池组过压告警阈值(V)","电池组欠压告警阈值(V)","电池组过压保护阈值(V)","电池组欠压保护阈值(V)",
            "充电温度高告警阈值(℃)","充电温度低告警阈值(℃)","充电温度高保护阈值(℃)","充电温度低保护阈值(℃)",
            "放电温度高告警阈值(℃)","放电温度低告警阈值(℃)","放电温度高保护阈值(℃)","放电温度低保护阈值(℃)",
            "充电过流告警阈值(C10)","充电过流保护阈值(C10)","放电过流保护阈值(C10)","放电过流告警阈值(C10)",
            "电池组欠压保护恢复阈值(V)","单体过压保护恢复阈值(V)","单体欠压保护恢复阈值(V)",
            "充电过温保护恢复阈值(℃)","放电过温保护恢复阈值(℃)","充电低温保护恢复阈值(℃)","放电低温保护恢复阈值(℃)",
            "单体落后告警电压差阈值(V)","环境温度高告警阈值(℃)","环境温度低告警阈值(℃)","防盗时间设置(min)"};
            //系统参数ID
            byte[] SystemParameterID = new byte[] { 0x80, 0x81, 0x82, 0x83, 0x84, 0x85, 0x86, 0xC4, 0xC5, 0xC6, 0xC7, 0xC8, 0xC9, 0xCA, 0xCB, 0xCC, 0xCD, 0xCE, 0xCF, 0xD0, 0xD1, 0xD2, 0xD3, 0xD4, 0xD5, 0xD6, 0xD7, 0xD8, 0xD9, 0xDA, 0xDB };
            //使能参数类型
            string[] FeLiParameterType = new string[] { "电池组标称容量（A,0x）", "短路保护使能","电池组充电过压保护使能","电池组放电欠压保护使能","单体充电过压保护使能","单体放电欠压保护使能",
            "充电过温保护使能","放电过温保护使能","充电低温保护使能","放电低温保护使能","蜂鸣器使能","加热垫使能"};
            //使能参数ID
            byte[] FeLiParameterID = new byte[] { 0x80, 0x88, 0x89, 0x8A, 0x8B, 0x8C, 0x8D, 0x8E, 0x8F, 0x90, 0x91, 0x92 };
            //Coslight电池参数类型
            string[] CoslightParameterType = new string[] { "单体过压告警恢复阈值(V)", "单体欠压告警恢复阈值(V)", "电池组过压告警恢复阈值(V)", "电池组欠压告警恢复阈值(V)", "电池组过压保护恢复阈值(V)", "充电温度高告警恢复阈值(℃)", "充电温度低告警恢复阈值(℃)",
            "放电温度高告警恢复阈值(℃)", "放电温度低告警恢复阈值(℃)", "充电过流告警恢复阈值(C10)", "放电过流告警恢复阈值(C10)", "单体落后告警电压差恢复阈值(V)", "环境温度高告警恢复阈值(℃)", "环境温度低告警恢复阈值(℃)", "额定容量(用于设置)",
            "累计充电容量", "累计放电容量", "电池组编号", "均衡启动电压", "均衡停止电压", "均衡启动压差", "充电截止电压", "充电截止电流", "SO,0x单体电池数量", "单体温度数量", "电池进入低功耗时间(秒)", "数据存储时间间隔(秒)",
            "RTC","单体电压系数","母线电压系数","充电电流系数","放电电流系数","单体温度系数","环境温度系数","MOS温度系数","SOC",
                "SOC低告警值","SOC低保护值","短路保护恢复次数设置","充电过流保护恢复次数","放电过流保护恢复次数","充电过流保护恢复值(℃)","放电过流保护恢复值(℃)","SOC低告警恢复值(℃)",
            "SOC低保护恢复值(℃)","SO,0x低告警值(℃)","SO,0x低告警恢复值(℃)","SO,0x低保护值(℃)","SO,0x低保护恢复值(℃)","电芯一致性差保护电压差阈值(V)","电芯一致性差保护电压差恢复阈值(V)",
            "电芯损坏保护值(V)","电芯损坏保护恢复值(V)","补充电电压(V)","补充电SOC(%)","均衡停止压差(V)","欠压保护关机时间(s)","充电过流限流时间(min)","放电过流保护恢复时间(s)","短路反接保护恢复时间(s)","GPS位移(m)"};
            //Coslight电池参数ID
            byte[] CoslightParameterID = new byte[] { 0x80,0x81,0x84,0x85,0x86,0xC5,0xC6,0xC9,0xCA,0xCD,0xD0,0xD8,0xD9,0xDA,0xDB,0xDC,0xDD,0xDE,0xDF,0xE0,0xE1,0xE2,0xE3,0xE4,0xE5,0xE6,0xE7,0xE8,0xE9,0xEA,0xEB,0xEC,0xED,0xEE,0xEF,0xF0,0xF1,
                0xF2,0xF3,0xF4,0xF5,0xF6,0x11,0x12,0x13,0x14,0x15,0x16,0x16,0x17,0x18,0x1A,0x1B,0x1C,0x1D,0x1E,0x1F,0x21,0x22,0x23,0x24,0x25};
            //故障类型
            string[] DryContactSetType = new string[] {"充电过压告警","充电过压保护","放电欠压告警","放电欠压保护","单体过压告警",
            "单体过压保护","单体欠压告警","单体欠压保护","充电一级过流告警","充电二级过流告警","放电过流告警","放电一级过流保护","放电二级过流保护",
            "充电高温保护","放电高温告警","放电高温保护","充电低温保护","放电低温告警","放电低温保护","环境温度高告警","环境温度低告警","电池容量低告警",
            "电池容量低保护","短路反接保护","系统失效","SO,0x告警使能","SO,0x保护使能","电芯一致性差告警使能","电芯一致性差保护","电芯损坏保护","充电过流保护",
            "电池被盗","单体高温告警","单体高温保护","短路保护","反接保护","电芯损坏","BMS故障" };
            //故障ID
            byte[] DryContactSetID = new byte[] { 0x88,0x89,0x8A,0x8B,0x8C,0x8D,0x8E,0x8F,0x90,0x91,0x92,0x93,0x94,0x95,0x96,0x97,0x98,0x99,0x9A,0x9B,0x9C,0x9D,
                0x9E,0x9F,0xA0,0xA2,0xA3,0xA4,0xA5,0xA6,0xA7,0xA8,0xA9,0xAA,0xAB,0xAC,0xAD,0xAE};
            //防盗参数类型
            string[] AgainstTheftParameterType = new string[] { "防盗总开关", "防盗使能_通讯线", "防盗使能_陀螺仪", "防盗使能_防盗线", "防盗使能_GPS", "布防方式", "一键布防", "通讯线断告警延时", "GPS告警位移", "陀螺仪告警倾角", "防盗线告警延时", "电池包初始状态" };
            //防盗参数ID
            byte[] AgainstTheftParameterID = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x0C, 0x0D, 0x0E, 0x0F, 0x10 };
            //GPS参数/ID
            string[] GPSPArameterType = new string[] { "站ID", "站经度", "站纬度", "站高度", "时区" };
            byte[] GPSPArameterID = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x07 };
            //管理员信息
            string[] WlanPArameterType = new string[] { "管理员手机号", "手机号1", "手机号2", "手机号3", "手机号4", "手机号5", "心跳周期", "APN", "APN用户名", "APN密码" };
            byte[] WlanPArameterID = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x0B, 0x0C, 0x0D };

            string[] WlanEnableType = new string[] { "4G休眠开关", "GPS休眠开关", "GPS防盗", "通讯超时防盗", "报警蜂鸣器", "报警短信", "报警测试" };
            byte[] WlanEnableID = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07 };

            string commandType = "";
            //如果message[16] == 0x46 && message[17] == 0x49则命令参数为设置系统参数
            if (message[16] == 0x46 && message[17] == 0x49)
            {
                for (int i = 0; i < SystemParameterID.Length; i++)
                {
                    //根据message[19]判断设置哪个系统参数
                    if (message[19] == SystemParameterID[i])
                    {
                        return SystemParameterType[i];
                    }
                }
            }
            //如果message[16] == 0x46 && message[17] == 0x80读取系统特殊参数
            if (message[16] == 0x46 && message[17] == 0x80)
            {
                for (int i = 0; i < FeLiParameterID.Length; i++)
                {
                    //根据message[19]判断读取哪个系统使能参数
                    if (message[19] == FeLiParameterID[i])
                    {
                        return FeLiParameterType[i];
                    }
                }
            }
            //如果message[16] == 0x46 && message[17] == 0x8C设置光宇系统参数
            if (message[16] == 0x46 && message[17] == 0x8C)
            {
                for (int i = 0; i < CoslightParameterID.Length; i++)
                {
                    //根据message[19]判断设置哪个光宇系统参数
                    if (message[19] == CoslightParameterID[i])
                    {
                        return CoslightParameterType[i];
                    }
                }
            }
            //如果message[16] == 0x46 &&( message[17] == 0x8E || message[17] == 0x92)设置系统特殊参数
            if (message[16] == 0x46 && (message[17] == 0x8E || message[17] == 0x92))
            {
                for (int i = 0; i < DryContactSetType.Length; i++)
                {
                    //根据message[19]判断设置哪个
                    if (message[19] == DryContactSetID[i])
                    {
                        return DryContactSetType[i];
                    }
                }
            }
            //message[16] == 0x47 && message[17] == 0x25 设置防盗参数
            if (message[16] == 0x47 && message[17] == 0x25)
            {
                for (int i = 0; i < AgainstTheftParameterID.Length; i++)
                {
                    //根据message[19]判断设置哪个
                    if (message[19] == AgainstTheftParameterID[i])
                    {
                        return AgainstTheftParameterType[i];
                    }
                }
            }
            //message[16] == 0x47 && message[17] == 0xA2设置GPS参数
            if (message[16] == 0x47 && message[17] == 0xA2)
            {
                for (int i = 0; i < GPSPArameterID.Length; i++)
                {
                    //根据message[19]判断设置哪个
                    if (message[19] == GPSPArameterID[i])
                    {
                        return GPSPArameterType[i];
                    }
                }
            }
            //message[16] == 0x47 && message[17] == 0xA3设置4G参数
            if (message[16] == 0x47 && message[17] == 0xA3)
            {
                for (int i = 0; i < WlanPArameterID.Length; i++)
                {
                    //根据message[19]判断设置哪个
                    if (message[19] == WlanPArameterID[i])
                    {
                        return WlanPArameterType[i];
                    }
                }
            }
            //message[16] == 0x47 && message[17] == 0xA5 设置4G_GPS特殊参数
            if (message[16] == 0x47 && message[17] == 0xA5)
            {
                for (int i = 0; i < WlanEnableID.Length; i++)
                {
                    //根据message[19]判断设置哪个
                    if (message[19] == WlanEnableID[i])
                    {
                        return WlanEnableType[i];
                    }
                }
            }
            //LastSendCommandType = "控制充放电回路";

            return commandType;
        }
        //返回命令值 空 未实现
        public string ReturnCommandValue(byte[] message)
        {
            string value = "";

            return value;
        }
    }
}


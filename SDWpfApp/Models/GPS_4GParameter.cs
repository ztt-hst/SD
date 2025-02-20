using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDWpfApp.Models
{
    //用于描述设备的 WLAN 参数信息，主要包括 GPS 数据、4G 硬件与软件版本等
    public class WlanParameter
    {
        public virtual int PackID { get; set; }         //包ID
        public virtual string Longitude { get; set; }   // 经度

        public virtual string Latitude { get; set; }    // 纬度

        public virtual string Altitude { get; set; }    // 海拔高度

        public virtual string NSAT { get; set; }        // 卫星数量

        public virtual string UTC { get; set; }         // 协调世界时间

        public virtual string TimeZone { get; set; }    // 时区

        public virtual string LTM { get; set; }         //本地时间

        public virtual string Date { get; set; }         // 日期

        public virtual string _4GHardwareVersion { get; set; }  // 4G 硬件版本
        public virtual string _4GSoftwareVersion { get; set; }  // 4G 软件版本
        public virtual string ModelType { get; set; }           // 设备型号

        public virtual string HeartbeatCycle { get; set; }      // 心跳周期      
        public virtual string APN { get; set; }                 // APN（接入点名称，用于移动网络配置）
        public virtual string APN_Account { get; set; }         // APN 用户名
        public virtual string APN_Password { get; set; }        // APN 密码
    }

    //用于描述与 WLAN 启用相关的状态和控制信息。
    public class WlanEnableControl
    {
        public virtual int PackID { get; set; }

        public virtual string State_GPS { get; set; }    // GPS 状态
        public virtual string State_4G { get; set; }     // 4G 状态
        public virtual string State_Wlan { get; set; }  // WLAN 状态
        public virtual string GPSTheftPrevention { get; set; }  // GPS 防盗启用状态
        public virtual string CommunicationTimeoutTheftPrevention { get; set; }     // 通信超时防盗启用状态
        public virtual string AlarmMessage { get; set; }    // 报警消息
        public virtual string AlarmTest { get; set; }   // 报警测试

        public virtual string AlarmBuzzer { get; set; } // 报警蜂鸣器
    }
}

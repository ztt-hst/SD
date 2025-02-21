using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DevExpress.Xpf.Core;
using System.Data;
using System.IO.Ports;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using DevExpress.Xpf.WindowsUI;
using System.Windows;
using SDWpfApp.ViewModels;
using DevExpress.Mvvm;

namespace SDWpfApp.Models
{
    public class SerialPortCommunicator
    {
        public static AuthorizationManager Manager { get; set; }//授权管理者

        public static byte IsReadingDeviceRunningData;//是否正在读取设备运行数据
        public static byte ReadingDeviceRunningDataCounter;//读取设备运行数据计数
        public static byte IsReadingDeviceWarningData;//是否正在读取设备警告数据
        public static byte ReadingDeviceWarningDataCounter;//读取设备警告数据计数
        public static byte ReadingDeviceEventDataCounter;//读取设备事件数据计数
        public static byte IsReadingDeviceEventData;//是否正在读取设备事件数据
        public static byte IsBootLaoderBMSConnectionCount { get; set; } = 0x00;//BootLaoder与BMS是否连接计数
        public static byte IsBootLaoderBMSConnection { get; set; } = 0x01;
        public static bool IsAntiTheftGPSState { get; set; }//20200715是否防盗GPS
        public int intBaudRate = 9600;//20200508波特率
        private Queue<byte[]> MessagesToBeSent;//被发送的信息
        private Queue<byte[]> MessagesToBeSent_UserAction;//
        public event EventHandler<byte[]> ReceiveEvent;//接收事件
        public event EventHandler<int> ReceiveFailureEvent;//接收失败
        private SerialPort port;//端口
        private int LastCommunicationDeviceAddress;//上一个通讯设备地址
        public static CommandType LastSendCommandType;//命令类型
        public static byte CommunicationProtocoVersionNumber { get; set; }//通信协议版本号
        public static string Ver = "";
        public static string ManufacturerName = "";
        public static string ZTE_Type = "";
        public virtual string PortName { get; set; }//端口名称
        public static float SOH = 100F;
        public static bool IsCommunicationFirst { get; set; }//第一次通讯
        public static bool boolCommunicationTypeZTE { get; set; }//通讯类型ZTE
        public static bool boolCommunicationTypeDPC { get; set; }//通讯类型DPC
        public static byte IsGPSConnection { get; set; }//GPS是否连接
        public static byte IsGyroSensorConnection { get; set; }//陀螺仪传感器是否连接
        public static byte IsAntiTheftConnection { get; set; }//防盗是否连接
        public static BootLaoderStatus BLStatus { get; set; }//20200502 BootLaoder状态枚举
        private ObservableCollection<Pack> PackCollection { get; set; }//电池包集
        private Timer CommunicationTimer;//定时器
          
        public SerialPortCommunicator(ObservableCollection<Pack> packCollection)//构造函数
        {
            MessagesToBeSent = new Queue<byte[]>();
            MessagesToBeSent_UserAction = new Queue<byte[]>();
            this.PackCollection = packCollection;
        }
        public static bool checkVerisOld()//20200429//20200518 1.73是否是旧版本
        {
            if (ManufacturerName == "ZTE-C" && ZTE_Type == "FB150C")
            {
                if (float.Parse(Ver) < 1.73f)
                {
                    return true;
                }
            }
            return false;
        }
        //通讯定时器触发回调函数
        private void CommunicationTimer_Elapsed(object obj)
        {
            CollectMessages();

            Send();
        }
        //打开端口
        private void StartPort()
        {
            this.port = new SerialPort
            {
                PortName = this.PortName,
                BaudRate = intBaudRate,
                DataBits = 8,
                StopBits = StopBits.One,
                Parity = Parity.None
            };

            try
            {
                this.port.Open();
            }
            catch (Exception e)
            {
                WinUIMessageBox.Show(null, e.Message.ToString(), "错误",
                      MessageBoxButton.OK,
                      MessageBoxImage.Information,
                      MessageBoxResult.None, MessageBoxOptions.None,
                      FloatingMode.Adorner);
                return;
            }

            CommunicationTimer = new Timer(CommunicationTimer_Elapsed, null, 0, 2000);
        }
        //关闭端口
        private void ClosePort()
        {
            CommunicationTimer?.Dispose();//?.空条件运算符，不为空时调用Dispose方法

            MessagesToBeSent.Clear();//清空待发送信息

            if (this.port != null)
            {
                if (this.port.IsOpen)
                {
                    this.port.Close();
                }

                this.port.Dispose();
            }
        }
        // 用户操作
        internal void UserAction(byte[] message)
        {
            MessagesToBeSent_UserAction.Enqueue(message);
        }
        //用户操作清除
        internal void UserAction_Clear()
        {
            MessagesToBeSent_UserAction.Clear();
        }
        //端口名称改变
        public void OnPortNameChanged()
        {
            try 
            {
                Console.WriteLine($"Attempting to change port to: {PortName}");               
                ClosePort();

                if (!string.IsNullOrEmpty(PortName))
                {
                    StartPort();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in OnPortNameChanged: {ex.Message}");
            }
        }
        //发送
        private void Send()
        {
            try
            {
                Receive();//？？？接收
                if (BLStatus == BootLaoderStatus.BootLoader已连接 || BLStatus == BootLaoderStatus.应用程序已连接)
                {
                    Thread.Sleep(500);//延迟500毫秒，确保稳定
                }
                byte[] message;

                if (MessagesToBeSent_UserAction.Count != 0)//先从用户操作队列中获取信息
                {
                    message = MessagesToBeSent_UserAction.Dequeue();
                }
                else
                {
                    message = MessagesToBeSent.Dequeue();
                }
                // 保存上次发送命令信息
                SaveLastSendCommandInfo(message);
                // DPC通信
                if (MainViewModel.communicationType == CommunicationType.DPC)
                {
                    int CID2 = Convert.ToByte(ProtocalProvider.ASCIIToString(message[7]) + ProtocalProvider.ASCIIToString(message[8]), 16);
                    if (CID2 == 0x42 || CID2 == 0x44)
                    {
                        LastCommunicationDeviceAddress = Convert.ToByte(ProtocalProvider.ASCIIToString(message[13]) + ProtocalProvider.ASCIIToString(message[14]), 16);
                    }
                }
                else
                {
                    LastCommunicationDeviceAddress = Convert.ToByte(ProtocalProvider.ASCIIToString(message[3]) + ProtocalProvider.ASCIIToString(message[4]), 16);
                }

                this.port.DiscardInBuffer();//清理输入缓冲区
                this.port.Write(message, 0, message.Length);//将消息写入串口
            }
            catch
            {

            }
        }
        //接收
        private void Receive()
        {
            if (this.port.BytesToRead == 0)//可读取字节为0
            {
                if (LastCommunicationDeviceAddress != 0 && LastCommunicationDeviceAddress != 0xFF)
                {
                    ReceiveFailureEvent(this, LastCommunicationDeviceAddress);//触发接收失败事件
                }
            }
            else
            {
                byte[] message = new byte[this.port.BytesToRead];

                this.port.Read(message, 0, this.port.BytesToRead);

                if (ProtocalProvider.CheckMessage(message))//检验消息
                {
                    ReceiveEvent(this, ProtocalProvider.CalculateHexMessage(message));
                }

                LastCommunicationDeviceAddress = 0;
            }
        }
        //收集信息
        private void CollectMessages()
        {
            //待发消息队列为0
            if (MessagesToBeSent_UserAction.Count == 0 && MessagesToBeSent.Count == 0)
            {
                if (BLStatus == BootLaoderStatus.应用程序连接中)//20200502
                {
                    IsBootLaoderBMSConnectionCount++;//连接计数++
                    if (intBaudRate == 9600)
                    {
                        if (IsBootLaoderBMSConnectionCount >= 12)//计数超过12
                        {
                            intBaudRate = 19200;//改变波特率
                            OnPortNameChanged();//端口名称改变
                        }
                    }
                    if (IsBootLaoderBMSConnectionCount >= 12)//计数超过12
                    {
                        IsBootLaoderBMSConnection = 0x01;//重置1
                    }

                    //获取APP连接的命令并加入MessagesToBeSent_UserAction队列
                    MessagesToBeSent_UserAction.Enqueue(ProtocalProvider.GetMessage_ApplicationConnection(IsBootLaoderBMSConnection));
                }
                else if (BLStatus == BootLaoderStatus.BootLoader连接中)//20200502
                {
                    IsBootLaoderBMSConnectionCount++;//计数++
                    if (intBaudRate == 9600)
                    {
                        if (IsBootLaoderBMSConnectionCount >= 12)
                        {
                            intBaudRate = 19200;
                            OnPortNameChanged();
                        }
                    }
                    if (IsBootLaoderBMSConnectionCount >= 12)
                    {
                        IsBootLaoderBMSConnection = 0x01;
                    }
                    //获取BootLaoder连接的命令并加入MessagesToBeSent_UserAction队列
                    MessagesToBeSent_UserAction.Enqueue(ProtocalProvider.GetMessage_BootLaoderConnection(IsBootLaoderBMSConnection));
                }
                else if (BLStatus == BootLaoderStatus.一键擦除中)//20200609
                {
                    //生成BootLoaderFlash擦除命令并加入队列
                    MessagesToBeSent_UserAction.Enqueue(ProtocalProvider.GetMessage_BootLaoderFlashErases(IsBootLaoderBMSConnection));
                }
                else if (IsReadingDeviceRunningData != 0 && ReadingDeviceRunningDataCounter <= 4)//???
                {
                    ReadingDeviceRunningDataCounter++;//计数++
                    if (MainViewModel.communicationType == CommunicationType.DPC)//DPC通讯
                    {
                        //获取DPC读取设备运行记录的命令并加入队列
                        MessagesToBeSent_UserAction.Enqueue(ProtocalProvider.GetMessage_ReadDeviceRunningRecord_DPC(IsReadingDeviceRunningData, 0x01));

                    }
                    else//非DPC通讯
                    {
                        //获取读取设备运行记录的命令并加入队列
                        MessagesToBeSent_UserAction.Enqueue(ProtocalProvider.GetMessage_ReadDeviceRunningRecord(IsReadingDeviceRunningData, 0x01));
                    }
                    return;
                }
                else if (IsReadingDeviceWarningData != 0 && ReadingDeviceWarningDataCounter <= 4)
                {
                    ReadingDeviceWarningDataCounter++;//计数++
                    if (MainViewModel.communicationType == CommunicationType.DPC)//
                    {
                        ////获取DPC读取设备警告记录的命令并加入队列
                        MessagesToBeSent_UserAction.Enqueue(ProtocalProvider.GetMessage_ReadDeviceWarningRecord_DPC(IsReadingDeviceWarningData, 0x01));
                    }
                    else
                    {
                        ////获取读取设备警告记录的命令并加入队列
                        MessagesToBeSent_UserAction.Enqueue(ProtocalProvider.GetMessage_ReadDeviceWarningRecord(IsReadingDeviceWarningData, 0x01));
                    }
                    return;
                }
                else if (IsReadingDeviceEventData != 0 && ReadingDeviceEventDataCounter <= 4)//20200610
                {
                    ReadingDeviceEventDataCounter++;//计数++
                    //获取读取设备事件记录的命令并加入队列
                    MessagesToBeSent_UserAction.Enqueue(ProtocalProvider.GetMessage_ReadDeviceEventRecord(IsReadingDeviceEventData, 0x01));
                    return;
                }
                else if (IsGPSConnection != 0 && MainViewModel.IsReadGPSAnolog)//是否存在 GPS 连接，是否需要读取 GPS 模拟量数据
                {
                    if (!IsAntiTheftGPSState)// GPS 未启用防盗功能
                    {
                        //生成读取防盗模拟量的消息并加入MessagesToBeSent.队列
                        MessagesToBeSent.Enqueue(ProtocalProvider.GetMessage_ReadAntiTheftAnalog(IsGPSConnection));
                    }
                    else
                    {
                        //生成读取 GPS 模拟量的消息并加入队列
                        MessagesToBeSent.Enqueue(ProtocalProvider.GetMessage_ReadGPSAnalog(IsGPSConnection));
                    }
                }
                else if (IsGyroSensorConnection != 0 && MainViewModel.IsReadGyroSensorAnolog)//陀螺仪是否连接，是否需要读取陀螺仪模拟量数据
                {
                    //获取读取陀螺仪模拟量消息并加入MessagesToBeSent队列
                    MessagesToBeSent.Enqueue(ProtocalProvider.GetMessage_ReadGyroSensorAnalog(IsGyroSensorConnection));
                    //获取读取防盗参数消息并加入MessagesToBeSent队列
                    MessagesToBeSent.Enqueue(ProtocalProvider.GetMessage_ReadAntiTheftParameter(IsGyroSensorConnection));
                }
                else if (IsAntiTheftConnection != 0 && MainViewModel.IsReadAntiTheftAnolog)//防盗是否连接，是否需要读取防盗模拟量                                   
                {
                    //读取防盗模拟量信息加入队列
                    MessagesToBeSent.Enqueue(ProtocalProvider.GetMessage_ReadAntiTheftAnalog(IsAntiTheftConnection));
                }

                foreach (Pack pack in this.PackCollection)//对这个电池包集中的每一个电池包
                {
                    if (pack.IsCommunicationEnabled)//电池包通讯使能
                    {
                        if (MainViewModel.communicationType == CommunicationType.DPC)//DPC通讯
                        {
                            if (CommunicationProtocoVersionNumber == 0)//通讯协议版本号为0
                            {
                                //获取版本号
                                MessagesToBeSent.Enqueue(ProtocalProvider.SendMessage(pack.PackID, CID2_Type.获取通信协议版本号));
                            }
                            else
                            if (string.IsNullOrEmpty(pack.ManufacturerName))//生产产商名称为空
                            {
                                MessagesToBeSent.Enqueue(ProtocalProvider.SendMessage(pack.PackID, CID2_Type.获取设备厂商信息));
                            }
                            else
                            {
                                MessagesToBeSent.Enqueue(ProtocalProvider.SendMessage(pack.PackID, CID2_Type.获取遥信量信息));//告警及其他
                                MessagesToBeSent.Enqueue(ProtocalProvider.SendMessage(pack.PackID, CID2_Type.获取遥测量信息));//模拟量
                                MessagesToBeSent.Enqueue(ProtocalProvider.SendMessage(pack.PackID, CID2_Type.获取时间));
                            }
                        }
                        else//非DPC通讯
                        {
                            if (string.IsNullOrEmpty(pack.SystemName))//系统名字为空
                            {
                                MessagesToBeSent.Enqueue(ProtocalProvider.GetMessage_ManufacturerInfo(pack.PackID));
                            }
                            //else if (pack.ManufacturerName == "ZTE-C" && pack.SystemName == "FB150C")//20200418
                            else//非空
                            //if (pack.ManufacturerName == "ZTE-C")
                            {
                                //if (pack.AntiThief == WarningType.正常)
                                //{
                                if (MainViewModel.communicationType == CommunicationType.ZTE邮电)//通讯类型为ZTE邮电
                                {
                                    MessagesToBeSent.Enqueue(ProtocalProvider.GetMessage_ReadWarning_Coslight(pack.PackID));//读取Coslight警告
                                    MessagesToBeSent.Enqueue(ProtocalProvider.GetMessage_ReadAnalog_Coslight(pack.PackID));//读取Coslight模拟量
                                    MessagesToBeSent.Enqueue(ProtocalProvider.GetMessage_ReadStatus_Coslight(pack.PackID));//读取Coslight状态
                                    MessagesToBeSent.Enqueue(ProtocalProvider.GetMessage_ReadAnalog(pack.PackID));//读取模拟量
                                    MessagesToBeSent.Enqueue(ProtocalProvider.GetMessage_ReadStatus(pack.PackID));//读取状态
                                    MessagesToBeSent.Enqueue(ProtocalProvider.GetMessage_ReadWarning(pack.PackID));//读取警告
                                }
                                else if (MainViewModel.communicationType == CommunicationType.移动)//
                                {
                                    MessagesToBeSent.Enqueue(ProtocalProvider.GetMessage_ReadTime_Mobile(pack.PackID));//获取移动读取时间
                                    MessagesToBeSent.Enqueue(ProtocalProvider.GetMessage_ReadAnalog(pack.PackID));//
                                    MessagesToBeSent.Enqueue(ProtocalProvider.GetMessage_ReadStatus(pack.PackID));
                                    MessagesToBeSent.Enqueue(ProtocalProvider.GetMessage_ReadWarning(pack.PackID));
                                }

                                //}
                            }
                        }
                    }
                }
            }
        }
        //保存上次发送命令信息
        private void SaveLastSendCommandInfo(byte[] message)
        {
            CommunicationTimer.Change(300, 300);//表示计时器将在创建或重新启动后 300 毫秒 触发第一次回调 且每300毫秒再次触发
            //
            if (message[5] == 0x34 && message[6] == 0x41)   // 判断CID1=4A modify by sunlw 2020-10-31 17:21
            {
                if (message[7] == 0x34 && message[8] == 0x32)
                {
                    LastSendCommandType = CommandType.读取模拟量;
                }
                else if (message[7] == 0x34 && message[8] == 0x33)
                {
                    LastSendCommandType = CommandType.读取开关量;
                }
                else if (message[7] == 0x34 && message[8] == 0x34)
                {
                    LastSendCommandType = CommandType.读取告警状态;
                }
                else if (message[7] == 0x34 && message[8] == 0x35)
                {
                    LastSendCommandType = CommandType.控制充放电回路;
                }
                else if (message[7] == 0x34 && message[8] == 0x37)
                {
                    LastSendCommandType = CommandType.读取系统参数;
                    CommunicationTimer.Change(1000, 1000);
                }
                else if (message[7] == 0x34 && message[8] == 0x39)
                {
                    LastSendCommandType = CommandType.设置系统参数;

                    if (MainViewModel.communicationType == CommunicationType.DPC)
                    {
                        CommunicationTimer.Change(1000, 1000);
                    }
                }
                else if (message[7] == 0x35 && message[8] == 0x31)
                {
                    LastSendCommandType = CommandType.获取厂家信息;
                }
                else if (message[7] == 0x38 && message[8] == 0x30)
                {
                    LastSendCommandType = CommandType.读取系统特殊参数;
                }
                else if ((message[7] == 0x39 && message[8] == 0x34))//20200529
                {
                    LastSendCommandType = CommandType.设置系统特殊参数;
                }
                else if (message[7] == 0x38 && (message[8] == 0x31 || message[8] == 0x45))
                {
                    LastSendCommandType = CommandType.设置系统特殊参数;
                }
                else if (message[7] == 0x34 && message[8] == 0x42)   // modify by sunlw 2020-11-01 19:32
                {
                    LastSendCommandType = CommandType.读取设备存储运行数据;
                    CommunicationTimer.Change(500, 500);
                }
                else if ((message[7] == 0x38 && message[8] == 0x35) || (message[7] == 0x39 && message[8] == 0x30))
                {
                    LastSendCommandType = CommandType.读取设备存储告警数据;
                    CommunicationTimer.Change(500, 500);
                }
                else if (message[7] == 0x38 && message[8] == 0x38)
                {
                    LastSendCommandType = CommandType.读取光宇模拟量;
                }
                else if (message[7] == 0x38 && message[8] == 0x39)
                {
                    LastSendCommandType = CommandType.读取光宇开关量;
                }
                else if (message[7] == 0x38 && message[8] == 0x44)
                {
                    LastSendCommandType = CommandType.读取干接点使能状态;
                }
                else if (message[7] == 0x39 && message[8] == 0x31)
                {
                    LastSendCommandType = CommandType.读取干接点2状态;
                }
                else if (message[7] == 0x38 && message[8] == 0x46)
                {
                    LastSendCommandType = CommandType.设置产品序列号;
                    CommunicationTimer.Change(1000, 1000);
                }
            }
            else if (message[5] == 0x34 && message[6] == 0x37)
            {
                if (message[7] == 0x41 && message[8] == 0x31)//20200426
                {
                    LastSendCommandType = CommandType.读取4G参数;
                    CommunicationTimer.Change(1000, 1000);
                }
                else if (message[7] == 0x32 && message[8] == 0x31)//2020609
                {
                    LastSendCommandType = CommandType.读取设备信息;
                }
                else if (message[7] == 0x41 && message[8] == 0x30)//20200518
                {
                    LastSendCommandType = CommandType.读取GPS参数;
                    CommunicationTimer.Change(1000, 2000);
                }
                else if (message[7] == 0x41 && message[8] == 0x36)//20200518
                {
                    LastSendCommandType = CommandType.读取GPS实时数据;
                    CommunicationTimer.Change(1000, 1000);
                }
                else if (message[7] == 0x41 && message[8] == 0x34)//20200518
                {
                    LastSendCommandType = CommandType.读取4G_GPS特殊参数;
                    CommunicationTimer.Change(1000, 1000);
                }
                else if (message[7] == 0x41 && message[8] == 0x32)//20200518
                {
                    LastSendCommandType = CommandType.设置GPS参数;
                    CommunicationTimer.Change(1000, 1000);
                }
                else if (message[7] == 0x41 && message[8] == 0x33)//20200518
                {
                    LastSendCommandType = CommandType.设置4G参数;
                    CommunicationTimer.Change(1000, 1000);
                }
                else if (message[7] == 0x41 && message[8] == 0x35)//20200518
                {
                    LastSendCommandType = CommandType.设置4G_GPS特殊参数;
                    CommunicationTimer.Change(1000, 1000);
                }
                else if (message[7] == 0x39 && message[8] == 0x30)//20200502
                {
                    LastSendCommandType = CommandType.应用程序连接;
                    CommunicationTimer.Change(100, 100);
                }
                else if (message[7] == 0x39 && message[8] == 0x33)//20200502
                {
                    LastSendCommandType = CommandType.BootLoader连接;
                    CommunicationTimer.Change(100, 100);
                }
                else if (message[7] == 0x39 && message[8] == 0x34)//20200609
                {
                    LastSendCommandType = CommandType.BootLoader擦除;
                    CommunicationTimer.Change(500, 500);
                }
                else if (message[7] == 0x39 && message[8] == 0x31)//20200502
                {
                    LastSendCommandType = CommandType.设置BootLoader文件名;
                    CommunicationTimer.Change(500, 500);
                }
                else if (message[7] == 0x39 && message[8] == 0x32)//20200502
                {
                    LastSendCommandType = CommandType.设置BootLoader参数;
                    CommunicationTimer.Change(300, 300);
                }
                else if (message[7] == 0x39 && message[8] == 0x35)//20200701
                {
                    LastSendCommandType = CommandType.设置BootLoader条数;
                    CommunicationTimer.Change(1000, 1000);
                }
                else if (message[7] == 0x32 && message[8] == 0x30)//20200528
                {
                    LastSendCommandType = CommandType.读取陀螺仪实时数据;
                    CommunicationTimer.Change(1000, 1000);
                }
                else if (message[7] == 0x32 && message[8] == 0x33)//20200610
                {
                    LastSendCommandType = CommandType.读取设备存储事件数据;
                    CommunicationTimer.Change(500, 500);
                }
                else if (message[7] == 0x32 && message[8] == 0x34)//20200610
                {
                    LastSendCommandType = CommandType.读取防盗状态;
                    CommunicationTimer.Change(500, 500);
                }
                else if (message[7] == 0x32 && message[8] == 0x35)
                {
                    LastSendCommandType = CommandType.设置防盗参数;
                    CommunicationTimer.Change(1000, 1000);
                }
                else if (message[7] == 0x32 && message[8] == 0x36)
                {
                    LastSendCommandType = CommandType.读取防盗参数;
                    CommunicationTimer.Change(500, 500);
                }
            }
        }
        // 添加测试方法
        public void SimulateReceiveForTest(byte[] testMessage)
        {
            if (ReceiveEvent != null)
            {
                ReceiveEvent(this, testMessage);
            }
        }
    }
}

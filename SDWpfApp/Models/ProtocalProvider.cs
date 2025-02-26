using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SDWpfApp.Models;
using SDWpfApp.ViewModels;

namespace SDWpfApp.Models
{
    public class ProtocalProvider
    {
        //电池组计数
        public static byte BatteryGroupCount { get; set; } = 0x01;
        //ASCII转String
        public static string ASCIIToString(int asciiCode)
        {
            if (asciiCode >= 0 && asciiCode <= 255)
            {
                System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
                byte[] byteArray = new byte[] { (byte)asciiCode };
                string strCharacter = asciiEncoding.GetString(byteArray);
                return (strCharacter);
            }
            else
            {
                throw new Exception("ASCII Code is not valid.");
            }
        }
        private static byte ValueToASCLL(int value)
        {
            return (byte)((value <= 9) ? value + 48 : value + 55);
        }
        //计算长度
        private static byte CalculateLength(int lenID, int wholeBits, int returnBits)
        {
            int result = 0;

            for (int i = 0; i < 3; i++)
            {
                result += (lenID >> (i * 4)) & 0x000F;
            }

            result = (((~(result % 16) + 1) & 0x0000000f) * 4096 + lenID) >> ((wholeBits - returnBits) * 4) & 0x000F;

            return ValueToASCLL(result);
        }

        //计算ASCII码
        private static byte CalculateASCLL(int value, int wholeBits, int returnBits)
        {
            value = value >> ((wholeBits - returnBits) * 4) & 0x000F;//移位并求模65536余数65536的16进制为1 0000
            return ValueToASCLL(value);
        }
        private static byte CalculateCHKSUM(byte[] unCheckCode, int wholeBits, int returnBits)
        {
            int addAmount = 0;

            for (int i = 1; i <= unCheckCode.Length - 6; i++)
            {
                addAmount += unCheckCode[i];
            }

            return CalculateASCLL(~(addAmount) + 1, wholeBits, returnBits);
        }
        //获取信息
        internal static byte[] GetMessage(byte Version, byte address, int CID1, int CID2, int lenID, byte Pack, byte[] commandType)
        {
            byte[] message = new byte[18 + lenID];

            message[0] = 0x7E;//SOI消息开始标志
            message[1] = CalculateASCLL(Version, 2, 1);//VER版本 ，每个字节用两个ASCII码表示
            message[2] = CalculateASCLL(Version, 2, 2);//VER

            message[3] = CalculateASCLL(address, 2, 1);//ADR设备地址
            message[4] = CalculateASCLL(address, 2, 2);//ADR

            message[5] = CalculateASCLL(CID1, 2, 1);//VER//CID1命令
            message[6] = CalculateASCLL(CID1, 2, 2);//VER//CID1

            message[7] = CalculateASCLL(CID2, 2, 1); //CID2
            message[8] = CalculateASCLL(CID2, 2, 2);//CID2

            message[9] = CalculateLength(lenID, 4, 1);//LENGTH数据长度
            message[10] = CalculateLength(lenID, 4, 2);//LENGTH
            message[11] = CalculateLength(lenID, 4, 3);//LENGTH
            message[12] = CalculateLength(lenID, 4, 4);//LENGTH

            if (lenID == 2)
            {
                message[13] = CalculateASCLL(Pack, 2, 1);//CommandGroup 0x46代表要所有电池组 0X30 0X31代表要第一组
                message[14] = CalculateASCLL(Pack, 2, 2);//CommandGroup 0x46代表要所有电池组

            }
            else if (lenID == 4)//遥控 4字节
            {
                message[13] = CalculateASCLL(commandType[0], 2, 1);     //00放电，01充电
                message[14] = CalculateASCLL(commandType[0], 2, 2);     //
                message[15] = CalculateASCLL(commandType[1], 2, 1);
                message[16] = CalculateASCLL(commandType[1], 2, 2);
            }
            else if (lenID == 0x0E)//同步时间
            {

                message[13] = CalculateASCLL(DateTime.Now.Year, 4, 1);//年份
                message[14] = CalculateASCLL(DateTime.Now.Year, 4, 2);
                message[13 + 2] = CalculateASCLL(DateTime.Now.Year, 4, 3);
                message[14 + 2] = CalculateASCLL(DateTime.Now.Year, 4, 4);

                message[15 + 2] = CalculateASCLL(DateTime.Now.Month, 2, 1);//月份
                message[16 + 2] = CalculateASCLL(DateTime.Now.Month, 2, 2);

                message[17 + 2] = CalculateASCLL(DateTime.Now.Day, 2, 1);//日期
                message[18 + 2] = CalculateASCLL(DateTime.Now.Day, 2, 2);

                message[19 + 2] = CalculateASCLL(DateTime.Now.Hour, 2, 1);//小时
                message[20 + 2] = CalculateASCLL(DateTime.Now.Hour, 2, 2);

                message[21 + 2] = CalculateASCLL(DateTime.Now.Minute, 2, 1);//分钟
                message[22 + 2] = CalculateASCLL(DateTime.Now.Minute, 2, 2);

                message[23 + 2] = CalculateASCLL(DateTime.Now.Second, 2, 1);//秒钟
                message[24 + 2] = CalculateASCLL(DateTime.Now.Second, 2, 2);
            }
            else
            { }

            message[message.Length - 5] = CalculateCHKSUM(message, 4, 1);//CHKSUM校验和
            message[message.Length - 4] = CalculateCHKSUM(message, 4, 2);//CHKSUM
            message[message.Length - 3] = CalculateCHKSUM(message, 4, 3);//CHKSUM
            message[message.Length - 2] = CalculateCHKSUM(message, 4, 4);//CHKSUM
            message[message.Length - 1] = 0x0D;//EOI

            return message;
        }
        //根据输入的地址和命令类型获得一条读取设备运行记录的命令消息
        internal static byte[] GetMessage_ReadDeviceRunningRecord_DPC(byte address, byte commandType)
        {
            int lenID = 0x04;

            byte[] message = new byte[20];

            message[0] = 0x7E;//SOI
            message[1] = 0x32;//版本
            message[2] = 0x30;

            message[3] = CalculateASCLL(address, 2, 1);//ADR
            message[4] = CalculateASCLL(address, 2, 2);//ADR

            message[5] = 0x34;//CID1
            message[6] = 0x41;//CID1     // 0x4A
            message[7] = 0x34;//CID2
            message[8] = 0x42;//CID2

            message[9] = CalculateLength(lenID, 4, 1);//LENGTH LCHKSUM
            message[10] = CalculateLength(lenID, 4, 2);//LENGTH LENID
            message[11] = CalculateLength(lenID, 4, 3);//LENGTH LENID
            message[12] = CalculateLength(lenID, 4, 4);//LENGTH LENID

            message[13] = CalculateASCLL(commandType, 2, 1);
            message[14] = CalculateASCLL(commandType, 2, 2);

            message[message.Length - 5] = CalculateCHKSUM(message, 4, 1);//CHKSUM
            message[message.Length - 4] = CalculateCHKSUM(message, 4, 2);//CHKSUM
            message[message.Length - 3] = CalculateCHKSUM(message, 4, 3);//CHKSUM
            message[message.Length - 2] = CalculateCHKSUM(message, 4, 4);//CHKSUM
            message[message.Length - 1] = 0x0D;//EOI
            return message;
        }
        internal static byte[] GetMessage_ManufacturerInfo(byte address)
        {
            int lenID = 0x00;

            byte[] message = new byte[18];

            message[0] = 0x7E;//SOI
            message[1] = 0x32;//VER
            message[2] = 0x30;//VER
            message[3] = CalculateASCLL(address, 2, 1);//ADR
            message[4] = CalculateASCLL(address, 2, 2);//ADR
            message[5] = 0x34;//CID1
            message[6] = 0x36;//CID1
            message[7] = 0x35;//CID2
            message[8] = 0x31;//CID2
            message[9] = CalculateLength(lenID, 4, 1);//LENGTH
            message[10] = CalculateLength(lenID, 4, 2);//LENGTH
            message[11] = CalculateLength(lenID, 4, 3);//LENGTH
            message[12] = CalculateLength(lenID, 4, 4);//LENGTH
            message[message.Length - 5] = CalculateCHKSUM(message, 4, 1);//CHKSUM
            message[message.Length - 4] = CalculateCHKSUM(message, 4, 2);//CHKSUM
            message[message.Length - 3] = CalculateCHKSUM(message, 4, 3);//CHKSUM
            message[message.Length - 2] = CalculateCHKSUM(message, 4, 4);//CHKSUM
            message[message.Length - 1] = 0x0D;//EOI
            return message;
        }
        //获取读取设备警告信息记录
        internal static byte[] GetMessage_ReadDeviceWarningRecord(byte address, byte commandType)
        {
            //判断是否是旧版本，旧版本只有一组电池
            if (SerialPortCommunicator.checkVerisOld()) 
            { 
                 ProtocalProvider.BatteryGroupCount = 0x01; 
            } 
            else 
            { 
                ProtocalProvider.BatteryGroupCount = address; 
            }
            int lenID = 0x04;

            byte[] message = new byte[22];

            message[0] = 0x7E;//SOI
            message[1] = 0x32;//VER
            message[2] = 0x30;//VER
            message[3] = CalculateASCLL(address, 2, 1);//ADR
            message[4] = CalculateASCLL(address, 2, 2);//ADR
            message[5] = 0x34;//CID1
            message[6] = 0x36;//CID1
            if (MainViewModel.communicationType == CommunicationType.ZTE邮电)
            {
                message[7] = 0x38;//CID2
                message[8] = 0x35;//CID2
            }
            else
            {
                message[7] = 0x34;//CID2
                message[8] = 0x43;//CID2
            }
            message[9] = CalculateLength(lenID, 4, 1);//LENGTH
            message[10] = CalculateLength(lenID, 4, 2);//LENGTH
            message[11] = CalculateLength(lenID, 4, 3);//LENGTH
            message[12] = CalculateLength(lenID, 4, 4);//LENGTH
            message[13] = CalculateASCLL(ProtocalProvider.BatteryGroupCount, 2, 1);//CommandGroup 0x46代表要所有电池组 0X30 0X31代表要第一组
            message[14] = CalculateASCLL(ProtocalProvider.BatteryGroupCount, 2, 2);//CommandGroup 0x46代表要所有电池组
            message[15] = CalculateASCLL(commandType, 2, 1);
            message[16] = CalculateASCLL(commandType, 2, 2);
            message[message.Length - 5] = CalculateCHKSUM(message, 4, 1);//CHKSUM
            message[message.Length - 4] = CalculateCHKSUM(message, 4, 2);//CHKSUM
            message[message.Length - 3] = CalculateCHKSUM(message, 4, 3);//CHKSUM
            message[message.Length - 2] = CalculateCHKSUM(message, 4, 4);//CHKSUM
            message[message.Length - 1] = 0x0D;//EOI
            return message;
        }
        //根据给定的 packID 和 CID2_Type 枚举值生成一个特定的协议命令消息
        internal static byte[] SendMessage(byte packID, CID2_Type type)
        {
            int CID2 = 0x42;//表示命令的具体类型
            int lenID = 2;//数据长度标识符
            switch (type)
            {
                case CID2_Type.获取通信协议版本号:
                    lenID = 2;
                    CID2 = 0x4F;
                    break;
                case CID2_Type.获取设备厂商信息:
                    lenID = 2;
                    CID2 = 0x51;
                    break;
                case CID2_Type.获取时间:
                    lenID = 2;
                    CID2 = 0x4D;
                    break;
                case CID2_Type.同步时间:
                    //lenID = 0x10;
                    lenID = 0x0E;
                    CID2 = 0x4E;
                    break;
                case CID2_Type.获取遥测量信息:
                    CID2 = 0x42;
                    break;
                case CID2_Type.获取遥信量信息:
                    CID2 = 0x44;
                    break;
                case CID2_Type.获取遥调量信息:
                    CID2 = 0x47;
                    break;
            }
            byte[] commandType = new byte[0];//数据部分初始化为空数组，表示当前命令不需要额外的数据
            return GetMessage(0x20, packID, 0x4A, CID2, lenID, packID, commandType);
        }
        //
        internal static byte[] SendMessage(byte packID, CID2_Type type, byte[] commandType)
        {
            int CID2 = 0x42;
            int lenID = 2;
            switch (type)
            {
                case CID2_Type.遥控命令:
                    lenID = 4;     // modfiy by sunlw 2020-10-31 16:49
                    CID2 = 0x45;
                    break;
            }
            return GetMessage(SerialPortCommunicator.CommunicationProtocoVersionNumber, packID, 0x4A, CID2, lenID, packID, commandType);
        }
        //读取设备事件记录
        internal static byte[] GetMessage_ReadDeviceEventRecord(byte address, byte commandType)//20200610
        {
            if (SerialPortCommunicator.checkVerisOld()) { { ProtocalProvider.BatteryGroupCount = 0x01; } } else { ProtocalProvider.BatteryGroupCount = address; }
            int lenID = 0x04;
            byte[] message = new byte[22];

            message[0] = 0x7E;//SOI
            message[1] = 0x32;//VER
            message[2] = 0x30;//VER
            message[3] = CalculateASCLL(address, 2, 1);//ADR
            message[4] = CalculateASCLL(address, 2, 2);//ADR
            message[5] = 0x34;//CID1
            message[6] = 0x37;//CID1
            message[7] = 0x32;//CID2
            message[8] = 0x33;//CID2
            message[9] = CalculateLength(lenID, 4, 1);//LENGTH
            message[10] = CalculateLength(lenID, 4, 2);//LENGTH
            message[11] = CalculateLength(lenID, 4, 3);//LENGTH
            message[12] = CalculateLength(lenID, 4, 4);//LENGTH
            message[13] = CalculateASCLL(ProtocalProvider.BatteryGroupCount, 2, 1);//CommandGroup 0x46代表要所有电池组 0X30 0X31代表要第一组
            message[14] = CalculateASCLL(ProtocalProvider.BatteryGroupCount, 2, 2);//CommandGroup 0x46代表要所有电池组
            message[15] = CalculateASCLL(commandType, 2, 1);
            message[16] = CalculateASCLL(commandType, 2, 2);
            message[message.Length - 5] = CalculateCHKSUM(message, 4, 1);//CHKSUM
            message[message.Length - 4] = CalculateCHKSUM(message, 4, 2);//CHKSUM
            message[message.Length - 3] = CalculateCHKSUM(message, 4, 3);//CHKSUM
            message[message.Length - 2] = CalculateCHKSUM(message, 4, 4);//CHKSUM
            message[message.Length - 1] = 0x0D;//EOI
            //WriteLog(message);
            return message;
        }
        //设置BootLoader文件名字
        internal static byte[] GetMessage_SetBootLaoderFileName(byte address, string Data)//20200502
        {
            int lenID = 32;
            byte[] message = new byte[18 + (32 + 5) * 2];

            message[0] = 0x7E;//SOI
            message[1] = 0x32;//VER
            message[2] = 0x31;//VER
            message[3] = CalculateASCLL(address, 2, 1);//ADR
            message[4] = CalculateASCLL(address, 2, 2);//ADR
            message[5] = 0x34;//CID1
            message[6] = 0x37;//CID1
            message[7] = 0x39;//CID2
            message[8] = 0x31;//CID2

            message[9] = CalculateLength(lenID, 4, 1);//LENGTH
            message[10] = CalculateLength(lenID, 4, 2);//LENGTH
            message[11] = CalculateLength(lenID, 4, 3);//LENGTH
            message[12] = CalculateLength(lenID, 4, 4);//LENGTH

            byte[] temp = Encoding.Default.GetBytes(Data.PadRight(32 + 5, ' '));

            for (int i = 0; i != 32 + 5; i++)
            {
                message[13 + i * 2] = CalculateASCLL(temp[i], 2, 1);
                message[14 + i * 2] = CalculateASCLL(temp[i], 2, 2);
            }
            message[message.Length - 5] = CalculateCHKSUM(message, 4, 1);//CHKSUM
            message[message.Length - 4] = CalculateCHKSUM(message, 4, 2);//CHKSUM
            message[message.Length - 3] = CalculateCHKSUM(message, 4, 3);//CHKSUM
            message[message.Length - 2] = CalculateCHKSUM(message, 4, 4);//CHKSUM
            message[message.Length - 1] = 0x0D;//EOI
            return message;
        }
        //设置BootLoader数字
        internal static byte[] GetMessage_SetBootLaoderNumber(byte address, byte[] temp)//20200701
        {
            int commandType = 0;
            int lenID = 4;
            byte[] message = new byte[22];

            message[0] = 0x7E;//SOI
            message[1] = 0x32;//VER
            message[2] = 0x31;//VER
            message[3] = CalculateASCLL(address, 2, 1);//ADR
            message[4] = CalculateASCLL(address, 2, 2);//ADR
            message[5] = 0x34;//CID1
            message[6] = 0x37;//CID1
            message[7] = 0x39;//CID2
            message[8] = 0x35;//CID2

            message[9] = CalculateLength(lenID, 4, 1);//LENGTH
            message[10] = CalculateLength(lenID, 4, 2);//LENGTH
            message[11] = CalculateLength(lenID, 4, 3);//LENGTH
            message[12] = CalculateLength(lenID, 4, 4);//LENGTH

            message[13] = CalculateASCLL(commandType, 2, 1);
            message[14] = CalculateASCLL(commandType, 2, 2);

            message[15] = CalculateASCLL(temp[0], 2, 1);
            message[16] = CalculateASCLL(temp[0], 2, 2);

            message[message.Length - 5] = CalculateCHKSUM(message, 4, 1);//CHKSUM
            message[message.Length - 4] = CalculateCHKSUM(message, 4, 2);//CHKSUM
            message[message.Length - 3] = CalculateCHKSUM(message, 4, 3);//CHKSUM
            message[message.Length - 2] = CalculateCHKSUM(message, 4, 4);//CHKSUM
            message[message.Length - 1] = 0x0D;//EOI
            return message;
        }
        //设置BootLoader参数
        internal static byte[] GetMessage_SetBootLaoderParameter(byte address, int commandType, byte[] temp)//20200502
        {
            int lenID = 0;
            byte[] message = new byte[0];

            if (commandType == 0x00)
            {
                lenID = 2 + 2;
                message = new byte[22];
            }
            else
            {
                lenID = 1024 * 2 + 2;
                message = new byte[20 + 1024 * 2];
            }

            message[0] = 0x7E;//SOI
            message[1] = 0x32;//VER
            message[2] = 0x31;//VER
            message[3] = CalculateASCLL(address, 2, 1);//ADR
            message[4] = CalculateASCLL(address, 2, 2);//ADR
            message[5] = 0x34;//CID1
            message[6] = 0x37;//CID1
            message[7] = 0x39;//CID2
            message[8] = 0x32;//CID2

            message[9] = CalculateLength(lenID, 4, 1);//LENGTH
            message[10] = CalculateLength(lenID, 4, 2);//LENGTH
            message[11] = CalculateLength(lenID, 4, 3);//LENGTH
            message[12] = CalculateLength(lenID, 4, 4);//LENGTH

            message[13] = CalculateASCLL(commandType, 2, 1);
            message[14] = CalculateASCLL(commandType, 2, 2);

            if (commandType == 0x00)
            {
                message[15] = CalculateASCLL(temp[0], 2, 1);
                message[16] = CalculateASCLL(temp[0], 2, 2);
            }
            else
            {
                
                for (int i = 0; i != 1024; i++)
                {
                    message[15 + i * 2] = CalculateASCLL(temp[i], 2, 1);
                    message[16 + i * 2] = CalculateASCLL(temp[i], 2, 2);
                }
               
            }

            message[message.Length - 5] = CalculateCHKSUM(message, 4, 1);//CHKSUM
            message[message.Length - 4] = CalculateCHKSUM(message, 4, 2);//CHKSUM
            message[message.Length - 3] = CalculateCHKSUM(message, 4, 3);//CHKSUM
            message[message.Length - 2] = CalculateCHKSUM(message, 4, 4);//CHKSUM
            message[message.Length - 1] = 0x0D;//EOI
            return message;
        }
        //读取干接点状态
        internal static byte[] GetMessage_ReadDryContactStatus(byte address)
        {
            if (SerialPortCommunicator.checkVerisOld()) { { ProtocalProvider.BatteryGroupCount = 0x01; } } else { ProtocalProvider.BatteryGroupCount = address; }//20200429 //20200428
            int lenID = 0x02;

            byte[] message = new byte[20];

            message[0] = 0x7E;//SOI
            message[1] = 0x32;//VER
            message[2] = 0x31;//VER
            message[3] = CalculateASCLL(address, 2, 1);//ADR
            message[4] = CalculateASCLL(address, 2, 2);//ADR
            message[5] = 0x34;//CID1
            message[6] = 0x36;//CID1
            message[7] = 0x38;//CID2
            message[8] = 0x44;//CID2
            message[9] = CalculateLength(lenID, 4, 1);//LENGTH
            message[10] = CalculateLength(lenID, 4, 2);//LENGTH
            message[11] = CalculateLength(lenID, 4, 3);//LENGTH
            message[12] = CalculateLength(lenID, 4, 4);//LENGTH
            message[13] = CalculateASCLL(ProtocalProvider.BatteryGroupCount, 2, 1);//CommandGroup 0x46代表要所有电池组 0X30 0X31代表要第一组
            message[14] = CalculateASCLL(ProtocalProvider.BatteryGroupCount, 2, 2);//CommandGroup 0x46代表要所有电池组
            message[message.Length - 5] = CalculateCHKSUM(message, 4, 1);//CHKSUM
            message[message.Length - 4] = CalculateCHKSUM(message, 4, 2);//CHKSUM
            message[message.Length - 3] = CalculateCHKSUM(message, 4, 3);//CHKSUM
            message[message.Length - 2] = CalculateCHKSUM(message, 4, 4);//CHKSUM
            message[message.Length - 1] = 0x0D;//EOI
            return message;
        }
        //获取读取特殊参数的信息
        internal static byte[] GetMessage_ReadSpecialParameter(byte address)
        {
            if (SerialPortCommunicator.checkVerisOld()) { { ProtocalProvider.BatteryGroupCount = 0x01; } } else { ProtocalProvider.BatteryGroupCount = address; }//20200429 //20200428
            int lenID = 0x02;

            byte[] message = new byte[20];

            message[0] = 0x7E;//SOI
            message[1] = 0x32;//VER
            message[2] = 0x31;//VER
            message[3] = CalculateASCLL(address, 2, 1);//ADR
            message[4] = CalculateASCLL(address, 2, 2);//ADR
            message[5] = 0x34;//CID1
            message[6] = 0x36;//CID1
            message[7] = 0x38;//CID2
            message[8] = 0x30;//CID2
            message[9] = CalculateLength(lenID, 4, 1);//LENGTH
            message[10] = CalculateLength(lenID, 4, 2);//LENGTH
            message[11] = CalculateLength(lenID, 4, 3);//LENGTH
            message[12] = CalculateLength(lenID, 4, 4);//LENGTH
            message[13] = CalculateASCLL(ProtocalProvider.BatteryGroupCount, 2, 1);//CommandGroup 0x46代表要所有电池组 0X30 0X31代表要第一组
            message[14] = CalculateASCLL(ProtocalProvider.BatteryGroupCount, 2, 2);//CommandGroup 0x46代表要所有电池组
            message[message.Length - 5] = CalculateCHKSUM(message, 4, 1);//CHKSUM
            message[message.Length - 4] = CalculateCHKSUM(message, 4, 2);//CHKSUM
            message[message.Length - 3] = CalculateCHKSUM(message, 4, 3);//CHKSUM
            message[message.Length - 2] = CalculateCHKSUM(message, 4, 4);//CHKSUM
            message[message.Length - 1] = 0x0D;//EOI
            return message;
        }
        //Coslight获取读取特殊参数的信息
        internal static byte[] GetMessage_ReadSpecialParameter_Coslight(byte address)//20200529 CID20x93
        {
            if (SerialPortCommunicator.checkVerisOld()) { { ProtocalProvider.BatteryGroupCount = 0x01; } } else { ProtocalProvider.BatteryGroupCount = address; }//20200429 //20200428
            int lenID = 0x02;

            byte[] message = new byte[20];

            message[0] = 0x7E;//SOI
            message[1] = 0x32;//VER
            message[2] = 0x31;//VER
            message[3] = CalculateASCLL(address, 2, 1);//ADR
            message[4] = CalculateASCLL(address, 2, 2);//ADR
            message[5] = 0x34;//CID1
            message[6] = 0x36;//CID1
            message[7] = 0x39;//CID2
            message[8] = 0x33;//CID2
            message[9] = CalculateLength(lenID, 4, 1);//LENGTH
            message[10] = CalculateLength(lenID, 4, 2);//LENGTH
            message[11] = CalculateLength(lenID, 4, 3);//LENGTH
            message[12] = CalculateLength(lenID, 4, 4);//LENGTH
            message[13] = CalculateASCLL(ProtocalProvider.BatteryGroupCount, 2, 1);//CommandGroup 0x46代表要所有电池组 0X30 0X31代表要第一组
            message[14] = CalculateASCLL(ProtocalProvider.BatteryGroupCount, 2, 2);//CommandGroup 0x46代表要所有电池组
            message[message.Length - 5] = CalculateCHKSUM(message, 4, 1);//CHKSUM
            message[message.Length - 4] = CalculateCHKSUM(message, 4, 2);//CHKSUM
            message[message.Length - 3] = CalculateCHKSUM(message, 4, 3);//CHKSUM
            message[message.Length - 2] = CalculateCHKSUM(message, 4, 4);//CHKSUM
            message[message.Length - 1] = 0x0D;//EOI
            return message;
        }
        //生成与应用连接的信息
        internal static byte[] GetMessage_ApplicationConnection(byte address)//20200502
        {
            int lenID = 0;
            byte[] message = new byte[20 - 2];

            message[0] = 0x7E;//SOI
            message[1] = 0x32;//VER
            message[2] = 0x31;//VER
            message[3] = CalculateASCLL(address, 2, 1);//ADR
            message[4] = CalculateASCLL(address, 2, 2);//ADR
            message[5] = 0x34;//CID1
            message[6] = 0x37;//CID1
            message[7] = 0x39;//CID2
            message[8] = 0x30;//CID2
            message[9] = CalculateLength(lenID, 4, 1);//LENGTH
            message[10] = CalculateLength(lenID, 4, 2);//LENGTH
            message[11] = CalculateLength(lenID, 4, 3);//LENGTH
            message[12] = CalculateLength(lenID, 4, 4);//LENGTH
            message[message.Length - 5] = CalculateCHKSUM(message, 4, 1);//CHKSUM
            message[message.Length - 4] = CalculateCHKSUM(message, 4, 2);//CHKSUM
            message[message.Length - 3] = CalculateCHKSUM(message, 4, 3);//CHKSUM
            message[message.Length - 2] = CalculateCHKSUM(message, 4, 4);//CHKSUM
            message[message.Length - 1] = 0x0D;//EOI
            return message;
        }
        //生成用于与 BootLoader 进行连接的消息
        internal static byte[] GetMessage_BootLaoderConnection(byte address)//20200516 CID2==93
        {
            int lenID = 0;
            byte[] message = new byte[20 - 2];

            message[0] = 0x7E;//SOI
            message[1] = 0x32;//VER
            message[2] = 0x31;//VER
            message[3] = CalculateASCLL(address, 2, 1);//ADR
            message[4] = CalculateASCLL(address, 2, 2);//ADR
            message[5] = 0x34;//CID1
            message[6] = 0x37;//CID1
            message[7] = 0x39;//CID2
            message[8] = 0x33;//CID2
            message[9] = CalculateLength(lenID, 4, 1);//LENGTH
            message[10] = CalculateLength(lenID, 4, 2);//LENGTH
            message[11] = CalculateLength(lenID, 4, 3);//LENGTH
            message[12] = CalculateLength(lenID, 4, 4);//LENGTH
            message[message.Length - 5] = CalculateCHKSUM(message, 4, 1);//CHKSUM
            message[message.Length - 4] = CalculateCHKSUM(message, 4, 2);//CHKSUM
            message[message.Length - 3] = CalculateCHKSUM(message, 4, 3);//CHKSUM
            message[message.Length - 2] = CalculateCHKSUM(message, 4, 4);//CHKSUM
            message[message.Length - 1] = 0x0D;//EOI
            return message;
        }
        //擦除BootLoader的FLASH
        internal static byte[] GetMessage_BootLaoderFlashErases(byte address)//20200609
        {
            int lenID = 0;
            byte[] message = new byte[20 - 2];

            message[0] = 0x7E;//SOI
            message[1] = 0x32;//VER
            message[2] = 0x31;//VER
            message[3] = CalculateASCLL(address, 2, 1);//ADR
            message[4] = CalculateASCLL(address, 2, 2);//ADR
            message[5] = 0x34;//CID1
            message[6] = 0x37;//CID1
            message[7] = 0x39;//CID2
            message[8] = 0x34;//CID2
            message[9] = CalculateLength(lenID, 4, 1);//LENGTH
            message[10] = CalculateLength(lenID, 4, 2);//LENGTH
            message[11] = CalculateLength(lenID, 4, 3);//LENGTH
            message[12] = CalculateLength(lenID, 4, 4);//LENGTH
            message[message.Length - 5] = CalculateCHKSUM(message, 4, 1);//CHKSUM
            message[message.Length - 4] = CalculateCHKSUM(message, 4, 2);//CHKSUM
            message[message.Length - 3] = CalculateCHKSUM(message, 4, 3);//CHKSUM
            message[message.Length - 2] = CalculateCHKSUM(message, 4, 4);//CHKSUM
            message[message.Length - 1] = 0x0D;//EOI
            return message;
        }
        //读取设备运行记录
        internal static byte[] GetMessage_ReadDeviceRunningRecord(byte address, byte commandType)
        {
            if (SerialPortCommunicator.checkVerisOld()) { { ProtocalProvider.BatteryGroupCount = 0x01; } } else { ProtocalProvider.BatteryGroupCount = address; }//20200429 //20200428
            int lenID = 0x04;

            byte[] message = new byte[22];

            message[0] = 0x7E;//SOI
            message[1] = 0x32;//VER
            message[2] = 0x30;//VER
            message[3] = CalculateASCLL(address, 2, 1);//ADR
            message[4] = CalculateASCLL(address, 2, 2);//ADR
            message[5] = 0x34;//CID1
            message[6] = 0x36;//CID1
            if (MainViewModel.communicationType == CommunicationType.ZTE邮电)
            {
                message[7] = 0x38;//CID2
                message[8] = 0x34;//CID2
            }
            else
            {
                message[7] = 0x34;//CID2
                message[8] = 0x42;//CID2
            }
            message[9] = CalculateLength(lenID, 4, 1);//LENGTH
            message[10] = CalculateLength(lenID, 4, 2);//LENGTH
            message[11] = CalculateLength(lenID, 4, 3);//LENGTH
            message[12] = CalculateLength(lenID, 4, 4);//LENGTH
            message[13] = CalculateASCLL(ProtocalProvider.BatteryGroupCount, 2, 1);//CommandGroup 0x46代表要所有电池组 0X30 0X31代表要第一组
            message[14] = CalculateASCLL(ProtocalProvider.BatteryGroupCount, 2, 2);//CommandGroup 0x46代表要所有电池组
            message[15] = CalculateASCLL(commandType, 2, 1);
            message[16] = CalculateASCLL(commandType, 2, 2);
            message[message.Length - 5] = CalculateCHKSUM(message, 4, 1);//CHKSUM
            message[message.Length - 4] = CalculateCHKSUM(message, 4, 2);//CHKSUM
            message[message.Length - 3] = CalculateCHKSUM(message, 4, 3);//CHKSUM
            message[message.Length - 2] = CalculateCHKSUM(message, 4, 4);//CHKSUM
            message[message.Length - 1] = 0x0D;//EOI
            //WriteLog(message);
            return message;
        }
        //根据输入的地址和命令类型获得一条读取设备警告记录的协议消息
        internal static byte[] GetMessage_ReadDeviceWarningRecord_DPC(byte address, byte commandType)//20200529 0x90-->0x85
        {
            int lenID = 0x04;

            byte[] message = new byte[22];

            message[0] = 0x7E;//SOI
            message[1] = CalculateASCLL(SerialPortCommunicator.CommunicationProtocoVersionNumber, 2, 1);//VER
            message[2] = CalculateASCLL(SerialPortCommunicator.CommunicationProtocoVersionNumber, 2, 2);//VER

            message[3] = CalculateASCLL(address, 2, 1);//ADR
            message[4] = CalculateASCLL(address, 2, 2);//ADR

            message[5] = 0x34;//CID1
            message[6] = 0x41;//CID1

            message[7] = 0x34;//CID2
            message[8] = 0x34;//CID2

            message[9] = CalculateLength(lenID, 4, 1);//LENGTH
            message[10] = CalculateLength(lenID, 4, 2);//LENGTH
            message[11] = CalculateLength(lenID, 4, 3);//LENGTH
            message[12] = CalculateLength(lenID, 4, 4);//LENGTH

            message[15] = CalculateASCLL(commandType, 2, 1);
            message[16] = CalculateASCLL(commandType, 2, 2);
            message[message.Length - 5] = CalculateCHKSUM(message, 4, 1);//CHKSUM
            message[message.Length - 4] = CalculateCHKSUM(message, 4, 2);//CHKSUM
            message[message.Length - 3] = CalculateCHKSUM(message, 4, 3);//CHKSUM
            message[message.Length - 2] = CalculateCHKSUM(message, 4, 4);//CHKSUM
            message[message.Length - 1] = 0x0D;//EOI
            return message;
        }
        //读取防盗模拟量
        internal static byte[] GetMessage_ReadAntiTheftAnalog(byte address)//20200520
        {
            int lenID = 0x02;

            byte[] message = new byte[20];

            message[0] = 0x7E;//SOI
            message[1] = 0x32;//VER
            message[2] = 0x31;//VER
            message[3] = CalculateASCLL(address, 2, 1);//ADR
            message[4] = CalculateASCLL(address, 2, 2);//ADR
            message[5] = 0x34;//CID1
            message[6] = 0x37;//CID1
            message[7] = 0x32;//CID2
            message[8] = 0x34;//CID2
            message[9] = CalculateLength(lenID, 4, 1);//LENGTH
            message[10] = CalculateLength(lenID, 4, 2);//LENGTH
            message[11] = CalculateLength(lenID, 4, 3);//LENGTH
            message[12] = CalculateLength(lenID, 4, 4);//LENGTH
            message[13] = 0x30;// CalculateASCLL(ProtocalProvider.BatteryGroupCount, 2, 1);//CommandGroup 0x46代表要所有电池组 0X30 0X31代表要第一组
            message[14] = 0x31;// CalculateASCLL(ProtocalProvider.BatteryGroupCount, 2, 2);//CommandGroup 0x46代表要所有电池组
            message[message.Length - 5] = CalculateCHKSUM(message, 4, 1);//CHKSUM
            message[message.Length - 4] = CalculateCHKSUM(message, 4, 2);//CHKSUM
            message[message.Length - 3] = CalculateCHKSUM(message, 4, 3);//CHKSUM
            message[message.Length - 2] = CalculateCHKSUM(message, 4, 4);//CHKSUM
            message[message.Length - 1] = 0x0D;//EOI

            return message;
        }
        //读取GPS模拟量
        internal static byte[] GetMessage_ReadGPSAnalog(byte address)//20200520
        {
            int lenID = 0x02;

            byte[] message = new byte[20];

            message[0] = 0x7E;//SOI
            message[1] = 0x32;//VER
            message[2] = 0x31;//VER
            message[3] = CalculateASCLL(address, 2, 1);//ADR
            message[4] = CalculateASCLL(address, 2, 2);//ADR
            message[5] = 0x34;//CID1
            message[6] = 0x37;//CID1
            message[7] = 0x41;//CID2
            message[8] = 0x36;//CID2
            message[9] = CalculateLength(lenID, 4, 1);//LENGTH
            message[10] = CalculateLength(lenID, 4, 2);//LENGTH
            message[11] = CalculateLength(lenID, 4, 3);//LENGTH
            message[12] = CalculateLength(lenID, 4, 4);//LENGTH
            message[13] = 0x30;// CalculateASCLL(ProtocalProvider.BatteryGroupCount, 2, 1);//CommandGroup 0x46代表要所有电池组 0X30 0X31代表要第一组
            message[14] = 0x31;// CalculateASCLL(ProtocalProvider.BatteryGroupCount, 2, 2);//CommandGroup 0x46代表要所有电池组
            message[message.Length - 5] = CalculateCHKSUM(message, 4, 1);//CHKSUM
            message[message.Length - 4] = CalculateCHKSUM(message, 4, 2);//CHKSUM
            message[message.Length - 3] = CalculateCHKSUM(message, 4, 3);//CHKSUM
            message[message.Length - 2] = CalculateCHKSUM(message, 4, 4);//CHKSUM
            message[message.Length - 1] = 0x0D;//EOI

            return message;
        }
        //生成读取陀螺仪模拟量的命令消息
        internal static byte[] GetMessage_ReadGyroSensorAnalog(byte address)//20200526
        {
            int lenID = 0x02;

            byte[] message = new byte[20];

            message[0] = 0x7E;//SOI
            message[1] = 0x32;//VER
            message[2] = 0x31;//VER
            message[3] = CalculateASCLL(address, 2, 1);//ADR
            message[4] = CalculateASCLL(address, 2, 2);//ADR
            message[5] = 0x34;//CID1
            message[6] = 0x37;//CID1
            message[7] = 0x32;//CID2
            message[8] = 0x30;//CID2
            message[9] = CalculateLength(lenID, 4, 1);//LENGTH
            message[10] = CalculateLength(lenID, 4, 2);//LENGTH
            message[11] = CalculateLength(lenID, 4, 3);//LENGTH
            message[12] = CalculateLength(lenID, 4, 4);//LENGTH
            message[13] = 0x30;// CalculateASCLL(ProtocalProvider.BatteryGroupCount, 2, 1);//CommandGroup 0x46代表要所有电池组 0X30 0X31代表要第一组
            message[14] = 0x31;// CalculateASCLL(ProtocalProvider.BatteryGroupCount, 2, 2);//CommandGroup 0x46代表要所有电池组
            message[message.Length - 5] = CalculateCHKSUM(message, 4, 1);//CHKSUM
            message[message.Length - 4] = CalculateCHKSUM(message, 4, 2);//CHKSUM
            message[message.Length - 3] = CalculateCHKSUM(message, 4, 3);//CHKSUM
            message[message.Length - 2] = CalculateCHKSUM(message, 4, 4);//CHKSUM
            message[message.Length - 1] = 0x0D;//EOI

            return message;
        }
        //读取防盗参数
        internal static byte[] GetMessage_ReadAntiTheftParameter(byte address)//20200622
        {
            int lenID = 0x02;

            byte[] message = new byte[20];

            message[0] = 0x7E;//SOI
            message[1] = 0x32;//VER
            message[2] = 0x31;//VER
            message[3] = CalculateASCLL(address, 2, 1);//ADR
            message[4] = CalculateASCLL(address, 2, 2);//ADR
            message[5] = 0x34;//CID1
            message[6] = 0x37;//CID1
            message[7] = 0x32;//CID2
            message[8] = 0x36;//CID2
            message[9] = CalculateLength(lenID, 4, 1);//LENGTH
            message[10] = CalculateLength(lenID, 4, 2);//LENGTH
            message[11] = CalculateLength(lenID, 4, 3);//LENGTH
            message[12] = CalculateLength(lenID, 4, 4);//LENGTH
            message[13] = 0x30;// CalculateASCLL(ProtocalProvider.BatteryGroupCount, 2, 1);//CommandGroup 0x46代表要所有电池组 0X30 0X31代表要第一组
            message[14] = 0x31;// CalculateASCLL(ProtocalProvider.BatteryGroupCount, 2, 2);//CommandGroup 0x46代表要所有电池组
            message[message.Length - 5] = CalculateCHKSUM(message, 4, 1);//CHKSUM
            message[message.Length - 4] = CalculateCHKSUM(message, 4, 2);//CHKSUM
            message[message.Length - 3] = CalculateCHKSUM(message, 4, 3);//CHKSUM
            message[message.Length - 2] = CalculateCHKSUM(message, 4, 4);//CHKSUM
            message[message.Length - 1] = 0x0D;//EOI

            return message;
        }
        //获取时间 4DH
        internal static byte[] GetMessage_ReadTime_Mobile(byte address)//20200701
        {
            int lenID = 0x02;

            byte[] message = new byte[20];

            message[0] = 0x7E;//SOI
            message[1] = 0x32;//VER
            message[2] = 0x31;//VER
            message[3] = CalculateASCLL(address, 2, 1);//ADR
            message[4] = CalculateASCLL(address, 2, 2);//ADR
            message[5] = 0x34;//CID1
            message[6] = 0x36;//CID1
            message[7] = 0x34;//CID2
            message[8] = 0x44;//CID2
            message[9] = CalculateLength(lenID, 4, 1);//LENGTH
            message[10] = CalculateLength(lenID, 4, 2);//LENGTH
            message[11] = CalculateLength(lenID, 4, 3);//LENGTH
            message[12] = CalculateLength(lenID, 4, 4);//LENGTH
            message[13] = CalculateASCLL(ProtocalProvider.BatteryGroupCount, 2, 1);//CommandGroup 0x46代表要所有电池组 0X30 0X31代表要第一组
            message[14] = CalculateASCLL(ProtocalProvider.BatteryGroupCount, 2, 2);//CommandGroup 0x46代表要所有电池组
            message[message.Length - 5] = CalculateCHKSUM(message, 4, 1);//CHKSUM
            message[message.Length - 4] = CalculateCHKSUM(message, 4, 2);//CHKSUM
            message[message.Length - 3] = CalculateCHKSUM(message, 4, 3);//CHKSUM
            message[message.Length - 2] = CalculateCHKSUM(message, 4, 4);//CHKSUM
            message[message.Length - 1] = 0x0D;//EOI

            return message;
        }
        //获取遥测量信息（告警信息）
        internal static byte[] GetMessage_ReadWarning(byte address)
        {
            if (SerialPortCommunicator.checkVerisOld()) { { ProtocalProvider.BatteryGroupCount = 0x01; } } else { ProtocalProvider.BatteryGroupCount = address; }//20200429 //20200428
            int lenID = 0x02;

            byte[] message = new byte[20];

            message[0] = 0x7E;//SOI
            message[1] = 0x32;//VER
            message[2] = 0x30;//VER
            message[3] = CalculateASCLL(address, 2, 1);//ADR
            message[4] = CalculateASCLL(address, 2, 2);//ADR
            message[5] = 0x34;//CID1
            message[6] = 0x36;//CID1
            message[7] = 0x34;//CID2
            message[8] = 0x34;//CID2
            message[9] = CalculateLength(lenID, 4, 1);//LENGTH
            message[10] = CalculateLength(lenID, 4, 2);//LENGTH
            message[11] = CalculateLength(lenID, 4, 3);//LENGTH
            message[12] = CalculateLength(lenID, 4, 4);//LENGTH
            message[13] = CalculateASCLL(ProtocalProvider.BatteryGroupCount, 2, 1);//CommandGroup 0x46代表要所有电池组 0X30 0X31代表要第一组
            message[14] = CalculateASCLL(ProtocalProvider.BatteryGroupCount, 2, 2);//CommandGroup 0x46代表要所有电池组
            message[message.Length - 5] = CalculateCHKSUM(message, 4, 1);//CHKSUM
            message[message.Length - 4] = CalculateCHKSUM(message, 4, 2);//CHKSUM
            message[message.Length - 3] = CalculateCHKSUM(message, 4, 3);//CHKSUM
            message[message.Length - 2] = CalculateCHKSUM(message, 4, 4);//CHKSUM

            message[message.Length - 1] = 0x0D;//EOI

            return message;
        }
        // 获取读取状态的信息
        internal static byte[] GetMessage_ReadStatus(byte address)
        {
            if (SerialPortCommunicator.checkVerisOld()) { { ProtocalProvider.BatteryGroupCount = 0x01; } } else { ProtocalProvider.BatteryGroupCount = address; }//20200429 //20200428
            int lenID = 0x02;

            byte[] message = new byte[20];

            message[0] = 0x7E;//SOI
            message[1] = 0x32;//VER
            message[2] = 0x30;//VER
            message[3] = CalculateASCLL(address, 2, 1);//ADR
            message[4] = CalculateASCLL(address, 2, 2);//ADR
            message[5] = 0x34;//CID1
            message[6] = 0x36;//CID1
            message[7] = 0x34;//CID2
            message[8] = 0x33;//CID2
            message[9] = CalculateLength(lenID, 4, 1);//LENGTH
            message[10] = CalculateLength(lenID, 4, 2);//LENGTH
            message[11] = CalculateLength(lenID, 4, 3);//LENGTH
            message[12] = CalculateLength(lenID, 4, 4);//LENGTH
            message[13] = CalculateASCLL(ProtocalProvider.BatteryGroupCount, 2, 1);//CommandGroup 0x46代表要所有电池组 0X30 0X31代表要第一组
            message[14] = CalculateASCLL(ProtocalProvider.BatteryGroupCount, 2, 2);//CommandGroup 0x46代表要所有电池组
            message[message.Length - 5] = CalculateCHKSUM(message, 4, 1);//CHKSUM
            message[message.Length - 4] = CalculateCHKSUM(message, 4, 2);//CHKSUM
            message[message.Length - 3] = CalculateCHKSUM(message, 4, 3);//CHKSUM
            message[message.Length - 2] = CalculateCHKSUM(message, 4, 4);//CHKSUM
            message[message.Length - 1] = 0x0D;//EOI
            return message;
        }
        // 获取遥测量信息
        internal static byte[] GetMessage_ReadAnalog(byte address)
        {
            if (SerialPortCommunicator.checkVerisOld()) { { ProtocalProvider.BatteryGroupCount = 0x01; } } else { ProtocalProvider.BatteryGroupCount = address; }//20200428//20200429

            int lenID = 0x02;

            byte[] message = new byte[20];

            message[0] = 0x7E;//SOI
            message[1] = 0x32;//VER
            message[2] = 0x30;//VER
            message[3] = CalculateASCLL(address, 2, 1);//ADR
            message[4] = CalculateASCLL(address, 2, 2);//ADR
            message[5] = 0x34;//CID1
            message[6] = 0x41;//CID1
            message[7] = 0x34;//CID2
            message[8] = 0x32;//CID2
            message[9] = CalculateLength(lenID, 4, 1);//LENGTH
            message[10] = CalculateLength(lenID, 4, 2);//LENGTH
            message[11] = CalculateLength(lenID, 4, 3);//LENGTH
            message[12] = CalculateLength(lenID, 4, 4);//LENGTH
            message[13] = CalculateASCLL(ProtocalProvider.BatteryGroupCount, 2, 1);//CommandGroup 0x46代表要所有电池组 0X30 0X31代表要第一组
            message[14] = CalculateASCLL(ProtocalProvider.BatteryGroupCount, 2, 2);//CommandGroup 0x46代表要所有电池组
            message[message.Length - 5] = CalculateCHKSUM(message, 4, 1);//CHKSUM
            message[message.Length - 4] = CalculateCHKSUM(message, 4, 2);//CHKSUM
            message[message.Length - 3] = CalculateCHKSUM(message, 4, 3);//CHKSUM
            message[message.Length - 2] = CalculateCHKSUM(message, 4, 4);//CHKSUM
            message[message.Length - 1] = 0x0D;//EOI

            return message;
        }
        //获取Coslight电池模拟量信息
        internal static byte[] GetMessage_ReadAnalog_Coslight(byte address)
        {
            if (SerialPortCommunicator.checkVerisOld()) { { ProtocalProvider.BatteryGroupCount = 0x01; } } else { ProtocalProvider.BatteryGroupCount = address; }//20200429 //20200428
            int lenID = 0x02;

            byte[] message = new byte[20];

            message[0] = 0x7E;//SOI
            message[1] = 0x32;//VER
            message[2] = 0x30;//VER
            message[3] = CalculateASCLL(address, 2, 1);//ADR
            message[4] = CalculateASCLL(address, 2, 2);//ADR
            message[5] = 0x34;//CID1
            message[6] = 0x36;//CID1
            message[7] = 0x38;//CID2
            message[8] = 0x38;//CID2
            message[9] = CalculateLength(lenID, 4, 1);//LENGTH
            message[10] = CalculateLength(lenID, 4, 2);//LENGTH
            message[11] = CalculateLength(lenID, 4, 3);//LENGTH
            message[12] = CalculateLength(lenID, 4, 4);//LENGTH
            message[13] = CalculateASCLL(ProtocalProvider.BatteryGroupCount, 2, 1);//CommandGroup 0x46代表要所有电池组 0X30 0X31代表要第一组
            message[14] = CalculateASCLL(ProtocalProvider.BatteryGroupCount, 2, 2);//CommandGroup 0x46代表要所有电池组
            message[message.Length - 5] = CalculateCHKSUM(message, 4, 1);//CHKSUM
            message[message.Length - 4] = CalculateCHKSUM(message, 4, 2);//CHKSUM
            message[message.Length - 3] = CalculateCHKSUM(message, 4, 3);//CHKSUM
            message[message.Length - 2] = CalculateCHKSUM(message, 4, 4);//CHKSUM
            message[message.Length - 1] = 0x0D;//EOI
            return message;
        }
        //获取读取Coslight电池状态的信息
        internal static byte[] GetMessage_ReadStatus_Coslight(byte address)
        {
            if (SerialPortCommunicator.checkVerisOld()) { { ProtocalProvider.BatteryGroupCount = 0x01; } } else { ProtocalProvider.BatteryGroupCount = address; }//20200429 //20200428
            int lenID = 0x02;

            byte[] message = new byte[20];

            message[0] = 0x7E;//SOI
            message[1] = 0x32;//VER
            message[2] = 0x30;//VER
            message[3] = CalculateASCLL(address, 2, 1);//ADR
            message[4] = CalculateASCLL(address, 2, 2);//ADR
            message[5] = 0x34;//CID1
            message[6] = 0x36;//CID1
            message[7] = 0x38;//CID2
            message[8] = 0x39;//CID2
            message[9] = CalculateLength(lenID, 4, 1);//LENGTH
            message[10] = CalculateLength(lenID, 4, 2);//LENGTH
            message[11] = CalculateLength(lenID, 4, 3);//LENGTH
            message[12] = CalculateLength(lenID, 4, 4);//LENGTH
            message[13] = CalculateASCLL(ProtocalProvider.BatteryGroupCount, 2, 1);//CommandGroup 0x46代表要所有电池组 0X30 0X31代表要第一组
            message[14] = CalculateASCLL(ProtocalProvider.BatteryGroupCount, 2, 2);//CommandGroup 0x46代表要所有电池组
            message[message.Length - 5] = CalculateCHKSUM(message, 4, 1);//CHKSUM
            message[message.Length - 4] = CalculateCHKSUM(message, 4, 2);//CHKSUM
            message[message.Length - 3] = CalculateCHKSUM(message, 4, 3);//CHKSUM
            message[message.Length - 2] = CalculateCHKSUM(message, 4, 4);//CHKSUM
            message[message.Length - 1] = 0x0D;//EOI
            return message;
        }
        //获取Coslight电池读取警告信息
        internal static byte[] GetMessage_ReadWarning_Coslight(byte address)
        {
            if (SerialPortCommunicator.checkVerisOld()) { { ProtocalProvider.BatteryGroupCount = 0x01; } } else { ProtocalProvider.BatteryGroupCount = address; }//20200429 //20200428
            int lenID = 0x02;
            byte[] message = new byte[20];

            message[0] = 0x7E;//SOI
            message[1] = 0x32;//VER
            message[2] = 0x30;//VER
            message[3] = CalculateASCLL(address, 2, 1);//ADR
            message[4] = CalculateASCLL(address, 2, 2);//ADR
            message[5] = 0x34;//CID1
            message[6] = 0x36;//CID1
            message[7] = 0x38;//CID2
            message[8] = 0x41;//CID2
            message[9] = CalculateLength(lenID, 4, 1);//LENGTH
            message[10] = CalculateLength(lenID, 4, 2);//LENGTH
            message[11] = CalculateLength(lenID, 4, 3);//LENGTH
            message[12] = CalculateLength(lenID, 4, 4);//LENGTH
            message[13] = CalculateASCLL(ProtocalProvider.BatteryGroupCount, 2, 1);//CommandGroup 0x46代表要所有电池组 0X30 0X31代表要第一组
            message[14] = CalculateASCLL(ProtocalProvider.BatteryGroupCount, 2, 2);//CommandGroup 0x46代表要所有电池组
            message[message.Length - 5] = CalculateCHKSUM(message, 4, 1);//CHKSUM
            message[message.Length - 4] = CalculateCHKSUM(message, 4, 2);//CHKSUM
            message[message.Length - 3] = CalculateCHKSUM(message, 4, 3);//CHKSUM
            message[message.Length - 2] = CalculateCHKSUM(message, 4, 4);//CHKSUM
            message[message.Length - 1] = 0x0D;//EOI
            return message;
        }
        //检查信息
        public static bool CheckMessage(byte[] message)
        {
            for (int i = 1; i <= 4; i++)//校验和有两个字节16位
            {
                if (message[message.Length + i - 6] != CalculateCHKSUM(message, 4, i))
                {
                    return false;
                }
            }

            return true;
        }    
        // 将一个字节数组中的数据解析为十六进制格式
        internal static byte[] CalculateHexMessage(byte[] message)
        {
            byte[] hexMessage = new byte[message.Length / 2 + 1];

            hexMessage[0] = message[0];
            hexMessage[hexMessage.Length - 1] = message[message.Length - 1];

            for (int i = 0; i != message.Length / 2 - 1; i++)
            {
                hexMessage[i + 1] = Convert.ToByte(ASCIIToString(message[1 + i * 2]) + ASCIIToString(message[2 + i * 2]), 16);
            }

            return hexMessage;
        }
    }
}

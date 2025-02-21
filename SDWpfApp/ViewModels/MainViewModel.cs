using SDWpfApp.Models;
using SDWpfApp.Views;
using DevExpress.Mvvm.POCO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using DevExpress.Mvvm;
using System.IO.Ports;
using System.Configuration;
using static DevExpress.Utils.Frames.FrameHelper;
using DevExpress.Xpf.WindowsUI.Navigation;


namespace SDWpfApp.ViewModels
{

    public class MainViewModel
    {
        public virtual List<string> AvaliablePorts { get; set; }
        public virtual List<string> AvaliableBaudRate { get; set; }
        private List<ResourceDictionary> DictionaryList;
        public List<string> ListCommunicationType { get; set; }
        public virtual bool IsReadParameter { get; set; }
        public virtual bool IsHistoryDataViewActivated { get; set; }
        public virtual bool IsDeviceHistoryDataViewActivated { get; set; }
        public virtual string AccCHCapacity { get; set; }//充电容量
        public virtual string AccDCHCapacity { get; set; }//放电容量
        private string filePath { get; set; }//文件路径
        private string LastView { get; set; }
        private string Language { get; set; }//语言
        public static float CellCapacity { get; set; } = 150;//电池容量
        public int IDWrite { get; private set; }
        private byte SelectedPackAddress { get; set; }//勾选包地址
        #region 48100C5 parameter
        public virtual string DryContactSetViewName { get; set; } = "DryContactSetView";
        public virtual string SystemSupervisionViewName { get; set; } = "SystemSupervisionView";
        public virtual string SOC { get; set; }
        private string currentView { get; set; }

        #endregion
        public XlsFileControler xlsFileControler { get; set; }

        public virtual ObservableCollection<CalibrationParameter> CalibrationParameterCollection { get; set; }
        public virtual ObservableCollection<SystemParameter> SystemParameterCollection { get; set; }
        public virtual ObservableCollection<SystemParameter> SystemParameter_1_Collection { get; set; }
        public virtual ObservableCollection<SystemParameter> SystemParameter_2_Collection { get; set; }
        public virtual ObservableCollection<DeviceRunningRecord_DPC> DeviceRunningRecords { get; set; }
        public virtual ObservableCollection<DeviceWarningRecord_COSPOWERS> DeviceWarningRecords_COSPOWERS { get; set; }
        public virtual ObservableCollection<DeviceWarningRecord> DeviceWarningRecords { get; set; }//设备警告记录
        //使能控制集
        public virtual ObservableCollection<SpecialParameter> EnableControlCollection { get; set; }
        public virtual ObservableCollection<DryContact2Item> DryContact2Collection { get; set; }
        public virtual ObservableCollection<DeviceEventRecord> DeviceEventRecords { get; set; }
        public virtual ObservableCollection<DryContactItem> DryContactCollection { get; set; }
        public virtual ObservableCollection<WlanParameter> WlanParameterCollection { get; set; }
        public virtual ObservableCollection<DataRecord> Records { get; set; }
        public virtual bool IsLoadingSpecialSystemParameter { get; set; }
        public virtual bool IsLoadingSystemParameter { get; set; }
        public virtual bool IsEnglishUI { get; set; }//是否是英文UI
        public virtual bool IsChineseUI { get; set; }//是否是中文UI
        public virtual bool IsLoadingCoslightSystemParameter { get; set; }//是否正在下载光宇系统参数
        public virtual bool IsAdministrator { get; set; }
        public virtual bool boolCommunicationTypeZTE { get; set; }
        public virtual bool boolCommunicationTypeDPC { get; set; }
        public bool IsZTE检测中心 { get; set; }
        //通讯
        private SerialPortCommunicator communicator { get; set; }
        public virtual ObservableCollection<Pack> PackCollection { get; set; }

        public static CommunicationType communicationType = new CommunicationType();
        protected virtual IMessageBoxService MessageBoxService { get { return null; } }
        protected virtual INavigationService NavigationService { get { return null; } }
        #region 4G+GPS参数     
        public static bool IsReadGPSAnolog { get; set; }
        public virtual bool IsLoading_4GSystemParameter { get; set; }
        public virtual bool IsLoadingGPSSystemParameter { get; set; }
        public virtual bool IsLoadingGPS_4GEnableControlParameter { get; set; }
        public virtual int TimeZoneHour_Local { get; set; }
        public virtual int TimeZoneMinutes_Local { get; set; }
        public virtual string Date_Local { get; set; }//当地日期
        public virtual string UTC_Local { get; set; }//
        public virtual string LTM_Local { get; set; }
        public virtual string Parameter_4G_AdministratorMobilePhoneNumber { get; set; }
        public virtual string Parameter_4G_CellPhoneNumber1 { get; set; }

        public virtual string Parameter_4G_CellPhoneNumber2 { get; set; }

        public virtual string Parameter_4G_CellPhoneNumber3 { get; set; }

        public virtual string Parameter_4G_CellPhoneNumber4 { get; set; }

        public virtual string Parameter_4G_CellPhoneNumber5 { get; set; }
        public virtual string WlanParameter_HeartbeatCycle { get; set; }
        public virtual string WlanParameter_APN { get; set; }
        public virtual string WlanParameter_APN_Account { get; set; }
        public virtual string WlanParameter_APN_Password { get; set; }

        public virtual string ParameterGPS_StationID { get; set; }

        public virtual string ParameterGPS_Longitude { get; set; }

        public virtual string ParameterGPS_Latitude { get; set; }

        public virtual string ParameterGPS_Altitude { get; set; }

        public virtual string ParameterGPS_TimeZone { get; set; }
        public virtual ObservableCollection<WlanEnableControl> WlanEnableControlCollection { get; set; }

        #endregion 4G+GPS参数

        #region BootLoader
        public byte[,] byteDatalong = new byte[999, 512 * 2];
        byte[] byteData = new byte[512 * 2];
        public virtual bool isBootRead { get; set; }
        public virtual bool isErases { get; set; }
        public virtual bool isApplicationRead { get; set; }
        public virtual bool isWriteSuccess { get; set; }
        public virtual int frameCode { get; set; }
        public virtual int BarBindingVal { get; set; }
        public string BootLoaderFileName { get; set; }
        public string ApplicationFileName01 { get; set; }
        public string ApplicationFileName02 { get; set; }
        public virtual string BootLaoderFileNameWrite { get; set; }
        public virtual string BootLaoderFileNameWriteZip { get; set; }
        public virtual string BootLaoderFileNameRead { get; set; }
        public virtual string ProjectCodeRead { get; set; }
        public virtual string COSPowerCodeRead { get; set; }
        public virtual string DevelopmentPhaseRead { get; set; }
        public virtual string VersionNumber1Read { get; set; }
        public virtual string VersionNumber2Read { get; set; }
        public virtual string VersionNumber3Read { get; set; }
        public virtual string OtherRead { get; set; }
        public virtual string ProjectCodeWrite { get; set; }
        public virtual string COSPowerCodeWrite { get; set; }
        public virtual string DevelopmentPhaseWrite { get; set; }
        public virtual string VersionNumber1Write { get; set; }
        public virtual string VersionNumber2Write { get; set; }
        public virtual string VersionNumber3Write { get; set; }
        public virtual string OtherWrite { get; set; }
        public virtual string StateContectInformation { get; set; }
        private string FileFolder { get; set; }
        public virtual BootLoaderFileNameCheck bootLoaderFileNameCheck { get; set; }
        #endregion BootLoader
        #region 陀螺仪
        public static bool IsReadGyroSensorAnolog { get; set; }
        public virtual string XRead { get; set; } //pitch
        public virtual string YRead { get; set; } //yaw
        public virtual string ZRead { get; set; } //roll
        #endregion 陀螺仪

        #region 设备信息
        public virtual string HardwarVersion_Coslight { get; set; }
        public virtual string ProjectCode_Coslight { get; set; }
        public virtual string SoftwareVersion_Coslight { get; set; }
        public virtual string PCBNumber_Coslight { get; set; }
        #endregion 设备信息

        #region 防盗
        public static bool IsReadAntiTheftAnolog { get; set; }
        public virtual bool IsReadAgainstTheftParameter { get; set; }
        public virtual bool isAntiTheftMasterControl { get; set; }
        public virtual bool isAntiTheftCommunicationControl { get; set; }
        public virtual bool isAntiTheftGyroSensorControl { get; set; }
        public virtual bool isAntiTheftSecurityLineControl { get; set; }
        public virtual bool isAntiTheftGPSControl { get; set; }
        ////状态开关
        public virtual bool isHandControlState { get; set; }
        public virtual bool isOneKeyPostEmergencyState { get; set; }
        //
        public virtual int AntiTheftCommunicationDelayTime { get; set; }
        public virtual int AntiTheftGyroSensorAngleInclination { get; set; }
        public virtual int AntiTheftSecurityLineDelayTime { get; set; }
        public virtual int AntiTheftGPSMovingDistance { get; set; }
        //
        public virtual string PostEmergencyState { get; set; }
        public virtual string AntiTheftCommunicationWaring { get; set; }
        public virtual string AntiTheftGyroSensorWaring { get; set; }
        public virtual string AntiTheftSecurityLineWaring { get; set; }
        public virtual string AntiTheftGPSWaring { get; set; }

        public virtual string AntiTheftCommunicationState { get; set; }
        public virtual string AntiTheftGyroSensorState { get; set; }
        public virtual string AntiTheftSecurityLineState { get; set; }
        public virtual string AntiTheftGPSState { get; set; }
        public virtual string WlanGridState { get; set; } = "Collapsed";
        public virtual string WlanContentState { get; set; } = "Visible";

        public virtual string lastAntiTheftMasterState { get; set; }
        public virtual string lastAntiTheftCommunicationState { get; set; }
        public virtual string lastAntiTheftGyroSensorState { get; set; }
        public virtual string lastAntiTheftSecurityLineState { get; set; }
        public virtual string lastAntiTheftGPSState { get; set; }
        public virtual string lastHandControlState { get; set; }
        public virtual string lastOneKeyPostEmergencyState { get; set; }
        public virtual string isAntiTheftCommunicationWaringRed { get; set; } = "Collapsed";
        public virtual string isAntiTheftGyroSensorWaringRed { get; set; } = "Collapsed";
        public virtual string isAntiTheftSecurityLineWaringRed { get; set; } = "Collapsed";
        public virtual string isAntiTheftGPSWaringRed { get; set; } = "Collapsed";

        public virtual string isAntiTheftCommunicationWaringGreen { get; set; } = "Collapsed";
        public virtual string isAntiTheftGyroSensorWaringGreen { get; set; } = "Collapsed";
        public virtual string isAntiTheftSecurityLineWaringGreen { get; set; } = "Collapsed";
        public virtual string isAntiTheftGPSWaringGreen { get; set; } = "Collapsed";

        public virtual string isAntiTheftCommunicationWaringGray { get; set; } = "Visible";
        public virtual string isAntiTheftGyroSensorWaringGray { get; set; } = "Visible";
        public virtual string isAntiTheftSecurityLineWaringGray { get; set; } = "Visible";
        public virtual string isAntiTheftGPSWaringGray { get; set; } = "Visible";
        public virtual string AntiTheftMasterState { get; set; }
        public virtual string AntiTheftCommunicationControl { get; set; }
        public virtual string AntiTheftGyroSensorControl { get; set; }
        public virtual string AntiTheftSecurityLineControl { get; set; }
        public virtual string AntiTheftGPSControl { get; set; }
        public virtual string HandControlState { get; set; }
        public virtual string OneKeyPostEmergencyState { get; set; }
        public virtual string PackIniState { get; set; }
        public int intSetParameterCount { get; set; }
        #endregion
        #region 权限管理
        public virtual string CustomerManagerState { get; set; }
        public virtual string CommunicationTypeState { get; set; }
        public virtual bool IsManagerState { get; set; }
        public virtual string SystemSetViewState { get; set; }
        public virtual string EnableControlViewState { get; set; }
        public virtual string DryContactSetViewState { get; set; }
        public virtual string SystemSetGPS_4GViewState { get; set; }
        public virtual string BootLoaderViewState { get; set; }
        public virtual string GyroSensorViewState { get; set; }
        public virtual string AgainstTheftViewState { get; set; }
        public virtual string CPTViewState { get; set; }
        public virtual string OthersViewState { get; set; }
        public virtual string VerName { get; set; }
        #endregion
        //构造函数
        public MainViewModel()
        {
            InitialSystem();
            IsZTE检测中心 = true;
        }
        // 添加 SimulateTestMessage 方法
    public void SimulateTestMessage(byte packId)
    {
        SerialPortCommunicator.LastSendCommandType = CommandType.读取模拟量;
    
        byte[] testMessage = new byte[] {
            0x7E,       // SOI
            0x20,       // VER
            packId,     // ADR (设备地址)
            0x00,       // 应答标志
            0x00,       // 保留
            0x4A,       // CID1
            0x42,       // CID2
            0x00,       // DATA_FLAG
            packId,     // Pack ID
            
            0x10,       // 单体电池数量 (16)
            
            // 16个单体电池电压数据 (每个2字节)
            0x0C, 0xB6, // 3256mV (3.256V)
            0x0C, 0xB6, // 3256mV
            0x0C, 0xB6, // 3256mV
            0x0C, 0xB6, // 3256mV
            0x0C, 0xB6, // 3256mV
            0x0C, 0xB6, // 3256mV
            0x0C, 0xB6, // 3256mV
            0x0C, 0xB6, // 3256mV
            0x0C, 0xB6, // 3256mV
            0x0C, 0xB6, // 3256mV
            0x0C, 0xB6, // 3256mV
            0x0C, 0xB6, // 3256mV
            0x0C, 0xB6, // 3256mV
            0x0C, 0xB6, // 3256mV
            0x0C, 0xB6, // 3256mV
            0x0C, 0xB6, // 3256mV

            0x04,       // 电芯温度数量 (N=4)
            
            // 温度数据 (单位0.1K, 实际温度℃=(传送值-2731)/10)
            0x0B, 0xAA, // 2986 (0.1K) -> 25.5℃
            0x0B, 0xAA, // 2986 (0.1K) -> 25.5℃
            0x0B, 0xAA, // 2986 (0.1K) -> 25.5℃
            0x0B, 0xAA, // 2986 (0.1K) -> 25.5℃
            
            // 环境温度
            0x0B, 0xAA, // 2986 (0.1K) -> 25.5℃
            // MOS温度
            0x0B, 0xAA, // 2986 (0.1K) -> 25.5℃

            // 电流数据 (2字节)
            0x03, 0xE8, // 1000 -> 10A
            
            // 总电压数据 (2字节)
            0x0F, 0xA0, // 4000 -> 40V
            
            // 电池剩余容量 (2字节)
            0x0C, 0x80, // 3200 -> 32AH
            
            // 电池总容量 (2字节)
            0x19, 0x00, // 6400 -> 64AH
            
            // 电池循环次数 (2字节)
            0x00, 0x64, // 100次

            0x01,       // 自定义遥测量数量P
            
            // SOH数据 (2字节)
            0x00, 0x64, // 100%
            
            // 母线电压 (2字节)
            0x0F, 0xA0, // 4000 -> 40V

            0x00, 0x00, // CHKSUM
            0x0D        // EOI
        };

        // 设置通信类型为DPC
        communicationType = CommunicationType.DPC;

        // 触发测试
        communicator?.SimulateReceiveForTest(testMessage);
    }
        //删除Temp文件夹下所有内容
        private void deleteTemp()
        {
            //获取Temp路径
            DirectoryInfo d = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "Temp");
            //获取所有文件和文件夹
            FileSystemInfo[] fsinfos = d.GetFileSystemInfos();
            //遍历删除
            foreach (FileSystemInfo fsinfo in fsinfos)
            {
                if (fsinfo is DirectoryInfo)     //判断是否为文件夹
                {
                    string path = fsinfo.FullName;
                    if (Directory.Exists(path))
                    {
                        //获取该路径下的文件路径
                        string[] filePathList = Directory.GetFiles(path);
                        foreach (string filePath in filePathList)
                        {
                            File.Delete(filePath);
                        }
                        //如果存在则删除
                        Directory.Delete(path);
                    }
        }
        else
        {
                    File.Delete(fsinfo.FullName);
                }
            }
        }
        //初始化波特率
        public void InitializeBaudRate()//20200604
        {
            AvaliableBaudRate = new List<string> { "9600", "115200" };
        }
        //选择波特率
        public void BaudRateSelected(string BaudRate)//20200604
        {
            communicator.intBaudRate = int.Parse(BaudRate);
            MessageBoxService.Show("选择波特率"+ BaudRate);
        }
        //获取串口列表
        public void RetriveAvaliablePort()
        {
            AvaliablePorts = SerialPort.GetPortNames().ToList();
            //SimulateTestMessage(0x01);
        }
        //选择串口,并创建HistoryData文件
        public void PortSelected(string portName)
        {
            communicator.PortName = portName;
            MessageBoxService.Show("选择端口"+portName);
            communicator.OnPortNameChanged();//20200604
            MessageBoxService.Show("选择端口"+portName+"完成");
            if (boolCommunicationTypeDPC)
            {
                this.filePath = AppDomain.CurrentDomain.BaseDirectory + "HistoryData_DPC\\" + DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ".dat";
            }
            if (boolCommunicationTypeZTE)
            {
                this.filePath = AppDomain.CurrentDomain.BaseDirectory + "HistoryData\\" + DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ".dat";
            }
        }
        //初始化通讯类型列表     
        private void InitialzeCommunicationType()
        {
            ListCommunicationType = new List<string> { "DPC" };
        }
        //读取干接点设置
        internal void ReadDryContactSettings(byte packAddress)//20200601
        {
            if (string.IsNullOrEmpty(SerialPortCommunicator.ManufacturerName))
            {
                communicator.UserAction(ProtocalProvider.GetMessage_ManufacturerInfo(packAddress));
            }

            this.SelectedPackAddress = packAddress;

            this.DryContactCollection.Clear();

            IsLoadingSpecialSystemParameter = true;

            IsReadParameter = true;

            communicator.UserAction(ProtocalProvider.GetMessage_ReadDryContactStatus(packAddress));
        }
        //DPC通讯读取
        internal void ReadSpecialValue_DPC(byte packAddress)
        {
            if (SerialPortCommunicator.CommunicationProtocoVersionNumber == 0)
            {
                communicator.UserAction(ProtocalProvider.SendMessage(packAddress, CID2_Type.获取通信协议版本号));
            }
            this.SelectedPackAddress = packAddress;

            this.EnableControlCollection.Clear();

            IsLoadingSpecialSystemParameter = true;

            communicator.UserAction(ProtocalProvider.SendMessage(packAddress, CID2_Type.获取遥调量信息));

            IsReadParameter = true;
        }
        //ZTE通讯读取
        internal void ReadSpecialValue_ZTE(byte packAddress)
        {
            if (string.IsNullOrEmpty(SerialPortCommunicator.ManufacturerName))
            {
                communicator.UserAction(ProtocalProvider.GetMessage_ManufacturerInfo(packAddress));
            }

            this.SelectedPackAddress = packAddress;

            this.EnableControlCollection.Clear();

            IsLoadingSpecialSystemParameter = true;

            if (SerialPortCommunicator.ManufacturerName == "ZTE-C" && SerialPortCommunicator.ZTE_Type == "FB150C")
            {
                communicator.UserAction(ProtocalProvider.GetMessage_ReadSpecialParameter(packAddress));
            }
            else
            {
                communicator.UserAction(ProtocalProvider.GetMessage_ReadSpecialParameter(packAddress));

                communicator.UserAction(ProtocalProvider.GetMessage_ReadSpecialParameter_Coslight(packAddress));
            }

            IsReadParameter = true;
        }
        //读取特殊值
        internal void ReadSpecialValue(byte packAddress)//20200529//20200601
        {
            if (communicationType == CommunicationType.DPC)
            {
                ReadSpecialValue_DPC(packAddress);
            }
            else
            {
                ReadSpecialValue_ZTE(packAddress);
            }
        }
        //切换语言
        private void SwitchLanguage(string language)
        {
            IsEnglishUI = language == "en-US";
            IsChineseUI = language == "zh-CN";

            Pack.IsChineseUI = IsChineseUI;//20200423
            Pack.IsEnglishUI = IsEnglishUI;//20200423
            Language = language;          
            string resourcePath = $"pack://application:,,,/SDWpfApp;component/Resources/Languages/{language}.xaml";
            Console.WriteLine("尝试加载资源路径：" + resourcePath);
            try
            {
                var resourceDictionary = new ResourceDictionary { Source = new Uri(resourcePath, UriKind.Absolute) };
                System.Windows.Application.Current.Resources.MergedDictionaries.Remove(resourceDictionary);
                System.Windows.Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);
            }
            catch (Exception ex)
            {
                Console.WriteLine("加载资源失败：" + ex.Message);
                throw new FileNotFoundException($"未找到资源文件: {resourcePath}");
            }
            if (currentView == "DryContactSetView")
            {
                ReadDryContactSettings(this.SelectedPackAddress);
            }
            else if (currentView == "EnableControlView")
            {
                ReadSpecialValue(this.SelectedPackAddress);
            }
            else if (currentView == "HistoryDataView")
            {
                foreach (DataRecord record in Records)
                {
                    //调整ENUM的值为中文或者英文
                    if (IsChineseUI)
                    {
                        record.ChargeCircuitSwitchStatus = (int)record.ChargeCircuitSwitchStatus <= 2 ? record.ChargeCircuitSwitchStatus : record.ChargeCircuitSwitchStatus - 3;
                        record.DischargeCircuitSwitchStatus = (int)record.DischargeCircuitSwitchStatus <= 2 ? record.DischargeCircuitSwitchStatus : record.DischargeCircuitSwitchStatus - 3;
                        record.BeeperSwitchStatus = (int)record.BeeperSwitchStatus <= 2 ? record.BeeperSwitchStatus : record.BeeperSwitchStatus - 3;
                        record.HeatingFilmSwitchStatus = (int)record.HeatingFilmSwitchStatus <= 2 ? record.HeatingFilmSwitchStatus : record.HeatingFilmSwitchStatus - 3;
                        record.FullyChargedStatus = (int)record.FullyChargedStatus <= 2 ? record.FullyChargedStatus : record.FullyChargedStatus - 3;
                        record.BatteryStatus = (int)record.BatteryStatus <= 5 ? record.BatteryStatus : record.BatteryStatus - 6;
                        record.BatteryChargingRequestFlag = (int)record.BatteryChargingRequestFlag <= 2 ? record.BatteryChargingRequestFlag : record.BatteryChargingRequestFlag - 3;
                    }
                    else
                    {
                        record.ChargeCircuitSwitchStatus = (int)record.ChargeCircuitSwitchStatus >= 3 ? record.ChargeCircuitSwitchStatus : record.ChargeCircuitSwitchStatus + 3;
                        record.DischargeCircuitSwitchStatus = (int)record.DischargeCircuitSwitchStatus >= 3 ? record.DischargeCircuitSwitchStatus : record.DischargeCircuitSwitchStatus + 3;
                        record.BeeperSwitchStatus = (int)record.BeeperSwitchStatus >= 3 ? record.BeeperSwitchStatus : record.BeeperSwitchStatus + 3;
                        record.HeatingFilmSwitchStatus = (int)record.HeatingFilmSwitchStatus >= 3 ? record.HeatingFilmSwitchStatus : record.HeatingFilmSwitchStatus + 3;
                        record.FullyChargedStatus = (int)record.FullyChargedStatus >= 3 ? record.FullyChargedStatus : record.FullyChargedStatus + 3;
                        record.BatteryStatus = (int)record.BatteryStatus >= 6 ? record.BatteryStatus : record.BatteryStatus + 6;
                        record.BatteryChargingRequestFlag = (int)record.BatteryChargingRequestFlag >= 3 ? record.BatteryChargingRequestFlag : record.BatteryChargingRequestFlag + 3;
                    }
                }
            }
        }
        //切换成中文
        public void SwitchToChinese()
        {
            SwitchLanguage("zh-CN");

            Configuration Config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            AppSettingsSection appSection = (AppSettingsSection)Config.GetSection("appSettings");

            appSection.Settings["UserLanguagePreference"].Value = "Chinese";
            Config.Save(ConfigurationSaveMode.Minimal);
            //调整为中文
            foreach (Pack pack in PackCollection)
            {
                pack.CommunicationStatus = (int)pack.CommunicationStatus <= 2 ? pack.CommunicationStatus : pack.CommunicationStatus - 3;
                pack.AntiThief = (int)pack.AntiThief <= 2 ? pack.AntiThief : pack.AntiThief - 3;
                pack.PreChargeStatus_Coslight = (int)pack.PreChargeStatus_Coslight <= 9 ? pack.PreChargeStatus_Coslight : pack.PreChargeStatus_Coslight - 16;
                pack.LimitCurrentStatus_Coslight = (int)pack.LimitCurrentStatus_Coslight <= 2 ? pack.LimitCurrentStatus_Coslight : pack.LimitCurrentStatus_Coslight - 3;
                pack.SingleCellTemperaturePowerStatus_Coslight = (int)pack.SingleCellTemperaturePowerStatus_Coslight <= 2 ? pack.SingleCellTemperaturePowerStatus_Coslight : pack.SingleCellTemperaturePowerStatus_Coslight - 3;
                pack.EEPROMPowerStatus_Coslight = (int)pack.EEPROMPowerStatus_Coslight <= 2 ? pack.EEPROMPowerStatus_Coslight : pack.EEPROMPowerStatus_Coslight - 3;
                pack.CommunicationPowerStatus_Coslight = (int)pack.CommunicationPowerStatus_Coslight <= 2 ? pack.CommunicationPowerStatus_Coslight : pack.CommunicationPowerStatus_Coslight - 3;
                pack.BusBarMeasurePowerStatus_Coslight = (int)pack.BusBarMeasurePowerStatus_Coslight <= 2 ? pack.BusBarMeasurePowerStatus_Coslight : pack.BusBarMeasurePowerStatus_Coslight - 3;
                pack.PowerOnSwitchKeyStatus_Coslight = (int)pack.PowerOnSwitchKeyStatus_Coslight <= 2 ? pack.PowerOnSwitchKeyStatus_Coslight : pack.PowerOnSwitchKeyStatus_Coslight - 3;

            }

        }
        //切换成英文
        public void SwitchToEnglish()
        {
            SwitchLanguage("en-US");

            Configuration Config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            AppSettingsSection appSection = (AppSettingsSection)Config.GetSection("appSettings");

            appSection.Settings["UserLanguagePreference"].Value = "English";
            Config.Save(ConfigurationSaveMode.Minimal);

            foreach (Pack pack in PackCollection)
            {
                pack.CommunicationStatus = (int)pack.CommunicationStatus >= 3 ? pack.CommunicationStatus : pack.CommunicationStatus + 3;
                pack.AntiThief = (int)pack.AntiThief >= 3 ? pack.AntiThief : pack.AntiThief + 3;
                pack.PreChargeStatus_Coslight = (int)pack.PreChargeStatus_Coslight >= 16 ? pack.PreChargeStatus_Coslight : pack.PreChargeStatus_Coslight + 16;
                pack.LimitCurrentStatus_Coslight = (int)pack.LimitCurrentStatus_Coslight >= 2 ? pack.LimitCurrentStatus_Coslight : pack.LimitCurrentStatus_Coslight + 3;
                pack.SingleCellTemperaturePowerStatus_Coslight = (int)pack.SingleCellTemperaturePowerStatus_Coslight >= 2 ? pack.SingleCellTemperaturePowerStatus_Coslight : pack.SingleCellTemperaturePowerStatus_Coslight + 3;
                pack.EEPROMPowerStatus_Coslight = (int)pack.EEPROMPowerStatus_Coslight >= 2 ? pack.EEPROMPowerStatus_Coslight : pack.EEPROMPowerStatus_Coslight + 3;
                pack.CommunicationPowerStatus_Coslight = (int)pack.CommunicationPowerStatus_Coslight >= 2 ? pack.CommunicationPowerStatus_Coslight : pack.CommunicationPowerStatus_Coslight + 3;
                pack.BusBarMeasurePowerStatus_Coslight = (int)pack.BusBarMeasurePowerStatus_Coslight >= 2 ? pack.BusBarMeasurePowerStatus_Coslight : pack.BusBarMeasurePowerStatus_Coslight + 3;
                pack.PowerOnSwitchKeyStatus_Coslight = (int)pack.PowerOnSwitchKeyStatus_Coslight >= 2 ? pack.PowerOnSwitchKeyStatus_Coslight : pack.PowerOnSwitchKeyStatus_Coslight + 3;
            }

        }
        //
        public void ViewHiddleOrVisbleControl()
        {

            VerName = "Version:1.1.1 Trial";

            communicationType = CommunicationType.DPC;
            CustomerManagerState = "Collapsed";
            CommunicationTypeState = "Visible";

            SystemSetViewState = "Visible";
            EnableControlViewState = "Visible";
            DryContactSetViewState = "Collapsed";
            SystemSetGPS_4GViewState = "Collapsed";
            BootLoaderViewState = "Visible";
            GyroSensorViewState = "Collapsed";
            AgainstTheftViewState = "Collapsed";
            CPTViewState = "Collapsed";
            OthersViewState = "Visible";

            IsManagerState = false;
            this.IsAdministrator = false;

            boolCommunicationTypeDPC = true;
            boolCommunicationTypeZTE = false;

            //SystemSupervisionViewName = "SystemSupervisionView_DPC";

            SerialPortCommunicator.boolCommunicationTypeDPC = boolCommunicationTypeDPC;
            SerialPortCommunicator.boolCommunicationTypeZTE = boolCommunicationTypeZTE;

            foreach (Pack pack in this.PackCollection)
            {
                pack.IsCommunicationEnabled = false;
            }
        }
        private void InitialSystem()
        {           
            //创建必要文件夹目录
            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "Temp"))
            {
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "Temp");
            }
            deleteTemp();//清理文件夹下内容
            //
            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "Register"))
            {
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "Register");
            }
            //
            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "RegisterExport"))
            {
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "RegisterExport");
            }
            //历史数据
            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "HistoryData"))
            {
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "HistoryData");
            }
            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "HistoryData_DPC"))
            {
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "HistoryData_DPC");
            }
            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "Log"))
            {
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "Log");
            }

            xlsFileControler = new XlsFileControler();

            DictionaryList = new List<ResourceDictionary>();
            foreach (ResourceDictionary dictionary in System.Windows.Application.Current.Resources.MergedDictionaries)
            {
                DictionaryList.Add(dictionary);
            }
            //初始化
            DeviceWarningRecords_COSPOWERS = new ObservableCollection<DeviceWarningRecord_COSPOWERS>();

            PackCollection = new ObservableCollection<Pack>();
            SystemParameterCollection = new ObservableCollection<Models.SystemParameter>();
            SystemParameter_1_Collection = new ObservableCollection<Models.SystemParameter>();
            CalibrationParameterCollection = new ObservableCollection<CalibrationParameter>();
            EnableControlCollection = new ObservableCollection<SpecialParameter>();
            DryContactCollection = new ObservableCollection<DryContactItem>();
            DeviceRunningRecords = new ObservableCollection<DeviceRunningRecord_DPC>();
            DeviceWarningRecords = new ObservableCollection<DeviceWarningRecord>();

            DeviceEventRecords = new ObservableCollection<DeviceEventRecord>();//20200610

            WlanParameterCollection = new ObservableCollection<WlanParameter>();//20200626
            WlanEnableControlCollection = new ObservableCollection<WlanEnableControl>();//20200626

            DryContact2Collection = new ObservableCollection<DryContact2Item>();//20200522

            SystemParameter_2_Collection = new ObservableCollection<SystemParameter>();//20200522
           
            for (byte i = 1; i != 17; i++)
            {
                PackCollection.Add(ViewModelSource.Create(() => new Pack(this){ PackID = i }));

            }

            //通讯
             communicator = ViewModelSource.Create(() => new SerialPortCommunicator(PackCollection));
            this.communicator.ReceiveEvent += communicator_ReceiveEvent;//绑定接收处理事件
            this.communicator.ReceiveFailureEvent += Communicator_ReceiveFailureEvent;//处理接收失败事件
            //
            InitializeBaudRate();
            RetriveAvaliablePort();
            InitialzeCommunicationType();
            IsChineseUI = ConfigurationManager.AppSettings["UserLanguagePreference"] == "Chinese";
            if (IsChineseUI)
            {
                SwitchToChinese();
            }
            else
            {
                SwitchToEnglish();
            }
            ViewHiddleOrVisbleControl();                           
        }
        //View Load
        public void OnViewLoaded()
        {
            NavigationService.Navigate(SystemSupervisionViewName, null, this);             
        }
        //导航
        public void Navigate(string view)
        {
            if (view == null)
            {
                view = SystemSupervisionViewName;
            }
            if (communicationType == CommunicationType.DPC || communicationType == CommunicationType.移动)
            {
                IsHistoryDataViewActivated = view == "HistoryDataView";

                IsDeviceHistoryDataViewActivated = view == "DeviceHistoryDataView";

                currentView = view;

                if (LastView == null)
                {
                    LastView = SystemSupervisionViewName;
                }

                if (LastView == SystemSupervisionViewName && currentView != "SystemSetView" && currentView != LastView)
                {
                    foreach (Pack pack in this.PackCollection)//20200719
                    {
                        pack.IsCommunicationEnabled = false;
                    }
                }
                if (LastView == "BootLoaderView" && currentView != LastView)
                {
                    if (communicator.intBaudRate == 19200)
                    {
                        communicator.intBaudRate = 9600;
                        communicator.OnPortNameChanged();
                    }

                    SerialPortCommunicator.BLStatus = BootLaoderStatus.应用程序;
                }

                LastView = view;//20200508

                if (currentView == "BootLoaderView")//20200508
                {

                    foreach (Pack pack in this.PackCollection)//20200606
                    {
                        pack.IsCommunicationEnabled = false;
                    }
                }



                NavigationService.Navigate(view, null, this);
            }
            else
            {
                if (SerialPortCommunicator.Manager == AuthorizationManager.开发版)
                {
                    RegisterViewModel.AuthorityStatus = AuthorityResult.RegisterOK;
                }

                if (RegisterViewModel.AuthorityStatus == AuthorityResult.RegisterOK || IsZTE检测中心)
                {
                    IsHistoryDataViewActivated = view == "HistoryDataView";

                    IsDeviceHistoryDataViewActivated = view == "DeviceHistoryDataView";

                    IsReadAgainstTheftParameter = !(view == "AgainstTheftView");

                    currentView = view;

                    if (LastView == null)
                    {
                        LastView = "SystemSupervisionView";
                    }

                    if (LastView == "SystemSetGPS_4GView" && currentView != LastView)//20200508
                    {
                        IsReadGPSAnolog = false;
                    }

                    if (LastView == "GyroSensorView" && currentView != LastView)//20200526
                    {
                        IsReadGyroSensorAnolog = false;
                    }

                    if (LastView == "AgainstTheftView" && currentView != LastView)//20200526
                    {
                        IsReadAntiTheftAnolog = false;

                        communicator.UserAction_Clear();
                    }

                    if (LastView == "BootLoaderView" && currentView != LastView)//20200508
                    {
                        //communicator.OnPortNameChanged();

                        SerialPortCommunicator.BLStatus = BootLaoderStatus.应用程序;
                    }
                    if (LastView == "SystemSupervisionView" && currentView != "SystemSetView" && currentView != LastView)
                    {
                        foreach (Pack pack in this.PackCollection)//20200719
                        {
                            pack.IsCommunicationEnabled = false;
                        }
                    }

                    LastView = view;//20200508

                    if (currentView == "BootLoaderView")//20200508
                    {
                        communicator.intBaudRate = 115200;
                        communicator.OnPortNameChanged();

                        foreach (Pack pack in this.PackCollection)//20200606
                        {
                            pack.IsCommunicationEnabled = false;
                        }
                    }

                    NavigationService.Navigate(view, null, this);
                }
            }
        }
        //下载数据
        public void LoadData()
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();

            openFileDialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory + "HistoryData";

            openFileDialog.Filter = "DataFile|*.dat";

            openFileDialog.RestoreDirectory = true;

            openFileDialog.FilterIndex = 1;

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Records = new ObservableCollection<DataRecord>();

                using (FileStream fs = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    byte[] recordFile = new byte[fs.Length];

                    fs.Read(recordFile, 0, (int)fs.Length);

                    int offset = 0;

                    while (offset < recordFile.Length)
                    {
                        DataRecord record = ViewModelSource.Create<DataRecord>();

                        record.PackID = BitConverter.ToInt16(new byte[] { recordFile[offset], recordFile[offset + 1] }, 0);
                        offset += 2;

                        int year = BitConverter.ToInt16(new byte[] { recordFile[offset], recordFile[offset + 1] }, 0);
                        offset += 2;

                        int month = BitConverter.ToInt16(new byte[] { recordFile[offset], recordFile[offset + 1] }, 0);
                        offset += 2;

                        int day = BitConverter.ToInt16(new byte[] { recordFile[offset], recordFile[offset + 1] }, 0);
                        offset += 2;

                        int hour = BitConverter.ToInt16(new byte[] { recordFile[offset], recordFile[offset + 1] }, 0);
                        offset += 2;

                        int minute = BitConverter.ToInt16(new byte[] { recordFile[offset], recordFile[offset + 1] }, 0);
                        offset += 2;

                        int second = BitConverter.ToInt16(new byte[] { recordFile[offset], recordFile[offset + 1] }, 0);
                        offset += 2;
                        try
                        {
                            record.SaveTime = new DateTime(year, month, day, hour, minute, second);
                        }
                        catch { }

                        record.BusBarMeasureVoltage_Coslight = BitConverter.ToInt16(new byte[] { recordFile[offset], recordFile[offset + 1] }, 0) * 0.01F;
                        offset += 2;
                        record.BatteryGroupVoltage = BitConverter.ToInt16(new byte[] { recordFile[offset], recordFile[offset + 1] }, 0) * 0.01F;
                        offset += 2;
                        record.BatteryGroupCurrent = BitConverter.ToInt16(new byte[] { recordFile[offset], recordFile[offset + 1] }, 0) * 0.01F;
                        offset += 2;
                        record.SOC = BitConverter.ToInt16(new byte[] { recordFile[offset], recordFile[offset + 1] }, 0);
                        offset += 2;
                        record.MOSFET_1_Coslight = BitConverter.ToInt16(new byte[] { recordFile[offset], recordFile[offset + 1] }, 0) * 0.01F;
                        offset += 2;
                        record.MOSFET_2_Coslight = BitConverter.ToInt16(new byte[] { recordFile[offset], recordFile[offset + 1] }, 0) * 0.01F;
                        offset += 2;
                        record.AmbinentTemperature_1_Coslight = BitConverter.ToInt16(new byte[] { recordFile[offset], recordFile[offset + 1] }, 0) * 0.01F;
                        offset += 2;
                        record.AmbinentTemperature_2_Coslight = BitConverter.ToInt16(new byte[] { recordFile[offset], recordFile[offset + 1] }, 0) * 0.01F;
                        offset += 2;

                        record.ChargeCircuitSwitchStatus = (SwitchStatus)BitConverter.ToInt16(new byte[] { recordFile[offset], recordFile[offset + 1] }, 0);
                        offset += 2;
                        record.DischargeCircuitSwitchStatus = (SwitchStatus)BitConverter.ToInt16(new byte[] { recordFile[offset], recordFile[offset + 1] }, 0);
                        offset += 2;
                        record.BeeperSwitchStatus = (SwitchStatus)BitConverter.ToInt16(new byte[] { recordFile[offset], recordFile[offset + 1] }, 0);
                        offset += 2;
                        record.HeatingFilmSwitchStatus = (SwitchStatus)BitConverter.ToInt16(new byte[] { recordFile[offset], recordFile[offset + 1] }, 0);
                        offset += 2;
                        record.FullyChargedStatus = (BatteryChargingStatus)BitConverter.ToInt16(new byte[] { recordFile[offset], recordFile[offset + 1] }, 0);
                        offset += 2;
                        record.BatteryStatus = (BatteryStatus)BitConverter.ToInt16(new byte[] { recordFile[offset], recordFile[offset + 1] }, 0);
                        offset += 2;
                        record.BatteryChargingRequestFlag = (BatteryChargingRequestStatus)BitConverter.ToInt16(new byte[] { recordFile[offset], recordFile[offset + 1] }, 0);
                        offset += 2;

                        record.Cell_01_Voltage = BitConverter.ToInt16(new byte[] { recordFile[offset], recordFile[offset + 1] }, 0) * 0.001F;
                        offset += 2;
                        record.Cell_02_Voltage = BitConverter.ToInt16(new byte[] { recordFile[offset], recordFile[offset + 1] }, 0) * 0.001F;
                        offset += 2;
                        record.Cell_03_Voltage = BitConverter.ToInt16(new byte[] { recordFile[offset], recordFile[offset + 1] }, 0) * 0.001F;
                        offset += 2;
                        record.Cell_04_Voltage = BitConverter.ToInt16(new byte[] { recordFile[offset], recordFile[offset + 1] }, 0) * 0.001F;
                        offset += 2;
                        record.Cell_05_Voltage = BitConverter.ToInt16(new byte[] { recordFile[offset], recordFile[offset + 1] }, 0) * 0.001F;
                        offset += 2;
                        record.Cell_06_Voltage = BitConverter.ToInt16(new byte[] { recordFile[offset], recordFile[offset + 1] }, 0) * 0.001F;
                        offset += 2;
                        record.Cell_07_Voltage = BitConverter.ToInt16(new byte[] { recordFile[offset], recordFile[offset + 1] }, 0) * 0.001F;
                        offset += 2;
                        record.Cell_08_Voltage = BitConverter.ToInt16(new byte[] { recordFile[offset], recordFile[offset + 1] }, 0) * 0.001F;
                        offset += 2;
                        record.Cell_09_Voltage = BitConverter.ToInt16(new byte[] { recordFile[offset], recordFile[offset + 1] }, 0) * 0.001F;
                        offset += 2;
                        record.Cell_10_Voltage = BitConverter.ToInt16(new byte[] { recordFile[offset], recordFile[offset + 1] }, 0) * 0.001F;
                        offset += 2;
                        record.Cell_11_Voltage = BitConverter.ToInt16(new byte[] { recordFile[offset], recordFile[offset + 1] }, 0) * 0.001F;
                        offset += 2;
                        record.Cell_12_Voltage = BitConverter.ToInt16(new byte[] { recordFile[offset], recordFile[offset + 1] }, 0) * 0.001F;
                        offset += 2;
                        record.Cell_13_Voltage = BitConverter.ToInt16(new byte[] { recordFile[offset], recordFile[offset + 1] }, 0) * 0.001F;
                        offset += 2;
                        record.Cell_14_Voltage = BitConverter.ToInt16(new byte[] { recordFile[offset], recordFile[offset + 1] }, 0) * 0.001F;
                        offset += 2;
                        record.Cell_15_Voltage = BitConverter.ToInt16(new byte[] { recordFile[offset], recordFile[offset + 1] }, 0) * 0.001F;
                        offset += 2;
                        record.Cell_16_Voltage = BitConverter.ToInt16(new byte[] { recordFile[offset], recordFile[offset + 1] }, 0) * 0.001F;
                        offset += 2;

                        record.Temp_01 = BitConverter.ToInt16(new byte[] { recordFile[offset], recordFile[offset + 1] }, 0) * 0.01F;
                        offset += 2;
                        record.Temp_02 = BitConverter.ToInt16(new byte[] { recordFile[offset], recordFile[offset + 1] }, 0) * 0.01F;
                        offset += 2;
                        record.Temp_03 = BitConverter.ToInt16(new byte[] { recordFile[offset], recordFile[offset + 1] }, 0) * 0.01F;
                        offset += 2;
                        record.Temp_04 = BitConverter.ToInt16(new byte[] { recordFile[offset], recordFile[offset + 1] }, 0) * 0.01F;
                        offset += 2;

                        Records.Add(record);
                    }

                    foreach (DataRecord record in Records)
                    {
                        if (IsChineseUI)
                        {
                            record.ChargeCircuitSwitchStatus = (int)record.ChargeCircuitSwitchStatus <= 2 ? record.ChargeCircuitSwitchStatus : record.ChargeCircuitSwitchStatus - 3;
                            record.DischargeCircuitSwitchStatus = (int)record.DischargeCircuitSwitchStatus <= 2 ? record.DischargeCircuitSwitchStatus : record.DischargeCircuitSwitchStatus - 3;
                            record.BeeperSwitchStatus = (int)record.BeeperSwitchStatus <= 2 ? record.BeeperSwitchStatus : record.BeeperSwitchStatus - 3;
                            record.HeatingFilmSwitchStatus = (int)record.HeatingFilmSwitchStatus <= 2 ? record.HeatingFilmSwitchStatus : record.HeatingFilmSwitchStatus - 3;
                            record.FullyChargedStatus = (int)record.FullyChargedStatus <= 2 ? record.FullyChargedStatus : record.FullyChargedStatus - 3;
                            record.BatteryStatus = (int)record.BatteryStatus <= 5 ? record.BatteryStatus : record.BatteryStatus - 6;
                            record.BatteryChargingRequestFlag = (int)record.BatteryChargingRequestFlag <= 2 ? record.BatteryChargingRequestFlag : record.BatteryChargingRequestFlag - 3;
                        }
                        else
                        {
                            record.ChargeCircuitSwitchStatus = (int)record.ChargeCircuitSwitchStatus >= 3 ? record.ChargeCircuitSwitchStatus : record.ChargeCircuitSwitchStatus + 3;
                            record.DischargeCircuitSwitchStatus = (int)record.DischargeCircuitSwitchStatus >= 3 ? record.DischargeCircuitSwitchStatus : record.DischargeCircuitSwitchStatus + 3;
                            record.BeeperSwitchStatus = (int)record.BeeperSwitchStatus >= 3 ? record.BeeperSwitchStatus : record.BeeperSwitchStatus + 3;
                            record.HeatingFilmSwitchStatus = (int)record.HeatingFilmSwitchStatus >= 3 ? record.HeatingFilmSwitchStatus : record.HeatingFilmSwitchStatus + 3;
                            record.FullyChargedStatus = (int)record.FullyChargedStatus >= 3 ? record.FullyChargedStatus : record.FullyChargedStatus + 3;
                            record.BatteryStatus = (int)record.BatteryStatus >= 6 ? record.BatteryStatus : record.BatteryStatus + 6;
                            record.BatteryChargingRequestFlag = (int)record.BatteryChargingRequestFlag >= 3 ? record.BatteryChargingRequestFlag : record.BatteryChargingRequestFlag + 3;
                        }
                    }
                }
            }
        }
        public void LoadData_DPC()//20200601
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();

            openFileDialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory + "HistoryData_DPC";

            openFileDialog.Filter = "DataFile|*.dat";

            openFileDialog.RestoreDirectory = true;

            openFileDialog.FilterIndex = 1;

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Records = new ObservableCollection<DataRecord>();

                using (FileStream fs = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    byte[] recordFile = new byte[fs.Length];

                    fs.Read(recordFile, 0, (int)fs.Length);

                    int offset = 0;

                    while (offset < recordFile.Length)
                    {
                        DataRecord record = ViewModelSource.Create<DataRecord>();

                        record.PackID = BitConverter.ToInt16(new byte[] { recordFile[offset], recordFile[offset + 1] }, 0);
                        offset += 2;

                        int year = BitConverter.ToInt16(new byte[] { recordFile[offset], recordFile[offset + 1] }, 0);
                        offset += 2;

                        int month = BitConverter.ToInt16(new byte[] { recordFile[offset], recordFile[offset + 1] }, 0);
                        offset += 2;

                        int day = BitConverter.ToInt16(new byte[] { recordFile[offset], recordFile[offset + 1] }, 0);
                        offset += 2;

                        int hour = BitConverter.ToInt16(new byte[] { recordFile[offset], recordFile[offset + 1] }, 0);
                        offset += 2;

                        int minute = BitConverter.ToInt16(new byte[] { recordFile[offset], recordFile[offset + 1] }, 0);
                        offset += 2;

                        int second = BitConverter.ToInt16(new byte[] { recordFile[offset], recordFile[offset + 1] }, 0);
                        offset += 2;
                        try
                        {
                            record.SaveTime = new DateTime(year, month, day, hour, minute, second);
                        }
                        catch { }
                        record.BatteryGroupVoltage = BitConverter.ToInt16(new byte[] { recordFile[offset], recordFile[offset + 1] }, 0) * 0.01F;
                        offset += 2;
                        record.BatteryGroupCurrent = BitConverter.ToInt16(new byte[] { recordFile[offset], recordFile[offset + 1] }, 0) * 0.01F;
                        offset += 2;
                        record.SOC = BitConverter.ToInt16(new byte[] { recordFile[offset], recordFile[offset + 1] }, 0);
                        offset += 2;
                        record.AmbinentTemperature_1_Coslight = BitConverter.ToInt16(new byte[] { recordFile[offset], recordFile[offset + 1] }, 0) * 0.01F;
                        offset += 2;
                        record.PowerTemperature = BitConverter.ToInt16(new byte[] { recordFile[offset], recordFile[offset + 1] }, 0) * 0.01F;
                        offset += 2;
                        record.SOH = BitConverter.ToInt16(new byte[] { recordFile[offset], recordFile[offset + 1] }, 0) * 0.01F;
                        offset += 2;
                        record.DischargeEnergy = BitConverter.ToInt16(new byte[] { recordFile[offset], recordFile[offset + 1] }, 0) * 0.01F;
                        offset += 2;

                        record.ChargeCircuitSwitchStatus = (SwitchStatus)BitConverter.ToInt16(new byte[] { recordFile[offset], recordFile[offset + 1] }, 0);
                        offset += 2;
                        record.DischargeCircuitSwitchStatus = (SwitchStatus)BitConverter.ToInt16(new byte[] { recordFile[offset], recordFile[offset + 1] }, 0);
                        offset += 2;
                        record.BeeperSwitchStatus = (SwitchStatus)BitConverter.ToInt16(new byte[] { recordFile[offset], recordFile[offset + 1] }, 0);
                        offset += 2;
                        record.HeatingFilmSwitchStatus = (SwitchStatus)BitConverter.ToInt16(new byte[] { recordFile[offset], recordFile[offset + 1] }, 0);
                        offset += 2;

                        record.BatteryStatus = (BatteryStatus)BitConverter.ToInt16(new byte[] { recordFile[offset], recordFile[offset + 1] }, 0);
                        offset += 2;

                        record.Cell_01_Voltage = BitConverter.ToInt16(new byte[] { recordFile[offset], recordFile[offset + 1] }, 0) * 0.001F;
                        offset += 2;
                        record.Cell_02_Voltage = BitConverter.ToInt16(new byte[] { recordFile[offset], recordFile[offset + 1] }, 0) * 0.001F;
                        offset += 2;
                        record.Cell_03_Voltage = BitConverter.ToInt16(new byte[] { recordFile[offset], recordFile[offset + 1] }, 0) * 0.001F;
                        offset += 2;
                        record.Cell_04_Voltage = BitConverter.ToInt16(new byte[] { recordFile[offset], recordFile[offset + 1] }, 0) * 0.001F;
                        offset += 2;
                        record.Cell_05_Voltage = BitConverter.ToInt16(new byte[] { recordFile[offset], recordFile[offset + 1] }, 0) * 0.001F;
                        offset += 2;
                        record.Cell_06_Voltage = BitConverter.ToInt16(new byte[] { recordFile[offset], recordFile[offset + 1] }, 0) * 0.001F;
                        offset += 2;
                        record.Cell_07_Voltage = BitConverter.ToInt16(new byte[] { recordFile[offset], recordFile[offset + 1] }, 0) * 0.001F;
                        offset += 2;
                        record.Cell_08_Voltage = BitConverter.ToInt16(new byte[] { recordFile[offset], recordFile[offset + 1] }, 0) * 0.001F;
                        offset += 2;
                        record.Cell_09_Voltage = BitConverter.ToInt16(new byte[] { recordFile[offset], recordFile[offset + 1] }, 0) * 0.001F;
                        offset += 2;
                        record.Cell_10_Voltage = BitConverter.ToInt16(new byte[] { recordFile[offset], recordFile[offset + 1] }, 0) * 0.001F;
                        offset += 2;
                        record.Cell_11_Voltage = BitConverter.ToInt16(new byte[] { recordFile[offset], recordFile[offset + 1] }, 0) * 0.001F;
                        offset += 2;
                        record.Cell_12_Voltage = BitConverter.ToInt16(new byte[] { recordFile[offset], recordFile[offset + 1] }, 0) * 0.001F;
                        offset += 2;
                        record.Cell_13_Voltage = BitConverter.ToInt16(new byte[] { recordFile[offset], recordFile[offset + 1] }, 0) * 0.001F;
                        offset += 2;
                        record.Cell_14_Voltage = BitConverter.ToInt16(new byte[] { recordFile[offset], recordFile[offset + 1] }, 0) * 0.001F;
                        offset += 2;
                        record.Cell_15_Voltage = BitConverter.ToInt16(new byte[] { recordFile[offset], recordFile[offset + 1] }, 0) * 0.001F;
                        offset += 2;
                        record.Cell_16_Voltage = BitConverter.ToInt16(new byte[] { recordFile[offset], recordFile[offset + 1] }, 0) * 0.001F;
                        offset += 2;

                        record.Temp_01 = BitConverter.ToInt16(new byte[] { recordFile[offset], recordFile[offset + 1] }, 0) * 0.01F;
                        offset += 2;
                        record.Temp_02 = BitConverter.ToInt16(new byte[] { recordFile[offset], recordFile[offset + 1] }, 0) * 0.01F;
                        offset += 2;
                        record.Temp_03 = BitConverter.ToInt16(new byte[] { recordFile[offset], recordFile[offset + 1] }, 0) * 0.01F;
                        offset += 2;
                        record.Temp_04 = BitConverter.ToInt16(new byte[] { recordFile[offset], recordFile[offset + 1] }, 0) * 0.01F;
                        offset += 2;

                        Records.Add(record);
                    }

                    foreach (DataRecord record in Records)
                    {
                        if (IsChineseUI)
                        {
                            record.ChargeCircuitSwitchStatus = (int)record.ChargeCircuitSwitchStatus <= 2 ? record.ChargeCircuitSwitchStatus : record.ChargeCircuitSwitchStatus - 3;
                            record.DischargeCircuitSwitchStatus = (int)record.DischargeCircuitSwitchStatus <= 2 ? record.DischargeCircuitSwitchStatus : record.DischargeCircuitSwitchStatus - 3;
                            record.BeeperSwitchStatus = (int)record.BeeperSwitchStatus <= 2 ? record.BeeperSwitchStatus : record.BeeperSwitchStatus - 3;
                            record.HeatingFilmSwitchStatus = (int)record.HeatingFilmSwitchStatus <= 2 ? record.HeatingFilmSwitchStatus : record.HeatingFilmSwitchStatus - 3;
                            record.FullyChargedStatus = (int)record.FullyChargedStatus <= 2 ? record.FullyChargedStatus : record.FullyChargedStatus - 3;
                            record.BatteryStatus = (int)record.BatteryStatus <= 5 ? record.BatteryStatus : record.BatteryStatus - 6;
                            record.BatteryChargingRequestFlag = (int)record.BatteryChargingRequestFlag <= 2 ? record.BatteryChargingRequestFlag : record.BatteryChargingRequestFlag - 3;
                        }
                        else
                        {
                            record.ChargeCircuitSwitchStatus = (int)record.ChargeCircuitSwitchStatus >= 3 ? record.ChargeCircuitSwitchStatus : record.ChargeCircuitSwitchStatus + 3;
                            record.DischargeCircuitSwitchStatus = (int)record.DischargeCircuitSwitchStatus >= 3 ? record.DischargeCircuitSwitchStatus : record.DischargeCircuitSwitchStatus + 3;
                            record.BeeperSwitchStatus = (int)record.BeeperSwitchStatus >= 3 ? record.BeeperSwitchStatus : record.BeeperSwitchStatus + 3;
                            record.HeatingFilmSwitchStatus = (int)record.HeatingFilmSwitchStatus >= 3 ? record.HeatingFilmSwitchStatus : record.HeatingFilmSwitchStatus + 3;
                            record.FullyChargedStatus = (int)record.FullyChargedStatus >= 3 ? record.FullyChargedStatus : record.FullyChargedStatus + 3;
                            record.BatteryStatus = (int)record.BatteryStatus >= 6 ? record.BatteryStatus : record.BatteryStatus + 6;
                            record.BatteryChargingRequestFlag = (int)record.BatteryChargingRequestFlag >= 3 ? record.BatteryChargingRequestFlag : record.BatteryChargingRequestFlag + 3;
                        }
                    }
                }
            }
        }
        //导入数据
        public void ExportData(object obj)
        {
            ((HistoryDataView)(NavigationService as FrameNavigationService).Current).ExportHistoryData_Button_Click(null, null);
        }
        //保存数据
        private void SaveData_DPC(Pack pack)
        {
            //保存数据的文件路径不为空
            if (string.IsNullOrEmpty(this.filePath))
            {
                return;
            }

            if (SerialPortCommunicator.IsCommunicationFirst)
            {
                if (pack.BatteryGroupCollection[0].CellCollection[0].Voltage == 0
                    && pack.BatteryGroupCollection[0].CellCollection[3].Voltage == 0
                    && pack.BatteryGroupCollection[0].CellCollection[7].Voltage == 0)
                {
                    return;
                }
                else
                {
                    SerialPortCommunicator.IsCommunicationFirst = false;
                }
            }

            using (FileStream fs = new FileStream(this.filePath, FileMode.Append, FileAccess.Write))
            {
                BinaryWriter bw = new BinaryWriter(fs);

                bw.Write((Int16)pack.PackID);

                bw.Write((Int16)DateTime.Now.Year);
                bw.Write((Int16)DateTime.Now.Month);
                bw.Write((Int16)DateTime.Now.Day);
                bw.Write((Int16)DateTime.Now.Hour);
                bw.Write((Int16)DateTime.Now.Minute);
                bw.Write((Int16)DateTime.Now.Second);

                bw.Write((Int16)(pack.BatteryGroupCollection[0].BatteryGroupVoltage * 100));
                bw.Write((Int16)(pack.BatteryGroupCollection[0].BatteryGroupCurrent * 100));
                bw.Write((Int16)pack.BatteryGroupCollection[0].SOC);
                bw.Write((Int16)(pack.AmbinentTemperature_1_Coslight * 100));
                bw.Write((Int16)(pack.PowerTemperature * 100));
                bw.Write((Int16)(pack.SOH * 100));
                bw.Write((Int16)(pack.DischargeEnergy * 100));

                bw.Write((Int16)pack.BatteryGroupCollection[0].ChargeCircuitSwitchStatus);
                bw.Write((Int16)pack.BatteryGroupCollection[0].DischargeCircuitSwitchStatus);
                bw.Write((Int16)pack.BatteryGroupCollection[0].BeeperSwitchStatus);
                bw.Write((Int16)pack.BatteryGroupCollection[0].HeatingFilmSwitchStatus);
                bw.Write((Int16)pack.BatteryGroupCollection[0].BatteryStatus);

                bw.Write((Int16)(pack.BatteryGroupCollection[0].CellCollection[0].Voltage * 1000));
                bw.Write((Int16)(pack.BatteryGroupCollection[0].CellCollection[1].Voltage * 1000));
                bw.Write((Int16)(pack.BatteryGroupCollection[0].CellCollection[2].Voltage * 1000));
                bw.Write((Int16)(pack.BatteryGroupCollection[0].CellCollection[3].Voltage * 1000));
                bw.Write((Int16)(pack.BatteryGroupCollection[0].CellCollection[4].Voltage * 1000));
                bw.Write((Int16)(pack.BatteryGroupCollection[0].CellCollection[5].Voltage * 1000));
                bw.Write((Int16)(pack.BatteryGroupCollection[0].CellCollection[6].Voltage * 1000));
                bw.Write((Int16)(pack.BatteryGroupCollection[0].CellCollection[7].Voltage * 1000));
                bw.Write((Int16)(pack.BatteryGroupCollection[0].CellCollection[8].Voltage * 1000));
                bw.Write((Int16)(pack.BatteryGroupCollection[0].CellCollection[9].Voltage * 1000));
                bw.Write((Int16)(pack.BatteryGroupCollection[0].CellCollection[10].Voltage * 1000));
                bw.Write((Int16)(pack.BatteryGroupCollection[0].CellCollection[11].Voltage * 1000));
                bw.Write((Int16)(pack.BatteryGroupCollection[0].CellCollection[12].Voltage * 1000));
                bw.Write((Int16)(pack.BatteryGroupCollection[0].CellCollection[13].Voltage * 1000));
                bw.Write((Int16)(pack.BatteryGroupCollection[0].CellCollection[14].Voltage * 1000));
                bw.Write((Int16)(pack.BatteryGroupCollection[0].CellCollection[15].Voltage * 1000));    // added by sunlw 2020-11-05 08:11

                bw.Write((Int16)(pack.BatteryGroupCollection[0].CellCollection[0].Temperature * 100));
                bw.Write((Int16)(pack.BatteryGroupCollection[0].CellCollection[1].Temperature * 100));
                bw.Write((Int16)(pack.BatteryGroupCollection[0].CellCollection[2].Temperature * 100));
                bw.Write((Int16)(pack.BatteryGroupCollection[0].CellCollection[3].Temperature * 100));

                bw.Flush();
                bw.Close();
                fs.Close();
            }
        }
        private void SaveData(Pack pack)
        {
            if (string.IsNullOrEmpty(this.filePath))
            {
                return;
            }

            if (SerialPortCommunicator.IsCommunicationFirst)
            {
                if (pack.BatteryGroupCollection[0].CellCollection[0].Voltage == 0
                    && pack.BatteryGroupCollection[0].CellCollection[3].Voltage == 0
                    && pack.BatteryGroupCollection[0].CellCollection[7].Voltage == 0)
                {
                    return;
                }
                else
                {
                    SerialPortCommunicator.IsCommunicationFirst = false;
                }
            }

            using (FileStream fs = new FileStream(this.filePath, FileMode.Append, FileAccess.Write))
            {
                BinaryWriter bw = new BinaryWriter(fs);

                bw.Write((Int16)pack.PackID);

                bw.Write((Int16)DateTime.Now.Year);
                bw.Write((Int16)DateTime.Now.Month);
                bw.Write((Int16)DateTime.Now.Day);
                bw.Write((Int16)DateTime.Now.Hour);
                bw.Write((Int16)DateTime.Now.Minute);
                bw.Write((Int16)DateTime.Now.Second);

                bw.Write((Int16)(pack.BusBarVoltage_Coslight * 100));
                bw.Write((Int16)(pack.BatteryGroupCollection[0].BatteryGroupVoltage * 100));
                bw.Write((Int16)(pack.BatteryGroupCollection[0].BatteryGroupCurrent * 100));
                bw.Write((Int16)pack.BatteryGroupCollection[0].SOC);
                bw.Write((Int16)(pack.MOSFET_1_Coslight * 100));
                bw.Write((Int16)(pack.MOSFET_2_Coslight * 100));
                bw.Write((Int16)(pack.AmbinentTemperature_1_Coslight * 100));
                bw.Write((Int16)(pack.AmbinentTemperature_2_Coslight * 100));

                bw.Write((Int16)pack.BatteryGroupCollection[0].ChargeCircuitSwitchStatus);
                bw.Write((Int16)pack.BatteryGroupCollection[0].DischargeCircuitSwitchStatus);
                bw.Write((Int16)pack.BatteryGroupCollection[0].BeeperSwitchStatus);
                bw.Write((Int16)pack.BatteryGroupCollection[0].HeatingFilmSwitchStatus);
                bw.Write((Int16)pack.BatteryGroupCollection[0].FullyChargedStatus);
                bw.Write((Int16)pack.BatteryGroupCollection[0].BatteryStatus);
                bw.Write((Int16)pack.BatteryGroupCollection[0].BatteryChargingRequestFlag);

                bw.Write((Int16)(pack.BatteryGroupCollection[0].CellCollection[0].Voltage * 1000));
                bw.Write((Int16)(pack.BatteryGroupCollection[0].CellCollection[1].Voltage * 1000));
                bw.Write((Int16)(pack.BatteryGroupCollection[0].CellCollection[2].Voltage * 1000));
                bw.Write((Int16)(pack.BatteryGroupCollection[0].CellCollection[3].Voltage * 1000));
                bw.Write((Int16)(pack.BatteryGroupCollection[0].CellCollection[4].Voltage * 1000));
                bw.Write((Int16)(pack.BatteryGroupCollection[0].CellCollection[5].Voltage * 1000));
                bw.Write((Int16)(pack.BatteryGroupCollection[0].CellCollection[6].Voltage * 1000));
                bw.Write((Int16)(pack.BatteryGroupCollection[0].CellCollection[7].Voltage * 1000));
                bw.Write((Int16)(pack.BatteryGroupCollection[0].CellCollection[8].Voltage * 1000));
                bw.Write((Int16)(pack.BatteryGroupCollection[0].CellCollection[9].Voltage * 1000));
                bw.Write((Int16)(pack.BatteryGroupCollection[0].CellCollection[10].Voltage * 1000));
                bw.Write((Int16)(pack.BatteryGroupCollection[0].CellCollection[11].Voltage * 1000));
                bw.Write((Int16)(pack.BatteryGroupCollection[0].CellCollection[12].Voltage * 1000));
                bw.Write((Int16)(pack.BatteryGroupCollection[0].CellCollection[13].Voltage * 1000));
                bw.Write((Int16)(pack.BatteryGroupCollection[0].CellCollection[14].Voltage * 1000));
                bw.Write((Int16)(pack.BatteryGroupCollection[0].CellCollection[15].Voltage * 1000));

                bw.Write((Int16)(pack.BatteryGroupCollection[0].CellCollection[0].Temperature * 100));
                bw.Write((Int16)(pack.BatteryGroupCollection[0].CellCollection[1].Temperature * 100));
                bw.Write((Int16)(pack.BatteryGroupCollection[0].CellCollection[2].Temperature * 100));
                bw.Write((Int16)(pack.BatteryGroupCollection[0].CellCollection[3].Temperature * 100));

                bw.Flush();
                bw.Close();
                fs.Close();
            }
        }
        //创建开关参数ID
        private byte[] creatSwitchParameterID_DPC(byte warningValue)
        {
            //8个字节
            byte[] warningIDTemp = new byte[8];

            //初始化为0
            for (int i = 0; i < 8; i++)
            {
                warningIDTemp[i] = 0;
            }

            //不是0，位解析
            if (warningValue != 0)
            {
                //根据每一位来决定warningIDTemp的byte值
                if ((warningValue & 0x01) == 0x01)
                {
                    warningIDTemp[0] = 1;
                }

                if ((warningValue & 0x02) == 0x02)
                {
                    warningIDTemp[1] = 1;
                }

                if ((warningValue & 0x04) == 0x04)
                {
                    warningIDTemp[2] = 1;
                }

                if ((warningValue & 0x08) == 0x08)
                {
                    warningIDTemp[3] = 1;
                }

                if ((warningValue & 0x10) == 0x10)
                {
                    warningIDTemp[4] = 1;
                }

                if ((warningValue & 0x20) == 0x20)
                {
                    warningIDTemp[5] = 1;
                }

                if ((warningValue & 0x40) == 0x40)
                {
                    warningIDTemp[6] = 1;
                }

                if ((warningValue & 0x80) == 0x80)
                {
                    warningIDTemp[7] = 1;
                }

            }

            return warningIDTemp;
        }
        //处理电池组警告
        private void ProcessBatteryGroupWarning_DPC(BatteryGroup batteryGroup, byte warningValue, string warningInfo)
        {
            if (warningValue != 0)
            {
                batteryGroup.WarningCollection.Add(new WarningItem { WarningItemID = batteryGroup.WarningCollection.Count + 1, WarningInfo = warningInfo });
            }
        }
        //处理电池组事件
        private void ProcessBatteryGroupEvent_DPC(BatteryGroup batteryGroup, string warningInfo)
        {
            batteryGroup.WarningCollection.Add(new WarningItem { WarningItemID = batteryGroup.WarningCollection.Count + 1, WarningInfo = warningInfo });
        }
        //返回
        private float[] ReturnSavedAndSetValue(int Byte, byte[] message, int offset)
        {
            float[] Value_F = new float[2];
            if (Byte == 1)
            {
                Value_F[0] = message[offset];
                Value_F[1] = message[offset];
            }
            else if (Byte == 2)
            {
                Value_F[0] = BitConverter.ToInt16(new byte[] { message[offset + 1], message[offset] }, 0);
                Value_F[1] = BitConverter.ToInt16(new byte[] { message[offset + 1], message[offset] }, 0);
            }
            else
            {
                Value_F[0] = BitConverter.ToInt16(new byte[] { message[offset + 1], message[offset] }, 0);
                Value_F[1] = BitConverter.ToInt16(new byte[] { message[offset + 1], message[offset] }, 0);
            }

            return Value_F;
        }
        private void ProcessSingleCellWarning_DPC(BatteryGroup batteryGroup)
        {
            foreach (Cell cell in batteryGroup.CellCollection)
            {
                if (cell.VoltageWarningStatus != VoltageWarningType.正常 && cell.VoltageWarningStatus != VoltageWarningType.Ok)
                {
                    bool flag = false;

                    foreach (WarningItem item in batteryGroup.WarningCollection)//20200423
                    {
                        if (item.WarningInfo == cell.VoltageWarningStatus.ToString())
                        {
                            flag = true;
                            break;
                        }
                    }

                    if (!flag)
                    {
                        batteryGroup.WarningCollection.Add(new WarningItem { WarningItemID = batteryGroup.WarningCollection.Count + 1, WarningInfo = cell.VoltageWarningStatus.ToString() });
                    }
                }

                if (cell.TemperatureWarningStatus != TemperatureWarningType.无告警 && cell.TemperatureWarningStatus != TemperatureWarningType.Ok)
                {
                    bool flag = false;

                    foreach (WarningItem item in batteryGroup.WarningCollection)
                    {
                        if (item.WarningInfo == cell.TemperatureWarningStatus.ToString())
                        {
                            flag = true;
                            break;
                        }
                    }

                    if (!flag)
                    {
                        batteryGroup.WarningCollection.Add(new WarningItem { WarningItemID = batteryGroup.WarningCollection.Count + 1, WarningInfo = cell.TemperatureWarningStatus.ToString() });
                    }
                }
            }

        }
        //创建开关参数
        private void creatSwitchParameter(byte[] message, int offset, DataParameter dataParameter)
        {
            switch (dataParameter.Name_Chinese)
            {
                case "电压功能开关参数":
                    for (int i = 0; i < 8; i++)
                    {
                        if (IsChineseUI)
                        {
                            EnableControlCollection.Add(ViewModelSource.Create(() => new SpecialParameter()
                            {
                                SpecialParameterID = EnableControlCollection.Count + 1,
                                CommandType = dataParameter.ID + 1,
                                Description = Enum.GetName(typeof(VoltageEvent_DPC), i) + "功能",
                                Description_US = Enum.GetName(typeof(VoltageEvent_DPC), i + 0x10),
                                CurrentValue = creatSwitchParameterID_DPC(message[offset])[i] == 1 ? "开启" : "关闭"
                            }));
                        }
                        if (IsEnglishUI)
                        {
                            EnableControlCollection.Add(ViewModelSource.Create(() => new SpecialParameter()
                            {
                                SpecialParameterID = EnableControlCollection.Count + 1,
                                CommandType = dataParameter.ID + 1,
                                Description = Enum.GetName(typeof(VoltageEvent_DPC), i) + "功能",
                                Description_US = Enum.GetName(typeof(VoltageEvent_DPC), i + 0x10),
                                CurrentValue = creatSwitchParameterID_DPC(message[offset])[i] == 1 ? "On" : "Off"
                            }));
                        }
                    }
                    break;
                case "均衡功能开关参数":
                    for (int i = 0; i < 7; i++)
                    {
                        if (i == 0)
                        {
                            if (IsChineseUI)
                            {
                                EnableControlCollection.Add(ViewModelSource.Create(() => new SpecialParameter
                                {
                                    SpecialParameterID = EnableControlCollection.Count + 1,
                                    CommandType = dataParameter.ID + 1,                                  
                                    Description = Enum.GetName(typeof(BalanceSwtich), i) + "功能",
                                    Description_US = Enum.GetName(typeof(BalanceSwtich), i + 0x10),
                                    CurrentValue = creatSwitchParameterID_DPC(message[offset])[i] == 1 ? "开启" : "关闭"                                   
                                }));
                            }
                            if (IsEnglishUI)
                            {
                                EnableControlCollection.Add(ViewModelSource.Create(() => new SpecialParameter()
                                {
                                    SpecialParameterID = EnableControlCollection.Count + 1,
                                    CommandType = dataParameter.ID + 1,
                                    Description = Enum.GetName(typeof(BalanceSwtich), i) + "功能",
                                    Description_US = Enum.GetName(typeof(BalanceSwtich), i + 0x10),
                                    CurrentValue = creatSwitchParameterID_DPC(message[offset])[i] == 1 ? "On" : "Off"
                                }));
                            }
                        }
                        else
                        {
                            if (IsChineseUI)
                            {
                                EnableControlCollection.Add(ViewModelSource.Create(() => new SpecialParameter
                                {
                                    SpecialParameterID = EnableControlCollection.Count + 1,
                                    CommandType = dataParameter.ID + 1,
                                    Description = Enum.GetName(typeof(BalanceSwtich), i) + "功能",
                                    Description_US = Enum.GetName(typeof(BalanceSwtich), i + 0x10),
                                    CurrentValue = creatSwitchParameterID_DPC(message[offset])[i] == 1 ? "开启" : "关闭"
                                }));
                            }
                            if (IsEnglishUI)
                            {
                                EnableControlCollection.Add(ViewModelSource.Create(() => new SpecialParameter()
                                {
                                    SpecialParameterID = EnableControlCollection.Count + 1,
                                    CommandType = dataParameter.ID + 1,
                                    Description = Enum.GetName(typeof(BalanceSwtich), i) + "功能",
                                    Description_US = Enum.GetName(typeof(BalanceSwtich), i + 0x10),
                                    CurrentValue = creatSwitchParameterID_DPC(message[offset])[i] == 1 ? "On" : "Off"
                                }));
                            }
                        }
                    }

                    break;
                case "温度功能开关参数":
                    for (int i = 0; i < 8; i++)
                    {
                        if (IsChineseUI)
                        {
                            EnableControlCollection.Add(ViewModelSource.Create(() => new SpecialParameter
                            {
                                SpecialParameterID = EnableControlCollection.Count + 1,
                                CommandType = dataParameter.ID + 1,
                                Description = Enum.GetName(typeof(TemperatureEvent_DPC), i) + "功能",
                                Description_US = Enum.GetName(typeof(TemperatureEvent_DPC), i + 0x10),
                                CurrentValue = creatSwitchParameterID_DPC(message[offset + 1])[i] == 1 ? "开启" : "关闭"
                            }));
                        }
                        if (IsEnglishUI)
                        {
                            EnableControlCollection.Add(ViewModelSource.Create(() => new SpecialParameter
                            {
                                SpecialParameterID = EnableControlCollection.Count + 1,
                                CommandType = dataParameter.ID + 1,
                                Description = Enum.GetName(typeof(TemperatureEvent_DPC), i) + "功能",
                                Description_US = Enum.GetName(typeof(TemperatureEvent_DPC), i + 0x10),
                                CurrentValue = creatSwitchParameterID_DPC(message[offset + 1])[i] == 1 ? "On" : "Off"
                            }));
                        }
                    }
                    for (int i = 0; i < 6; i++)
                    {
                        if (IsChineseUI)
                        {
                            EnableControlCollection.Add(ViewModelSource.Create(() => new SpecialParameter
                            {
                                SpecialParameterID = EnableControlCollection.Count + 1,
                                CommandType = dataParameter.ID + 1,
                                Description = Enum.GetName(typeof(TemperatureEvent_DPC), i + 8) + "功能",
                                Description_US = Enum.GetName(typeof(TemperatureEvent_DPC), i + 8 + 0x10),
                                CurrentValue = creatSwitchParameterID_DPC(message[offset])[i] == 1 ? "开启" : "关闭"
                            }));
                        }
                        if (IsEnglishUI)
                        {
                            EnableControlCollection.Add(ViewModelSource.Create(() => new SpecialParameter
                            {
                                SpecialParameterID = EnableControlCollection.Count + 1,
                                CommandType = dataParameter.ID + 1,
                                Description = Enum.GetName(typeof(TemperatureEvent_DPC), i + 8) + "功能",
                                Description_US = Enum.GetName(typeof(TemperatureEvent_DPC), i + 8 + 0x10),
                                CurrentValue = creatSwitchParameterID_DPC(message[offset])[i] == 1 ? "On" : "Off"
                            }));
                        }
                    }
                    break;
                case "电流功能开关参数":
                    for (int i = 0; i < 8; i++)
                    {
                        if (i < 4)
                        {
                            if (IsChineseUI)
                            {
                                EnableControlCollection.Add(ViewModelSource.Create(() => new SpecialParameter
                                {
                                    SpecialParameterID = EnableControlCollection.Count + 1,
                                    CommandType = dataParameter.ID + 1,
                                    Description = Enum.GetName(typeof(CurrentEvent_DPC), i) + "功能",
                                    Description_US = Enum.GetName(typeof(CurrentEvent_DPC), i + 0x10),
                                    CurrentValue = creatSwitchParameterID_DPC(message[offset])[i] == 1 ? "开启" : "关闭"
                                }));
                            }
                            if (IsEnglishUI)
                            {
                                EnableControlCollection.Add(ViewModelSource.Create(() => new SpecialParameter
                                {
                                    SpecialParameterID = EnableControlCollection.Count + 1,
                                    CommandType = dataParameter.ID + 1,
                                    Description = Enum.GetName(typeof(CurrentEvent_DPC), i) + "功能",
                                    Description_US = Enum.GetName(typeof(CurrentEvent_DPC), i + 0x10),
                                    CurrentValue = creatSwitchParameterID_DPC(message[offset])[i] == 1 ? "On" : "Off"
                                }));
                            }
                        }
                        else
                        {
                            if (IsChineseUI)
                            {
                                EnableControlCollection.Add(ViewModelSource.Create(() => new SpecialParameter
                                {
                                    SpecialParameterID = EnableControlCollection.Count + 1,
                                    CommandType = dataParameter.ID + 1,
                                    Description = Enum.GetName(typeof(CurrentEvent_DPC), i) + "功能",
                                    Description_US = Enum.GetName(typeof(CurrentEvent_DPC), i + 0x10),
                                    CurrentValue = creatSwitchParameterID_DPC(message[offset])[i] == 1 ? "开启" : "关闭"
                                }));
                            }
                            if (IsEnglishUI)
                            {
                                EnableControlCollection.Add(ViewModelSource.Create(() => new SpecialParameter
                                {
                                    SpecialParameterID = EnableControlCollection.Count + 1,
                                    CommandType = dataParameter.ID + 1,
                                    Description = Enum.GetName(typeof(CurrentEvent_DPC), i) + "功能",
                                    Description_US = Enum.GetName(typeof(CurrentEvent_DPC), i + 0x10),
                                    CurrentValue = creatSwitchParameterID_DPC(message[offset])[i] == 1 ? "On" : "Off"
                                }));
                            }
                        }
                    }
                    break;
                case "容量及其它功能开关参数":
                    for (int i = 0; i < 6; i++)
                    {
                        if (i == 0)
                        {
                            if (IsChineseUI)
                            {
                                EnableControlCollection.Add(ViewModelSource.Create(() => new SpecialParameter
                                {
                                    SpecialParameterID = EnableControlCollection.Count + 1,
                                    CommandType = dataParameter.ID + 1,
                                    Description = Enum.GetName(typeof(CapacityAndOtherSwtich), i) + "功能",
                                    Description_US = Enum.GetName(typeof(CapacityAndOtherSwtich), i + 0x10),
                                    CurrentValue = creatSwitchParameterID_DPC(message[offset])[i] == 1 ? "开启" : "关闭"
                                }));
                            }
                            if (IsEnglishUI)
                            {
                                EnableControlCollection.Add(ViewModelSource.Create(() => new SpecialParameter
                                {
                                    SpecialParameterID = EnableControlCollection.Count + 1,
                                    CommandType = dataParameter.ID + 1,
                                    Description = Enum.GetName(typeof(CapacityAndOtherSwtich), i) + "功能",
                                    Description_US = Enum.GetName(typeof(CapacityAndOtherSwtich), i + 0x10),
                                    CurrentValue = creatSwitchParameterID_DPC(message[offset])[i] == 1 ? "On" : "Off"
                                }));
                            }
                        }
                        else if (i == 5)
                        {
                            if (IsChineseUI)
                            {
                                EnableControlCollection.Add(ViewModelSource.Create(() => new SpecialParameter
                                {
                                    SpecialParameterID = EnableControlCollection.Count + 1,
                                    CommandType = dataParameter.ID + 1,
                                    Description = Enum.GetName(typeof(CapacityAndOtherSwtich), i) + "功能",
                                    Description_US = Enum.GetName(typeof(CapacityAndOtherSwtich), i + 0x10),
                                    CurrentValue = creatSwitchParameterID_DPC(message[offset])[i] == 1 ? "主动" : "被动"
                                }));
                            }
                            if (IsEnglishUI)
                            {
                                EnableControlCollection.Add(ViewModelSource.Create(() => new SpecialParameter
                                {
                                    SpecialParameterID = EnableControlCollection.Count + 1,
                                    CommandType = dataParameter.ID + 1,
                                    Description = Enum.GetName(typeof(CapacityAndOtherSwtich), i) + "功能",
                                    Description_US = Enum.GetName(typeof(CapacityAndOtherSwtich), i + 0x10),
                                    CurrentValue = creatSwitchParameterID_DPC(message[offset])[i] == 1 ? "Positive" : "Passive"
                                }));
                            }
                        }
                        else
                        {
                            if (IsChineseUI)
                            {
                                EnableControlCollection.Add(ViewModelSource.Create(() => new SpecialParameter
                                {
                                    SpecialParameterID = EnableControlCollection.Count + 1,
                                    CommandType = dataParameter.ID + 1,
                                    Description = Enum.GetName(typeof(CapacityAndOtherSwtich), i) + "功能",
                                    Description_US = Enum.GetName(typeof(CapacityAndOtherSwtich), i + 0x10),
                                    CurrentValue = creatSwitchParameterID_DPC(message[offset])[i] == 1 ? "开启" : "关闭"
                                }));
                            }
                            if (IsEnglishUI)
                            {
                                EnableControlCollection.Add(ViewModelSource.Create(() => new SpecialParameter
                                {
                                    SpecialParameterID = EnableControlCollection.Count + 1,
                                    CommandType = dataParameter.ID + 1,
                                    Description = Enum.GetName(typeof(CapacityAndOtherSwtich), i) + "功能",
                                    Description_US = Enum.GetName(typeof(CapacityAndOtherSwtich), i + 0x10),
                                    CurrentValue = creatSwitchParameterID_DPC(message[offset])[i] == 1 ? "On" : "Off"
                                }));
                            }
                        }
                    }
                    break;
                case "指示功能开关参数":
                    for (int i = 0; i < 2; i++)
                    {
                        if (i == 0)
                        {
                            if (IsChineseUI)
                            {
                                EnableControlCollection.Add(ViewModelSource.Create(() => new SpecialParameter
                                {
                                    SpecialParameterID = EnableControlCollection.Count + 1,
                                    CommandType = dataParameter.ID + 1,
                                    Description = Enum.GetName(typeof(IindicatorLightSwtich), i) + "功能",
                                    Description_US = Enum.GetName(typeof(IindicatorLightSwtich), i + 0x10),
                                    CurrentValue = creatSwitchParameterID_DPC(message[offset])[i] == 1 ? "开启" : "关闭"
                                }));
                            }
                            if (IsEnglishUI)
                            {
                                EnableControlCollection.Add(ViewModelSource.Create(() => new SpecialParameter
                                {
                                    SpecialParameterID = EnableControlCollection.Count + 1,
                                    CommandType = dataParameter.ID + 1,
                                    Description = Enum.GetName(typeof(IindicatorLightSwtich), i) + "功能",
                                    Description_US = Enum.GetName(typeof(IindicatorLightSwtich), i + 0x10),
                                    CurrentValue = creatSwitchParameterID_DPC(message[offset])[i] == 1 ? "On" : "Off"
                                }));
                            }
                        }
                        else
                        {
                            if (IsChineseUI)
                            {
                                EnableControlCollection.Add(ViewModelSource.Create(() => new SpecialParameter
                                {
                                    SpecialParameterID = EnableControlCollection.Count + 1,
                                    CommandType = dataParameter.ID + 1,
                                    Description = Enum.GetName(typeof(IindicatorLightSwtich), i) + "功能",
                                    Description_US = Enum.GetName(typeof(IindicatorLightSwtich), i + 0x10),
                                    CurrentValue = creatSwitchParameterID_DPC(message[offset])[i] == 1 ? "开启" : "关闭"
                                }));
                            }
                            if (IsEnglishUI)
                            {
                                EnableControlCollection.Add(ViewModelSource.Create(() => new SpecialParameter
                                {
                                    SpecialParameterID = EnableControlCollection.Count + 1,
                                    CommandType = dataParameter.ID + 1,
                                    Description = Enum.GetName(typeof(IindicatorLightSwtich), i) + "功能",
                                    Description_US = Enum.GetName(typeof(IindicatorLightSwtich), i + 0x10),
                                    CurrentValue = creatSwitchParameterID_DPC(message[offset])[i] == 1 ? "On" : "Off"
                                }));
                            }
                        }
                    }
                    break;
            }
        }
        private void ProcessBatteryGroupWarning(BatteryGroup batteryGroup, byte warningValue, byte targetValue, string warningInfo)
        {
            if (warningValue == targetValue)
            {
                batteryGroup.WarningCollection.Add(new WarningItem { WarningItemID = batteryGroup.WarningCollection.Count + 1, WarningInfo = warningInfo });
            }
        }
        private void ProcessSingleCellWarning(BatteryGroup batteryGroup)
        {
            foreach (Cell cell in batteryGroup.CellCollection)
            {
                if (cell.VoltageWarningStatus != VoltageWarningType.正常 && cell.VoltageWarningStatus != VoltageWarningType.Ok)
                {
                    bool flag = false;

                    foreach (WarningItem item in batteryGroup.WarningCollection)//20200423
                    {
                        //if (item.WarningInfo == "单体电芯" + cell.VoltageWarningStatus.ToString())
                        if (item.WarningInfo == cell.VoltageWarningStatus.ToString())
                        {
                            flag = true;
                            break;
                        }
                    }

                    if (!flag)
                    {
                        batteryGroup.WarningCollection.Add(new WarningItem { WarningItemID = batteryGroup.WarningCollection.Count + 1, WarningInfo = cell.VoltageWarningStatus.ToString() });
                    }
                }

                if (cell.TemperatureWarningStatus != TemperatureWarningType.无告警 && cell.TemperatureWarningStatus != TemperatureWarningType.Ok)
                {
                    bool flag = false;

                    foreach (WarningItem item in batteryGroup.WarningCollection)
                    {
                        if (item.WarningInfo == cell.TemperatureWarningStatus.ToString())
                        {
                            flag = true;
                            break;
                        }
                    }

                    if (!flag)
                    {
                       batteryGroup.WarningCollection.Add(new WarningItem { WarningItemID = batteryGroup.WarningCollection.Count + 1, WarningInfo = cell.TemperatureWarningStatus.ToString() });
                    }
                }
            }
        }
        // 遥信量（告警）
        private void ParseMessage_WarningData_DPC(byte[] message)
        {
            if (message.Length != 0x3B)//???
            {
                return;
            }
            int offset = 8;
            int packAddress = message[2] - 1;

            if (ProtocalProvider.BatteryGroupCount == 0xFF)
            {
                //单体电池数量
                PackCollection[packAddress].BatteryGroupCount = message[offset++];
            }
            else
            {
                PackCollection[packAddress].BatteryGroupCount = 1;
            }

            if (PackCollection[packAddress].BatteryGroupCollection.Count == PackCollection[packAddress].BatteryGroupCount)
            {
                for (int i = 0; i != PackCollection[packAddress].BatteryGroupCount; i++)
                {
                    offset++;

                    PackCollection[packAddress].BatteryGroupCollection[i].SingleCellVoltageCount = message[offset++];

                    if (PackCollection[packAddress].BatteryGroupCollection[i].SingleCellVoltageCount == PackCollection[packAddress].BatteryGroupCollection[i].CellCollection.Count)
                    {
                        for (int j = 0; j != PackCollection[packAddress].BatteryGroupCollection[i].SingleCellVoltageCount; j++)
                        {
                            PackCollection[packAddress].BatteryGroupCollection[i].CellCollection[j].VoltageWarningStatus_DPC = (Cell_WarningState_DPC)message[offset++];
                            if (IsEnglishUI)
                            {
                                PackCollection[packAddress].BatteryGroupCollection[i].CellCollection[j].VoltageWarningStatus_DPC = PackCollection[packAddress].BatteryGroupCollection[i].CellCollection[j].VoltageWarningStatus_DPC + 0x10;
                            }
                                
                        }

                        //电芯温度数量
                        PackCollection[packAddress].BatteryGroupCollection[i].SingleCellTemperatureCount = message[offset++] - 2;//???

                        if (PackCollection[packAddress].BatteryGroupCollection[i].SingleCellTemperatureCount <= PackCollection[packAddress].BatteryGroupCollection[i].CellCollection.Count)
                        {
                            for (int j = 0; j != PackCollection[packAddress].BatteryGroupCollection[i].SingleCellTemperatureCount; j++)
                            {
                                PackCollection[packAddress].BatteryGroupCollection[i].CellCollection[j].TemperatureWarningStatus_DPC = (Cell_WarningState_DPC)message[offset++];
                                if (IsEnglishUI)
                                {
                                    PackCollection[packAddress].BatteryGroupCollection[i].CellCollection[j].TemperatureWarningStatus_DPC = PackCollection[packAddress].BatteryGroupCollection[i].CellCollection[j].TemperatureWarningStatus_DPC + 0x10;
                                }
                            }

                            // 特殊处理温度告警
                            if (IsChineseUI)
                            {
                                PackCollection[packAddress].BatteryGroupCollection[i].CellCollection[2].TemperatureWarningStatus_DPC = (Cell_WarningState_DPC)message[offset++];
                                PackCollection[packAddress].BatteryGroupCollection[i].CellCollection[3].TemperatureWarningStatus_DPC = (Cell_WarningState_DPC)message[offset++];
                            }
                            else
                            {
                                PackCollection[packAddress].BatteryGroupCollection[i].CellCollection[2].TemperatureWarningStatus_DPC = (Cell_WarningState_DPC)(message[offset++] + 0x10);
                                PackCollection[packAddress].BatteryGroupCollection[i].CellCollection[3].TemperatureWarningStatus_DPC = (Cell_WarningState_DPC)(message[offset++] + 0x10);
                            }
                            //4到15的电芯温度X告警状态
                            for (int n = 4; n != PackCollection[packAddress].BatteryGroupCollection[0].CellCollection.Count; n++)
                            {
                                if (IsEnglishUI)
                                {
                                    PackCollection[packAddress].BatteryGroupCollection[0].CellCollection[n].TemperatureWarningStatus_DPC = Cell_WarningState_DPC.ok;
                                }
                                if (IsChineseUI)
                                {
                                    PackCollection[packAddress].BatteryGroupCollection[0].CellCollection[n].TemperatureWarningStatus_DPC = Cell_WarningState_DPC.无告警;
                                }
                            }

                            PackCollection[packAddress].BatteryGroupCollection[i].WarningCollection.Clear();

                            byte[] byteTemp;

                            //环境温度告警状态 到 剩余容量告警
                            if (IsChineseUI)
                            {
                                ProcessBatteryGroupWarning_DPC(PackCollection[packAddress].BatteryGroupCollection[i], message[offset], "环境温度告警：" + (Cell_WarningState_DPC)message[offset]); 
                                offset++;
                                ProcessBatteryGroupWarning_DPC(PackCollection[packAddress].BatteryGroupCollection[i], message[offset], "MOS温度告警：" + (Cell_WarningState_DPC)message[offset]); 
                                offset++;
                                ProcessBatteryGroupWarning_DPC(PackCollection[packAddress].BatteryGroupCollection[i], message[offset], "充放电流告警：" + (Cell_WarningState_DPC)message[offset]); 
                                offset++;
                                ProcessBatteryGroupWarning_DPC(PackCollection[packAddress].BatteryGroupCollection[i], message[offset], "电池总压告警：" + (Cell_WarningState_DPC)message[offset]); 
                                offset++;
                                //自定义告警量数量
                                PackCollection[packAddress].BatteryGroupCollection[i].UserSelfDefineCount_Warning = message[offset++];

                                //////////////////////////////////////////////////////////////////////////

                                // 均衡事件代码解析
                                byteTemp = creatSwitchParameterID_DPC(message[offset]); 
                                offset++;
                                for (int t = 0; t < byteTemp.Length; t++)
                                {
                                    //根据byte数组的值
                                    if (byteTemp[t] != 0)
                                    {
                                        //处理电池组事件
                                        ProcessBatteryGroupEvent_DPC(PackCollection[packAddress].BatteryGroupCollection[i], "均衡事件：" + (BalanceEvent_DPC)t);
                                    }
                                }
                                // 电压事件代码解析
                                byteTemp = creatSwitchParameterID_DPC(message[offset]); 
                                offset++;
                                for (int t = 0; t < byteTemp.Length; t++)
                                {
                                    if (byteTemp[t] != 0)
                                    {
                                        ProcessBatteryGroupEvent_DPC(PackCollection[packAddress].BatteryGroupCollection[i], "电压事件：" + (VoltageEvent_DPC)t);
                                    }
                                }
                                // 温度事件代码 1byte
                                byteTemp = creatSwitchParameterID_DPC(message[offset]); 
                                offset++;
                                for (int t = 0; t < byteTemp.Length; t++)
                                {
                                    if (byteTemp[t] != 0)
                                    {
                                        ProcessBatteryGroupEvent_DPC(PackCollection[packAddress].BatteryGroupCollection[i], "温度事件：" + (TemperatureEvent_DPC)(t + 8));
                                    }
                                }

                                // 温度事件代码 2byte
                                byteTemp = creatSwitchParameterID_DPC(message[offset]); 
                                offset++;
                                for (int t = 0; t < byteTemp.Length; t++)
                                {
                                    if (byteTemp[t] != 0)
                                    {
                                        ProcessBatteryGroupEvent_DPC(PackCollection[packAddress].BatteryGroupCollection[i], "温度事件：" + (TemperatureEvent_DPC)t);
                                    }
                                }

                                // 电流事件代码
                                byteTemp = creatSwitchParameterID_DPC(message[offset]); offset++;
                                for (int t = 0; t < byteTemp.Length; t++)
                                {
                                    if (byteTemp[t] != 0)
                                    {
                                        ProcessBatteryGroupEvent_DPC(PackCollection[packAddress].BatteryGroupCollection[i], "电流事件：" + (CurrentEvent_DPC)t);
                                    }
                                }

                                //剩余容量告警
                                byteTemp = creatSwitchParameterID_DPC(message[offset]); offset++;
                                for (int t = 0; t < byteTemp.Length; t++)
                                {
                                    if (byteTemp[t] != 0)
                                    {
                                        ProcessBatteryGroupEvent_DPC(PackCollection[packAddress].BatteryGroupCollection[i], "剩余容量告警：" + (SOC_WarningState_DPC)t);
                                    }
                                }

                            }
                            else
                            {
                                ProcessBatteryGroupWarning_DPC(PackCollection[packAddress].BatteryGroupCollection[i], message[offset], "Ambient_T_Warning:" + (Cell_WarningState_DPC)(message[offset] + 0x10)); offset++;
                                ProcessBatteryGroupWarning_DPC(PackCollection[packAddress].BatteryGroupCollection[i], message[offset], "Power_T_Warning:" + (Cell_WarningState_DPC)(message[offset] + 0x10)); offset++;
                                ProcessBatteryGroupWarning_DPC(PackCollection[packAddress].BatteryGroupCollection[i], message[offset], "CH_Current_Warning:" + (Cell_WarningState_DPC)(message[offset] + 0x10)); offset++;
                                ProcessBatteryGroupWarning_DPC(PackCollection[packAddress].BatteryGroupCollection[i], message[offset], "BB_Voltage_Warning:" + (Cell_WarningState_DPC)(message[offset] + 0x10)); offset++;

                                PackCollection[packAddress].BatteryGroupCollection[i].UserSelfDefineCount_Warning = message[offset++];

                                byteTemp = creatSwitchParameterID_DPC(message[offset]); 
                                offset++;
                                for (int t = 0; t < byteTemp.Length; t++) 
                                { if (byteTemp[t] != 0) 
                                    { 
                                        ProcessBatteryGroupEvent_DPC(PackCollection[packAddress].BatteryGroupCollection[i], "Balance Event:" + (BalanceEvent_DPC)((t + 0x10))); 
                                    } 
                                }

                                byteTemp = creatSwitchParameterID_DPC(message[offset]); 
                                offset++;
                                for (int t = 0; t < byteTemp.Length; t++) 
                                { 
                                    if (byteTemp[t] != 0) 
                                    { 
                                        ProcessBatteryGroupEvent_DPC(PackCollection[packAddress].BatteryGroupCollection[i], "Voltage Event:" + (VoltageEvent_DPC)(t + 0x10)); 
                                    } 
                                }

                                byteTemp = creatSwitchParameterID_DPC(message[offset]); 
                                offset++;
                                for (int t = 0; t < byteTemp.Length; t++) 
                                { 
                                    if (byteTemp[t] != 0) 
                                    { 
                                        ProcessBatteryGroupEvent_DPC(PackCollection[packAddress].BatteryGroupCollection[i], "Temperature Event: " + (TemperatureEvent_DPC)(t + 0x10)); 
                                    } 
                                }

                                byteTemp = creatSwitchParameterID_DPC(message[offset]); 
                                offset++;
                                for (int t = 0; t < byteTemp.Length; t++) 
                                { 
                                    if (byteTemp[t] != 0) 
                                    { 
                                        ProcessBatteryGroupEvent_DPC(PackCollection[packAddress].BatteryGroupCollection[i], "Temperature Event:" + (TemperatureEvent_DPC)((t + 0x10) + 8)); 
                                    } 
                                }

                                byteTemp = creatSwitchParameterID_DPC(message[offset]); 
                                offset++;
                                for (int t = 0; t < byteTemp.Length; t++) 
                                { 
                                    if (byteTemp[t] != 0) 
                                    { 
                                        ProcessBatteryGroupEvent_DPC(PackCollection[packAddress].BatteryGroupCollection[i], "Current Event:" + (CurrentEvent_DPC)(t + 0x10)); 
                                    } 
                                }

                                byteTemp = creatSwitchParameterID_DPC(message[offset]); 
                                offset++;
                                for (int t = 0; t < byteTemp.Length; t++) 
                                { 
                                    if (byteTemp[t] != 0) 
                                    { 
                                        ProcessBatteryGroupEvent_DPC(PackCollection[packAddress].BatteryGroupCollection[i], "SOC State:" + (SOC_WarningState_DPC)(t + 0x10)); 
                                    } 
                                }

                            }
                            
                            try
                            {
                                //FET 状态
                                byteTemp = creatSwitchParameterID_DPC(message[offset]); 
                                offset++;
                                PackCollection[packAddress].BatteryGroupCollection[i].ChargeCircuitSwitchStatus = (SwitchStatus)byteTemp[1];  // 充电开关开启
                                PackCollection[packAddress].BatteryGroupCollection[i].DischargeCircuitSwitchStatus = (SwitchStatus)byteTemp[0];  // 放电开关开启
                                PackCollection[packAddress].BatteryGroupCollection[i].BeeperSwitchStatus = (SwitchStatus)byteTemp[2];    // 限流开关开启
                                PackCollection[packAddress].BatteryGroupCollection[i].HeatingFilmSwitchStatus = (SwitchStatus)byteTemp[3];   // 加热开关开启

                                // 系统状态代码 ???
                                if (message[offset] == 0x04)//0000 0100
                                {
                                    PackCollection[packAddress].BatteryGroupCollection[i].BatteryStatus_DPC = SystemState_DPC.待机;
                                }
                                else if (message[offset] == 0x10)//0001 0000
                                {
                                    PackCollection[packAddress].BatteryGroupCollection[i].BatteryStatus_DPC = SystemState_DPC.保护;
                                }
                                else if (message[offset] == 0x0C)//0000 1100
                                {
                                    PackCollection[packAddress].BatteryGroupCollection[i].BatteryStatus_DPC = SystemState_DPC.静置;
                                }
                                else if (message[offset] == 0x20)//0010 0000
                                {
                                    PackCollection[packAddress].BatteryGroupCollection[i].BatteryStatus_DPC = SystemState_DPC.故障;
                                }
                                else
                                {
                                    byteTemp = creatSwitchParameterID_DPC(message[offset]);

                                    for (int t = 0; t < byteTemp.Length; t++)
                                    {
                                        if (byteTemp[t] != 0)
                                        {
                                            PackCollection[packAddress].BatteryGroupCollection[i].BatteryStatus_DPC = (SystemState_DPC)t;
                                        }
                                    }
                                }
                                offset++;

                                //均衡状态代码解析
                                PackCollection[packAddress].BatteryGroupCollection[0].CellCollection[8].BalanceStatus = (message[offset] & 0x01) == 0x01 ? BalanceStatus.Active : BalanceStatus.Off;
                                PackCollection[packAddress].BatteryGroupCollection[0].CellCollection[9].BalanceStatus = (message[offset] & 0x02) == 0x02 ? BalanceStatus.Active : BalanceStatus.Off;
                                PackCollection[packAddress].BatteryGroupCollection[0].CellCollection[10].BalanceStatus = (message[offset] & 0x04) == 0x04 ? BalanceStatus.Active : BalanceStatus.Off;
                                PackCollection[packAddress].BatteryGroupCollection[0].CellCollection[11].BalanceStatus = (message[offset] & 0x08) == 0x08 ? BalanceStatus.Active : BalanceStatus.Off;
                                PackCollection[packAddress].BatteryGroupCollection[0].CellCollection[12].BalanceStatus = (message[offset] & 0x10) == 0x10 ? BalanceStatus.Active : BalanceStatus.Off;
                                PackCollection[packAddress].BatteryGroupCollection[0].CellCollection[13].BalanceStatus = (message[offset] & 0x20) == 0x20 ? BalanceStatus.Active : BalanceStatus.Off;
                                PackCollection[packAddress].BatteryGroupCollection[0].CellCollection[14].BalanceStatus = (message[offset] & 0x40) == 0x40 ? BalanceStatus.Active : BalanceStatus.Off;
                                //PackCollection[packAddress].BatteryGroupCollection[0].CellCollection[15].BalanceStatus = (message[offset] & 0x80) == 0x80 ? BalanceStatus.Active : BalanceStatus.Off;
                                offset++;

                                PackCollection[packAddress].BatteryGroupCollection[0].CellCollection[0].BalanceStatus = (message[offset] & 0x01) == 0x01 ? BalanceStatus.Active : BalanceStatus.Off;
                                PackCollection[packAddress].BatteryGroupCollection[0].CellCollection[1].BalanceStatus = (message[offset] & 0x02) == 0x02 ? BalanceStatus.Active : BalanceStatus.Off;
                                PackCollection[packAddress].BatteryGroupCollection[0].CellCollection[2].BalanceStatus = (message[offset] & 0x04) == 0x04 ? BalanceStatus.Active : BalanceStatus.Off;
                                PackCollection[packAddress].BatteryGroupCollection[0].CellCollection[3].BalanceStatus = (message[offset] & 0x08) == 0x08 ? BalanceStatus.Active : BalanceStatus.Off;
                                PackCollection[packAddress].BatteryGroupCollection[0].CellCollection[4].BalanceStatus = (message[offset] & 0x10) == 0x10 ? BalanceStatus.Active : BalanceStatus.Off;
                                PackCollection[packAddress].BatteryGroupCollection[0].CellCollection[5].BalanceStatus = (message[offset] & 0x20) == 0x20 ? BalanceStatus.Active : BalanceStatus.Off;
                                PackCollection[packAddress].BatteryGroupCollection[0].CellCollection[6].BalanceStatus = (message[offset] & 0x40) == 0x40 ? BalanceStatus.Active : BalanceStatus.Off;
                                PackCollection[packAddress].BatteryGroupCollection[0].CellCollection[7].BalanceStatus = (message[offset] & 0x80) == 0x80 ? BalanceStatus.Active : BalanceStatus.Off;
                                offset++; 
                                offset++;
                                offset++;
                                if (IsChineseUI)
                                {
                                    //处理电池组警告
                                    ProcessBatteryGroupWarning_DPC(PackCollection[packAddress].BatteryGroupCollection[i], message[offset], "单体压差失效");
                                }
                                if (IsEnglishUI)
                                {
                                    ProcessBatteryGroupWarning_DPC(PackCollection[packAddress].BatteryGroupCollection[i], message[offset], "Cell Volt Diff Fail");
                                }
                                offset++; offset++; offset++; offset++; offset++; offset++;

                                offset++;

                                byteTemp = creatSwitchParameterID_DPC(message[offset]); 
                                offset++;
                                for (int t = 0; t < byteTemp.Length; t++)
                                {
                                    if (byteTemp[t] != 0)
                                    {
                                        if (IsChineseUI)
                                        {
                                            ProcessBatteryGroupEvent_DPC(PackCollection[packAddress].BatteryGroupCollection[i], "TOPBAND：" + (TOPBAND_DPC)t);
                                        }
                                        if (IsEnglishUI)
                                        {
                                            ProcessBatteryGroupEvent_DPC(PackCollection[packAddress].BatteryGroupCollection[i], "TOPBAND：" + (TOPBAND_DPC)(t + 0x10));

                                        }
                                    }
                                }
                                if (IsEnglishUI)
                                {
                                    PackCollection[packAddress].BatteryGroupCollection[i].ChargeCircuitSwitchStatus = PackCollection[packAddress].BatteryGroupCollection[i].ChargeCircuitSwitchStatus + 3;
                                    PackCollection[packAddress].BatteryGroupCollection[i].DischargeCircuitSwitchStatus = PackCollection[packAddress].BatteryGroupCollection[i].DischargeCircuitSwitchStatus + 3;
                                    PackCollection[packAddress].BatteryGroupCollection[i].BeeperSwitchStatus = PackCollection[packAddress].BatteryGroupCollection[i].BeeperSwitchStatus + 3;
                                    PackCollection[packAddress].BatteryGroupCollection[i].HeatingFilmSwitchStatus = PackCollection[packAddress].BatteryGroupCollection[i].HeatingFilmSwitchStatus + 3;
                                    PackCollection[packAddress].BatteryGroupCollection[i].BatteryStatus_DPC = PackCollection[packAddress].BatteryGroupCollection[i].BatteryStatus_DPC + 0x10;
                                }
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }

                            ProcessSingleCellWarning_DPC(PackCollection[packAddress].BatteryGroupCollection[i]);
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        return;
                    }
                }
            }

        }
        private void ParseMessage_WarningData(byte[] message)
        {
            if (message.Length != 0x4e)
            {
                return;
            }

            int offset = 8;
            int packAddress = message[2] - 1;
            int enumValue = 0;

            if (ProtocalProvider.BatteryGroupCount == 0xFF)
            {
                PackCollection[packAddress].BatteryGroupCount = message[offset++];
            }
            else
            {
                PackCollection[packAddress].BatteryGroupCount = 1;
            }

            if (PackCollection[packAddress].BatteryGroupCollection.Count == PackCollection[packAddress].BatteryGroupCount)
            {
                for (int i = 0; i != PackCollection[packAddress].BatteryGroupCount; i++)
                {
                    offset++;

                    PackCollection[packAddress].BatteryGroupCollection[i].SingleCellVoltageCount = message[offset++];

                    if (PackCollection[packAddress].BatteryGroupCollection[i].SingleCellVoltageCount == PackCollection[packAddress].BatteryGroupCollection[i].CellCollection.Count)
                    {
                        for (int j = 0; j != PackCollection[packAddress].BatteryGroupCollection[i].SingleCellVoltageCount; j++)
                        {
                            enumValue = message[offset++];
                            PackCollection[packAddress].BatteryGroupCollection[i].CellCollection[j].VoltageWarningStatus = Enum.IsDefined(typeof(VoltageWarningType), enumValue) ? (VoltageWarningType)enumValue : VoltageWarningType.未定义;
                            if (IsEnglishUI) PackCollection[packAddress].BatteryGroupCollection[i].CellCollection[j].VoltageWarningStatus = PackCollection[packAddress].BatteryGroupCollection[i].CellCollection[j].VoltageWarningStatus + 6;
                        }

                        for (int j = 0; j != PackCollection[packAddress].BatteryGroupCollection[i].SingleCellVoltageCount; j++)
                        {
                            if ((enumValue = message[offset++]) != 0)
                            {
                                PackCollection[packAddress].BatteryGroupCollection[i].CellCollection[j].VoltageWarningStatus = Enum.IsDefined(typeof(VoltageWarningType), (enumValue + 2)) ? (VoltageWarningType)(enumValue + 2) : VoltageWarningType.未定义;
                                if (IsEnglishUI) PackCollection[packAddress].BatteryGroupCollection[i].CellCollection[j].VoltageWarningStatus = PackCollection[packAddress].BatteryGroupCollection[i].CellCollection[j].VoltageWarningStatus + 6;
                            }
                        }

                        PackCollection[packAddress].BatteryGroupCollection[i].WarningCollection.Clear();

                        if (IsChineseUI)
                        {
                            ProcessBatteryGroupWarning(PackCollection[packAddress].BatteryGroupCollection[i], message[offset], 1, "电池组欠压告警");
                            ProcessBatteryGroupWarning(PackCollection[packAddress].BatteryGroupCollection[i], message[offset++], 2, "电池组过压告警");
                            ProcessBatteryGroupWarning(PackCollection[packAddress].BatteryGroupCollection[i], message[offset], 1, "电池组欠压保护");
                            ProcessBatteryGroupWarning(PackCollection[packAddress].BatteryGroupCollection[i], message[offset++], 2, "电池组过压保护");
                            ProcessBatteryGroupWarning(PackCollection[packAddress].BatteryGroupCollection[i], message[offset++], 1, "单体电压落后告警");
                        }
                        else
                        {
                            ProcessBatteryGroupWarning(PackCollection[packAddress].BatteryGroupCollection[i], message[offset], 1, "Pack BV Warning");
                            ProcessBatteryGroupWarning(PackCollection[packAddress].BatteryGroupCollection[i], message[offset++], 2, "Pack OV Warning");
                            ProcessBatteryGroupWarning(PackCollection[packAddress].BatteryGroupCollection[i], message[offset], 1, "Pack BV Protection");
                            ProcessBatteryGroupWarning(PackCollection[packAddress].BatteryGroupCollection[i], message[offset++], 2, "Pack OV Protection");
                            ProcessBatteryGroupWarning(PackCollection[packAddress].BatteryGroupCollection[i], message[offset++], 1, "Cell Voltage Behind");
                        }

                        PackCollection[packAddress].BatteryGroupCollection[i].SingleCellTemperatureCount = message[offset++];

                        if (PackCollection[packAddress].BatteryGroupCollection[i].SingleCellTemperatureCount <= PackCollection[packAddress].BatteryGroupCollection[i].CellCollection.Count)
                        {
                            for (int j = 0; j != PackCollection[packAddress].BatteryGroupCollection[i].SingleCellTemperatureCount; j++)
                            {
                                PackCollection[packAddress].BatteryGroupCollection[i].CellCollection[j].TemperatureWarningStatus = TemperatureWarningType.无告警;
                                if (IsEnglishUI) PackCollection[packAddress].BatteryGroupCollection[i].CellCollection[j].TemperatureWarningStatus = TemperatureWarningType.Ok;
                            }
                            for (int k = 0; k != 10; k += 2)
                            {
                                for (int j = 0; j != PackCollection[packAddress].BatteryGroupCollection[i].SingleCellTemperatureCount; j++)
                                {
                                    if ((enumValue = message[offset++]) != 0)
                                    {
                                        PackCollection[packAddress].BatteryGroupCollection[i].CellCollection[j].TemperatureWarningStatus = Enum.IsDefined(typeof(TemperatureWarningType), (enumValue + k)) ? (TemperatureWarningType)(enumValue + k) : TemperatureWarningType.未定义;
                                        if (IsEnglishUI) PackCollection[packAddress].BatteryGroupCollection[i].CellCollection[j].TemperatureWarningStatus = PackCollection[packAddress].BatteryGroupCollection[i].CellCollection[j].TemperatureWarningStatus + 16;
                                    }
                                }
                            }

                            for (int n = 4; n != PackCollection[packAddress].BatteryGroupCollection[0].CellCollection.Count; n++)
                            {
                                if (IsEnglishUI) PackCollection[packAddress].BatteryGroupCollection[0].CellCollection[n].TemperatureWarningStatus = TemperatureWarningType.Ok;
                                if (IsChineseUI) PackCollection[packAddress].BatteryGroupCollection[0].CellCollection[n].TemperatureWarningStatus = TemperatureWarningType.无告警;
                            }

                            if (IsChineseUI)
                            {
                                ProcessBatteryGroupWarning(PackCollection[packAddress].BatteryGroupCollection[i], message[offset++], 1, "环境温度高告警");
                                ProcessBatteryGroupWarning(PackCollection[packAddress].BatteryGroupCollection[i], message[offset++], 1, "环境温度低告警");
                                ProcessBatteryGroupWarning(PackCollection[packAddress].BatteryGroupCollection[i], message[offset++], 1, "环境温度传感器无效告警");

                                if (SerialPortCommunicator.ManufacturerName == "ZTE-C" && SerialPortCommunicator.ZTE_Type == "FB150C")//20200528
                                {
                                    ProcessBatteryGroupWarning(PackCollection[packAddress].BatteryGroupCollection[i], message[offset++], 1, "电池组充电过流告警");
                                }
                                else
                                {
                                    ProcessBatteryGroupWarning(PackCollection[packAddress].BatteryGroupCollection[i], message[offset++], 1, "电池组充电过流告警");
                                    //ProcessBatteryGroupWarning(PackCollection[packAddress].BatteryGroupCollection[i], message[offset++], 1, "电池组充电过流告警");//20200617中兴要求
                                }

                                ProcessBatteryGroupWarning(PackCollection[packAddress].BatteryGroupCollection[i], message[offset++], 1, "电池组充电过流保护");
                                ProcessBatteryGroupWarning(PackCollection[packAddress].BatteryGroupCollection[i], message[offset++], 1, "电池组放电过流告警");
                                ProcessBatteryGroupWarning(PackCollection[packAddress].BatteryGroupCollection[i], message[offset++], 1, "电池组放电过流保护");
                                PackCollection[packAddress].BatteryGroupCollection[i].UserSelfDefineCount_Warning = message[offset++];
                                ProcessBatteryGroupWarning(PackCollection[packAddress].BatteryGroupCollection[i], message[offset++], 1, "电池组短路保护或极性反接保护");
                                ProcessBatteryGroupWarning(PackCollection[packAddress].BatteryGroupCollection[i], message[offset++], 1, "放电回路开关失效告警");
                                ProcessBatteryGroupWarning(PackCollection[packAddress].BatteryGroupCollection[i], message[offset++], 1, "充电回路开关失效告警");
                            }
                            else
                            {
                                ProcessBatteryGroupWarning(PackCollection[packAddress].BatteryGroupCollection[i], message[offset++], 1, "Abm. Temperature High Warning");
                                ProcessBatteryGroupWarning(PackCollection[packAddress].BatteryGroupCollection[i], message[offset++], 1, "Abm. Temperature Low Warning");
                                ProcessBatteryGroupWarning(PackCollection[packAddress].BatteryGroupCollection[i], message[offset++], 1, "Abm. Temperature Sensor Failure");
                                if (SerialPortCommunicator.ManufacturerName == "ZTE-C" && SerialPortCommunicator.ZTE_Type == "FB150C")//20200528
                                {
                                    ProcessBatteryGroupWarning(PackCollection[packAddress].BatteryGroupCollection[i], message[offset++], 1, "Pack CH OC Warning");
                                }
                                else
                                {
                                    ProcessBatteryGroupWarning(PackCollection[packAddress].BatteryGroupCollection[i], message[offset++], 1, "Pack CH OC Warning");
                                    //ProcessBatteryGroupWarning(PackCollection[packAddress].BatteryGroupCollection[i], message[offset++], 1, "Pack CH OC Warning");//20200617中兴要求
                                }
                                ProcessBatteryGroupWarning(PackCollection[packAddress].BatteryGroupCollection[i], message[offset++], 1, "Pack CH OC Protection");
                                ProcessBatteryGroupWarning(PackCollection[packAddress].BatteryGroupCollection[i], message[offset++], 1, "Pack DCH OC Warning");
                                ProcessBatteryGroupWarning(PackCollection[packAddress].BatteryGroupCollection[i], message[offset++], 1, "Pack DCH OC Protection");
                                PackCollection[packAddress].BatteryGroupCollection[i].UserSelfDefineCount_Warning = message[offset++];
                                ProcessBatteryGroupWarning(PackCollection[packAddress].BatteryGroupCollection[i], message[offset++], 1, "Pack Short Circuit");
                                ProcessBatteryGroupWarning(PackCollection[packAddress].BatteryGroupCollection[i], message[offset++], 1, "CH Switch Failure");
                                ProcessBatteryGroupWarning(PackCollection[packAddress].BatteryGroupCollection[i], message[offset++], 1, "DCH Switch Failure");
                            }
                            ProcessSingleCellWarning(PackCollection[packAddress].BatteryGroupCollection[i]);
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        return;
                    }
                }
            }
        }
        private void ParseMessage_WarningData_Coslight(byte[] message)
        {
            PackCollection[message[2] - 1].AntiThief = Enum.IsDefined(typeof(WarningType), (int)message[16]) ? (WarningType)message[16] : WarningType.未定义;

            if (IsEnglishUI)
            {
                PackCollection[message[2] - 1].AntiThief += 3;
            }

            PackCollection[message[2] - 1].MOSProtectionStatus = Enum.IsDefined(typeof(MOSProtectionType), (int)message[8]) ? (MOSProtectionType)message[8] : MOSProtectionType.未定义;
            if (IsEnglishUI) PackCollection[message[2] - 1].MOSProtectionStatus = PackCollection[message[2] - 1].MOSProtectionStatus + 4;

            PackCollection[message[2] - 1].PackCH_OC_Level2_Protection = Enum.IsDefined(typeof(WarningType), (int)message[9]) ? (WarningType)message[9] : WarningType.未定义;
            if (IsEnglishUI) PackCollection[message[2] - 1].PackCH_OC_Level2_Protection = PackCollection[message[2] - 1].PackCH_OC_Level2_Protection + 3;

            PackCollection[message[2] - 1].PackCH_OC_Level3_Protection = Enum.IsDefined(typeof(WarningType), (int)message[10]) ? (WarningType)message[10] : WarningType.未定义;
            if (IsEnglishUI) PackCollection[message[2] - 1].PackCH_OC_Level3_Protection = PackCollection[message[2] - 1].PackCH_OC_Level3_Protection + 3;

            PackCollection[message[2] - 1].PackDCH_OC_Level2_Protection = Enum.IsDefined(typeof(WarningType), (int)message[11]) ? (WarningType)message[11] : WarningType.未定义;
            if (IsEnglishUI) PackCollection[message[2] - 1].PackDCH_OC_Level2_Protection = PackCollection[message[2] - 1].PackDCH_OC_Level2_Protection + 3;

            PackCollection[message[2] - 1].SOCLowWarning = Enum.IsDefined(typeof(WarningType), (int)message[12]) ? (WarningType)message[12] : WarningType.未定义;
            if (IsEnglishUI) PackCollection[message[2] - 1].SOCLowWarning = PackCollection[message[2] - 1].SOCLowWarning + 3;

            PackCollection[message[2] - 1].SOCLowProtection = Enum.IsDefined(typeof(WarningType), (int)message[13]) ? (WarningType)message[13] : WarningType.未定义;
            if (IsEnglishUI) PackCollection[message[2] - 1].SOCLowProtection = PackCollection[message[2] - 1].SOCLowProtection + 3;
            PackCollection[message[2] - 1].PrechargeFailure = Enum.IsDefined(typeof(WarningType1), (int)message[14]) ? (WarningType1)message[14] : WarningType1.未定义;
            if (IsEnglishUI) PackCollection[message[2] - 1].PrechargeFailure = PackCollection[message[2] - 1].PrechargeFailure + 3;

            PackCollection[message[2] - 1].PoleBackward = Enum.IsDefined(typeof(WarningType), (int)message[15]) ? (WarningType)message[15] : WarningType.未定义;
            if (IsEnglishUI) PackCollection[message[2] - 1].PoleBackward = PackCollection[message[2] - 1].PoleBackward + 3;

            PackCollection[message[2] - 1].VoltageSampleFailure = Enum.IsDefined(typeof(WarningType), (int)message[16]) ? (WarningType)message[16] : WarningType.未定义;
            if (IsEnglishUI) PackCollection[message[2] - 1].VoltageSampleFailure = PackCollection[message[2] - 1].VoltageSampleFailure + 3;

            PackCollection[message[2] - 1].CellFailure = Enum.IsDefined(typeof(WarningType), (int)message[17]) ? (WarningType)message[17] : WarningType.未定义;
            if (IsEnglishUI) PackCollection[message[2] - 1].CellFailure = PackCollection[message[2] - 1].CellFailure + 3;

            if (SerialPortCommunicator.ManufacturerName == "ZTE-C" && SerialPortCommunicator.ZTE_Type != "FB150C")//20200528
            {
                PackCollection[message[2] - 1].CellConsistencyProtection = Enum.IsDefined(typeof(WarningType), (int)message[18]) ? (WarningType)message[18] : WarningType.未定义;
                if (IsEnglishUI) PackCollection[message[2] - 1].CellConsistencyProtection = PackCollection[message[2] - 1].CellConsistencyProtection + 3;

                PackCollection[message[2] - 1].MOSWarningStatus = Enum.IsDefined(typeof(MOSWarningType), (int)message[19]) ? (MOSWarningType)message[19] : MOSWarningType.未定义;
                if (IsEnglishUI) PackCollection[message[2] - 1].MOSWarningStatus = PackCollection[message[2] - 1].MOSWarningStatus + 4;

                PackCollection[message[2] - 1].SOHLowStatus = Enum.IsDefined(typeof(WarningProtectType), (int)message[20]) ? (WarningProtectType)message[20] : WarningProtectType.未定义;
                if (IsEnglishUI) PackCollection[message[2] - 1].SOHLowStatus = PackCollection[message[2] - 1].SOHLowStatus + 4;
            }

            if (PackCollection[message[2] - 1].BatteryGroupCount == 0)
            {
                PackCollection[message[2] - 1].BatteryGroupCount = 1;
            }

            PackCollection[message[2] - 1].BatteryGroupCollection[0].CoslightWarningCollection.Clear();

            if (PackCollection[message[2] - 1].MOSProtectionStatus != MOSProtectionType.正常 && PackCollection[message[2] - 1].MOSProtectionStatus != MOSProtectionType.OK)
            {
                PackCollection[message[2] - 1].BatteryGroupCollection[0].CoslightWarningCollection.Add(new WarningItem { WarningItemID = PackCollection[message[2] - 1].BatteryGroupCollection[0].CoslightWarningCollection.Count + 1, WarningInfo = PackCollection[message[2] - 1].MOSProtectionStatus.ToString() });
            }

            if (PackCollection[message[2] - 1].PackCH_OC_Level2_Protection != WarningType.正常 && PackCollection[message[2] - 1].PackCH_OC_Level2_Protection != WarningType.OK)
            {
                if (SerialPortCommunicator.ManufacturerName == "ZTE-C" && SerialPortCommunicator.ZTE_Type == "FB150C")//20200528
                {
                    if (IsChineseUI)
                    {
                        PackCollection[message[2] - 1].BatteryGroupCollection[0].CoslightWarningCollection.Add(new WarningItem { WarningItemID = PackCollection[message[2] - 1].BatteryGroupCollection[0].CoslightWarningCollection.Count + 1, WarningInfo = "电池组充电过流二级保护" });
                    }
                    else
                    {
                        PackCollection[message[2] - 1].BatteryGroupCollection[0].CoslightWarningCollection.Add(new WarningItem { WarningItemID = PackCollection[message[2] - 1].BatteryGroupCollection[0].CoslightWarningCollection.Count + 1, WarningInfo = "Pack CH OC Level2 Protection" });
                    }
                }
                else
                {
                    if (IsChineseUI)
                    {
                        PackCollection[message[2] - 1].BatteryGroupCollection[0].CoslightWarningCollection.Add(new WarningItem { WarningItemID = PackCollection[message[2] - 1].BatteryGroupCollection[0].CoslightWarningCollection.Count + 1, WarningInfo = "电池组充电过流二级告警" });
                    }
                    else
                    {
                        PackCollection[message[2] - 1].BatteryGroupCollection[0].CoslightWarningCollection.Add(new WarningItem { WarningItemID = PackCollection[message[2] - 1].BatteryGroupCollection[0].CoslightWarningCollection.Count + 1, WarningInfo = "Pack CH OC Level2 Warning" });
                    }
                }


            }

            if (PackCollection[message[2] - 1].PackCH_OC_Level3_Protection != WarningType.正常 && PackCollection[message[2] - 1].PackCH_OC_Level3_Protection != WarningType.OK)
            {
                if (SerialPortCommunicator.ManufacturerName == "ZTE-C" && SerialPortCommunicator.ZTE_Type == "FB150C")//20200528
                {
                    if (IsChineseUI)
                    {
                        PackCollection[message[2] - 1].BatteryGroupCollection[0].CoslightWarningCollection.Add(new WarningItem { WarningItemID = PackCollection[message[2] - 1].BatteryGroupCollection[0].CoslightWarningCollection.Count + 1, WarningInfo = "电池组充电过流三级保护" });
                    }
                    else
                    {
                        PackCollection[message[2] - 1].BatteryGroupCollection[0].CoslightWarningCollection.Add(new WarningItem { WarningItemID = PackCollection[message[2] - 1].BatteryGroupCollection[0].CoslightWarningCollection.Count + 1, WarningInfo = "Pack CH OC Level3 Protection" });
                    }
                }
                else
                {
                    if (IsChineseUI)
                    {
                        PackCollection[message[2] - 1].BatteryGroupCollection[0].CoslightWarningCollection.Add(new WarningItem { WarningItemID = PackCollection[message[2] - 1].BatteryGroupCollection[0].CoslightWarningCollection.Count + 1, WarningInfo = "电池组充电过流三级告警" });
                    }
                    else
                    {
                        PackCollection[message[2] - 1].BatteryGroupCollection[0].CoslightWarningCollection.Add(new WarningItem { WarningItemID = PackCollection[message[2] - 1].BatteryGroupCollection[0].CoslightWarningCollection.Count + 1, WarningInfo = "Pack CH OC Level3 Warning" });
                    }
                }

            }

            if (PackCollection[message[2] - 1].PackDCH_OC_Level2_Protection != WarningType.正常 && PackCollection[message[2] - 1].PackDCH_OC_Level2_Protection != WarningType.OK)
            {
                if (IsChineseUI)
                {
                    PackCollection[message[2] - 1].BatteryGroupCollection[0].CoslightWarningCollection.Add(new WarningItem { WarningItemID = PackCollection[message[2] - 1].BatteryGroupCollection[0].CoslightWarningCollection.Count + 1, WarningInfo = "电池组放电过流二级保护" });
                }
                else
                {
                    PackCollection[message[2] - 1].BatteryGroupCollection[0].CoslightWarningCollection.Add(new WarningItem { WarningItemID = PackCollection[message[2] - 1].BatteryGroupCollection[0].CoslightWarningCollection.Count + 1, WarningInfo = "Pack DCH OC Level2 Protection" });
                }
            }

            if (PackCollection[message[2] - 1].SOCLowWarning != WarningType.正常 && PackCollection[message[2] - 1].SOCLowWarning != WarningType.OK)
            {
                if (IsChineseUI)
                {
                    PackCollection[message[2] - 1].BatteryGroupCollection[0].CoslightWarningCollection.Add(new WarningItem { WarningItemID = PackCollection[message[2] - 1].BatteryGroupCollection[0].CoslightWarningCollection.Count + 1, WarningInfo = "SOC低告警" });
                }
                else
                {
                    PackCollection[message[2] - 1].BatteryGroupCollection[0].CoslightWarningCollection.Add(new WarningItem { WarningItemID = PackCollection[message[2] - 1].BatteryGroupCollection[0].CoslightWarningCollection.Count + 1, WarningInfo = "SOC Low Warning" });
                }
            }

            if (PackCollection[message[2] - 1].SOCLowProtection != WarningType.正常 && PackCollection[message[2] - 1].SOCLowProtection != WarningType.OK)
            {
                if (IsChineseUI)
                {
                    PackCollection[message[2] - 1].BatteryGroupCollection[0].CoslightWarningCollection.Add(new WarningItem { WarningItemID = PackCollection[message[2] - 1].BatteryGroupCollection[0].CoslightWarningCollection.Count + 1, WarningInfo = "SOC低保护" });
                }
                else
                {
                    PackCollection[message[2] - 1].BatteryGroupCollection[0].CoslightWarningCollection.Add(new WarningItem { WarningItemID = PackCollection[message[2] - 1].BatteryGroupCollection[0].CoslightWarningCollection.Count + 1, WarningInfo = "SOC Low Protection" });
                }
            }

            if (PackCollection[message[2] - 1].PrechargeFailure != WarningType1.正常 && PackCollection[message[2] - 1].PrechargeFailure != WarningType1.OK)
            {
                if (IsChineseUI)
                {
                    PackCollection[message[2] - 1].BatteryGroupCollection[0].CoslightWarningCollection.Add(new WarningItem { WarningItemID = PackCollection[message[2] - 1].BatteryGroupCollection[0].CoslightWarningCollection.Count + 1, WarningInfo = "预放电失败告警" });
                }
                else
                {
                    PackCollection[message[2] - 1].BatteryGroupCollection[0].CoslightWarningCollection.Add(new WarningItem { WarningItemID = PackCollection[message[2] - 1].BatteryGroupCollection[0].CoslightWarningCollection.Count + 1, WarningInfo = "PreDischarge Failure Warning" });
                }
            }

            if (PackCollection[message[2] - 1].PoleBackward != WarningType.正常 && PackCollection[message[2] - 1].PoleBackward != WarningType.OK)
            {
                if (IsChineseUI)
                {
                    PackCollection[message[2] - 1].BatteryGroupCollection[0].CoslightWarningCollection.Add(new WarningItem { WarningItemID = PackCollection[message[2] - 1].BatteryGroupCollection[0].CoslightWarningCollection.Count + 1, WarningInfo = "电池组极性反接告警" });
                }
                else
                {
                    PackCollection[message[2] - 1].BatteryGroupCollection[0].CoslightWarningCollection.Add(new WarningItem { WarningItemID = PackCollection[message[2] - 1].BatteryGroupCollection[0].CoslightWarningCollection.Count + 1, WarningInfo = "Pack Pole Backward Warning" });
                }
            }

            if (PackCollection[message[2] - 1].VoltageSampleFailure != WarningType.正常 && PackCollection[message[2] - 1].VoltageSampleFailure != WarningType.OK)//防盗告警20200619
            {
                if (IsChineseUI)
                {
                    PackCollection[message[2] - 1].BatteryGroupCollection[0].CoslightWarningCollection.Add(new WarningItem { WarningItemID = PackCollection[message[2] - 1].BatteryGroupCollection[0].CoslightWarningCollection.Count + 1, WarningInfo = "防盗告警" });
                }
                else
                {
                    PackCollection[message[2] - 1].BatteryGroupCollection[0].CoslightWarningCollection.Add(new WarningItem { WarningItemID = PackCollection[message[2] - 1].BatteryGroupCollection[0].CoslightWarningCollection.Count + 1, WarningInfo = "AntiThief" });
                }
            }

            if (PackCollection[message[2] - 1].CellFailure != WarningType.正常 && PackCollection[message[2] - 1].CellFailure != WarningType.OK)
            {
                if (IsChineseUI)
                {
                    //PackCollection[message[2] - 1].BatteryGroupCollection[0].CoslightWarningCollection.Add(new WarningItem { WarningItemID = PackCollection[message[2] - 1].BatteryGroupCollection[0].CoslightWarningCollection.Count + 1, WarningInfo = "电芯失效告警" });
                    PackCollection[message[2] - 1].BatteryGroupCollection[0].CoslightWarningCollection.Add(new WarningItem { WarningItemID = PackCollection[message[2] - 1].BatteryGroupCollection[0].CoslightWarningCollection.Count + 1, WarningInfo = "电池组失效告警" });
                }
                else
                {
                    PackCollection[message[2] - 1].BatteryGroupCollection[0].CoslightWarningCollection.Add(new WarningItem { WarningItemID = PackCollection[message[2] - 1].BatteryGroupCollection[0].CoslightWarningCollection.Count + 1, WarningInfo = "Cell Failure Warning" });
                }
            }

            if (SerialPortCommunicator.ManufacturerName == "ZTE-C" && SerialPortCommunicator.ZTE_Type != "FB150C")//20200528
            {
                if (PackCollection[message[2] - 1].CellConsistencyProtection != WarningType.正常 && PackCollection[message[2] - 1].CellConsistencyProtection != WarningType.OK)
                {
                    if (IsChineseUI)
                    {
                        PackCollection[message[2] - 1].BatteryGroupCollection[0].CoslightWarningCollection.Add(new WarningItem { WarningItemID = PackCollection[message[2] - 1].BatteryGroupCollection[0].CoslightWarningCollection.Count + 1, WarningInfo = "电芯一致性差保护" });
                    }
                    else
                    {
                        PackCollection[message[2] - 1].BatteryGroupCollection[0].CoslightWarningCollection.Add(new WarningItem { WarningItemID = PackCollection[message[2] - 1].BatteryGroupCollection[0].CoslightWarningCollection.Count + 1, WarningInfo = "Cell Consistency Protection" });
                    }
                }

                if (PackCollection[message[2] - 1].MOSWarningStatus != MOSWarningType.正常 && PackCollection[message[2] - 1].MOSWarningStatus != MOSWarningType.OK)
                {
                    PackCollection[message[2] - 1].BatteryGroupCollection[0].CoslightWarningCollection.Add(new WarningItem { WarningItemID = PackCollection[message[2] - 1].BatteryGroupCollection[0].CoslightWarningCollection.Count + 1, WarningInfo = PackCollection[message[2] - 1].MOSWarningStatus.ToString() });
                }

                if (PackCollection[message[2] - 1].SOHLowStatus != WarningProtectType.正常 && PackCollection[message[2] - 1].SOHLowStatus != WarningProtectType.OK)
                {
                    PackCollection[message[2] - 1].BatteryGroupCollection[0].CoslightWarningCollection.Add(new WarningItem { WarningItemID = PackCollection[message[2] - 1].BatteryGroupCollection[0].CoslightWarningCollection.Count + 1, WarningInfo = PackCollection[message[2] - 1].SOHLowStatus.ToString() });
                }
            }
        }
        //系统参数 设定遥调量信息命令 
        private void ParseMessage_SystemParameter_DPC(byte[] message)
        {
            SystemParameterCollection.Clear();//清空集合内的所有 SystemParameter 实例
            SystemParameter_1_Collection.Clear();

            int offset = 8;
            for (int i = 0; i < Math.Min(xlsFileControler.dataParameter.Count, message.Length - 5 / 2); i++)
            {
                try
                {
                    //不包含数量
                    if (!xlsFileControler.dataParameter[i].Name_Chinese.Contains("数量"))
                    {
                        float[] Value_F = ReturnSavedAndSetValue(xlsFileControler.dataParameter[i].Byte, message, offset);

                        if (xlsFileControler.dataParameter[i].Name_Chinese == "SOC")
                        {
                            SystemParameter_2_Collection.Clear();

                            SystemParameter_2_Collection.Add(ViewModelSource.Create(() => new SystemParameter
                            {
                                SystemParameterID = SystemParameter_1_Collection.Count + 1,
                                CommandType = xlsFileControler.dataParameter[i].ID + 1,
                                Description = xlsFileControler.dataParameter[i].Name_Chinese,
                                Description_US = xlsFileControler.dataParameter[i].Name_English,
                                DefaultValue = xlsFileControler.dataParameter[i].DefaultValue,
                                SavedValue = Value_F[0] / xlsFileControler.dataParameter[i].coef,
                                SetValue = Value_F[1] / xlsFileControler.dataParameter[i].coef,
                                Coef = xlsFileControler.dataParameter[i].coef,
                                ParameterScope = xlsFileControler.dataParameter[i].SettingRange,
                                MinValue = xlsFileControler.dataParameter[i].SettingRangeMin,
                                MaxValue = xlsFileControler.dataParameter[i].SettingRangeMax,
                                Byte = xlsFileControler.dataParameter[i].Byte
                            }));

                            SOC = SystemParameter_2_Collection[0].SavedValue.ToString();
                        }
                        else if (xlsFileControler.dataParameter[i].Name_Chinese.Contains("保护板型号"))
                        {
                            SystemParameter_1_Collection.Add(ViewModelSource.Create(() => new SystemParameter
                            {
                                SystemParameterID = SystemParameter_1_Collection.Count + 1,
                                CommandType = xlsFileControler.dataParameter[i].ID + 1,
                                Description = xlsFileControler.dataParameter[i].Name_Chinese,
                                Description_US = xlsFileControler.dataParameter[i].Name_English,
                                DefaultValue = xlsFileControler.dataParameter[i].DefaultValue,
                                strSavedValue = Encoding.ASCII.GetString(message, offset, 10).Trim(),
                                strSetValue = Encoding.ASCII.GetString(message, offset, 10).Trim(),
                                Coef = xlsFileControler.dataParameter[i].coef,
                                ParameterScope = xlsFileControler.dataParameter[i].SettingRange,
                                MinValue = xlsFileControler.dataParameter[i].SettingRangeMin,
                                MaxValue = xlsFileControler.dataParameter[i].SettingRangeMax,
                                Byte = xlsFileControler.dataParameter[i].Byte
                            }));
                        }
                        else if (xlsFileControler.dataParameter[i].Name_Chinese.Contains("只读"))
                        {
                            SystemParameter_1_Collection.Add(ViewModelSource.Create(() => new SystemParameter
                            {
                                SystemParameterID = SystemParameter_1_Collection.Count + 1,
                                Description = xlsFileControler.dataParameter[i].Name_Chinese,
                                Description_US = xlsFileControler.dataParameter[i].Name_English,
                                strSavedValue = BitConverter.ToInt16(new byte[] { message[offset + 1], message[offset] }, 0).ToString()
                            }));

                            if (!xlsFileControler.dataParameter[i].Name_Chinese.Contains("数参数"))
                            {
                                SystemParameter_1_Collection[SystemParameter_1_Collection.Count - 1].Description = xlsFileControler.dataParameter[i].Name_Chinese + "_" + xlsFileControler.dataParameter[i].Unit;
                                SystemParameter_1_Collection[SystemParameter_1_Collection.Count - 1].Description_US = xlsFileControler.dataParameter[i].Name_English + "_" + xlsFileControler.dataParameter[i].Unit;
                            }
                        }
                        else if (xlsFileControler.dataParameter[i].Name_Chinese.Contains("开关"))
                        {
                            EnableControlCollection.Add(ViewModelSource.Create(() => new SpecialParameter
                            {
                                SpecialParameterID = EnableControlCollection.Count + 1,
                                Description = xlsFileControler.dataParameter[i].Name_Chinese,
                                Description_US = xlsFileControler.dataParameter[i].Name_English
                            }));
                            creatSwitchParameter(message, offset, xlsFileControler.dataParameter[i]);

                            IsLoadingSpecialSystemParameter = false;
                        }
                        else
                        {
                            SystemParameterCollection.Add(ViewModelSource.Create(() => new SystemParameter
                            {
                                SystemParameterID = SystemParameterCollection.Count + 1,
                                CommandType = xlsFileControler.dataParameter[i].ID + 1,
                                Description = xlsFileControler.dataParameter[i].Name_Chinese,
                                Description_US = xlsFileControler.dataParameter[i].Name_English,
                                DefaultValue = xlsFileControler.dataParameter[i].DefaultValue,
                                SavedValue = Value_F[0] / xlsFileControler.dataParameter[i].coef,
                                SetValue = Value_F[1] / xlsFileControler.dataParameter[i].coef,
                                Coef = xlsFileControler.dataParameter[i].coef,
                                ParameterScope = xlsFileControler.dataParameter[i].SettingRange,
                                MinValue = xlsFileControler.dataParameter[i].SettingRangeMin,
                                MaxValue = xlsFileControler.dataParameter[i].SettingRangeMax,
                                Byte = xlsFileControler.dataParameter[i].Byte
                            }));

                            if (!xlsFileControler.dataParameter[i].Name_Chinese.Contains("数参数"))
                            {
                                SystemParameterCollection[SystemParameterCollection.Count - 1].Description = xlsFileControler.dataParameter[i].Name_Chinese + "_" + xlsFileControler.dataParameter[i].Unit;
                                SystemParameterCollection[SystemParameterCollection.Count - 1].Description_US = xlsFileControler.dataParameter[i].Name_English + "_" + xlsFileControler.dataParameter[i].Unit;
                            }
                        }

                    }
                }
                catch { }
                offset += xlsFileControler.dataParameter[i].Byte;
            }

            IsLoadingSystemParameter = false;
        }
        private void ParseMessage_SystemParameter(byte[] message)
        {
            int packAddress = message[2] - 1;
            int offset = 7;

            if (ProtocalProvider.BatteryGroupCount == 0xFF)
            {
                offset++;
            }

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0x80,
                CID2 = 0x49,
                Description = "单体过压告警阈值(V)",
                Description_US = "Cell OV Warning(V)",
                DefaultValue = 3.8F,
                SavedValue = BitConverter.ToUInt16(new byte[] { message[10], message[9] }, 0) / 100F,
                SetValue = BitConverter.ToUInt16(new byte[] { message[10], message[9] }, 0) / 100F,
                Coef = 100
            }));

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0x81,
                CID2 = 0x49,
                Description = "单体欠压告警阈值(V)",
                Description_US = "Cell UV Warning(V)",
                DefaultValue = 2.8F,
                SavedValue = BitConverter.ToUInt16(new byte[] { message[12], message[11] }, 0) / 100F,
                SetValue = BitConverter.ToUInt16(new byte[] { message[12], message[11] }, 0) / 100F,
                Coef = 100
            }));

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0x82,
                CID2 = 0x49,
                Description = "单体过压保护阈值(V)",
                Description_US = "Cell OV Protection(V)",
                DefaultValue = 3.85F,
                SavedValue = BitConverter.ToUInt16(new byte[] { message[14], message[13] }, 0) / 100F,
                SetValue = BitConverter.ToUInt16(new byte[] { message[14], message[13] }, 0) / 100F,
                Coef = 100
            }));

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0x83,
                CID2 = 0x49,
                Description = "单体欠压保护阈值(V)",
                Description_US = "Cell UV Protection(V)",
                DefaultValue = 2.55F,
                SavedValue = BitConverter.ToUInt16(new byte[] { message[16], message[15] }, 0) / 100F,
                SetValue = BitConverter.ToUInt16(new byte[] { message[16], message[15] }, 0) / 100F,
                Coef = 100
            }));

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0x84,
                CID2 = 0x49,
                Description = "电池组过压告警阈值(V)",
                Description_US = "Pack OV Warning(V)",
                DefaultValue = 55F,
                SavedValue = BitConverter.ToUInt16(new byte[] { message[18], message[17] }, 0) / 10F,
                SetValue = BitConverter.ToUInt16(new byte[] { message[18], message[17] }, 0) / 10F,
                Coef = 10
            }));

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0x85,
                CID2 = 0x49,
                Description = "电池组欠压告警阈值(V)",
                Description_US = "Pack UV Warning(V)",
                DefaultValue = 44.0F,
                SavedValue = BitConverter.ToUInt16(new byte[] { message[20], message[19] }, 0) / 10F,
                SetValue = BitConverter.ToUInt16(new byte[] { message[20], message[19] }, 0) / 10F,
                Coef = 10
            }));

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0x86,
                CID2 = 0x49,
                Description = "电池组过压保护阈值(V)",
                Description_US = "Pack OV Protection(V)",
                DefaultValue = 55.5F,
                SavedValue = BitConverter.ToUInt16(new byte[] { message[22], message[21] }, 0) / 10F,
                SetValue = BitConverter.ToUInt16(new byte[] { message[22], message[21] }, 0) / 10F,
                Coef = 10
            }));

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0xC4,
                CID2 = 0x49,
                Description = "电池组欠压保护阈值(V)",
                Description_US = "Pack UV Protection(V)",
                DefaultValue = 42.0F,
                SavedValue = BitConverter.ToUInt16(new byte[] { message[24], message[23] }, 0) / 10F,
                SetValue = BitConverter.ToUInt16(new byte[] { message[24], message[23] }, 0) / 10F,
                Coef = 10
            }));

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0xC5,
                CID2 = 0x49,
                Description = "充电温度高告警阈值(℃)",
                Description_US = "CH OT Warning(℃)",
                DefaultValue = 50.0F,
                SavedValue = BitConverter.ToInt16(new byte[] { message[26], message[25] }, 0) / 1F,
                SetValue = BitConverter.ToInt16(new byte[] { message[26], message[25] }, 0) / 1F,
                Coef = 1
            }));

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0xC6,
                CID2 = 0x49,
                Description = "充电温度低告警阈值(℃)",
                Description_US = "CH UT Warning(℃)",
                DefaultValue = 10F,
                SavedValue = BitConverter.ToInt16(new byte[] { message[28], message[27] }, 0) / 1F,
                SetValue = BitConverter.ToInt16(new byte[] { message[28], message[27] }, 0) / 1F,
                Coef = 1
            }));

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0xC7,
                CID2 = 0x49,
                Description = "充电温度高保护阈值(℃)",
                Description_US = "CH OT Protection(℃)",
                DefaultValue = 60.0F,
                SavedValue = BitConverter.ToInt16(new byte[] { message[30], message[29] }, 0) / 1F,
                SetValue = BitConverter.ToInt16(new byte[] { message[30], message[29] }, 0) / 1F,
                Coef = 1
            }));

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0xC8,
                CID2 = 0x49,
                Description = "充电温度低保护阈值(℃)",
                Description_US = "CH UT Protection(℃)",
                DefaultValue = 0F,
                SavedValue = BitConverter.ToInt16(new byte[] { message[32], message[31] }, 0) / 1F,
                SetValue = BitConverter.ToInt16(new byte[] { message[32], message[31] }, 0) / 1F,
                Coef = 1
            }));

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0xC9,
                CID2 = 0x49,
                Description = "放电温度高告警阈值(℃)",
                Description_US = "DCH OT Warning(℃)",
                DefaultValue = 55F,
                SavedValue = BitConverter.ToInt16(new byte[] { message[34], message[33] }, 0) / 1F,
                SetValue = BitConverter.ToInt16(new byte[] { message[34], message[33] }, 0) / 1F,
                Coef = 1
            }));

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0xCA,
                CID2 = 0x49,
                Description = "放电温度低告警阈值(℃)",
                Description_US = "DCH UT Warning(℃)",
                DefaultValue = -15F,
                SavedValue = BitConverter.ToInt16(new byte[] { message[36], message[35] }, 0) / 1F,
                SetValue = BitConverter.ToInt16(new byte[] { message[36], message[35] }, 0) / 1F,
                Coef = 1
            }));

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0xCB,
                CID2 = 0x49,
                Description = "放电温度高保护阈值(℃)",
                Description_US = "DCH OT Protection(℃)",
                DefaultValue = 65F,
                SavedValue = BitConverter.ToInt16(new byte[] { message[38], message[37] }, 0) / 1F,
                SetValue = BitConverter.ToInt16(new byte[] { message[38], message[37] }, 0) / 1F,
                Coef = 1
            }));

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0xCC,
                CID2 = 0x49,
                Description = "放电温度低保护阈值(℃)",
                Description_US = "DCH UT Protection(℃)",
                DefaultValue = -20F,
                SavedValue = BitConverter.ToInt16(new byte[] { message[40], message[39] }, 0) / 1F,
                SetValue = BitConverter.ToInt16(new byte[] { message[40], message[39] }, 0) / 1F,
                Coef = 1
            }));

            if (SerialPortCommunicator.ZTE_Type == "FB150C")
            {
                SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
                {
                    SystemParameterID = SystemParameterCollection.Count + 1,
                    CommandType = 0xCD,
                    CID2 = 0x49,
                    Description = "充电过流告警阈值(A)",
                    Description_US = "CH OC Warning(A)",
                    DefaultValue = 105F,//20200422
                    //DefaultValue = 90F,
                    SavedValue = BitConverter.ToUInt16(new byte[] { message[42], message[41] }, 0) * CellCapacity / 1000F,
                    SetValue = BitConverter.ToUInt16(new byte[] { message[42], message[41] }, 0) * CellCapacity / 1000F,
                    Coef = 1000
                }));

                SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
                {
                    SystemParameterID = SystemParameterCollection.Count + 1,
                    CommandType = 0xCE,
                    CID2 = 0x49,
                    Description = "充电过流保护阈值(A)",
                    Description_US = "CH OC Protection (A)",
                    DefaultValue = 110F,//20200422
                    //DefaultValue = 105F,
                    SavedValue = BitConverter.ToUInt16(new byte[] { message[44], message[43] }, 0) * CellCapacity / 1000F,
                    SetValue = BitConverter.ToUInt16(new byte[] { message[44], message[43] }, 0) * CellCapacity / 1000F,
                    Coef = 1000
                }));

            }
            else
            {
                SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
                {
                    SystemParameterID = SystemParameterCollection.Count + 1,
                    CommandType = 0xCD,
                    CID2 = 0x49,
                    Description = "充电过流告警阈值(A)",
                    Description_US = "CH OC Warning(A)",
                    //DefaultValue = 105F,
                    DefaultValue = 90F,//20200418
                    SavedValue = BitConverter.ToUInt16(new byte[] { message[42], message[41] }, 0) * CellCapacity / 1000F,
                    SetValue = BitConverter.ToUInt16(new byte[] { message[42], message[41] }, 0) * CellCapacity / 1000F,
                    Coef = 1000
                }));

                SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
                {
                    SystemParameterID = SystemParameterCollection.Count + 1,
                    CommandType = 0xCE,
                    CID2 = 0x49,
                    Description = "充电过流保护阈值(A)",
                    Description_US = "CH OC Protection (A)",
                    //DefaultValue = 110F,
                    DefaultValue = 105F,//20200418
                    SavedValue = BitConverter.ToUInt16(new byte[] { message[44], message[43] }, 0) * CellCapacity / 1000F,
                    SetValue = BitConverter.ToUInt16(new byte[] { message[44], message[43] }, 0) * CellCapacity / 1000F,
                    Coef = 1000
                }));
            }

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0xCF,
                CID2 = 0x49,
                Description = "放电过流保护阈值(A)",
                Description_US = "DCH OC Protection(A)",
                DefaultValue = 105F,
                SavedValue = BitConverter.ToUInt16(new byte[] { message[46], message[45] }, 0) * CellCapacity / 1000F,
                SetValue = BitConverter.ToUInt16(new byte[] { message[46], message[45] }, 0) * CellCapacity / 1000F,
                Coef = 1000
            }));

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0xD0,
                CID2 = 0x49,
                Description = "放电过流告警阈值(A)",
                Description_US = "DCH OC Warning(A)",
                DefaultValue = 90F,
                SavedValue = BitConverter.ToUInt16(new byte[] { message[48], message[47] }, 0) * CellCapacity / 1000F,
                SetValue = BitConverter.ToUInt16(new byte[] { message[48], message[47] }, 0) * CellCapacity / 1000F,
                Coef = 1000
            }));

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0xD1,
                CID2 = 0x49,
                Description = "电池组欠压保护恢复阈值(V)",
                Description_US = "Pack UV Protection Restore(V)",
                DefaultValue = 48F,
                SavedValue = BitConverter.ToUInt16(new byte[] { message[51], message[50] }, 0) / 100F,
                SetValue = BitConverter.ToUInt16(new byte[] { message[51], message[50] }, 0) / 100F,
                Coef = 100
            }));

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0xD2,
                CID2 = 0x49,
                Description = "单体过压保护恢复阈值(V)",
                Description_US = "Cell OV Protection Restore(V)",
                DefaultValue = 3.6F,
                SavedValue = BitConverter.ToUInt16(new byte[] { message[53], message[52] }, 0) / 100F,
                SetValue = BitConverter.ToUInt16(new byte[] { message[53], message[52] }, 0) / 100F,
                Coef = 100
            }));

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0xD3,
                CID2 = 0x49,
                Description = "单体欠压保护恢复阈值(V)",
                Description_US = "Cell UV Protection Restore(V)",
                DefaultValue = 3.1F,
                SavedValue = BitConverter.ToUInt16(new byte[] { message[55], message[54] }, 0) / 100F,
                SetValue = BitConverter.ToUInt16(new byte[] { message[55], message[54] }, 0) / 100F,
                Coef = 100
            }));

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0xD4,
                CID2 = 0x49,
                Description = "充电过温保护恢复阈值(℃)",
                Description_US = "CH OT Protection(℃)",
                DefaultValue = 55F,
                SavedValue = BitConverter.ToInt16(new byte[] { message[57], message[56] }, 0) / 100F,
                SetValue = BitConverter.ToInt16(new byte[] { message[57], message[56] }, 0) / 100F,
                Coef = 100
            }));

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0xD5,
                CID2 = 0x49,
                Description = "放电过温保护恢复阈值(℃)",
                Description_US = "CH OT Protection(℃)",
                DefaultValue = 60F,
                SavedValue = BitConverter.ToInt16(new byte[] { message[59], message[58] }, 0) / 100F,
                SetValue = BitConverter.ToInt16(new byte[] { message[59], message[58] }, 0) / 100F,
                Coef = 100
            }));

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0xD6,
                CID2 = 0x49,
                Description = "充电低温保护恢复阈值(℃)",
                Description_US = "CH UT Protection(℃)",
                DefaultValue = 3F,
                SavedValue = BitConverter.ToInt16(new byte[] { message[61], message[60] }, 0) / 100F,
                SetValue = BitConverter.ToInt16(new byte[] { message[61], message[60] }, 0) / 100F,
                Coef = 100
            }));

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0xD7,
                CID2 = 0x49,
                Description = "放电低温保护恢复阈值(℃)",
                Description_US = "DCH UT Protection(℃)",
                DefaultValue = -17F,
                SavedValue = BitConverter.ToInt16(new byte[] { message[63], message[62] }, 0) / 100F,
                SetValue = BitConverter.ToInt16(new byte[] { message[63], message[62] }, 0) / 100F,
                Coef = 100
            }));

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0xD8,
                CID2 = 0x49,
                Description = "单体落后告警电压差阈值(V)",
                Description_US = "Cell Behind Volt Diff Warning(V)",
                DefaultValue = 0.5F,
                SavedValue = BitConverter.ToUInt16(new byte[] { message[65], message[64] }, 0) / 100F,
                SetValue = BitConverter.ToUInt16(new byte[] { message[65], message[64] }, 0) / 100F,
                Coef = 100
            }));

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0xD9,
                CID2 = 0x49,
                Description = "环境温度高告警阈值(℃)",
                Description_US = "Ambient Temperature High Warning",
                DefaultValue = 60.0F,
                SavedValue = BitConverter.ToInt16(new byte[] { message[67], message[66] }, 0) / 1F,
                SetValue = BitConverter.ToInt16(new byte[] { message[67], message[66] }, 0) / 1F,
                Coef = 1
            }));

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0xDA,
                CID2 = 0x49,
                Description = "环境温度低告警阈值(℃)",
                Description_US = "Ambient Temperature Low Warning",
                DefaultValue = 0F,
                SavedValue = BitConverter.ToInt16(new byte[] { message[69], message[68] }, 0) / 1F,
                SetValue = BitConverter.ToInt16(new byte[] { message[69], message[68] }, 0) / 1F,
                Coef = 1
            }));
            if (SerialPortCommunicator.ManufacturerName == "ZTE-C" && SerialPortCommunicator.ZTE_Type != "FB100C5")
            {
                SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
                {
                    SystemParameterID = SystemParameterCollection.Count + 1,
                    CommandType = 0xDB,
                    CID2 = 0x49,
                    Description = "防盗开启时间(min)",
                    Description_US = "Anti-Theft Start Time(min)",
                    DefaultValue = 480F,
                    SavedValue = BitConverter.ToUInt16(new byte[] { message[71], message[70] }, 0) / 1F,
                    SetValue = BitConverter.ToUInt16(new byte[] { message[71], message[70] }, 0) / 1F,
                    Coef = 1
                }));
            }

            IsLoadingSystemParameter = false;
        }
        private void ParseMessage_SystemParameter_Coslight(byte[] message)
        {

            int packAddress = message[2] - 1;

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0x80,
                CID2 = 0x8C,
                Description = "单体过压告警恢复阈值(V)",
                Description_US = "Cell OV Warning Restore(V)",
                DefaultValue = 3.6F,
                SavedValue = BitConverter.ToUInt16(new byte[] { message[8], message[7] }, 0) / 1000F,
                SetValue = BitConverter.ToUInt16(new byte[] { message[8], message[7] }, 0) / 1000F,
                Coef = 1000
            }));

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0x81,
                CID2 = 0x8C,
                Description = "单体欠压告警恢复阈值(V)",
                Description_US = "Cell UV Warning Restore(V)",
                DefaultValue = 3.1F,
                SavedValue = BitConverter.ToUInt16(new byte[] { message[10], message[9] }, 0) / 1000F,
                SetValue = BitConverter.ToUInt16(new byte[] { message[10], message[9] }, 0) / 1000F,
                Coef = 1000
            }));

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0x84,
                CID2 = 0x8C,
                Description = "电池组过压告警恢复阈值(V)",
                Description_US = "Pack OV Warning Restore(V)",
                DefaultValue = 52.5F,
                SavedValue = BitConverter.ToUInt16(new byte[] { message[12], message[11] }, 0) / 100F,
                SetValue = BitConverter.ToUInt16(new byte[] { message[12], message[11] }, 0) / 100F,
                Coef = 100
            }));

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0x85,
                CID2 = 0x8C,
                Description = "电池组欠压告警恢复阈值(V)",
                Description_US = "Pack UV Warning Restore(V)",
                DefaultValue = 47.0F,
                SavedValue = BitConverter.ToUInt16(new byte[] { message[14], message[13] }, 0) / 100F,
                SetValue = BitConverter.ToUInt16(new byte[] { message[14], message[13] }, 0) / 100F,
                Coef = 100
            }));

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0x86,
                CID2 = 0x8C,
                Description = "电池组过压保护恢复阈值(V)",
                Description_US = "Pack OV Protection Restore(V)",
                DefaultValue = 50.0F,
                SavedValue = BitConverter.ToUInt16(new byte[] { message[16], message[15] }, 0) / 100F,
                SetValue = BitConverter.ToUInt16(new byte[] { message[16], message[15] }, 0) / 100F,
                Coef = 100
            }));

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0xC5,
                CID2 = 0x8C,
                Description = "充电温度高告警恢复阈值(℃)",
                Description_US = "CH OT Warning Restore(℃)",
                DefaultValue = 45F,
                SavedValue = BitConverter.ToInt16(new byte[] { message[18], message[17] }, 0) / 100F,
                SetValue = BitConverter.ToInt16(new byte[] { message[18], message[17] }, 0) / 100F,
                Coef = 100
            }));

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0xC6,
                CID2 = 0x8C,
                Description = "充电温度低告警恢复阈值(℃)",
                Description_US = "CH UT Warning Restore(℃)",
                DefaultValue = 12F,
                SavedValue = BitConverter.ToInt16(new byte[] { message[20], message[19] }, 0) / 100F,
                SetValue = BitConverter.ToInt16(new byte[] { message[20], message[19] }, 0) / 100F,
                Coef = 100
            }));

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0xC9,
                CID2 = 0x8C,
                Description = "放电温度高告警恢复阈值(℃)",
                Description_US = "DCH OT Warning Restore(℃)",
                DefaultValue = 53F,
                SavedValue = BitConverter.ToInt16(new byte[] { message[22], message[21] }, 0) / 100F,
                SetValue = BitConverter.ToInt16(new byte[] { message[22], message[21] }, 0) / 100F,
                Coef = 100
            }));

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0xCA,
                CID2 = 0x8C,
                Description = "放电温度低告警恢复阈值(℃)",
                Description_US = "DCH UT Warning Restore(℃)",
                DefaultValue = -13F,
                SavedValue = BitConverter.ToInt16(new byte[] { message[24], message[23] }, 0) / 100F,
                SetValue = BitConverter.ToInt16(new byte[] { message[24], message[23] }, 0) / 100F,
                Coef = 100
            }));

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0xD0,
                CID2 = 0x8C,
                Description = "放电过流告警恢复阈值(A)",
                Description_US = "DCH OC Warning Restore(A)",
                DefaultValue = 80F,
                SavedValue = BitConverter.ToUInt16(new byte[] { message[28], message[27] }, 0) / 100F,
                SetValue = BitConverter.ToUInt16(new byte[] { message[28], message[27] }, 0) / 100F,
                Coef = 100
            }));

            if (SerialPortCommunicator.ZTE_Type == "FB150C")
            {
                SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
                {
                    SystemParameterID = SystemParameterCollection.Count + 1,
                    CommandType = 0xCD,
                    CID2 = 0x8C,
                    Description = "充电过流告警恢复阈值(A)",
                    Description_US = "CH OC Warning Restore(A)",
                    DefaultValue = 100F,//20200422
                    //DefaultValue = 80F,
                    SavedValue = BitConverter.ToUInt16(new byte[] { message[26], message[25] }, 0) / 100F,
                    SetValue = BitConverter.ToUInt16(new byte[] { message[26], message[25] }, 0) / 100F,
                    Coef = 100
                }));
            }
            else
            {
                SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
                {
                    SystemParameterID = SystemParameterCollection.Count + 1,
                    CommandType = 0xCD,
                    CID2 = 0x8C,
                    Description = "充电过流告警恢复阈值(A)",
                    Description_US = "CH OC Warning Restore(A)",
                    //DefaultValue = 100F,
                    DefaultValue = 80F,//20200418
                    SavedValue = BitConverter.ToUInt16(new byte[] { message[26], message[25] }, 0) / 100F,
                    SetValue = BitConverter.ToUInt16(new byte[] { message[26], message[25] }, 0) / 100F,
                    Coef = 100
                }));
            }

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0xD8,
                CID2 = 0x8C,
                Description = "单体落后告警电压差恢复阈值(V)",
                Description_US = "Cell Behind Warning Volt Diff Restore(V)",
                DefaultValue = 0.3F,
                SavedValue = BitConverter.ToUInt16(new byte[] { message[30], message[29] }, 0) / 100F,
                SetValue = BitConverter.ToUInt16(new byte[] { message[30], message[29] }, 0) / 100F,
                Coef = 100
            }));

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0xD9,
                CID2 = 0x8C,
                Description = "环境温度高告警恢复阈值(℃)",
                Description_US = "Ambient OT Warning Restore(℃)",
                DefaultValue = 55F,
                SavedValue = BitConverter.ToInt16(new byte[] { message[32], message[31] }, 0) / 100F,
                SetValue = BitConverter.ToInt16(new byte[] { message[32], message[31] }, 0) / 100F,
                Coef = 100
            }));

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0xDA,
                CID2 = 0x8C,
                Description = "环境温度低告警恢复阈值(℃)",
                Description_US = "Ambient UT Warning Restore(℃)",
                DefaultValue = 5F,
                SavedValue = BitConverter.ToInt16(new byte[] { message[34], message[33] }, 0) / 100F,
                SetValue = BitConverter.ToInt16(new byte[] { message[34], message[33] }, 0) / 100F,
                Coef = 100
            }));

            SystemParameter_1_Collection.Clear();

            if (SerialPortCommunicator.ZTE_Type == "FB150C")
            {
                SystemParameter_1_Collection.Add(ViewModelSource.Create(() => new Models.SystemParameter
                {
                    SystemParameterID = SystemParameter_1_Collection.Count + 1,
                    CommandType = 0xDB,
                    CID2 = 0x8C,
                    Description = "额定容量(Ah)",
                    Description_US = "Rated Capacity(Ah)",
                    DefaultValue = 150F,//20200422
                    //DefaultValue = 100F,
                    SavedValue = BitConverter.ToUInt16(new byte[] { message[37], message[36] }, 0) / 100F,
                    SetValue = BitConverter.ToUInt16(new byte[] { message[37], message[36] }, 0) / 100F,
                    Coef = 100
                }));

                SystemParameter_1_Collection.Add(ViewModelSource.Create(() => new Models.SystemParameter
                {
                    SystemParameterID = SystemParameter_1_Collection.Count + 1,
                    CommandType = 0xE8,
                    CID2 = 0x8C,
                    Description = "数据存储时间间隔(S)",
                    Description_US = "Data Save Interval(S)",
                    DefaultValue = 1800F,//20200422
                    //DefaultValue = 180F,
                    SavedValue = BitConverter.ToUInt16(new byte[] { message[67], message[66] }, 0),
                    SetValue = BitConverter.ToUInt16(new byte[] { message[67], message[66] }, 0),
                    Coef = 1
                }));

                SystemParameter_1_Collection.Add(ViewModelSource.Create(() => new Models.SystemParameter
                {
                    SystemParameterID = SystemParameter_1_Collection.Count + 1,
                    CommandType = 0xE2,
                    CID2 = 0x8C,
                    Description = "充电截至电压(V)",
                    Description_US = "CH Terminal Voltage(V)",
                    DefaultValue = 52F,//20200422
                    //DefaultValue = 52.5F,
                    SavedValue = BitConverter.ToUInt16(new byte[] { message[57], message[56] }, 0) / 100F,
                    SetValue = BitConverter.ToUInt16(new byte[] { message[57], message[56] }, 0) / 100F,
                    Coef = 100
                }));
            }
            else
            {
                SystemParameter_1_Collection.Add(ViewModelSource.Create(() => new Models.SystemParameter
                {
                    SystemParameterID = SystemParameter_1_Collection.Count + 1,
                    CommandType = 0xDB,
                    CID2 = 0x8C,
                    Description = "额定容量(Ah)",
                    Description_US = "Rated Capacity(Ah)",
                    //DefaultValue = 150F,
                    DefaultValue = 100F,//20200418
                    SavedValue = BitConverter.ToUInt16(new byte[] { message[37], message[36] }, 0) / 100F,
                    SetValue = BitConverter.ToUInt16(new byte[] { message[37], message[36] }, 0) / 100F,
                    Coef = 100
                }));

                SystemParameter_1_Collection.Add(ViewModelSource.Create(() => new Models.SystemParameter
                {
                    SystemParameterID = SystemParameter_1_Collection.Count + 1,
                    CommandType = 0xE8,
                    CID2 = 0x8C,
                    Description = "数据存储时间间隔(S)",
                    Description_US = "Data Save Interval(S)",
                    //DefaultValue = 1800F,
                    DefaultValue = 180F,//20200418
                    SavedValue = BitConverter.ToUInt16(new byte[] { message[67], message[66] }, 0),
                    SetValue = BitConverter.ToUInt16(new byte[] { message[67], message[66] }, 0),
                    Coef = 1
                }));

                SystemParameter_1_Collection.Add(ViewModelSource.Create(() => new Models.SystemParameter
                {
                    SystemParameterID = SystemParameter_1_Collection.Count + 1,
                    CommandType = 0xE2,
                    CID2 = 0x8C,
                    Description = "充电截至电压(V)",
                    Description_US = "CH Terminal Voltage(V)",
                    //DefaultValue = 52F,
                    DefaultValue = 52.5F,//20200418
                    SavedValue = BitConverter.ToUInt16(new byte[] { message[57], message[56] }, 0) / 100F,
                    SetValue = BitConverter.ToUInt16(new byte[] { message[57], message[56] }, 0) / 100F,
                    Coef = 100
                }));
            }

            SystemParameter_2_Collection.Clear();

            SystemParameter_2_Collection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameter_1_Collection.Count + 1,
                CommandType = 0xDC,
                CID2 = 0x8C,
                Description = "累计充电容量(Ah)",
                Description_US = "Accumulative CH Capacity(Ah)",
                SavedValue = BitConverter.ToUInt32(new byte[] { message[41], message[40], message[39], message[38] }, 0) / 100F,
                SetValue = BitConverter.ToUInt32(new byte[] { message[41], message[40], message[39], message[38] }, 0) / 100F,
                Coef = 100
            }));

            AccCHCapacity = SystemParameter_2_Collection[0].SavedValue.ToString();

            SystemParameter_2_Collection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameter_1_Collection.Count + 1,
                CommandType = 0xDD,
                CID2 = 0x8C,
                Description = "累计放电容量(Ah)",
                Description_US = "Accumulative DCH Capacity(Ah)",
                SavedValue = BitConverter.ToInt32(new byte[] { message[45], message[44], message[43], message[42] }, 0) / 100F,
                SetValue = BitConverter.ToInt32(new byte[] { message[45], message[44], message[43], message[42] }, 0) / 100F,
                Coef = 100
            }));

            AccDCHCapacity = SystemParameter_2_Collection[1].SavedValue.ToString();

            SystemParameter_1_Collection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameter_1_Collection.Count + 1,
                CommandType = 0xDE,
                CID2 = 0x8C,
                Description = "电池组编号",
                Description_US = "Pack ID",
                DefaultValue = 1F,
                SavedValue = BitConverter.ToUInt32(new byte[] { message[49], message[48], message[47], message[46] }, 0),
                SetValue = BitConverter.ToUInt32(new byte[] { message[49], message[48], message[47], message[46] }, 0),
                Coef = 1
            }));

            SystemParameter_1_Collection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameter_1_Collection.Count + 1,
                CommandType = 0xDF,
                CID2 = 0x8C,
                Description = "均衡启动电压(V)",
                Description_US = "Balance Start Volt(V)",
                DefaultValue = 3.35F,
                SavedValue = BitConverter.ToUInt16(new byte[] { message[51], message[50] }, 0) / 1000F,
                SetValue = BitConverter.ToUInt16(new byte[] { message[51], message[50] }, 0) / 1000F,
                Coef = 1000
            }));

            SystemParameter_1_Collection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameter_1_Collection.Count + 1,
                CommandType = 0xE0,
                CID2 = 0x8C,
                Description = "均衡停止电压(V)",
                Description_US = "Balance Stop Volt(V)",
                DefaultValue = 3.2F,
                SavedValue = BitConverter.ToUInt16(new byte[] { message[53], message[52] }, 0) / 1000F,
                SetValue = BitConverter.ToUInt16(new byte[] { message[53], message[52] }, 0) / 1000F,
                Coef = 1000
            }));

            SystemParameter_1_Collection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameter_1_Collection.Count + 1,
                CommandType = 0xE1,
                CID2 = 0x8C,
                Description = "均衡启动压差(V)",
                Description_US = "Balance Start Volt Diff(V)",
                DefaultValue = 0.03F,
                SavedValue = BitConverter.ToUInt16(new byte[] { message[55], message[54] }, 0) / 1000F,
                SetValue = BitConverter.ToUInt16(new byte[] { message[55], message[54] }, 0) / 1000F,
                Coef = 1000
            }));

            SystemParameter_1_Collection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameter_1_Collection.Count + 1,
                CommandType = 0xE3,
                CID2 = 0x8C,
                Description = "充电截至电流(A)",
                Description_US = "CH Terminal Current(A)",
                DefaultValue = 3F,
                SavedValue = BitConverter.ToUInt16(new byte[] { message[59], message[58] }, 0) / 100F,
                SetValue = BitConverter.ToUInt16(new byte[] { message[59], message[58] }, 0) / 100F,
                Coef = 100
            }));

            if (IsManagerState)//20200722添加管理权限
            {
                SystemParameter_1_Collection.Add(ViewModelSource.Create(() => new Models.SystemParameter//20200722关闭设置
                {
                    SystemParameterID = SystemParameter_1_Collection.Count + 1,
                    CommandType = 0xE4,
                    CID2 = 0x8C,
                    Description = "放电循环次数",
                    Description_US = "DCH Cycle Times",
                    DefaultValue = 0F,
                    SavedValue = BitConverter.ToUInt16(new byte[] { message[61], message[60] }, 0),
                    SetValue = BitConverter.ToUInt16(new byte[] { message[61], message[60] }, 0),
                    Coef = 1
                }));
            }

            SystemParameter_1_Collection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameter_1_Collection.Count + 1,
                CommandType = 0xE5,
                CID2 = 0x8C,
                Description = "电池单体数量",
                Description_US = "Cell Count",
                DefaultValue = 15F,
                SavedValue = message[62],
                SetValue = message[62],
                Coef = 1
            }));

            SystemParameter_1_Collection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameter_1_Collection.Count + 1,
                CommandType = 0xE6,
                CID2 = 0x8C,
                Description = "电池温度数量",
                Description_US = "Cell Temp Count",
                DefaultValue = 4F,
                SavedValue = message[63],
                SetValue = message[63],
                Coef = 1
            }));

            SystemParameter_1_Collection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameter_1_Collection.Count + 1,
                CommandType = 0xE7,
                CID2 = 0x8C,
                Description = "电池进入低功耗时间(分钟)",
                Description_US = "Enter Low Comsumption Time(Min)",
                DefaultValue = 2880,
                SavedValue = BitConverter.ToUInt16(new byte[] { message[65], message[64] }, 0),
                SetValue = BitConverter.ToUInt16(new byte[] { message[65], message[64] }, 0),
                Coef = 1
            }));


            SystemParameter_1_Collection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameter_1_Collection.Count + 1,
                CommandType = 0xE9,
                CID2 = 0x8C,
                Description = "RTC年",
                Description_US = "RTC Year",
                DefaultValue = 2018F,
                SavedValue = BitConverter.ToUInt16(new byte[] { message[69], message[68] }, 0),
                SetValue = BitConverter.ToUInt16(new byte[] { message[69], message[68] }, 0),
                Coef = 1
            }));

            SystemParameter_1_Collection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameter_1_Collection.Count + 1,
                CommandType = 0xE9,
                CID2 = 0x8C,
                Description = "RTC月",
                Description_US = "RTC Month",
                DefaultValue = 1F,
                SavedValue = message[70],
                SetValue = message[70],
                Coef = 1
            }));

            SystemParameter_1_Collection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameter_1_Collection.Count + 1,
                CommandType = 0xE9,
                CID2 = 0x8C,
                Description = "RTC日",
                Description_US = "RTC Day",
                DefaultValue = 1F,
                SavedValue = message[71],
                SetValue = message[71],
                Coef = 1
            }));

            SystemParameter_1_Collection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameter_1_Collection.Count + 1,
                CommandType = 0xE9,
                CID2 = 0x8C,
                Description = "RTC时",
                Description_US = "RTC Hour",
                DefaultValue = 1F,
                SavedValue = message[72],
                SetValue = message[72],
                Coef = 1
            }));

            SystemParameter_1_Collection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameter_1_Collection.Count + 1,
                CommandType = 0xE9,
                CID2 = 0x8C,
                Description = "RTC分",
                Description_US = "RTC Minute",
                DefaultValue = 1F,
                SavedValue = message[73],
                SetValue = message[73],
                Coef = 1
            }));

            SystemParameter_1_Collection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameter_1_Collection.Count + 1,
                CommandType = 0xE9,
                CID2 = 0x8C,
                Description = "RTC秒",
                Description_US = "RTC Second",
                DefaultValue = 1F,
                SavedValue = message[74],
                SetValue = message[74],
                Coef = 1
            }));

            CalibrationParameterCollection.Clear();

            CalibrationParameterCollection.Add(ViewModelSource.Create(() => new CalibrationParameter
            {
                CalibrationParameterID = CalibrationParameterCollection.Count + 1,
                CommandType = 0xEA,
                CID2 = 0x8C,
                Description = "单体电压系数",
                Description_US = "Cell Voltage Coef",
                SavedCoef = BitConverter.ToUInt16(new byte[] { message[76], message[75] }, 0),
                ReadValue = 0F,
                MeasureValue = 0F
            }));

            CalibrationParameterCollection.Add(ViewModelSource.Create(() => new CalibrationParameter
            {
                CalibrationParameterID = CalibrationParameterCollection.Count + 1,
                CommandType = 0xEB,
                CID2 = 0x8C,
                Description = "母线电压系数",
                Description_US = "BusBar Voltage Coef",
                SavedCoef = BitConverter.ToUInt16(new byte[] { message[78], message[77] }, 0),
                ReadValue = 0F,
                MeasureValue = 0F
            }));

            CalibrationParameterCollection.Add(ViewModelSource.Create(() => new CalibrationParameter
            {
                CalibrationParameterID = CalibrationParameterCollection.Count + 1,
                CommandType = 0xEC,
                CID2 = 0x8C,
                Description = "充电电流系数",
                Description_US = "CH Current Coef",
                SavedCoef = BitConverter.ToUInt16(new byte[] { message[80], message[79] }, 0),
                ReadValue = 0F,
                MeasureValue = 0F
            }));

            CalibrationParameterCollection.Add(ViewModelSource.Create(() => new CalibrationParameter
            {
                CalibrationParameterID = CalibrationParameterCollection.Count + 1,
                CommandType = 0xED,
                CID2 = 0x8C,
                Description = "放电电流系数",
                Description_US = "DCH Current Coef",
                SavedCoef = BitConverter.ToUInt16(new byte[] { message[82], message[81] }, 0),
                ReadValue = 0F,
                MeasureValue = 0F
            }));

            CalibrationParameterCollection.Add(ViewModelSource.Create(() => new CalibrationParameter
            {
                CalibrationParameterID = CalibrationParameterCollection.Count + 1,
                CommandType = 0xEE,
                CID2 = 0x8C,
                Description = "单体温度系数",
                Description_US = "Cell Temp Coef",
                SavedCoef = BitConverter.ToUInt16(new byte[] { message[84], message[83] }, 0),
                ReadValue = 0F,
                MeasureValue = 0F
            }));

            CalibrationParameterCollection.Add(ViewModelSource.Create(() => new CalibrationParameter
            {
                CalibrationParameterID = CalibrationParameterCollection.Count + 1,
                CommandType = 0xEF,
                CID2 = 0x8C,
                Description = "环境温度系数",
                Description_US = "Ambient T Coef",
                SavedCoef = BitConverter.ToUInt16(new byte[] { message[86], message[85] }, 0),
                ReadValue = 0F,
                MeasureValue = 0F
            }));

            CalibrationParameterCollection.Add(ViewModelSource.Create(() => new CalibrationParameter
            {
                CalibrationParameterID = CalibrationParameterCollection.Count + 1,
                CommandType = 0xF0,
                CID2 = 0x8C,
                Description = "MOS温度系数",
                Description_US = "MOS Temp Coef",
                SavedCoef = BitConverter.ToUInt16(new byte[] { message[88], message[87] }, 0),
                ReadValue = 0F,
                MeasureValue = 0F
            }));

            CalibrationParameterCollection.Add(ViewModelSource.Create(() => new CalibrationParameter
            {
                CalibrationParameterID = CalibrationParameterCollection.Count + 1,
                CommandType = 0xF1,
                CID2 = 0x8C,
                Description = "SOC(%)",
                Description_US = "SOC(%)",
                SavedCoef = BitConverter.ToUInt16(new byte[] { message[90], message[89] }, 0),
                ReadValue = 0F,
                MeasureValue = 0F
            }));

            SystemParameter_1_Collection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameter_1_Collection.Count + 1,
                CommandType = 0xF2,
                CID2 = 0x8C,
                Description = "SOC低告警值(%)",
                Description_US = "SOC Low Warning(%)",
                DefaultValue = 5F,
                SavedValue = BitConverter.ToUInt16(new byte[] { message[92], message[91] }, 0) / 100F,
                SetValue = BitConverter.ToUInt16(new byte[] { message[92], message[91] }, 0) / 100F,
                Coef = 100
            }));

            SystemParameter_1_Collection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameter_1_Collection.Count + 1,
                CommandType = 0xF3,
                CID2 = 0x8C,
                Description = "SOC低保护值(%)",
                Description_US = "SOC Low Protection(%)",
                DefaultValue = 0F,
                SavedValue = BitConverter.ToUInt16(new byte[] { message[94], message[93] }, 0) / 100F,
                SetValue = BitConverter.ToUInt16(new byte[] { message[94], message[93] }, 0) / 100F,
                Coef = 100
            }));

            IsLoadingCoslightSystemParameter = false;
        }
        private void ParseMessage_SystemParameter_Coslight_FB100C5(byte[] message)
        {
            if (message.Length != 0x8d)
            {
                    return;
                }

                int packAddress = message[2] - 1;

            #region SystemParameterCollection

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0x80,
                CID2 = 0x8C,
                Description = "单体过压告警恢复阈值(V)",
                Description_US = "Cell OV Warning Restore(V)",
                DefaultValue = 3.6F,
                SavedValue = BitConverter.ToUInt16(new byte[] { message[8], message[7] }, 0) / 1000F,
                SetValue = BitConverter.ToUInt16(new byte[] { message[8], message[7] }, 0) / 1000F,
                Coef = 1000,
                ParameterScope = "2~4V",
                MaxValue = 4,
                MinValue = 2
            }));

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0x81,
                CID2 = 0x8C,
                Description = "单体欠压告警恢复阈值(V)",
                Description_US = "Cell UV Warning Restore(V)",
                DefaultValue = 3.2F,
                SavedValue = BitConverter.ToUInt16(new byte[] { message[10], message[9] }, 0) / 1000F,
                SetValue = BitConverter.ToUInt16(new byte[] { message[10], message[9] }, 0) / 1000F,
                Coef = 1000,
                ParameterScope = "2~4V",
                MaxValue = 4,
                MinValue = 2
            }));

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0x84,
                CID2 = 0x8C,
                Description = "电池组过压告警恢复阈值(V)",
                Description_US = "Pack OV Warning Restore(V)",
                DefaultValue = 54F,
                SavedValue = BitConverter.ToUInt16(new byte[] { message[12], message[11] }, 0) / 100F,
                SetValue = BitConverter.ToUInt16(new byte[] { message[12], message[11] }, 0) / 100F,
                Coef = 100,
                ParameterScope = "40~60V",
                MaxValue = 60,
                MinValue = 40
            }));

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0x85,
                CID2 = 0x8C,
                Description = "电池组欠压告警恢复阈值(V)",
                Description_US = "Pack UV Warning Restore(V)",
                DefaultValue = 48F,
                SavedValue = BitConverter.ToUInt16(new byte[] { message[14], message[13] }, 0) / 100F,
                SetValue = BitConverter.ToUInt16(new byte[] { message[14], message[13] }, 0) / 100F,
                Coef = 100,
                ParameterScope = "40~60V",
                MaxValue = 60,
                MinValue = 40
            }));

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0x86,
                CID2 = 0x8C,
                Description = "电池组过压保护恢复阈值(V)",
                Description_US = "Pack OV Protection Restore(V)",
                DefaultValue = 50.25F,
                SavedValue = BitConverter.ToUInt16(new byte[] { message[16], message[15] }, 0) / 100F,
                SetValue = BitConverter.ToUInt16(new byte[] { message[16], message[15] }, 0) / 100F,
                Coef = 100,
                ParameterScope = "40~60V",
                MaxValue = 60,
                MinValue = 40
            }));

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0xC5,
                CID2 = 0x8C,
                Description = "充电温度高告警恢复阈值(℃)",
                Description_US = "CH OT Warning Restore(℃)",
                DefaultValue = 47F,
                SavedValue = BitConverter.ToInt16(new byte[] { message[18], message[17] }, 0) / 100F,
                SetValue = BitConverter.ToInt16(new byte[] { message[18], message[17] }, 0) / 100F,
                Coef = 100,
                ParameterScope = "25~65℃",
                MaxValue = 65,
                MinValue = 25
            }));

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0xC6,
                CID2 = 0x8C,
                Description = "充电温度低告警恢复阈值(℃)",
                Description_US = "CH UT Warning Restore(℃)",
                DefaultValue = 8F,
                SavedValue = BitConverter.ToInt16(new byte[] { message[20], message[19] }, 0) / 100F,
                SetValue = BitConverter.ToInt16(new byte[] { message[20], message[19] }, 0) / 100F,
                Coef = 100,
                ParameterScope = "-20~25℃",
                MaxValue = 25,
                MinValue = -20
            }));

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0xC9,
                CID2 = 0x8C,
                Description = "放电温度高告警恢复阈值(℃)",
                Description_US = "DCH OT Warning Restore(℃)",
                DefaultValue = 57F,
                SavedValue = BitConverter.ToInt16(new byte[] { message[22], message[21] }, 0) / 100F,
                SetValue = BitConverter.ToInt16(new byte[] { message[22], message[21] }, 0) / 100F,
                Coef = 100,
                ParameterScope = "25~65℃",
                MaxValue = 65,
                MinValue = 25
            }));

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0xCA,
                CID2 = 0x8C,
                Description = "放电温度低告警恢复阈值(℃)",
                Description_US = "DCH UT Warning Restore(℃)",
                DefaultValue = -12F,
                SavedValue = BitConverter.ToInt16(new byte[] { message[24], message[23] }, 0) / 100F,
                SetValue = BitConverter.ToInt16(new byte[] { message[24], message[23] }, 0) / 100F,
                Coef = 100,
                ParameterScope = "-20~25℃",
                MaxValue = 25,
                MinValue = -20
            }));

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0xD0,
                CID2 = 0x8C,
                Description = "放电过流告警恢复阈值(A)",
                Description_US = "DCH OC Warning Restore(A)",
                DefaultValue = 100F,
                SavedValue = BitConverter.ToUInt16(new byte[] { message[28], message[27] }, 0) / 100F,
                SetValue = BitConverter.ToUInt16(new byte[] { message[28], message[27] }, 0) / 100F,
                Coef = 100,
                ParameterScope = (0.2f * CellCapacity).ToString() + "~" + (2f * CellCapacity).ToString() + "A",
                MaxValue = 2f * CellCapacity,
                MinValue = 0.2f * CellCapacity
            }));

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0xCD,
                CID2 = 0x8C,
                Description = "充电过流告警恢复阈值(A)",
                Description_US = "CH OC Warning Restore(A)",
                DefaultValue = 100F,//20200422
                                    //DefaultValue = 80F,
                SavedValue = BitConverter.ToUInt16(new byte[] { message[26], message[25] }, 0) / 100F,
                SetValue = BitConverter.ToUInt16(new byte[] { message[26], message[25] }, 0) / 100F,
                Coef = 100,
                ParameterScope = (0.2f * CellCapacity).ToString() + "~" + (2f * CellCapacity).ToString() + "A",
                MaxValue = 2f * CellCapacity,
                MinValue = 0.2f * CellCapacity
            }));


            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0xD8,
                CID2 = 0x8C,
                Description = "电芯一致性差告警电压差恢复阈值(V)",
                Description_US = "Cell Behind Warning Volt Diff Restore(V)",
                DefaultValue = 0.3F,
                SavedValue = BitConverter.ToUInt16(new byte[] { message[30], message[29] }, 0) / 100F,
                SetValue = BitConverter.ToUInt16(new byte[] { message[30], message[29] }, 0) / 100F,
                Coef = 100,
                ParameterScope = "0.05~1.5V",
                MaxValue = 1.5f,
                MinValue = 0.05f
            }));

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0xD9,
                CID2 = 0x8C,
                Description = "环境温度高告警恢复阈值(℃)",
                Description_US = "Ambient OT Warning Restore(℃)",
                DefaultValue = 50F,
                SavedValue = BitConverter.ToInt16(new byte[] { message[32], message[31] }, 0) / 100F,
                SetValue = BitConverter.ToInt16(new byte[] { message[32], message[31] }, 0) / 100F,
                Coef = 100,
                ParameterScope = "25~65℃",
                MaxValue = 65,
                MinValue = 25
            }));

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0xDA,
                CID2 = 0x8C,
                Description = "环境温度低告警恢复阈值(℃)",
                Description_US = "Ambient UT Warning Restore(℃)",
                DefaultValue = 5F,
                SavedValue = BitConverter.ToInt16(new byte[] { message[34], message[33] }, 0) / 100F,
                SetValue = BitConverter.ToInt16(new byte[] { message[34], message[33] }, 0) / 100F,
                Coef = 100,
                ParameterScope = "-20~25℃",
                MaxValue = 25,
                MinValue = -20
            }));
            #endregion

            #region SystemParameter_1_Collection

            SystemParameter_1_Collection.Clear();

            SystemParameter_1_Collection.Add(ViewModelSource.Create(() => new Models.SystemParameter//不可设置不显示
            {
                SystemParameterID = SystemParameter_1_Collection.Count + 1,
                CommandType = 0xDB,
                CID2 = 0x8C,
                Description = "额定容量(Ah)",
                Description_US = "Rated Capacity(Ah)",
                DefaultValue = 100F,
                SavedValue = BitConverter.ToUInt16(new byte[] { message[37], message[36] }, 0) / 100F,
                SetValue = BitConverter.ToUInt16(new byte[] { message[37], message[36] }, 0) / 100F,
                Coef = 100,
                ParameterScope = "Disallowed"
            }));

            SystemParameter_1_Collection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameter_1_Collection.Count + 1,
                CommandType = 0xE8,
                CID2 = 0x8C,
                Description = "数据存储时间间隔(S)",
                Description_US = "Data Save Interval(S)",
                DefaultValue = 180F,
                SavedValue = BitConverter.ToUInt16(new byte[] { message[67], message[66] }, 0),
                SetValue = BitConverter.ToUInt16(new byte[] { message[67], message[66] }, 0),
                Coef = 1,
                ParameterScope = "10~6000s",
                MaxValue = 6000f,
                MinValue = 10f
            }));

            SystemParameter_1_Collection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameter_1_Collection.Count + 1,
                CommandType = 0xE2,
                CID2 = 0x8C,
                Description = "充电截至电压(V)",
                Description_US = "CH Terminal Voltage(V)",
                DefaultValue = 52F,
                SavedValue = BitConverter.ToUInt16(new byte[] { message[57], message[56] }, 0) / 100F,
                SetValue = BitConverter.ToUInt16(new byte[] { message[57], message[56] }, 0) / 100F,
                Coef = 100,
                ParameterScope = "50~60V",
                MaxValue = 60f,
                MinValue = 50f
            }));


            SystemParameter_1_Collection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameter_1_Collection.Count + 1,
                CommandType = 0xDE,
                CID2 = 0x8C,
                Description = "电池组编号",
                Description_US = "Pack ID",
                DefaultValue = 1F,
                SavedValue = BitConverter.ToUInt32(new byte[] { message[49], message[48], message[47], message[46] }, 0),
                SetValue = BitConverter.ToUInt32(new byte[] { message[49], message[48], message[47], message[46] }, 0),
                Coef = 1,
                ParameterScope = "0~0xFFFFFFFF",
                MaxValue = 0xFFFFFFFF,
                MinValue = 0
            }));

            SystemParameter_1_Collection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameter_1_Collection.Count + 1,
                CommandType = 0xDF,
                CID2 = 0x8C,
                Description = "均衡启动电压(V)",
                Description_US = "Balance Start Volt(V)",
                DefaultValue = 3.35F,
                SavedValue = BitConverter.ToUInt16(new byte[] { message[51], message[50] }, 0) / 1000F,
                SetValue = BitConverter.ToUInt16(new byte[] { message[51], message[50] }, 0) / 1000F,
                Coef = 1000,
                ParameterScope = "3.0~4.2V",
                MaxValue = 4.2f,
                MinValue = 3.0f
            }));

            SystemParameter_1_Collection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameter_1_Collection.Count + 1,
                CommandType = 0xE0,
                CID2 = 0x8C,
                Description = "均衡停止电压(V)",
                Description_US = "Balance Stop Volt Diff(V)",
                DefaultValue = 3.2F,
                SavedValue = BitConverter.ToUInt16(new byte[] { message[53], message[52] }, 0) / 1000F,
                SetValue = BitConverter.ToUInt16(new byte[] { message[53], message[52] }, 0) / 1000F,
                Coef = 1000,
                ParameterScope = "3.0~4.2V",
                MaxValue = 4.2f,
                MinValue = 3.0f
            }));

            SystemParameter_1_Collection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameter_1_Collection.Count + 1,
                CommandType = 0xE1,
                CID2 = 0x8C,
                Description = "均衡启动压差(V)",
                Description_US = "Balance Start Volt Diff(V)",
                DefaultValue = 0.03F,
                SavedValue = BitConverter.ToUInt16(new byte[] { message[55], message[54] }, 0) / 1000F,
                SetValue = BitConverter.ToUInt16(new byte[] { message[55], message[54] }, 0) / 1000F,
                Coef = 1000,
                ParameterScope = "0.01~0.1V",
                MaxValue = 0.1f,
                MinValue = 0.01f
            }));

            SystemParameter_1_Collection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameter_1_Collection.Count + 1,
                CommandType = 0xE3,
                CID2 = 0x8C,
                Description = "充电截至电流(A)",
                Description_US = "CH Terminal Current(A)",
                DefaultValue = 2F,
                SavedValue = BitConverter.ToUInt16(new byte[] { message[59], message[58] }, 0) / 100F,
                SetValue = BitConverter.ToUInt16(new byte[] { message[59], message[58] }, 0) / 100F,
                Coef = 100,
                ParameterScope = "0.01~0.1C",
                MaxValue = 0.1f * 100,
                MinValue = 0.01f * 100
            }));

            if (IsManagerState)//20200722添加管理权限
            {
                SystemParameter_1_Collection.Add(ViewModelSource.Create(() => new Models.SystemParameter//20200722关闭设置
                {
                    SystemParameterID = SystemParameter_1_Collection.Count + 1,
                    CommandType = 0xE4,
                    CID2 = 0x8C,
                    Description = "放电循环次数",
                    Description_US = "DCH Cycle Times",
                    DefaultValue = 0F,
                    SavedValue = BitConverter.ToUInt16(new byte[] { message[61], message[60] }, 0),
                    SetValue = BitConverter.ToUInt16(new byte[] { message[61], message[60] }, 0),
                    Coef = 1
                }));
            }

            SystemParameter_1_Collection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameter_1_Collection.Count + 1,
                CommandType = 0xE5,
                CID2 = 0x8C,
                Description = "电池单体数量",
                Description_US = "Cell Count",
                DefaultValue = 15F,
                SavedValue = message[62],
                SetValue = message[62],
                Coef = 1,
                ParameterScope = "4~16",
                MaxValue = 16f,
                MinValue = 4f
            }));

            SystemParameter_1_Collection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameter_1_Collection.Count + 1,
                CommandType = 0xE6,
                CID2 = 0x8C,
                Description = "电池温度数量",
                Description_US = "Cell Temp Count",
                DefaultValue = 4F,
                SavedValue = message[63],
                SetValue = message[63],
                Coef = 1,
                ParameterScope = "4~8",
                MaxValue = 8f,
                MinValue = 4f
            }));

            SystemParameter_1_Collection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameter_1_Collection.Count + 1,
                CommandType = 0xE7,
                CID2 = 0x8C,
                Description = "静置关机时间(min)",
                Description_US = "Static PowerOff Time(min)",
                DefaultValue = 720,
                SavedValue = BitConverter.ToUInt16(new byte[] { message[65], message[64] }, 0),
                SetValue = BitConverter.ToUInt16(new byte[] { message[65], message[64] }, 0),
                Coef = 1,
                ParameterScope = "0~5760min",
                MaxValue = 5760f,
                MinValue = 0f
            }));


            SystemParameter_1_Collection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameter_1_Collection.Count + 1,
                CommandType = 0xE9,
                CID2 = 0x8C,
                Description = "RTC年",
                Description_US = "RTC Year",
                DefaultValue = 2018F,
                SavedValue = BitConverter.ToUInt16(new byte[] { message[69], message[68] }, 0),
                SetValue = BitConverter.ToUInt16(new byte[] { message[69], message[68] }, 0),
                Coef = 1,
                ParameterScope = "≤2099",
                MaxValue = 2099f,
                MinValue = 0f
            }));

            SystemParameter_1_Collection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameter_1_Collection.Count + 1,
                CommandType = 0xE9,
                CID2 = 0x8C,
                Description = "RTC月",
                Description_US = "RTC Month",
                DefaultValue = 1F,
                SavedValue = message[70],
                SetValue = message[70],
                Coef = 1,
                ParameterScope = "≤12",
                MaxValue = 12f,
                MinValue = 0f
            }));

            SystemParameter_1_Collection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameter_1_Collection.Count + 1,
                CommandType = 0xE9,
                CID2 = 0x8C,
                Description = "RTC日",
                Description_US = "RTC Day",
                DefaultValue = 1F,
                SavedValue = message[71],
                SetValue = message[71],
                Coef = 1,
                ParameterScope = "≤31",
                MaxValue = 31f,
                MinValue = 0f
            }));

            SystemParameter_1_Collection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameter_1_Collection.Count + 1,
                CommandType = 0xE9,
                CID2 = 0x8C,
                Description = "RTC时",
                Description_US = "RTC Hour",
                DefaultValue = 1F,
                SavedValue = message[72],
                SetValue = message[72],
                Coef = 1,
                ParameterScope = "≤24",
                MaxValue = 24f,
                MinValue = 0f
            }));

            SystemParameter_1_Collection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameter_1_Collection.Count + 1,
                CommandType = 0xE9,
                CID2 = 0x8C,
                Description = "RTC分",
                Description_US = "RTC Minute",
                DefaultValue = 1F,
                SavedValue = message[73],
                SetValue = message[73],
                Coef = 1,
                ParameterScope = "≤59",
                MaxValue = 59f,
                MinValue = 0f
            }));

            SystemParameter_1_Collection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameter_1_Collection.Count + 1,
                CommandType = 0xE9,
                CID2 = 0x8C,
                Description = "RTC秒",
                Description_US = "RTC Second",
                DefaultValue = 1F,
                SavedValue = message[74],
                SetValue = message[74],
                Coef = 1,
                ParameterScope = "≤59",
                MaxValue = 59f,
                MinValue = 0f
            }));

            CalibrationParameterCollection.Clear();

            CalibrationParameterCollection.Add(ViewModelSource.Create(() => new CalibrationParameter
            {
                CalibrationParameterID = CalibrationParameterCollection.Count + 1,
                CommandType = 0xEA,
                CID2 = 0x8C,
                Description = "单体电压系数",
                Description_US = "Cell Voltage Coef",
                SavedCoef = BitConverter.ToUInt16(new byte[] { message[76], message[75] }, 0),
                ReadValue = 0F,
                MeasureValue = 0F,

                MaxValue = 18000,
                MinValue = 4000
            }));

            CalibrationParameterCollection.Add(ViewModelSource.Create(() => new CalibrationParameter
            {
                CalibrationParameterID = CalibrationParameterCollection.Count + 1,
                CommandType = 0xEB,
                CID2 = 0x8C,
                Description = "母线电压系数",
                Description_US = "BusBar Voltage Coef",
                SavedCoef = BitConverter.ToUInt16(new byte[] { message[78], message[77] }, 0),
                ReadValue = 0F,
                MeasureValue = 0F,

                MaxValue = 18000,
                MinValue = 4000
            }));

            CalibrationParameterCollection.Add(ViewModelSource.Create(() => new CalibrationParameter
            {
                CalibrationParameterID = CalibrationParameterCollection.Count + 1,
                CommandType = 0xEC,
                CID2 = 0x8C,
                Description = "充电电流系数",
                Description_US = "CH Current Coef",
                SavedCoef = BitConverter.ToUInt16(new byte[] { message[80], message[79] }, 0),
                ReadValue = 0F,
                MeasureValue = 0F,

                MaxValue = 40000,
                MinValue = 4000
            }));

            CalibrationParameterCollection.Add(ViewModelSource.Create(() => new CalibrationParameter
            {
                CalibrationParameterID = CalibrationParameterCollection.Count + 1,
                CommandType = 0xED,
                CID2 = 0x8C,
                Description = "放电电流系数",
                Description_US = "DCH Current Coef",
                SavedCoef = BitConverter.ToUInt16(new byte[] { message[82], message[81] }, 0),
                ReadValue = 0F,
                MeasureValue = 0F,

                MaxValue = 40000,
                MinValue = 4000
            }));

            CalibrationParameterCollection.Add(ViewModelSource.Create(() => new CalibrationParameter
            {
                CalibrationParameterID = CalibrationParameterCollection.Count + 1,
                CommandType = 0xEE,
                CID2 = 0x8C,
                Description = "单体温度系数",
                Description_US = "Cell Temp Coef",
                SavedCoef = BitConverter.ToUInt16(new byte[] { message[84], message[83] }, 0),
                ReadValue = 0F,
                MeasureValue = 0F,

                MaxValue = 18000,
                MinValue = 4000
            }));

            CalibrationParameterCollection.Add(ViewModelSource.Create(() => new CalibrationParameter
            {
                CalibrationParameterID = CalibrationParameterCollection.Count + 1,
                CommandType = 0xEF,
                CID2 = 0x8C,
                Description = "环境温度系数",
                Description_US = "Ambient T Coef",
                SavedCoef = BitConverter.ToUInt16(new byte[] { message[86], message[85] }, 0),
                ReadValue = 0F,
                MeasureValue = 0F,

                MaxValue = 18000,
                MinValue = 4000
            }));

            CalibrationParameterCollection.Add(ViewModelSource.Create(() => new CalibrationParameter
            {
                CalibrationParameterID = CalibrationParameterCollection.Count + 1,
                CommandType = 0xF0,
                CID2 = 0x8C,
                Description = "MOS温度系数",
                Description_US = "MOS T Coef",
                SavedCoef = BitConverter.ToUInt16(new byte[] { message[88], message[87] }, 0),
                ReadValue = 0F,
                MeasureValue = 0F,

                MaxValue = 18000,
                MinValue = 4000
            }));

            SystemParameter_1_Collection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameter_1_Collection.Count + 1,
                CommandType = 0xF2,
                CID2 = 0x8C,
                Description = "SOC低告警值(%)",
                Description_US = "SOC Low Warning(%)",
                DefaultValue = 5F,
                SavedValue = BitConverter.ToUInt16(new byte[] { message[92], message[91] }, 0) / 100F,
                SetValue = BitConverter.ToUInt16(new byte[] { message[92], message[91] }, 0) / 100F,
                Coef = 100,
                ParameterScope = "0~60%",
                MaxValue = 60f,
                MinValue = 0f
            }));

            SystemParameter_1_Collection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameter_1_Collection.Count + 1,
                CommandType = 0xF3,
                CID2 = 0x8C,
                Description = "SOC低保护值(%)",
                Description_US = "SOC Low Protection(%)",
                DefaultValue = 0F,
                SavedValue = BitConverter.ToUInt16(new byte[] { message[94], message[93] }, 0) / 100F,
                SetValue = BitConverter.ToUInt16(new byte[] { message[94], message[93] }, 0) / 100F,
                Coef = 100,
                ParameterScope = "0~60%",
                MaxValue = 60f,
                MinValue = 0f
            }));

            #endregion

            SystemParameter_2_Collection.Clear();

            SystemParameter_2_Collection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameter_1_Collection.Count + 1,
                CommandType = 0xDC,
                CID2 = 0x8C,
                Description = "累计充电容量(Ah)",
                Description_US = "Accumulative CH Capacity(Ah)",
                SavedValue = BitConverter.ToUInt32(new byte[] { message[41], message[40], message[39], message[38] }, 0),
                SetValue = BitConverter.ToUInt32(new byte[] { message[41], message[40], message[39], message[38] }, 0),
                Coef = 1
            }));

            AccCHCapacity = SystemParameter_2_Collection[0].SavedValue.ToString();

            SystemParameter_2_Collection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameter_1_Collection.Count + 1,
                CommandType = 0xDD,
                CID2 = 0x8C,
                Description = "累计放电容量(Ah)",
                Description_US = "Accumulative DCH Capacity(Ah)",
                SavedValue = BitConverter.ToInt32(new byte[] { message[45], message[44], message[43], message[42] }, 0),
                SetValue = BitConverter.ToInt32(new byte[] { message[45], message[44], message[43], message[42] }, 0),
                Coef = 1
            }));

            AccDCHCapacity = SystemParameter_2_Collection[1].SavedValue.ToString();

            SystemParameter_2_Collection.Add(ViewModelSource.Create(() => new SystemParameter
            {
                SystemParameterID = SystemParameter_1_Collection.Count + 1,
                CommandType = 0xF1,
                CID2 = 0x8C,
                Description = "SOC(%)",
                Description_US = "SOC(%)",
                SavedValue = BitConverter.ToUInt16(new byte[] { message[90], message[89] }, 0) / 100f,
                SetValue = BitConverter.ToUInt16(new byte[] { message[90], message[89] }, 0) / 100f,
                Coef = 100
            }));

            SOC = SystemParameter_2_Collection[2].SavedValue.ToString();

            int offset = 98 + 2 * 2;
            int intCommandType = 0x13;

            SystemParameter_1_Collection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameter_1_Collection.Count + 1,
                CommandType = intCommandType,
                CID2 = 0x8C,
                Description = "SOC低告警恢复值(%)",
                Description_US = "SOC Warning Restore(%)",
                DefaultValue = 7F,
                SavedValue = BitConverter.ToUInt16(new byte[] { message[offset + 1], message[offset] }, 0) / 100F,
                SetValue = BitConverter.ToUInt16(new byte[] { message[offset + 1], message[offset] }, 0) / 100F,
                Coef = 100,
                ParameterScope = "0~60%",
                MaxValue = 60f,
                MinValue = 0f
            }));

            offset += 2;

            intCommandType++;
            intCommandType++;
            offset += 2;
            SystemParameter_1_Collection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameter_1_Collection.Count + 1,
                CommandType = intCommandType,
                CID2 = 0x8C,
                Description = "电池SOH低告警值(%)",
                Description_US = "SOH Warning(%)",
                DefaultValue = 50F,
                SavedValue = BitConverter.ToUInt16(new byte[] { message[offset + 1], message[offset] }, 0) / 100F,
                SetValue = BitConverter.ToUInt16(new byte[] { message[offset + 1], message[offset] }, 0) / 100F,
                Coef = 100,
                ParameterScope = "0~70%",
                MaxValue = 70f,
                MinValue = 0f
            }));

            intCommandType++;
            offset += 2;
            SystemParameter_1_Collection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameter_1_Collection.Count + 1,
                CommandType = intCommandType,
                CID2 = 0x8C,
                Description = "电池SOH低告警恢复值(%)",
                Description_US = "SOH Warning Restore(%)",
                DefaultValue = 60F,
                SavedValue = BitConverter.ToUInt16(new byte[] { message[offset + 1], message[offset] }, 0) / 100F,
                SetValue = BitConverter.ToUInt16(new byte[] { message[offset + 1], message[offset] }, 0) / 100F,
                Coef = 100,
                ParameterScope = "0~70%",
                MaxValue = 70f,
                MinValue = 0f
            }));

            intCommandType++;
            offset += 2;
            SystemParameter_1_Collection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameter_1_Collection.Count + 1,
                CommandType = intCommandType,
                CID2 = 0x8C,
                Description = "电池SOH低保护值(%)",
                Description_US = "SOH Protection(%)",
                DefaultValue = 30F,
                SavedValue = BitConverter.ToUInt16(new byte[] { message[offset + 1], message[offset] }, 0) / 100F,
                SetValue = BitConverter.ToUInt16(new byte[] { message[offset + 1], message[offset] }, 0) / 100F,
                Coef = 100,
                ParameterScope = "0~70%",
                MaxValue = 70f,
                MinValue = 0f
            }));

            offset += 2;
            intCommandType++;
            intCommandType++;
            offset += 2;
            SystemParameter_1_Collection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameter_1_Collection.Count + 1,
                CommandType = intCommandType,
                CID2 = 0x8C,
                Description = "电芯一致性差保护电压差阈值(V)",
                Description_US = "Cell Consistency Protection(V)",
                DefaultValue = 0.5F,
                SavedValue = BitConverter.ToUInt16(new byte[] { message[offset + 1], message[offset] }, 0) / 1000F,
                SetValue = BitConverter.ToUInt16(new byte[] { message[offset + 1], message[offset] }, 0) / 1000F,
                Coef = 1000,
                ParameterScope = "0.05~1.5V",
                MaxValue = 1.5f,
                MinValue = 0.05f
            }));

            intCommandType++;
            offset += 2;
            SystemParameter_1_Collection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameter_1_Collection.Count + 1,
                CommandType = intCommandType,
                CID2 = 0x8C,
                Description = "电芯一致性差保护电压差恢复阈值(V)",
                Description_US = "Cell Consistency Protection Restore(V)",
                DefaultValue = 0.3F,
                SavedValue = BitConverter.ToUInt16(new byte[] { message[offset + 1], message[offset] }, 0) / 1000F,
                SetValue = BitConverter.ToUInt16(new byte[] { message[offset + 1], message[offset] }, 0) / 1000F,
                Coef = 1000,
                ParameterScope = "0.05~1.5V",
                MaxValue = 1.5f,
                MinValue = 0.05f
            }));

            intCommandType++;
            offset += 2;
            SystemParameter_1_Collection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameter_1_Collection.Count + 1,
                CommandType = intCommandType,
                CID2 = 0x8C,
                Description = "电芯损坏保护值(V)",
                Description_US = "Cell Damage Protection(V)",
                DefaultValue = 1.5F,
                SavedValue = BitConverter.ToUInt16(new byte[] { message[offset + 1], message[offset] }, 0) / 1000F,
                SetValue = BitConverter.ToUInt16(new byte[] { message[offset + 1], message[offset] }, 0) / 1000F,
                Coef = 1000,
                ParameterScope = "1~2.5V",
                MaxValue = 2.5f,
                MinValue = 1f
            }));

            intCommandType++;
            offset += 2;
            SystemParameter_1_Collection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameter_1_Collection.Count + 1,
                CommandType = intCommandType,
                CID2 = 0x8C,
                Description = "电芯损坏保护恢复值(V)",
                Description_US = "Cell Damage Protection Restore(V)",
                DefaultValue = 2F,
                SavedValue = BitConverter.ToUInt16(new byte[] { message[offset + 1], message[offset] }, 0) / 1000F,
                SetValue = BitConverter.ToUInt16(new byte[] { message[offset + 1], message[offset] }, 0) / 1000F,
                Coef = 1000,
                ParameterScope = "1~2.5V",
                MaxValue = 2.5f,
                MinValue = 1f
            }));

            intCommandType++;
            offset += 2;
            SystemParameter_1_Collection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameter_1_Collection.Count + 1,
                CommandType = intCommandType,
                CID2 = 0x8C,
                Description = "补充电电压（V）",
                Description_US = "Supplementary Voltage(V)",
                DefaultValue = 50.25F,
                SavedValue = BitConverter.ToUInt16(new byte[] { message[offset + 1], message[offset] }, 0) / 100F,
                SetValue = BitConverter.ToUInt16(new byte[] { message[offset + 1], message[offset] }, 0) / 100F,
                Coef = 100,
                ParameterScope = "49~53V",
                MaxValue = 53f,
                MinValue = 49f
            }));

            intCommandType++;
            offset += 2;
            SystemParameter_1_Collection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameter_1_Collection.Count + 1,
                CommandType = intCommandType,
                CID2 = 0x8C,
                Description = "补充电SOC(%)",
                Description_US = "Supplementary SOC(%)",
                DefaultValue = 95F,
                SavedValue = BitConverter.ToUInt16(new byte[] { message[offset + 1], message[offset] }, 0) / 100F,
                SetValue = BitConverter.ToUInt16(new byte[] { message[offset + 1], message[offset] }, 0) / 100F,
                Coef = 100,
                ParameterScope = "80~100%",
                MaxValue = 100,
                MinValue = 80
            }));

            intCommandType++;
            offset += 2;
            SystemParameter_1_Collection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameter_1_Collection.Count + 1,
                CommandType = intCommandType,
                CID2 = 0x8C,
                Description = "均衡停止压差(V)",
                Description_US = "Balance Stop Volt Diff(V)",
                DefaultValue = 0.02F,
                SavedValue = BitConverter.ToUInt16(new byte[] { message[offset + 1], message[offset] }, 0) / 1000F,
                SetValue = BitConverter.ToUInt16(new byte[] { message[offset + 1], message[offset] }, 0) / 1000F,
                Coef = 1000,
                ParameterScope = "0.01~0.1V",
                MaxValue = 0.1f,
                MinValue = 0.01f
            }));

            intCommandType++;
            intCommandType++;
            offset += 2;
            SystemParameter_1_Collection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameter_1_Collection.Count + 1,
                CommandType = intCommandType,
                CID2 = 0x8C,
                Description = "欠压保护关机时间(s)",
                Description_US = "UV Protection PowerOff Time(s)",
                DefaultValue = 300F,
                SavedValue = BitConverter.ToUInt16(new byte[] { message[offset + 1], message[offset] }, 0),
                SetValue = BitConverter.ToUInt16(new byte[] { message[offset + 1], message[offset] }, 0),
                Coef = 1,
                ParameterScope = "5~300s",
                MaxValue = 300f,
                MinValue = 5f
            }));

            intCommandType++;
            offset += 2;
            SystemParameter_1_Collection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameter_1_Collection.Count + 1,
                CommandType = intCommandType,
                CID2 = 0x8C,
                Description = "充电过流限流时间(min)",
                Description_US = "CH OC Current-Limited Time(min)",
                DefaultValue = 30F,
                SavedValue = BitConverter.ToUInt16(new byte[] { message[offset + 1], message[offset] }, 0),
                SetValue = BitConverter.ToUInt16(new byte[] { message[offset + 1], message[offset] }, 0),
                Coef = 1,
                ParameterScope = "1~60min",
                MaxValue = 60f,
                MinValue = 1f
            }));

            intCommandType++;
            offset += 2;
            SystemParameter_1_Collection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameter_1_Collection.Count + 1,
                CommandType = intCommandType,
                CID2 = 0x8C,
                Description = "放电过流保护恢复时间(s)",
                Description_US = "DCH OC Protection Restore Time(s)",
                DefaultValue = 60F,
                SavedValue = BitConverter.ToUInt16(new byte[] { message[offset + 1], message[offset] }, 0),
                SetValue = BitConverter.ToUInt16(new byte[] { message[offset + 1], message[offset] }, 0),
                Coef = 1,
                ParameterScope = "5~300s",
                MaxValue = 300f,
                MinValue = 5f
            }));

            intCommandType++;
            offset += 2;
            SystemParameter_1_Collection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameter_1_Collection.Count + 1,
                CommandType = intCommandType,
                CID2 = 0x8C,
                Description = "短路反接保护恢复时间(s)",
                Description_US = "Short Circuit Protection Restore Time(s)",
                DefaultValue = 60F,
                SavedValue = BitConverter.ToUInt16(new byte[] { message[offset + 1], message[offset] }, 0),
                SetValue = BitConverter.ToUInt16(new byte[] { message[offset + 1], message[offset] }, 0),
                Coef = 1,
                ParameterScope = "5~300s",
                MaxValue = 300f,
                MinValue = 5f
            }));

            intCommandType++;
            offset += 2;         
            IsLoadingCoslightSystemParameter = false;
        }
        private void ParseMessage_SystemParameter_FB100C5(byte[] message)
        {
            //if (message.Length != 0x4b)
            //{
            //    return;
            //}

            int packAddress = message[2] - 1;
            int offset = 7;

            if (ProtocalProvider.BatteryGroupCount == 0xFF)
            {
                offset++;
            }

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0x80,
                CID2 = 0x49,
                Description = "单体过压告警阈值(V)",
                Description_US = "Cell OV Warning(V)",
                DefaultValue = 3.75F,
                SavedValue = BitConverter.ToUInt16(new byte[] { message[10], message[9] }, 0) / 100F,
                SetValue = BitConverter.ToUInt16(new byte[] { message[10], message[9] }, 0) / 100F,
                Coef = 100,
                ParameterScope = "2~4V",
                MaxValue = 4,
                MinValue = 2
            }));

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0x81,
                CID2 = 0x49,
                Description = "单体欠压告警阈值(V)",
                Description_US = "Cell UV Warning(V)",
                DefaultValue = 3.0F,
                SavedValue = BitConverter.ToUInt16(new byte[] { message[12], message[11] }, 0) / 100F,
                SetValue = BitConverter.ToUInt16(new byte[] { message[12], message[11] }, 0) / 100F,
                Coef = 100,
                ParameterScope = "2~4V",
                MaxValue = 4,
                MinValue = 2
            }));

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0x82,
                CID2 = 0x49,
                Description = "单体过压保护阈值(V)",
                Description_US = "Cell OV Protection(V)",
                DefaultValue = 3.85F,
                SavedValue = BitConverter.ToUInt16(new byte[] { message[14], message[13] }, 0) / 100F,
                SetValue = BitConverter.ToUInt16(new byte[] { message[14], message[13] }, 0) / 100F,
                Coef = 100,
                ParameterScope = "2~4V",
                MaxValue = 4,
                MinValue = 2
            }));

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0x83,
                CID2 = 0x49,
                Description = "单体欠压保护阈值(V)",
                Description_US = "Cell UV Protection(V)",
                DefaultValue = 2.9F,
                SavedValue = BitConverter.ToUInt16(new byte[] { message[16], message[15] }, 0) / 100F,
                SetValue = BitConverter.ToUInt16(new byte[] { message[16], message[15] }, 0) / 100F,
                Coef = 100,
                ParameterScope = "2~4V",
                MaxValue = 4,
                MinValue = 2
            }));

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0x84,
                CID2 = 0x49,
                Description = "电池组过压告警阈值(V)",
                Description_US = "Pack OV Warning(V)",
                DefaultValue = 54.6F,
                SavedValue = BitConverter.ToUInt16(new byte[] { message[18], message[17] }, 0) / 10F,
                SetValue = BitConverter.ToUInt16(new byte[] { message[18], message[17] }, 0) / 10F,
                Coef = 10,
                ParameterScope = "40~60V",
                MaxValue = 60,
                MinValue = 40
            }));

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0x85,
                CID2 = 0x49,
                Description = "电池组欠压告警阈值(V)",
                Description_US = "Pack UV Warning(V)",
                DefaultValue = 46.5F,
                SavedValue = BitConverter.ToUInt16(new byte[] { message[20], message[19] }, 0) / 10F,
                SetValue = BitConverter.ToUInt16(new byte[] { message[20], message[19] }, 0) / 10F,
                Coef = 10,
                ParameterScope = "40~60V",
                MaxValue = 60,
                MinValue = 40
            }));

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0x86,
                CID2 = 0x49,
                Description = "电池组过压保护阈值(V)",
                Description_US = "Pack OV Protection(V)",
                DefaultValue = 55F,
                SavedValue = BitConverter.ToUInt16(new byte[] { message[22], message[21] }, 0) / 10F,
                SetValue = BitConverter.ToUInt16(new byte[] { message[22], message[21] }, 0) / 10F,
                Coef = 10,
                ParameterScope = "40~60V",
                MaxValue = 60,
                MinValue = 40
            }));

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0xC4,
                CID2 = 0x49,
                Description = "电池组欠压保护阈值(V)",
                Description_US = "Pack UV Protection(V)",
                DefaultValue = 45F,
                SavedValue = BitConverter.ToUInt16(new byte[] { message[24], message[23] }, 0) / 10F,
                SetValue = BitConverter.ToUInt16(new byte[] { message[24], message[23] }, 0) / 10F,
                Coef = 10,
                ParameterScope = "40~60V",
                MaxValue = 60,
                MinValue = 40
            }));

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0xC5,
                CID2 = 0x49,
                Description = "充电温度高告警阈值(℃)",
                Description_US = "CH OT Warning(℃)",
                DefaultValue = 50F,
                SavedValue = BitConverter.ToInt16(new byte[] { message[26], message[25] }, 0) / 1F,
                SetValue = BitConverter.ToInt16(new byte[] { message[26], message[25] }, 0) / 1F,
                Coef = 1,
                ParameterScope = "25~65℃",
                MaxValue = 65,
                MinValue = 25
            }));

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0xC6,
                CID2 = 0x49,
                Description = "充电温度低告警阈值(℃)",
                Description_US = "CH UT Warning(℃)",
                DefaultValue = 5F,
                SavedValue = BitConverter.ToInt16(new byte[] { message[28], message[27] }, 0) / 1F,
                SetValue = BitConverter.ToInt16(new byte[] { message[28], message[27] }, 0) / 1F,
                Coef = 1,
                ParameterScope = "-20~25℃",
                MaxValue = 25,
                MinValue = -20
            }));

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0xC7,
                CID2 = 0x49,
                Description = "充电温度高保护阈值(℃)",
                Description_US = "CH OT Protection(℃)",
                DefaultValue = 60F,
                SavedValue = BitConverter.ToInt16(new byte[] { message[30], message[29] }, 0) / 1F,
                SetValue = BitConverter.ToInt16(new byte[] { message[30], message[29] }, 0) / 1F,
                Coef = 1,
                ParameterScope = "25~65℃",
                MaxValue = 65,
                MinValue = 25
            }));

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0xC8,
                CID2 = 0x49,
                Description = "充电温度低保护阈值(℃)",
                Description_US = "CH UT Protection(℃)",
                DefaultValue = 0F,
                SavedValue = BitConverter.ToInt16(new byte[] { message[32], message[31] }, 0) / 1F,
                SetValue = BitConverter.ToInt16(new byte[] { message[32], message[31] }, 0) / 1F,
                Coef = 1,
                ParameterScope = "-20~25℃",
                MaxValue = 25,
                MinValue = -20
            }));

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0xC9,
                CID2 = 0x49,
                Description = "放电温度高告警阈值(℃)",
                Description_US = "DCH OT Warning(℃)",
                DefaultValue = 60F,
                SavedValue = BitConverter.ToInt16(new byte[] { message[34], message[33] }, 0) / 1F,
                SetValue = BitConverter.ToInt16(new byte[] { message[34], message[33] }, 0) / 1F,
                Coef = 1,
                ParameterScope = "25~65℃",
                MaxValue = 65,
                MinValue = 25
            }));

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0xCA,
                CID2 = 0x49,
                Description = "放电温度低告警阈值(℃)",
                Description_US = "DCH UT Warning(℃)",
                DefaultValue = -15F,
                SavedValue = BitConverter.ToInt16(new byte[] { message[36], message[35] }, 0) / 1F,
                SetValue = BitConverter.ToInt16(new byte[] { message[36], message[35] }, 0) / 1F,
                Coef = 1,
                ParameterScope = "-20~25℃",
                MaxValue = 25,
                MinValue = -20
            }));

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0xCB,
                CID2 = 0x49,
                Description = "放电温度高保护阈值(℃)",
                Description_US = "DCH OT Protection(℃)",
                DefaultValue = 65F,
                SavedValue = BitConverter.ToInt16(new byte[] { message[38], message[37] }, 0) / 1F,
                SetValue = BitConverter.ToInt16(new byte[] { message[38], message[37] }, 0) / 1F,
                Coef = 1,
                ParameterScope = "25~65℃",
                MaxValue = 65,
                MinValue = 25
            }));

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0xCC,
                CID2 = 0x49,
                Description = "放电温度低保护阈值(℃)",
                Description_US = "DCH UT Protection(℃)",
                DefaultValue = -20F,
                SavedValue = BitConverter.ToInt16(new byte[] { message[40], message[39] }, 0) / 1F,
                SetValue = BitConverter.ToInt16(new byte[] { message[40], message[39] }, 0) / 1F,
                Coef = 1,
                ParameterScope = "-20~25℃",
                MaxValue = 25,
                MinValue = -20
            }));

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0xCD,
                CID2 = 0x49,
                Description = "充电过流告警阈值(A)",
                Description_US = "CH OC Warning(A)",
                DefaultValue = 105F,
                SavedValue = BitConverter.ToUInt16(new byte[] { message[42], message[41] }, 0) * CellCapacity / 1000F,
                SetValue = BitConverter.ToUInt16(new byte[] { message[42], message[41] }, 0) * CellCapacity / 1000F,
                Coef = 1000,
                ParameterScope = (0.2f * CellCapacity).ToString() + "~" + (2f * CellCapacity).ToString() + "A",
                MaxValue = 2f * CellCapacity,
                MinValue = 0.2f * CellCapacity
            }));

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0xCE,
                CID2 = 0x49,
                Description = "充电过流保护阈值(A)",
                Description_US = "CH OC Protection (A)",
                DefaultValue = 110F,
                SavedValue = BitConverter.ToUInt16(new byte[] { message[44], message[43] }, 0) * CellCapacity / 1000F,
                SetValue = BitConverter.ToUInt16(new byte[] { message[44], message[43] }, 0) * CellCapacity / 1000F,
                Coef = 1000,
                ParameterScope = (0.2f * CellCapacity).ToString() + "~" + (2f * CellCapacity).ToString() + "A",
                MaxValue = 2f * CellCapacity,
                MinValue = 0.2f * CellCapacity
            }));

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0xCF,
                CID2 = 0x49,
                Description = "放电过流保护阈值(A)",
                Description_US = "DCH OC Protection(A)",
                DefaultValue = 110F,
                SavedValue = BitConverter.ToUInt16(new byte[] { message[46], message[45] }, 0) * CellCapacity / 1000F,
                SetValue = BitConverter.ToUInt16(new byte[] { message[46], message[45] }, 0) * CellCapacity / 1000F,
                Coef = 1000,
                ParameterScope = (0.2f * CellCapacity).ToString() + "~" + (2f * CellCapacity).ToString() + "A",
                MaxValue = 2f * CellCapacity,
                MinValue = 0.2f * CellCapacity
            }));

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0xD0,
                CID2 = 0x49,
                Description = "放电过流告警阈值(A)",
                Description_US = "DCH OC Warning(A)",
                DefaultValue = 105F,
                SavedValue = BitConverter.ToUInt16(new byte[] { message[48], message[47] }, 0) * CellCapacity / 1000F,
                SetValue = BitConverter.ToUInt16(new byte[] { message[48], message[47] }, 0) * CellCapacity / 1000F,
                Coef = 1000,
                ParameterScope = (0.2f * CellCapacity).ToString() + "~" + (2f * CellCapacity).ToString() + "A",
                MaxValue = 2f * CellCapacity,
                MinValue = 0.2f * CellCapacity
            }));

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0xD1,
                CID2 = 0x49,
                Description = "电池组欠压保护恢复阈值(V)",
                Description_US = "Pack UV Protection Restore(V)",
                DefaultValue = 48F,
                SavedValue = BitConverter.ToUInt16(new byte[] { message[51], message[50] }, 0) / 100F,
                SetValue = BitConverter.ToUInt16(new byte[] { message[51], message[50] }, 0) / 100F,
                Coef = 100,
                ParameterScope = "40~60V",
                MaxValue = 60,
                MinValue = 40
            }));

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0xD2,
                CID2 = 0x49,
                Description = "单体过压保护恢复阈值(V)",
                Description_US = "Cell OV Protection Restore(V)",
                DefaultValue = 3.6F,
                SavedValue = BitConverter.ToUInt16(new byte[] { message[53], message[52] }, 0) / 100F,
                SetValue = BitConverter.ToUInt16(new byte[] { message[53], message[52] }, 0) / 100F,
                Coef = 100,
                ParameterScope = "2~4V",
                MaxValue = 4,
                MinValue = 2
            }));

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0xD3,
                CID2 = 0x49,
                Description = "单体欠压保护恢复阈值(V)",
                Description_US = "Cell UV Protection Restore(V)",
                DefaultValue = 3.2F,
                SavedValue = BitConverter.ToUInt16(new byte[] { message[55], message[54] }, 0) / 100F,
                SetValue = BitConverter.ToUInt16(new byte[] { message[55], message[54] }, 0) / 100F,
                Coef = 100,
                ParameterScope = "2~4V",
                MaxValue = 4,
                MinValue = 2
            }));

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0xD4,
                CID2 = 0x49,
                Description = "充电过温保护恢复阈值(℃)",
                Description_US = "CH OT Protection(℃)",
                DefaultValue = 57F,
                SavedValue = BitConverter.ToInt16(new byte[] { message[57], message[56] }, 0) / 100F,
                SetValue = BitConverter.ToInt16(new byte[] { message[57], message[56] }, 0) / 100F,
                Coef = 100,
                ParameterScope = "25~65℃",
                MaxValue = 65,
                MinValue = 25
            }));

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0xD5,
                CID2 = 0x49,
                Description = "放电过温保护恢复阈值(℃)",
                Description_US = "CH OT Protection(℃)",
                DefaultValue = 62F,
                SavedValue = BitConverter.ToInt16(new byte[] { message[59], message[58] }, 0) / 100F,
                SetValue = BitConverter.ToInt16(new byte[] { message[59], message[58] }, 0) / 100F,
                Coef = 100,
                ParameterScope = "25~65℃",
                MaxValue = 65,
                MinValue = 25
            }));

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0xD6,
                CID2 = 0x49,
                Description = "充电低温保护恢复阈值(℃)",
                Description_US = "CH UT Protection(℃)",
                DefaultValue = 3F,
                SavedValue = BitConverter.ToInt16(new byte[] { message[61], message[60] }, 0) / 100F,
                SetValue = BitConverter.ToInt16(new byte[] { message[61], message[60] }, 0) / 100F,
                Coef = 100,
                ParameterScope = "-20~25℃",
                MaxValue = 25,
                MinValue = -20
            }));

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0xD7,
                CID2 = 0x49,
                Description = "放电低温保护恢复阈值(℃)",
                Description_US = "DCH UT Protection(℃)",
                DefaultValue = -17F,
                SavedValue = BitConverter.ToInt16(new byte[] { message[63], message[62] }, 0) / 100F,
                SetValue = BitConverter.ToInt16(new byte[] { message[63], message[62] }, 0) / 100F,
                Coef = 100,
                ParameterScope = "-20~25℃",
                MaxValue = 25,
                MinValue = -20
            }));

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0xD8,
                CID2 = 0x49,
                Description = "电芯一致性差告警电压差阈值(V)",
                Description_US = "Cell Behind Volt Diff Warning(V)",
                DefaultValue = 0.4F,
                SavedValue = BitConverter.ToUInt16(new byte[] { message[65], message[64] }, 0) / 100F,
                SetValue = BitConverter.ToUInt16(new byte[] { message[65], message[64] }, 0) / 100F,
                Coef = 100,
                ParameterScope = "0.05~1.5V",
                MaxValue = 1.5f,
                MinValue = 0.05f
            }));

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0xD9,
                CID2 = 0x49,
                Description = "环境温度高告警阈值(℃)",
                Description_US = "Ambient Temperature High Warning",
                DefaultValue = 55F,
                SavedValue = BitConverter.ToInt16(new byte[] { message[67], message[66] }, 0) / 1F,
                SetValue = BitConverter.ToInt16(new byte[] { message[67], message[66] }, 0) / 1F,
                Coef = 1,
                ParameterScope = "25~65℃",
                MaxValue = 65,
                MinValue = 25
            }));

            SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
            {
                SystemParameterID = SystemParameterCollection.Count + 1,
                CommandType = 0xDA,
                CID2 = 0x49,
                Description = "环境温度低告警阈值(℃)",
                Description_US = "Ambient Temperature Low Warning",
                DefaultValue = 0F,
                SavedValue = BitConverter.ToInt16(new byte[] { message[69], message[68] }, 0) / 1F,
                SetValue = BitConverter.ToInt16(new byte[] { message[69], message[68] }, 0) / 1F,
                Coef = 1,
                ParameterScope = "-20~25℃",
                MaxValue = 25,
                MinValue = -20
            }));
            if (SerialPortCommunicator.ManufacturerName == "ZTE-C" && SerialPortCommunicator.ZTE_Type != "FB100C5")
            {
                SystemParameterCollection.Add(ViewModelSource.Create(() => new Models.SystemParameter
                {
                    SystemParameterID = SystemParameterCollection.Count + 1,
                    CommandType = 0xDB,
                    CID2 = 0x49,
                    Description = "防盗开启时间(min)",
                    Description_US = "Anti-Theft Start Time(min)",
                    DefaultValue = 480F,
                    SavedValue = BitConverter.ToUInt16(new byte[] { message[71], message[70] }, 0) / 1F,
                    SetValue = BitConverter.ToUInt16(new byte[] { message[71], message[70] }, 0) / 1F,
                    Coef = 1,
                    ParameterScope = "≤1440min",
                    MaxValue = 1440f,
                    MinValue = 0f
                }));
            }

            IsLoadingSystemParameter = false;
        }
        private void ParseMessage_DeviceRunningRecord_DPC(byte[] message)
        {
            if (message.Length != 0x4E)
            {
                    return;
                }

            SerialPortCommunicator.ReadingDeviceRunningDataCounter = 0;

            int offset = 7;

            //创建 ViewModel 实例，并且 自动为 ViewModel 添加 INotifyPropertyChanged 机制。
            DeviceRunningRecord_DPC record = ViewModelSource.Create<DeviceRunningRecord_DPC>();

            record.DeviceRunningRecordID = message[offset];
            offset += 2;

            int year = BitConverter.ToUInt16(new byte[] { message[offset + 1], message[offset] }, 0);

            DateTime SaveTime = new DateTime(year, message[offset + 2], message[offset + 3], message[offset + 4], message[offset + 5], message[offset + 6]);
            record.SaveTime = SaveTime;
            offset += 7 + 2;
            byte[] byteTemp = creatSwitchParameterID_DPC(message[offset]); offset++; record.CellStateEvent = "";
            for (int t = 0; t < byteTemp.Length; t++) { if (byteTemp[t] != 0) { record.CellStateEvent += (CellStateEvent_DPC)t; } }
            byteTemp = creatSwitchParameterID_DPC(message[offset]); offset++; record.CellVoltageEvent = "";
            for (int t = 0; t < byteTemp.Length; t++) { if (byteTemp[t] != 0) { record.CellVoltageEvent += (VoltageEvent_DPC)t; } }
            byteTemp = creatSwitchParameterID_DPC(message[offset]); offset++; record.TemperatureEvent = "";
            for (int t = 0; t < byteTemp.Length; t++) { if (byteTemp[t] != 0) { record.TemperatureEvent += (TemperatureEvent_DPC)(t + 8); } }
            byteTemp = creatSwitchParameterID_DPC(message[offset]); offset++;
            for (int t = 0; t < byteTemp.Length; t++) { if (byteTemp[t] != 0) { record.TemperatureEvent += (TemperatureEvent_DPC)t; } }
            byteTemp = creatSwitchParameterID_DPC(message[offset]); offset++; record.CurrentEvent = "";
            for (int t = 0; t < byteTemp.Length; t++) { if (byteTemp[t] != 0) { record.CurrentEvent += (CurrentEvent_DPC)t; } }
            byteTemp = creatSwitchParameterID_DPC(message[offset]); offset++; record.CapacityEvent = "";
            for (int t = 0; t < byteTemp.Length; t++) { if (byteTemp[t] != 0) { record.CapacityEvent += (CapacityEvent_DPC)t; } }

            int iii = BitConverter.ToInt16(new byte[] { message[offset + 1], message[offset] }, 0);
            offset++;

            record.BatteryCurrent = BitConverter.ToInt16(new byte[] { message[offset + 1], message[offset] }, 0) * 0.01F; offset += 2;
            record.BatteryVoltage = BitConverter.ToInt16(new byte[] { message[offset + 1], message[offset] }, 0) * 0.01F; offset += 2;
            record.ResidualCapacity = BitConverter.ToInt16(new byte[] { message[offset + 1], message[offset] }, 0) * 0.01F; offset += 2;

            record.Temp_01 = BitConverter.ToInt16(new byte[] { message[offset + 1], message[offset] }, 0) * 0.01F;
            offset += 2;
            record.Temp_02 = BitConverter.ToInt16(new byte[] { message[offset + 1], message[offset] }, 0) * 0.01F;
            offset += 2;
            record.Temp_03 = BitConverter.ToInt16(new byte[] { message[offset + 1], message[offset] }, 0) * 0.01F;
            offset += 2;
            record.Temp_04 = BitConverter.ToInt16(new byte[] { message[offset + 1], message[offset] }, 0) * 0.01F;
            offset += 2;

            record.AmbinentTemperature = BitConverter.ToInt16(new byte[] { message[offset + 1], message[offset] }, 0) * 0.01F;
            offset += 2;
            record.PowerTemperature = BitConverter.ToInt16(new byte[] { message[offset + 1], message[offset] }, 0) * 0.01F;
            offset += 2;

            record.Cell_01_Voltage = BitConverter.ToInt16(new byte[] { message[offset + 1], message[offset] }, 0) * 0.001F;
            offset += 2;
            record.Cell_02_Voltage = BitConverter.ToInt16(new byte[] { message[offset + 1], message[offset] }, 0) * 0.001F;
            offset += 2;
            record.Cell_03_Voltage = BitConverter.ToInt16(new byte[] { message[offset + 1], message[offset] }, 0) * 0.001F;
            offset += 2;
            record.Cell_04_Voltage = BitConverter.ToInt16(new byte[] { message[offset + 1], message[offset] }, 0) * 0.001F;
            offset += 2;
            record.Cell_05_Voltage = BitConverter.ToInt16(new byte[] { message[offset + 1], message[offset] }, 0) * 0.001F;
            offset += 2;
            record.Cell_06_Voltage = BitConverter.ToInt16(new byte[] { message[offset + 1], message[offset] }, 0) * 0.001F;
            offset += 2;
            record.Cell_07_Voltage = BitConverter.ToInt16(new byte[] { message[offset + 1], message[offset] }, 0) * 0.001F;
            offset += 2;
            record.Cell_08_Voltage = BitConverter.ToInt16(new byte[] { message[offset + 1], message[offset] }, 0) * 0.001F;
            offset += 2;
            record.Cell_09_Voltage = BitConverter.ToInt16(new byte[] { message[offset + 1], message[offset] }, 0) * 0.001F;
            offset += 2;
            record.Cell_10_Voltage = BitConverter.ToInt16(new byte[] { message[offset + 1], message[offset] }, 0) * 0.001F;
            offset += 2;
            record.Cell_11_Voltage = BitConverter.ToInt16(new byte[] { message[offset + 1], message[offset] }, 0) * 0.001F;
            offset += 2;
            record.Cell_12_Voltage = BitConverter.ToInt16(new byte[] { message[offset + 1], message[offset] }, 0) * 0.001F;
            offset += 2;
            record.Cell_13_Voltage = BitConverter.ToInt16(new byte[] { message[offset + 1], message[offset] }, 0) * 0.001F;
            offset += 2;
            record.Cell_14_Voltage = BitConverter.ToInt16(new byte[] { message[offset + 1], message[offset] }, 0) * 0.001F;
            offset += 2;
            record.Cell_15_Voltage = BitConverter.ToInt16(new byte[] { message[offset + 1], message[offset] }, 0) * 0.001F;
            offset += 2;
            record.Cell_16_Voltage = BitConverter.ToInt16(new byte[] { message[offset + 1], message[offset] }, 0) * 0.001F;
            offset += 2;

            DeviceRunningRecords.Add(record);  

            if (SerialPortCommunicator.IsReadingDeviceRunningData == 0)
            {
                    return;
                }
            if (message[8] == 0x00)
            {
                communicator.UserAction(ProtocalProvider.GetMessage_ReadDeviceRunningRecord_DPC(message[2], 0x01));
            }
            else if (message[8] == 0x01)
            {
                communicator.UserAction(ProtocalProvider.GetMessage_ReadDeviceRunningRecord_DPC(message[2], 0x03));
                SerialPortCommunicator.IsReadingDeviceRunningData = 0;
            }
        }
        private void ParseMessage_DeviceWarningRecord_DPC(byte[] message)
        {
            SerialPortCommunicator.ReadingDeviceWarningDataCounter = 0;
            try
            {
                DeviceWarningRecord_COSPOWERS record = new DeviceWarningRecord_COSPOWERS();

                record.DeviceWarningRecordID = DeviceWarningRecords_COSPOWERS.Count + 1;
                record.SaveTime = new DateTime(DateTime.Now.Year, message[8 + 1], message[9 + 1], message[10 + 1], message[11 + 1], message[12 + 1]);

                record.Warnings = "";

                if (message[13 + 2 + 1] != 0)
                {
                    #region 13+2
                    if ((message[13 + 2 + 1] & 0x01) == 0x01) { record.Warnings += IsChineseUI ? "单体过压告警；" : "Cell OV Warning;"; }

                    if ((message[13 + 2 + 1] & 0x02) == 0x02) { record.Warnings += IsChineseUI ? "单体过压保护；" : "Cell OV Protection;"; }

                    if ((message[13 + 2 + 1] & 0x04) == 0x04) { record.Warnings += IsChineseUI ? "单体欠压告警；" : "Cell BV Warning;"; }

                    if ((message[13 + 2 + 1] & 0x08) == 0x08) { record.Warnings += IsChineseUI ? "单体欠压保护；" : "Cell BV Protection;"; }

                    if ((message[13 + 2 + 1] & 0x10) == 0x10) { record.Warnings += IsChineseUI ? "电池组过压告警；" : "Pack OV Warning;"; }

                    if ((message[13 + 2 + 1] & 0x20) == 0x20) { record.Warnings += IsChineseUI ? "电池组过压保护；" : "Pack BV Protection;"; }

                    if ((message[13 + 2 + 1] & 0x40) == 0x40) { record.Warnings += IsChineseUI ? "电池组欠压告警；" : "Pack OV Warning;"; }

                    if ((message[13 + 2 + 1] & 0x80) == 0x80) { record.Warnings += IsChineseUI ? "电池组欠压保护；" : "Pack BV Protection;"; }
                    #endregion
                }

                if (message[14 + 2 + 1] != 0)
                {
                    #region 14+2
                    if ((message[14 + 2 + 1] & 0x01) == 0x01) { record.Warnings += IsChineseUI ? "单体充电高温告警；" : "CH OT Warning;"; }

                    if ((message[14 + 2 + 1] & 0x02) == 0x02) { record.Warnings += IsChineseUI ? "单体充电高温保护；" : "CH OT Protection;"; }

                    if ((message[14 + 2 + 1] & 0x04) == 0x04) { record.Warnings += IsChineseUI ? "单体充电低温告警；" : "CH BT Warning;"; }

                    if ((message[14 + 2 + 1] & 0x08) == 0x08) { record.Warnings += IsChineseUI ? "单体充电低温保护；" : "CH BT Protection;"; }

                    if ((message[14 + 2 + 1] & 0x10) == 0x10) { record.Warnings += IsChineseUI ? "单体放电高温告警；" : "DCH OT Warning;"; }

                    if ((message[14 + 2 + 1] & 0x20) == 0x20) { record.Warnings += IsChineseUI ? "单体放电高温保护；" : "DCH OT Protection;"; }

                    if ((message[14 + 2 + 1] & 0x40) == 0x40) { record.Warnings += IsChineseUI ? "单体放电低温告警；" : "DCH BT Warning;"; }

                    if ((message[14 + 2 + 1] & 0x80) == 0x80) { record.Warnings += IsChineseUI ? "单体放电低温保护；" : "DCH BT Protection;"; }
                    #endregion
                }

                if (message[15 + 2 + 1] != 0)
                {
                    #region 15+2
                    if ((message[15 + 2 + 1] & 0x01) == 0x01) { record.Warnings += IsChineseUI ? "电池组充电过流告警；" : "Pack CH OC Warning;"; }

                    if ((message[15 + 2 + 1] & 0x02) == 0x02) { record.Warnings += IsChineseUI ? "电池组充电过流保护；" : "Pack CH OC Protection;"; }

                    if ((message[15 + 2 + 1] & 0x04) == 0x04) { record.Warnings += IsChineseUI ? "电池组充电过流保护2级；" : "Pack CH OC Protection 2 Level;"; }

                    if ((message[15 + 2 + 1] & 0x08) == 0x08) { record.Warnings += IsChineseUI ? "电池组放电过流告警；" : "Pack DCH OC Warning;"; }

                    if ((message[15 + 2 + 1] & 0x10) == 0x10) { record.Warnings += IsChineseUI ? "电池组放电过流保护；" : "Pack DCH OC Protection;"; }

                    if ((message[15 + 2 + 1] & 0x20) == 0x20) { record.Warnings += IsChineseUI ? "电池组放电过流保护2级；" : "Pack DCH OC Protection 2 Level;"; }

                    if ((message[15 + 2 + 1] & 0x40) == 0x40) { record.Warnings += IsChineseUI ? "电池组短路保护或极性反接保护；" : "Pack Short Circuit;"; }
                    if ((message[15 + 2 + 1] & 0x80) == 0x80)
                    {
                        record.Warnings += IsChineseUI ? "防盗告警；" : "Anti-Theft Warning;";
                    }
                    #endregion
                }

                if (message[16 + 2 + 1] != 0)
                {
                    #region 16+2
                    if ((message[16 + 2 + 1] & 0x01) == 0x01) { record.Warnings += IsChineseUI ? "电芯损坏告警；" : "Cell Damage Warning;"; }

                    if ((message[16 + 2 + 1] & 0x02) == 0x02) { record.Warnings += IsChineseUI ? "温度传感器无效告警；" : "Temperature Sensor Failure;"; }

                    if ((message[16 + 2 + 1] & 0x04) == 0x04) { record.Warnings += IsChineseUI ? "充电回路开关失效告警；" : "CH Switch Failure;"; }

                    if ((message[16 + 2 + 1] & 0x08) == 0x08) { record.Warnings += IsChineseUI ? "放电回路开关失效告警；" : "DCH Switch Failure;"; }

                    if ((message[16 + 2 + 1] & 0x10) == 0x10) { record.Warnings += IsChineseUI ? "单体电压落后告警；" : "Cell Voltage Behind Warning;"; }

                    if ((message[16 + 2 + 1] & 0x20) == 0x20) { record.Warnings += IsChineseUI ? "单体电压落后保护；" : "Cell Voltage Behind Protection;"; }

                    if ((message[16 + 2 + 1] & 0x40) == 0x40) { record.Warnings += IsChineseUI ? "环境温度高告警；" : "Ambient Temperature High Warning;"; }

                    if ((message[16 + 2 + 1] & 0x80) == 0x80) { record.Warnings += IsChineseUI ? "环境温度低告警；" : "Ambient Temperature Low Warning;"; }
                    #endregion
                }

                if (message[17 + 2 + 1] != 0)
                {
                    #region 17+2
                    if ((message[17 + 2 + 1] & 0x01) == 0x01) { record.Warnings += IsChineseUI ? "SOC低告警；" : "SOC Low Warning;"; }

                    if ((message[17 + 2 + 1] & 0x02) == 0x02) { record.Warnings += IsChineseUI ? "SOC低保护；" : "SOC Low Protection;"; }

                    if ((message[17 + 2 + 1] & 0x04) == 0x04) { record.Warnings += IsChineseUI ? "SOH低告警；" : "SOH Low Warning;"; }

                    if ((message[17 + 2 + 1] & 0x08) == 0x08) { record.Warnings += IsChineseUI ? "SOH低保护；" : "SOH Low Protection;"; }

                    if ((message[17 + 2 + 1] & 0x10) == 0x10) { record.Warnings += IsChineseUI ? " MOS充电过温告警；" : "MOS CH OT Warning;"; }

                    if ((message[17 + 2 + 1] & 0x40) == 0x20) { record.Warnings += IsChineseUI ? " MOS放电过温告警；" : "MOS DCH OT Warning;"; }

                    if ((message[17 + 2 + 1] & 0x20) == 0x40) { record.Warnings += IsChineseUI ? "MOS充电过温保护；" : "MOS CH OT Protection;"; }

                    if ((message[17 + 2 + 1] & 0x80) == 0x80) { record.Warnings += IsChineseUI ? "MOS放电过温保护；" : "MOS DCH OT Protection;"; }


                    #endregion
                }

                if (message[18 + 2 + 1] != 0)
                {
                    #region 18+2
                    if ((message[18 + 2 + 1] & 0x01) == 0x01) { record.Warnings += IsChineseUI ? "电压采样线失效告警；" : "Voltage Sampling Line Failure;"; }
                    #endregion
                }
                DeviceWarningRecords_COSPOWERS.Add(record);
            }
            catch { }

            if (SerialPortCommunicator.IsReadingDeviceWarningData == 0)
            {
                    return;
                }
            if (message[8] == 0x00)
            {
                communicator.UserAction(ProtocalProvider.GetMessage_ReadDeviceWarningRecord(message[2], 0x01));
            }
            else if (message[8] == 0x01)
            {
                communicator.UserAction(ProtocalProvider.GetMessage_ReadDeviceWarningRecord(message[2], 0x03));
                SerialPortCommunicator.IsReadingDeviceWarningData = 0;
            }
        }
        //写日志
        private void WriteLog(byte[] message)
        {
            bool Writeflag = false;

            string LogPath = AppDomain.CurrentDomain.BaseDirectory + "Log\\";

            switch (SerialPortCommunicator.LastSendCommandType)
            {
                case CommandType.读取模拟量:

                    LogPath += "模拟量.txt";
                    Writeflag = true;
                    break;

                case CommandType.读取光宇模拟量:

                    LogPath += "光宇模拟量.txt";
                    Writeflag = true;
                    break;

                case CommandType.读取告警状态:

                    LogPath += "告警状态.txt";
                    Writeflag = true;
                    break;

                case CommandType.读取光宇告警状态:

                    LogPath += "光宇告警状态.txt";
                    Writeflag = true;
                    break;

                case CommandType.读取开关量:

                    LogPath += "开关量.txt";
                    Writeflag = true;
                    break;

                case CommandType.读取光宇开关量:

                    LogPath += "光宇开关量.txt";
                    Writeflag = true;
                    break;

                case CommandType.读取设备存储运行数据:

                    LogPath += "设备存储运行数据.txt";
                    Writeflag = true;
                    break;
            }

            if (Writeflag)
            {
                StringBuilder sb = new StringBuilder();

                using (FileStream fs = new FileStream(LogPath, FileMode.Append, FileAccess.Write))
                {
                    StreamWriter sw = new StreamWriter(fs);

                    sb.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                    for (int i = 0; i != message.Length; i++)
                    {
                        sb.Append(" " + message[i].ToString("X2"));
                    }

                    sw.Write(sb.ToString());

                    sw.Flush();

                    sw.Close();
                    fs.Close();
                }
            }
        }
        /*接收事件处理
         * message：16进制
        */
        private void communicator_ReceiveEvent(object sender, byte[] message)
        {
            if (message == null)
            {
                MessageBoxService?.ShowMessage("接收到的消息为空");
                return; // 处理 null 情况
            }
            try
            {              
                if (message[4] != 0x00)
                {
                    return;
                }
                //DPC通讯
                if (communicationType == CommunicationType.DPC)
                {
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        if (message[5] == 0 && message[6] == 0)//INFO信息长度为0时，返回信息对应获取版本号命令
                        {
                            SerialPortCommunicator.CommunicationProtocoVersionNumber = message[1];
                        }
                        else if (message[5] == 0x20 && message[6] == 0x0E)//获取时间的响应信息LENID=0EH
                        {
                            this.PackCollection[message[2] - 1].BMSTime = new DateTime(BitConverter.ToUInt16(new byte[] { message[8], message[7] }, 0), message[9], message[10], message[11], message[12], message[13]);
                        }
                        else if (SerialPortCommunicator.LastSendCommandType == CommandType.获取厂家信息)
                        {
                            //解析获取厂家信息的响应信息
                            ParseMessage_DeviceManufactureInfo_DPC(message);
                        }
                        else if (SerialPortCommunicator.LastSendCommandType == CommandType.读取模拟量)
                        {
                            MessageBoxService.Show("开始解析模拟量数据");
                            if (this.PackCollection[message[8] - 1].IsCommunicationEnabled)
                            {
                                this.PackCollection[message[8] - 1].CommunicationFailureCount = 0;
                                this.PackCollection[message[8] - 1].CommunicationStatus = IsChineseUI ? CommunicationStatus.通讯正常 : CommunicationStatus.OK;//20200429
                                this.PackCollection[message[8] - 1].RefreshTime = DateTime.Now;

                            }
                            ParseMessage_AnalogData_DPC(message);
                            MessageBoxService.Show("解析完成");
                        }
                        else if (SerialPortCommunicator.LastSendCommandType == CommandType.读取告警状态)
                        {
                            if (this.PackCollection[message[8] - 1].IsCommunicationEnabled)
                            {
                                this.PackCollection[message[8] - 1].CommunicationFailureCount = 0;
                                this.PackCollection[message[8] - 1].CommunicationStatus = IsChineseUI ? CommunicationStatus.通讯正常 : CommunicationStatus.OK;//20200429
                                this.PackCollection[message[8] - 1].RefreshTime = DateTime.Now;
                            }
                            ParseMessage_WarningData_DPC(message);
                        }
                        else if (SerialPortCommunicator.LastSendCommandType == CommandType.读取系统参数)
                        {
                            ParseMessage_SystemParameter_DPC(message);
                        }
                        else if (SerialPortCommunicator.LastSendCommandType == CommandType.读取设备存储运行数据)
                        {
                            ParseMessage_DeviceRunningRecord_DPC(message);
                        }
                        else if (SerialPortCommunicator.LastSendCommandType == CommandType.读取设备存储告警数据)
                        {
                            ParseMessage_DeviceWarningRecord_DPC(message);
                        }

                        WriteLog(message);
                    });
                }
                else
                {
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        if (this.PackCollection[message[2] - 1].IsCommunicationEnabled)
                        {
                            this.PackCollection[message[2] - 1].CommunicationFailureCount = 0;
                            this.PackCollection[message[2] - 1].CommunicationStatus = IsChineseUI ? CommunicationStatus.通讯正常 : CommunicationStatus.OK;//20200429
                            this.PackCollection[message[2] - 1].RefreshTime = DateTime.Now;
                        }

                        if (SerialPortCommunicator.LastSendCommandType == CommandType.获取厂家信息)
                        {
                            ParseMessage_DeviceManufactureInfo(message);
                        }
                        else if (SerialPortCommunicator.LastSendCommandType == CommandType.读取模拟量)
                        {
                            ParseMessage_AnalogData(message);
                        }
                        else if (SerialPortCommunicator.LastSendCommandType == CommandType.读取光宇模拟量)
                        {
                            if (SerialPortCommunicator.ManufacturerName == "ZTE-C" && SerialPortCommunicator.ZTE_Type == "FB150C")//20200522//20200601
                            {
                                ParseMessage_AnalogData_Coslight(message);
                            }
                            else
                            {
                                ParseMessage_AnalogData_Coslight_FB100C5(message);
                            }
                        }
                        else if (SerialPortCommunicator.LastSendCommandType == CommandType.读取开关量)
                        {
                            ParseMessage_StatusData(message);
                        }
                        else if (SerialPortCommunicator.LastSendCommandType == CommandType.读取光宇开关量)
                        {
                            ParseMessage_StatusData_Coslight(message);
                        }
                        else if (SerialPortCommunicator.LastSendCommandType == CommandType.读取告警状态)
                        {
                            ParseMessage_WarningData(message);
                        }
                        else if (SerialPortCommunicator.LastSendCommandType == CommandType.读取光宇告警状态)
                        {
                            ParseMessage_WarningData_Coslight(message);
                        }
                        else if (SerialPortCommunicator.LastSendCommandType == CommandType.读取系统参数)
                        {
                            if (SerialPortCommunicator.ManufacturerName == "ZTE-C" && SerialPortCommunicator.ZTE_Type == "FB150C")//20200522//20200601
                            {
                                ParseMessage_SystemParameter(message);
                            }
                            else
                            {
                                ParseMessage_SystemParameter_FB100C5(message);
                            }
                        }
                        else if (SerialPortCommunicator.LastSendCommandType == CommandType.读取光宇系统参数)
                        {
                            if (SerialPortCommunicator.ManufacturerName == "ZTE-C" && SerialPortCommunicator.ZTE_Type == "FB150C")//20200522//20200601
                            {
                                ParseMessage_SystemParameter_Coslight(message);
                            }
                            else
                            {
                                ParseMessage_SystemParameter_Coslight_FB100C5(message);
                            }
                        }
                        else if (SerialPortCommunicator.LastSendCommandType == CommandType.读取系统特殊参数)
                        {
                            ParseMessage_SystemSpecialParameter(message);
                        }
                        else if (SerialPortCommunicator.LastSendCommandType == CommandType.读取光宇特殊参数)
                        {
                            ParseMessage_SystemSpecialParameter_Coslight(message);
                        }
                        else if (SerialPortCommunicator.LastSendCommandType == CommandType.读取设备存储运行数据)
                        {
                            ParseMessage_DeviceRunningRecord(message);//空函数
                        }
                        else if (SerialPortCommunicator.LastSendCommandType == CommandType.读取设备存储告警数据)
                        {
                            ParseMessage_DeviceWarningRecord(message);
                        }
                        else if (SerialPortCommunicator.LastSendCommandType == CommandType.读取设备存储事件数据)
                        {
                            ParseMessage_DeviceEventRecord(message);
                        }
                        else if (SerialPortCommunicator.LastSendCommandType == CommandType.读取干接点使能状态)
                        {
                            if (SerialPortCommunicator.ManufacturerName == "ZTE-C" && SerialPortCommunicator.ZTE_Type == "FB100C5")//20200522
                            {
                                ParseMessage_DryContactStatus_FB100C5(message);
                            }
                            else
                            {
                                ParseMessage_DryContactStatus(message);
                            }
                        }
                        else if (SerialPortCommunicator.LastSendCommandType == CommandType.读取干接点2状态)//20200522
                        {
                            ParseMessage_DryContact2Status_FB100C5(message);
                        }
                        else if (SerialPortCommunicator.LastSendCommandType == CommandType.设置产品序列号)
                        {
                            communicator.UserAction(ProtocalProvider.GetMessage_ManufacturerInfo(SelectedPackAddress));
                        }
                        else if (SerialPortCommunicator.LastSendCommandType == CommandType.读取4G参数)//20200426
                        {
                            ParseMessage_4GParameter_Coslight(message);
                        }
                        else if (SerialPortCommunicator.LastSendCommandType == CommandType.读取GPS参数)//20200426
                        {
                            ParseMessage_GPSParameter_Coslight(message);
                        }
                        else if (SerialPortCommunicator.LastSendCommandType == CommandType.读取GPS实时数据)//20200520
                        {
                            ParseMessage_GPSAnolog_Coslight(message);
                        }
                        else if (SerialPortCommunicator.LastSendCommandType == CommandType.读取4G_GPS特殊参数)//20200430
                        {
                            ParseMessage_4G_GPSSpecialParameter_Coslight(message);
                        }
                        else if (SerialPortCommunicator.LastSendCommandType == CommandType.应用程序连接)//20200502
                        {
                            ParseMessage_BootLoaderApplicationConnection_Coslight(message);
                        }
                        else if (SerialPortCommunicator.LastSendCommandType == CommandType.BootLoader连接)//20200502
                        {
                            ParseMessage_BootLoaderConnection_Coslight(message);
                        }
                        else if (SerialPortCommunicator.LastSendCommandType == CommandType.设置BootLoader文件名)//20200502
                        {
                            ParseMessage_BootLoaderWriteFileName_Coslight(message);
                        }
                        else if (SerialPortCommunicator.LastSendCommandType == CommandType.设置BootLoader参数)//20200504
                        {
                            ParseMessage_BootLoaderSet_Coslight(message);
                        }
                        else if (SerialPortCommunicator.LastSendCommandType == CommandType.设置BootLoader条数)//20200701
                        {
                            ParseMessage_BootLoaderNumberSet_Coslight(message);
                        }
                        else if (SerialPortCommunicator.LastSendCommandType == CommandType.BootLoader擦除)//20200609
                        {
                            ParseMessage_BootLoaderErases_Coslight(message);
                        }
                        else if (SerialPortCommunicator.LastSendCommandType == CommandType.读取陀螺仪实时数据)//20200520
                        {
                            ParseMessage_GyroSensorAnolog_Coslight(message);
                        }
                        else if (SerialPortCommunicator.LastSendCommandType == CommandType.读取设备信息)
                        {
                            ParseMessage_ManufacturerInfo_Coslight(message);
                        }
                        else if (SerialPortCommunicator.LastSendCommandType == CommandType.读取防盗状态)
                        {
                            ParseMessage_AntiTheftAnolog_Coslight(message);
                        }
                        else if (SerialPortCommunicator.LastSendCommandType == CommandType.读取防盗参数)
                        {
                            ParseMessage_AntiTheftParameter_Coslight(message);
                        }

                        WriteLog(message);

                    });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //接收失败事件处理
        private void Communicator_ReceiveFailureEvent(object sender, int deviceAddress)
        {
            try
            {
                App.Current.Dispatcher.Invoke(() =>
                {
                    if (this.PackCollection[deviceAddress - 1].IsCommunicationEnabled)
                    {
                        this.PackCollection[deviceAddress - 1].CommunicationFailureCount++;

                        if (this.PackCollection[deviceAddress - 1].CommunicationFailureCount >= 10 * 2)//20200422
                        {
                            this.PackCollection[deviceAddress - 1].CommunicationFailureCount = 10 * 2;//20200422

                            this.PackCollection[deviceAddress - 1].CommunicationStatus = IsChineseUI ? CommunicationStatus.通讯中断 : CommunicationStatus.Interrupt;
                        }
                    }

                    if (currentView == "SystemSetGPS_4GView" && IsReadParameter && (WlanParameterCollection.Count == 0))//20200426
                    {
                        IsReadParameter = false;

                        if (IsChineseUI)
                        {
                            MessageBoxService.Show("读取参数失败 !");
                        }

                        else
                        {
                            MessageBoxService.Show("Read Parameter Failed !");
                        }
                    }

                    if (currentView == "SystemSetView" && IsReadParameter && SystemParameterCollection.Count == 0)
                    {
                        IsReadParameter = false;

                        if (IsChineseUI)
                        {
                            MessageBoxService.Show("读取参数失败 !");
                        }

                        else
                        {
                            MessageBoxService.Show("Read Parameter Failed !");
                        }
                    }

                    if (currentView == "DryContactSetView" && IsReadParameter && DryContactCollection.Count == 0)
                    {
                        IsReadParameter = false;

                        if (IsChineseUI)
                        {
                            MessageBoxService.Show("读取参数失败 !");
                        }

                        else
                        {
                            MessageBoxService.Show("Read Parameter Failed !");
                        }
                    }

                    if (currentView == "DryContact2SetView" && IsReadParameter && IsLoadingSpecialSystemParameter)//20200522
                    {
                        IsReadParameter = false;
                        IsLoadingSpecialSystemParameter = false;

                        if (IsChineseUI)
                        {
                            MessageBoxService.Show("读取参数失败 !");
                        }

                        else
                        {
                            MessageBoxService.Show("Read Parameter Failed !");
                        }
                    }

                    if (currentView == "EnableControlView" && IsReadParameter && EnableControlCollection.Count == 0)
                    {
                        IsReadParameter = false;

                        if (IsChineseUI)
                        {
                            MessageBoxService.Show("读取参数失败 !");
                        }

                        else
                        {
                            MessageBoxService.Show("Read Parameter Failed !");
                        }
                    }
                });
            }
            catch { }
        }
        private void ParseMessage_StatusData(byte[] message)
        {
            if (message.Length != 0x15)
            {
                return;
            }

            int offset = 9;           
            int packAddress = message[2] - 1;
            int enumValue = 0;

            if (ProtocalProvider.BatteryGroupCount == 0xFF)
            {
                PackCollection[packAddress].BatteryGroupCount = message[offset++];
            }
            else
            {
                PackCollection[packAddress].BatteryGroupCount = 1;
            }

            if (PackCollection[packAddress].BatteryGroupCollection.Count == PackCollection[packAddress].BatteryGroupCount)
            {
                for (int i = 0; i != PackCollection[packAddress].BatteryGroupCount; i++)//解析各个电池组数据
                {
                    PackCollection[packAddress].BatteryGroupCollection[i].SwitchCount = message[offset++];

                    enumValue = message[offset++];
                    PackCollection[packAddress].BatteryGroupCollection[i].ChargeCircuitSwitchStatus = Enum.IsDefined(typeof(SwitchStatus), enumValue) ? (SwitchStatus)enumValue : SwitchStatus.未定义;

                    if (IsEnglishUI)
                    {
                        PackCollection[packAddress].BatteryGroupCollection[i].ChargeCircuitSwitchStatus = PackCollection[packAddress].BatteryGroupCollection[i].ChargeCircuitSwitchStatus + 3;
                    }

                    enumValue = message[offset++];
                    PackCollection[packAddress].BatteryGroupCollection[i].DischargeCircuitSwitchStatus = Enum.IsDefined(typeof(SwitchStatus), enumValue) ? (SwitchStatus)enumValue : SwitchStatus.未定义;

                    if (IsEnglishUI)
                    {
                        PackCollection[packAddress].BatteryGroupCollection[i].DischargeCircuitSwitchStatus = PackCollection[packAddress].BatteryGroupCollection[i].DischargeCircuitSwitchStatus + 3;
                    }

                    enumValue = message[offset++];
                    PackCollection[packAddress].BatteryGroupCollection[i].BeeperSwitchStatus = Enum.IsDefined(typeof(SwitchStatus), enumValue) ? (SwitchStatus)enumValue : SwitchStatus.未定义;

                    if (IsEnglishUI)
                    {
                        PackCollection[packAddress].BatteryGroupCollection[i].BeeperSwitchStatus = PackCollection[packAddress].BatteryGroupCollection[i].BeeperSwitchStatus + 3;
                    }

                    enumValue = message[offset++];
                    PackCollection[packAddress].BatteryGroupCollection[i].HeatingFilmSwitchStatus = Enum.IsDefined(typeof(SwitchStatus), enumValue) ? (SwitchStatus)enumValue : SwitchStatus.未定义;

                    if (IsEnglishUI)
                    {
                        PackCollection[packAddress].BatteryGroupCollection[i].HeatingFilmSwitchStatus = PackCollection[packAddress].BatteryGroupCollection[i].HeatingFilmSwitchStatus + 3;
                    }

                    PackCollection[packAddress].BatteryGroupCollection[i].UserSelfDefineCount_State = message[offset++];
                    enumValue = message[offset++];
                    PackCollection[packAddress].BatteryGroupCollection[i].FullyChargedStatus = Enum.IsDefined(typeof(BatteryChargingStatus), enumValue) ? (BatteryChargingStatus)enumValue : BatteryChargingStatus.未定义;

                    if (IsEnglishUI)
                    {
                        PackCollection[packAddress].BatteryGroupCollection[i].FullyChargedStatus = PackCollection[packAddress].BatteryGroupCollection[i].FullyChargedStatus + 3;
                    }

                    enumValue = message[offset++];
                    PackCollection[packAddress].BatteryGroupCollection[i].BatteryStatus = Enum.IsDefined(typeof(BatteryStatus), enumValue) ? (BatteryStatus)enumValue : BatteryStatus.未定义;

                    if (IsEnglishUI)
                    {
                        PackCollection[packAddress].BatteryGroupCollection[i].BatteryStatus = PackCollection[packAddress].BatteryGroupCollection[i].BatteryStatus + 6;
                    }

                    enumValue = message[offset++];
                    PackCollection[packAddress].BatteryGroupCollection[i].BatteryChargingRequestFlag = Enum.IsDefined(typeof(BatteryChargingRequestStatus), enumValue) ? (BatteryChargingRequestStatus)enumValue : BatteryChargingRequestStatus.未定义;

                    if (IsEnglishUI)
                    {
                        PackCollection[packAddress].BatteryGroupCollection[i].BatteryChargingRequestFlag = PackCollection[packAddress].BatteryGroupCollection[i].BatteryChargingRequestFlag + 3;
                    }
                }
            }
        }
        private void ParseMessage_StatusData_Coslight(byte[] message)
        {
            if (message.Length != 0x14)
            {
                return;
            }

            int offset = 7;
            int packAddress = message[2] - 1;
            int enumValue = 0;

            PackCollection[packAddress].SwitchCount_Coslight = message[offset++];

            enumValue = message[offset++];
            PackCollection[packAddress].SingleCellTemperaturePowerStatus_Coslight = Enum.IsDefined(typeof(Models.PowerState), enumValue) ? (Models.PowerState)enumValue : Models.PowerState.未定义;
            if (IsEnglishUI) PackCollection[packAddress].SingleCellTemperaturePowerStatus_Coslight = PackCollection[packAddress].SingleCellTemperaturePowerStatus_Coslight + 3;

            enumValue = message[offset++];
            PackCollection[packAddress].EEPROMPowerStatus_Coslight = Enum.IsDefined(typeof(Models.PowerState), enumValue) ? (Models.PowerState)enumValue : Models.PowerState.未定义;
            if (IsEnglishUI) PackCollection[packAddress].EEPROMPowerStatus_Coslight = PackCollection[packAddress].EEPROMPowerStatus_Coslight + 3;
            enumValue = message[offset++];
            PackCollection[packAddress].CommunicationPowerStatus_Coslight = Enum.IsDefined(typeof(Models.PowerState), enumValue) ? (Models.PowerState)enumValue : Models.PowerState.未定义;
            if (IsEnglishUI) PackCollection[packAddress].CommunicationPowerStatus_Coslight = PackCollection[packAddress].CommunicationPowerStatus_Coslight + 3;
            enumValue = message[offset++];
            PackCollection[packAddress].BusBarMeasurePowerStatus_Coslight = Enum.IsDefined(typeof(Models.PowerState), enumValue) ? (Models.PowerState)enumValue : Models.PowerState.未定义;
            if (IsEnglishUI) PackCollection[packAddress].BusBarMeasurePowerStatus_Coslight = PackCollection[packAddress].BusBarMeasurePowerStatus_Coslight + 3;
            enumValue = message[offset++];
            PackCollection[packAddress].PowerOnSwitchKeyStatus_Coslight = Enum.IsDefined(typeof(KeyStatus), enumValue) ? (KeyStatus)enumValue : KeyStatus.未定义;
            if (IsEnglishUI) PackCollection[packAddress].PowerOnSwitchKeyStatus_Coslight = PackCollection[packAddress].PowerOnSwitchKeyStatus_Coslight + 3;

            PackCollection[packAddress].BatteryGroupCollection[0].CellCollection[8].BalanceStatus = (message[offset] & 0x01) == 0x01 ? BalanceStatus.Active : BalanceStatus.Off;
            PackCollection[packAddress].BatteryGroupCollection[0].CellCollection[9].BalanceStatus = (message[offset] & 0x02) == 0x02 ? BalanceStatus.Active : BalanceStatus.Off;
            PackCollection[packAddress].BatteryGroupCollection[0].CellCollection[10].BalanceStatus = (message[offset] & 0x04) == 0x04 ? BalanceStatus.Active : BalanceStatus.Off;
            PackCollection[packAddress].BatteryGroupCollection[0].CellCollection[11].BalanceStatus = (message[offset] & 0x08) == 0x08 ? BalanceStatus.Active : BalanceStatus.Off;
            PackCollection[packAddress].BatteryGroupCollection[0].CellCollection[12].BalanceStatus = (message[offset] & 0x10) == 0x10 ? BalanceStatus.Active : BalanceStatus.Off;
            PackCollection[packAddress].BatteryGroupCollection[0].CellCollection[13].BalanceStatus = (message[offset] & 0x20) == 0x20 ? BalanceStatus.Active : BalanceStatus.Off;
            PackCollection[packAddress].BatteryGroupCollection[0].CellCollection[14].BalanceStatus = (message[offset++] & 0x40) == 0x40 ? BalanceStatus.Active : BalanceStatus.Off;        
            PackCollection[packAddress].BatteryGroupCollection[0].CellCollection[0].BalanceStatus = (message[offset] & 0x01) == 0x01 ? BalanceStatus.Active : BalanceStatus.Off;
            PackCollection[packAddress].BatteryGroupCollection[0].CellCollection[1].BalanceStatus = (message[offset] & 0x02) == 0x02 ? BalanceStatus.Active : BalanceStatus.Off;
            PackCollection[packAddress].BatteryGroupCollection[0].CellCollection[2].BalanceStatus = (message[offset] & 0x04) == 0x04 ? BalanceStatus.Active : BalanceStatus.Off;
            PackCollection[packAddress].BatteryGroupCollection[0].CellCollection[3].BalanceStatus = (message[offset] & 0x08) == 0x08 ? BalanceStatus.Active : BalanceStatus.Off;
            PackCollection[packAddress].BatteryGroupCollection[0].CellCollection[4].BalanceStatus = (message[offset] & 0x10) == 0x10 ? BalanceStatus.Active : BalanceStatus.Off;
            PackCollection[packAddress].BatteryGroupCollection[0].CellCollection[5].BalanceStatus = (message[offset] & 0x20) == 0x20 ? BalanceStatus.Active : BalanceStatus.Off;
            PackCollection[packAddress].BatteryGroupCollection[0].CellCollection[6].BalanceStatus = (message[offset] & 0x40) == 0x40 ? BalanceStatus.Active : BalanceStatus.Off;
            PackCollection[packAddress].BatteryGroupCollection[0].CellCollection[7].BalanceStatus = (message[offset++] & 0x80) == 0x80 ? BalanceStatus.Active : BalanceStatus.Off;

            enumValue = message[offset++];
            PackCollection[packAddress].PreChargeStatus_Coslight = Enum.IsDefined(typeof(PreChargeStatus), enumValue) ? (PreChargeStatus)enumValue : PreChargeStatus.未定义;
            if (IsEnglishUI) PackCollection[packAddress].PreChargeStatus_Coslight = PackCollection[packAddress].PreChargeStatus_Coslight + 16;
            enumValue = message[offset];
            PackCollection[packAddress].BatteryGroupCollection[0].LimitCurrentStatus_Coslight = PackCollection[packAddress].LimitCurrentStatus_Coslight = Enum.IsDefined(typeof(LimitCurrentStatus), enumValue) ? (LimitCurrentStatus)enumValue : LimitCurrentStatus.未定义;
            if (IsEnglishUI) PackCollection[packAddress].BatteryGroupCollection[0].LimitCurrentStatus_Coslight = PackCollection[packAddress].LimitCurrentStatus_Coslight + 3;

            SaveData(PackCollection[packAddress]);
        }
        //解析模拟量
        private void ParseMessage_AnalogData_DPC(byte[] message)
        {
            //DATA_FLAG=0x00
            if (message[7] != 0x00)
            {
                return;
            }
    
            int offset = 9;//位移           
            int packAddress = message[8] - 1;//上位机需要获取的 PACK 组位置

            PackCollection[packAddress].BatteryGroupCount = 1;
            int i = 0;
            //单体电池电压数量
            PackCollection[packAddress].BatteryGroupCollection[i].SingleCellVoltageCount = message[offset++];

            // 检查 CellCollection
            if (PackCollection[packAddress].BatteryGroupCollection[i].CellCollection == null)
            {
                MessageBoxService.Show("CellCollection 是 null");
            }
            MessageBoxService.Show("CellCollection "+PackCollection[packAddress].BatteryGroupCollection[i].CellCollection.Count);
            if (PackCollection[packAddress].BatteryGroupCollection[i].SingleCellVoltageCount == PackCollection[packAddress].BatteryGroupCollection[i].CellCollection.Count)
            {
                MessageBoxService.Show("解析单体电池电压");
                for (int j = 0; j != PackCollection[packAddress].BatteryGroupCollection[i].SingleCellVoltageCount; j++)
                {
                    //单体电池 j 电压 实际值=传送值/1000；
                    PackCollection[packAddress].BatteryGroupCollection[i].CellCollection[j].Voltage = BitConverter.ToUInt16(new byte[] { message[offset + 1], message[offset] }, 0) * 0.001F; 
                    offset += 2;
                    //MessageBoxService.Show("单体电池电压"+PackCollection[packAddress].BatteryGroupCollection[i].CellCollection[j].Voltage);

                }
            }

            //计算平均电压
            PackCollection[packAddress].BatteryGroupCollection[i].AverageVoltage_Coslight = PackCollection[packAddress].AverageVoltage_Coslight = PackCollection[packAddress].BatteryGroupCollection[i].CellCollection.Average(cell => cell.Voltage);//20200611
            MessageBoxService.Show("平均电压"+PackCollection[packAddress].BatteryGroupCollection[i].AverageVoltage_Coslight);

            //电芯温度数量
            PackCollection[packAddress].BatteryGroupCollection[i].SingleCellTemperatureCount = message[offset++] - 2;//为什么-2???
            //解析电芯温度
            if (PackCollection[packAddress].BatteryGroupCollection[i].SingleCellTemperatureCount <= PackCollection[packAddress].BatteryGroupCollection[i].CellCollection.Count)
            {
                MessageBoxService.Show("解析单体电池温度");
                for (int j = 0; j != PackCollection[packAddress].BatteryGroupCollection[i].SingleCellTemperatureCount; j++)
                {
                    //电芯温度 j 数据 传输数值单位0.1K
                    PackCollection[packAddress].BatteryGroupCollection[i].CellCollection[j].Temperature = (BitConverter.ToInt16(new byte[] { message[offset + 1], message[offset] }, 0) - 2731) * 0.1F; 
                    offset += 2;

                }
                // 温度就是显示2个，实在木有办法了，特殊处理 modify by sunlw 2020-11-2 18:42
                //为什么[2][3]独立处理
                PackCollection[packAddress].BatteryGroupCollection[i].CellCollection[2].Temperature = (BitConverter.ToInt16(new byte[] { message[offset + 1], message[offset] }, 0) - 2731) * 0.1F;             
                offset += 2;
                PackCollection[packAddress].BatteryGroupCollection[i].CellCollection[3].Temperature = (BitConverter.ToInt16(new byte[] { message[offset + 1], message[offset] }, 0) - 2731) * 0.1F; 
                offset += 2;
            }
            // 环境温度
            PackCollection[packAddress].AmbinentTemperature_1_Coslight = PackCollection[packAddress].BatteryGroupCollection[i].AmbinentTemperature = (BitConverter.ToInt16(new byte[] { message[offset + 1], message[offset] }, 0) - 2731) * 0.1F; 
            offset += 2;
            // MOS温度数据 //功率温度
            PackCollection[packAddress].PowerTemperature = (BitConverter.ToUInt16(new byte[] { message[offset + 1], message[offset] }, 0) - 2731) * 0.1F; 
            offset += 2;
            // 电池电流数据 /100A
            PackCollection[packAddress].BatteryGroupCurrent = PackCollection[packAddress].BatteryGroupCollection[i].BatteryGroupCurrent = BitConverter.ToInt16(new byte[] { message[offset + 1], message[offset] }, 0) * 0.01F; 
            offset += 2;
            MessageBoxService.Show("电池电流数据"+PackCollection[packAddress].BatteryGroupCurrent);
            // 电池总压数据 /100V
            PackCollection[packAddress].BatteryGroupVoltage = PackCollection[packAddress].BatteryGroupCollection[i].BatteryGroupVoltage = BitConverter.ToInt16(new byte[] { message[offset + 1], message[offset] }, 0) * 0.01F;            
            offset += 2;
            MessageBoxService.Show("电池电压数据"+PackCollection[packAddress].BatteryGroupVoltage);
            // 电池剩余容量 /100AH
            PackCollection[packAddress].SOC = PackCollection[packAddress].BatteryGroupCollection[i].SOC = BitConverter.ToUInt16(new byte[] { message[offset + 1], message[offset] }, 0) * 0.01F; 
            offset += 2;

            // 电池总容量 /100AH
            PackCollection[packAddress].BatteryGroupCollection[i].MaxAvailiableCapacity = BitConverter.ToUInt16(new byte[] { message[offset + 1], message[offset] }, 0) * 0.01F; 
            offset += 2;

            // 电池循环次数
            PackCollection[packAddress].BatteryGroupCollection[i].CycleTimes = BitConverter.ToUInt16(new byte[] { message[offset + 1], message[offset] }, 0); 
            offset += 2;

            // 自定义遥测量数量P
            PackCollection[packAddress].BatteryGroupCollection[i].UserSelfDefineCount_Analog = message[offset];
            offset++;
            //SOH
            PackCollection[packAddress].SOH = PackCollection[packAddress].BatteryGroupCollection[i].SOH = BitConverter.ToUInt16(new byte[] { message[offset + 1], message[offset] }, 0) * 0.01F; 
            offset += 2;
            //母线电压
            PackCollection[packAddress].BusBarVoltage_Coslight = PackCollection[packAddress].BatteryGroupCollection[i].BusBarVoltage_Coslight = BitConverter.ToUInt16(new byte[] { message[offset + 1], message[offset] }, 0) * 0.01F; 
            offset += 2;

            SaveData_DPC(PackCollection[packAddress]);
        }
        private void ParseMessage_AnalogData(byte[] message)
        {
            if (message.Length != 0x40)
            {
                return;
            }
            int offset = 8;
            int packAddress = message[2] - 1;

            if (ProtocalProvider.BatteryGroupCount == 0xFF)
            {
                PackCollection[packAddress].BatteryGroupCount = message[offset++];
            }
            else
            {
                PackCollection[packAddress].BatteryGroupCount = 1;
            }

            if (PackCollection[packAddress].BatteryGroupCollection.Count == PackCollection[packAddress].BatteryGroupCount)
            {
                for (int i = 0; i != PackCollection[packAddress].BatteryGroupCount; i++)//解析各个电池组数据
                {
                    PackCollection[packAddress].BatteryGroupCollection[i].SingleCellVoltageCount = message[offset++];

                    if (PackCollection[packAddress].BatteryGroupCollection[i].SingleCellVoltageCount == PackCollection[packAddress].BatteryGroupCollection[i].CellCollection.Count)
                    {
                        for (int j = 0; j != PackCollection[packAddress].BatteryGroupCollection[i].SingleCellVoltageCount; j++)
                        {
                            if (packAddress == SelectedPackAddress - 1 && i == 0 && j == 0 && this.CalibrationParameterCollection.Count == 0x08)
                            {
                                this.CalibrationParameterCollection[0].ReadValue = BitConverter.ToUInt16(new byte[] { message[offset + 1], message[offset] }, 0) * 0.001F;
                            }

                            PackCollection[packAddress].BatteryGroupCollection[i].CellCollection[j].Voltage = BitConverter.ToUInt16(new byte[] { message[offset + 1], message[offset] }, 0) * 0.001F; offset += 2;
                        }
                    }

                    PackCollection[packAddress].BatteryGroupCollection[i].AverageVoltage_Coslight = PackCollection[packAddress].AverageVoltage_Coslight = PackCollection[packAddress].BatteryGroupCollection[i].CellCollection.Average(cell => cell.Voltage);//20200611

                    PackCollection[packAddress].BatteryGroupCollection[i].SingleCellTemperatureCount = message[offset++];

                    if (PackCollection[packAddress].BatteryGroupCollection[i].SingleCellTemperatureCount <= PackCollection[packAddress].BatteryGroupCollection[i].CellCollection.Count)
                    {
                        for (int j = 0; j != PackCollection[packAddress].BatteryGroupCollection[i].SingleCellTemperatureCount; j++)
                        {
                            if (packAddress == SelectedPackAddress - 1 && i == 0 && j == 0 && this.CalibrationParameterCollection.Count == 0x08)
                            {
                                this.CalibrationParameterCollection[4].ReadValue = BitConverter.ToInt16(new byte[] { message[offset + 1], message[offset] }, 0) * 0.01F;
                            }
                            PackCollection[packAddress].BatteryGroupCollection[i].CellCollection[j].Temperature = BitConverter.ToInt16(new byte[] { message[offset + 1], message[offset] }, 0) * 0.01F; offset += 2;

                            PackCollection[packAddress].BatteryGroupCollection[i].CellCollection[j + 4].Temperature = PackCollection[packAddress].BatteryGroupCollection[i].CellCollection[j].Temperature;

                            PackCollection[packAddress].BatteryGroupCollection[i].CellCollection[j + 8].Temperature = PackCollection[packAddress].BatteryGroupCollection[i].CellCollection[j].Temperature;

                            if (j + 12 < 15)
                            {
                                PackCollection[packAddress].BatteryGroupCollection[i].CellCollection[j + 12].Temperature = PackCollection[packAddress].BatteryGroupCollection[i].CellCollection[j].Temperature;
                            }
                        }
                    }

                    PackCollection[packAddress].BatteryGroupCurrent = PackCollection[packAddress].BatteryGroupCollection[i].BatteryGroupCurrent = BitConverter.ToInt16(new byte[] { message[offset + 1], message[offset] }, 0) * 0.01F; offset += 2;
                    PackCollection[packAddress].BatteryGroupVoltage = PackCollection[packAddress].BatteryGroupCollection[i].BatteryGroupVoltage = BitConverter.ToInt16(new byte[] { message[offset + 1], message[offset] }, 0) * 0.01F; offset += 2;
                    PackCollection[packAddress].BatteryGroupCollection[i].UserSelfDefineCount_Analog = message[offset++];
                    PackCollection[packAddress].BatteryGroupCollection[i].AmbinentTemperature = BitConverter.ToInt16(new byte[] { message[offset + 1], message[offset] }, 0) * 0.01F; offset += 2;
                    PackCollection[packAddress].SOC = PackCollection[packAddress].BatteryGroupCollection[i].SOC = BitConverter.ToUInt16(new byte[] { message[offset + 1], message[offset] }, 0) * 0.01F; offset += 2;

                    PackCollection[packAddress].SOC_ZTE = PackCollection[packAddress].BatteryGroupCollection[i].SOC_ZTE = Math.Ceiling(PackCollection[packAddress].SOC);

                    if (PackCollection[packAddress].BatteryGroupCurrent > 0)//20200719
                    {
                        PackCollection[packAddress].ResidualDischargeTime = "--";
                        PackCollection[packAddress].ResidualChargeTime = ((100 - PackCollection[packAddress].SOC) * CellCapacity / (PackCollection[packAddress].BatteryGroupCurrent * 100f)).ToString();
                    }
                    else if (PackCollection[packAddress].BatteryGroupCurrent < 0)
                    {
                        PackCollection[packAddress].ResidualChargeTime = "--";
                        PackCollection[packAddress].ResidualDischargeTime = (PackCollection[packAddress].SOC * CellCapacity / (PackCollection[packAddress].BatteryGroupCurrent * 100f)).ToString();
                    }
                    else
                    {
                        PackCollection[packAddress].ResidualChargeTime = "--";
                        PackCollection[packAddress].ResidualDischargeTime = "--";
                    }

                    if (PackCollection[packAddress].BatteryGroupCollection[i].BusBarVoltage_Coslight != 0f 
                        && (PackCollection[packAddress].BatteryGroupCollection[i].LimitCurrentStatus_Coslight == LimitCurrentStatus.Working 
                        || PackCollection[packAddress].BatteryGroupCollection[i].LimitCurrentStatus_Coslight == LimitCurrentStatus.启动))
                    {
                        PackCollection[packAddress].BatteryGroupCollection[i].CurrentLimit = PackCollection[packAddress].BatteryGroupCollection[i].BatteryGroupVoltage * PackCollection[packAddress].BatteryGroupCollection[i].BatteryGroupCurrent / PackCollection[packAddress].BatteryGroupCollection[i].BusBarVoltage_Coslight;
                    }
                    else
                    {
                        PackCollection[packAddress].BatteryGroupCollection[i].CurrentLimit = 0f;
                    }
                    if (SerialPortCommunicator.ManufacturerName == "ZTE-C" && SerialPortCommunicator.ZTE_Type == "FB150C")//20200423
                    {
                        uint aa = BitConverter.ToUInt16(new byte[] { message[offset + 1], message[offset] }, 0);
                        PackCollection[packAddress].BatteryGroupCollection[i].MaxAvailiableCapacity = BitConverter.ToUInt16(new byte[] { message[offset + 1], message[offset] }, 0) * 0.01F * SerialPortCommunicator.SOH * 0.01F; offset += 2;
                    }
                    else
                    {
                        PackCollection[packAddress].BatteryGroupCollection[i].MaxAvailiableCapacity = BitConverter.ToUInt16(new byte[] { message[offset + 1], message[offset] }, 0) * 0.01F; offset += 2;

                        PackCollection[packAddress].BatteryGroupCollection[i].MaxAvailiableCapacity_ZTE = Math.Ceiling(PackCollection[packAddress].BatteryGroupCollection[i].MaxAvailiableCapacity);
                    }
                    PackCollection[packAddress].BatteryGroupCollection[i].CycleTimes = BitConverter.ToUInt16(new byte[] { message[offset + 1], message[offset] }, 0); offset += 2;

                    if (packAddress == SelectedPackAddress - 1 && i == 0 && this.CalibrationParameterCollection.Count >= 0x08 - 1)
                    {
                        this.CalibrationParameterCollection[2].ReadValue = PackCollection[packAddress].BatteryGroupCollection[i].BatteryGroupCurrent;
                        this.CalibrationParameterCollection[3].ReadValue = PackCollection[packAddress].BatteryGroupCollection[i].BatteryGroupCurrent;
                    }
                }
            }
        }
        private void ParseMessage_AnalogData_Coslight(byte[] message)
        {
            if (message.Length != 0x22)
            {
                return;
            }

            int offset = 7;
            int packAddress = message[2] - 1;

            PackCollection[packAddress].VoltageDataCount_Coslight = message[offset++];
            PackCollection[packAddress].BusBarVoltage_Coslight = BitConverter.ToInt16(new byte[] { message[offset + 1], message[offset] }, 0) * 0.01F; offset += 2;

            PackCollection[packAddress].SingleCellTemperaturePowerVoltage_Coslight = BitConverter.ToInt16(new byte[] { message[offset + 1], message[offset] }, 0) * 0.001F; offset += 2;
            PackCollection[packAddress].EEPromPowerVoltage_Coslight = BitConverter.ToInt16(new byte[] { message[offset + 1], message[offset] }, 0) * 0.001F; offset += 2;
            PackCollection[packAddress].CommunicationPowerVoltage_Coslight = BitConverter.ToInt16(new byte[] { message[offset + 1], message[offset] }, 0) * 0.001F; offset += 2;
            PackCollection[packAddress].BusBarMeasureVoltage_Coslight = BitConverter.ToInt16(new byte[] { message[offset + 1], message[offset] }, 0) * 0.001F; offset += 2;
            PackCollection[packAddress].DischargeMOSDriveVoltage_Coslight = BitConverter.ToInt16(new byte[] { message[offset + 1], message[offset] }, 0) * 0.01F; offset += 2;
            PackCollection[packAddress].CellTemperatureCount_Coslight = message[offset++];
            PackCollection[packAddress].MOSFET_1_Coslight = BitConverter.ToInt16(new byte[] { message[offset + 1], message[offset] }, 0) * 0.01F; offset += 2;
            PackCollection[packAddress].MOSFET_2_Coslight = BitConverter.ToInt16(new byte[] { message[offset + 1], message[offset] }, 0) * 0.01F; offset += 2;
            PackCollection[packAddress].AmbinentTemperature_1_Coslight = BitConverter.ToInt16(new byte[] { message[offset + 1], message[offset] }, 0) * 0.01F; offset += 2;
            PackCollection[packAddress].AmbinentTemperature_2_Coslight = BitConverter.ToInt16(new byte[] { message[offset + 1], message[offset] }, 0) * 0.01F; offset += 2;
            PackCollection[packAddress].SOH = BitConverter.ToInt16(new byte[] { message[offset + 1], message[offset] }, 0) * 0.01F; offset += 2;

            SerialPortCommunicator.SOH = PackCollection[packAddress].SOH;//20200423

            if (packAddress == SelectedPackAddress - 1 && this.CalibrationParameterCollection.Count == 0x08 - 1)
            {
                this.CalibrationParameterCollection[1].ReadValue = PackCollection[packAddress].BusBarVoltage_Coslight;
                this.CalibrationParameterCollection[5].ReadValue = PackCollection[packAddress].AmbinentTemperature_1_Coslight;
                this.CalibrationParameterCollection[6].ReadValue = PackCollection[packAddress].MOSFET_1_Coslight;
            }
        }
        private void ParseMessage_AnalogData_Coslight_FB100C5(byte[] message)//20200606
        {
            if (message.Length < 0x22)
            {
                return;
            }

            int offset = 7;
            int packAddress = message[2] - 1;

            PackCollection[packAddress].VoltageDataCount_Coslight = message[offset++];
            PackCollection[packAddress].BatteryGroupCollection[0].BusBarVoltage_Coslight = PackCollection[packAddress].BusBarVoltage_Coslight = BitConverter.ToInt16(new byte[] { message[offset + 1], message[offset] }, 0) * 0.01F; offset += 2;

            PackCollection[packAddress].SingleCellTemperaturePowerVoltage_Coslight = BitConverter.ToInt16(new byte[] { message[offset + 1], message[offset] }, 0) * 0.001F; offset += 2;
            PackCollection[packAddress].EEPromPowerVoltage_Coslight = BitConverter.ToInt16(new byte[] { message[offset + 1], message[offset] }, 0) * 0.001F; offset += 2;
            PackCollection[packAddress].CommunicationPowerVoltage_Coslight = BitConverter.ToInt16(new byte[] { message[offset + 1], message[offset] }, 0) * 0.001F; offset += 2;
            PackCollection[packAddress].BusBarMeasureVoltage_Coslight = BitConverter.ToInt16(new byte[] { message[offset + 1], message[offset] }, 0) * 0.001F; offset += 2;
            PackCollection[packAddress].DischargeMOSDriveVoltage_Coslight = BitConverter.ToInt16(new byte[] { message[offset + 1], message[offset] }, 0) * 0.01F; offset += 2;
            PackCollection[packAddress].CellTemperatureCount_Coslight = message[offset++];
            PackCollection[packAddress].MOSFET_1_Coslight = BitConverter.ToInt16(new byte[] { message[offset + 1], message[offset] }, 0) * 0.01F; offset += 2;
            PackCollection[packAddress].MOSFET_2_Coslight = BitConverter.ToInt16(new byte[] { message[offset + 1], message[offset] }, 0) * 0.01F; offset += 2;
            PackCollection[packAddress].AmbinentTemperature_1_Coslight = BitConverter.ToInt16(new byte[] { message[offset + 1], message[offset] }, 0) * 0.01F; offset += 2;
            PackCollection[packAddress].AmbinentTemperature_2_Coslight = BitConverter.ToInt16(new byte[] { message[offset + 1], message[offset] }, 0) * 0.01F; offset += 2;
            PackCollection[packAddress].BatteryGroupCollection[0].SOH = PackCollection[packAddress].SOH = BitConverter.ToInt16(new byte[] { message[offset + 1], message[offset] }, 0) * 0.01F; offset += 2;

            PackCollection[packAddress].BatteryGroupCollection[0].SOH_ZTE = PackCollection[packAddress].SOH_ZTE = Math.Ceiling(PackCollection[packAddress].SOH);

            try
            {
                PackCollection[packAddress].CurrentLimitingTemperature_Coslight = BitConverter.ToInt16(new byte[] { message[offset + 1], message[offset] }, 0) * 0.01F; offset += 2;
                PackCollection[packAddress].BatteryGroupCollection[0].AccCHCapacity_Coslight = PackCollection[packAddress].AccCHCapacity_Coslight = BitConverter.ToUInt32(new byte[] { message[offset + 3], message[offset + 2], message[offset + 1], message[offset] }, 0); offset += 4;
                PackCollection[packAddress].BatteryGroupCollection[0].AccDCHCapacity_Coslight = PackCollection[packAddress].AccDCHCapacity_Coslight = BitConverter.ToUInt32(new byte[] { message[offset + 3], message[offset + 2], message[offset + 1], message[offset] }, 0); offset += 4;
            }
            catch { }
            SerialPortCommunicator.SOH = PackCollection[packAddress].SOH;//20200423

            if (packAddress == SelectedPackAddress - 1 && this.CalibrationParameterCollection.Count == 0x08 - 1)
            {
                this.CalibrationParameterCollection[1].ReadValue = PackCollection[packAddress].BusBarVoltage_Coslight;
                this.CalibrationParameterCollection[5].ReadValue = PackCollection[packAddress].AmbinentTemperature_1_Coslight;
                this.CalibrationParameterCollection[6].ReadValue = PackCollection[packAddress].MOSFET_1_Coslight;
            }

        }
        //解析设备厂商信息
        private void ParseMessage_DeviceManufactureInfo_DPC(byte[] message)
        {
            //获取电池包地址
            int packAddress = message[2] - 1;
            // 电池生产厂商信息 第 7 个字节 开始，读取 20 个字节，然后将这些字节转换为 ASCII 字符串，.Trim()去掉前后空格
            PackCollection[packAddress].SystemName = System.Text.Encoding.ASCII.GetString(message, 7, 20).Trim();
            // 电池型号
            string BatteryModel = System.Text.Encoding.ASCII.GetString(message, 27, 2).Trim();
            if (IsChineseUI)
            {
                if (BatteryModel == "01")
                    PackCollection[packAddress].BatteryModel = "铁锂";
                else if (BatteryModel == "02")
                    PackCollection[packAddress].BatteryModel = "钛酸锂";
                else if (BatteryModel == "03")
                    PackCollection[packAddress].BatteryModel = "三元锂电";
            }
            if (IsEnglishUI)
            {
                if (BatteryModel == "01")
                    PackCollection[packAddress].BatteryModel = "FP";
                else if (BatteryModel == "02")
                    PackCollection[packAddress].BatteryModel = "LTO";
                else if (BatteryModel == "03")
                    PackCollection[packAddress].BatteryModel = "NCMP";
            }
            // BMS生产厂商
            PackCollection[packAddress].ManufacturerName = System.Text.Encoding.ASCII.GetString(message, 29, 20).Trim();
            // BMS型号
            PackCollection[packAddress].BMSModel = "BMS" + System.Text.Encoding.ASCII.GetString(message, 49, 1).Trim()
                + "." + System.Text.Encoding.ASCII.GetString(message, 50, 1).Trim().ToString();
            // BMS通信软件版本号
            PackCollection[packAddress].SoftwareVersion = System.Text.Encoding.ASCII.GetString(message, 51, 1).Trim()
                + "." + System.Text.Encoding.ASCII.GetString(message, 52, 1).Trim().ToString();

            Application.Current.MainWindow.Title = PackCollection[packAddress].SystemName + " - " + PackCollection[packAddress].ManufacturerName + " - " + PackCollection[packAddress].SoftwareVersion;

        }
        private void ParseMessage_DeviceManufactureInfo(byte[] message)
        {
            try
            {
                int packAddress = message[2] - 1;
                PackCollection[packAddress].SystemName = System.Text.Encoding.ASCII.GetString(message, 7, 10).Trim();
                SerialPortCommunicator.Ver = PackCollection[packAddress].SoftwareVersion = message[17].ToString("D2") + "." + message[18].ToString("D2");
                PackCollection[packAddress].ManufacturerName = System.Text.Encoding.ASCII.GetString(message, 19, 20).Trim();

                if (PackCollection[packAddress].SystemName.Contains("FB150C"))
                {
                    SerialPortCommunicator.ZTE_Type = "FB150C";

                    CellCapacity = 150;
                }
                else if (PackCollection[packAddress].SystemName.Contains("FB100C5"))
                {
                    SerialPortCommunicator.ZTE_Type = "FB100C5";
                    CellCapacity = 100;
                }
                else if (PackCollection[packAddress].SystemName.Contains("FB100C1"))
                {
                    SerialPortCommunicator.ZTE_Type = "FB100C1";
                    CellCapacity = 100;
                }

                if (PackCollection[packAddress].ManufacturerName.Contains("ZTE"))
                {
                    SerialPortCommunicator.ManufacturerName = "ZTE-C";
                }

                if (SerialPortCommunicator.Manager == AuthorizationManager.开发版)
                {
                    Application.Current.MainWindow.Title = "EIHT - " + PackCollection[packAddress].SystemName + " - " + PackCollection[packAddress].SoftwareVersion + " - " + PackCollection[packAddress].ManufacturerName;//20200507
                }
                else
                {
                    Application.Current.MainWindow.Title = PackCollection[packAddress].SystemName + " - " + PackCollection[packAddress].SoftwareVersion + " - " + PackCollection[packAddress].ManufacturerName;//20200507
                }

                PackCollection[packAddress].ProductSN = System.Text.Encoding.ASCII.GetString(message, 51, 24).Trim();

                if (SerialPortCommunicator.ManufacturerName == "ZTE-C" && SerialPortCommunicator.ZTE_Type == "FB100C5")//20200522
                {
                    DryContactSetViewName = "DryContact2SetView";
                }
                else
                {
                    DryContactSetViewName = "DryContactSetView";
                }
            }
            catch
            {

            }
        }
        //系统特殊参数
        private void ParseMessage_SystemSpecialParameter(byte[] message)
        {

            int packAddress = message[2] - 1;

            EnableControlCollection.Clear();

            if (IsChineseUI)
            {
                EnableControlCollection.Add(ViewModelSource.Create(() => new SpecialParameter() 
                { 
                    SpecialParameterID = EnableControlCollection.Count + 1, 
                    Description_US = "Rated Capacity(AH)", 
                    Description = "电池组标称容量(AH)", 
                    CommandType = 0x80, 
                    CurrentValue = BitConverter.ToUInt16(new byte[] { message[10], message[9] }, 0).ToString() 
                }));
                EnableControlCollection.Add(ViewModelSource.Create(() => new SpecialParameter() 
                { 
                    SpecialParameterID = EnableControlCollection.Count + 1, 
                    Description_US = "Pack CH OV Protection", 
                    Description = "电池组充电过压保护使能", 
                    CommandType = 0x89, 
                    CurrentValue = message[12] == 0x00 ? "使能" : "禁用" 
                }));
                EnableControlCollection.Add(ViewModelSource.Create(() => new SpecialParameter() 
                { 
                    SpecialParameterID = EnableControlCollection.Count + 1, 
                    Description_US = "Pack DCH UV Protection", 
                    Description = "电池组放电欠压保护使能", 
                    CommandType = 0x8A, 
                    CurrentValue = message[13] == 0x00 ? "使能" : "禁用" 
                }));
                EnableControlCollection.Add(ViewModelSource.Create(() => new SpecialParameter() 
                { 
                    SpecialParameterID = EnableControlCollection.Count + 1, 
                    Description_US = "Cell CH OV Protection", 
                    Description = "单体充电过压保护使能", 
                    CommandType = 0x8B, 
                    CurrentValue = message[14] == 0x00 ? "使能" : "禁用" 
                }));
                EnableControlCollection.Add(ViewModelSource.Create(() => new SpecialParameter() 
                { 
                    SpecialParameterID = EnableControlCollection.Count + 1, 
                    Description_US = "Cell DCH UV Protection", 
                    Description = "单体放电欠压保护使能", 
                    CommandType = 0x8C, 
                    CurrentValue = message[15] == 0x00 ? "使能" : "禁用" 
                }));
                EnableControlCollection.Add(ViewModelSource.Create(() => new SpecialParameter() 
                { 
                    SpecialParameterID = EnableControlCollection.Count + 1, 
                    Description_US = "CH OT Protection", 
                    Description = "充电过温保护使能", 
                    CommandType = 0x8D, 
                    CurrentValue = message[16] == 0x00 ? "使能" : "禁用" 
                }));
                EnableControlCollection.Add(ViewModelSource.Create(() => new SpecialParameter() 
                { 
                    SpecialParameterID = EnableControlCollection.Count + 1, 
                    Description_US = "DCH OT Protection", 
                    Description = "放电过温保护使能", 
                    CommandType = 0x8E, 
                    CurrentValue = message[17] == 0x00 ? "使能" : "禁用" 
                }));
                EnableControlCollection.Add(ViewModelSource.Create(() => new SpecialParameter() 
                { 
                    SpecialParameterID = EnableControlCollection.Count + 1, 
                    Description_US = "CH UT Protection", 
                    Description = "充电低温保护使能", 
                    CommandType = 0x8F, 
                    CurrentValue = message[18] == 0x00 ? "使能" : "禁用" 
                }));
                EnableControlCollection.Add(ViewModelSource.Create(() => new SpecialParameter() 
                { 
                    SpecialParameterID = EnableControlCollection.Count + 1, 
                    Description_US = "DCH UT Protection", 
                    Description = "放电低温保护使能", 
                    CommandType = 0x90, 
                    CurrentValue = message[19] == 0x00 ? "使能" : "禁用" 
                }));
                EnableControlCollection.Add(ViewModelSource.Create(() => new SpecialParameter() 
                { 
                    SpecialParameterID = EnableControlCollection.Count + 1, 
                    Description_US = "Beeper",
                    Description = "蜂鸣器使能", 
                    CommandType = 0x91, 
                    CurrentValue = message[20] == 0x00 ? "使能" : "禁用" 
                }));
                EnableControlCollection.Add(ViewModelSource.Create(() => new SpecialParameter() 
                { 
                    SpecialParameterID = EnableControlCollection.Count + 1, 
                    Description_US = "Heating Function", 
                    Description = "加热垫使能", 
                    CommandType = 0x92, 
                    CurrentValue = message[21] == 0x00 ? "使能" : "禁用" 
                }));
                if (SerialPortCommunicator.ManufacturerName == "ZTE-C" && SerialPortCommunicator.ZTE_Type != "FB100C5")
                {
                    EnableControlCollection.Add(ViewModelSource.Create(() => new SpecialParameter() 
                    { 
                        SpecialParameterID = EnableControlCollection.Count + 1, 
                        Description_US = "Anti-Theft Function", 
                        Description = "软件防盗使能", 
                        CommandType = 0x94, 
                        CurrentValue = message[22] == 0x00 ? "使能" : "禁用" 
                    }));
                }

                EnableControlCollection.Add(ViewModelSource.Create(() => new SpecialParameter() 
                { 
                    SpecialParameterID = EnableControlCollection.Count + 1, 
                    Description = "短路使能保护", 
                    CurrentValue = "使能 " 
                }));

            }
            else
            {
                EnableControlCollection.Add(ViewModelSource.Create(() => new SpecialParameter() 
                { 
                    SpecialParameterID = EnableControlCollection.Count + 1, 
                    Description_US = "Rated Capacity(AH)", 
                    Description = "电池组标称容量(AH)", 
                    CommandType = 0x80, 
                    CurrentValue = BitConverter.ToUInt16(new byte[] { message[10], message[9] }, 0).ToString() 
                }));
                EnableControlCollection.Add(ViewModelSource.Create(() => new SpecialParameter() 
                { 
                    SpecialParameterID = EnableControlCollection.Count + 1, 
                    Description_US = "Pack CH OV Protection", 
                    Description = "电池组充电过压保护使能", 
                    CommandType = 0x89, 
                    CurrentValue = message[12] == 0x00 ? "Enable" : "Disable" 
                }));
                EnableControlCollection.Add(ViewModelSource.Create(() => new SpecialParameter() 
                { 
                    SpecialParameterID = EnableControlCollection.Count + 1, 
                    Description_US = "Pack DCH UV Protection", 
                    Description = "电池组放电欠压保护使能", 
                    CommandType = 0x8A, 
                    CurrentValue = message[13] == 0x00 ? "Enable" : "Disable" 
                }));
                EnableControlCollection.Add(ViewModelSource.Create(() => new SpecialParameter() 
                { 
                    SpecialParameterID = EnableControlCollection.Count + 1, 
                    Description_US = "Cell CH OV Protection", 
                    Description = "单体充电过压保护使能", 
                    CommandType = 0x8B, 
                    CurrentValue = message[14] == 0x00 ? "Enable" : "Disable" 
                }));
                EnableControlCollection.Add(ViewModelSource.Create(() => new SpecialParameter() 
                { 
                    SpecialParameterID = EnableControlCollection.Count + 1, 
                    Description_US = "Cell DCH UV Protection", 
                    Description = "单体放电欠压保护使能", 
                    CommandType = 0x8C, 
                    CurrentValue = message[15] == 0x00 ? "Enable" : "Disable" 
                }));
                EnableControlCollection.Add(ViewModelSource.Create(() => new SpecialParameter() 
                { 
                    SpecialParameterID = EnableControlCollection.Count + 1, 
                    Description_US = "CH OT Protection", 
                    Description = "充电过温保护使能", 
                    CommandType = 0x8D, 
                    CurrentValue = message[16] == 0x00 ? "Enable" : "Disable" 
                }));
                EnableControlCollection.Add(ViewModelSource.Create(() => new SpecialParameter() 
                { 
                    SpecialParameterID = EnableControlCollection.Count + 1, 
                    Description_US = "DCH OT Protection", 
                    Description = "放电过温保护使能", 
                    CommandType = 0x8E, 
                    CurrentValue = message[17] == 0x00 ? "Enable" : "Disable" 
                }));
                EnableControlCollection.Add(ViewModelSource.Create(() => new SpecialParameter() 
                { 
                    SpecialParameterID = EnableControlCollection.Count + 1, 
                    Description_US = "CH UT Protection", 
                    Description = "充电低温保护使能", 
                    CommandType = 0x8F, 
                    CurrentValue = message[18] == 0x00 ? "Enable" : "Disable" 
                }));
                EnableControlCollection.Add(ViewModelSource.Create(() => new SpecialParameter() 
                { 
                    SpecialParameterID = EnableControlCollection.Count + 1, 
                    Description_US = "DCH UT Protection", 
                    Description = "放电低温保护使能", 
                    CommandType = 0x90, 
                    CurrentValue = message[19] == 0x00 ? "Enable" : "Disable" 
                }));
                EnableControlCollection.Add(ViewModelSource.Create(() => new SpecialParameter() 
                { 
                    SpecialParameterID = EnableControlCollection.Count + 1, 
                    Description_US = "Beeper", 
                    Description = "蜂鸣器使能", 
                    CommandType = 0x91, 
                    CurrentValue = message[20] == 0x00 ? "Enable" : "Disable" 
                }));
                EnableControlCollection.Add(ViewModelSource.Create(() => new SpecialParameter() 
                { 
                    SpecialParameterID = EnableControlCollection.Count + 1, 
                    Description_US = "Heating Function", 
                    Description = "加热垫使能", 
                    CommandType = 0x92, 
                    CurrentValue = message[21] == 0x00 ? "Enable" : "Disable" 
                }));
               if (SerialPortCommunicator.ManufacturerName == "ZTE-C" && SerialPortCommunicator.ZTE_Type != "FB100C5")
                {
                    EnableControlCollection.Add(ViewModelSource.Create(() => new SpecialParameter() 
                    { 
                        SpecialParameterID = EnableControlCollection.Count + 1, 
                        Description_US = "Anti-Theft Function", 
                        Description = "软件防盗使能", 
                        CommandType = 0x94, 
                        CurrentValue = message[22] == 0x00 ? "Enable" : "Disable" 
                    }));
                }

                EnableControlCollection.Add(ViewModelSource.Create(() => new SpecialParameter() 
                { 
                    SpecialParameterID = EnableControlCollection.Count + 1, 
                    Description = "短路使能保护", 
                    Description_US = "Short Circuit Protection", 
                    CurrentValue = "Enable " 
                }));

            }

            IsLoadingSpecialSystemParameter = false;
        }
        private void ParseMessage_SystemSpecialParameter_Coslight(byte[] message)
        {
            if (IsChineseUI)
            {
                EnableControlCollection.Add(ViewModelSource.Create(() => new SpecialParameter() 
                { 
                    SpecialParameterID = EnableControlCollection.Count + 1, 
                    Description = "SOC保护使能", 
                    CommandType = 0x01, 
                    Description_US = "SOC Protection", 
                    CurrentValue = message[7] == 0x01 ? "使能" : "禁用" 
                }));
                if (SerialPortCommunicator.Manager == AuthorizationManager.开发版)
                {
                    EnableControlCollection.Add(ViewModelSource.Create(() => new SpecialParameter() 
                    { 
                        SpecialParameterID = EnableControlCollection.Count + 1, 
                        Description_US = "Current Limit Switch", 
                        Description = "限流开关", 
                        CommandType = 0x06, 
                        CurrentValue = message[8] == 0x00 ? "10A" : "20A" 
                    }));
                }

            }
            else
            {
                EnableControlCollection.Add(ViewModelSource.Create(() => new SpecialParameter() 
                { 
                    SpecialParameterID = EnableControlCollection.Count + 1, 
                    Description = "SOC保护使能", 
                    CommandType = 0x01, 
                    Description_US = "SOC Protection", 
                    CurrentValue = message[7] == 0x01 ? "Enable" : "Disable" 
                }));
                if (SerialPortCommunicator.Manager == AuthorizationManager.开发版)
                {
                    EnableControlCollection.Add(ViewModelSource.Create(() => new SpecialParameter() 
                    { 
                        SpecialParameterID = EnableControlCollection.Count + 1, 
                        Description_US = "Current Limit Switch", 
                        Description = "限流开关", 
                        CommandType = 0x06, 
                        CurrentValue = message[8] == 0x00 ? "10A" : "20A" 
                    }));

                }
            }

            IsLoadingSpecialSystemParameter = false;
        }
        //设备运行记录
        private void ParseMessage_DeviceRunningRecord(byte[] message)
        {
            //if (message.Length != 0x26)
            //{
            //    return;
            //}

            //WriteLog(message);

            //SerialPortCommunicator.ReadingDeviceRunningDataCounter = 0;

            //DeviceRunningRecords.Add(
            //    ViewModelSource.Create(() => new DeviceRunningRecord
            //    {
            //        SaveTime = new DateTime(DateTime.Now.Year, message[8], message[9], message[10], message[11], message[12]),
            //        DeviceRunningRecordID = DeviceRunningRecords.Count + 1,
            //        HighestCellVoltage = BitConverter.ToUInt16(new byte[] { message[16], message[15] }, 0) * 0.001F,
            //        LowestCellVoltage = BitConverter.ToUInt16(new byte[] { message[18], message[17] }, 0) * 0.001F,
            //        BatteryCurrent = BitConverter.ToInt16(new byte[] { message[20], message[19] }, 0) * 0.01F,
            //        BatteryVoltage = BitConverter.ToUInt16(new byte[] { message[22], message[21] }, 0) * 0.01F,
            //        HighestTemperature = BitConverter.ToInt16(new byte[] { message[24], message[23] }, 0) * 0.01F,
            //        LowestTemperature = BitConverter.ToInt16(new byte[] { message[26], message[25] }, 0) * 0.01F,
            //        ResidualCapacity = BitConverter.ToUInt16(new byte[] { message[28], message[27] }, 0) * 0.01F,
            //        CycleTimes = BitConverter.ToUInt16(new byte[] { message[30], message[29] }, 0),
            //        MaxAvaliableCapacity = BitConverter.ToInt16(new byte[] { message[32], message[31] }, 0) * 0.01F,
            //        SOC = BitConverter.ToInt16(new byte[] { message[34], message[33] }, 0) * 0.01F
            //    }));
            //if (SerialPortCommunicator.IsReadingDeviceRunningData == 0)
            //{
            //    return;
            //}
            //if (message[7] == 0x00)
            //{
            //    communicator.UserAction(ProtocalProvider.GetMessage_ReadDeviceRunningRecord(message[2], 0x01));
            //}
            //else if (message[7] == 0x01)
            //{
            //    communicator.UserAction(ProtocalProvider.GetMessage_ReadDeviceRunningRecord(message[2], 0x03));
            //    SerialPortCommunicator.IsReadingDeviceRunningData = 0;
            //}
        }
        //设备警告记录
        private void ParseMessage_DeviceWarningRecord(byte[] message)
        {
            SerialPortCommunicator.ReadingDeviceWarningDataCounter = 0;
            try
            {
                DeviceWarningRecord record = new DeviceWarningRecord();
                record.DeviceWarningRecordID = DeviceWarningRecords.Count + 1;
                record.SaveTime = new DateTime(DateTime.Now.Year, message[8], message[9], message[10], message[11], message[12]);
                switch (message[15])
                {
                    case 0x00:

                        record.CellVoltageWarningStatus = IsChineseUI ? "正常" : "OK";
                        break;

                    case 0x01:

                        record.CellVoltageWarningStatus = IsChineseUI ? "单体欠压告警" : "Cell UV Warning";
                        break;

                    case 0x02:

                        record.CellVoltageWarningStatus = IsChineseUI ? "单体过压告警" : "Cell OV Warning";
                        break;
                }

                switch (message[16])
                {
                    case 0x00:

                        record.CellVoltageProtectionStatus = IsChineseUI ? "正常" : "OK";
                        break;

                    case 0x01:

                        record.CellVoltageProtectionStatus = IsChineseUI ? "单体欠压保护" : "Cell UV Protection";
                        break;

                    case 0x02:

                        record.CellVoltageProtectionStatus = IsChineseUI ? "单体过压保护" : "Cell OV Protection";
                        break;
                }

                switch (message[17])
                {
                    case 0x00:

                        record.BatteryGroupVoltageWarningStatus = IsChineseUI ? "正常" : "OK";
                        break;

                    case 0x01:

                        record.BatteryGroupVoltageWarningStatus = IsChineseUI ? "电池组欠压告警" : "Pack UV Warning";
                        break;

                    case 0x02:

                        record.CellVoltageProtectionStatus = IsChineseUI ? "电池组过压告警" : "Pack OV Warning";
                        break;
                }

                switch (message[18])
                {
                    case 0x00:

                        record.BatteryGroupVoltageProtectionStatus = IsChineseUI ? "正常" : "OK";
                        break;

                    case 0x01:

                        record.BatteryGroupVoltageProtectionStatus = IsChineseUI ? "电池组欠压保护" : "Pack UV Protection";
                        break;

                    case 0x02:

                        record.BatteryGroupVoltageProtectionStatus = IsChineseUI ? "电池组过压保护" : "Pack OV Protection";
                        break;
                }

                switch (message[19])
                {
                    case 0x00:

                        record.CellTemperatureWarningStatus = IsChineseUI ? "正常" : "OK";
                        break;

                    case 0x01:

                        record.CellTemperatureWarningStatus = IsChineseUI ? "单体充电低温告警" : "Cell CH UT Warning";
                        break;

                    case 0x02:

                        record.CellTemperatureWarningStatus = IsChineseUI ? "单体充电高温告警" : "Cell CH OT Warning";
                        break;

                    case 0x03:

                        record.CellTemperatureWarningStatus = IsChineseUI ? "单体放电低温告警" : "Cell DCH UT Warning";
                        break;

                    case 0x04:

                        record.CellTemperatureWarningStatus = IsChineseUI ? "单体放电高温告警" : "Cell DCH OT Warning";
                        break;
                }

                switch (message[20])
                {
                    case 0x00:

                        record.CellTemperatureProtectionStatus = IsChineseUI ? "正常" : "OK";
                        break;

                    case 0x01:

                        record.CellTemperatureProtectionStatus = IsChineseUI ? "单体充电低温保护" : "Cell CH UT Protection";
                        break;

                    case 0x02:

                        record.CellTemperatureProtectionStatus = IsChineseUI ? "单体充电高温保护" : "Cell CH OT Protection";
                        break;

                    case 0x03:

                        record.CellTemperatureProtectionStatus = IsChineseUI ? "单体放电低温保护" : "Cell DCH UT Protection";
                        break;

                    case 0x04:

                        record.CellTemperatureProtectionStatus = IsChineseUI ? "单体放电高温保护" : "Cell DCH OT Protection";
                        break;
                }

                switch (message[21])
                {
                    case 0x00:

                        record.CHOCWarningStatus = IsChineseUI ? "正常" : "OK";
                        break;

                    case 0x01:

                        record.CHOCWarningStatus = IsChineseUI ? "电池组充电过流告警" : "Pack CH OC Warning";
                        break;
                }

                switch (message[22])
                {
                    case 0x00:

                        record.CHOCProtectionStatus = IsChineseUI ? "正常" : "OK";
                        break;

                    case 0x01:

                        record.CHOCProtectionStatus = IsChineseUI ? "电池组充电过流保护" : "Pack CH OC Protection";
                        break;

                    case 0x02:

                        record.CHOCProtectionStatus = IsChineseUI ? "电池组充电过流二级保护" : "Pack CH OC Level2 Protection";
                        break;
                }

                switch (message[23])
                {
                    case 0x00:

                        record.DCHOCWarningStatus = IsChineseUI ? "正常" : "OK";
                        break;

                    case 0x01:

                        record.DCHOCWarningStatus = IsChineseUI ? "电池组放电过流告警" : "Pack DCH OC Warning";
                        break;
                }

                switch (message[24])
                {
                    case 0x00:

                        record.DCHOCProtectionStatus = IsChineseUI ? "正常" : "OK";
                        break;

                    case 0x01:

                        record.DCHOCProtectionStatus = IsChineseUI ? "电池组放电过流保护" : "Pack DCH OC Protection";
                        break;

                    case 0x02:

                        record.DCHOCProtectionStatus = IsChineseUI ? "电池组放电过流二级保护" : "Pack DCH OC Level2 Protection";
                        break;
                }

                switch (message[25])
                {
                    case 0x00:

                        record.ShortCircuitProtectionStatus = IsChineseUI ? "正常" : "OK";
                        break;

                    case 0x01:

                        record.ShortCircuitProtectionStatus = IsChineseUI ? "电池组短路保护" : "Short Circuit Protection";
                        break;
                }

                DeviceWarningRecords.Add(record);

                if (SerialPortCommunicator.IsReadingDeviceWarningData == 0)
                {
                    return;
                }
            }
            catch
            {

            }
            if (message[7] == 0x00)
            {
                communicator.UserAction(ProtocalProvider.GetMessage_ReadDeviceWarningRecord(message[2], 0x01));
            }
            else if (message[7] == 0x01)
            {
                communicator.UserAction(ProtocalProvider.GetMessage_ReadDeviceWarningRecord(message[2], 0x03));
                SerialPortCommunicator.IsReadingDeviceWarningData = 0;
            }
        }
        //干接点状态
        private void ParseMessage_DryContactStatus(byte[] message)
        {
            int packAddress = message[2] - 1;
            DryContactCollection.Clear();

            if (IsChineseUI)
            {
                DryContactCollection.Add(ViewModelSource.Create(() => new DryContactItem() 
                { 
                    DryContactItemID = DryContactCollection.Count + 1, 
                    Description = "充电高压告警", 
                    Description_US = "CH OV Warning", 
                    CommandType = 0x88, 
                    CurrentValue = message[7] == 0x00 ? "开" : "关" 
                }));
                DryContactCollection.Add(ViewModelSource.Create(() => new DryContactItem() 
                { 
                    DryContactItemID = DryContactCollection.Count + 1, 
                    Description = "充电高压保护", 
                    Description_US = "CH OV Protection", 
                    CommandType = 0x89, 
                    CurrentValue = message[8] == 0x00 ? "开" : "关" 
                }));
                DryContactCollection.Add(ViewModelSource.Create(() => new DryContactItem() 
                { 
                    DryContactItemID = DryContactCollection.Count + 1, 
                    Description = "放电欠压告警", 
                    Description_US = "DCH UV Warning", 
                    CommandType = 0x8A, 
                    CurrentValue = message[9] == 0x00 ? "开" : "关" 
                }));
                DryContactCollection.Add(ViewModelSource.Create(() => new DryContactItem() 
                { 
                    DryContactItemID = DryContactCollection.Count + 1, 
                    Description = "放电欠压保护", 
                    Description_US = "DCH UV Protection", 
                    CommandType = 0x8B, 
                    CurrentValue = message[10] == 0x00 ? "开" : "关" 
                }));
                DryContactCollection.Add(ViewModelSource.Create(() => new DryContactItem() 
                { 
                    DryContactItemID = DryContactCollection.Count + 1, 
                    Description = "单体高压告警", 
                    Description_US = "Cell OV Warning", 
                    CommandType = 0x8C, 
                    CurrentValue = message[11] == 0x00 ? "开" : "关" 
                }));
                DryContactCollection.Add(ViewModelSource.Create(() => new DryContactItem() 
                { 
                    DryContactItemID = DryContactCollection.Count + 1, 
                    Description = "单体高压保护", 
                    CommandType = 0x8D, 
                    Description_US = "Cell OV Protection", 
                    CurrentValue = message[12] == 0x00 ? "开" : "关" 
                }));
                DryContactCollection.Add(ViewModelSource.Create(() => new DryContactItem() 
                { 
                    DryContactItemID = DryContactCollection.Count + 1, 
                    Description = "单体欠压告警", 
                    CommandType = 0x8E, 
                    Description_US = "Cell UV Warning", 
                    CurrentValue = message[13] == 0x00 ? "开" : "关" 
                }));
                DryContactCollection.Add(ViewModelSource.Create(() => new DryContactItem() 
                { 
                    DryContactItemID = DryContactCollection.Count + 1, 
                    Description = "单体欠压保护", 
                    CommandType = 0x8F, 
                    Description_US = "Cell UV Protection", 
                    CurrentValue = message[14] == 0x00 ? "开" : "关" 
                }));
                if (SerialPortCommunicator.ManufacturerName == "ZTE-C" && SerialPortCommunicator.ZTE_Type == "FB150C")
                {
                    DryContactCollection.Add(ViewModelSource.Create(() => new DryContactItem() 
                    { 
                        DryContactItemID = DryContactCollection.Count + 1, 
                        Description = "充电过流保护", 
                        CommandType = 0x90, 
                        Description_US = "CH OC Protection", 
                        CurrentValue = message[15] == 0x00 ? "开" : "关" 
                    }));
                    DryContactCollection.Add(ViewModelSource.Create(() => new DryContactItem() 
                    { 
                        DryContactItemID = DryContactCollection.Count + 1, 
                        Description = "充电二级过流保护", 
                        CommandType = 0x91, 
                        Description_US = "CH OC Level2 Protection", 
                        CurrentValue = message[16] == 0x00 ? "开" : "关" 
                    }));
                }
                else
                {
                    DryContactCollection.Add(ViewModelSource.Create(() => new DryContactItem() 
                    { 
                        DryContactItemID = DryContactCollection.Count + 1, 
                        Description = "充电过流告警", 
                        CommandType = 0x90, 
                        Description_US = "CH OC Warning", 
                        CurrentValue = message[15] == 0x00 ? "开" : "关" 
                    }));
                    DryContactCollection.Add(ViewModelSource.Create(() => new DryContactItem() 
                    { 
                        DryContactItemID = DryContactCollection.Count + 1, 
                        Description = "充电二级过流告警", 
                        CommandType = 0x91, 
                        Description_US = "CH OC Level2 Warning", 
                        CurrentValue = message[16] == 0x00 ? "开" : "关" 
                    }));

                }
                DryContactCollection.Add(ViewModelSource.Create(() => new DryContactItem() 
                { 
                    DryContactItemID = DryContactCollection.Count + 1, 
                    Description = "放电过流告警", 
                    CommandType = 0x92, 
                    Description_US = "DCH OC Warning", 
                    CurrentValue = message[17] == 0x00 ? "开" : "关" 
                }));
                DryContactCollection.Add(ViewModelSource.Create(() => new DryContactItem() 
                { 
                    DryContactItemID = DryContactCollection.Count + 1, 
                    Description = "放电过流保护", 
                    CommandType = 0x93, 
                    Description_US = "DCH OC Protection", 
                    CurrentValue = message[18] == 0x00 ? "开" : "关" 
                }));
                DryContactCollection.Add(ViewModelSource.Create(() => new DryContactItem() 
                { 
                    DryContactItemID = DryContactCollection.Count + 1, 
                    Description = "放电二级过流保护", 
                    CommandType = 0x94, 
                    Description_US = "DCH OC Level2 Protection", 
                    CurrentValue = message[19] == 0x00 ? "开" : "关" 
                }));
                DryContactCollection.Add(ViewModelSource.Create(() => new DryContactItem() 
                { 
                    DryContactItemID = DryContactCollection.Count + 1, 
                    Description = "充电高温保护", 
                    CommandType = 0x95, 
                    Description_US = "CH OT Protection", 
                    CurrentValue = message[20] == 0x00 ? "开" : "关" 
                }));
                DryContactCollection.Add(ViewModelSource.Create(() => new DryContactItem() 
                { 
                    DryContactItemID = DryContactCollection.Count + 1, 
                    Description = "放电高温告警", 
                    CommandType = 0x96, 
                    Description_US = "DCH OT Warning", 
                    CurrentValue = message[21] == 0x00 ? "开" : "关" 
                }));
                DryContactCollection.Add(ViewModelSource.Create(() => new DryContactItem() 
                { 
                    DryContactItemID = DryContactCollection.Count + 1, 
                    Description = "放电高温保护", 
                    CommandType = 0x97, 
                    Description_US = "DCH OT Protection", 
                    CurrentValue = message[22] == 0x00 ? "开" : "关" 
                }));
                DryContactCollection.Add(ViewModelSource.Create(() => new DryContactItem() 
                { 
                    DryContactItemID = DryContactCollection.Count + 1, 
                    Description = "充电低温保护", 
                    CommandType = 0x98, 
                    Description_US = "CH UT Protection", 
                    CurrentValue = message[23] == 0x00 ? "开" : "关" 
                }));
                DryContactCollection.Add(ViewModelSource.Create(() => new DryContactItem() 
                { 
                    DryContactItemID = DryContactCollection.Count + 1, 
                    Description = "放电低温告警", 
                    CommandType = 0x99, 
                    Description_US = "DCH UT Warning", 
                    CurrentValue = message[24] == 0x00 ? "开" : "关" 
                }));
                DryContactCollection.Add(ViewModelSource.Create(() => new DryContactItem() 
                { 
                    DryContactItemID = DryContactCollection.Count + 1, 
                    Description = "放电低温保护", 
                    CommandType = 0x9A, 
                    Description_US = "DCH UT Protection", 
                    CurrentValue = message[25] == 0x00 ? "开" : "关" 
                }));
                DryContactCollection.Add(ViewModelSource.Create(() => new DryContactItem() 
                { 
                    DryContactItemID = DryContactCollection.Count + 1, 
                    Description = "环境温度高告警", 
                    CommandType = 0x9B, 
                    Description_US = "Ambient T High Warning", 
                    CurrentValue = message[26] == 0x00 ? "开" : "关" 
                }));
                DryContactCollection.Add(ViewModelSource.Create(() => new DryContactItem() 
                { 
                    DryContactItemID = DryContactCollection.Count + 1, 
                    Description = "环境温度低告警", 
                    CommandType = 0x9C, 
                    Description_US = "Ambient T Low Warning", 
                    CurrentValue = message[27] == 0x00 ? "开" : "关" 
                }));
                DryContactCollection.Add(ViewModelSource.Create(() => new DryContactItem() 
                { 
                    DryContactItemID = DryContactCollection.Count + 1, 
                    Description = "电池容量低告警", 
                    CommandType = 0x9D, 
                    Description_US = "Capacity Low Warning", 
                    CurrentValue = message[28] == 0x00 ? "开" : "关" 
                }));
                DryContactCollection.Add(ViewModelSource.Create(() => new DryContactItem() 
                { 
                    DryContactItemID = DryContactCollection.Count + 1, 
                    Description = "电池容量低保护", 
                    CommandType = 0x9E, 
                    Description_US = "Capacity Low Protection", 
                    CurrentValue = message[29] == 0x00 ? "开" : "关" 
                }));
                DryContactCollection.Add(ViewModelSource.Create(() => new DryContactItem() 
                { 
                    DryContactItemID = DryContactCollection.Count + 1, 
                    Description = "短路保护", 
                    CommandType = 0x9F, 
                    Description_US = "Short Circuit Protection", 
                    CurrentValue = message[30] == 0x00 ? "开" : "关" }));
                DryContactCollection.Add(ViewModelSource.Create(() => new DryContactItem() 
                { 
                    DryContactItemID = DryContactCollection.Count + 1, 
                    Description = "系统失效", 
                    CommandType = 0xA0, 
                    Description_US = "System Failure", 
                    CurrentValue = message[31] == 0x00 ? "开" : "关" 
                }));

                if (SerialPortCommunicator.ManufacturerName == "ZTE-C" && (SerialPortCommunicator.ZTE_Type == "FB150C"))
                {
                    DryContactCollection.Add(ViewModelSource.Create(() => new DryContactItem() 
                    { 
                        DryContactItemID = DryContactCollection.Count + 1, 
                        Description = "SOC保护使能", 
                        CommandType = 0xA1, 
                        Description_US = "SOC Protection", 
                        CurrentValue = message[32] == 0x00 ? "开" : "关" 
                    }));
                }

                if (SerialPortCommunicator.ManufacturerName == "ZTE-C" && SerialPortCommunicator.ZTE_Type != "FB150C")
                {
                    DryContactCollection.Add(ViewModelSource.Create(() => new DryContactItem() 
                    { 
                        DryContactItemID = DryContactCollection.Count + 1, 
                        Description = "SOH告警使能", 
                        CommandType = 0xA2, Description_US = "SOH Warning", CurrentValue = message[33] == 0x00 ? "开" : "关" 
                    }));
                    DryContactCollection.Add(ViewModelSource.Create(() => new DryContactItem() 
                    { 
                        DryContactItemID = DryContactCollection.Count + 1, 
                        Description = "SOH保护使能", 
                        CommandType = 0xA3, 
                        Description_US = "SOH Protection",
                        CurrentValue = message[34] == 0x00 ? "开" : "关" 
                    }));
                    DryContactCollection.Add(ViewModelSource.Create(() => new DryContactItem() 
                    { 
                        DryContactItemID = DryContactCollection.Count + 1, 
                        Description = "电芯一致性差告警使能", 
                        CommandType = 0xA4, 
                        Description_US = "Cell Consistency Warning", 
                        CurrentValue = message[35] == 0x00 ? "开" : "关" 
                    }));
                    DryContactCollection.Add(ViewModelSource.Create(() => new DryContactItem() 
                    { 
                        DryContactItemID = DryContactCollection.Count + 1, 
                        Description = "电芯一致性差保护", 
                        CommandType = 0xA5, 
                        Description_US = "Cell Consistency Protection", 
                        CurrentValue = message[36] == 0x00 ? "开" : "关" 
                    }));
                    DryContactCollection.Add(ViewModelSource.Create(() => new DryContactItem() 
                    { 
                        DryContactItemID = DryContactCollection.Count + 1, 
                        Description = "电芯损坏保护", 
                        CommandType = 0xA6, 
                        Description_US = "Cell Damage Protection", 
                        CurrentValue = message[37] == 0x00 ? "开" : "关" 
                    }));
                    DryContactCollection.Add(ViewModelSource.Create(() => new DryContactItem() 
                    { 
                        DryContactItemID = DryContactCollection.Count + 1, 
                        Description = "充电过流保护", 
                        CommandType = 0xA7, 
                        Description_US = "CH OC Protection", 
                        CurrentValue = message[38] == 0x00 ? "开" : "关" 
                    }));
                    DryContactCollection.Add(ViewModelSource.Create(() => new DryContactItem() 
                    { 
                        DryContactItemID = DryContactCollection.Count + 1, 
                        Description = "电池被盗", 
                        CommandType = 0xA8, 
                        Description_US = "Batteries Stolen", 
                        CurrentValue = message[39] == 0x00 ? "开" : "关" 
                    }));
                }
            }
            else
            {
                DryContactCollection.Add(ViewModelSource.Create(() => new DryContactItem() 
                { 
                    DryContactItemID = DryContactCollection.Count + 1, 
                    Description = "充电高压告警", 
                    Description_US = "CH OV Warning", 
                    CommandType = 0x88, 
                    CurrentValue = message[7] == 0x00 ? "On" : "Off" 
                }));
                DryContactCollection.Add(ViewModelSource.Create(() => new DryContactItem() 
                { 
                    DryContactItemID = DryContactCollection.Count + 1, 
                    Description = "充电高压保护", 
                    Description_US = "CH OV Protection", 
                    CommandType = 0x89, 
                    CurrentValue = message[8] == 0x00 ? "On" : "Off" 
                }));
                DryContactCollection.Add(ViewModelSource.Create(() => new DryContactItem() 
                { 
                    DryContactItemID = DryContactCollection.Count + 1, 
                    Description = "放电欠压告警", 
                    Description_US = "DCH UV Warning", 
                    CommandType = 0x8A, 
                    CurrentValue = message[9] == 0x00 ? "On" : "Off" 
                }));
                DryContactCollection.Add(ViewModelSource.Create(() => new DryContactItem() 
                { 
                    DryContactItemID = DryContactCollection.Count + 1, 
                    Description = "放电欠压保护", 
                    Description_US = "DCH UV Protection", 
                    CommandType = 0x8B, 
                    CurrentValue = message[10] == 0x00 ? "On" : "Off" 
                }));
                DryContactCollection.Add(ViewModelSource.Create(() => new DryContactItem() 
                { 
                    DryContactItemID = DryContactCollection.Count + 1, 
                    Description = "单体高压告警", 
                    Description_US = "Cell OV Warning", 
                    CommandType = 0x8C, 
                    CurrentValue = message[11] == 0x00 ? "On" : "Off" 
                }));
                DryContactCollection.Add(ViewModelSource.Create(() => new DryContactItem() 
                { 
                    DryContactItemID = DryContactCollection.Count + 1, 
                    Description = "单体高压保护", 
                    Description_US = "Cell OV Protection", 
                    CommandType = 0x8D, 
                    CurrentValue = message[12] == 0x00 ? "On" : "Off" 
                }));
                DryContactCollection.Add(ViewModelSource.Create(() => new DryContactItem() 
                { 
                    DryContactItemID = DryContactCollection.Count + 1, 
                    Description = "单体欠压告警", 
                    Description_US = "Cell UV Warning", 
                    CommandType = 0x8E, 
                    CurrentValue = message[13] == 0x00 ? "On" : "Off" 
                }));
                DryContactCollection.Add(ViewModelSource.Create(() => new DryContactItem() 
                { 
                    DryContactItemID = DryContactCollection.Count + 1, 
                    Description = "单体欠压保护", 
                    Description_US = "Cell UV Protection", 
                    CommandType = 0x8F, 
                    CurrentValue = message[14] == 0x00 ? "On" : "Off" 
                }));
                if (SerialPortCommunicator.ManufacturerName == "ZTE-C" && SerialPortCommunicator.ZTE_Type == "FB150C")
                {
                    DryContactCollection.Add(ViewModelSource.Create(() => new DryContactItem() 
                    { 
                        DryContactItemID = DryContactCollection.Count + 1, 
                        Description = "充电过流保护", 
                        Description_US = "CH OC Protection", 
                        CommandType = 0x90, 
                        CurrentValue = message[15] == 0x00 ? "On" : "Off" 
                    }));
                    DryContactCollection.Add(ViewModelSource.Create(() => new DryContactItem() 
                    { 
                        DryContactItemID = DryContactCollection.Count + 1, 
                        Description = "充电二级过流保护", 
                        Description_US = "CH OC Level2 Protection", 
                        CommandType = 0x91, 
                        CurrentValue = message[16] == 0x00 ? "On" : "Off" 
                    }));
                }
                else
                {
                    DryContactCollection.Add(ViewModelSource.Create(() => new DryContactItem() 
                    { 
                        DryContactItemID = DryContactCollection.Count + 1, 
                        Description = "充电过流告警", 
                        CommandType = 0x90, 
                        Description_US = "CH OC Warning", 
                        CurrentValue = message[15] == 0x00 ? "On" : "Off" 
                    }));
                    DryContactCollection.Add(ViewModelSource.Create(() => new DryContactItem() 
                    { 
                        DryContactItemID = DryContactCollection.Count + 1, 
                        Description = "充电二级过流告警", 
                        CommandType = 0x91, 
                        Description_US = "CH OC Level2 Warning", 
                        CurrentValue = message[16] == 0x00 ? "On" : "Off" 
                    }));
                }

                DryContactCollection.Add(ViewModelSource.Create(() => new DryContactItem() 
                { 
                    DryContactItemID = DryContactCollection.Count + 1, 
                    Description = "放电过流告警", 
                    Description_US = "DCH OC Warning", 
                    CommandType = 0x92, 
                    CurrentValue = message[17] == 0x00 ? "On" : "Off" 
                }));
                DryContactCollection.Add(ViewModelSource.Create(() => new DryContactItem() 
                { 
                    DryContactItemID = DryContactCollection.Count + 1, 
                    Description = "放电过流保护", 
                    Description_US = "DCH OC Protection", 
                    CommandType = 0x93, 
                    CurrentValue = message[18] == 0x00 ? "On" : "Off" 
                }));
                DryContactCollection.Add(ViewModelSource.Create(() => new DryContactItem() 
                { 
                    DryContactItemID = DryContactCollection.Count + 1, 
                    Description = "放电二级过流保护", 
                    Description_US = "DCH OC Level2 Protection", 
                    CommandType = 0x94, 
                    CurrentValue = message[19] == 0x00 ? "On" : "Off" 
                }));
                DryContactCollection.Add(ViewModelSource.Create(() => new DryContactItem() 
                { 
                    DryContactItemID = DryContactCollection.Count + 1, 
                    Description = "充电高温保护", 
                    Description_US = "CH OT Protection", 
                    CommandType = 0x95, 
                    CurrentValue = message[20] == 0x00 ? "On" : "Off" 
                }));
                DryContactCollection.Add(ViewModelSource.Create(() => new DryContactItem() 
                { 
                    DryContactItemID = DryContactCollection.Count + 1, 
                    Description = "放电高温告警", 
                    Description_US = "DCH OT Warning", 
                    CommandType = 0x96, 
                    CurrentValue = message[21] == 0x00 ? "On" : "Off" 
                }));
                DryContactCollection.Add(ViewModelSource.Create(() => new DryContactItem() 
                { 
                    DryContactItemID = DryContactCollection.Count + 1,
                    Description = "放电高温保护", 
                    Description_US = "DCH OT Protection", 
                    CommandType = 0x97, 
                    CurrentValue = message[22] == 0x00 ? "On" : "Off" 
                }));
                DryContactCollection.Add(ViewModelSource.Create(() => new DryContactItem() 
                { 
                    DryContactItemID = DryContactCollection.Count + 1, 
                    Description = "充电低温保护", 
                    Description_US = "CH UT Protection", 
                    CommandType = 0x98, 
                    CurrentValue = message[23] == 0x00 ? "On" : "Off" 
                }));
                DryContactCollection.Add(ViewModelSource.Create(() => new DryContactItem() 
                { 
                    DryContactItemID = DryContactCollection.Count + 1, 
                    Description = "放电低温告警", 
                    Description_US = "DCH UT Warning", 
                    CommandType = 0x99, 
                    CurrentValue = message[24] == 0x00 ? "On" : "Off" 
                }));
                DryContactCollection.Add(ViewModelSource.Create(() => new DryContactItem() 
                { 
                    DryContactItemID = DryContactCollection.Count + 1, 
                    Description = "放电低温保护", 
                    Description_US = "DCH UT Protection", 
                    CommandType = 0x9A, 
                    CurrentValue = message[25] == 0x00 ? "On" : "Off" 
                }));
                DryContactCollection.Add(ViewModelSource.Create(() => new DryContactItem() 
                { 
                    DryContactItemID = DryContactCollection.Count + 1, 
                    Description = "环境温度高告警", 
                    Description_US = "Ambient T High Warning", 
                    CommandType = 0x9B, 
                    CurrentValue = message[26] == 0x00 ? "On" : "Off" 
                }));
                DryContactCollection.Add(ViewModelSource.Create(() => new DryContactItem() 
                { 
                    DryContactItemID = DryContactCollection.Count + 1, 
                    Description = "环境温度低告警", 
                    Description_US = "Ambient T Low Warning", 
                    CommandType = 0x9C, 
                    CurrentValue = message[27] == 0x00 ? "On" : "Off" 
                }));
                DryContactCollection.Add(ViewModelSource.Create(() => new DryContactItem() 
                { 
                    DryContactItemID = DryContactCollection.Count + 1, 
                    Description = "电池容量低告警", 
                    Description_US = "Capacity Low Warning", 
                    CommandType = 0x9D, 
                    CurrentValue = message[28] == 0x00 ? "On" : "Off" 
                }));
                DryContactCollection.Add(ViewModelSource.Create(() => new DryContactItem() 
                { 
                    DryContactItemID = DryContactCollection.Count + 1, 
                    Description = "电池容量低保护", 
                    Description_US = "Capacity Low Protection", 
                    CommandType = 0x9E, 
                    CurrentValue = message[29] == 0x00 ? "On" : "Off" 
                }));
                DryContactCollection.Add(ViewModelSource.Create(() => new DryContactItem() 
                { 
                    DryContactItemID = DryContactCollection.Count + 1, 
                    Description = "短路保护", 
                    Description_US = "Short Circuit Protection", 
                    CommandType = 0x9F, 
                    CurrentValue = message[30] == 0x00 ? "On" : "Off" 
                }));
                DryContactCollection.Add(ViewModelSource.Create(() => new DryContactItem() 
                { 
                    DryContactItemID = DryContactCollection.Count + 1, 
                    Description = "系统失效", 
                    Description_US = "System Failure", 
                    CommandType = 0xA0, 
                    CurrentValue = message[31] == 0x00 ? "On" : "Off" 
                }));
                DryContactCollection.Add(ViewModelSource.Create(() => new DryContactItem() 
                { 
                    DryContactItemID = DryContactCollection.Count + 1, 
                    Description = "SOC保护使能", 
                    Description_US = "SOC Protection", 
                    CommandType = 0xA1, 
                    CurrentValue = message[32] == 0x00 ? "On" : "Off" 
                }));
                if (SerialPortCommunicator.ManufacturerName == "ZTE-C" && (SerialPortCommunicator.ZTE_Type == "FB100C1" || SerialPortCommunicator.ZTE_Type == "FB100C5"))
                {
                    DryContactCollection.Add(ViewModelSource.Create(() => new DryContactItem() 
                    { 
                        DryContactItemID = DryContactCollection.Count + 1, 
                        Description = "SOH告警使能", 
                        CommandType = 0xA2, 
                        Description_US = "SOH Warning", 
                        CurrentValue = message[33] == 0x00 ? "On" : "Off" 
                    }));
                    DryContactCollection.Add(ViewModelSource.Create(() => new DryContactItem() 
                    { 
                        DryContactItemID = DryContactCollection.Count + 1, 
                        Description = "SOH保护使能", 
                        CommandType = 0xA3, 
                        Description_US = "SOH Protection", 
                        CurrentValue = message[34] == 0x00 ? "On" : "Off" 
                    }));
                    DryContactCollection.Add(ViewModelSource.Create(() => new DryContactItem() 
                    { 
                        DryContactItemID = DryContactCollection.Count + 1, 
                        Description = "电芯一致性差告警使能", 
                        CommandType = 0xA4, 
                        Description_US = "Cell Consistency Warning", 
                        CurrentValue = message[35] == 0x00 ? "On" : "Off" 
                    }));
                    DryContactCollection.Add(ViewModelSource.Create(() => new DryContactItem() 
                    { 
                        DryContactItemID = DryContactCollection.Count + 1, 
                        Description = "电芯一致性差保护", 
                        CommandType = 0xA5, 
                        Description_US = "Cell Consistency Protection", 
                        CurrentValue = message[36] == 0x00 ? "On" : "Off" 
                    }));
                    DryContactCollection.Add(ViewModelSource.Create(() => new DryContactItem() 
                    { 
                        DryContactItemID = DryContactCollection.Count + 1, 
                        Description = "电芯损坏保护", 
                        CommandType = 0xA6, 
                        Description_US = "Cell Damage Protection", 
                        CurrentValue = message[37] == 0x00 ? "On" : "Off" 
                    }));
                    DryContactCollection.Add(ViewModelSource.Create(() => new DryContactItem() 
                    { 
                        DryContactItemID = DryContactCollection.Count + 1, 
                        Description = "充电过流保护", 
                        CommandType = 0xA7, 
                        Description_US = "CH OC Protection", 
                        CurrentValue = message[38] == 0x00 ? "On" : "Off" 
                    }));
                    DryContactCollection.Add(ViewModelSource.Create(() => new DryContactItem() 
                    { 
                        DryContactItemID = DryContactCollection.Count + 1, 
                        Description = "电池被盗", CommandType = 0xA8, 
                        Description_US = "Batteries Stolen", 
                        CurrentValue = message[39] == 0x00 ? "On" : "Off" 
                    }));
                }

            }

            IsLoadingSpecialSystemParameter = false;
        }
        private void ParseMessage_DryContactStatus_FB100C5(byte[] message)
        {
            if (message.Length != 0x2a)
            {
                return;
            }
            int packAddress = message[2] - 1;
            try
            {
                if (DryContact2Collection.Count != 0)
                {
                    for (int i = 0; i < DryContact2Collection.Count; i++)
                    {
                        if (IsChineseUI)
                        {
                            if (i < 9)
                            {
                                DryContact2Collection[i].CurrentValue = message[7 + i] == 0x00 ? "开" : "关";
                            }
                            else if (i >= 9 && i < 11)
                            {
                                DryContact2Collection[i].CurrentValue = message[7 + i + 1] == 0x00 ? "开" : "关";
                            }
                            else
                            {
                                DryContact2Collection[i].CurrentValue = message[7 + i + 2] == 0x00 ? "开" : "关";
                            }
                        }
                        else
                        {
                            if (i < 9)
                            {
                                DryContact2Collection[i].CurrentValue = message[7 + i] == 0x00 ? "On" : "Off";
                            }
                            else if (i >= 9 && i < 11)
                            {
                                DryContact2Collection[i].CurrentValue = message[7 + i + 1] == 0x00 ? "On" : "Off";
                            }
                            else
                            {
                                DryContact2Collection[i].CurrentValue = message[7 + i + 2] == 0x00 ? "On" : "Off";
                            }
                        }
                    }

                    IsLoadingSpecialSystemParameter = false;
                }
            }
            catch { }
        }
        private void ParseMessage_DryContact2Status_FB100C5(byte[] message)
        {
            if (message.Length != 0x2a)
            {
                return;
            }
            int packAddress = message[2] - 1;
            try
            {
                if (DryContact2Collection.Count != 0)
                {
                    for (int i = 0; i < DryContact2Collection.Count; i++)
                    {
                        if (i < 9)
                        {
                            DryContact2Collection[i].CurrentDryContactValue = message[7 + i] == 0x00 ? "1" : "2";
                        }
                        else if (i >= 9 && i < 11)
                        {
                            DryContact2Collection[i].CurrentDryContactValue = message[7 + i + 1] == 0x00 ? "1" : "2";
                        }
                        else
                        {
                            DryContact2Collection[i].CurrentDryContactValue = message[7 + i + 2] == 0x00 ? "1" : "2";
                        }
                    }

                    IsLoadingSpecialSystemParameter = false;
                }
            }
            catch { }
        }
        //读取4G参数
        private void ParseMessage_4GParameter_Coslight(byte[] message)//202000509
        {
            int packAddress = message[2] - 1;
            int offset = 7;

            for (int i = 0; i < message.Length; i++)
            {
                if (message[i] == 0)
                {
                    message[i] = 0x20;
                }
            }
            if (ProtocalProvider.BatteryGroupCount == 0xFF)
            {
                offset++;
            }

            Parameter_4G_AdministratorMobilePhoneNumber = Encoding.ASCII.GetString(message, offset, 20).Trim();

            offset += 20;

            Parameter_4G_CellPhoneNumber1 = Encoding.ASCII.GetString(message, offset, 20).Trim();

            offset += 20;

            Parameter_4G_CellPhoneNumber2 = Encoding.ASCII.GetString(message, offset, 20).Trim();

            offset += 20;
            Parameter_4G_CellPhoneNumber3 = Encoding.ASCII.GetString(message, offset, 20).Trim();

            offset += 20;
            Parameter_4G_CellPhoneNumber4 = Encoding.ASCII.GetString(message, offset, 20).Trim();

            offset += 20;
            Parameter_4G_CellPhoneNumber5 = Encoding.ASCII.GetString(message, offset, 20).Trim();

            offset += 20;
            try
            {
                WlanParameterCollection[0]._4GHardwareVersion = Encoding.ASCII.GetString(message, offset, 20).Trim();

                offset += 20;

                WlanParameterCollection[0]._4GSoftwareVersion = Encoding.ASCII.GetString(message, offset, 20).Trim();

                offset += 20;

                WlanParameterCollection[0].ModelType = Encoding.ASCII.GetString(message, offset, 20).Trim();

                offset += 20;
                if (message[offset] == 0x20)
                { message[offset] = 0; }
                if (message[offset + 1] == 0x20)
                { message[offset + 1] = 0; }
                WlanParameterCollection[0].HeartbeatCycle = WlanParameter_HeartbeatCycle = BitConverter.ToUInt16(new byte[] { message[offset + 1], message[offset] }, 0).ToString();

                offset += 2;

                WlanParameterCollection[0].APN = WlanParameter_APN = Encoding.ASCII.GetString(message, offset, 20).Trim();

                offset += 20;


                WlanParameterCollection[0].APN_Account = WlanParameter_APN_Account = Encoding.ASCII.GetString(message, offset, 20).Trim();

                offset += 20;

                WlanParameterCollection[0].APN_Password = WlanParameter_APN_Password = Encoding.ASCII.GetString(message, offset, 20).Trim();

                offset += 20;
            }
            catch { }

            IsLoading_4GSystemParameter = false;
        }
        //读取GPS参数
        private void ParseMessage_GPSParameter_Coslight(byte[] message)//20200509
        {
            int packAddress = message[2] - 1;
            int offset = 7;

            if (ProtocalProvider.BatteryGroupCount == 0xFF)
            {
                offset++;
            }

            for (int i = 0; i < message.Length; i++)
            {
                if (message[i] == 0)
                {
                    message[i] = 0x20;
                }
            }

            ParameterGPS_StationID = Encoding.ASCII.GetString(message, offset, 20).Trim();

            offset += 20;

            ParameterGPS_Longitude = Encoding.ASCII.GetString(message, offset, 20).Trim();

            offset += 20;

            ParameterGPS_Latitude = Encoding.ASCII.GetString(message, offset, 20).Trim();

            offset += 20;

            ParameterGPS_Altitude = Encoding.ASCII.GetString(message, offset, 20).Trim();

            offset += 20;

            ParameterGPS_TimeZone = Encoding.ASCII.GetString(message, offset, 8).Trim();

            if (!string.IsNullOrEmpty(ParameterGPS_TimeZone))
            {
                int hour = int.Parse(ParameterGPS_TimeZone.Substring(0, ParameterGPS_TimeZone.IndexOf(":")));
                int minite = int.Parse(ParameterGPS_TimeZone.Substring(ParameterGPS_TimeZone.IndexOf(":") + 1));
                TimeZoneHour_Local = hour;
                TimeZoneMinutes_Local = minite;
            }

            offset += 8;

            IsLoadingGPSSystemParameter = false;
        }//设备事件记录
        //读取GPS实时数据
        private void ParseMessage_GPSAnolog_Coslight(byte[] message)//20200520
        {
            int offset = 7;
            int packAddress = message[2] - 1;

            for (int i = 0; i < message.Length; i++)
            {
                if (message[i] == 0)
                {
                    message[i] = 0x20;
                }
            }

            WlanParameterCollection[0].Longitude = Encoding.ASCII.GetString(message, offset, 20).Trim();
            offset += 20;

            WlanParameterCollection[0].Latitude = Encoding.ASCII.GetString(message, offset, 20).Trim();
            offset += 20;

            WlanParameterCollection[0].Altitude = Encoding.ASCII.GetString(message, offset, 20).Trim();
            offset += 20;

            WlanParameterCollection[0].NSAT = Encoding.ASCII.GetString(message, offset, 20).Trim();
            offset += 20;

            WlanParameterCollection[0].Date = Encoding.ASCII.GetString(message, offset, 20).Trim();
            Date_Local = Encoding.ASCII.GetString(message, offset, 20).Trim();
            offset += 20;

            WlanParameterCollection[0].UTC = Encoding.ASCII.GetString(message, offset, 20).Trim();

            UTC_Local = Encoding.ASCII.GetString(message, offset, 20).Trim();
            offset += 20;

            try
            {
                string dt = "20" + Date_Local.Substring(4, 2) + "/" + Date_Local.Substring(2, 2) + "/" + Date_Local.Substring(0, 2) + " " + UTC_Local.Substring(0, 2) + ":" + UTC_Local.Substring(2, 2) + ":" + UTC_Local.Substring(4, 2);
                DateTime datetime; 
                if (DateTime.TryParse(dt, out datetime))
                {
                    if (UTC_Local != "" && UTC_Local != null)
                    {
                        string LTM = DateTime.Parse(dt).AddHours(TimeZoneHour_Local).ToString();
                        LTM = DateTime.Parse(LTM).AddMinutes(TimeZoneMinutes_Local).ToString();

                        LTM_Local = LTM;

                        WlanParameterCollection[0].LTM = LTM_Local;
                    }
                }
            }
            catch { }
        }
        //读取4G_GPS特殊数据
        private void ParseMessage_4G_GPSSpecialParameter_Coslight(byte[] message)
        {
            int packAddress = message[2] - 1;

            if (IsChineseUI)
            {
                WlanEnableControlCollection[0].State_4G = message[7] == 0x01 ? "运行" : "休眠";
                WlanEnableControlCollection[0].State_GPS = message[8] == 0x01 ? "运行" : "休眠";
                WlanEnableControlCollection[0].GPSTheftPrevention = message[9] == 0x01 ? "开" : "关";
                WlanEnableControlCollection[0].CommunicationTimeoutTheftPrevention = message[10] == 0x01 ? "开" : "关";
                WlanEnableControlCollection[0].AlarmBuzzer = message[11] == 0x01 ? "开" : "关";
                WlanEnableControlCollection[0].AlarmMessage = message[12] == 0x01 ? "开" : "关";
                WlanEnableControlCollection[0].AlarmTest = message[13] == 0x01 ? "开" : "关";
            }
            else
            {
                WlanEnableControlCollection[0].State_4G = message[7] == 0x01 ? "Function" : "Dormancy";
                WlanEnableControlCollection[0].State_GPS = message[8] == 0x01 ? "Function" : "Dormancy";
                WlanEnableControlCollection[0].GPSTheftPrevention = message[9] == 0x01 ? "On" : "Off";
                WlanEnableControlCollection[0].CommunicationTimeoutTheftPrevention = message[10] == 0x01 ? "On" : "Off";
                WlanEnableControlCollection[0].AlarmBuzzer = message[11] == 0x01 ? "On" : "Off";
                WlanEnableControlCollection[0].AlarmMessage = message[12] == 0x01 ? "On" : "Off";
                WlanEnableControlCollection[0].AlarmTest = message[13] == 0x01 ? "On" : "Off";
            }
            IsLoadingGPS_4GEnableControlParameter = false;
        }
        //检查文件名字
        private BootLoaderFileNameCheck checkFileNameRead(string bootLaoderFileNameRead)
        {
            bootLoaderFileNameCheck = BootLoaderFileNameCheck.OK;

            if (isBootRead)
            {
                if (COSPowerCodeRead != "CNCT")
                {
                    bootLoaderFileNameCheck = BootLoaderFileNameCheck.光宇代码错误;
                }
                else if (String.Compare(DevelopmentPhaseRead, "boot", true) != 0)
                {
                    bootLoaderFileNameCheck = BootLoaderFileNameCheck.其他错误;
                }
                else
                {
                    BootLaoderFileNameWrite = BootLoaderFileName;//20200703
                    bootLoaderFileNameCheck = checkFileNameDiff(BootLaoderFileNameRead, BootLaoderFileNameWrite);
                }
            }

            if (isApplicationRead)
            {
                if (bootLaoderFileNameRead == "FF")
                {
                    bootLoaderFileNameCheck = BootLoaderFileNameCheck.OK;

                    BootLaoderFileNameWrite = ApplicationFileName01;//20200703

                    bootLoaderFileNameCheck = checkFileNameDiff(BootLaoderFileNameRead, BootLaoderFileNameWrite);
                }
                else if (COSPowerCodeRead != "CNCT")
                {
                    bootLoaderFileNameCheck = BootLoaderFileNameCheck.光宇代码错误;
                }
                else if (OtherRead.Substring(0, 2) != "TB")
                {
                    bootLoaderFileNameCheck = BootLoaderFileNameCheck.电芯材料错误;
                }
                else
                {
                    if (VersionNumber2Read == "0001")
                    {
                        BootLaoderFileNameWrite = ApplicationFileName02;//20200703
                    }
                    else
                    {
                        BootLaoderFileNameWrite = ApplicationFileName01;//20200703
                    }
                    bootLoaderFileNameCheck = checkFileNameDiff(BootLaoderFileNameRead, BootLaoderFileNameWrite);
                }
            }

            return bootLoaderFileNameCheck;
        }
        private BootLoaderFileNameCheck checkFileNameDiff(string bootLaoderFileNameRead, string bootLaoderFileNameWrite)
        {
            bootLoaderFileNameCheck = BootLoaderFileNameCheck.OK;

            string[] temp = Regex.Split(bootLaoderFileNameWrite, "_", RegexOptions.IgnoreCase);

            if (isBootRead)
            {
                ProjectCodeWrite = "";
                COSPowerCodeWrite = temp[0];
                OtherWrite = temp[1];
                DevelopmentPhaseWrite = temp[2];
                VersionNumber1Write = temp[3];
                VersionNumber2Write = "";
                VersionNumber3Write = temp[4];

                if (OtherRead != OtherWrite)
                {
                    bootLoaderFileNameCheck = BootLoaderFileNameCheck.CPU型号不一致;
                }
                else if (VersionNumber3Read != VersionNumber3Write)
                {
                    bootLoaderFileNameCheck = BootLoaderFileNameCheck.版本号不一致;
                }
            }

            if (isApplicationRead)
            {
                ProjectCodeWrite = temp[0];
                COSPowerCodeWrite = temp[1];
                DevelopmentPhaseWrite = temp[2];
                VersionNumber1Write = temp[3];
                VersionNumber2Write = temp[4];
                OtherWrite = temp[5];
                VersionNumber3Write = temp[6];

                if (bootLaoderFileNameRead == "FF")
                {
                    bootLoaderFileNameCheck = BootLoaderFileNameCheck.OK;
                }
                else if (ProjectCodeRead != ProjectCodeWrite)
                {
                    bootLoaderFileNameCheck = BootLoaderFileNameCheck.项目代码不一致;
                }
                else if (uint.Parse(VersionNumber2Read) % 2 == uint.Parse(VersionNumber2Write) % 2)
                {
                    bootLoaderFileNameCheck = BootLoaderFileNameCheck.奇偶校验错误;
                }

            }
            return bootLoaderFileNameCheck;
        }
        internal void WriteBootLoaderFileName(string BootLaoderFileNameWrite)//20200515//20200608
        {
            communicator.UserAction(ProtocalProvider.GetMessage_SetBootLaoderFileName(SerialPortCommunicator.IsBootLaoderBMSConnection, BootLaoderFileNameWrite));
        }
        internal void BootLoaderWirteNumber(byte[] data)//20200701
        {
            communicator.UserAction(ProtocalProvider.GetMessage_SetBootLaoderNumber(SerialPortCommunicator.IsBootLaoderBMSConnection, data));
        }
        public void Application_Updates()
        {
            WriteBootLoaderFileName(BootLaoderFileNameWrite);
        }
        //将 frameCode 解析成 byte 并写入 BootLoader
        public void BootLoader_Updates()
        {
            byte[] byteData = new byte[1];
            byteData[0] = byte.Parse(frameCode.ToString());
            BootLoaderWirteNumber(byteData);
        }
        //负责调用 ProtocalProvider.GetMessage_SetBootLaoderParameter() 生成 BootLoader 设置指令，并通过 communicator.UserAction() 发送
        internal void BootLoaderUpdates(int commandType, byte[] data)//20200502
        {
            communicator.UserAction(ProtocalProvider.GetMessage_SetBootLaoderParameter(SerialPortCommunicator.IsBootLaoderBMSConnection, commandType, data));
        }
        //下载文件
        private void LoaderFile(string bootLaoderFileNameWrite)
        {
            FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "Temp\\" + FileFolder + "\\" + bootLaoderFileNameWrite + ".bin", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            BinaryReader br = new BinaryReader(fs);

            byte[] byteData = new byte[512 * 2];

            if (fs.Length % (512 * 2) == 0)
            {
                frameCode = int.Parse((fs.Length / (512 * 2)).ToString());
            }
            else
            {
                frameCode = int.Parse((fs.Length / (512 * 2) + 1).ToString());
            }

            for (int j = 0; j < frameCode; j++)
            {
                for (int i = 0; i < 512 * 2; i++)
                {
                    byteDatalong[j, i] = 0xFF;
                }
            }

            for (int j = 0; j < frameCode; j++)
            {
                byteData = br.ReadBytes(512 * 2);

                if (j == frameCode - 1)
                {
                    for (int i = 0; i < fs.Length % (512 * 2); i++)
                    {
                        byteDatalong[j, i] = byteData[i];
                    }
                }
                else
                {
                    for (int i = 0; i < 512 * 2; i++)
                    {
                        byteDatalong[j, i] = byteData[i];
                    }
                }
            }
            fs.Close();
            fs.Dispose();

            BarBindingVal = 0;
        }
        public void Updates()
        {
            LoaderFile(BootLaoderFileNameWrite);
            if (Pack.IsChineseUI)
            {
                StateContectInformation = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：BMS已连接，开始更新！";
            }
            else
            {
                StateContectInformation = DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy") + ":BMS is connected, update starting!";
            }

            if (SerialPortCommunicator.BLStatus == BootLaoderStatus.应用程序已连接)
            {
                Application_Updates();
            }
            else if (SerialPortCommunicator.BLStatus == BootLaoderStatus.BootLoader已连接)
            {
                BootLoader_Updates();
            }

            IDWrite = 0;
        }
        //应用程序连接
        private void ParseMessage_BootLoaderApplicationConnection_Coslight(byte[] message)//20200502
        {
            try
            {
                if (message[7] == 0xFF)
                {
                    BootLaoderFileNameRead = "FF";
                    ProjectCodeRead = "FF";
                    COSPowerCodeRead = "FF";
                    DevelopmentPhaseRead = "FF";
                    VersionNumber1Read = "FF";
                    VersionNumber2Read = "FF";
                    VersionNumber3Read = "FF";
                    OtherRead = "FF";

                }
                else
                {
                    BootLaoderFileNameRead = Encoding.ASCII.GetString(message, 7, 32 + 5).Trim();
                    COSPowerCodeRead = Encoding.ASCII.GetString(message, 7, 4).Trim();
                    ProjectCodeRead = Encoding.ASCII.GetString(message, 12, 4).Trim();
                    DevelopmentPhaseRead = Encoding.ASCII.GetString(message, 17, 4).Trim();
                    VersionNumber1Read = Encoding.ASCII.GetString(message, 22, 4).Trim();
                    VersionNumber2Read = Encoding.ASCII.GetString(message, 27, 4).Trim();
                    OtherRead = Encoding.ASCII.GetString(message, 32, 7).Trim();
                    VersionNumber3Read = Encoding.ASCII.GetString(message, 40, 4).Trim();
                }

                IsReadParameter = false;

                if (SerialPortCommunicator.BLStatus == BootLaoderStatus.应用程序连接中)
                {
                    if (isErases)
                    {
                        SerialPortCommunicator.BLStatus = BootLaoderStatus.一键擦除中;
                    }
                    else
                    {
                        SerialPortCommunicator.BLStatus = BootLaoderStatus.应用程序已连接;

                        bootLoaderFileNameCheck = checkFileNameRead(BootLaoderFileNameRead);

                        if (bootLoaderFileNameCheck != BootLoaderFileNameCheck.OK)
                        {
                            if (Pack.IsChineseUI)
                            {
                                StateContectInformation = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：文件错误：" + bootLoaderFileNameCheck + "！";
                            }
                            else
                            {
                                StateContectInformation = DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy") + ":Error File:" + (BootLoaderFileNameCheck)((int)bootLoaderFileNameCheck + 0x10) + "!";
                            }
                        }
                        else
                        {
                            Updates();
                        }
                    }
                }
            }
            catch
            {
                SerialPortCommunicator.BLStatus = BootLaoderStatus.应用程序连接中;

                if (Pack.IsChineseUI)
                {
                    StateContectInformation = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：应用程序连接错误！";
                }
                else
                {
                    StateContectInformation = DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy") + ":Error:Application Connection!";
                }
            }
        }
        //bootloader链接
        private void ParseMessage_BootLoaderConnection_Coslight(byte[] message)
        {
            try
            {
                BootLaoderFileNameRead = Encoding.ASCII.GetString(message, 7, 20 + 5).Trim();
                COSPowerCodeRead = Encoding.ASCII.GetString(message, 7, 4).Trim();
                ProjectCodeRead = "";
                OtherRead = Encoding.ASCII.GetString(message, 12, 4).Trim();
                DevelopmentPhaseRead = Encoding.ASCII.GetString(message, 17, 4).Trim();
                VersionNumber1Read = Encoding.ASCII.GetString(message, 22, 4).Trim();
                VersionNumber3Read = Encoding.ASCII.GetString(message, 27, 4).Trim();
                VersionNumber2Read = "";
                if (SerialPortCommunicator.BLStatus == BootLaoderStatus.BootLoader连接中)
                {
                    SerialPortCommunicator.BLStatus = BootLaoderStatus.BootLoader已连接;
                }
                bootLoaderFileNameCheck = checkFileNameRead(BootLaoderFileNameRead);

                if (bootLoaderFileNameCheck != BootLoaderFileNameCheck.OK)
                {
                    if (Pack.IsChineseUI)
                    {
                        StateContectInformation = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：文件错误：" + bootLoaderFileNameCheck + "！";
                    }
                    else
                    {
                        StateContectInformation = DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy") + ":Error File:" + (BootLoaderFileNameCheck)((int)bootLoaderFileNameCheck + 0x10) + "!";
                    }
                }
                else
                {
                    Updates();
                }
            }
            catch
            {
                SerialPortCommunicator.BLStatus = BootLaoderStatus.BootLoader连接中;

                if (Pack.IsChineseUI)
                {
                    StateContectInformation = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：Boot连接错误！";
                }
                else
                {
                    StateContectInformation = DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy") + ":Error:Boot Connection!";
                }
            }
        }
        //设置Bootloader文件名
        private void ParseMessage_BootLoaderWriteFileName_Coslight(byte[] message)
        {
            int packAddress = message[2] - 1;
            int offset = 7;

            if (message[offset] == 0x00)
            {
                if (message[offset + 1] == 0x01)
                {
                    byte[] byteData = new byte[1];
                    byteData[0] = byte.Parse(frameCode.ToString());
                    BootLoaderWirteNumber(byteData);
                }
                else
                {
                    IsReadParameter = false;

                    if (IsChineseUI)
                    {
                        MessageBoxService.Show("写BootLoader文件名失败 !");
                    }

                    else
                    {
                        MessageBoxService.Show("Write BootLoader File Name Failed !");
                    }


                }
            }
        }
        //设置BootLoader连接
        private void ParseMessage_BootLoaderSet_Coslight(byte[] message)
        {
            int packAddress = message[2] - 1;
            int offset = 7;

            if (IsChineseUI)
            {
                StateContectInformation = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + IDWrite + " / " + frameCode + "：更新中！";
            }
            else
            {
                StateContectInformation = DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy") + " " + IDWrite + " / " + frameCode + ":BootLoader updating!";
            }

            if (message[offset + 1] == 0x02)
            {
                for (int i = 0; i < 512 * 2; i++)
                {
                    byteData[i] = byteDatalong[IDWrite, i];
                }
                IDWrite++;
                BootLoaderUpdates(IDWrite, byteData);
            }
            else if (message[offset + 1] == 0x03)
            {
                IDWrite = 0x01;
                BootLoaderUpdates(IDWrite, byteData);
            }
            else if (message[offset + 1] == 0x04)
            {
                if (IDWrite == frameCode)
                {
                    SerialPortCommunicator.BLStatus = BootLaoderStatus.更新完成;
                }
                else
                {
                    SerialPortCommunicator.BLStatus = BootLaoderStatus.终止更新;
                }

                if (IsChineseUI)
                {
                    StateContectInformation = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：更新完成！";
                }

                else
                {
                    StateContectInformation = DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy") + ":Update is finished!";
                }

                isWriteSuccess = false;

                string path = AppDomain.CurrentDomain.BaseDirectory + "Temp\\" + FileFolder;
                if (Directory.Exists(path))
                {
                    //获取该路径下的文件路径
                    string[] filePathList = Directory.GetFiles(path);
                    foreach (string filePath in filePathList)
                    {
                        File.Delete(filePath);
                    }
                    //如果存在则删除
                    Directory.Delete(AppDomain.CurrentDomain.BaseDirectory + "Temp\\" + FileFolder);
                    BootLaoderFileNameWriteZip = "";
                }

            }

            if (frameCode != 0)
            {
                BarBindingVal = IDWrite * 100 / frameCode;
            }

        }
        //设置BootLoader条数
        private void ParseMessage_BootLoaderNumberSet_Coslight(byte[] message)
        {
            int packAddress = message[2] - 1;
            int offset = 7;

            if (message[offset + 1] == 0x01)
            {
                SerialPortCommunicator.BLStatus = BootLaoderStatus.更新中;

                isWriteSuccess = false;//20200611

                for (int i = 0; i < 512 * 2; i++)
                {
                    byteData[i] = byteDatalong[IDWrite, i];
                }

                IDWrite++;

                BootLoaderUpdates(IDWrite, byteData);
            }
            else
            {
                IsReadParameter = false;


                if (IsChineseUI)
                {
                    StateContectInformation = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：写BootLoader条数失败！";
                }

                else
                {
                    StateContectInformation = DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy") + ":Write BootLoader Failed!";
                }
            }
        }
        //BootLoader擦除
        private void ParseMessage_BootLoaderErases_Coslight(byte[] message)
        {
            int packAddress = message[2] - 1;
            int offset = 7;

            ProjectCodeRead = "";
            COSPowerCodeRead = "";
            DevelopmentPhaseRead = "";
            VersionNumber1Read = "";
            VersionNumber2Read = "";

            VersionNumber3Read = "";
            OtherRead = "";

            ProjectCodeWrite = "";
            COSPowerCodeWrite = "";
            DevelopmentPhaseWrite = "";
            VersionNumber1Write = "";
            VersionNumber2Write = "";
            BootLaoderFileNameWrite = "";

            VersionNumber3Write = "";
            OtherWrite = "";
            frameCode = 0;

            if (message[offset] == 0x00)
            {
                if (message[offset + 1] == 0x01)
                {
                    SerialPortCommunicator.BLStatus = BootLaoderStatus.应用程序;

                    if (IsChineseUI)
                    {
                        StateContectInformation = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：擦除成功！";
                    }

                    else
                    {
                        StateContectInformation = DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy") + ":Erases Success!";
                    }

                    isErases = false;
                }
                else
                {
                    isErases = false;
                    IsReadParameter = false;

                    if (IsChineseUI)
                    {
                        StateContectInformation = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：擦除失败！";
                    }
                    else
                    {
                        StateContectInformation = DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy") + ":Erases Failed!";
                    }
                }
            }
        }
        //读取陀螺仪数据
        private void ParseMessage_GyroSensorAnolog_Coslight(byte[] message)
        {
            int offset = 7;

            int packAddress = message[2] - 1;

            XRead = (BitConverter.ToInt32(new byte[] { message[offset + 3], message[offset + 2], message[offset + 1], message[offset] }, 0) * 0.001F).ToString();

            offset += 4;

            YRead = (BitConverter.ToInt32(new byte[] { message[offset + 3], message[offset + 2], message[offset + 1], message[offset] }, 0) * 0.001F).ToString();

            offset += 4;

            ZRead = (BitConverter.ToInt32(new byte[] { message[offset + 3], message[offset + 2], message[offset + 1], message[offset] }, 0) * 0.001F).ToString();
        }
        //读取设备信息
        private void ParseMessage_ManufacturerInfo_Coslight(byte[] message)
        {
            try
            {
                int packAddress = message[2] - 1;

                ProjectCode_Coslight = System.Text.Encoding.ASCII.GetString(message, 7, 16).Trim();

                HardwarVersion_Coslight = System.Text.Encoding.ASCII.GetString(message, 23, 16).Trim();

                SoftwareVersion_Coslight = System.Text.Encoding.ASCII.GetString(message, 39, 32).Trim();

                PCBNumber_Coslight = System.Text.Encoding.ASCII.GetString(message, 71, 32).Trim();
            }
            catch
            {

            }
        }
        //读取防盗状态
        private void ParseMessage_AntiTheftAnolog_Coslight(byte[] message)
        {
            int offset = 7;
            int packAddress = message[2] - 1;
            if (IsChineseUI)
            {
                PostEmergencyState = message[offset] == 0x01 ? "已布防" : "未布防"; offset++;

                AntiTheftCommunicationWaring = message[offset] == 0x01 ? "告警" : "正常"; offset++;
                AntiTheftGyroSensorWaring = message[offset] == 0x01 ? "告警" : "正常"; offset++;
                AntiTheftSecurityLineWaring = message[offset] == 0x01 ? "告警" : "正常"; offset++;
                AntiTheftGPSWaring = message[offset] == 0x01 ? "告警" : "正常"; offset++;

                try
                {
                    AntiTheftCommunicationState = message[offset] == 0x01 ? "在线" : "不在线"; offset++;
                    AntiTheftGyroSensorState = message[offset] == 0x01 ? "在线" : "不在线"; offset++;
                    AntiTheftSecurityLineState = message[offset] == 0x01 ? "在线" : "不在线"; offset++;
                    AntiTheftGPSState = message[offset] == 0x01 ? "在线" : "不在线"; offset++;

                    SerialPortCommunicator.IsAntiTheftGPSState = AntiTheftGPSState == "在线" ? true : false;


                }
                catch { }
            }
            else
            {
                PostEmergencyState = message[offset] == 0x01 ? "Have" : "No"; offset++;

                AntiTheftCommunicationWaring = message[offset] == 0x01 ? "告警" : "正常"; offset++;
                AntiTheftGyroSensorWaring = message[offset] == 0x01 ? "告警" : "正常"; offset++;
                AntiTheftSecurityLineWaring = message[offset] == 0x01 ? "告警" : "正常"; offset++;
                AntiTheftGPSWaring = message[offset] == 0x01 ? "告警" : "正常"; offset++;

                try
                {
                    AntiTheftCommunicationState = message[offset] == 0x01 ? "Online" : "Offline"; offset++;
                    AntiTheftGyroSensorState = message[offset] == 0x01 ? "Online" : "Offline"; offset++;
                    AntiTheftSecurityLineState = message[offset] == 0x01 ? "Online" : "Offline"; offset++;
                    AntiTheftGPSState = message[offset] == 0x01 ? "Online" : "Offline"; offset++;

                    SerialPortCommunicator.IsAntiTheftGPSState = AntiTheftGPSState == "Online" ? true : false;
                }
                catch { }


            }

            if (SerialPortCommunicator.IsAntiTheftGPSState)
            {
                WlanGridState = "Visible";
                WlanContentState = "Collapsed";
            }
            else
            {
                WlanGridState = "Collapsed";
                WlanContentState = "Visible";
            }

            if (isAntiTheftMasterControl)
            {
                isAntiTheftCommunicationWaringGray = "Collapsed";
                isAntiTheftGyroSensorWaringGray = "Collapsed";
                isAntiTheftSecurityLineWaringGray = "Collapsed";
                isAntiTheftGPSWaringGray = "Collapsed";

                isAntiTheftCommunicationWaringRed = AntiTheftCommunicationWaring == "告警" ? "Visible" : "Collapsed";
                isAntiTheftGyroSensorWaringRed = AntiTheftGyroSensorWaring == "告警" ? "Visible" : "Collapsed";
                isAntiTheftSecurityLineWaringRed = AntiTheftSecurityLineWaring == "告警" ? "Visible" : "Collapsed";
                isAntiTheftGPSWaringRed = AntiTheftGPSWaring == "告警" ? "Visible" : "Collapsed";

                isAntiTheftCommunicationWaringGreen = AntiTheftCommunicationWaring == "告警" ? "Collapsed" : "Visible";
                isAntiTheftGyroSensorWaringGreen = AntiTheftGyroSensorWaring == "告警" ? "Collapsed" : "Visible";
                isAntiTheftSecurityLineWaringGreen = AntiTheftSecurityLineWaring == "告警" ? "Collapsed" : "Visible";
                isAntiTheftGPSWaringGreen = AntiTheftGPSWaring == "告警" ? "Collapsed" : "Visible";
            }
            else
            {
                isAntiTheftCommunicationWaringGray = "Visible";
                isAntiTheftGyroSensorWaringGray = "Visible";
                isAntiTheftSecurityLineWaringGray = "Visible";
                isAntiTheftGPSWaringGray = "Visible";

                isAntiTheftCommunicationWaringRed = "Collapsed";
                isAntiTheftGyroSensorWaringRed = "Collapsed";
                isAntiTheftSecurityLineWaringRed = "Collapsed";
                isAntiTheftGPSWaringRed = "Collapsed";

                isAntiTheftCommunicationWaringGreen = "Collapsed";
                isAntiTheftGyroSensorWaringGreen = "Collapsed";
                isAntiTheftSecurityLineWaringGreen = "Collapsed";
                isAntiTheftGPSWaringGreen = "Collapsed";
            }

            IsReadParameter = false;
        }
        //读取防盗参数
        private void ParseMessage_AntiTheftParameter_Coslight(byte[] message)
        {
            int offset = 7;
            int packAddress = message[2] - 1;

            AntiTheftMasterState = message[offset] == 0x01 ? "开启" : "关闭"; offset++;

            AntiTheftCommunicationControl = message[offset] == 0x01 ? "有" : "无"; offset++;
            AntiTheftGyroSensorControl = message[offset] == 0x01 ? "有" : "无"; offset++;
            AntiTheftSecurityLineControl = message[offset] == 0x01 ? "有" : "无"; offset++;
            AntiTheftGPSControl = message[offset] == 0x01 ? "有" : "无"; offset++;

            HandControlState = message[offset] == 0x01 ? "手动" : "自动"; offset++;
            OneKeyPostEmergencyState = message[offset] == 0x01 ? "一键布防" : "解除布防"; offset++;

            isAntiTheftMasterControl = AntiTheftMasterState == "开启" ? true : false;
            isAntiTheftCommunicationControl = AntiTheftCommunicationControl == "有" ? true : false;
            isAntiTheftGyroSensorControl = AntiTheftGyroSensorControl == "有" ? true : false;
            isAntiTheftSecurityLineControl = AntiTheftSecurityLineControl == "有" ? true : false;
            isAntiTheftGPSControl = AntiTheftGPSControl == "有" ? true : false;

            isHandControlState = HandControlState == "手动" ? true : false;
            isOneKeyPostEmergencyState = OneKeyPostEmergencyState == "一键布防" ? true : false;

            AntiTheftCommunicationDelayTime = BitConverter.ToUInt16(new byte[] { message[offset + 1], message[offset] }, 0); offset += 2;
            AntiTheftGPSMovingDistance = BitConverter.ToUInt16(new byte[] { message[offset + 1], message[offset] }, 0); offset += 2;
            AntiTheftGyroSensorAngleInclination = BitConverter.ToUInt16(new byte[] { message[offset + 1], message[offset] }, 0); offset += 2;
            AntiTheftSecurityLineDelayTime = BitConverter.ToUInt16(new byte[] { message[offset + 1], message[offset] }, 0); offset += 2;

            int intPackIniState = BitConverter.ToUInt16(new byte[] { message[offset + 1], message[offset] }, 0); offset += 2;//20200615
            if (IsChineseUI)
            {
                string[] state = new string[] { "未设置", "正向横放", "反向横放", "正向侧放", "反向侧放", "反向竖放", "正向竖放", "", "", "", "失败" };
                try
                {
                    PackIniState = state[intPackIniState];
                }
                catch { }
            }
            else
            {
                string[] state = new string[] { "NoInit", "X_Positive", "X_Negative", "Y_Positive", "Y_Negative", "Z_Negative", "Z_Positive", "", "", "", "Fail" };

                try
                {
                    PackIniState = state[intPackIniState];
                }
                catch { }
            }

            if (lastAntiTheftMasterState == null || lastAntiTheftMasterState != AntiTheftMasterState)
            {
                lastAntiTheftMasterState = AntiTheftMasterState;
                intSetParameterCount = 0;
            }

            if (lastAntiTheftCommunicationState == null || lastAntiTheftCommunicationState != AntiTheftCommunicationControl)
            {
                lastAntiTheftCommunicationState = AntiTheftCommunicationControl;
                intSetParameterCount = 0;
            }

            if (lastAntiTheftGyroSensorState == null || lastAntiTheftGyroSensorState != AntiTheftGyroSensorControl)
            {
                lastAntiTheftGyroSensorState = AntiTheftGyroSensorControl;
                intSetParameterCount = 0;
            }
            if (lastAntiTheftSecurityLineState == null || lastAntiTheftSecurityLineState != AntiTheftSecurityLineControl)
            {
                lastAntiTheftSecurityLineState = AntiTheftSecurityLineControl;
                intSetParameterCount = 0;
            }
            if (lastAntiTheftGPSState == null || lastAntiTheftGPSState != AntiTheftGPSControl)
            {
                lastAntiTheftGPSState = AntiTheftGPSControl;
                intSetParameterCount = 0;
            }
            if (lastHandControlState == null || lastHandControlState != HandControlState)
            {
                lastHandControlState = HandControlState;
                intSetParameterCount = 0;
            }
            if (lastOneKeyPostEmergencyState == null || lastOneKeyPostEmergencyState != OneKeyPostEmergencyState)
            {
                lastOneKeyPostEmergencyState = OneKeyPostEmergencyState;
                intSetParameterCount = 0;
            }

            if (intSetParameterCount >= 4)
            {
                intSetParameterCount = 0;
            }

            IsReadParameter = false;

            IsReadAgainstTheftParameter = true;
        }
        //设备事件记录
        private void ParseMessage_DeviceEventRecord(byte[] message)
        {
            SerialPortCommunicator.ReadingDeviceEventDataCounter = 0;
            DeviceEventRecord record = new DeviceEventRecord();
            record.DeviceEventRecordID = DeviceEventRecords.Count + 1;
            record.SaveTime = new DateTime(BitConverter.ToUInt16(new byte[] { message[9], message[8] }, 0), message[10], message[11], message[12], message[13], message[14]);

            if (message[15] < 0x10)
            {
                record.EventType = IsChineseUI ? "参数改变" : "Parameter Change";
                switch (message[15])
                {
                    case 0x00:
                        record.EventState = IsChineseUI ? "参数配置" : "Parameter configuration";
                        break;
                    case 0x01:
                        record.EventState = IsChineseUI ? "使能配置" : "Enable configuration";
                        break;
                    case 0x02:
                        record.EventState = IsChineseUI ? "干结点配置" : "Dry Node configuration";
                        break;
                    case 0x03:
                        record.EventState = IsChineseUI ? "更新Boot" : "Update Boot";
                        break;
                }

                record.CID2 = record.SaveLastSendCommandInfo(message);

                record.CommandType = record.ReturnCommandType(message);

                record.CommandValue = BitConverter.ToUInt32(new byte[] { message[23], message[22], message[21], message[20] }, 0).ToString();

            }
            if (message[15] >= 0x10)
            {
                record.EventType = IsChineseUI ? "状态改变" : "State Change";
                switch (message[15])
                {
                    case 0x10:
                        record.EventState = IsChineseUI ? "待机" : "Standby";
                        break;
                    case 0x11:
                        record.EventState = IsChineseUI ? "充电" : "Charging";
                        break;
                    case 0x12:
                        record.EventState = IsChineseUI ? "放电" : "Discharge";
                        break;
                    case 0x13:
                        record.EventState = IsChineseUI ? "保护" : "Protection";
                        break;
                    case 0x14:
                        record.EventState = IsChineseUI ? "故障" : "Failure";
                        break;
                    case 0x15:
                        record.EventState = IsChineseUI ? "开机" : "";
                        break;
                    case 0x16:
                        record.EventState = IsChineseUI ? "关机" : "";
                        break;
                }

                record.CID2 = "";

                record.CommandType = "";

                record.CommandValue = "";
            }

            DeviceEventRecords.Add(record);
            if (SerialPortCommunicator.IsReadingDeviceEventData == 0)
            {
                return;
            }

            if (message[7] == 0x00)
            {
                communicator.UserAction(ProtocalProvider.GetMessage_ReadDeviceEventRecord(message[2], 0x01));
            }
            else if (message[7] == 0x01)
            {
                communicator.UserAction(ProtocalProvider.GetMessage_ReadDeviceEventRecord(message[2], 0x03));
                SerialPortCommunicator.IsReadingDeviceEventData = 0;
            }
        }
            }
        }

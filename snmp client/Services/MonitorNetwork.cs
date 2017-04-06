using System;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using Pocos;
using Service;
using SnmpSharpNet;

namespace snmp_client.Services
{
    public class MonitorNetwork
    {
        private Pdu _pdu;
        private readonly IOIDService _ioidService;
        private readonly ILogService _logService;
        private readonly IWMIService _wmiService;
        private readonly OctetString _community;
        private readonly AgentParameters _paramList;
        private readonly IpAddress _ipAddress;
        private readonly UdpTarget _target;

        private OIDModel _interfaceType;
        private OIDModel _sysUptime;
        private OIDModel _sysName;
        private OIDModel _sysLocation;
        private OIDModel _sysServices;

        private WMIModel _cpu;
        private WMIModel _memory;
        private WMIModel _diskReads;
        private WMIModel _diskWrites;
        private WMIModel _diskTransfers;

        private readonly PerformanceCounter _cpuCounter;
        private readonly PerformanceCounter _memoryCounter;
        private readonly PerformanceCounter _diskReadsCounter;
        private readonly PerformanceCounter _diskWritesCounter;
        private readonly PerformanceCounter _diskTransfersCounter;

        private readonly WebTimerService.GetCurrentTimeSoapClient _webTimer;
        public MonitorNetwork()
        {
            _community = new OctetString(ConfigurationManager.AppSettings["community"]);
            _paramList = new AgentParameters(_community) { Version = SnmpVersion.Ver1 };
            _ipAddress = new IpAddress(ConfigurationManager.AppSettings["IpAddress"]);

            var port = Convert.ToInt32(ConfigurationManager.AppSettings["port"]);
            var timeout = Convert.ToInt32(ConfigurationManager.AppSettings["timeout"]);
            _target = new UdpTarget((IPAddress)_ipAddress, port, timeout, 1);

            _cpuCounter = new PerformanceCounter();
            _memoryCounter = new PerformanceCounter();
            _diskReadsCounter = new PerformanceCounter();
            _diskWritesCounter = new PerformanceCounter();
            _diskTransfersCounter = new PerformanceCounter();

            _ioidService = new OIDService();
            _logService = new LogService();
            _wmiService = new WMIService();
            _webTimer = new WebTimerService.GetCurrentTimeSoapClient();
            PopulatePdu();
            InitialiseCounters();
        }

        private void PopulatePdu()
        {
            var oids = _ioidService.GetAllOiDs();
            _pdu = new Pdu(PduType.Get);
            _interfaceType = oids?.FirstOrDefault(x => Regex.IsMatch(x.Item, "Interface Type", RegexOptions.IgnoreCase));
            _pdu.VbList.Add(_interfaceType?.OID1); //Interface Type

            _sysUptime = oids?.FirstOrDefault(x => Regex.IsMatch(x.Item, "sysUptime", RegexOptions.IgnoreCase));
            _pdu.VbList.Add(_sysUptime?.OID1); //sysUpTime

            _sysName = oids?.FirstOrDefault(x => Regex.IsMatch(x.Item, "sysName", RegexOptions.IgnoreCase));
            _pdu.VbList.Add(_sysName?.OID1); //sysName

            _sysLocation = oids?.FirstOrDefault(x => Regex.IsMatch(x.Item, "sysLocation", RegexOptions.IgnoreCase));
            _pdu.VbList.Add(_sysLocation?.OID1); //sysLocation

            _sysServices = oids?.FirstOrDefault(x => Regex.IsMatch(x.Item, "sysServices", RegexOptions.IgnoreCase));
            _pdu.VbList.Add(_sysServices?.OID1); //sysServices

        }
        private void InitialiseCounters()
        {
            var wmis = _wmiService.GetAllWMIs();
            _cpuCounter.CategoryName = "Processor";
            _cpuCounter.CounterName = "% Processor Time";
            _cpuCounter.InstanceName = "_Total";
            _cpu = wmis.FirstOrDefault(x => Regex.IsMatch(x.Description, "CPU", RegexOptions.IgnoreCase));

            _memoryCounter.CategoryName = "Memory";
            _memoryCounter.CounterName = "Available MBytes";
            _memory = wmis.FirstOrDefault(x => Regex.IsMatch(x.Description, "Memory", RegexOptions.IgnoreCase));

            _diskReadsCounter.CategoryName = "PhysicalDisk";
            _diskReadsCounter.CounterName = "Disk Reads/sec";
            _diskReadsCounter.InstanceName = "_Total";
            _diskReads = wmis.FirstOrDefault(x => Regex.IsMatch(x.Description, "Disk Disk reads / sec", RegexOptions.IgnoreCase));

            _diskWritesCounter.CategoryName = "PhysicalDisk";
            _diskWritesCounter.CounterName = "Disk Writes/sec";
            _diskWritesCounter.InstanceName = "_Total";
            _diskWrites = wmis.FirstOrDefault(x => Regex.IsMatch(x.Description, "Disk writes / sec", RegexOptions.IgnoreCase));

            _diskTransfersCounter.CategoryName = "PhysicalDisk";
            _diskTransfersCounter.CounterName = "Disk Transfers/sec";
            _diskTransfersCounter.InstanceName = "_Total";
            _diskTransfers = wmis.FirstOrDefault(x => Regex.IsMatch(x.Description, "Disk transfers / sec", RegexOptions.IgnoreCase));
        }

        public void WritePackets(bool checkIp)
        {
            var result = (SnmpV1Packet)_target.Request(_pdu, _paramList);
            //var webtime = DateTime.Now.Date;
            var webtime = Convert.ToDateTime(_webTimer.currentTime());
            string ip = string.Empty;
            if (checkIp)
            { ip = GetIP(); }

            if (result?.Pdu.ErrorStatus == 0)
            {
                SaveEachPacket(_interfaceType.OIDKey, result.Pdu.VbList[0].Value.ToString(), webtime, false, ip);
                SaveEachPacket(_sysUptime.OIDKey, result.Pdu.VbList[1].Value.ToString(), webtime, false, ip);
                SaveEachPacket(_sysName.OIDKey, result.Pdu.VbList[2].Value.ToString(), webtime, false, ip);
                SaveEachPacket(_sysLocation.OIDKey, result.Pdu.VbList[3].Value.ToString(), webtime, false, ip);
                SaveEachPacket(_sysServices.OIDKey, result.Pdu.VbList[4].Value.ToString(), webtime, false, ip);

                SaveEachPacket(_cpu.WMIID, _cpuCounter.NextValue().ToString(), webtime, true, ip);
                SaveEachPacket(_memory.WMIID, _memoryCounter.NextValue().ToString(), webtime, true, ip);
                SaveEachPacket(_diskReads.WMIID, _diskReadsCounter.NextValue().ToString(), webtime, true, ip);
                SaveEachPacket(_diskWrites.WMIID, _diskWritesCounter.NextValue().ToString(), webtime, true, ip);
                SaveEachPacket(_diskTransfers.WMIID, _diskTransfersCounter.NextValue().ToString(), webtime, true, ip);
            }

        }
        public void SaveEachPacket(int key, string value, DateTime webTime, bool isWMI, string ip)
        {
            _logService.AddLog(new LogModel
            {
                DateTime = webTime,
                IpAddress = ip,
                OIDKey = !isWMI ? key : (int?)null,
                WMIID = isWMI ? key : (int?)null,
                Value = value
            });
        }
        private string GetIP()
        {
            IIP ip = new IP();
            ip.ReadIPAddress();
            return ip.IpModel.ip;
        }

    }
}

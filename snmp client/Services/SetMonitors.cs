using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Pocos;
using Service;
using SnmpSharpNet;

namespace snmp_client.Services
{

    public class SetMonitors
    {
        private Pdu _pdu;
        private readonly OctetString _community;
        private readonly AgentParameters _paramList;
        private readonly IpAddress _ipAddress;
        private readonly UdpTarget _target;
        private readonly IOIDService _ioidService;

        public SetMonitors()
        {
            _community = new OctetString(ConfigurationManager.AppSettings["community"]);
            _paramList = new AgentParameters(_community) { Version = SnmpVersion.Ver1 };
            _ipAddress = new IpAddress(ConfigurationManager.AppSettings["IpAddress"]);

            var port = Convert.ToInt32(ConfigurationManager.AppSettings["port"]);
            var timeout = Convert.ToInt32(ConfigurationManager.AppSettings["timeout"]);
            _target = new UdpTarget((IPAddress)_ipAddress, port, timeout, 1);
            _ioidService = new OIDService();

        }

        public SnmpV1Packet SetRequest(OIDModel ioOidModel, string newValue)
        {
            _pdu = new Pdu(PduType.Set);
            _pdu.VbList.Add(new Oid(ioOidModel.OID1), new OctetString(newValue));
            var results = (SnmpV1Packet)_target.Request(_pdu, _paramList);
            if (results != null && results.Pdu.ErrorStatus == 0)
            {
                ioOidModel.OID1 = newValue;
                _ioidService.UpdateOid(ioOidModel);
            }

            return results;
        }
    }
}

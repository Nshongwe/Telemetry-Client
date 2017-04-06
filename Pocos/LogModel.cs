using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pocos
{
    public class LogModel
    {
        public int LogID { get; set; }
        public int? OIDKey { get; set; }
        public string Value { get; set; }
        public DateTime DateTime { get; set; }
        public int? WMIID { get; set; }
        public string IpAddress { get; set; }
        public string Item { get; set; }
        public string WMIDescr { get; set; }

        public OIDModel OID { get; set; }
        public WMIModel WMI { get; set; }
    }
}

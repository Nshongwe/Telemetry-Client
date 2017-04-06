using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pocos
{
   public class LogSearchModel
    {
        public string Item { get; set; }
        public string Description { get; set; }
        public string Value { get; set; }
        public System.DateTime DateTime { get; set; }
        public string IPAddress { get; set; }
    }
}

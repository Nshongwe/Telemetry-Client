using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pocos;

namespace Data.Helpers
{
   public class Mapper
    {
        public static OIDModel OIDToModel(OID oid)
        {
            return new OIDModel() {OIDKey = oid.OIDKey, OID1 = oid.OID1, Item = oid.Item};
        }

       public static Log ModelToLogEntity(LogModel logModel)
       {
           return new Log()
           {
               Value = logModel.Value,
               DateTime=logModel.DateTime,
               IPAddress=logModel.IpAddress,
               OIDKey =logModel.OIDKey,
               WMIID =logModel.WMIID
           };
       }

       public static LogModel LogToModel(Log log)
       {
           return new LogModel()
           {
               Value = log.Value,
               DateTime = log.DateTime,
               IpAddress = log.IPAddress,
               Item = log.OID?.Item,
               WMIDescr = log.WMI?.Description
           };
       }

       public static WMIModel WMIToModel(WMI wmi)
       {
           return new WMIModel {Description = wmi.Description, WMIID = wmi.WMIID};
       }

       public static LogSearchModel EntityToModel(uspGetLog_Result log)
       {
           return new LogSearchModel
           {
               DateTime = log.DateTime,
               Description = log.Description,
               IPAddress = log.IPAddress,
               Item = log.Item,
               Value = log.Value,
           };
       }

    }
}

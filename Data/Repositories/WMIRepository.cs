using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Helpers;
using Pocos;

namespace Data.Repositories
{
    public interface IWMIRepository
    {
        List<WMIModel> GetAllWMIs();
    }

    public class WMIRepository : IWMIRepository
    {
        private readonly IBaseRepository _baseRepository;

        public WMIRepository()
        {
            _baseRepository = new BaseRepository { dbContext = new Snmp_Client_DBEntities() };
        }

        public List<WMIModel> GetAllWMIs()
        {
            var wmiModel = new List<WMIModel>();
            var wmis = _baseRepository.GetList<WMI>().ToList();
            wmis.ForEach(delegate (WMI wmi)
            {
                wmiModel.Add(Mapper.WMIToModel(wmi));
            });
            return wmiModel;
        }
    }
}

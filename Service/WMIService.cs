using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Repositories;
using Pocos;

namespace Service
{
    public interface IWMIService
    {
        List<WMIModel> GetAllWMIs();
    }

    public class WMIService : IWMIService
    {
        private readonly IWMIRepository _iwmiRepository;

        public WMIService()
        {
            _iwmiRepository = new WMIRepository();
        }

        public List<WMIModel> GetAllWMIs()
        {
            return _iwmiRepository.GetAllWMIs();
        }
    }
}

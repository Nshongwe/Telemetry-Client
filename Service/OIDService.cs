using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Repositories;
using Pocos;

namespace Service
{
    public interface IOIDService
    {
        List<OIDModel> GetAllOiDs();
        int UpdateOid(OIDModel oidModel);
    }

    public class OIDService : IOIDService
    {
        private readonly IOIDRepository _ioidRepository;

        public OIDService()
        {
            _ioidRepository = new OIDRepository();
        }

        public List<OIDModel> GetAllOiDs()
        {
           return  _ioidRepository.GetAllOiDs();
        }

        public int UpdateOid(OIDModel oidModel)
        {
            return _ioidRepository.UpdateOid(oidModel);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Helpers;
using Pocos;

namespace Data.Repositories
{
    public interface IOIDRepository
    {
        int UpdateOid(OIDModel oidModel);
        List<OIDModel> GetAllOiDs();
    }

    public class OIDRepository : IOIDRepository
    {
        private readonly IBaseRepository _baseRepository;

        public OIDRepository()
        {
            _baseRepository = new BaseRepository { dbContext = new Snmp_Client_DBEntities() };
        }

        public int UpdateOid(OIDModel oidModel)
        {
            var oid = _baseRepository.Get<OID>(x => x.OIDKey.Equals(oidModel.OIDKey));
            oid.OID1 = oidModel.OID1;
            _baseRepository.Update(oid);
            return _baseRepository.SaveChanges();
        }

        public List<OIDModel> GetAllOiDs()
        {
            var oidModels = new List<OIDModel>();
            var oids = _baseRepository.GetList<OID>().ToList();
            oids.ForEach(delegate (OID oid)
            {
                oidModels.Add(Mapper.OIDToModel(oid));
            });
            return oidModels;
        }
    }
}

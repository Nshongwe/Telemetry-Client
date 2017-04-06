using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Helpers;
using Pocos;

namespace Data.Repositories
{
    public interface ILogRepository
    {
        int AddLog(LogModel logModel);
        List<LogModel> GetAllLogs();
        List<LogSearchModel> Search(string search);
    }

    public class LogRepository : ILogRepository
    {
        private readonly IBaseRepository _baseRepository;

        public LogRepository()
        {
            _baseRepository = new BaseRepository { dbContext =  new Snmp_Client_DBEntities() };
        }

        public int AddLog(LogModel logModel)
        {
            var log = Mapper.ModelToLogEntity(logModel);
            _baseRepository.Insert<Log>(log);
            return _baseRepository.SaveChanges();
        }

        public List<LogSearchModel> Search(string search)
        {
            var context = (Snmp_Client_DBEntities) _baseRepository.dbContext;
            var logs = context.uspGetLog(search);
            return logs.Select(lg => Mapper.EntityToModel(lg)).ToList();
        }

        public List<LogModel> GetAllLogs()
        {
            var logModels = new List<LogModel>();
            var logs = _baseRepository.GetList<Log>().ToList();
            logs.ForEach(delegate (Log log)
            {
                logModels.Add(Mapper.LogToModel(log));
            });
            return logModels;
        }

    }
}

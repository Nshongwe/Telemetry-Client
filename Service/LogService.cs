using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Repositories;
using Pocos;

namespace Service
{
    public interface ILogService
    {
        int AddLog(LogModel logModel);
        List<LogModel> GetLogs();
        List<LogSearchModel> Search(string search);
    }

    public class LogService : ILogService
    {
        private readonly ILogRepository _logRepository;

        public LogService()
        {
            _logRepository = new LogRepository();
        }

        public int AddLog(LogModel logModel)
        {
            return _logRepository.AddLog(logModel);
        }

        public List<LogModel> GetLogs()
        {
            return _logRepository.GetAllLogs();
        }

        public List<LogSearchModel> Search(string search)
        {
            return _logRepository.Search(search);
        } 
    }
}

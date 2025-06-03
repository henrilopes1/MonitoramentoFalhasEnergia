using MyApp.Data.Interfaces;
using MyApp.Domain.Models;

namespace MyApp.Services.Services
{
    public class LogService
    {
        private readonly ILogRepository _logRepo;

        public LogService(ILogRepository logRepo)
        {
            _logRepo = logRepo;
        }

        public void GravarLog(NivelLog nivel, string mensagem, string origem)
        {
            var log = new LogEvento(nivel, mensagem, origem);
            _logRepo.Salvar(log);
        }

        public IList<LogEvento> ObterTodos() => _logRepo.ObterTodos();
    }
}

using MyApp.Domain.Models;
using System.Collections.Generic;

namespace MyApp.Data.Interfaces
{
    public interface ILogRepository
    {
        void Salvar(LogEvento log);
        IList<LogEvento> ObterTodos();
    }
}

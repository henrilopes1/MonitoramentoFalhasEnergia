using MyApp.Domain.Models;
using System.Collections.Generic;

namespace MyApp.Data.Interfaces
{
    public interface ITarefaRepository
    {
        void Salvar(TarefaManutencao tarefa);
        IList<TarefaManutencao> ObterTodas();
    }
}

using MyApp.Domain.Models;
using System;
using System.Collections.Generic;

namespace MyApp.Data.Interfaces
{
    public interface IFalhaRepository
    {
        void Salvar(FalhaEnergia falha);
        void Atualizar(FalhaEnergia falha);
        FalhaEnergia ObterPorId(Guid id);
        IList<FalhaEnergia> ObterPorPeriodo(DateTime inicio, DateTime fim);
    }
}

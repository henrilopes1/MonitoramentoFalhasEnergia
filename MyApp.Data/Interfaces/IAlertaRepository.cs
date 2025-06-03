using MyApp.Domain.Models;
using System;
using System.Collections.Generic;

namespace MyApp.Data.Interfaces
{
    public interface IAlertaRepository
    {
        void Salvar(Alerta alerta);
        IList<Alerta> ObterPorPeriodo(DateTime inicio, DateTime fim);
    }
}

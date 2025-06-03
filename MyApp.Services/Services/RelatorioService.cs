using MyApp.Data.Interfaces;
using System;
using System.Linq;
using System.Text;

namespace MyApp.Services.Services
{
    public class RelatorioService
    {
        private readonly IFalhaRepository _falhaRepo;
        private readonly IAlertaRepository _alertaRepo;

        public RelatorioService(IFalhaRepository falhaRepo, IAlertaRepository alertaRepo)
        {
            _falhaRepo = falhaRepo;
            _alertaRepo = alertaRepo;
        }

        public string GerarRelatorioSemanal()
        {
            var hoje = DateTime.Today;
            var inicioSemana = hoje.AddDays(-7);

            var falhas = _falhaRepo.ObterPorPeriodo(inicioSemana, hoje);
            var alertas = _alertaRepo.ObterPorPeriodo(inicioSemana, hoje);

            int qtdFalhas = falhas.Count;
            double duracaoTotal = falhas.Sum(f => f.ObterDuracaoMinutos());
            int qtdAlertas = alertas.Count;

            var sb = new StringBuilder();
            sb.AppendLine("Data;QtdFalhas;DuracaoTotal(min);QtdAlertas");
            sb.AppendLine($"{hoje:yyyy-MM-dd};{qtdFalhas};{duracaoTotal:F2};{qtdAlertas}");

            return sb.ToString();
        }
    }
}

using System;

namespace MyApp.Domain.Models
{
    public class FalhaEnergia
    {
        public Guid Id { get; private set; }
        public DateTime DataInicio { get; private set; }
        public DateTime? DataFim { get; private set; }
        public string LocalDispositivo { get; private set; }
        public string Descricao { get; private set; }

        public FalhaEnergia(DateTime dataInicio, string localDispositivo, string descricao)
        {
            if (dataInicio > DateTime.Now)
                throw new ArgumentException("Data de início não pode ser no futuro.");
            if (string.IsNullOrWhiteSpace(localDispositivo))
                throw new ArgumentException("Local/Dispositivo obrigatório.");

            Id = Guid.NewGuid();
            DataInicio = dataInicio;
            LocalDispositivo = localDispositivo;
            Descricao = descricao;
        }

        public void Encerrar(DateTime dataFim)
        {
            if (dataFim < DataInicio)
                throw new ArgumentException("Data de término não pode ser anterior à data de início.");

            DataFim = dataFim;
        }

        public double ObterDuracaoMinutos()
        {
            if (!DataFim.HasValue) return 0;
            return (DataFim.Value - DataInicio).TotalMinutes;
        }
    }
}

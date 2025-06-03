using System;

namespace MyApp.Domain.Models
{
    public enum StatusTarefa { Aberto, EmProgresso, Concluído }

    public class TarefaManutencao
    {
        public Guid Id { get; private set; }
        public string Descricao { get; private set; }
        public StatusTarefa Status { get; private set; }
        public DateTime DataCriacao { get; private set; }

        public TarefaManutencao(string descricao)
        {
            if (string.IsNullOrWhiteSpace(descricao))
                throw new ArgumentException("Descrição da tarefa não pode ficar vazia.");

            Id = Guid.NewGuid();
            Descricao = descricao;
            Status = StatusTarefa.Aberto;
            DataCriacao = DateTime.Now;
        }

        public void AtualizarStatus(StatusTarefa novoStatus)
        {
            Status = novoStatus;
        }
    }
}

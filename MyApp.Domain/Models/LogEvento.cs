using System;

namespace MyApp.Domain.Models
{
    public enum NivelLog { INFO, WARN, ERROR }

    public class LogEvento
    {
        public DateTime Timestamp { get; private set; }
        public NivelLog Nivel { get; private set; }
        public string Mensagem { get; private set; }
        public string Origem { get; private set; }

        public LogEvento(NivelLog nivel, string mensagem, string origem)
        {
            Timestamp = DateTime.Now;
            Nivel = nivel;
            Mensagem = mensagem;
            Origem = origem;
        }

        public override string ToString() =>
            $"[{Timestamp:yyyy-MM-dd HH:mm:ss}][{Nivel}] ({Origem}): {Mensagem}";
    }
}

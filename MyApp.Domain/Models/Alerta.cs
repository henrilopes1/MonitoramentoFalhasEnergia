using System;

namespace MyApp.Domain.Models
{
    public class Alerta
    {
        public Guid Id { get; private set; }
        public DateTime DataHora { get; private set; }
        public string Mensagem { get; private set; }
        public bool FoiEnviado { get; private set; }

        public Alerta(string mensagem)
        {
            if (string.IsNullOrWhiteSpace(mensagem))
                throw new ArgumentException("Mensagem de alerta não pode ficar vazia.");

            Id = Guid.NewGuid();
            DataHora = DateTime.Now;
            Mensagem = mensagem;
            FoiEnviado = false;
        }

        public void MarcarComoEnviado()
        {
            FoiEnviado = true;
        }
    }
}

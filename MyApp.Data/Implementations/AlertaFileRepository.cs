using MyApp.Data.Interfaces;
using MyApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace MyApp.Data.Implementations
{
    public class AlertaFileRepository : IAlertaRepository
    {
        private readonly string _caminhoArquivo;
        private readonly List<Alerta> _cache = new();

        public AlertaFileRepository(string caminhoArquivo)
        {
            _caminhoArquivo = caminhoArquivo;
            CarregarDoArquivo();
        }

        private void CarregarDoArquivo()
        {
            if (!File.Exists(_caminhoArquivo)) return;
            var linhas = File.ReadAllLines(_caminhoArquivo);
            foreach (var linha in linhas)
            {
                // Formato CSV: Id;DataHora;Mensagem;FoiEnviado
                var partes = linha.Split(';');
                var id = Guid.Parse(partes[0]);
                var dataHora = DateTime.ParseExact(partes[1], "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
                var msg = partes[2];
                var foiEnviado = bool.Parse(partes[3]);

                var alerta = new Alerta(msg);
                // Ajustar DataHora e Id:
                typeof(Alerta).GetProperty("DataHora")!.SetValue(alerta, dataHora);
                typeof(Alerta).GetProperty("Id")!.SetValue(alerta, id);
                if (foiEnviado)
                    alerta.MarcarComoEnviado();

                _cache.Add(alerta);
            }
        }

        private void SalvarTudoNoArquivo()
        {
            var linhas = _cache.Select(a =>
                string.Join(";", new[]
                {
                    a.Id.ToString(),
                    a.DataHora.ToString("yyyy-MM-dd HH:mm"),
                    a.Mensagem,
                    a.FoiEnviado.ToString()
                }));
            File.WriteAllLines(_caminhoArquivo, linhas);
        }

        public void Salvar(Alerta alerta)
        {
            _cache.Add(alerta);
            SalvarTudoNoArquivo();
        }

        public IList<Alerta> ObterPorPeriodo(DateTime inicio, DateTime fim)
        {
            return _cache
                .Where(a => a.DataHora >= inicio && a.DataHora <= fim)
                .ToList();
        }
    }
}

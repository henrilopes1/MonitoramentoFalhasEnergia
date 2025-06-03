using MyApp.Data.Interfaces;
using MyApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace MyApp.Data.Implementations
{
    public class TarefaFileRepository : ITarefaRepository
    {
        private readonly string _caminhoArquivo;
        private readonly List<TarefaManutencao> _cache = new();

        public TarefaFileRepository(string caminhoArquivo)
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
                // Formato CSV: Id;Descricao;Status;DataCriacao
                var partes = linha.Split(';');
                var id = Guid.Parse(partes[0]);
                var desc = partes[1];
                var status = Enum.Parse<StatusTarefa>(partes[2]);
                var dataCriacao = DateTime.ParseExact(partes[3], "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);

                var t = new TarefaManutencao(desc);
                // Ajustar campos que o construtor inicializou por padrão:
                typeof(TarefaManutencao).GetProperty("Id")!.SetValue(t, id);
                typeof(TarefaManutencao).GetProperty("DataCriacao")!.SetValue(t, dataCriacao);
                t.AtualizarStatus(status);

                _cache.Add(t);
            }
        }

        private void SalvarTudoNoArquivo()
        {
            var linhas = _cache.Select(t =>
                string.Join(";", new[]
                {
                    t.Id.ToString(),
                    t.Descricao,
                    t.Status.ToString(),
                    t.DataCriacao.ToString("yyyy-MM-dd HH:mm")
                }));
            File.WriteAllLines(_caminhoArquivo, linhas);
        }

        public void Salvar(TarefaManutencao tarefa)
        {
            _cache.Add(tarefa);
            SalvarTudoNoArquivo();
        }

        public IList<TarefaManutencao> ObterTodas()
        {
            return _cache;
        }
    }
}

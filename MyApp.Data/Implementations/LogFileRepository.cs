using MyApp.Data.Interfaces;
using MyApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace MyApp.Data.Implementations
{
    public class LogFileRepository : ILogRepository
    {
        private readonly string _caminhoArquivo;
        private readonly List<LogEvento> _cache = new();

        public LogFileRepository(string caminhoArquivo)
        {
            _caminhoArquivo = caminhoArquivo;
            // Não vamos reler logs antigos, a menos que queiramos reconstruir o cache
            // do arquivo. Para simplificar, só lidamos com o que for registrado em runtime.
        }

        private void AppendNoArquivo(string conteudo)
        {
            File.AppendAllText(_caminhoArquivo, conteudo + Environment.NewLine);
        }

        public void Salvar(LogEvento log)
        {
            _cache.Add(log);
            AppendNoArquivo(log.ToString());
        }

        public IList<LogEvento> ObterTodos()
        {
            return _cache;
        }
    }
}

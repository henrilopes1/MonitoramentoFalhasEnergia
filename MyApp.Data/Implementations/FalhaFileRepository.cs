using MyApp.Data.Interfaces;
using MyApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace MyApp.Data.Implementations
{
    public class FalhaFileRepository : IFalhaRepository
    {
        private readonly string _caminhoArquivo;
        private readonly List<FalhaEnergia> _cache = new();

        public FalhaFileRepository(string caminhoArquivo)
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
                // Formato CSV: Id;DataInicio;DataFim;LocalDispositivo;Descricao
                var partes = linha.Split(';');
                var id = Guid.Parse(partes[0]);
                var dtInicio = DateTime.ParseExact(partes[1], "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
                var strDataFim = partes[2];
                DateTime? dataFim = null;
                if (!string.IsNullOrWhiteSpace(strDataFim))
                    dataFim = DateTime.ParseExact(strDataFim, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);

                var local = partes[3];
                var desc = partes[4];

                var falha = new FalhaEnergia(dtInicio, local, desc);
                if (dataFim.HasValue)
                    falha.Encerrar(dataFim.Value);

                // Ajustar o Id que o construtor criou:
                typeof(FalhaEnergia).GetProperty("Id")!.SetValue(falha, id);

                _cache.Add(falha);
            }
        }

        private void SalvarTudoNoArquivo()
        {
            var linhas = _cache.Select(f =>
                string.Join(";", new[]
                {
                    f.Id.ToString(),
                    f.DataInicio.ToString("yyyy-MM-dd HH:mm"),
                    f.DataFim.HasValue ? f.DataFim.Value.ToString("yyyy-MM-dd HH:mm") : "",
                    f.LocalDispositivo,
                    f.Descricao
                }));
            File.WriteAllLines(_caminhoArquivo, linhas);
        }

        public void Salvar(FalhaEnergia falha)
        {
            _cache.Add(falha);
            SalvarTudoNoArquivo();
        }

        public void Atualizar(FalhaEnergia falha)
        {
            SalvarTudoNoArquivo();
        }

        public FalhaEnergia ObterPorId(Guid id)
        {
            var f = _cache.FirstOrDefault(x => x.Id == id);
            if (f == null)
                throw new KeyNotFoundException("Falha não encontrada.");
            return f;
        }

        public IList<FalhaEnergia> ObterPorPeriodo(DateTime inicio, DateTime fim)
        {
            return _cache
                .Where(f => f.DataInicio >= inicio && f.DataInicio <= fim)
                .ToList();
        }
    }
}

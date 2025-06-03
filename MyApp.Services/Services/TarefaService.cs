using MyApp.Data.Interfaces;
using MyApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyApp.Services.Services
{
    public class TarefaService
    {
        private readonly ITarefaRepository _tarefaRepo;
        private readonly LogService _logService;

        public TarefaService(ITarefaRepository tarefaRepo, LogService logService)
        {
            _tarefaRepo = tarefaRepo;
            _logService = logService;
        }

        public TarefaManutencao CriarTarefa(string descricao)
        {
            try
            {
                var tarefa = new TarefaManutencao(descricao);
                _tarefaRepo.Salvar(tarefa);

                _logService.GravarLog(NivelLog.INFO, $"Tarefa criada: {tarefa.Id}", nameof(TarefaService));
                return tarefa;
            }
            catch (ArgumentException ae)
            {
                _logService.GravarLog(NivelLog.WARN, $"Erro ao criar tarefa: {ae.Message}", nameof(TarefaService));
                throw;
            }
            catch (Exception ex)
            {
                _logService.GravarLog(NivelLog.ERROR, $"Erro inesperado em TarefaService: {ex.Message}", nameof(TarefaService));
                throw;
            }
        }

        public IList<TarefaManutencao> ObterTodasTarefas() => _tarefaRepo.ObterTodas();

        public void AtualizarStatusTarefa(Guid idTarefa, StatusTarefa novoStatus)
        {
            var todas = _tarefaRepo.ObterTodas();
            var t = todas.FirstOrDefault(x => x.Id == idTarefa);
            if (t == null)
                throw new KeyNotFoundException("Tarefa não encontrada.");

            t.AtualizarStatus(novoStatus);
            _logService.GravarLog(NivelLog.INFO, $"Tarefa {idTarefa} atualizada para {novoStatus}", nameof(TarefaService));
            // (Salva automaticamente quando o Repositório for File; no caso do In-Memory você teria que sobrescrever.)
        }
    }
}

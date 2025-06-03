using MyApp.Data.Interfaces;
using MyApp.Domain.Models;
using System;
using System.Collections.Generic;

namespace MyApp.Services.Services
{
    public class FalhaService
    {
        private readonly IFalhaRepository _falhaRepo;
        private readonly LogService _logService;

        public FalhaService(IFalhaRepository falhaRepo, LogService logService)
        {
            _falhaRepo = falhaRepo;
            _logService = logService;
        }

        public FalhaEnergia RegistrarFalha(string dataInicioStr, string local, string descricao)
        {
            try
            {
                if (!DateTime.TryParse(dataInicioStr, out var dtInicio))
                    throw new FormatException("Data de início inválida.");

                var falha = new FalhaEnergia(dtInicio, local, descricao);
                _falhaRepo.Salvar(falha);

                _logService.GravarLog(NivelLog.INFO, $"Falha registrada: {falha.Id}", nameof(FalhaService));
                return falha;
            }
            catch (FormatException fe)
            {
                _logService.GravarLog(NivelLog.WARN, $"Erro de formatação: {fe.Message}", nameof(FalhaService));
                throw;
            }
            catch (ArgumentException ae)
            {
                _logService.GravarLog(NivelLog.WARN, $"Falha ao validar dados: {ae.Message}", nameof(FalhaService));
                throw;
            }
            catch (Exception ex)
            {
                _logService.GravarLog(NivelLog.ERROR, $"Erro ao registrar falha: {ex.Message}", nameof(FalhaService));
                throw;
            }
        }

        public void EncerrarFalha(string idFalhaStr, string dataFimStr)
        {
            try
            {
                if (!Guid.TryParse(idFalhaStr, out var idFalha))
                    throw new FormatException("ID de falha inválido.");
                if (!DateTime.TryParse(dataFimStr, out var dtFim))
                    throw new FormatException("Data de término inválida.");

                var falha = _falhaRepo.ObterPorId(idFalha);
                falha.Encerrar(dtFim);
                _falhaRepo.Atualizar(falha);

                _logService.GravarLog(NivelLog.INFO, $"Falha encerrada: {falha.Id}", nameof(FalhaService));
            }
            catch (KeyNotFoundException)
            {
                _logService.GravarLog(NivelLog.ERROR, $"Falha não encontrada: {idFalhaStr}", nameof(FalhaService));
                throw;
            }
            catch (FormatException fe)
            {
                _logService.GravarLog(NivelLog.WARN, $"Erro de conversão: {fe.Message}", nameof(FalhaService));
                throw;
            }
            catch (ArgumentException ae)
            {
                _logService.GravarLog(NivelLog.WARN, $"Erro de validação: {ae.Message}", nameof(FalhaService));
                throw;
            }
            catch (Exception ex)
            {
                _logService.GravarLog(NivelLog.ERROR, $"Erro ao encerrar falha: {ex.Message}", nameof(FalhaService));
                throw;
            }
        }

        public IList<FalhaEnergia> ObterFalhasPorPeriodo(string inicioStr, string fimStr)
        {
            try
            {
                if (!DateTime.TryParse(inicioStr, out var dtInicio))
                    throw new FormatException("Data inicial inválida.");
                if (!DateTime.TryParse(fimStr, out var dtFim))
                    throw new FormatException("Data final inválida.");

                return _falhaRepo.ObterPorPeriodo(dtInicio, dtFim);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

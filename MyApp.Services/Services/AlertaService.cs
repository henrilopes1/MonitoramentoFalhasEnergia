using MyApp.Data.Interfaces;
using MyApp.Domain.Models;
using System;

namespace MyApp.Services.Services
{
    public class AlertaService
    {
        private readonly IAlertaRepository _alertaRepo;
        private readonly LogService _logService;

        public AlertaService(IAlertaRepository alertaRepo, LogService logService)
        {
            _alertaRepo = alertaRepo;
            _logService = logService;
        }

        public Alerta CriarAlerta(string mensagem)
        {
            try
            {
                var alerta = new Alerta(mensagem);
                _alertaRepo.Salvar(alerta);

                _logService.GravarLog(NivelLog.INFO, $"Alerta criado: {alerta.Id}", nameof(AlertaService));

                // Simula envio de e-mail:
                alerta.MarcarComoEnviado();
                _logService.GravarLog(NivelLog.INFO, $"Alerta marcado como enviado: {alerta.Id}", nameof(AlertaService));

                return alerta;
            }
            catch (ArgumentException ae)
            {
                _logService.GravarLog(NivelLog.WARN, $"Erro na criação do alerta: {ae.Message}", nameof(AlertaService));
                throw;
            }
            catch (Exception ex)
            {
                _logService.GravarLog(NivelLog.ERROR, $"Erro ao criar alerta: {ex.Message}", nameof(AlertaService));
                throw;
            }
        }
    }
}

using MyApp.Data.Implementations;
using MyApp.Data.Interfaces;
using MyApp.Domain.Models;
using MyApp.Services.Services;
using MyApp.Security;
using System;

namespace MyApp.UI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 1) Instanciar Repositórios com arquivos em disco
            string pastaDados = "dados";
            if (!System.IO.Directory.Exists(pastaDados))
                System.IO.Directory.CreateDirectory(pastaDados);

            IFalhaRepository falhaRepo = new FalhaFileRepository($"{pastaDados}/falhas.txt");
            IAlertaRepository alertaRepo = new AlertaFileRepository($"{pastaDados}/alertas.txt");
            ILogRepository logRepo = new LogFileRepository($"{pastaDados}/logs.txt");
            ITarefaRepository tarefaRepo = new TarefaFileRepository($"{pastaDados}/tarefas.txt");

            // 2) Instanciar Services (injetando repositórios)
            var logService = new LogService(logRepo);
            var falhaService = new FalhaService(falhaRepo, logService);
            var alertaService = new AlertaService(alertaRepo, logService);
            var relatorioService = new RelatorioService(falhaRepo, alertaRepo);
            var tarefaService = new TarefaService(tarefaRepo, logService);

            // 3) Autenticação simples
            var authenticator = new SimpleAuthenticator();

            Console.WriteLine("=== SISTEMA DE MONITORAMENTO DE FALHAS DE ENERGIA ===");
            try
            {
                Console.Write("Usuário: ");
                var usuario = Console.ReadLine();

                Console.Write("Senha: ");
                var senha = Console.ReadLine();

                if (!authenticator.Autenticar(usuario!, senha!))
                {
                    Console.WriteLine("Credenciais inválidas. Encerrando...");
                    return;
                }

                Console.WriteLine($"Bem‐vindo, {usuario}!");
                MostrarMenu(falhaService, alertaService, relatorioService, tarefaService, logService);
            }
            catch (ArgumentException ae)
            {
                Console.WriteLine($"Erro: {ae.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro inesperado: {ex.Message}");
            }
        }

        static void MostrarMenu(
            FalhaService falhaService,
            AlertaService alertaService,
            RelatorioService relatorioService,
            TarefaService tarefaService,
            LogService logService)
        {
            while (true)
            {
                Console.WriteLine("\n--- MENU PRINCIPAL ---");
                Console.WriteLine("1. Registrar nova falha");
                Console.WriteLine("2. Encerrar falha existente");
                Console.WriteLine("3. Gerar relatório semanal");
                Console.WriteLine("4. Criar tarefa de manutenção");
                Console.WriteLine("5. Ver tarefas pendentes");
                Console.WriteLine("6. Gerar alerta manual (teste)");
                Console.WriteLine("7. Ver logs");
                Console.WriteLine("0. Sair");
                Console.Write("Opção: ");

                var opcao = Console.ReadLine();
                Console.WriteLine();

                switch (opcao)
                {
                    case "1":
                        try
                        {
                            Console.Write("Data de Início (yyyy-MM-dd HH:mm): ");
                            var dataInicio = Console.ReadLine();

                            Console.Write("Local/Dispositivo: ");
                            var local = Console.ReadLine();

                            Console.Write("Descrição: ");
                            var desc = Console.ReadLine();

                            var falha = falhaService.RegistrarFalha(dataInicio!, local!, desc!);
                            Console.WriteLine($"Falha registrada com sucesso! ID = {falha.Id}");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"Erro ao registrar falha: {e.Message}");
                        }
                        break;

                    case "2":
                        try
                        {
                            Console.Write("ID da Falha a Encerrar: ");
                            var id = Console.ReadLine();

                            Console.Write("Data de Término (yyyy-MM-dd HH:mm): ");
                            var dataFim = Console.ReadLine();

                            falhaService.EncerrarFalha(id!, dataFim!);
                            Console.WriteLine("Falha encerrada com sucesso!");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"Erro ao encerrar falha: {e.Message}");
                        }
                        break;

                    case "3":
                        try
                        {
                            var rel = relatorioService.GerarRelatorioSemanal();
                            Console.WriteLine("--- RELATÓRIO SEMANAL ---");
                            Console.WriteLine(rel);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"Erro ao gerar relatório: {e.Message}");
                        }
                        break;

                    case "4":
                        try
                        {
                            Console.Write("Descrição da tarefa: ");
                            var descTarefa = Console.ReadLine();
                            var tarefa = tarefaService.CriarTarefa(descTarefa!);
                            Console.WriteLine($"Tarefa criada com ID: {tarefa.Id}");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"Erro ao criar tarefa: {e.Message}");
                        }
                        break;

                    case "5":
                        Console.WriteLine("--- TAREFAS PENDENTES ---");
                        var todas = tarefaService.ObterTodasTarefas();
                        if (todas.Count == 0)
                        {
                            Console.WriteLine("Nenhuma tarefa cadastrada.");
                        }
                        else
                        {
                            foreach (var t in todas)
                                Console.WriteLine($"[{t.Id}] {t.Descricao} – Status: {t.Status}");
                        }
                        break;

                    case "6":
                        try
                        {
                            Console.Write("Digite a mensagem de alerta para teste: ");
                            var msgAlerta = Console.ReadLine();
                            var alerta = alertaService.CriarAlerta(msgAlerta!);
                            Console.WriteLine($"Alerta gerado com ID: {alerta.Id}");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"Erro ao criar alerta: {e.Message}");
                        }
                        break;

                    case "7":
                        Console.WriteLine("--- LOGS DO SISTEMA ---");
                        var logs = logService.ObterTodos();
                        foreach (var l in logs)
                            Console.WriteLine(l.ToString());
                        break;

                    case "0":
                        Console.WriteLine("Encerrando aplicação. Até mais!");
                        return;

                    default:
                        Console.WriteLine("Opção inválida. Tente novamente.");
                        break;
                }
            }
        }
    }
}


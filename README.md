## Descrição do Projeto

O **Monitoramento de Falhas de Energia** é um sistema simples em C# (Console App, .NET 6.0) que permite:

* Registrar eventos de queda e restauração de energia em dispositivos críticos, com data/hora, local e descrição.
* Encerrar falhas, calculando automaticamente a duração e armazenando as informações.
* Gerar alertas automáticos quando durações de falha excedem um limite configurado, além de permitir alertas manuais de teste.
* Manter logs de auditoria (INFO, WARN, ERROR) para garantir rastreabilidade de segurança.
* Criar e acompanhar tarefas de manutenção associadas a cada falha.
* Gerar relatórios semanais em formato CSV exibindo quantidade de falhas, duração total (em minutos) e número de alertas.

O sistema foi projetado para rodar em **Windows e Linux**, usando apenas arquivos texto para persistência e mantendo uma arquitetura modular que pode ser facilmente estendida (por exemplo, migrar para banco de dados ou adicionar notificações por e-mail).

---

## 🚀 Funcionalidades Principais

1. **Autenticação Simples**

   * Ao iniciar, o usuário fornece **usuário e senha**.
   * Senhas ficam “hard-coded” em memória, mas a estrutura permite usar um repositório externo futuramente.

2. **Registro de Falha de Energia**

   * Usuário informa data/hora de início (formato `yyyy-MM-dd HH:mm`), local/dispositivo e descrição.
   * O sistema gera um GUID único, persiste em `dados/falhas.txt` e registra log de nível INFO.

3. **Encerramento de Falha**

   * Usuário informa o GUID da falha e a data/hora de término.
   * Validações garantem que a data de término não seja anterior à de início.
   * Atualiza o registro no arquivo e gera log INFO.

4. **Geração de Alertas**

   * Se uma falha permanecer aberta por mais de 10 minutos (configurável), ao encerrar a falha é gerado um `Alerta` automático.
   * Usuário também pode criar um alerta manual (opção de teste), inserindo uma mensagem.
   * Alertas são salvos em `dados/alertas.txt` e marcados como enviados.

5. **Logs de Auditoria**

   * Todas as ações críticas (registro, encerramento, geração de alerta, criação/atualização de tarefas, erros de validação) geram um `LogEvento` com nível INFO/WARN/ERROR.
   * Os logs ficam em `dados/logs.txt` e podem ser exibidos via menu “Ver logs”.

6. **Gerar Relatório Semanal**

   * Calcula o período de 7 dias anteriores até hoje.
   * Conta quantidade de falhas no período, soma duração total (em minutos) e conta alertas gerados.
   * Exibe CSV no console (ou salva em arquivo de relatório) com cabeçalho:

     ```
     Data;QtdFalhas;DuracaoTotal(min);QtdAlertas
     2025-06-01;3;125.00;2
     ```

7. **Gerenciamento de Tarefas de Manutenção**

   * Permite criar tarefas para cada falha informando descrição e definindo status inicial como “Aberto”.
   * Exibe todas as tarefas pendentes (status Aberto ou Em Progresso).
   * Opcionalmente permite atualizar status para “Concluído”, gerando log INFO.

8. **Exibir Logs**

   * Exibe no console todas as entradas de `logs.txt` em ordem cronológica.
   * Facilita auditoria e investigação de incidentes.

---

## 🛠️ Tecnologias Utilizadas

* **Linguagem:** C# 10.0
* **.NET SDK:** .NET 6.0 (LTS)
* **IDE/Editor:** Visual Studio Code com extensão C# (OmniSharp)
* **Persistência:** Arquivos texto (`.txt`) na pasta `dados/`

  * `dados/falhas.txt`
  * `dados/alertas.txt`
  * `dados/logs.txt`
  * `dados/tarefas.txt`
* **Coleções & LINQ:**

  * Uso de `List<T>`, `Where()`, `Sum()`, `Count()` para manipulação de dados em memória e geração de relatórios.
* **Tratamento de Exceções:**

  * Blocos `try-catch` em serviços para capturar `FormatException`, `ArgumentException` e outros erros de execução, gravando logs de WARN/ERROR.
* **Autenticação Básica:**

  * Interface `IUserAuthenticator` e implementação `SimpleAuthenticator` que mantém usuários em memória.
* **Controle de Versão:**

  * Git para versionamento, repositório no GitHub.
  * `.gitignore` configurado para ignorar `bin/`, `obj/`, arquivos de configuração do VS Code e credenciais locais.

---

## 📁 Estrutura de Pastas

```
MonitoramentoFalhasEnergia/
├── dados/                           ← Arquivos de persistência
│   ├── falhas.txt
│   ├── alertas.txt
│   ├── logs.txt
│   └── tarefas.txt
│
├── MyApp.Domain/                    ← Modelos e Exceções
│   ├── Models/
│   │   ├── FalhaEnergia.cs
│   │   ├── Alerta.cs
│   │   ├── LogEvento.cs
│   │   └── TarefaManutencao.cs
│   └── Exceptions/
│       └── ValidacaoException.cs
│
├── MyApp.Data/                      ← Repositórios e Interfaces
│   ├── Interfaces/
│   │   ├── IFalhaRepository.cs
│   │   ├── IAlertaRepository.cs
│   │   ├── ILogRepository.cs
│   │   └── ITarefaRepository.cs
│   └── Implementations/
│       ├── FalhaFileRepository.cs
│       ├── AlertaFileRepository.cs
│       ├── LogFileRepository.cs
│       └── TarefaFileRepository.cs
│
├── MyApp.Services/                  ← Serviços e Lógica de Negócio
│   ├── Services/
│   │   ├── FalhaService.cs
│   │   ├── AlertaService.cs
│   │   ├── LogService.cs
│   │   ├── RelatorioService.cs
│   │   └── TarefaService.cs
│   └── Utils/
│       └── ValidacaoUtils.cs
│
├── MyApp.Security/                  ← Autenticação
│   ├── IUserAuthenticator.cs
│   └── SimpleAuthenticator.cs
│
├── MyApp.UI/                        ← Console App (interface com usuário)
│   ├── Program.cs
│   └── MenuPrincipal.cs (opcional)
│
├── MonitoramentoFalhasEnergia.sln   ← Solution .NET
└── README.md                        ← Este arquivo
```

---

## ⚙️ Como Executar o Projeto

1. **Pré-requisitos**

   * .NET 6.0 SDK (ou superior) instalado
   * Visual Studio Code (ou outro editor)

2. **Clonar o repositório**

   ```bash
   git clone https://github.com/SeuUsuario/MonitoramentoFalhasEnergia.git
   cd MonitoramentoFalhasEnergia
   ```

3. **Criar a pasta de dados e arquivos vazios**

   * Dentro da raiz do projeto, crie a pasta `dados` e quatro arquivos vazios:

     ```bash
     mkdir dados
     type nul > dados\falhas.txt       # Windows
     type nul > dados\alertas.txt
     type nul > dados\logs.txt
     type nul > dados\tarefas.txt
     ```

     ou, em Linux/macOS:

     ```bash
     mkdir dados
     touch dados/falhas.txt dados/alertas.txt dados/logs.txt dados/tarefas.txt
     ```

4. **Restaurar pacotes e compilar**

   ```bash
   dotnet restore
   dotnet build
   ```

5. **Executar o Console App**

   ```bash
   dotnet run --project MyApp.UI
   ```

   * O console exibirá:

     ```
     === SISTEMA DE MONITORAMENTO DE FALHAS DE ENERGIA ===
     Usuário:
     ```
   * Informe **usuário** e **senha** (ex.: `admin` / `Senha123`).

6. **Usar o Menu**

   * **1. Registrar nova falha**: informe data/hora (início), local e descrição.
   * **2. Encerrar falha existente**: informe GUID da falha e data/hora (término).
   * **3. Gerar relatório semanal**: exibe CSV com estatísticas.
   * **4. Criar tarefa de manutenção**: informe descrição da tarefa.
   * **5. Ver tarefas pendentes**: lista todas as tarefas abertas.
   * **6. Gerar alerta manual (teste)**: informe mensagem de alerta.
   * **7. Ver logs**: exibe histórico de eventos (falhas, alertas, erros).
   * **0. Sair**: encerra a aplicação.

---

## 📋 Regras de Negócio e Validações

* **Data de Início**

  * Deve ser ≤ data/hora atual. Caso contrário, gera `ArgumentException` e log WARN.
* **Data de Término**

  * Deve ser ≥ data de início. Caso contrário, gera `ArgumentException` e log WARN.
* **Campos Obrigatórios**

  * `LocalDispositivo` (no registro de falha), `Mensagem` (no alerta), `Descricao` (na tarefa) não podem ficar vazios. Violar gera `ArgumentException`.
* **Conversão de Datas**

  * Se usar formato incorreto, gera `FormatException`, exibe msg “Data inválida” e grava log WARN.
* **Geração de Alerta Automático**

  * Se a diferença `DataFim – DataInicio` ultrapassar 10 minutos, o sistema cria alerta automaticamente no encerramento da falha.
* **Persistência em Arquivos**

  * Cada entidade salva em arquivo CSV em `dados/` (uso de ponto‐e-vírgula como separador).
  * Ao iniciar, os repositórios carregam o conteúdo existente em memória.
  * Qualquer linha mal formatada nos arquivos é ignorada, gerando log ERROR.

---

## 📂 Estrutura de Projetos e Dependências

* Cada projeto (`*.csproj`) está referenciado na Solution `MonitoramentoFalhasEnergia.sln`.
* As dependências internas seguem este fluxo de referências:

  ```
  MyApp.Domain ← (modelos e exceções)  
        ↑  
  MyApp.Data ← referência MyApp.Domain  
        ↑  
  MyApp.Services ← referência MyApp.Data, MyApp.Domain  
        ↑  
  MyApp.Security ← referência MyApp.Domain  
        ↑  
  MyApp.UI ← referência MyApp.Services, MyApp.Data, MyApp.Security, MyApp.Domain
  ```
* Não há dependências externas de pacotes NuGet além das bibliotecas padrão do .NET 6.0.

---

## 📈 Possíveis Extensões Futuras

1. **Persistência em Banco de Dados**

   * Implementar classes que usem Entity Framework Core ou Dapper para armazenar em SQL Server, PostgreSQL, etc.
   * Basta criar, por exemplo, `FalhaEfRepository` que implemente `IFalhaRepository` e apontar a URL de conexão em `appsettings.json`.

2. **Envio de Alerta Real por E-mail ou SMS**

   * Em `AlertaService`, substituir a marcação “Enviado” simulado por integração com SMTP (`System.Net.Mail`) ou Twilio.
   * Configurar parâmetros em arquivo de configuração seguro.

3. **API REST ou Painel Web**

   * Criar um Web API (ASP.NET Core) que exponha endpoints para registrar/encerrar falhas e exibir relatórios.
   * Desenvolver uma interface web ou dashboard (React, Angular ou Blazor) para visualização em tempo real.

4. **Integração com Ferramentas de Ticket**

   * Ao criar uma tarefa de manutenção, integrar com Jira, ServiceNow ou outro sistema de chamados via API.

5. **Relatórios Agendados**

   * Configurar um serviço (Windows Service ou cron job) para gerar relatórios automaticamente toda segunda-feira às 9h e enviar por e-mail.

---

## 🤝 Contribuição

1. Faça um fork deste repositório.
2. Crie uma branch (`git checkout -b feature/minha-nova-funcionalidade`).
3. Implemente suas melhorias e faça commits claros.
4. Abra um Pull Request detalhando as mudanças.

---

Obrigado por utilizar o **Monitoramento de Falhas de Energia**! Qualquer dúvida, abra uma issue ou entre em contato.

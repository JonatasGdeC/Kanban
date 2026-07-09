# Kanban

Aplicação Full Stack para gerenciamento de tarefas em formato Kanban, desenvolvida com **ASP.NET Core** e **Blazor WebAssembly**.

O sistema permite organizar quadros (boards), colunas e tarefas, com suporte a subtarefas, autenticação de usuários e recuperação de senha por e-mail.

Desafio proposto pela plataforma [Frontend Mentor](https://www.frontendmentor.io/challenges/kanban-task-management-web-app-wgQLt-HlbB).

![Preview da aplicação Kanban](images/preview.jpg)

## Demonstração

**Aplicação:** [https://kanban-jgc.vercel.app/](https://kanban-jgc.vercel.app/)

> A API está hospedada em uma instância EC2 da AWS.

---

## Funcionalidades

### Autenticação

* Cadastro de usuários
* Login com JWT
* Recuperação de senha por e-mail
* Alteração de perfil
* Alteração de senha

### Gestão de Quadros

* Cadastro de boards
* Cadastro de colunas
* Cadastro de tarefas
* Cadastro de subtarefas
* Reordenação e movimentação de tarefas entre colunas

---

## Arquitetura

O projeto segue os princípios de:

* Clean Architecture
* SOLID
* Domain Driven Design (DDD)
* Repository Pattern

Estrutura atual:

```text
src
├── Backend
│   ├── Kanban.Api
│   ├── Kanban.Application
│   ├── Kanban.Domain
│   └── Kanban.Infrastructure
│
├── Shared
│   ├── Kanban.Communication
│   └── Kanban.Exception
│
└── Web
    ├── Kanban.Adapter
    └── Kanban.App
```

---

## Tecnologias Utilizadas

### Backend

* C#
* .NET 10
* ASP.NET Core
* Entity Framework Core
* JWT Authentication
* PostgreSQL
* MailKit
* Swagger

### Frontend

* Blazor WebAssembly
* HTML
* CSS
* JavaScript

### Infraestrutura

* GitHub Actions
* Docker
* GitHub Container Registry (GHCR)
* AWS EC2
* Vercel

---

## Executando Localmente

> É necessário ter o [PostgreSQL](https://www.postgresql.org/download/) instalado e rodando na máquina para executar a API localmente.

### Clonar o repositório

```bash
git clone https://github.com/JonatasGdeC/Kanban.git
```

```bash
cd Kanban
```

### Restaurar dependências

```bash
dotnet restore Kanban.sln
```

### Configurar o appsettings

Crie o arquivo `src/Backend/Kanban.Api/appsettings.Development.json` com o seguinte conteúdo:

```json
{
  "ConnectionStrings": {
    "connection": "Host=localhost;Database=KanbanDb;Username=postgres;Password=sua_senha"
  },
  "Settings": {
    "Jwt": {
      "SigningKey": "sua_chave_secreta_jwt",
      "ExpiresMinutes": 1000
    },
    "PasswordResetToken": {
      "ExpiresMinutes": 15
    }
  },
  "EmailSettings": {
    "Host": "smtp.gmail.com",
    "Port": 587,
    "Username": "seuemail@gmail.com",
    "Password": "sua_app_password",
    "From": "seuemail@gmail.com"
  }
}
```

> Para o envio de e-mails, é necessário uma **App Password** do Gmail. Para gerar uma, acesse [myaccount.google.com/apppasswords](https://myaccount.google.com/apppasswords) (requer verificação em duas etapas ativa na conta).

---

### Executar API

```bash
dotnet run --project src/Backend/Kanban.Api/Kanban.Api.csproj
```

### Executar Frontend

```bash
dotnet run --project src/Web/Kanban.App/Kanban.App.csproj
```

---

## Testes

O backend possui testes automatizados cobrindo validadores e casos de uso (use cases) das principais entidades do domínio: `Board`, `Column`, `Task` e `SubTask`, além do fluxo de usuário (registro, login e atualização).

### Ferramentas

* xUnit
* Moq
* Bogus
* FluentAssertions

### Estrutura

```text
tests
└── Backend
    ├── CommomTestsUtilies   # Builders e utilitários compartilhados entre os testes
    ├── UseCases.Tests       # Testes dos casos de uso da aplicação
    └── Validators.Tests     # Testes das regras de validação (FluentValidation)
```

### Executar os testes

```bash
dotnet test Kanban.sln
```

---

## Autor

**Jônatas Gonçalves de Carvalho**

* GitHub: [JonatasGdeC](https://github.com/JonatasGdeC)
* LinkedIn: [Jônatas Carvalho](https://www.linkedin.com/in/jonatasgdec)
* Portfólio: [Portfólio Pessoal](https://jonatasgdec-portfolio.vercel.app/)

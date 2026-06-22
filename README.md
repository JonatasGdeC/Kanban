# Kanban

Aplicação Full Stack para gerenciamento de tarefas em formato Kanban, desenvolvida com **ASP.NET Core** e **Blazor WebAssembly**.

O sistema permite organizar quadros (boards), colunas e tarefas, com suporte a subtarefas, autenticação de usuários e recuperação de senha por e-mail.

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

### Executar API

```bash
dotnet run --project src/Backend/Kanban.Api/Kanban.Api.csproj
```

### Executar Frontend

```bash
dotnet run --project src/Web/Kanban.App/Kanban.App.csproj
```

---

## Autor

**Jônatas Gonçalves de Carvalho**

* GitHub: [JonatasGdeC](https://github.com/JonatasGdeC)
* LinkedIn: [Jônatas Carvalho](https://www.linkedin.com/in/jonatasgdec)
* Portfólio: [Portfólio Pessoal](https://jonatasgdec-portfolio.vercel.app/)

<h1 align="center">BankApi</h1>

<p align="center">
  Aplicação bancária Full Stack com <strong>.NET 8</strong> e <strong>Angular 20</strong>.<br/>
  Clean Architecture · JWT · Docker · Testes Unitários
</p>

<p align="center">
  <img src="https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" />
  <img src="https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white" />
  <img src="https://img.shields.io/badge/Angular-20-DD0031?style=for-the-badge&logo=angular&logoColor=white" />
  <img src="https://img.shields.io/badge/SQL_Server-CC2927?style=for-the-badge&logo=microsoft-sql-server&logoColor=white" />
  <img src="https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=docker&logoColor=white" />
  <img src="https://img.shields.io/badge/JWT-000000?style=for-the-badge&logo=json-web-tokens&logoColor=white" />
  <img src="https://img.shields.io/badge/xUnit-5C2D91?style=for-the-badge&logo=.net&logoColor=white" />
</p>

<p align="center">
  <img src="https://img.shields.io/badge/testes-12%20passando-brightgreen?style=flat-square" />
  <img src="https://img.shields.io/badge/arquitetura-clean-blue?style=flat-square" />
  <img src="https://img.shields.io/badge/licença-MIT-green?style=flat-square" />
</p>

---

## Sobre o Projeto

**BankApi** é uma aplicação bancária Full Stack desenvolvida para estudo e portfólio. Cobre o ciclo completo de uma aplicação financeira — do cadastro de usuários e autenticação JWT, passando por gerenciamento de conta e transações, até um frontend Angular responsivo com guards de rota e formulários reativos.

O objetivo foi aplicar padrões utilizados em produção em um contexto realista: Clean Architecture, Repository Pattern, DTO Pattern, um tipo de resultado padronizado (`ApiResponse<T>`) e um frontend com interceptors e rotas com lazy loading.

---

## O Que Este Projeto Demonstra

> Para **recrutadores** e **desenvolvedores** que avaliam este projeto.

| Área | Habilidades |
|------|-------------|
| **Arquitetura** | Clean Architecture (4 camadas), princípios SOLID, Injeção de Dependência |
| **Design de API** | Endpoints RESTful, `ApiResponse<T>` padronizado, FluentValidation, tratamento global de erros |
| **Segurança** | Autenticação JWT Bearer, hash de senha com BCrypt, extração de claims do token |
| **Acesso a Dados** | Entity Framework Core 8, Repository Pattern, migrações Code-First, EF Configurations |
| **Observabilidade** | Serilog com logging estruturado, enriquecimento de contexto, sinks Console + Arquivo |
| **Testes** | xUnit, Moq (mock de dependências), FluentAssertions — 12 testes unitários, 100% passando |
| **Frontend** | Angular 20 com componentes standalone, Angular Material, Reactive Forms, lazy loading |
| **Padrões Frontend** | `HttpInterceptorFn` funcional (injeção JWT), `CanActivateFn` guard, modelo `ApiResponse` |
| **DevOps** | Docker Compose com SQL Server 2022, health check, configuração por variáveis de ambiente |

---

## Stack Tecnológica

### Backend
| Tecnologia | Versão | Função |
|---|---|---|
| .NET / ASP.NET Core | 8.0 | Framework da Web API |
| Entity Framework Core | 8.0 | ORM / migrações Code-First |
| SQL Server | 2022 | Banco de dados relacional |
| JWT Bearer | 8.0 | Autenticação |
| FluentValidation | 11.9 | Validação de entrada |
| Serilog | 8.0 | Logging estruturado |
| BCrypt.Net-Next | 4.0 | Hash de senha |
| Swashbuckle | 6.5 | Swagger / OpenAPI |
| xUnit + Moq + FluentAssertions | — | Testes unitários |

### Frontend
| Tecnologia | Versão | Função |
|---|---|---|
| Angular | 20 | Framework SPA |
| Angular Material | 20 | Biblioteca de componentes UI |
| RxJS | — | Programação reativa |
| Reactive Forms | — | Formulários com validação |

### Infraestrutura
| Tecnologia | Função |
|---|---|
| Docker | Containerização |
| Docker Compose | Orquestração local |

---

## Arquitetura

O backend segue **Clean Architecture** — as dependências sempre apontam para dentro. O Domain não tem dependências externas; a camada de API não conhece BCrypt nem SQL.

```
┌──────────────────────────────────────────────────────────┐
│                       BankApi.Api                        │
│          Controllers · Middlewares · Extensions          │
├──────────────────────────────────────────────────────────┤
│                   BankApi.Application                    │
│            Services · DTOs · Validators · Interfaces     │
├──────────────────────────────────────────────────────────┤
│                  BankApi.Infrastructure                  │
│       Repositories · EF Core · JWT · BCrypt · Settings  │
├──────────────────────────────────────────────────────────┤
│                     BankApi.Domain                       │
│              Entities · Interfaces · Enums               │
│             (sem dependências externas)                  │
└──────────────────────────────────────────────────────────┘
```

**Fluxo de uma requisição:**

```
Requisição HTTP
    → Controller
    → FluentValidator
    → Service (regras de negócio)
    → Repository
    → Banco de dados
         ↓
    ← Entity → DTO → ApiResponse<T>
         ↓
Resposta HTTP
```

**Padrões aplicados:**

- **Repository Pattern** — interfaces no Domain, implementações na Infrastructure
- **Service Layer** — regras de negócio nos Services; Controllers apenas orquestram
- **DTO Pattern** — entidades de domínio nunca são expostas na API; DTOs dedicados por operação
- **Result Pattern** — `ApiResponse<T>` padroniza toda resposta de sucesso e erro
- **Interceptor Funcional** — `HttpInterceptorFn` do Angular para injeção automática do JWT
- **Guard Funcional** — `CanActivateFn` do Angular para rotas protegidas

---

## Estrutura de Pastas

```
BankAPI/
├── src/
│   ├── BankApi.Domain/              ← Entidades, Interfaces, Enums
│   │   ├── Entities/
│   │   │   ├── User.cs
│   │   │   ├── Account.cs           ← Lógica de Deposit/Withdraw encapsulada aqui
│   │   │   └── Transaction.cs
│   │   ├── Interfaces/              ← IUserRepository, IAccountRepository, ITransactionRepository
│   │   └── Enums/
│   │       └── TransactionType.cs
│   │
│   ├── BankApi.Application/         ← Lógica de negócio, DTOs, Validators
│   │   ├── Common/
│   │   │   └── ApiResponse.cs       ← Tipo de resultado padronizado da API
│   │   ├── DTOs/
│   │   │   ├── Auth/                ← RegisterRequest/Response, LoginRequest/Response
│   │   │   ├── Account/             ← AccountResponseDto
│   │   │   └── Transaction/         ← TransactionRequest/Response, StatementItemDto
│   │   ├── Services/
│   │   │   ├── AuthService.cs
│   │   │   ├── AccountService.cs
│   │   │   └── TransactionService.cs
│   │   └── Validators/              ← RegisterRequestValidator, LoginRequestValidator, TransactionRequestValidator
│   │
│   ├── BankApi.Infrastructure/      ← EF Core, Repositories, JWT, BCrypt
│   │   ├── Persistence/
│   │   │   ├── AppDbContext.cs
│   │   │   └── Configurations/      ← IEntityTypeConfiguration por entidade
│   │   ├── Repositories/            ← Implementações concretas
│   │   ├── Security/
│   │   │   ├── JwtService.cs
│   │   │   └── PasswordHasher.cs
│   │   └── Migrations/
│   │
│   ├── BankApi.Api/                 ← Ponto de entrada
│   │   ├── Controllers/             ← AuthController, AccountController, TransactionController
│   │   ├── Middlewares/
│   │   │   └── ExceptionMiddleware.cs
│   │   ├── Extensions/
│   │   │   └── ServiceCollectionExtensions.cs
│   │   └── Program.cs
│   │
│   └── BankApi.Tests/               ← Testes unitários xUnit
│       └── Services/
│           ├── AuthServiceTests.cs
│           └── TransactionServiceTests.cs
│
├── bank-app/                        ← Frontend Angular 20
│   └── src/app/
│       ├── core/
│       │   ├── guards/auth.guard.ts
│       │   ├── interceptors/jwt.interceptor.ts
│       │   ├── models/
│       │   └── services/
│       └── pages/
│           ├── login/
│           ├── register/
│           ├── dashboard/
│           ├── deposit/
│           ├── withdraw/
│           └── statement/
│
├── docker-compose.yml
└── README.md
```

---

## Executando o Projeto

### Pré-requisitos

| Ferramenta | Versão mínima |
|---|---|
| .NET SDK | 8.0 |
| Node.js | 20 |
| Angular CLI | qualquer (`npm i -g @angular/cli`) |
| Docker Desktop | qualquer |

### Opção 1 — Docker (recomendado)

Sobe a API e o SQL Server com um único comando:

```bash
docker compose up --build
```

| Serviço | URL |
|---|---|
| API + Swagger | http://localhost:5000 |
| SQL Server | localhost:1433 |

> As migrações são aplicadas automaticamente no startup. Em modo Development, acessar `/` redireciona para o Swagger.

```bash
# Parar
docker compose down

# Parar e remover o volume do banco
docker compose down -v
```

### Opção 2 — Local (sem Docker)

**1. Suba o SQL Server:**

```bash
docker run -d \
  --name bankapi-sqlserver \
  -e "ACCEPT_EULA=Y" \
  -e "SA_PASSWORD=BankApi@2024!" \
  -p 1433:1433 \
  mcr.microsoft.com/mssql/server:2022-latest
```

**2. Execute a API:**

```bash
cd src/BankApi.Api
dotnet run
# Swagger → http://localhost:5057
```

**3. Execute o Frontend:**

```bash
cd bank-app
npm install
ng serve
# App → http://localhost:4200
```

---

## Documentação da API

Todos os endpoints retornam o mesmo envelope padronizado:

```json
{
  "success": true,
  "message": "Operation completed successfully.",
  "data": {},
  "errors": null
}
```

> Endpoints marcados com 🔒 exigem o header `Authorization: Bearer <token>`.

### Autenticação

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| POST | `/api/auth/register` | Cadastra um novo usuário |
| POST | `/api/auth/login` | Autentica e retorna um JWT |

<details>
<summary><strong>POST /api/auth/register</strong></summary>

**Request:**
```json
{
  "name": "Andrey Felipe",
  "email": "andrey@email.com",
  "password": "Secret123"
}
```

**Validações:** `name` ≥ 2 chars · email válido · senha ≥ 6 chars com ao menos 1 maiúscula e 1 dígito

**201 Created:**
```json
{
  "success": true,
  "message": "User registered successfully.",
  "data": {
    "userId": "b38000d3-b1f8-42df-8838-dc8b0c99706e",
    "name": "Andrey Felipe",
    "email": "andrey@email.com"
  },
  "errors": null
}
```

**400 Bad Request** (email duplicado):
```json
{
  "success": false,
  "message": "A user with this email already exists.",
  "data": null,
  "errors": ["A user with this email already exists."]
}
```
</details>

<details>
<summary><strong>POST /api/auth/login</strong></summary>

**Request:**
```json
{
  "email": "andrey@email.com",
  "password": "Secret123"
}
```

**200 OK:**
```json
{
  "success": true,
  "message": "Login successful.",
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "name": "Andrey Felipe",
    "email": "andrey@email.com",
    "expiresAt": "2026-05-22T17:00:00Z"
  },
  "errors": null
}
```

**401 Unauthorized:**
```json
{
  "success": false,
  "message": "Invalid credentials.",
  "data": null,
  "errors": ["Invalid credentials."]
}
```
</details>

### Conta Bancária

| Método | Endpoint | Auth | Descrição |
|--------|----------|------|-----------|
| POST | `/api/account/create` | 🔒 | Cria a conta bancária (uma por usuário) |
| GET | `/api/account/balance` | 🔒 | Retorna saldo atual |

<details>
<summary><strong>POST /api/account/create 🔒</strong></summary>

**201 Created:**
```json
{
  "success": true,
  "message": "Account created successfully.",
  "data": {
    "accountId": "26bae223-2b18-4670-873c-28751d43d340",
    "accountNumber": "33552803",
    "balance": 0.00,
    "createdAt": "2026-05-21T16:59:10Z"
  },
  "errors": null
}
```
</details>

### Transações

| Método | Endpoint | Auth | Descrição |
|--------|----------|------|-----------|
| POST | `/api/transaction/deposit` | 🔒 | Realiza um depósito |
| POST | `/api/transaction/withdraw` | 🔒 | Realiza um saque |
| GET | `/api/transaction/statement` | 🔒 | Extrato completo |

<details>
<summary><strong>POST /api/transaction/deposit 🔒</strong></summary>

**Request:**
```json
{ "amount": 1500.00 }
```

**200 OK:**
```json
{
  "success": true,
  "message": "Deposit completed successfully.",
  "data": {
    "transactionId": "2c0623a9-8700-4eed-9c00-96fab12b7efc",
    "type": "Deposit",
    "amount": 1500.00,
    "balanceAfter": 1500.00,
    "createdAt": "2026-05-21T16:59:11Z"
  },
  "errors": null
}
```
</details>

<details>
<summary><strong>POST /api/transaction/withdraw 🔒</strong></summary>

**Request:**
```json
{ "amount": 300.00 }
```

**200 OK:**
```json
{
  "success": true,
  "message": "Withdrawal completed successfully.",
  "data": {
    "type": "Withdrawal",
    "amount": 300.00,
    "balanceAfter": 1200.00,
    "createdAt": "2026-05-21T16:59:11Z"
  },
  "errors": null
}
```

**400 Bad Request** (saldo insuficiente):
```json
{
  "success": false,
  "message": "Insufficient funds.",
  "data": null,
  "errors": ["Insufficient funds."]
}
```
</details>

<details>
<summary><strong>GET /api/transaction/statement 🔒</strong></summary>

**200 OK:**
```json
{
  "success": true,
  "message": "Operation completed successfully.",
  "data": [
    {
      "date": "2026-05-21T16:59:11Z",
      "type": "Withdrawal",
      "amount": 300.00,
      "balanceAfter": 1200.00
    },
    {
      "date": "2026-05-21T16:59:10Z",
      "type": "Deposit",
      "amount": 1500.00,
      "balanceAfter": 1500.00
    }
  ],
  "errors": null
}
```
</details>

---

## Regras de Negócio

| Regra | Detalhe |
|-------|---------|
| Saldo insuficiente | Saque bloqueado se `amount > balance` |
| Valor positivo | Depósito e saque exigem `amount > 0` |
| Uma conta por usuário | Segundo `POST /account/create` retorna `400` |
| Email único | Cadastro com email já existente retorna `400` |
| Senha forte | Mínimo 6 chars, ao menos 1 maiúscula e 1 dígito |
| Snapshot de saldo | Toda transação armazena `balanceAfter` no momento da operação |
| UserId via token | O `userId` é extraído do JWT no servidor — nunca aceito no body da requisição |

---

## Testes

```bash
dotnet test src/BankApi.Tests/BankApi.Tests.csproj
```

**Resultado: 12 testes — 12 passando**

| Suite | Cenário |
|-------|---------|
| `AuthServiceTests` | Registro com sucesso |
| `AuthServiceTests` | Registro com email duplicado lança exceção |
| `AuthServiceTests` | Login com credenciais válidas |
| `AuthServiceTests` | Login com senha incorreta lança exceção |
| `AuthServiceTests` | Login com usuário inexistente lança exceção |
| `TransactionServiceTests` | Depósito com valor válido |
| `TransactionServiceTests` | Saque com saldo suficiente |
| `TransactionServiceTests` | Saque com saldo insuficiente lança exceção |
| `TransactionServiceTests` | Depósito com valor zero lança exceção |
| `TransactionServiceTests` | Depósito em conta inexistente lança exceção |
| `TransactionServiceTests` | Extrato retorna lista ordenada |

---

## Migrações EF Core

A migration `InitialCreate` já está em `src/BankApi.Infrastructure/Migrations/` e é aplicada automaticamente no startup da API.

```bash
# Criar nova migration
dotnet ef migrations add <NomeDaMigration> \
  --project src/BankApi.Infrastructure/BankApi.Infrastructure.csproj \
  --startup-project src/BankApi.Api/BankApi.Api.csproj

# Aplicar manualmente
dotnet ef database update \
  --project src/BankApi.Infrastructure/BankApi.Infrastructure.csproj \
  --startup-project src/BankApi.Api/BankApi.Api.csproj
```

---

## Decisões Arquiteturais

**Por que Clean Architecture?**
Cada camada tem uma única responsabilidade e pode ser testada, substituída ou evoluída de forma independente. O Domain não conhece EF Core. A Application não conhece SQL Server. A API não conhece BCrypt.

**Por que a lógica de negócio fica na entidade de Domain?**
`account.Deposit(amount)` e `account.Withdraw(amount)` vivem na própria entidade, não no Service. Isso garante que as invariantes de negócio são sempre respeitadas, independente de quem chame o método.

**Por que `ApiResponse<T>` em todos os endpoints?**
Padroniza o contrato da API para o frontend e para integrações futuras. Todo consumidor sabe exatamente o shape da resposta — sucesso ou erro.

**Por que o `userId` vem do token e não do body?**
Segurança: o cliente não pode se passar por outro usuário alterando um campo no body. O servidor extrai o `NameIdentifier` do JWT validado.

**Por que FluentValidation em vez de DataAnnotations?**
Validators são classes separadas, testáveis individualmente, seguindo o Princípio da Responsabilidade Única. DataAnnotations misturam validação com o modelo.

**Por que Serilog?**
Logging estruturado com enriquecimento de contexto (RequestId, tempo de execução) e múltiplos sinks (Console + Arquivo) configurados via `appsettings.json` — sem tocar no código da aplicação.

---

## Licença

Este projeto está sob a licença MIT. Veja o arquivo [LICENSE](LICENSE) para mais detalhes.

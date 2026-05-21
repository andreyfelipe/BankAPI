

## Sobre o Projeto

**BankApi** é uma aplicação bancária Full Stack desenvolvida para estudo e portfólio. Cobre o ciclo completo de uma aplicação financeira do cadastro de usuários e autenticação JWT, passando por gerenciamento de conta e transações, até um frontend Angular responsivo com guards de rota e formulários reativos.

O objetivo foi aplicar padrões utilizados em produção em um contexto realista: Clean Architecture, Repository Pattern.

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


### Infraestrutura
| Tecnologia | Função |
|---|---|
| Docker | Containerização |
| Docker Compose | Orquestração local |

---



**Padrões aplicados:**

- **Repository Pattern** — interfaces no Domain, implementações na Infrastructure

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

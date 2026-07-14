# FCG UsersAPI

Microsserviço responsável por cadastro, autenticação (JWT) e gerenciamento de usuários da plataforma FIAP Cloud Games.

## Responsabilidades

- Registro e login de usuários
- Geração de token JWT
- CRUD de usuários com autorização por roles (Admin/User)
- Publica `UserCreatedEvent` no RabbitMQ após cada registro

## Endpoints

| Método | Rota | Descrição | Auth |
|--------|------|-----------|------|
| POST | `/api/auth/register` | Registrar novo usuário | Não |
| POST | `/api/auth/login` | Login e geração de token | Não |
| GET | `/api/users` | Listar usuários | Admin |
| GET | `/api/users/{id}` | Obter usuário por ID | User/Admin |
| PUT | `/api/users/{id}` | Atualizar dados | User/Admin |
| PATCH | `/api/users/{id}/password` | Alterar senha | User |
| DELETE | `/api/users/{id}` | Desativar usuário | Admin |
| PATCH | `/api/users/{id}/promote` | Promover a Admin | Admin |

## Variáveis de Ambiente

| Variável | Descrição | Exemplo |
|----------|-----------|---------|
| `ConnectionStrings__DefaultConnection` | Connection string PostgreSQL | `Host=postgres-users;Database=fcg_users;...` |
| `JwtSettings__SecretKey` | Chave secreta JWT (mín. 32 chars) | `FCG_SECRET_KEY_...` |
| `JwtSettings__Issuer` | Emissor do token | `FCG` |
| `JwtSettings__Audience` | Audiência do token | `FCG` |
| `JwtSettings__ExpireInMinutes` | Expiração do token | `60` |
| `RabbitMQ_Host` | Host do RabbitMQ | `rabbitmq` |
| `RabbitMQ_Username` | Usuário RabbitMQ | `fcg` |
| `RabbitMQ_Password` | Senha RabbitMQ | `fcg@pass` |

## Executar Localmente (Docker Compose)

```bash
# A partir do repositório fcg-infra:
docker-compose up users-api
```

## Executar Isoladamente

```bash
cd src/UsersAPI
dotnet run
```

## Testes

```bash
dotnet test testes/Users.UnitTests/
```

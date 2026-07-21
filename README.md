# LOCERP

Sistema ERP SaaS para gestao de clientes, equipamentos, locacoes, financeiro e usuarios internos.

## Stack inicial

- ASP.NET Core 10
- Blazor Web App com interatividade Server
- ASP.NET Core Identity
- Entity Framework Core
- PostgreSQL no Supabase via Npgsql
- Layout responsivo para celular e desktop

## Banco de dados

O projeto nao usa SQLite. A aplicacao espera uma connection string PostgreSQL chamada `DefaultConnection`.

Em desenvolvimento, configure com User Secrets:

```powershell
dotnet user-secrets set --project src\Locerp.Web "ConnectionStrings:DefaultConnection" "Host=SEU_HOST_SUPABASE;Port=5432;Database=postgres;Username=SEU_USUARIO;Password=SUA_SENHA;SSL Mode=Require;Trust Server Certificate=true"
```

Para aplicar migrations ao iniciar a aplicacao:

```powershell
dotnet user-secrets set --project src\Locerp.Web "Database:ApplyMigrationsOnStartup" "true"
```

Em hospedagem, use variaveis de ambiente:

```text
ConnectionStrings__DefaultConnection=...
Database__ApplyMigrationsOnStartup=true
```

## Rodar localmente

```powershell
dotnet restore LOCERP.slnx
dotnet run --project src\Locerp.Web
```

## Niveis de usuario

Os papeis iniciais sao criados automaticamente no startup:

- Administrador
- Gerente
- Operador

O sistema nao cria usuario administrador automaticamente. Crie o primeiro usuario e vincule o papel `Administrador` pelo SQL Editor do Supabase quando precisar liberar o acesso inicial.

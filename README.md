# ICT2112-P2 — Developer Onboarding Guide

> **Read this fully before writing a single line of code.**  
> This guide covers everything you need to get your local environment running and understand how the team works together.

---

## Table of Contents

1. [Project Overview](#1-project-overview)
2. [Prerequisites](#2-prerequisites)
3. [First-Time Setup](#3-first-time-setup)
4. [Daily Development Workflow](#4-daily-development-workflow)
5. [Project Structure](#5-project-structure)
6. [Where to Put Your Code](#6-where-to-put-your-code)
7. [Database & Schema Rules](#7-database--schema-rules)
8. [Git & Branching Strategy](#8-git--branching-strategy)
9. [Demo Day Checklist](#9-demo-day-checklist)
10. [FAQ & Troubleshooting](#10-faq--troubleshooting)

---

## 1. Project Overview

This is a **C# ASP.NET Core MVC** application backed by **PostgreSQL 17**.

Multiple teams share the same codebase and database schema. Each team implements one feature module and runs the app independently on their own machine for demo day.

### Architecture

The project follows a **3-layer architecture** with **ECB (Entity-Control-Boundary)** class organisation:

```
[Presentation Layer]   Controllers/ + Views/
        ↓               Boundary — handles HTTP requests and renders pages
[Domain Layer]         Domain/Entities/ + Domain/Control/
        ↓               Entity — data holders | Control — business logic
[Data Source Layer]    Data/UnitOfWork/ + Data/Gateways/
        ↓               Your Data pattern 
                        AppDbContext acts as the Unit of Work (EF Core)
[PostgreSQL 17]        Your local database — the single source of truth
```

**ECB roles:**

| Role | Folder | Responsibility |
|------|--------|----------------|
| Boundary | `Controllers/`, `Views/` | HTTP interface — receive requests, return responses |
| Control | `Domain/Control/` | Business logic — coordinates entities, enforces rules |
| Entity | `Domain/Entities/` | Data holders — auto-generated from DB schema via scaffolding |

> **Rule:** Business logic belongs in `Domain/Control/` — never in Controllers or Views. Controllers call Control classes through service interfaces; they do not contain logic themselves.

> **Database-first:** This project uses **EF Core Reverse Engineering (Scaffolding)**. The database schema in `initial_schema.sql` is always the source of truth. Entity classes in `Domain/Entities/` and `AppDbContext` are **auto-generated** — never edit them by hand.

---

## 2. Prerequisites

Everyone must use the **same versions**.

| Tool | Version | Install |
|------|---------|---------|
| .NET SDK | 9.0 | [https://dotnet.microsoft.com/download](https://dotnet.microsoft.com/download) |
| PostgreSQL | 17 | [https://www.postgresql.org/download/](https://www.postgresql.org/download/) (install everything except Stack Builder) |
| pgAdmin | Latest | [https://www.pgadmin.org/download/](https://www.pgadmin.org/download/) |
| EF Core CLI | Latest | `dotnet tool install --global dotnet-ef` |
| Git | Latest | [https://git-scm.com/downloads](https://git-scm.com/downloads) |
| VS Code | Latest | [https://code.visualstudio.com/](https://code.visualstudio.com/) |

Verify installs:
```bash
dotnet --version   # 9.x.x
psql --version     # 17.x
dotnet ef --version
```

---

## 3. First-Time Setup

Follow these steps once when you first join the project.

### Step 1 — Clone the Team Repo

```bash
git clone <your-team-repo-url>
cd <repo-folder>
```

### Step 2 — Add the Base Repo as Upstream

```bash
git remote add upstream <base-repo-url>
git remote -v   # should show both origin and upstream
```

### Step 3 — Create Your Local Database

In pgAdmin Query Tool or `psql -U postgres`:

```sql
CREATE DATABASE pro_rental;
CREATE USER devuser WITH PASSWORD 'devpassword';
GRANT ALL PRIVILEGES ON DATABASE pro_rental TO devuser;
```

### Step 4 — Configure Your Local Settings

Edit `appsettings.Development.json` with your credentials:

```json
{
  "ConnectionStrings": {
    "Default": "Host=localhost;Port=5432;Database=pro_rental;Username=devuser;Password=devpassword"
  }
}
```

> `appsettings.Development.json` is gitignored and will never be committed. Never put credentials in `appsettings.json`.

### Step 5 — Create the Database Tables

Run `initial_schema.sql` against your empty database:

**pgAdmin:** right-click `pro_rental` → Query Tool → open `initial_schema.sql` → Run

**psql:**
```bash
psql -U postgres -d pro_rental -f initial_schema.sql
```

You should see `CREATE TYPE` and `CREATE TABLE` statements with no errors.

### Step 6 — Scaffold the Entity Classes and DbContext

Run the scaffold command to generate `Domain/Entities/` and `AppDbContext` from the database:

```bash
dotnet ef dbcontext scaffold \
  "Host=localhost;Port=5432;Database=pro_rental;Username=devuser;Password=devpassword" \
  Npgsql.EntityFrameworkCore.PostgreSQL \
  --output-dir Domain/Entities \
  --context-dir Data/UnitOfWork \
  --context AppDbContext \
  --force
```

The `--force` flag overwrites existing generated files. This is expected and correct.

### Step 7 — Restore Packages and Build

```bash
dotnet restore
dotnet build
```

Output ending with `Build succeeded` means your local environment is ready.

---

## 4. Daily Development Workflow

Do this at the start of every work session:

```bash
git pull origin main     # get teammates' latest changes
dotnet restore           # restore packages if needed
dotnet build             # verify the project compiles
```

### If the Schema Has Been Updated

When a teammate announces that `initial_schema.sql` has changed in the base repo:

```bash
# 1. Pull the updated schema
git fetch upstream
git merge upstream/main

# 2. Re-apply the schema to your local database
psql -U postgres -d pro_rental -f initial_schema.sql

# 3. Re-scaffold to regenerate entity classes and DbContext
dotnet ef dbcontext scaffold \
  "Host=localhost;Port=5432;Database=pro_rental;Username=devuser;Password=devpassword" \
  Npgsql.EntityFrameworkCore.PostgreSQL \
  --output-dir Domain/Entities \
  --context-dir Data/UnitOfWork \
  --context AppDbContext \
  --force

# 4. Restore and build
dotnet restore
dotnet build
```

---

## 5. Project Structure

```
ProRental/
│
├── Controllers/                    ← Presentation Layer — Boundary (ECB)
│   ├── Home/
│   │   └── HomeController.cs
│   ├── ApplicationController.cs    ← Base dispatcher (add manually)
│   └── PageController.cs           ← Base class for all feature controllers (add manually)
│
├── Domain/                         ← Domain Layer
│   ├── Entities/                   ← Entity (ECB) — AUTO-GENERATED by scaffolding
│   │   ├── User.cs                 │  namespace: ProRental.Domain.Entities
│   │   ├── Order.cs                │  ⚠️ Do not edit these files manually
│   │   └── ...
│   └── Control/                    ← Control (ECB) — business logic managers
│       └── (your Control classes)  │  namespace: ProRental.Domain.Control
│
├── Data/                           ← Data Source Layer
│   ├── UnitOfWork/
│   │   └── AppDbContext.cs         ← Unit of Work (EF Core) — AUTO-GENERATED by scaffolding
│   │                                  namespace: ProRental.Data.UnitOfWork
│   │                                  ⚠️ Do not edit this file manually
│   ├── Gateways/                   ← Table Data Gateway classes (one per DB table)
│   │   └── (your Gateway classes)  │  namespace: ProRental.Data.Gateways
│   └── Interfaces/                 ← Gateway interfaces (e.g. IOrderGateway)
│       └── (your interfaces)       │  namespace: ProRental.Data.Interfaces
│
├── Interfaces/                     ← Cross-layer service interfaces
│   └── (e.g. IOrderService)        ← Controllers depend on these, not on Control classes directly
│                                      namespace: ProRental.Interfaces
│
├── Views/                          ← Presentation Layer — Boundary (ECB)
│   ├── Home/
│   └── Shared/
│
├── wwwroot/                        ← Static assets (CSS, JS)
├── appsettings.json                ← Placeholder only — never put credentials here
├── appsettings.Development.json    ← GITIGNORED — your local credentials
├── initial_schema.sql              ← Authoritative DB schema — all teams share this
├── DesignTimeDbContextFactory.cs   ← Lets dotnet ef commands work without a running app
└── Program.cs                      ← App entry point and DI registration
```

---

## 6. Where to Put Your Code

| What you're building | Where it goes | Namespace |
|----------------------|---------------|-----------|
| Domain object (data holder) | `Domain/Entities/` — **auto-generated, do not edit** | `ProRental.Domain.Entities` |
| Business logic class | `Domain/Control/` | `ProRental.Domain.Control` |
| Database access class | `Data/Gateways/` | `ProRental.Data.Gateways` |
| Gateway interface | `Data/Interfaces/` | `ProRental.Data.Interfaces` |
| Service interface (cross-layer) | `Interfaces/` | `ProRental.Interfaces` |
| Controller | `Controllers/<Feature>/` | `ProRental.Controllers` |
| Razor view | `Views/<Feature>/` | — |

> **Why two interface folders?**
> - `Data/Interfaces/` — contracts for **gateway/mapper classes** (data access layer). Only `Domain/Control/` classes consume these.
> - `Interfaces/` — contracts for **service/control classes** (domain layer). Only `Controllers/` consume these.
>
> This enforces strict layer direction: `Controller` → `IService` → `Control` → `IGateway` → `Gateway`

**Flow when adding a feature:**
1. Confirm the relevant tables exist in `initial_schema.sql` — entity classes come from the DB, not the other way around
2. Add gateway interface in `Data/Interfaces/` and implementation in `Data/Gateways/`
3. Add service interface in `Interfaces/` and Control class in `Domain/Control/`
4. Add controller in `Controllers/<Feature>/` — inject the service interface, never the Control class directly
5. Add views in `Views/<Feature>/`

---

## 7. Database & Schema Rules

### This Project is Database-First

Entity classes in `Domain/Entities/` and `AppDbContext` are **auto-generated by EF Core Scaffolding**. The database schema defined in `initial_schema.sql` is always the source of truth.

- ✅ Read from and write to existing tables via your Gateway classes
- ✅ Re-scaffold after every schema update (see [Daily Development Workflow](#4-daily-development-workflow))
- ❌ **Never edit files in `Domain/Entities/` or `AppDbContext` by hand** — they will be overwritten on the next scaffold
- ❌ **Never run `dotnet ef migrations add`** — this project does not use code-first migrations
- ❌ Do not modify `initial_schema.sql` without approval from the project lead

### If You Think You Need a Schema Change

1. Raise it with your team lead
2. If approved, the project lead updates `initial_schema.sql` in the base repo
3. All teams pull, re-apply the schema, and re-scaffold:

```bash
psql -U postgres -d pro_rental -f initial_schema.sql

dotnet ef dbcontext scaffold \
  "Host=localhost;Port=5432;Database=pro_rental;Username=devuser;Password=devpassword" \
  Npgsql.EntityFrameworkCore.PostgreSQL \
  --output-dir Domain/Entities \
  --context-dir Data/UnitOfWork \
  --context AppDbContext \
  --force
```

---

## 8. Git & Branching Strategy

### Branch Structure

```
main                            ← Always stable, always demo-ready
├── feature/your-feature-name   ← Your work goes here
├── feature/another-feature     ← Another sub-team's work
└── fix/bug-description         ← Bug fixes
```

### Rules

- ❌ Never commit directly to `main`
- ✅ Always create a feature branch
- ✅ Open a Pull Request when ready; at least one teammate must review before merging
- ✅ Delete your branch after merging
- ✅ Announce in the team channel before merging anything into `main`

### Typical Branch Workflow

```bash
git checkout main
git pull origin main
git checkout -b feature/my-feature

git add .
git commit -m "feat: add order gateway"
git push origin feature/my-feature
# → open a Pull Request
```

### Commit Message Convention

| Prefix | Use for |
|--------|---------|
| `feat:` | New feature |
| `fix:` | Bug fix |
| `refactor:` | Code restructure, no behaviour change |
| `docs:` | README or comment updates |
| `chore:` | Config, setup, dependency changes |

---

## 9. Demo Day Checklist

```
☐ git pull origin main
☐ dotnet restore
☐ dotnet build
☐ initial_schema.sql has been applied to your local database
☐ Scaffold has been re-run if schema was updated
☐ Database has all expected tables (verify in pgAdmin)
☐ appsettings.Development.json is configured with your credentials
☐ Your team's feature works end-to-end
☐ No unhandled exceptions or error pages visible
```

---

## 10. FAQ & Troubleshooting

**Q: I get "relation does not exist" errors.**  
A: Run `initial_schema.sql` against your database first, then re-run the scaffold command.

**Q: My entity classes or `AppDbContext` look outdated or are missing new columns.**  
A: Re-run the scaffold command with `--force`. The schema has likely been updated.

**Q: `dotnet ef` is not recognised.**  
A: Run `dotnet tool install --global dotnet-ef`

**Q: Build fails with "connection refused" or a config error.**  
A: PostgreSQL is not running, or `appsettings.Development.json` credentials are wrong.

**Q: I pulled from main and the project won't build.**  
A: Run `dotnet restore` first — a teammate likely added a new NuGet package. If entity classes look wrong, re-run the scaffold command.

**Q: `appsettings.Development.json` keeps getting overwritten.**  
A: It should be listed in `.gitignore`. If it isn't, add it. Never commit this file.

**Q: I accidentally committed to main.**  
A: Do not push. Run `git reset HEAD~1`, then tell your team lead.

**Q: Two people edited the same file and there's a merge conflict.**  
A: Open the file, resolve the `<<<<<<<` markers manually, then `git add . && git commit`.  
If the conflict is inside `Domain/Entities/` or `AppDbContext.cs`, **do not resolve it manually** — re-run the scaffold command to regenerate them cleanly instead.

**Q: Should I ever run `dotnet ef migrations add`?**  
A: No. This project is database-first. All schema changes go through `initial_schema.sql` → re-scaffold. Never create migrations manually.

**Q: Can I add methods or logic to entity classes in `Domain/Entities/`?**  
A: No. Those files are auto-generated and will be overwritten on the next scaffold. Put all business logic in `Domain/Control/` classes instead.

---

## Workflow for schema change

1. Drop tables in postgres 

2. Execute new initial_schema as a query

3. Verify Change

4. Scaffold command

Run this command any time the schema changes or you need to regenerate entity classes:

```bash
dotnet ef dbcontext scaffold "Host=localhost;Port=5432;Database=pro_rental;Username=postgres;Password=password" Npgsql.EntityFrameworkCore.PostgreSQL --output-dir Domain/Entities --context-dir Data/UnitOfWork --context AppDbContext --force --no-onconfiguring
```

This flag is important because it prevents your connection string from being written into appdbcontext.
```bash
--no-onconfiguring
```

> Replace the connection string credentials with your own if they differ from the defaults above.

*Last updated: March 2026*

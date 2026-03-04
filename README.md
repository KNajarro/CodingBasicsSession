# CodingBasics

A .NET Minimal Web API workshop project using Clean Architecture with AdventureWorks database.

## Project Structure

```
CodingBasics/
├── src/
│   ├── CodingBasics.Api/         # API Layer (Minimal API endpoints)
│   ├── CodingBasics.Application/    # Application Layer (Services)
│   ├── CodingBasics.Domain/         # Domain Layer (Entities, DTOs, Contracts)
│   └── CodingBasics.Infrastructure/   # Infrastructure Layer (EF Core, Repositories)
├── frontend/             # Vue 3 Frontend
└── CodingBasics.sln
```

## Prerequisites

- .NET SDK 8.0+
- SQL Server Express with AdventureWorks2022 database
- Node.js LTS (for the Vue frontend)

## Getting Started

### Backend (.NET API)

```bash
cd src/CodingBasics.Api
dotnet run
# Swagger UI: http://localhost:5000/swagger
```

### Frontend (Vue 3)

```bash
cd frontend
npm install
npm run dev
# App: http://localhost:3000
```

## Workshop Tasks

See the README in each project and the inline TODO comments in each source file.
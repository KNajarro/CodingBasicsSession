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

## Recommended Workshop Order

To get the most out of the workshop, follow this order:

1. **Domain Layer** ([src/CodingBasics.Domain/README.md](src/CodingBasics.Domain/README.md))
   - Define entities, DTOs, and contracts for People and Products.
   - Complete modeling tasks as described in the Domain README and inline TODOs.

2. **Application Layer** ([src/CodingBasics.Application/README.md](src/CodingBasics.Application/README.md))
   - Implement service logic for People and Products.
   - Register services for dependency injection.

3. **Infrastructure Layer** ([src/CodingBasics.Infrastructure/README.md](src/CodingBasics.Infrastructure/README.md))
   - Implement EF Core DbContext and repositories.
   - Register infrastructure services for DI.

4. **API Layer** ([src/CodingBasics.Api/README.md](src/CodingBasics.Api/README.md))
   - Implement Minimal API endpoints for People and Products.
   - Enable Swagger and connect endpoints to services.

5. **Frontend** ([frontend/README.md](frontend/README.md))
   - Implement service methods to call the API.
   - Complete table rendering and search functionality in Vue components.

Refer to the README in each project and the inline TODO comments in each source file for specific coding tasks.

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
   - Define entities, DTOs, and contracts for full CRUD operations on People and Products.
   - Complete modeling tasks for Create, Read, Update, Delete as described in the Domain README and inline TODOs.

2. **Application Layer** ([src/CodingBasics.Application/README.md](src/CodingBasics.Application/README.md))
   - Implement full CRUD service logic for People and Products.
   - Register services for dependency injection.

3. **Infrastructure Layer** ([src/CodingBasics.Infrastructure/README.md](src/CodingBasics.Infrastructure/README.md))
   - Implement full CRUD in EF Core DbContext and repositories.
   - Register infrastructure services for DI.

4. **API Layer** ([src/CodingBasics.Api/README.md](src/CodingBasics.Api/README.md))
   - Implement full CRUD API endpoints for People and Products.
   - Enable Swagger and connect endpoints to services.

5. **Frontend** ([frontend/README.md](frontend/README.md))
   - Implement full CRUD service methods to call the API.
   - Complete table rendering, forms, and CRUD functionality in Vue components.

Refer to the README in each project and the inline TODO comments in each source file for specific coding tasks.

# CRUD Workflow Plan

This project implements a full CRUD (Create, Read, Update, Delete) workflow. Below are the steps and files involved:

## Backend (ASP.NET Core)

- Endpoints for Create, Read, Update, Delete operations
- Data models and DTOs
- Service layer for business logic
- Repository layer for data access

## Frontend (Vue.js)

- UI components for CRUD actions
- API service for backend communication
- Routing for CRUD views

## Steps

1. Review existing CRUD implementation
2. Identify missing CRUD features
3. Plan backend CRUD endpoints
4. Plan frontend CRUD UI components
5. Implement Create endpoint and UI
6. Implement Read endpoint and UI
7. Implement Update endpoint and UI
8. Implement Delete endpoint and UI
9. Test all CRUD operations
10. Document CRUD API and UI usage

Refer to individual README files in each project for more details.

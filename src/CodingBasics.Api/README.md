# CodingBasics.Api

This is the API layer for the AdventureWorks Workshop project.

## Workshop TODOs

- Implement full CRUD (Create, Read, Update, Delete) API endpoints for People and Products.
- See inline TODOs in Program.cs for endpoint implementation tasks (GET, POST, PUT, DELETE).
- Swagger is enabled for API documentation at `/swagger` when running locally.
- Register Infrastructure layer in DI after implementation (see TODO in Program.cs).

## Getting Started

1. Run with:
   ```bash
   dotnet run
   ```
2. Access Swagger UI at [http://localhost:5000/swagger](http://localhost:5000/swagger)

## Key Files

- Program.cs: Main API entry point. Contains TODOs for endpoint implementation.
- appsettings.json: Configuration settings.

---

See inline TODOs for specific coding tasks.

# Backend CRUD Implementation

This backend implements CRUD operations for domain entities using ASP.NET Core Web API.

## Features

- Create: POST endpoints to add new entities
- Read: GET endpoints to retrieve entities
- Update: PUT/PATCH endpoints to modify entities
- Delete: DELETE endpoints to remove entities

## Structure

- Controllers: API endpoints
- Services: Business logic
- Repositories: Data access
- Domain: Entity models and DTOs

## Workflow Steps

See the main project README for the full CRUD workflow plan.

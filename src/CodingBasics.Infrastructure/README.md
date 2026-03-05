# CodingBasics.Infrastructure

This is the Infrastructure layer for the AdventureWorks Workshop project.

## Workshop TODOs

- Implement full CRUD (Create, Read, Update, Delete) in EF Core DbContext and repositories for People and Products.
- Register Infrastructure layer in DI (see TODO in DependencyInjection.cs).
- See inline TODOs in Persistence/AdventureWorksDbContext.cs and Repositories/Repositories.cs for CRUD implementation tasks.

## Getting Started

- This project is referenced by CodingBasics.Api.
- No direct startup required; build as part of the solution.

## Key Files

- Persistence/AdventureWorksDbContext.cs: EF Core DbContext.
- Repositories/Repositories.cs: Repository implementations.
- DependencyInjection.cs: Registers infrastructure services for DI.

## Infrastructure Layer

Implements data persistence and repository logic for CRUD operations.

## Features

- AdventureWorksDbContext: Entity Framework context
- Repositories: CRUD methods for entities

---

See inline TODOs for specific coding tasks.

# Project Context: AdventureWorks Workshop Challenge

## Tech Stack
- Backend: .NET 10 with Entity Framework Core.
- Frontend: Vue 3 (Vite, Vue Router, Axios).
- Database: SQL Server Express — `AdventureWorks2022` (existing database with real data, no seeding needed).
- Architecture: Clean Architecture.

## Directory Structure
- Infrastructure (DbContext): src/CodingBasics.Infrastructure/Persistence/AdventureWorksDbContext.cs
- Domain (Entities): src/CodingBasics.Domain/AdventureWorks/Entities/
- Frontend Application: frontend/

## Current State
- The API already exposes paginated endpoints for People and Products (default pageSize = 20).
- `ProductDto` currently does NOT include `SafetyStockLevel` or `ReorderPoint` — these must be added before the dashboard can use them.
- All existing Vue components use Options API — maintain that style.

## Dashboard Requirements (Part 2: Vue.js)
- Development Path: Work within the frontend/ directory.
- Required Components:
    1. Inventory Value KPI: A card showing the total sum of ListPrice for all products.
    2. Color Distribution: A doughnut chart showing product count per color (use Chart.js).
    3. Customer Analysis: A bar chart showing the number of people grouped by PersonType.
    4. Stock Management: A table highlighting products with low SafetyStockLevel (threshold: 500).

## Coding Standards
- Use modern C# features (.NET 10).
- Maintain strict separation of concerns between layers.
- All code comments and variable names must be in English.
- Do NOT modify Program.cs, DependencyInjection.cs, entity classes, or any interface.
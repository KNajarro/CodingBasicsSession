# Prompt: Challenge — Vue.js Dashboard

## Context

I am working on a Clean Architecture project with:
- **Backend**: .NET 10 Web API with EF Core, connected to `AdventureWorks2022` on SQL Server Express (`localhost:5261`).
- **Frontend**: Vue 3 + Vite + Vue Router + Axios (`frontend/` folder, port 3000, Vite proxies `/api` to the backend).
- The API has paginated endpoints: `GET /api/products` and `GET /api/people`, both return `{ page, pageSize, totalCount, items }` with a default `pageSize` of 20.
- The frontend already has `productsService.js` and `peopleService.js` with `getAll()` and `search()` methods using Axios.
- All existing Vue components use **Options API** — keep that style throughout.

---

## What I Need

Build a **Dashboard page** at route `/dashboard` with a nav link added to the existing navbar.

The dashboard must show:

### 1. KPI Card
- Total sum of `ListPrice` across all products, formatted as USD currency.
- Dark background (`#2c3e50`), white text, centered layout.

### 2. Doughnut Chart — Products by Color
- Product count grouped by `Color` (use `"No Color"` for null/empty values).
- Each segment gets an auto-generated HSL color with white borders between slices.
- Tooltip must show both the count and the percentage of total (e.g., `Red: 38 (7.5%)`).
- Legend positioned to the right.
- Below the chart, display a **"Total Products: X"** summary line.

### 3. Bar Chart — People by Person Type
- Number of people grouped by `PersonType`, using full labels: Store Contact, Individual, Sales Person, Employee, Vendor Contact, General Contact.
- **Each bar must have a different color** (use a palette like red, blue, green, orange, purple, teal).
- Bars should have rounded corners (`borderRadius: 4`).
- Tooltip shows `"X people"` format.
- Below the chart, display a **"Total People: X"** summary line.

### 4. Stock Management Table
- Products where `SafetyStockLevel` is below a configurable threshold (default 500).
- Columns: **Name, Category, Subcategory, Color, Size, Standard Cost, List Price, Safety Stock**.

**Filters** (inside a light-gray filter bar):
- **Threshold** — numeric input.
- **Name** — text search (case-insensitive partial match).
- **Category** — dropdown populated from filtered data.
- **Subcategory** — dropdown that adapts based on selected Category.
- **Color** — dropdown populated from filtered data.
- **Size** — dropdown that adapts based on selected Category/Subcategory.
- **Clear Filters** button to reset all filters at once.
- Above the table, show a badge with the count of matching products.

**Design requirements for the table:**
- **No red background.** Use alternating row colors (white / light gray) with a subtle hover effect.
- Wrap the table inside a white card container with rounded corners and a soft border.
- Column headers: dark background (`#2c3e50`), uppercase, small letter-spacing.
- **Color column** shows a small colored dot next to the color name.
- **Standard Cost / List Price** use monospaced font for alignment.
- **Safety Stock** column displays a pill-style badge with color coding:
  - **Critical** (≤ 10): red background, red text.
  - **Low** (≤ 100): yellow/amber background, dark yellow text.
  - **Medium** (> 100): green background, green text.

Use **Chart.js** (not vue-chartjs) installed via `npm install chart.js`.

---

## Important Technical Constraints

- The API's default pagination only returns 20 records. **Add a new `getAllForDashboard()` method** to both services that requests `pageSize: 9999` to get all records for the charts.
- The current `ProductDto` (in `src/CodingBasics.Domain/Contracts/Dtos.cs`) does **not** expose `SafetyStockLevel`, `ReorderPoint`, `StandardCost` or `Size`. You must add all four fields to the DTO and map them in the LINQ projections inside `src/CodingBasics.Infrastructure/Repositories/Repositories.cs` (both `GetAllAsync` and `SearchAsync`).
- Do **not** touch `Program.cs`, `DependencyInjection.cs`, entity classes, or any interface.

---

## Files to create or modify

- `src/CodingBasics.Domain/Contracts/Dtos.cs`
- `src/CodingBasics.Infrastructure/Repositories/Repositories.cs`
- `frontend/src/services/productsService.js`
- `frontend/src/services/peopleService.js`
- `frontend/src/views/DashboardView.vue` *(create)*
- `frontend/src/router/index.js`
- `frontend/src/App.vue`

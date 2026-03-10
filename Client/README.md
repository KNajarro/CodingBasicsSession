# CodingBasics Dashboard Client

Vue 3 + Vite dashboard that consumes the .NET API under `/api`.

## Features

- Inventory Value KPI card
- Product distribution by color chart
- Customer analysis by person type chart
- Stock management table with low-stock highlighting and filtering

## Run

```bash
npm install
npm run dev
```

The Vite proxy sends `/api/*` to `http://localhost:5261`.

## Example API calls used

- `GET /api/products?page=1&pageSize=500`
- `GET /api/people?page=1&pageSize=500`

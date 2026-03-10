# Adventure Works Frontend Challenge

A modern Vue 3 dashboard for analysing Adventure Works product and customer data.

## Features

- **KPI Cards**: Display key metrics including total inventory value, product count, and customer count
- **Color Distribution**: Donut chart showing product count per color
- **Customer Analysis**: Bar chart showing customer distribution by person type
- **Stock Management**: Interactive table with filtering and highlighting of low-stock products

## Setup & Installation

### Prerequisites
- Node.js 16+ 
- npm 8+

### Installation

```bash
npm install
```

### Development Server

```bash
npm run dev
```

The app will be available at `http://localhost:5174`

### Build for Production

```bash
npm build
```

## Environment Variables

Configure the backend API URL in `.env`:

```
VITE_API_BASE_URL=http://localhost:5261
```

## Backend Requirements

Make sure the backend API is running on `http://localhost:5261` with the following endpoints:
- `GET /api/products` - Returns products with pagination
- `GET /api/people` - Returns people with pagination

## Project Structure

```
src/
├── components/
│   ├── KPICard.vue                    # Reusable KPI metric card
│   ├── ColorDistributionChart.vue     # Donut chart for color distribution
│   ├── CustomerAnalysisChart.vue      # Bar chart for customer types
│   └── StockManagementTable.vue       # Filterable products table
├── views/
│   └── DashboardView.vue              # Main dashboard page
├── services/
│   ├── api.js                         # Axios instance
│   ├── productsService.js             # Products API calls
│   └── peopleService.js               # People API calls
├── router/
│   └── index.js                       # Vue Router config
├── App.vue
└── main.js
```

## Technologies Used

- Vue 3.4.15
- Vue Router 4.2.5
- Axios 1.6.7
- ApexCharts 3.45.0
- Bootstrap 5.3.2
- Vite 5.0.12

## Features Documentation

### KPI Card Component
Displays key performance indicators with:
- Currency, number, and percentage formatting
- Bootstrap icons
- Hover animations

### Color Distribution Chart
- Donut chart showing product count per color
- Interactive legend and tooltips
- Responsive design

### Customer Analysis Chart
- Bar chart showing customer count by person type
- Data labels and hover states
- Grid background

### Stock Management Table
- Real-time search by product name
- Filter by product color
- Color-coded rows based on safety stock levels:
  - 🔴 Red: < 50 units
  - 🟡 Yellow: 50-99 units
  - 🟢 Green: ≥ 100 units

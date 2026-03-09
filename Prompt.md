# Dashboard Implementation Prompt & Challenge

## Overview
This document specifies the complete requirements for building a production-ready dashboard application in Vue 3, assigned by the team leader as a coding challenge to demonstrate advanced Vue.js capabilities and best practices.

---

## Challenge Assignment
**Project**: Vue 3 Dashboard with Business Intelligence Features  
**Assigned By**: Team Leader  
**Technology Stack**: Vue 3, Vue Router, Axios, Chart.js, Vite  
**Scope**: Create a comprehensive analytics dashboard consuming a .NET API

---

## Requirements

### 1. Folder Structure
Create a new `Cliente/` folder within `@frontend/src/views/` with the following organization:
```
frontend/src/views/Cliente/
├── DashboardView.vue (Main dashboard component)
└── components/
    ├── InventoryValueCard.vue
    ├── DistributionByColorChart.vue
    ├── CustomerAnalysisChart.vue
    └── StockManagementTable.vue
```

### 2. Dashboard Components

#### 2.1 Inventory Value KPI Card
- **Purpose**: Display the total sum of `ListPrice` across all products
- **Features**:
  - Shows aggregated inventory value in USD currency format
  - Styled as an attractive KPI card with gradient background
  - Includes an icon and descriptive label
  - Responsive design with hover effects
  - Formula: `SUM(Product.ListPrice)` for all products

#### 2.2 Distribution by Color Chart
- **Type**: Pie or Doughnut Chart (using Chart.js)
- **Purpose**: Visualize product count per color
- **Features**:
  - Groups products by the `color` property
  - Displays count of products for each color
  - Handles "No Color" for products without color value
  - Interactive tooltips showing product counts
  - Legend positioned at the bottom
  - Responsive scaling

#### 2.3 Customer Analysis Chart
- **Type**: Bar Chart (using Chart.js)
- **Purpose**: Display number of people grouped by `PersonType`
- **Features**:
  - Aggregates people by PersonType codes: SC, IN, SP, EM, VC, GC
  - Shows counts for:
    - SC: Store Contact
    - IN: Individual
    - SP: Sales Person
    - EM: Employee
    - VC: Vendor Contact
    - GC: General Contact
  - Vertical bar chart with clear axis labels
  - Interactive tooltips with count information
  - Grid lines for better readability

#### 2.4 Stock Management Table
- **Purpose**: Highlight and manage products with low `SafetyStockLevel`
- **Features**:
  - Filters products where `SafetyStockLevel < 100`
  - Displays the following columns:
    - Product ID
    - Product Name (with product number)
    - Safety Stock Level (with badge styling)
    - Reorder Point
    - List Price (formatted as currency)
    - Status (Color-coded: Critical, Warning, Adequate)
  - Row highlighting based on stock level:
    - **Critical** (Red): SafetyStockLevel < 25 or = 0
    - **Warning** (Orange): SafetyStockLevel 25-49
    - **Low Risk** (Blue): SafetyStockLevel 50-99
  - Sorted by SafetyStockLevel in ascending order
  - Empty state message when all products are adequately stocked
  - Summary alert section with restock recommendations

### 3. Navigation Integration
- **Location**: Main navbar in App.vue
- **Implementation**: Add "Dashboard" link between "Home" and "People"
- **Route**: `/dashboard`
- **Active State**: Highlight current page using `router-link-active` class

### 4. API Integration
- **Data Sources**:
  - Products: `/api/products` endpoint (productsService)
  - People: `/api/people` endpoint (peopleService)
- **Data Loading Strategy**:
  - Load all products with pagination (pageSize: 500)
  - Load all people with pagination (pageSize: 500)
  - Computed properties for data aggregation
  - Error handling with user-friendly messages
  - Loading indicator during data fetch

### 5. Design & UX Guidelines
- **Color Scheme**: Follow existing project guidelines
  - Primary: #3498db (Blue)
  - Secondary: #2c3e50 (Dark Blue-Gray)
  - Accent: #667eea, #764ba2 (Purple gradient)
  - Success: #27ae60 (Green)
  - Warning: #ffa500 (Orange)
  - Critical: #ff6b6b (Red)

- **Typography**:
  - Headers: Bold, uppercase for section titles
  - Body: Clean, readable sans-serif
  - Font sizes: Responsive scaling for mobile

- **Spacing**: Consistent grid with 1rem, 1.5rem, 2rem intervals
- **Shadows**: Subtle elevation (0 2px 8px rgba(0, 0, 0, 0.1))
- **Borders**: Light gray (#eee, #dee2e6) for separators
- **Animations**: Smooth transitions (0.3s ease) for hover effects

### 6. Performance Considerations
- **Optimization**:
  - Use Vue 3 Composition API with `ref` and `computed`
  - Lazy-load charts only when component mounts
  - Computed properties for data transformations
  - Memoized calculations to avoid redundant processing
  - Responsive grid layout to minimize reflows

- **Best Practices**:
  - Proper error handling and user feedback
  - Loading states with spinner animation
  - Accessible component structure (semantic HTML)
  - Proper prop validation with type definitions
  - Scoped styling to prevent CSS conflicts

### 7. Responsive Design
- **Desktop** (1024px+): Multi-column grid layout
  - KPI cards: Auto-fit grid
  - Charts: 2-column layout
  - Table: Full-width with horizontal scroll

- **Tablet** (768px - 1023px): 1-column charts, responsive table
- **Mobile** (< 768px): Single column layout, optimized table view

### 8. Technical Implementation

#### Dependencies to Install
```bash
npm install chart.js vue-chartjs
```

#### File Structure
- **DashboardView.vue**: Main dashboard container
  - Uses Composition API with `ref` and `computed`
  - Manages data loading and error states
  - Orchestrates child components
  - Implements responsive layout

- **Component Files**: Each component follows Vue 3 best practices
  - Self-contained and reusable
  - Prop-based data flow
  - Proper type validation
  - Scoped styling

### 9. Quality Assurance
- **Testing Scenarios**:
  - Verify KPI calculation with various product sets
  - Validate color distribution across all color values
  - Confirm PersonType grouping accuracy
  - Test stock level filtering thresholds
  - Responsive design on multiple screen sizes
  - Error handling with network failures
  - Empty state handling

- **Code Review Focus Areas**:
  - Security: No XSS vulnerabilities, proper data validation
  - Performance: Efficient data processing, minimal re-renders
  - Maintainability: Clear code structure, proper comments, DRY principle
  - Accessibility: Semantic HTML, proper ARIA labels
  - User Experience: Smooth animations, clear feedback, error messages

---

## Deliverables

### Code Files Created
1. `frontend/src/views/Cliente/DashboardView.vue`
2. `frontend/src/views/Cliente/components/InventoryValueCard.vue`
3. `frontend/src/views/Cliente/components/DistributionByColorChart.vue`
4. `frontend/src/views/Cliente/components/CustomerAnalysisChart.vue`
5. `frontend/src/views/Cliente/components/StockManagementTable.vue`

### Configuration Updates
1. `frontend/src/router/index.js` - Added Dashboard route
2. `frontend/src/App.vue` - Added Dashboard navigation link
3. `frontend/package.json` - Added dependencies (chart.js, vue-chartjs)

### Documentation
- `Prompt.md` - This specification document

### Git Commit
- **Message**: "feat: implement business intelligence dashboard with KPI cards and analytics charts"
- **Context**: "Team Leader Challenge - Vue 3 Dashboard Implementation"
- **Scope**: Complete dashboard with 4 key features (Inventory Value, Color Distribution, Customer Analysis, Stock Management)

---

## Acceptance Criteria

✅ Dashboard loads without errors
✅ KPI card displays correct total inventory value
✅ Color distribution chart shows all product colors with accurate counts
✅ Customer analysis chart displays all 6 person types with correct counts
✅ Stock management table filters products correctly (SafetyStockLevel < 100)
✅ Row highlighting works based on stock level thresholds
✅ Navigation link appears in navbar and routes correctly
✅ Responsive design works on mobile, tablet, and desktop
✅ Error handling displays user-friendly messages
✅ Loading indicator displays during data fetch
✅ All components are properly documented and styled
✅ Code follows Vue 3 Composition API best practices
✅ Performance is optimized with computed properties
✅ Charts render correctly with interactive tooltips

---

## Notes for Development
- **Browser Compatibility**: Target modern browsers (ES6+)
- **API Assumptions**: Assumes stable .NET API endpoints
- **Data Assumptions**: 
  - Products have: `productID`, `name`, `productNumber`, `listPrice`, `color`, `safetyStockLevel`, `reorderPoint`
  - People have: `businessEntityID`, `personType`, `firstName`, `lastName`
- **Fallbacks**: Handle missing/null values gracefully (use default values)
- **Testing**: Manual testing on actual data to verify calculations

---

## Challenge Success Metrics
1. **Functionality**: All 4 dashboard widgets function correctly
2. **Data Accuracy**: Calculations match expected results
3. **User Experience**: Dashboard is intuitive and responsive
4. **Code Quality**: Follows Vue 3 best practices and project guidelines
5. **Performance**: Dashboard loads and renders efficiently
6. **Documentation**: Clear, comprehensive code documentation

---

**Challenge Status**: ✅ ASSIGNED AND IMPLEMENTED  
**Date**: March 09, 2026  
**Team Lead Approval**: Pending code review

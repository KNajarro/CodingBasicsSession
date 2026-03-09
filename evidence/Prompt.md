# Dashboard Implementation Prompt & Challenge

## Overview
This document specifies the complete requirements for building a production-ready dashboard application in Vue 3, assigned by the team leader as a coding challenge to demonstrate advanced Vue.js capabilities and best practices.

---

## Challenge Assignment
**Project**: Vue 3 Dashboard with Business Intelligence Features  
**Assigned By**: Team Leader  
**Technology Stack**: Vue 3, Vue Router, Axios, Chart.js, Vite  
**Scope**: Create a comprehensive analytics dashboard consuming a .NET API with enterprise-grade styling

---

## Requirements

### 1. Folder Structure
Create a new `Client/` folder within `@frontend/src/views/` with the following organization:
```
frontend/src/views/Client/
├── DashboardView.vue (Main dashboard component)
└── components/
    ├── InventoryValueCard.vue
    ├── DistributionByColorChart.vue
    ├── CustomerAnalysisChart.vue
    └── StockManagementTable.vue
```

### 2. Dashboard Components

#### 2.1 Dashboard Banner
- **Style**: Simple, clean banner matching Home, People, and Products views
- **Design**: Plain white background with h2 title in #2c3e50 color
- **Consistency**: Follows project's simple, minimalist header pattern
- **No gradient or decorative elements**

#### 2.2 Inventory Value KPI Card
- **Purpose**: Display the total sum of `ListPrice` across all products
- **Styling**: Professional enterprise design with:
  - White background with subtle border (#e0e0e0)
  - Left border accent (#3498db) for visual hierarchy
  - Light gray icon background (#ecf0f1)
  - Subtle box shadow for depth (0 2px 8px rgba(0,0,0,0.08))
  - Hover effect with enhanced shadow
  - **NO gradients** - clean, professional appearance
- **Layout**: Horizontal flex layout with icon on left, content on right
- **Typography**:
  - Label: Uppercase, small font, gray color (#7f8c8d)
  - Value: Large, bold, dark color (#2c3e50)
  - Description: Helper text in light gray (#95a5a6)
- **Currency Format**: USD with 0 decimal places
- **Responsive**: Stack vertically on mobile with centered text

#### 2.3 Product Distribution by Color Chart
- **Type**: Doughnut Chart (using Chart.js)
- **Purpose**: Visualize product count per color
- **Title**: "Product Distribution by Color"
- **Subtitle**: "Product count grouped by color"
- **Features**:
  - Groups products by the `color` property
  - Displays count of products for each color
  - Handles "No Color" for products without color value
  - Interactive tooltips showing product counts
  - Legend positioned at the bottom
  - Responsive scaling
  - Shows summary: Total X products across Y colors
- **Color Palette**: Dynamic color generation (15 distinct colors)

#### 2.4 Customer Analysis Chart
- **Type**: Bar Chart (using Chart.js)
- **Purpose**: Display number of people grouped by `PersonType`
- **Title**: "Customer Analysis by Person Type"
- **Subtitle**: "Distribution of people across different person types"
- **Features**:
  - Aggregates people by PersonType codes: SC, IN, SP, EM, VC, GC
  - Displays full labels:
    - SC: Store Contact
    - IN: Individual
    - SP: Sales Person
    - EM: Employee
    - VC: Vendor Contact
    - GC: General Contact
  - **Multi-color bars** - each person type has distinct color:
    - Store Contact: Blue (#3498db)
    - Individual: Red (#e74c3c)
    - Sales Person: Green (#2ecc71)
    - Employee: Orange (#f39c12)
    - Vendor Contact: Purple (#9b59b6)
    - General Contact: Teal (#1abc9c)
  - Vertical bar chart with clear axis labels
  - Interactive tooltips with count information
  - Grid lines for better readability
  - Shows summary: Total X people across Y categories

#### 2.5 Stock Management Table
- **Purpose**: Highlight and manage products with low `SafetyStockLevel`
- **Filter Criteria**: Shows products where `SafetyStockLevel < 100`
- **Search & Filter Features**:
  - Search by product name with **dedicated Search button** (prevents excessive API calls)
  - Search field accepts keyboard Enter key to trigger search
  - Filter by status (Critical, Warning, Adequate) - applies in real-time
  - Filter by minimum stock level (numeric input) - applies in real-time
  - **Search Button**: Only applies name filter when clicked (prevents API overload)
  - Clear button resets all filters, search, and pagination
  - Filters and pagination work together seamlessly
- **Pagination**:
  - Page size options: 10, 20, 50, 100 records per page
  - Previous/Next navigation buttons
  - Shows current page and total pages
  - Displays count of visible records
  - Pagination info section with page size selector
- **Table Columns**:
  - Product ID
  - Product Name (with product number below)
  - Safety Stock Level (in blue badge)
  - Reorder Point
  - List Price (formatted as currency, green text)
  - Status (color-coded badge)
- **Row Highlighting** based on stock level:
  - **Critical** (Light Red - #ffe4e6): SafetyStockLevel < 25 or = 0
  - **Warning** (Light Orange - #fff4e4): SafetyStockLevel 25-49
  - **Adequate** (Light Blue - #f0f8ff): SafetyStockLevel 50-99
- **Status Badges**:
  - Critical: Red background (#ff6b6b)
  - Warning: Orange background (#ffa500)
  - Adequate: Green background (#27ae60)
- **Sorting**: Default sort by SafetyStockLevel in ascending order
- **Empty State**: 
  - Shows message when no products match filters
  - Different message if all products are adequately stocked
- **Alert Section**: Summary message about products below threshold
- **Responsive Design**: Filters stack on mobile, pagination controls adapt

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
  - Load all products with pagination using pageSize: 5000 (ensures all data loaded on first request)
  - Load all people with pagination using pageSize: 5000 (ensures all data loaded on first request)
  - Computed properties for data aggregation
  - Error handling with user-friendly messages
  - Loading indicator during data fetch
- **API Endpoints Used**:
  - `GET /api/products?page=1&pageSize=5000` - Retrieves all products
  - `GET /api/people?page=1&pageSize=5000` - Retrieves all people

### 5. Design & UX Guidelines
- **Color Scheme**: Follow existing project guidelines
  - Primary: #3498db (Blue)
  - Secondary: #2c3e50 (Dark Blue-Gray)
  - Success: #27ae60 (Green)
  - Warning: #ffa500 (Orange)
  - Critical: #ff6b6b (Red)
  - Info: #1abc9c (Teal)
  - Text: #555 (Medium Gray)
  - Light Background: #f8f9fa

- **Typography**:
  - Headers: Bold, #2c3e50 for clarity
  - Body: Clean, readable sans-serif
  - Font sizes: Responsive scaling for mobile

- **Spacing**: Consistent grid with 1rem, 1.5rem, 2rem intervals
- **Shadows**: Subtle elevation (0 2px 8px rgba(0, 0, 0, 0.08-0.1))
- **Borders**: Light gray (#dee2e6, #eee) for separators
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
  - Table: Full-width with horizontal scroll if needed
  - Filters: Single row layout

- **Tablet** (768px - 1023px): 
  - Charts: 1-column layout
  - Responsive table with adjusted padding
  - Filters: Flexible wrap

- **Mobile** (< 768px): 
  - Single column layout
  - Stacked filters and pagination controls
  - Optimized table view with smaller fonts
  - Hidden product numbers in table

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
  - Simple h2 banner (no gradient)

- **InventoryValueCard.vue**: KPI Card Component
  - Professional enterprise styling
  - White background with left border accent
  - Flex layout with icon and content
  - Hover effects with shadow enhancement
  - No gradients or decorative elements

- **DistributionByColorChart.vue**: Product Color Distribution
  - Doughnut chart wrapper
  - Dynamic color generation for variable number of colors
  - Responsive sizing
  - Summary statistics display

- **CustomerAnalysisChart.vue**: Person Type Bar Chart
  - Multi-color bar chart (6 distinct colors)
  - One color per person type for clear distinction
  - Grid lines and readable axis labels
  - Dynamic color assignment

- **StockManagementTable.vue**: Stock Management Component
  - Data state management for search, filters, pagination
  - Computed properties for filtering and sorting
  - Pagination logic with page size selector
  - Search and filter methods
  - Status determination and styling methods
  - Responsive filter and pagination layout

### 9. Quality Assurance
- **Testing Scenarios**:
  - Verify KPI calculation with various product sets
  - Validate color distribution across all color values
  - Confirm PersonType grouping accuracy
  - Test stock level filtering thresholds
  - Test search and filter combinations
  - Test pagination with different page sizes
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
1. `frontend/src/views/Client/DashboardView.vue`
2. `frontend/src/views/Client/components/InventoryValueCard.vue`
3. `frontend/src/views/Client/components/DistributionByColorChart.vue`
4. `frontend/src/views/Client/components/CustomerAnalysisChart.vue`
5. `frontend/src/views/Client/components/StockManagementTable.vue`

### Configuration Updates
1. `frontend/src/router/index.js` - Added Dashboard route
2. `frontend/src/App.vue` - Added Dashboard navigation link
3. `frontend/package.json` - Added dependencies (chart.js, vue-chartjs)

### Documentation
- `Prompt.md` - This specification document (Updated with design enhancements)

---

## Acceptance Criteria

✅ Dashboard loads without errors  
✅ KPI card displays correct total inventory value with professional styling  
✅ Color distribution chart shows all product colors with accurate counts  
✅ Customer analysis chart displays all 6 person types with distinct colors  
✅ Stock management table filters products correctly (SafetyStockLevel < 100)  
✅ Search functionality works for product names  
✅ Status filter works correctly (Critical, Warning, Adequate)  
✅ Pagination works with multiple page sizes  
✅ Filters and pagination work together seamlessly  
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
- **Testing**: Manual testing on actual data to verify calculations and filtering
- **Styling**: All colors defined in project color scheme
- **Accessibility**: Use semantic HTML, proper labels on form inputs

---

## Design Evolution

### Initial Implementation
- Gradient header (#667eea → #764ba2)
- Single color bar chart
- Limited filtering capabilities
- No pagination on stock table

### Version 1.0 - First Redesign
- **Simple banner**: Consistent with Home, People, Products views
- **Professional KPI Card**: Enterprise styling without gradients
  - White background with subtle borders and shadows
  - Left accent border for visual hierarchy
  - No gradient effects
- **Enhanced Customer Chart**: Multi-color bars (6 distinct colors)
  - Each person type has unique, memorable color
  - Improved visual distinction
- **Advanced Stock Table**: 
  - Full search and filter capabilities
  - Pagination with configurable page size
  - Responsive filter layout
  - Dynamic filtering by name, status, and stock level
  - Clear and intuitive user interface

### Version 2.0 - API Optimization & Search Button (Current)
- **Fixed Data Loading**: Updated to use pageSize 5000 to ensure all products/people are loaded
  - Resolved issue with incomplete data loading
  - Ensures comprehensive dashboard data on first load
- **Smart Search Implementation**:
  - Separated search input from active filter
  - Added dedicated "Search" button to apply name filter
  - Prevents excessive API calls on every keystroke
  - Enter key support for quick search
  - Improves UX and reduces server load
- **Enhanced Filter UI**:
  - Search button alongside other filters
  - Clear button to reset all filters, searches, and pagination
  - Better visual organization of filter controls

---

## Challenge Success Metrics
1. **Functionality**: All 4 dashboard widgets function correctly with new features
2. **Data Accuracy**: Calculations match expected results
3. **User Experience**: Dashboard is intuitive, responsive, and feature-rich
4. **Code Quality**: Follows Vue 3 best practices and project guidelines
5. **Performance**: Dashboard loads and renders efficiently
6. **Design**: Professional, enterprise-grade appearance
7. **Documentation**: Clear, comprehensive code and specification documentation

---

**Challenge Status**: ✅ IMPLEMENTED AND REDESIGNED  
**Date**: March 09, 2026  
**Version**: 2.0 (Redesigned with Enhanced Features)  
**Team Lead Approval**: Pending code review

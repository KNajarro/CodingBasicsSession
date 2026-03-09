<template>
  <div class="stock-management">
    <!-- Header with Title and Stats -->
    <div class="stock-header">
      <h3 class="section-title">Stock Management</h3>
      <div class="stock-stats">
        <span class="stat-badge">{{ filteredProducts.length }} products</span>
        <span class="stat-badge warning">Below SafetyStockLevel</span>
      </div>
    </div>

    <!-- Filters Section -->
    <div class="filters-section">
      <div class="filter-group">
        <label>Search by Product Name:</label>
        <input type="text" v-model="searchNameInput" placeholder="Search product name..." class="filter-input"
          @keyup.enter="handleSearch" />
      </div>
      <div class="filter-group">
        <label>Filter by Category:</label>
        <select v-model="filterCategory" class="filter-select">
          <option value="">All Categories</option>
          <option v-for="category in uniqueCategories" :key="category" :value="category">
            {{ category }}
          </option>
        </select>
      </div>
      <div class="filter-group">
        <label>Filter by Subcategory:</label>
        <select v-model="filterSubcategory" class="filter-select">
          <option value="">All Subcategories</option>
          <option v-for="subcategory in filteredSubcategories" :key="subcategory" :value="subcategory">
            {{ subcategory }}
          </option>
        </select>
      </div>
      <div class="filter-group">
        <label>Filter by Status:</label>
        <select v-model="filterStatus" class="filter-select">
          <option value="">All Status</option>
          <option value="critical">Critical</option>
          <option value="warning">Warning</option>
          <option value="adequate">Adequate</option>
        </select>
      </div>
      <div class="filter-group">
        <label>Min Stock Level:</label>
        <input type="number" v-model.number="filterMinStock" placeholder="Min..."
          class="filter-input filter-input-small" />
      </div>
      <button @click="handleSearch" class="btn btn-primary filter-btn">Search</button>
      <button @click="clearFilters" class="btn btn-secondary filter-btn">Clear</button>
    </div>

    <!-- Empty State -->
    <div v-if="filteredProducts.length === 0" class="empty-stock">
      <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
        <path d="M9 12h6m-6 4h6m2-9H7a2 2 0 00-2 2v12a2 2 0 002 2h10a2 2 0 002-2V9a2 2 0 00-2-2z"></path>
      </svg>
      <p>{{ allProductsAdequatelyStocked ? 'All products are adequately stocked!' : 'No products match your filters.' }}
      </p>
    </div>

    <!-- Table Section -->
    <div v-else class="table-wrapper">
      <table class="stock-table">
        <thead>
          <tr>
            <th @click="sortBy('productID')" class="sortable">
              Product ID
              <span v-if="sortKey === 'productID'" class="sort-indicator">{{ sortOrder === 'asc' ? '▲' : '▼' }}</span>
            </th>
            <th @click="sortBy('name')" class="sortable">
              Product Name
              <span v-if="sortKey === 'name'" class="sort-indicator">{{ sortOrder === 'asc' ? '▲' : '▼' }}</span>
            </th>
            <th @click="sortBy('categoryName')" class="sortable">
              Category
              <span v-if="sortKey === 'categoryName'" class="sort-indicator">{{ sortOrder === 'asc' ? '▲' : '▼'
                }}</span>
            </th>
            <th @click="sortBy('subcategoryName')" class="sortable">
              Subcategory
              <span v-if="sortKey === 'subcategoryName'" class="sort-indicator">{{ sortOrder === 'asc' ? '▲' : '▼'
                }}</span>
            </th>
            <th @click="sortBy('safetyStockLevel')" class="sortable">
              Safety Stock Level
              <span v-if="sortKey === 'safetyStockLevel'" class="sort-indicator">{{ sortOrder === 'asc' ? '▲' : '▼'
                }}</span>
            </th>
            <th @click="sortBy('listPrice')" class="sortable">
              List Price
              <span v-if="sortKey === 'listPrice'" class="sort-indicator">{{ sortOrder === 'asc' ? '▲' : '▼' }}</span>
            </th>
            <th @click="sortBy('status')" class="sortable">
              Status
              <span v-if="sortKey === 'status'" class="sort-indicator">{{ sortOrder === 'asc' ? '▲' : '▼' }}</span>
            </th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="product in paginatedProducts" :key="product.productID" :class="getRowClass(product)">
            <td class="id-cell">{{ product.productID }}</td>
            <td class="name-cell">
              <div class="product-name">{{ product.name }}</div>
              <div class="product-number">{{ product.productNumber }}</div>
            </td>
            <td class="category-cell">{{ product.categoryName || '-' }}</td>
            <td class="subcategory-cell">{{ product.subcategoryName || '-' }}</td>
            <td class="stock-level">{{ product.safetyStockLevel || 0 }}</td>
            <td class="price">{{ formatPrice(product.listPrice) }}</td>
            <td class="status">
              <span :class="getStatusClass(product)">{{ getStatusText(product) }}</span>
            </td>
          </tr>
        </tbody>
      </table>
    </div>

    <!-- Pagination Section -->
    <div v-if="filteredProducts.length > 0" class="pagination-info">
      <div class="info-text">
        <span>Page <strong>{{ currentPage }}</strong> of <strong>{{ totalPages }}</strong></span>
        <span class="separator">•</span>
        <span>Showing <strong>{{ paginatedProducts.length }}</strong> of <strong>{{ filteredProducts.length }}</strong>
          records</span>
      </div>
      <div class="pagination-controls">
        <button @click="previousPage" class="btn btn-pagination" :disabled="currentPage === 1">
          ← Previous
        </button>
        <div class="page-size-selector">
          <label>Page Size:</label>
          <select v-model.number="pageSize" @change="resetPage">
            <option value="10">10</option>
            <option value="20">20</option>
            <option value="50">50</option>
            <option value="100">100</option>
          </select>
        </div>
        <button @click="nextPage" class="btn btn-pagination" :disabled="currentPage >= totalPages">
          Next →
        </button>
      </div>
    </div>

    <!-- Alert Section -->
    <div v-if="filteredProducts.length > 0" class="stock-footer">
      <p class="footer-text">
        <strong>Alert:</strong> {{ lowStockCount }} product(s) have safety stock levels below 100 units.
        Consider restocking to avoid supply disruptions.
      </p>
    </div>
  </div>
</template>

<script>
export default {
  name: 'StockManagementTable',
  props: {
    products: {
      type: Array,
      required: true,
      default: () => []
    }
  },
  data() {
    return {
      currentPage: 1,
      pageSize: 20,
      searchNameInput: '', // Input field for search (not applied until button click)
      searchName: '', // Actual search term used for filtering
      filterCategory: '',
      filterSubcategory: '',
      filterStatus: '',
      filterMinStock: null,
      sortKey: 'safetyStockLevel', // Default sort by stock level
      sortOrder: 'asc' // asc or desc
    }
  },
  computed: {
    uniqueCategories() {
      const categories = new Set()
      this.products.forEach(product => {
        if (product.categoryName) {
          categories.add(product.categoryName)
        }
      })
      return Array.from(categories).sort()
    },

    filteredSubcategories() {
      const subcategories = new Set()
      this.products.forEach(product => {
        // Only include subcategories that match selected category (if any)
        if (!this.filterCategory || product.categoryName === this.filterCategory) {
          if (product.subcategoryName) {
            subcategories.add(product.subcategoryName)
          }
        }
      })
      return Array.from(subcategories).sort()
    },

    filteredProducts() {
      let filtered = this.products.filter(product => {
        const stockLevel = product.safetyStockLevel || 0

        // Filter by name search (applied only when search button clicked)
        if (this.searchName && !product.name.toLowerCase().includes(this.searchName.toLowerCase())) {
          return false
        }

        // Filter by category
        if (this.filterCategory && product.categoryName !== this.filterCategory) {
          return false
        }

        // Filter by subcategory
        if (this.filterSubcategory && product.subcategoryName !== this.filterSubcategory) {
          return false
        }

        // Filter by status
        if (this.filterStatus) {
          const status = this.getStatusKey(product)
          if (status !== this.filterStatus) {
            return false
          }
        }

        // Filter by minimum stock level
        if (this.filterMinStock !== null && this.filterMinStock !== '' && stockLevel < this.filterMinStock) {
          return false
        }

        return true
      })

      // Sort by selected key
      return filtered.sort((a, b) => {
        let aValue = a[this.sortKey]
        let bValue = b[this.sortKey]

        // Handle null/undefined values
        if (aValue === null || aValue === undefined) aValue = ''
        if (bValue === null || bValue === undefined) bValue = ''

        // Compare values
        if (typeof aValue === 'string') {
          aValue = aValue.toLowerCase()
          bValue = bValue.toLowerCase()
          return this.sortOrder === 'asc' ? aValue.localeCompare(bValue) : bValue.localeCompare(aValue)
        } else {
          return this.sortOrder === 'asc' ? aValue - bValue : bValue - aValue
        }
      })
    },

    totalPages() {
      return Math.ceil(this.filteredProducts.length / this.pageSize) || 1
    },

    paginatedProducts() {
      const start = (this.currentPage - 1) * this.pageSize
      const end = start + this.pageSize
      return this.filteredProducts.slice(start, end)
    },

    lowStockCount() {
      return this.filteredProducts.filter(product => {
        const stockLevel = product.safetyStockLevel || 0
        return stockLevel < 100
      }).length
    },

    allProductsAdequatelyStocked() {
      return this.products.length === 0
    }
  },
  methods: {
    formatPrice(price) {
      return new Intl.NumberFormat('en-US', {
        style: 'currency',
        currency: 'USD'
      }).format(price)
    },
    getRowClass(product) {
      const stockLevel = product.safetyStockLevel || 0
      if (stockLevel === 0) return 'critical'
      if (stockLevel < 25) return 'critical'
      if (stockLevel < 50) return 'high-risk'
      return 'low-risk'
    },
    getStatusClass(product) {
      const stockLevel = product.safetyStockLevel || 0
      if (stockLevel === 0) return 'status-critical'
      if (stockLevel < 25) return 'status-critical'
      if (stockLevel < 50) return 'status-warning'
      return 'status-info'
    },
    getStatusText(product) {
      const stockLevel = product.safetyStockLevel || 0
      if (stockLevel === 0) return 'Critical'
      if (stockLevel < 25) return 'Critical'
      if (stockLevel < 50) return 'Warning'
      return 'Adequate'
    },
    getStatusKey(product) {
      const stockLevel = product.safetyStockLevel || 0
      if (stockLevel === 0 || stockLevel < 25) return 'critical'
      if (stockLevel < 50) return 'warning'
      return 'adequate'
    },
    handleSearch() {
      // Apply the search term only when button is clicked
      this.searchName = this.searchNameInput
      this.currentPage = 1 // Reset to first page
    },
    previousPage() {
      if (this.currentPage > 1) {
        this.currentPage--
      }
    },
    nextPage() {
      if (this.currentPage < this.totalPages) {
        this.currentPage++
      }
    },
    resetPage() {
      this.currentPage = 1
    },
    sortBy(key) {
      // If clicking the same header, toggle sort order
      if (this.sortKey === key) {
        this.sortOrder = this.sortOrder === 'asc' ? 'desc' : 'asc'
      } else {
        // If clicking a different header, set to ascending and reset to first page
        this.sortKey = key
        this.sortOrder = 'asc'
      }
      // Always go to first page when sorting
      this.currentPage = 1
    },
    clearFilters() {
      this.searchNameInput = ''
      this.searchName = ''
      this.filterCategory = ''
      this.filterSubcategory = ''
      this.filterStatus = ''
      this.filterMinStock = null
      this.sortKey = 'safetyStockLevel'
      this.sortOrder = 'asc'
      this.currentPage = 1
    }
  }
}
</script>

<style scoped>
.stock-management {
  display: flex;
  flex-direction: column;
}

.stock-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 1.5rem;
  padding-bottom: 1rem;
  border-bottom: 2px solid #3498db;
}

.section-title {
  margin: 0;
  font-size: 1.1rem;
  font-weight: 600;
  color: #2c3e50;
}

.stock-stats {
  display: flex;
  gap: 0.75rem;
  align-items: center;
}

.stat-badge {
  display: inline-block;
  padding: 0.4rem 0.8rem;
  background: #ecf0f1;
  color: #555;
  border-radius: 20px;
  font-size: 0.85rem;
  font-weight: 500;
}

.stat-badge.warning {
  background: #ffe4e6;
  color: #c00;
}

/* Filters Section */
.filters-section {
  display: flex;
  gap: 1rem;
  margin-bottom: 1.5rem;
  flex-wrap: wrap;
  align-items: flex-end;
  padding: 1rem;
  background: #f8f9fa;
  border-radius: 8px;
}

.filter-group {
  display: flex;
  flex-direction: column;
  gap: 0.4rem;
}

.filter-group label {
  font-size: 0.85rem;
  font-weight: 600;
  color: #2c3e50;
}

.filter-input,
.filter-select {
  padding: 0.5rem 0.75rem;
  border: 1px solid #dee2e6;
  border-radius: 4px;
  font-size: 0.9rem;
  min-width: 150px;
}

.filter-input:focus,
.filter-select:focus {
  outline: none;
  border-color: #3498db;
  box-shadow: 0 0 0 3px rgba(52, 152, 219, 0.1);
}

.filter-input-small {
  min-width: 145px !important;
  width: 145px;
}

.filter-btn {
  margin-top: 0.4rem;
}

/* Empty State */
.empty-stock {
  text-align: center;
  padding: 3rem 2rem;
  color: #666;
  background: #f8f9fa;
  border-radius: 8px;
}

.empty-stock svg {
  width: 48px;
  height: 48px;
  margin-bottom: 1rem;
  opacity: 0.5;
}

.empty-stock p {
  margin: 0;
  font-size: 1.1rem;
}

/* Table */
.table-wrapper {
  overflow-x: auto;
  border-radius: 8px;
  background: white;
  margin-bottom: 1rem;
}

.stock-table {
  width: 100%;
  border-collapse: collapse;
  font-size: 0.95rem;
}

.stock-table thead {
  background: #f8f9fa;
  border-bottom: 2px solid #dee2e6;
}

.stock-table th {
  padding: 1rem;
  text-align: left;
  font-weight: 600;
  color: #2c3e50;
  font-size: 0.9rem;
  text-transform: uppercase;
  letter-spacing: 0.5px;
  white-space: nowrap;
}

.stock-table th.sortable {
  cursor: pointer;
  user-select: none;
  transition: background-color 0.2s ease;
}

.stock-table th.sortable:hover {
  background-color: #ecf0f1;
}

.sort-indicator {
  margin-left: 0.5rem;
  font-size: 0.75rem;
  display: inline-block;
}

.stock-table td {
  padding: 0.75rem 1rem;
  border-bottom: 1px solid #dee2e6;
  color: #555;
}

.stock-table tbody tr {
  transition: background-color 0.3s ease;
}

.stock-table tbody tr:hover {
  background: #f8f9fa;
}

.stock-table tbody tr.critical {
  background: #ffe4e6;
}

.stock-table tbody tr.critical:hover {
  background: #ffd4d9;
}

.stock-table tbody tr.high-risk {
  background: #fff4e4;
}

.stock-table tbody tr.high-risk:hover {
  background: #ffe4cc;
}

.stock-table tbody tr.low-risk {
  background: #f0f8ff;
}

.stock-table tbody tr.low-risk:hover {
  background: #e0f0ff;
}

.id-cell {
  font-weight: 600;
  color: #2c3e50;
  width: 80px;
}

.name-cell {
  min-width: 200px;
}

.product-name {
  font-weight: 600;
  color: #2c3e50;
}

.product-number {
  font-size: 0.85rem;
  color: #999;
  margin-top: 0.2rem;
}

.stock-level {
  text-align: left;
}

.badge {
  display: inline-block;
  background: #3498db;
  color: white;
  padding: 0.3rem 0.6rem;
  border-radius: 4px;
  font-weight: 600;
  font-size: 0.9rem;
}

.category-cell {
  text-align: left;
}

.subcategory-cell {
  text-align: left;
}

.reorder-point {
  text-align: left;
  color: #999;
}

.price {
  font-weight: 600;
  color: #27ae60;
  text-align: left;
  min-width: 100px;
}

.status {
  text-align: left;
}

.status-critical {
  display: inline-block;
  padding: 0.3rem 0.8rem;
  background: #ff6b6b;
  color: white;
  border-radius: 4px;
  font-size: 0.85rem;
  font-weight: 600;
  text-transform: uppercase;
}

.status-warning {
  display: inline-block;
  padding: 0.3rem 0.8rem;
  background: #ffa500;
  color: white;
  border-radius: 4px;
  font-size: 0.85rem;
  font-weight: 600;
  text-transform: uppercase;
}

.status-info {
  display: inline-block;
  padding: 0.3rem 0.8rem;
  background: #27ae60;
  color: white;
  border-radius: 4px;
  font-size: 0.85rem;
  font-weight: 600;
  text-transform: uppercase;
}

/* Pagination */
.pagination-info {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin: 1.5rem 0;
  padding: 1rem;
  background: #f8f9fa;
  border-radius: 8px;
  flex-wrap: wrap;
  gap: 1rem;
}

.info-text {
  display: flex;
  gap: 0.5rem;
  font-size: 0.95rem;
  color: #555;
}

.separator {
  color: #ccc;
}

.pagination-controls {
  display: flex;
  gap: 1rem;
  align-items: center;
  flex-wrap: wrap;
}

.page-size-selector {
  display: flex;
  gap: 0.5rem;
  align-items: center;
  font-size: 0.9rem;
}

.page-size-selector label {
  color: #555;
  font-weight: 500;
}

.page-size-selector select {
  padding: 0.4rem 0.6rem;
  border: 1px solid #dee2e6;
  border-radius: 4px;
  background: white;
  cursor: pointer;
}

.btn {
  padding: 0.5rem 1rem;
  border: 1px solid #dee2e6;
  border-radius: 4px;
  font-size: 0.9rem;
  font-weight: 500;
  cursor: pointer;
  transition: all 0.2s ease;
}

.btn-primary {
  background: #3498db;
  color: white;
  border-color: #3498db;
}

.btn-primary:hover {
  background: #2980b9;
  border-color: #2980b9;
}

.btn-secondary {
  background: #95a5a6;
  color: white;
  border-color: #95a5a6;
}

.btn-secondary:hover {
  background: #7f8c8d;
  border-color: #7f8c8d;
}

.btn-pagination {
  background: white;
  color: #2c3e50;
  border-color: #dee2e6;
}

.btn-pagination:hover:not(:disabled) {
  background: #ecf0f1;
  border-color: #3498db;
}

.btn-pagination:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

/* Alert Section */
.stock-footer {
  margin-top: 1rem;
  padding: 1rem;
  background: #fff3cd;
  border-left: 4px solid #ffa500;
  border-radius: 4px;
}

.footer-text {
  margin: 0;
  font-size: 0.95rem;
  color: #856404;
}

/* Responsive */
@media (max-width: 1024px) {
  .filters-section {
    flex-direction: column;
    align-items: stretch;
  }

  .filter-group {
    flex: 1;
  }

  .filter-input,
  .filter-select {
    width: 100%;
    min-width: unset;
  }

  .filter-btn {
    width: 100%;
  }

  .pagination-info {
    flex-direction: column;
    align-items: flex-start;
  }

  .pagination-controls {
    width: 100%;
    justify-content: flex-start;
  }
}

@media (max-width: 768px) {
  .stock-header {
    flex-direction: column;
    align-items: flex-start;
    gap: 1rem;
  }

  .stock-stats {
    width: 100%;
    justify-content: flex-start;
  }

  .filters-section {
    padding: 0.75rem;
  }

  .stock-table {
    font-size: 0.85rem;
  }

  .stock-table th,
  .stock-table td {
    padding: 0.5rem;
  }

  .name-cell {
    min-width: 150px;
  }

  .product-number {
    display: none;
  }

  .pagination-info {
    flex-direction: column;
    align-items: flex-start;
  }

  .pagination-controls {
    width: 100%;
    flex-direction: column;
    gap: 0.5rem;
  }

  .page-size-selector {
    width: 100%;
  }

  .btn-pagination {
    width: 100%;
  }
}
</style>

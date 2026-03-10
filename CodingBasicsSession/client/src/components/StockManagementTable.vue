<template>
  <div class="table-container">
    <div class="table-header">
      <h5 class="table-title mb-3">Stock Management</h5>
      
      <div class="row g-3 mb-4">
        <div class="col-md-6">
          <div class="input-group">
            <span class="input-group-text"><i class="bi bi-search"></i></span>
            <input 
              v-model="searchQuery" 
              type="text" 
              class="form-control" 
              placeholder="Search by product name..."
            >
          </div>
        </div>
        <div class="col-md-6">
          <div class="input-group">
            <span class="input-group-text"><i class="bi bi-funnel"></i></span>
            <select v-model="colorFilter" class="form-select">
              <option value="">All Colors</option>
              <option v-for="color in availableColors" :key="color" :value="color">
                {{ color }}
              </option>
            </select>
          </div>
        </div>
      </div>

      <div class="filter-info mb-3">
        <span class="badge bg-warning text-dark">
          <i class="bi bi-exclamation-triangle"></i> 
          {{ lowStockProducts.length }} products with low stock
        </span>
      </div>
    </div>

    <div v-if="filteredProducts.length > 0" class="table-responsive">
      <table class="table table-hover mb-0">
        <thead class="table-light">
          <tr>
            <th>Product Name</th>
            <th>Color</th>
            <th>List Price</th>
            <th class="stock-level-header">Safety Stock Level</th>
            <th>Category</th>
          </tr>
        </thead>
        <tbody>
          <tr 
            v-for="product in filteredProducts" 
            :key="product.productID"
            :class="getRowClass(product)"
          >
            <td class="product-name">
              <i v-if="isLowStock(product)" class="bi bi-exclamation-circle-fill text-danger"></i>
              {{ product.name }}
            </td>
            <td>
              <span class="badge" :style="{ backgroundColor: getColorBadge(product.color) }">
                {{ product.color || 'N/A' }}
              </span>
            </td>
            <td class="price">{{ formatPrice(product.listPrice) }}</td>
            <td :class="getStockClass(product)">
              <strong>{{ product.safetyStockLevel }}</strong>
            </td>
            <td>{{ product.categoryName || 'Uncategorized' }}</td>
          </tr>
        </tbody>
      </table>
    </div>

    <div v-else class="alert alert-info mt-3">
      No products found matching your filters.
    </div>

    <div v-if="filteredProducts.length > 0" class="table-footer mt-3 text-muted">
      Showing {{ filteredProducts.length }} of {{ products.length }} products
    </div>
  </div>
</template>

<script>
export default {
  name: 'StockManagementTable',
  props: {
    products: {
      type: Array,
      default: () => []
    }
  },
  data() {
    return {
      searchQuery: '',
      colorFilter: ''
    }
  },
  computed: {
    availableColors() {
      const colors = new Set()
      this.products.forEach(product => {
        if (product.color) colors.add(product.color)
      })
      return Array.from(colors).sort()
    },
    
    lowStockProducts() {
      return this.products.filter(p => p.safetyStockLevel < 100)
    },
    
    filteredProducts() {
      return this.products.filter(product => {
        const matchesSearch = product.name.toLowerCase().includes(this.searchQuery.toLowerCase())
        const matchesColor = !this.colorFilter || product.color === this.colorFilter
        return matchesSearch && matchesColor
      })
    }
  },
  methods: {
    isLowStock(product) {
      return product.safetyStockLevel < 100
    },
    
    getRowClass(product) {
      if (product.safetyStockLevel < 50) {
        return 'table-danger'
      } else if (product.safetyStockLevel < 100) {
        return 'table-warning'
      } else {
        return 'table-success'
      }
    },
    
    getStockClass(product) {
      if (product.safetyStockLevel < 50) {
        return 'text-danger'
      } else if (product.safetyStockLevel < 100) {
        return 'text-warning'
      } else {
        return 'text-success'
      }
    },
    
    getColorBadge(color) {
      const colorMap = {
        'Black': '#000000',
        'Silver': '#c0c0c0',
        'Red': '#dc3545',
        'Blue': '#007bff',
        'Green': '#28a745',
        'Yellow': '#ffc107',
        'White': '#f8f9fa',
        'Orange': '#fd7e14',
        'Pink': '#e83e8c',
        'Purple': '#6f42c1'
      }
      return colorMap[color] || '#6c757d'
    },
    
    formatPrice(price) {
      return new Intl.NumberFormat('en-US', {
        style: 'currency',
        currency: 'USD'
      }).format(price)
    }
  }
}
</script>

<style scoped>
.table-container {
  background: white;
  border-radius: 8px;
  padding: 20px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
}

.table-title {
  font-weight: 600;
  color: #1a1a1a;
}

.table-responsive {
  border-radius: 4px;
  overflow: hidden;
}

.table {
  margin: 0;
}

.table thead {
  background-color: #f8f9fa;
}

.table th {
  font-weight: 600;
  padding: 12px;
  border-bottom: 2px solid #dee2e6;
  font-size: 0.875rem;
  text-transform: uppercase;
  letter-spacing: 0.5px;
}

.table td {
  padding: 12px;
  vertical-align: middle;
}

.product-name {
  font-weight: 500;
}

.price {
  font-weight: 500;
  color: #28a745;
}

.table-warning {
  background-color: #fff3cd;
}

.table-danger {
  background-color: #f8d7da;
}

.table-success {
  background-color: #d4edda;
}

.filter-info {
  display: flex;
  gap: 10px;
}

.table-footer {
  text-align: right;
  font-size: 0.875rem;
}

.stock-level-header {
  min-width: 150px;
}
</style>

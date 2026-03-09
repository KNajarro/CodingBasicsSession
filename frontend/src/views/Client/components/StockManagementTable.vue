<template>
  <div class="stock-management">
    <div class="stock-header">
      <h3 class="section-title">Stock Management</h3>
      <div class="stock-stats">
        <span class="stat-badge">{{ products.length }} products</span>
        <span class="stat-badge warning">Below SafetyStockLevel</span>
      </div>
    </div>

    <div v-if="products.length === 0" class="empty-stock">
      <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
        <path d="M9 12h6m-6 4h6m2-9H7a2 2 0 00-2 2v12a2 2 0 002 2h10a2 2 0 002-2V9a2 2 0 00-2-2z"></path>
      </svg>
      <p>All products are adequately stocked!</p>
    </div>

    <div v-else class="table-wrapper">
      <table class="stock-table">
        <thead>
          <tr>
            <th>Product ID</th>
            <th>Product Name</th>
            <th>Safety Stock Level</th>
            <th>Reorder Point</th>
            <th>List Price</th>
            <th>Status</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="product in products" :key="product.productID" :class="getRowClass(product)">
            <td class="id-cell">{{ product.productID }}</td>
            <td class="name-cell">
              <div class="product-name">{{ product.name }}</div>
              <div class="product-number">{{ product.productNumber }}</div>
            </td>
            <td class="stock-level">
              <span class="badge">{{ product.safetyStockLevel || 0 }}</span>
            </td>
            <td class="reorder-point">{{ product.reorderPoint || '-' }}</td>
            <td class="price">{{ formatPrice(product.listPrice) }}</td>
            <td class="status">
              <span :class="getStatusClass(product)">{{ getStatusText(product) }}</span>
            </td>
          </tr>
        </tbody>
      </table>
    </div>

    <div v-if="products.length > 0" class="stock-footer">
      <p class="footer-text">
        <strong>Alert:</strong> {{ products.length }} product(s) have safety stock levels below 100 units.
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

.table-wrapper {
  overflow-x: auto;
  border-radius: 8px;
  background: white;
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
  text-align: center;
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

.reorder-point {
  text-align: center;
  color: #999;
}

.price {
  font-weight: 600;
  color: #27ae60;
  text-align: right;
  min-width: 100px;
}

.status {
  text-align: center;
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

.stock-footer {
  margin-top: 1.5rem;
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
}
</style>

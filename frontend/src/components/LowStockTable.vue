<template>
  <div class="table-wrapper">
    <h3>Low Stock Products</h3>
    <div class="table-container-scroll">
      <table class="data-table">
      <thead>
        <tr>
          <th>ID</th>
          <th>Name</th>
          <th>Color</th>
          <th>Price</th>
          <th>Safety Stock</th>
        </tr>
      </thead>
      <tbody>
        <tr v-if="products.length === 0">
          <td colspan="5" class="empty-row">No products available</td>
        </tr>
        <tr
          v-else
          v-for="p in products"
          :key="p.productID"
          :class="{ low: p.safetyStockLevel !== undefined && p.safetyStockLevel < 10 }"
        >
          <td>{{ p.productID }}</td>
          <td>{{ p.name }}</td>
          <td>{{ p.color || '-' }}</td>
          <td>{{ formatPrice(p.listPrice) }}</td>
          <td>{{ p.safetyStockLevel !== undefined ? p.safetyStockLevel : '-' }}</td>
        </tr>
      </tbody>
      </table>
    </div>
    <p v-if="products.length > 0" class="record-count">Total: {{ products.length }}</p>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { productsService } from '../services/productsService'

const products = ref([])

function formatPrice(p) {
  return new Intl.NumberFormat('en-US', {
    style: 'currency',
    currency: 'USD'
  }).format(p)
}

async function loadProducts() {
  try {
    const all = await productsService.getAll()
    products.value = all
  } catch (err) {
    console.error('Failed to load products for low stock table', err)
  }
}

onMounted(loadProducts)
</script>

<style scoped>
.table-wrapper {
  background: white;
  border-radius: 8px;
  padding: 1rem;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  width: 100%; /* fill available column */
  /* allow the wrapper to grow but limit height for scrolling */
  max-height: 600px;
  overflow: hidden;
}
.table-container-scroll {
  overflow-y: auto;
  overflow-x: hidden; /* prevent horizontal scrolling */
  max-height: 400px;
}
.data-table {
  width: 100%;
  border-collapse: collapse;
}
.data-table th,
.data-table td {
  padding: 0.75rem 1rem;
  text-align: left;
  border-bottom: 1px solid #eee;
}
.data-table th {
  background: #2c3e50;
  color: white;
}
.data-table tbody tr:hover {
  background: #f5f5f5;
}
.low {
  background-color: #ffecec;
}
.empty-row {
  text-align: center;
  color: #999;
  font-style: italic;
  padding: 2rem !important;
}
.record-count {
  padding: 1rem;
  color: #666;
  text-align: right;
}
</style>
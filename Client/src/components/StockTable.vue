<template>
  <section class="table-card">
    <header class="table-header">
      <h2>Stock Management</h2>
      <label class="toggle">
        <input v-model="showOnlyLowStock" type="checkbox" />
        Show only low stock
      </label>
    </header>

    <div class="table-wrap">
      <table>
        <thead>
          <tr>
            <th>Product Name</th>
            <th>Color</th>
            <th>Quantity in Stock</th>
            <th>Price</th>
            <th>Inventory Value</th>
          </tr>
        </thead>
        <tbody>
          <tr v-if="filteredRows.length === 0">
            <td colspan="5" class="empty">No products match the current filter.</td>
          </tr>
          <tr
            v-for="row in filteredRows"
            :key="row.id"
            :class="{ low: row.stock <= lowStockThreshold }"
          >
            <td>{{ row.name }}</td>
            <td>{{ row.color }}</td>
            <td>
              {{ row.stock }}
              <span v-if="row.stock <= lowStockThreshold" class="chip">Low</span>
            </td>
            <td>{{ formatCurrency(row.price) }}</td>
            <td>{{ formatCurrency(row.inventoryValue) }}</td>
          </tr>
        </tbody>
      </table>
    </div>
    <p class="note">Stock quantity is estimated from available product data in the current API model.</p>
  </section>
</template>

<script setup>
import { computed, ref } from 'vue'

const props = defineProps({
  rows: { type: Array, default: () => [] },
  lowStockThreshold: { type: Number, default: 20 }
})

const showOnlyLowStock = ref(false)

const filteredRows = computed(() => {
  if (!showOnlyLowStock.value) return props.rows
  return props.rows.filter(row => row.stock <= props.lowStockThreshold)
})

function formatCurrency(value) {
  return new Intl.NumberFormat('en-US', {
    style: 'currency',
    currency: 'USD',
    maximumFractionDigits: 2
  }).format(value || 0)
}
</script>

<style scoped>
.table-card {
  background: var(--panel);
  border: 1px solid var(--line);
  border-radius: var(--radius);
  box-shadow: var(--shadow);
  padding: 1rem;
}

.table-header {
  display: flex;
  justify-content: space-between;
  gap: 1rem;
  align-items: center;
  flex-wrap: wrap;
}

.table-header h2 {
  margin: 0;
  font-size: 1.05rem;
}

.toggle {
  font-size: 0.92rem;
  color: var(--muted);
  display: flex;
  align-items: center;
  gap: 0.45rem;
}

.table-wrap {
  margin-top: 0.9rem;
  overflow: auto;
}

table {
  width: 100%;
  border-collapse: collapse;
  min-width: 720px;
}

th,
td {
  text-align: left;
  border-bottom: 1px solid var(--line);
  padding: 0.72rem 0.55rem;
}

th {
  font-size: 0.84rem;
  text-transform: uppercase;
  letter-spacing: 0.04em;
  color: var(--muted);
}

tr.low {
  background: var(--warn-soft);
}

.chip {
  display: inline-block;
  margin-left: 0.45rem;
  font-size: 0.72rem;
  border: 1px solid #d57d50;
  color: var(--warn);
  padding: 0.1rem 0.35rem;
  border-radius: 999px;
}

.empty {
  color: var(--muted);
  text-align: center;
}

.note {
  margin: 0.8rem 0 0;
  color: var(--muted);
  font-size: 0.83rem;
}
</style>

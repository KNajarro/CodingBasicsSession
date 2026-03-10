<template>
  <div class="table-card">
    <div class="table-header">
      <div>
        <h3 class="table-title">Stock Management</h3>
        <p class="table-sub">
          Products with SafetyStockLevel ≤ {{ threshold }} · Page {{ page }} of {{ totalPages }}
          ({{ totalCount }} total)
        </p>
      </div>
      <div class="threshold-control">
        <label for="threshold-input">Threshold</label>
        <input
          id="threshold-input"
          v-model.number="threshold"
          type="number"
          min="1"
          max="1000"
          @change="fetchPage(1)"
        />
      </div>
    </div>

    <div class="table-scroll">
      <table>
        <thead>
          <tr>
            <th>Product</th>
            <th>Number</th>
            <th>Category</th>
            <th>List Price</th>
            <th>Safety Stock</th>
            <th>Reorder Point</th>
          </tr>
        </thead>
        <tbody>
          <tr v-if="loading">
            <td colspan="6" class="state-cell">Loading…</td>
          </tr>
          <tr v-else-if="!items.length">
            <td colspan="6" class="state-cell">No products found</td>
          </tr>
          <template v-else>
            <tr
              v-for="item in items"
              :key="item.productID"
              :class="rowClass(item)"
            >
              <td class="cell-name">{{ item.name }}</td>
              <td class="cell-mono">{{ item.productNumber }}</td>
              <td>{{ item.categoryName ?? '—' }}</td>
              <td class="cell-mono">{{ formatCurrency(item.listPrice) }}</td>
              <td class="cell-stock">
                <span class="badge" :class="stockBadgeClass(item)">
                  {{ item.safetyStockLevel }}
                </span>
              </td>
              <td class="cell-mono">{{ item.reorderPoint }}</td>
            </tr>
          </template>
        </tbody>
      </table>
    </div>

    <div class="pagination">
      <button :disabled="page <= 1" @click="fetchPage(page - 1)">&#8592; Prev</button>
      <span class="page-info">{{ page }} / {{ totalPages }}</span>
      <button :disabled="page >= totalPages" @click="fetchPage(page + 1)">Next &#8594;</button>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue'
import { dashboardService } from '../../services/dashboardService'

const PAGE_SIZE = 20
const CRITICAL_THRESHOLD = 50

const items     = ref([])
const page      = ref(1)
const totalCount = ref(0)
const loading   = ref(false)
const threshold = ref(100)

const totalPages = computed(() => Math.max(1, Math.ceil(totalCount.value / PAGE_SIZE)))

async function fetchPage (p) {
  page.value = p
  loading.value = true
  try {
    const res = await dashboardService.getLowStock(p, threshold.value)
    items.value      = res.data.items
    totalCount.value = res.data.totalCount
  } finally {
    loading.value = false
  }
}

const formatCurrency = v =>
  new Intl.NumberFormat('en-US', { style: 'currency', currency: 'USD' }).format(v)

const rowClass = item =>
  item.safetyStockLevel <= CRITICAL_THRESHOLD ? 'row-critical' : ''

const stockBadgeClass = item =>
  item.safetyStockLevel <= CRITICAL_THRESHOLD ? 'badge-critical' : 'badge-warn'

onMounted(() => fetchPage(1))
</script>

<style scoped>
.table-card {
  background: #111827;
  border: 1px solid rgba(255,255,255,0.07);
  border-radius: 16px;
  overflow: hidden;
  box-shadow: 0 4px 24px rgba(0,0,0,0.25);
}

.table-header {
  display: flex;
  align-items: flex-start;
  justify-content: space-between;
  gap: 1rem;
  padding: 1.5rem 1.5rem 1rem;
}

.table-title {
  margin: 0 0 0.2rem;
  font-size: 1rem;
  font-weight: 700;
  color: #f1f5f9;
}

.table-sub {
  margin: 0;
  font-size: 0.72rem;
  color: rgba(255,255,255,0.35);
}

.threshold-control {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  flex-shrink: 0;
}

.threshold-control label {
  font-size: 0.75rem;
  color: rgba(255,255,255,0.5);
}

.threshold-control input {
  width: 72px;
  background: rgba(255,255,255,0.06);
  border: 1px solid rgba(255,255,255,0.12);
  border-radius: 8px;
  color: #f1f5f9;
  font-size: 0.85rem;
  padding: 0.3rem 0.5rem;
  text-align: center;
}

.threshold-control input:focus {
  outline: none;
  border-color: rgba(99,210,255,0.5);
}

.table-scroll {
  overflow-x: auto;
}

table {
  width: 100%;
  border-collapse: collapse;
  font-size: 0.82rem;
}

thead tr {
  border-bottom: 1px solid rgba(255,255,255,0.08);
}

th {
  padding: 0.65rem 1.25rem;
  text-align: left;
  font-size: 0.7rem;
  font-weight: 600;
  letter-spacing: 0.08em;
  text-transform: uppercase;
  color: rgba(255,255,255,0.4);
  white-space: nowrap;
}

td {
  padding: 0.7rem 1.25rem;
  border-bottom: 1px solid rgba(255,255,255,0.04);
  color: rgba(255,255,255,0.75);
}

.state-cell {
  text-align: center;
  color: rgba(255,255,255,0.25);
  padding: 2rem;
}

.cell-name {
  font-weight: 500;
  color: #f1f5f9;
}

.cell-mono {
  font-family: 'Fira Code', monospace;
  font-size: 0.8rem;
}

.cell-stock {
  text-align: center;
}

/* Critical row highlight */
.row-critical td {
  background: rgba(248, 113, 113, 0.06);
}

.badge {
  display: inline-block;
  padding: 0.15rem 0.6rem;
  border-radius: 999px;
  font-size: 0.75rem;
  font-weight: 600;
  font-family: 'Fira Code', monospace;
}

.badge-warn {
  background: rgba(245, 158, 11, 0.15);
  color: #f59e0b;
}

.badge-critical {
  background: rgba(248, 113, 113, 0.18);
  color: #f87171;
}

.pagination {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 1rem;
  padding: 1rem;
  border-top: 1px solid rgba(255,255,255,0.06);
}

.pagination button {
  background: rgba(255,255,255,0.06);
  border: 1px solid rgba(255,255,255,0.1);
  border-radius: 8px;
  color: rgba(255,255,255,0.7);
  padding: 0.35rem 0.9rem;
  font-size: 0.8rem;
  cursor: pointer;
  transition: background 0.15s;
}

.pagination button:hover:not(:disabled) {
  background: rgba(99,210,255,0.12);
  color: #63d2ff;
  border-color: rgba(99,210,255,0.3);
}

.pagination button:disabled {
  opacity: 0.3;
  cursor: not-allowed;
}

.page-info {
  font-size: 0.8rem;
  color: rgba(255,255,255,0.4);
  min-width: 60px;
  text-align: center;
}
</style>

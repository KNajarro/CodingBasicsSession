<template>
  <div class="dashboard">
    <h2 class="dashboard-title">📊 Inventory Dashboard</h2>

    <div v-if="loading" class="loading">Loading dashboard data…</div>
    <div v-if="error" class="error-message">{{ error }}</div>

    <template v-if="!loading && !error">

      <!-- ── KPI Cards ── -->
      <div class="kpi-row">
        <div class="kpi-card">
          <span class="kpi-label">Total Inventory Value</span>
          <span class="kpi-value">{{ formatCurrency(totalInventoryValue) }}</span>
          <span class="kpi-sub">Sum of all List Prices</span>
        </div>
        <div class="kpi-card">
          <span class="kpi-label">Total Products</span>
          <span class="kpi-value">{{ products.length }}</span>
          <span class="kpi-sub">Across all categories</span>
        </div>
        <div class="kpi-card kpi-warning">
          <span class="kpi-label">⚠ Low Stock Products</span>
          <span class="kpi-value">{{ lowStockCount }}</span>
          <span class="kpi-sub">SafetyStockLevel &lt; {{ STOCK_THRESHOLD }}</span>
        </div>
        <div class="kpi-card">
          <span class="kpi-label">Total People</span>
          <span class="kpi-value">{{ people.length }}</span>
          <span class="kpi-sub">All person types</span>
        </div>
      </div>

      <!-- ── Charts Row ── -->
      <div class="charts-row">
        <div class="chart-card">
          <h3>Product Distribution by Color</h3>
          <div class="chart-container">
            <Doughnut :data="colorChartData" :options="doughnutOptions" />
          </div>
        </div>
        <div class="chart-card">
          <h3>Customer Analysis by Person Type</h3>
          <div class="chart-container">
            <Bar :data="personTypeChartData" :options="barOptions" />
          </div>
        </div>
      </div>

      <!-- ── Stock Management Table ── -->
      <div class="section-card">
        <div class="section-header">
          <h3>Stock Management</h3>
          <div class="search-group">
            <input
              v-model="stockSearch"
              type="text"
              placeholder="Filter by product name…"
              class="search-input"
            />
            <span class="legend-badge low-stock-badge">
              ⚠ Low Stock (&lt; {{ STOCK_THRESHOLD }})
            </span>
          </div>
        </div>
        <DataTable
          :columns="stockColumns"
          :items="filteredProducts"
          :rowClass="stockRowClass"
          empty-message="No products match your search."
        >
          <template #listPrice="{ value }">
            {{ formatCurrency(value) }}
          </template>
          <template #safetyStockLevel="{ value, item }">
            <span :class="{ 'low-stock-text': item.safetyStockLevel < STOCK_THRESHOLD }">
              {{ value }}
            </span>
          </template>
          <template #color="{ value }">{{ value || '—' }}</template>
          <template #categoryName="{ value }">{{ value || '—' }}</template>
        </DataTable>
      </div>

    </template>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue'
import { Doughnut, Bar } from 'vue-chartjs'
import {
  Chart as ChartJS,
  ArcElement,
  Tooltip,
  Legend,
  CategoryScale,
  LinearScale,
  BarElement,
  Title
} from 'chart.js'
import productsService from '../services/productsService'
import peopleService   from '../services/peopleService'
import DataTable       from '../components/DataTable.vue'

ChartJS.register(ArcElement, Tooltip, Legend, CategoryScale, LinearScale, BarElement, Title)

// ── Constants ──────────────────────────────────────────────────────────────
const STOCK_THRESHOLD = 500

// ── State ──────────────────────────────────────────────────────────────────
const products   = ref([])
const people     = ref([])
const loading    = ref(true)
const error      = ref(null)
const stockSearch = ref('')

// ── Fetch on mount ─────────────────────────────────────────────────────────
onMounted(async () => {
  try {
    // search() with no args → GET /api/products/search and /api/people/search
    // Both return a flat (non-paginated) array of all records
    const [prods, pers] = await Promise.all([
      productsService.search(),
      peopleService.search()
    ])
    products.value = prods
    people.value   = pers
  } catch (e) {
    error.value = 'Failed to load dashboard data. Make sure the API is running.'
    console.error(e)
  } finally {
    loading.value = false
  }
})

// ── KPI computeds ──────────────────────────────────────────────────────────
const totalInventoryValue = computed(() =>
  products.value.reduce((sum, p) => sum + (p.listPrice ?? 0), 0)
)

const lowStockCount = computed(() =>
  products.value.filter(p => p.safetyStockLevel < STOCK_THRESHOLD).length
)

const formatCurrency = (val) =>
  new Intl.NumberFormat('en-US', { style: 'currency', currency: 'USD' }).format(val)

// ── Shared color palette ───────────────────────────────────────────────────
const PALETTE = [
  '#3498db', '#e74c3c', '#2ecc71', '#f39c12', '#9b59b6',
  '#1abc9c', '#e67e22', '#34495e', '#e91e63', '#00bcd4',
  '#8bc34a', '#ff5722'
]

// ── Doughnut: products by color ────────────────────────────────────────────
const colorChartData = computed(() => {
  const counts = {}
  for (const p of products.value) {
    const key = p.color || 'N/A'
    counts[key] = (counts[key] ?? 0) + 1
  }
  const labels = Object.keys(counts).sort((a, b) => counts[b] - counts[a])
  return {
    labels,
    datasets: [{
      data: labels.map(l => counts[l]),
      backgroundColor: labels.map((_, i) => PALETTE[i % PALETTE.length]),
      borderWidth: 2,
      borderColor: '#fff'
    }]
  }
})

const doughnutOptions = {
  responsive: true,
  maintainAspectRatio: false,
  plugins: {
    legend: { position: 'right', labels: { font: { size: 12 }, padding: 16 } }
  }
}

// ── Bar chart: people by PersonType ───────────────────────────────────────
const PERSON_TYPE_LABELS = {
  SC: 'Store Contact',
  IN: 'Individual',
  SP: 'Sales Person',
  EM: 'Employee',
  GC: 'General Contact',
  VC: 'Vendor Contact'
}

const personTypeChartData = computed(() => {
  const counts = {}
  for (const p of people.value) {
    const type = p.personType || 'Unknown'
    counts[type] = (counts[type] ?? 0) + 1
  }
  const labels = Object.keys(counts)
  return {
    labels: labels.map(l => PERSON_TYPE_LABELS[l] ?? l),
    datasets: [{
      label: 'Count',
      data: labels.map(l => counts[l]),
      backgroundColor: labels.map((_, i) => PALETTE[i % PALETTE.length]),
      borderColor:     labels.map((_, i) => PALETTE[i % PALETTE.length]),
      borderWidth: 1,
      borderRadius: 4
    }]
  }
})

const barOptions = {
  responsive: true,
  maintainAspectRatio: false,
  plugins: { legend: { display: false } },
  scales: {
    y: { beginAtZero: true, grid: { color: '#f0f0f0' } },
    x: { grid: { display: false } }
  }
}

// ── Stock table ────────────────────────────────────────────────────────────
const stockColumns = [
  { key: 'name',             label: 'Product Name' },
  { key: 'productNumber',    label: 'Number'       },
  { key: 'color',            label: 'Color'        },
  { key: 'listPrice',        label: 'List Price'   },
  { key: 'safetyStockLevel', label: 'Safety Stock' },
  { key: 'categoryName',     label: 'Category'     }
]

const filteredProducts = computed(() => {
  const q = stockSearch.value.trim().toLowerCase()
  if (!q) return products.value
  return products.value.filter(p => p.name?.toLowerCase().includes(q))
})

const stockRowClass = (item) =>
  item.safetyStockLevel < STOCK_THRESHOLD ? 'row-low-stock' : ''
</script>

<style scoped>
/* ── Page layout ── */
.dashboard        { padding: 0; }
.dashboard-title  { color: #2c3e50; margin-bottom: 1.5rem; font-size: 1.75rem; }

/* ── KPI cards ── */
.kpi-row {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(210px, 1fr));
  gap: 1rem;
  margin-bottom: 1.5rem;
}

.kpi-card {
  background: white;
  border-radius: 8px;
  box-shadow: 0 2px 8px rgba(0,0,0,.08);
  padding: 1.25rem 1.5rem;
  display: flex;
  flex-direction: column;
  gap: .3rem;
  border-left: 4px solid #3498db;
}
.kpi-card.kpi-warning   { border-left-color: #e67e22; }
.kpi-label {
  font-size: .75rem;
  font-weight: 600;
  text-transform: uppercase;
  letter-spacing: .06em;
  color: #888;
}
.kpi-value {
  font-size: 2rem;
  font-weight: 700;
  color: #2c3e50;
  line-height: 1.1;
}
.kpi-sub { font-size: .75rem; color: #aaa; }

/* ── Charts ── */
.charts-row {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 1rem;
  margin-bottom: 1.5rem;
}

.chart-card {
  background: white;
  border-radius: 8px;
  box-shadow: 0 2px 8px rgba(0,0,0,.08);
  padding: 1.25rem 1.5rem;
}
.chart-card h3     { margin: 0 0 1rem; font-size: 1rem; color: #2c3e50; }
.chart-container   { height: 280px; position: relative; }

/* ── Section card (stock table) ── */
.section-card {
  background: white;
  border-radius: 8px;
  box-shadow: 0 2px 8px rgba(0,0,0,.08);
  padding: 1.25rem 1.5rem;
  margin-bottom: 1.5rem;
}
.section-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  flex-wrap: wrap;
  gap: .75rem;
  margin-bottom: 1rem;
}
.section-header h3 { margin: 0; font-size: 1rem; color: #2c3e50; }

.search-group {
  display: flex;
  align-items: center;
  gap: .75rem;
  flex-wrap: wrap;
}
.search-input {
  padding: .45rem .75rem;
  border: 1px solid #ddd;
  border-radius: 4px;
  font-size: .9rem;
  width: 240px;
  outline: none;
  transition: border-color .2s;
}
.search-input:focus { border-color: #3498db; }

.legend-badge {
  font-size: .75rem;
  padding: .25rem .65rem;
  border-radius: 12px;
  font-weight: 500;
}
.low-stock-badge { background: #fff3e0; color: #e67e22; border: 1px solid #f0c070; }

/* ── Row highlighting (penetrates DataTable scoped styles) ── */
:deep(.row-low-stock)        { background-color: #fff8e1 !important; }
:deep(.row-low-stock:hover)  { background-color: #ffefc0 !important; }
.low-stock-text              { color: #e67e22; font-weight: 700; }

/* ── Loading / Error ── */
.loading {
  text-align: center;
  padding: 3rem;
  color: #666;
  font-size: 1.1rem;
}
.error-message {
  background: #fde8e8;
  border: 1px solid #f5c6c6;
  color: #c0392b;
  border-radius: 6px;
  padding: .75rem 1rem;
  margin-bottom: 1rem;
}

/* ── Responsive ── */
@media (max-width: 768px) {
  .charts-row { grid-template-columns: 1fr; }
  .kpi-row    { grid-template-columns: 1fr 1fr; }
  .search-input { width: 100%; }
}
</style>

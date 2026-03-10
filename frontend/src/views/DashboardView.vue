<template>
  <div class="dashboard">
    <h2>Dashboard</h2>

    <div v-if="loading" class="loading">Loading dashboard data…</div>

    <div v-else>
      <!-- KPI Card -->
      <div class="kpi-card">
        <h3>Total List Price</h3>
        <p class="kpi-value">{{ totalListPrice }}</p>
      </div>

      <!-- Charts row -->
      <div class="charts-row">
        <div class="chart-container">
          <h3>Products by Color</h3>
          <canvas ref="colorChart"></canvas>
          <p class="chart-total">Total Products: {{ products.length }}</p>
        </div>
        <div class="chart-container">
          <h3>People by Person Type</h3>
          <canvas ref="personTypeChart"></canvas>
          <p class="chart-total">Total People: {{ people.length }}</p>
        </div>
      </div>

      <!-- Stock Management -->
      <div class="low-stock-section">
        <div class="section-header">
          <h3>Stock Management</h3>
          <span class="badge">{{ filteredLowStock.length }} products</span>
        </div>

        <!-- Filters row -->
        <div class="filters-row">
          <div class="filter-group">
            <label for="threshold">Threshold</label>
            <input id="threshold" v-model.number="stockThreshold" type="number" min="0" class="threshold-input" />
          </div>
          <div class="filter-group">
            <label for="fname">Name</label>
            <input id="fname" v-model="filterName" type="text" placeholder="Search…" class="filter-input" />
          </div>
          <div class="filter-group">
            <label for="fcat">Category</label>
            <select id="fcat" v-model="filterCategory" class="filter-select">
              <option value="">All</option>
              <option v-for="c in categoryOptions" :key="c" :value="c">{{ c }}</option>
            </select>
          </div>
          <div class="filter-group">
            <label for="fsub">Subcategory</label>
            <select id="fsub" v-model="filterSubcategory" class="filter-select">
              <option value="">All</option>
              <option v-for="s in subcategoryOptions" :key="s" :value="s">{{ s }}</option>
            </select>
          </div>
          <div class="filter-group">
            <label for="fcol">Color</label>
            <select id="fcol" v-model="filterColor" class="filter-select">
              <option value="">All</option>
              <option v-for="c in colorOptions" :key="c" :value="c">{{ c }}</option>
            </select>
          </div>
          <div class="filter-group">
            <label for="fsize">Size</label>
            <select id="fsize" v-model="filterSize" class="filter-select">
              <option value="">All</option>
              <option v-for="s in sizeOptions" :key="s" :value="s">{{ s }}</option>
            </select>
          </div>
          <div class="filter-group filter-group--action">
            <button class="btn-clear" @click="clearFilters">Clear Filters</button>
          </div>
        </div>

        <div class="table-wrapper">
          <table class="low-stock-table">
            <thead>
              <tr>
                <th>Name</th>
                <th>Category</th>
                <th>Subcategory</th>
                <th>Color</th>
                <th>Size</th>
                <th>Standard Cost</th>
                <th>List Price</th>
                <th>Safety Stock</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="(p, i) in filteredLowStock" :key="p.productID" :class="{ 'row-even': i % 2 === 0, 'row-odd': i % 2 !== 0 }">
                <td class="col-name">{{ p.name }}</td>
                <td>{{ p.categoryName || '—' }}</td>
                <td>{{ p.subcategoryName || '—' }}</td>
                <td>
                  <span class="color-dot" :style="{ backgroundColor: getColorHex(p.color) }" v-if="p.color"></span>
                  {{ p.color || '—' }}
                </td>
                <td>{{ p.size || '—' }}</td>
                <td class="col-money">{{ formatCurrency(p.standardCost) }}</td>
                <td class="col-money">{{ formatCurrency(p.listPrice) }}</td>
                <td class="col-stock">
                  <span class="stock-badge" :class="stockLevel(p.safetyStockLevel)">{{ p.safetyStockLevel }}</span>
                </td>
              </tr>
              <tr v-if="filteredLowStock.length === 0">
                <td colspan="8" class="empty-row">No products match the current filters</td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { Chart, DoughnutController, ArcElement, BarController, BarElement, CategoryScale, LinearScale, Tooltip, Legend } from 'chart.js'
import productsService from '../services/productsService'
import peopleService from '../services/peopleService'

Chart.register(DoughnutController, ArcElement, BarController, BarElement, CategoryScale, LinearScale, Tooltip, Legend)

const PERSON_TYPE_LABELS = {
  SC: 'Store Contact',
  IN: 'Individual',
  SP: 'Sales Person',
  EM: 'Employee',
  VC: 'Vendor Contact',
  GC: 'General Contact'
}

export default {
  name: 'DashboardView',
  data() {
    return {
      products: [],
      people: [],
      loading: true,
      stockThreshold: 500,
      filterName: '',
      filterCategory: '',
      filterSubcategory: '',
      filterColor: '',
      filterSize: '',
      colorChartInstance: null,
      personTypeChartInstance: null
    }
  },
  computed: {
    totalListPrice() {
      const sum = this.products.reduce((acc, p) => acc + p.listPrice, 0)
      return sum.toLocaleString('en-US', { style: 'currency', currency: 'USD' })
    },
    lowStockProducts() {
      return this.products.filter(p => p.safetyStockLevel < this.stockThreshold)
    },
    filteredLowStock() {
      return this.lowStockProducts.filter(p => {
        if (this.filterName && !p.name.toLowerCase().includes(this.filterName.toLowerCase())) return false
        if (this.filterCategory && (p.categoryName || '') !== this.filterCategory) return false
        if (this.filterSubcategory && (p.subcategoryName || '') !== this.filterSubcategory) return false
        if (this.filterColor && (p.color || '') !== this.filterColor) return false
        if (this.filterSize && (p.size || '') !== this.filterSize) return false
        return true
      })
    },
    categoryOptions() {
      const set = new Set(this.lowStockProducts.map(p => p.categoryName).filter(Boolean))
      return [...set].sort()
    },
    subcategoryOptions() {
      let items = this.lowStockProducts
      if (this.filterCategory) items = items.filter(p => p.categoryName === this.filterCategory)
      const set = new Set(items.map(p => p.subcategoryName).filter(Boolean))
      return [...set].sort()
    },
    colorOptions() {
      const set = new Set(this.lowStockProducts.map(p => p.color).filter(Boolean))
      return [...set].sort()
    },
    sizeOptions() {
      let items = this.lowStockProducts
      if (this.filterCategory) items = items.filter(p => p.categoryName === this.filterCategory)
      if (this.filterSubcategory) items = items.filter(p => p.subcategoryName === this.filterSubcategory)
      const set = new Set(items.map(p => p.size).filter(Boolean))
      return [...set].sort()
    }
  },
  async mounted() {
    try {
      const [products, people] = await Promise.all([
        productsService.getAllForDashboard(),
        peopleService.getAllForDashboard()
      ])
      this.products = products
      this.people = people
    } catch (err) {
      console.error('Failed to load dashboard data:', err)
    } finally {
      this.loading = false
    }
    this.$nextTick(() => {
      this.renderColorChart()
      this.renderPersonTypeChart()
    })
  },
  beforeUnmount() {
    if (this.colorChartInstance) this.colorChartInstance.destroy()
    if (this.personTypeChartInstance) this.personTypeChartInstance.destroy()
  },
  methods: {
    formatCurrency(value) {
      return (value ?? 0).toLocaleString('en-US', { style: 'currency', currency: 'USD' })
    },
    clearFilters() {
      this.filterName = ''
      this.filterCategory = ''
      this.filterSubcategory = ''
      this.filterColor = ''
      this.filterSize = ''
    },
    stockLevel(val) {
      if (val <= 10) return 'stock-critical'
      if (val <= 100) return 'stock-low'
      return 'stock-medium'
    },
    getColorHex(color) {
      const map = {
        Black: '#333', Red: '#e74c3c', Blue: '#3498db', Silver: '#bdc3c7',
        White: '#ecf0f1', Yellow: '#f1c40f', Multi: 'linear-gradient(90deg,#e74c3c,#3498db,#2ecc71)',
        Grey: '#95a5a6'
      }
      return map[color] || '#999'
    },
    renderColorChart() {
      const counts = {}
      this.products.forEach(p => {
        const color = p.color || 'No Color'
        counts[color] = (counts[color] || 0) + 1
      })
      const labels = Object.keys(counts)
      const data = Object.values(counts)

      const ctx = this.$refs.colorChart
      if (!ctx) return

      const totalProducts = this.products.length

      this.colorChartInstance = new Chart(ctx, {
        type: 'doughnut',
        data: {
          labels,
          datasets: [{
            data,
            backgroundColor: labels.map((_, i) =>
              `hsl(${(i * 360) / labels.length}, 70%, 55%)`
            ),
            borderWidth: 2,
            borderColor: '#fff'
          }]
        },
        options: {
          responsive: true,
          plugins: {
            legend: { position: 'right' },
            tooltip: {
              callbacks: {
                label(ctx) {
                  const val = ctx.parsed
                  const pct = ((val / totalProducts) * 100).toFixed(1)
                  return ` ${ctx.label}: ${val} (${pct}%)`
                }
              }
            }
          }
        }
      })
    },
    renderPersonTypeChart() {
      const counts = {}
      this.people.forEach(p => {
        const code = p.personType
        counts[code] = (counts[code] || 0) + 1
      })
      const codes = Object.keys(counts)
      const labels = codes.map(c => PERSON_TYPE_LABELS[c] || c)
      const data = codes.map(c => counts[c])

      const ctx = this.$refs.personTypeChart
      if (!ctx) return

      const barColors = [
        '#e74c3c', '#3498db', '#2ecc71', '#f39c12', '#9b59b6', '#1abc9c',
        '#e67e22', '#34495e'
      ]

      this.personTypeChartInstance = new Chart(ctx, {
        type: 'bar',
        data: {
          labels,
          datasets: [{
            label: 'Count',
            data,
            backgroundColor: codes.map((_, i) => barColors[i % barColors.length]),
            borderRadius: 4
          }]
        },
        options: {
          responsive: true,
          plugins: {
            legend: { display: false },
            tooltip: {
              callbacks: {
                label(ctx) {
                  return ` ${ctx.parsed.y.toLocaleString()} people`
                }
              }
            }
          },
          scales: {
            y: { beginAtZero: true, ticks: { precision: 0 } },
            x: { ticks: { maxRotation: 45, minRotation: 25 } }
          }
        }
      })
    }
  }
}
</script>

<style scoped>
.dashboard { padding: 1rem 0; }
.loading { text-align: center; padding: 2rem; font-size: 1.2rem; color: #666; }

.kpi-card {
  background: #2c3e50;
  color: white;
  border-radius: 8px;
  padding: 1.5rem 2rem;
  text-align: center;
  margin-bottom: 2rem;
}
.kpi-card h3 { margin: 0 0 0.5rem; font-size: 1rem; opacity: 0.8; }
.kpi-value { font-size: 2.2rem; font-weight: bold; margin: 0; }

.charts-row {
  display: flex;
  gap: 2rem;
  margin-bottom: 2rem;
}
.chart-container {
  flex: 1;
  background: #fff;
  border: 1px solid #ddd;
  border-radius: 8px;
  padding: 1rem;
}
.chart-container h3 { margin-top: 0; }
.chart-total {
  text-align: center;
  font-size: 0.9rem;
  color: #666;
  margin: 0.5rem 0 0;
  font-weight: 600;
}

.low-stock-section {
  margin-top: 1rem;
  background: #fff;
  border: 1px solid #e0e0e0;
  border-radius: 8px;
  padding: 1.5rem;
}
.section-header {
  display: flex;
  align-items: center;
  gap: 0.75rem;
  margin-bottom: 1rem;
}
.section-header h3 { margin: 0; }
.badge {
  background: #2c3e50;
  color: #fff;
  font-size: 0.75rem;
  padding: 0.2rem 0.6rem;
  border-radius: 12px;
}

.filters-row {
  display: flex;
  flex-wrap: wrap;
  gap: 0.75rem;
  align-items: flex-end;
  margin-bottom: 1rem;
  padding: 1rem;
  background: #f8f9fa;
  border-radius: 6px;
}
.filter-group {
  display: flex;
  flex-direction: column;
  gap: 0.2rem;
}
.filter-group label {
  font-size: 0.75rem;
  font-weight: 600;
  color: #666;
  text-transform: uppercase;
  letter-spacing: 0.03em;
}
.filter-group--action {
  justify-content: flex-end;
}
.btn-clear {
  padding: 0.4rem 0.8rem;
  border: 1px solid #ccc;
  border-radius: 4px;
  background: #fff;
  cursor: pointer;
  font-size: 0.85rem;
  color: #555;
  transition: all 0.2s;
}
.btn-clear:hover {
  background: #e74c3c;
  color: #fff;
  border-color: #e74c3c;
}
.filter-input, .filter-select {
  margin-top: 0;
  padding: 0.4rem 0.5rem;
  border: 1px solid #ddd;
  border-radius: 4px;
  font-size: 0.85rem;
  background: #fff;
}
.filter-input { width: 160px; }
.filter-select { min-width: 130px; }
.threshold-input {
  width: 70px;
  padding: 0.4rem 0.5rem;
  border: 1px solid #ddd;
  border-radius: 4px;
  font-size: 0.85rem;
}

.table-wrapper {
  overflow-x: auto;
  border-radius: 6px;
  border: 1px solid #e0e0e0;
}

.low-stock-table {
  width: 100%;
  border-collapse: collapse;
}
.low-stock-table th, .low-stock-table td {
  padding: 0.6rem 0.8rem;
  text-align: left;
  border-bottom: 1px solid #eee;
}
.low-stock-table th {
  background: #2c3e50;
  color: white;
  font-size: 0.8rem;
  text-transform: uppercase;
  letter-spacing: 0.04em;
}
.row-even { background: #fff; }
.row-odd { background: #f8f9fa; }
.row-even:hover, .row-odd:hover { background: #edf2f7; }
.col-name { font-weight: 500; }
.col-money { font-family: 'Courier New', monospace; }
.col-stock { text-align: center; }
.color-dot {
  display: inline-block;
  width: 10px;
  height: 10px;
  border-radius: 50%;
  margin-right: 0.3rem;
  vertical-align: middle;
  border: 1px solid #ccc;
}
.stock-badge {
  display: inline-block;
  padding: 0.15rem 0.5rem;
  border-radius: 10px;
  font-size: 0.8rem;
  font-weight: 600;
}
.stock-critical { background: #fde8e8; color: #c0392b; }
.stock-low { background: #fef3cd; color: #856404; }
.stock-medium { background: #e8f5e9; color: #2e7d32; }
.empty-row {
  text-align: center;
  padding: 2rem;
  color: #999;
}
</style>

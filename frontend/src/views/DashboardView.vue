<template>
  <div class="dashboard-view">
    <h2>Dashboard</h2>

    <div v-if="error" class="error-message">{{ error }}</div>
    <div v-if="loading" class="loading">Loading dashboard...</div>

    <template v-else>
      <div class="kpi-grid">
        <div class="kpi-card">
          <h3>Inventory Value</h3>
          <p class="kpi-value">{{ formatPrice(inventoryValue) }}</p>
        </div>

        <div class="kpi-card">
          <h3>Total Products</h3>
          <p class="kpi-value">{{ products.length }}</p>
        </div>

        <div class="kpi-card">
          <h3>Total People</h3>
          <p class="kpi-value">{{ people.length }}</p>
        </div>

        <div class="kpi-card alert-card">
          <h3>Low Stock Products</h3>
          <p class="kpi-value">{{ lowStockProducts.length }}</p>
        </div>
      </div>

      <div class="charts-grid">
        <div class="chart-card">
          <h3>Distribution by Color</h3>
          <div class="chart-wrapper">
            <canvas ref="colorChart"></canvas>
          </div>
        </div>

        <div class="chart-card">
          <h3>Customer Analysis by Person Type</h3>
          <div class="chart-wrapper">
            <canvas ref="personTypeChart"></canvas>
          </div>
        </div>
      </div>

      <div class="table-card">
        <div class="table-header">
          <h3>Stock Management</h3>
          <label class="checkbox-label">
            <input type="checkbox" v-model="showOnlyLowStock" />
            Show only low stock
          </label>
        </div>

        <div class="table-container">
          <table class="data-table">
            <thead>
              <tr>
                <th>ID</th>
                <th>Name</th>
                <th>Product Number</th>
                <th>Category</th>
                <th>Subcategory</th>
                <th>Safety Stock</th>
                <th>Reorder Point</th>
                <th>Status</th>
              </tr>
            </thead>
            <tbody>
              <tr
                v-for="product in visibleStockProducts"
                :key="product.productID"
                :class="{ 'low-stock-row': isLowStock(product) }"
              >
                <td>{{ product.productID }}</td>
                <td>{{ product.name }}</td>
                <td>{{ product.productNumber }}</td>
                <td>{{ product.categoryName || "-" }}</td>
                <td>{{ product.subcategoryName || "-" }}</td>
                <td>{{ product.safetyStockLevel }}</td>
                <td>{{ product.reorderPoint }}</td>
                <td>
                  <span
                    class="status-badge"
                    :class="isLowStock(product) ? 'status-low' : 'status-ok'"
                  >
                    {{ isLowStock(product) ? "Low Stock" : "OK" }}
                  </span>
                </td>
              </tr>
            </tbody>
          </table>
        </div>

        <div class="pagination" v-if="stockTotalPages > 1">
          <button
            class="btn btn-secondary btn-sm"
            @click="goToPrevStockPage"
            :disabled="stockCurrentPage === 1"
          >
            Prev
          </button>

          <span class="page-info">
            Page {{ stockCurrentPage }} of {{ stockTotalPages }}
          </span>

          <button
            class="btn btn-secondary btn-sm"
            @click="goToNextStockPage"
            :disabled="stockCurrentPage === stockTotalPages"
          >
            Next
          </button>
        </div>

        <p class="record-count">
          Showing {{ visibleStockProducts.length }} of
          {{ filteredStockProducts.length }}
        </p>
      </div>
    </template>
  </div>
</template>

<script>
import { Chart, registerables } from "chart.js";
import productsService from "../services/productsService";
import peopleService from "../services/peopleService";

Chart.register(...registerables);

export default {
  name: "DashboardView",
  data() {
    return {
      loading: false,
      error: null,
      products: [],
      people: [],
      showOnlyLowStock: false,
      colorChartInstance: null,
      personTypeChartInstance: null,
      stockCurrentPage: 1,
      stockPageSize: 30,
    };
  },
  computed: {
    inventoryValue() {
      return this.products.reduce(
        (sum, product) => sum + Number(product.listPrice || 0),
        0,
      );
    },
    lowStockProducts() {
      return this.products.filter((product) => this.isLowStock(product));
    },
    filteredStockProducts() {
      return this.showOnlyLowStock ? this.lowStockProducts : this.products;
    },
    stockTotalPages() {
      return Math.max(
        1,
        Math.ceil(this.filteredStockProducts.length / this.stockPageSize),
      );
    },
    visibleStockProducts() {
      const start = (this.stockCurrentPage - 1) * this.stockPageSize;
      const end = start + this.stockPageSize;
      return this.filteredStockProducts.slice(start, end);
    },
  },
  watch: {
    showOnlyLowStock() {
      this.stockCurrentPage = 1;
    },
  },
  async mounted() {
    await this.loadDashboard();
  },
  beforeUnmount() {
    if (this.colorChartInstance) this.colorChartInstance.destroy();
    if (this.personTypeChartInstance) this.personTypeChartInstance.destroy();
  },
  methods: {
    async loadDashboard() {
      this.loading = true;
      this.error = null;

      try {
        const [products, peopleResponse] = await Promise.all([
          productsService.getAll(),
          peopleService.getAll(1, 1000),
        ]);

        this.products = products || [];
        this.people = peopleResponse?.items || [];
        this.stockCurrentPage = 1;

        await this.$nextTick();

        setTimeout(() => {
          this.renderColorChart();
          this.renderPersonTypeChart();
        }, 0);
      } catch (err) {
        this.error =
          err?.response?.data?.message ||
          err?.message ||
          "Failed to load dashboard data.";
      } finally {
        this.loading = false;
      }
    },

    formatPrice(value) {
      return new Intl.NumberFormat("en-US", {
        style: "currency",
        currency: "USD",
      }).format(value || 0);
    },

    isLowStock(product) {
      return (
        Number(product.reorderPoint || 0) >=
        Number(product.safetyStockLevel || 0)
      );
    },

    buildColorDistribution() {
      const counts = {};

      for (const product of this.products) {
        const color =
          product.color && product.color.trim()
            ? product.color.trim()
            : "No Color";
        counts[color] = (counts[color] || 0) + 1;
      }

      return {
        labels: Object.keys(counts),
        values: Object.values(counts),
      };
    },

    buildPersonTypeDistribution() {
      const counts = {};

      for (const person of this.people) {
        const type =
          person.personType && person.personType.trim()
            ? person.personType.trim()
            : "Unknown";
        counts[type] = (counts[type] || 0) + 1;
      }

      return {
        labels: Object.keys(counts),
        values: Object.values(counts),
      };
    },

    renderColorChart() {
      if (this.colorChartInstance) {
        this.colorChartInstance.destroy();
      }

      const canvas = this.$refs.colorChart;
      if (!canvas) return;

      const distribution = this.buildColorDistribution();

      this.colorChartInstance = new Chart(canvas.getContext("2d"), {
        type: "doughnut",
        data: {
          labels: distribution.labels,
          datasets: [
            {
              data: distribution.values,
              backgroundColor: [
                "#3498db",
                "#9b59b6",
                "#e74c3c",
                "#f39c12",
                "#2ecc71",
                "#1abc9c",
                "#34495e",
                "#e67e22",
                "#16a085",
                "#c0392b",
                "#7f8c8d",
                "#8e44ad",
              ],
              borderColor: "#ffffff",
              borderWidth: 2,
            },
          ],
        },
        options: {
          responsive: true,
          maintainAspectRatio: false,
          plugins: {
            legend: {
              position: "bottom",
            },
          },
        },
      });
    },

    renderPersonTypeChart() {
      if (this.personTypeChartInstance) {
        this.personTypeChartInstance.destroy();
      }

      const canvas = this.$refs.personTypeChart;
      if (!canvas) return;

      const distribution = this.buildPersonTypeDistribution();

      this.personTypeChartInstance = new Chart(canvas.getContext("2d"), {
        type: "bar",
        data: {
          labels: distribution.labels,
          datasets: [
            {
              label: "People Count",
              data: distribution.values,
              backgroundColor: [
                "#3498db",
                "#9b59b6",
                "#2ecc71",
                "#f39c12",
                "#e74c3c",
                "#1abc9c",
                "#34495e",
              ],
              borderRadius: 8,
            },
          ],
        },
        options: {
          responsive: true,
          maintainAspectRatio: false,
          scales: {
            y: {
              beginAtZero: true,
              ticks: {
                precision: 0,
              },
            },
          },
          plugins: {
            legend: {
              display: false,
            },
          },
        },
      });
    },

    goToPrevStockPage() {
      if (this.stockCurrentPage > 1) {
        this.stockCurrentPage--;
      }
    },

    goToNextStockPage() {
      if (this.stockCurrentPage < this.stockTotalPages) {
        this.stockCurrentPage++;
      }
    },
  },
};
</script>

<style scoped>
h2 {
  margin-bottom: 1.5rem;
  color: #2c3e50;
}

h3 {
  margin: 0 0 1rem;
  color: #2c3e50;
}

.dashboard-view {
  display: flex;
  flex-direction: column;
  gap: 1.5rem;
}

.kpi-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(220px, 1fr));
  gap: 1rem;
}

.kpi-card,
.chart-card,
.table-card {
  background: white;
  border-radius: 10px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.08);
  padding: 1.25rem;
}

.kpi-value {
  font-size: 1.8rem;
  font-weight: 700;
  color: #3498db;
  margin: 0;
}

.alert-card .kpi-value {
  color: #e74c3c;
}

.charts-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(320px, 1fr));
  gap: 1rem;
}

.chart-wrapper {
  position: relative;
  height: 340px;
  width: 100%;
}

.table-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  gap: 1rem;
  margin-bottom: 1rem;
}

.checkbox-label {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  color: #555;
}

.error-message {
  background: #fee;
  color: #c00;
  padding: 1rem;
  border-radius: 4px;
}

.loading {
  text-align: center;
  padding: 2rem;
  color: #666;
  background: white;
  border-radius: 10px;
}

.table-container {
  overflow-x: auto;
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
  background: #f8f9fa;
}

.low-stock-row {
  background: #fff3f3;
}

.status-badge {
  display: inline-block;
  padding: 0.25rem 0.65rem;
  border-radius: 999px;
  font-size: 0.85rem;
  font-weight: 600;
}

.status-low {
  background: #fde2e2;
  color: #c0392b;
}

.status-ok {
  background: #e5f8ec;
  color: #1e8449;
}

.pagination {
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 1rem;
  padding: 1rem 0 0;
}

.page-info {
  color: #555;
  font-weight: 600;
}

.record-count {
  padding-top: 1rem;
  color: #666;
  text-align: right;
}

.btn {
  padding: 0.5rem 1rem;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-size: 1rem;
}

.btn-secondary {
  background: #95a5a6;
  color: white;
}

.btn-secondary:hover {
  background: #7f8c8d;
}

.btn-sm {
  padding: 0.35rem 0.65rem;
  font-size: 0.875rem;
}

.chart-wrapper {
  position: relative;
  height: 340px;
  width: 100%;
}

.chart-card {
  background: white;
  border-radius: 10px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.08);
  padding: 1.25rem;
}
</style>

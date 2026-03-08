<template>
  <div class="dashboard">
    <div class="dashboard-header">
  <div>
    <h2>Dashboard</h2>
    <p v-if="lastUpdated" class="last-updated">
      Last updated: {{ lastUpdated }}
    </p>
  </div>

  <button class="refresh-btn" @click="loadDashboard" :disabled="loading">
    {{ loading ? "Refreshing..." : "Refresh" }}
  </button>
</div>

    <div v-if="error" class="error-message">{{ error }}</div>
    <div v-if="loading" class="loading">Loading dashboard...</div>

    <template v-else>
      <div class="kpi-grid">
        <div class="kpi-card">
          <h3>Inventory Value</h3>
          <p class="kpi-value">{{ formatCurrency(inventoryValue) }}</p>
        </div>

        <div class="kpi-card">
          <h3>Total Products</h3>
          <p class="kpi-value">{{ products.length }}</p>
        </div>

        <div class="kpi-card">
          <h3>Total People</h3>
          <p class="kpi-value">{{ people.length }}</p>
        </div>

        <div class="kpi-card">
          <h3>Low Stock Products</h3>
          <p class="kpi-value">{{ lowStockProducts.length }}</p>
          <p class="kpi-subtext">Threshold: {{ lowStockThreshold }}</p>
        </div>
      </div>

      <div class="charts-grid">
        <div class="chart-card">
          <h3>Distribution by Color</h3>
          <div class="chart-wrapper">
            <Doughnut :data="colorChartData" :options="chartOptions" />
          </div>
        </div>

        <div class="chart-card">
          <h3>Customer Analysis by Person Type</h3>
          <div class="chart-wrapper">
            <Bar :data="personTypeChartData" :options="barChartOptions" />
          </div>
        </div>
      </div>

      <div class="table-card">
        <div class="table-header">
          <h3>Stock Management</h3>
          <span class="table-note">
            Products with Safety Stock Level ≤ {{ lowStockThreshold }}
          </span>
        </div>

        <table class="data-table">
          <thead>
            <tr>
              <th>ID</th>
              <th>Name</th>
              <th>Product Number</th>
              <th>Safety Stock Level</th>
              <th>List Price</th>
            </tr>
          </thead>
          <tbody>
            <tr v-if="lowStockProducts.length === 0">
              <td colspan="5" class="placeholder">No low stock products found</td>
            </tr>
            <tr
              v-for="product in lowStockProducts"
              :key="product.productID"
              class="low-stock-row"
            >
              <td>{{ product.productID }}</td>
              <td>{{ product.name }}</td>
              <td>{{ product.productNumber }}</td>
              <td>
                <span
                  class="stock-badge"
                  :class="{
                    critical: product.safetyStockLevel <= 100,
                    warning: product.safetyStockLevel > 100 && product.safetyStockLevel <= lowStockThreshold
                  }"
                >
                  {{ product.safetyStockLevel }}
                </span>
              </td>
              <td>{{ formatCurrency(product.listPrice) }}</td>
            </tr>
          </tbody>
        </table>
      </div>
    </template>
  </div>
</template>

<script>
import { Bar, Doughnut } from "vue-chartjs";
import {
  Chart as ChartJS,
  Title,
  Tooltip,
  Legend,
  ArcElement,
  BarElement,
  CategoryScale,
  LinearScale,
} from "chart.js";

import peopleService from "../services/peopleService";
import productsService from "../services/productsService";

ChartJS.register(
  Title,
  Tooltip,
  Legend,
  ArcElement,
  BarElement,
  CategoryScale,
  LinearScale
);

export default {
  name: "DashboardView",
  components: {
    Bar,
    Doughnut,
  },
  data() {
  return {
    loading: false,
    error: null,
    people: [],
    products: [],
    lowStockThreshold: 500,
    lastUpdated: "",
  };
},
  computed: {
    inventoryValue() {
      return this.products.reduce((sum, product) => {
        return sum + (product.listPrice || 0);
      }, 0);
    },

    lowStockProducts() {
      return [...this.products]
        .filter((product) => product.safetyStockLevel <= this.lowStockThreshold)
        .sort((a, b) => a.safetyStockLevel - b.safetyStockLevel);
    },

    colorChartData() {
      const grouped = {};

      for (const product of this.products) {
        const rawColor = product.color?.trim();
        const color = rawColor ? rawColor : "No Color";
        grouped[color] = (grouped[color] || 0) + 1;
      }

      const sortedEntries = Object.entries(grouped).sort((a, b) => b[1] - a[1]);

      return {
        labels: sortedEntries.map(([label]) => label),
        datasets: [
          {
            label: "Products by Color",
            data: sortedEntries.map(([, value]) => value),
          },
        ],
      };
    },

    personTypeChartData() {
      const grouped = {};

      for (const person of this.people) {
        const type = person.personType?.trim() || "Unknown";
        grouped[type] = (grouped[type] || 0) + 1;
      }

      const sortedEntries = Object.entries(grouped).sort((a, b) => b[1] - a[1]);

      return {
        labels: sortedEntries.map(([label]) => label),
        datasets: [
          {
            label: "People by Person Type",
            data: sortedEntries.map(([, value]) => value),
          },
        ],
      };
    },

    chartOptions() {
      return {
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
          legend: {
            position: "top",
          },
        },
      };
    },

    barChartOptions() {
      return {
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
          legend: {
            display: true,
          },
        },
        scales: {
          y: {
            beginAtZero: true,
            ticks: {
              precision: 0,
            },
          },
        },
      };
    },
  },
  methods: {
    formatCurrency(value) {
      return new Intl.NumberFormat("en-US", {
        style: "currency",
        currency: "USD",
      }).format(value || 0);
    },

    async loadDashboard() {
  this.loading = true;
  this.error = null;

  try {
    const [peopleResponse, productsResponse] = await Promise.all([
      peopleService.getAll(1, 2000),
      productsService.getAll(),
    ]);

    this.people = peopleResponse.items || [];
    this.products = productsResponse || [];
    this.lastUpdated = new Date().toLocaleString();
  } catch (err) {
    this.error =
      err.response?.data?.message ||
      err.message ||
      "Error loading dashboard";
  } finally {
    this.loading = false;
  }
},
  },
  mounted() {
    this.loadDashboard();
  },
};
</script>

<style scoped>
.dashboard h2 {
  margin-bottom: 1.5rem;
  color: #2c3e50;
}

.kpi-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(220px, 1fr));
  gap: 1rem;
  margin-bottom: 1.5rem;
}

.kpi-card,
.chart-card,
.table-card {
  background: white;
  border-radius: 8px;
  padding: 1.25rem;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
}

.kpi-card h3,
.chart-card h3,
.table-card h3 {
  margin-bottom: 0.75rem;
  color: #2c3e50;
}

.kpi-value {
  font-size: 2rem;
  font-weight: bold;
  color: #3498db;
}

.kpi-subtext {
  margin-top: 0.5rem;
  color: #777;
  font-size: 0.95rem;
}

.charts-grid {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 1.5rem;
  margin-bottom: 1.5rem;
}

.chart-wrapper {
  position: relative;
  height: 320px;
}

.table-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  gap: 1rem;
  margin-bottom: 1rem;
}

.table-note {
  color: #666;
  font-size: 0.95rem;
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

.low-stock-row {
  background: #fff5f5;
}

.stock-badge {
  display: inline-block;
  min-width: 56px;
  text-align: center;
  padding: 0.25rem 0.6rem;
  border-radius: 999px;
  font-weight: 600;
}

.stock-badge.warning {
  background: #fff3cd;
  color: #856404;
}

.stock-badge.critical {
  background: #f8d7da;
  color: #842029;
}

.placeholder {
  text-align: center;
  color: #999;
  font-style: italic;
  padding: 2rem !important;
}

.error-message {
  background: #fee;
  color: #c00;
  padding: 1rem;
  border-radius: 4px;
  margin-bottom: 1rem;
}

.loading {
  text-align: center;
  padding: 2rem;
  color: #666;
}

.dashboard-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 1.5rem;
  gap: 1rem;
}

.last-updated {
  margin-top: 0.35rem;
  color: #666;
  font-size: 0.95rem;
}

.refresh-btn {
  background: #3498db;
  color: white;
  border: none;
  border-radius: 6px;
  padding: 0.65rem 1rem;
  cursor: pointer;
  font-size: 0.95rem;
  font-weight: 600;
}

.refresh-btn:hover {
  background: #2980b9;
}

.refresh-btn:disabled {
  background: #95a5a6;
  cursor: not-allowed;
}

@media (max-width: 900px) {
  .charts-grid {
    grid-template-columns: 1fr;
  }

  .table-header {
    flex-direction: column;
    align-items: flex-start;
  }

  .dashboard-header {
    flex-direction: column;
    align-items: flex-start;
  }
}
</style>
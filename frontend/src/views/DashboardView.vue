<template>
  <div class="dashboard">
    <h2>Dashboard</h2>

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
            <Bar :data="personTypeChartData" :options="chartOptions" />
          </div>
        </div>
      </div>

      <div class="table-card">
        <h3>Stock Management</h3>
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
              <td>{{ product.safetyStockLevel }}</td>
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
    };
  },
  computed: {
    inventoryValue() {
      return this.products.reduce((sum, product) => {
        return sum + (product.listPrice || 0);
      }, 0);
    },

    lowStockProducts() {
      return this.products.filter(
        (product) => product.safetyStockLevel <= this.lowStockThreshold
      );
    },

    colorChartData() {
      const grouped = {};

      for (const product of this.products) {
        const color = product.color || "No Color";
        grouped[color] = (grouped[color] || 0) + 1;
      }

      return {
        labels: Object.keys(grouped),
        datasets: [
          {
            label: "Products by Color",
            data: Object.values(grouped),
          },
        ],
      };
    },

    personTypeChartData() {
      const grouped = {};

      for (const person of this.people) {
        const type = person.personType || "Unknown";
        grouped[type] = (grouped[type] || 0) + 1;
      }

      return {
        labels: Object.keys(grouped),
        datasets: [
          {
            label: "People by Person Type",
            data: Object.values(grouped),
          },
        ],
      };
    },

    chartOptions() {
      return {
        responsive: true,
        maintainAspectRatio: false,
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
          peopleService.getAll(),
          productsService.getAll(),
        ]);

        this.people = peopleResponse.items || [];
        this.products = productsResponse || [];
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

@media (max-width: 900px) {
  .charts-grid {
    grid-template-columns: 1fr;
  }
}
</style>
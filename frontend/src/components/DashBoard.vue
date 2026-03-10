<template>
  <div class="dashboard-container">
    <div class="kpi-card">
      <h3>Total Inventory Value</h3>
      <p class="price-display">${{ totalInventoryValue.toLocaleString() }}</p>
    </div>

    <div class="charts-grid">
      <div class="chart-item">
        <h3>Product Distribution by Color</h3>
        <Doughnut v-if="colorData.loaded" :data="colorData.chartData" />
      </div>

      <div class="chart-item">
        <h3>People by Type</h3>
        <Bar v-if="peopleData.loaded" :data="peopleData.chartData" />
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, reactive } from 'vue';
import api from '../api';
import { Chart as ChartJS, Title, Tooltip, Legend, BarElement, CategoryScale, LinearScale, ArcElement } from 'chart.js';
import { Bar, Doughnut } from 'vue-chartjs';

ChartJS.register(Title, Tooltip, Legend, BarElement, CategoryScale, LinearScale, ArcElement);

// --- State ---
const totalInventoryValue = ref(0);
const colorData = reactive({ loaded: false, chartData: null });
const peopleData = reactive({ loaded: false, chartData: null });

onMounted(async () => {
  try {
    // Fetch Products Data
    const productRes = await api.get('/products');
    const products = productRes.data;

    // 1. Inventory Value (Frontend "LINQ" equivalent)
    totalInventoryValue.value = products.reduce((acc, item) => acc + (item.listPrice || 0), 0);

    // 2. Distribution by Color (Grouping Logic)
    const colorCounts = products.reduce((acc, item) => {
      const color = item.color || 'Unknown';
      acc[color] = (acc[color] || 0) + 1;
      return acc;
    }, {});

    colorData.chartData = {
      labels: Object.keys(colorCounts),
      datasets: [{
        backgroundColor: ['#41B883', '#E46651', '#00D8FF', '#DD1B16', '#FADB14'],
        data: Object.values(colorCounts)
      }]
    };
    colorData.loaded = true;

    // 3. Customer Analysis (People API)
    const peopleRes = await api.get('/people');
    const people = peopleRes.data;

    const typeCounts = people.reduce((acc, person) => {
      const type = person.personType || 'Other';
      acc[type] = (acc[type] || 0) + 1;
      return acc;
    }, {});

    peopleData.chartData = {
      labels: Object.keys(typeCounts),
      datasets: [{
        label: 'Number of People',
        backgroundColor: '#3498db',
        data: Object.values(typeCounts)
      }]
    };
    peopleData.loaded = true;

  } catch (error) {
    console.error("Error fetching dashboard data:", error);
  }
});
</script>

<style scoped>
.dashboard-container { padding: 20px; font-family: sans-serif; }
.kpi-card { 
  background: #f8f9fa; border-radius: 8px; padding: 20px; 
  box-shadow: 0 2px 4px rgba(0,0,0,0.1); width: 250px; margin-bottom: 20px;
}
.price-display { font-size: 2rem; font-weight: bold; color: #2c3e50; margin: 0; }
.charts-grid { display: flex; gap: 40px; flex-wrap: wrap; }
.chart-item { width: 400px; }
</style>
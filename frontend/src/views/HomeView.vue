<template>
  <div class="dashboard p-6">
    <h1 class="text-2xl font-bold mb-6">Business Dashboard</h1>

    <div class="card mb-8 p-6 bg-white shadow rounded-lg border-t-4 border-green-500">
      <h3 class="text-gray-500 text-sm font-semibold uppercase">Total Inventory Value</h3>
      <p class="text-4xl font-bold">${{ totalInventoryValue.toLocaleString() }}</p>
    </div>

    <div class="grid grid-cols-1 md:grid-cols-2 gap-10">
      <div class="bg-white p-4 shadow rounded-lg">
        <h3 class="text-center font-bold mb-4">Product Distribution by Color</h3>
        <Doughnut v-if="loaded" :data="colorChartData" />
      </div>

      <div class="bg-white p-4 shadow rounded-lg">
        <h3 class="text-center font-bold mb-4">Customer Analysis (Person Type)</h3>
        <Bar v-if="loaded" :data="peopleChartData" />
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import { getProducts } from '../services/productsService';
import { getPeople } from '../services/peopleService';
import { Chart as ChartJS, Title, Tooltip, Legend, BarElement, CategoryScale, LinearScale, ArcElement } from 'chart.js';
import { Bar, Doughnut } from 'vue-chartjs';

ChartJS.register(Title, Tooltip, Legend, BarElement, CategoryScale, LinearScale, ArcElement);

const loaded = ref(false);
const totalInventoryValue = ref(0);
const colorChartData = ref(null);
const peopleChartData = ref(null);

onMounted(async () => {
  try {
    const [prodRes, peopleRes] = await Promise.all([getProducts(), getPeople()]);
    const products = prodRes.data;
    const people = peopleRes.data;

    // Logic 1: Sum ListPrice
    totalInventoryValue.value = products.reduce((acc, p) => acc + (p.listPrice || 0), 0);

    // Logic 2: Count per Color
    const colors = products.reduce((acc, p) => {
      const c = p.color || 'N/A';
      acc[c] = (acc[c] || 0) + 1;
      return acc;
    }, {});

    colorChartData.value = {
      labels: Object.keys(colors),
      datasets: [{ backgroundColor: ['#41B883', '#E46651', '#00D8FF', '#DD1B16'], data: Object.values(colors) }]
    };

    // Logic 3: Group by Person Type
    const types = people.reduce((acc, p) => {
      const t = p.personType || 'Unknown';
      acc[t] = (acc[t] || 0) + 1;
      return acc;
    }, {});

    peopleChartData.value = {
      labels: Object.keys(types),
      datasets: [{ label: 'Quantity', backgroundColor: '#3498db', data: Object.values(types) }]
    };

    loaded.value = true;
  } catch (error) {
    console.error("Dashboard Load Error:", error);
  }
});
</script>
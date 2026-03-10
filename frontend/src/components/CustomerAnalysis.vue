<template>
  <BarChart :chart-data="chartData" />
</template>

<script setup>
import { ref, onMounted } from 'vue'
import BarChart from '../components/BarChart.vue'
import { peopleService } from '../services/peopleService'
import { Chart as ChartJS, Title, Tooltip, Legend, BarElement, CategoryScale, LinearScale } from 'chart.js'

ChartJS.register(Title, Tooltip, Legend, BarElement, CategoryScale, LinearScale)

const chartData = ref({
  labels: [],
  datasets: [{ label: 'Customers', data: [], backgroundColor: 'orange' }]
});

onMounted(async () => {
  const grouped = await peopleService.groupByPersonType();
  console.log({personTypes: grouped.map(g => g.personType), totals: grouped.map(g => g.total)});

  chartData.value.labels = grouped.map(g => g.personType);
  chartData.value.datasets[0].data = grouped.map(g => g.total);
});
</script>

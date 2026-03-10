<template>
  <PieChart :chart-data="chartData" />
</template>

<script setup>
import { ref, onMounted } from 'vue'
import PieChart from './PieChart.vue'
import { productsService } from '../services/productsService'

const chartData = ref({
  labels: [],
  datasets: [{ data: [], backgroundColor: [] }]
})

const palette = [
  '#3498db',
  '#e74c3c',
  '#f1c40f',
  '#2ecc71',
  '#9b59b6',
  '#34495e',
  '#e67e22',
  '#1abc9c'
]

onMounted(async () => {
  const products = await productsService.getAll()

  const grouped = products.reduce((acc, p) => {
    const color = p.color || 'Unknown'
    acc[color] = (acc[color] || 0) + 1
    return acc
  }, {})

  const labels = Object.keys(grouped)
  chartData.value.labels = labels
  chartData.value.datasets[0].data = Object.values(grouped)
  chartData.value.datasets[0].backgroundColor = labels.map((label, idx) =>
    palette[idx % palette.length]
  )
})
</script>

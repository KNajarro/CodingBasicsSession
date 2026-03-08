<template>
  <div class="chart-wrapper">
    <div v-if="chartData">
      <Doughnut :data="chartData" :options="options" />
    </div>
    <div v-else class="loading">Loading...</div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { Doughnut } from 'vue-chartjs'
import { Chart, ArcElement, Tooltip, Legend } from 'chart.js'
import { productsService } from '../services/productsService'

Chart.register(ArcElement, Tooltip, Legend)

const chartData = ref(null)
const options = {
  responsive: true,
  maintainAspectRatio: false,
  plugins: {
    legend: { position: 'bottom' }
  }
}

async function loadData() {
  try {
    const products = await productsService.getAll()
    const counts = {}
    products.forEach(p => {
      const color = p.color || 'Unknown'
      counts[color] = (counts[color] || 0) + 1
    })
    const labels = Object.keys(counts)
    const data = Object.values(counts)
    const palette = [
      '#3498db',
      '#e74c3c',
      '#2ecc71',
      '#9b59b6',
      '#f1c40f',
      '#34495e',
      '#1abc9c',
      '#e67e22'
    ]
    const backgroundColor = labels.map((_, i) => palette[i % palette.length])
    chartData.value = {
      labels,
      datasets: [
        {
          data,
          backgroundColor
        }
      ]
    }
  } catch (err) {
    console.error('Failed to load color distribution', err)
  }
}

onMounted(loadData)
</script>

<style scoped>
.chart-wrapper {
  background: white;
  border-radius: 8px;
  padding: 1rem;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  width: 100%;
  min-height: 480px;
  height: 100%;
  display: flex;
  justify-content: center;
  align-items: center;
}
.loading {
  text-align: center;
  padding: 2rem;
  color: #666;
}
</style>
<template>
  <canvas ref="chartCanvas"></canvas>
</template>

<script setup>
import { ref, watch, onMounted } from 'vue'
import { Chart, ArcElement, Tooltip, Legend, Title, PieController } from 'chart.js'

const props = defineProps({
  chartData: {
    type: Object,
    required: true
  },
  options: {
    type: Object,
    required: false,
    default: () => ({})
  }
})

Chart.register(PieController, ArcElement, Tooltip, Legend, Title)

const chartCanvas = ref(null)
let chartInstance = null

const renderChart = () => {
  if (chartInstance) {
    chartInstance.destroy()
  }

  chartInstance = new Chart(chartCanvas.value, {
    type: 'pie',
    data: props.chartData,
    options: {
      responsive: true,
      plugins: {
        legend: {
          position: 'top'
        },
        title: {
          display: true,
          text: 'Product Count by Color'
        }
      },
      ...props.options
    }
  })
}

onMounted(() => {
  renderChart()
})

watch(
  () => props.chartData,
  () => {
    renderChart()
  },
  { deep: true }
)
</script>

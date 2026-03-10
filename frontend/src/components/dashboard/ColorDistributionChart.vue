<template>
  <div class="chart-card">
    <h3 class="chart-title">Distribution by Color</h3>
    <p class="chart-sub">Products grouped by color attribute</p>
    <div class="chart-wrapper">
      <Doughnut v-if="chartData" :data="chartData" :options="chartOptions" />
      <div v-else class="chart-empty">No data</div>
    </div>
  </div>
</template>

<script setup>
import { computed } from 'vue'
import { Doughnut } from 'vue-chartjs'
import {
  Chart as ChartJS,
  ArcElement,
  Tooltip,
  Legend
} from 'chart.js'

ChartJS.register(ArcElement, Tooltip, Legend)

const props = defineProps({
  distribution: {
    type: Array,
    default: () => []
  }
})

// Map AdventureWorks color names → real CSS colors
const COLOR_MAP = {
  'Black':        '#1a1a1a',
  'Silver':       '#c0c0c0',
  'Red':          '#e53e3e',
  'Yellow':       '#ecc94b',
  'Blue':         '#3b82f6',
  'Multi':        '#a78bfa',
  'Silver/Black': '#718096',
  'White':        '#f0f0f0',
  'Grey':         '#9ca3af',
  'Green':        '#22c55e',
  'Orange':       '#f97316',
  'Pink':         '#f472b6',
  'Purple':       '#9333ea',
  'Brown':        '#92400e',
  'Gold':         '#d97706',
}

function resolveColor (name) {
  if (!name) return '#64748b'
  const match = Object.keys(COLOR_MAP).find(
    k => k.toLowerCase() === name.toLowerCase()
  )
  return match ? COLOR_MAP[match] : '#64748b'
}

const chartData = computed(() => {
  if (!props.distribution.length) return null
  return {
    labels: props.distribution.map(d => d.color),
    datasets: [{
      data: props.distribution.map(d => d.count),
      backgroundColor: props.distribution.map(d => resolveColor(d.color)),
      borderColor: '#111827',
      borderWidth: 2,
      hoverOffset: 8
    }]
  }
})

const chartOptions = {
  responsive: true,
  maintainAspectRatio: false,
  cutout: '65%',
  plugins: {
    legend: {
      position: 'right',
      labels: {
        color: 'rgba(255,255,255,0.7)',
        font: { size: 11, family: "'Inter', sans-serif" },
        padding: 14,
        boxWidth: 10,
        boxHeight: 10,
        usePointStyle: true
      }
    },
    tooltip: {
      callbacks: {
        label: ctx => ` ${ctx.label}: ${ctx.parsed} products`
      }
    }
  }
}
</script>

<style scoped>
.chart-card {
  background: #111827;
  border: 1px solid rgba(255,255,255,0.07);
  border-radius: 16px;
  padding: 1.5rem;
  box-shadow: 0 4px 24px rgba(0,0,0,0.25);
}

.chart-title {
  margin: 0 0 0.2rem;
  font-size: 1rem;
  font-weight: 700;
  color: #f1f5f9;
  letter-spacing: -0.01em;
}

.chart-sub {
  margin: 0 0 1.25rem;
  font-size: 0.75rem;
  color: rgba(255,255,255,0.35);
}

.chart-wrapper {
  height: 220px;
}

.chart-empty {
  display: flex;
  align-items: center;
  justify-content: center;
  height: 100%;
  color: rgba(255,255,255,0.25);
  font-size: 0.85rem;
}
</style>

<template>
  <div class="chart-card">
    <h3 class="chart-title">Customer Analysis</h3>
    <p class="chart-sub">People count by PersonType</p>
    <div class="chart-wrapper">
      <Bar v-if="chartData" :data="chartData" :options="chartOptions" />
      <div v-else class="chart-empty">No data</div>
    </div>
  </div>
</template>

<script setup>
import { computed } from 'vue'
import { Bar } from 'vue-chartjs'
import {
  Chart as ChartJS,
  BarElement,
  CategoryScale,
  LinearScale,
  Tooltip,
  Legend
} from 'chart.js'

ChartJS.register(BarElement, CategoryScale, LinearScale, Tooltip, Legend)

// Map coded PersonType values to readable labels
const TYPE_LABELS = {
  EM: 'Employee',
  SP: 'Sales Person',
  SC: 'Store Contact',
  IN: 'Individual',
  VC: 'Vendor Contact',
  GC: 'General Contact'
}

const props = defineProps({
  distribution: {
    type: Array,
    default: () => []
  }
})

const chartData = computed(() => {
  if (!props.distribution.length) return null
  return {
    labels: props.distribution.map(d => TYPE_LABELS[d.personType] ?? d.personType),
    datasets: [{
      label: 'People',
      data: props.distribution.map(d => d.count),
      backgroundColor: 'rgba(99, 210, 255, 0.7)',
      borderColor: '#63d2ff',
      borderWidth: 1,
      borderRadius: 6,
      borderSkipped: false
    }]
  }
})

const chartOptions = {
  responsive: true,
  maintainAspectRatio: false,
  plugins: {
    legend: { display: false },
    tooltip: {
      callbacks: {
        label: ctx => ` ${ctx.parsed.y.toLocaleString()} people`
      }
    }
  },
  scales: {
    x: {
      ticks: { color: 'rgba(255,255,255,0.55)', font: { size: 11 } },
      grid: { color: 'rgba(255,255,255,0.05)' }
    },
    y: {
      ticks: {
        color: 'rgba(255,255,255,0.55)',
        font: { size: 11 },
        callback: v => v.toLocaleString()
      },
      grid: { color: 'rgba(255,255,255,0.05)' }
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

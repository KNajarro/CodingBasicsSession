<template>
  <div class="chart-container">
    <h3 class="chart-title">Distribution by Color</h3>
    <div class="chart-wrapper">
      <Doughnut :data="data" :options="options" />
    </div>
    <div class="chart-legend">
      <p class="legend-info">Total {{ data.datasets[0].data.length }} colors identified</p>
    </div>
  </div>
</template>

<script>
import { Doughnut } from 'vue-chartjs'
import { Chart as ChartJS, ArcElement, Tooltip, Legend } from 'chart.js'

ChartJS.register(ArcElement, Tooltip, Legend)

export default {
  name: 'DistributionByColorChart',
  components: {
    Doughnut
  },
  props: {
    data: {
      type: Object,
      required: true
    }
  },
  computed: {
    options() {
      return {
        responsive: true,
        maintainAspectRatio: true,
        plugins: {
          legend: {
            position: 'bottom',
            labels: {
              font: {
                size: 12,
                family: "-apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif"
              },
              padding: 15,
              color: '#555'
            }
          },
          tooltip: {
            backgroundColor: 'rgba(0, 0, 0, 0.8)',
            padding: 12,
            titleFont: { size: 14, weight: 'bold' },
            bodyFont: { size: 13 },
            borderColor: '#ddd',
            borderWidth: 1
          }
        }
      }
    }
  }
}
</script>

<style scoped>
.chart-container {
  display: flex;
  flex-direction: column;
  height: 100%;
}

.chart-title {
  margin: 0 0 1rem 0;
  font-size: 1.1rem;
  font-weight: 600;
  color: #2c3e50;
  border-bottom: 2px solid #3498db;
  padding-bottom: 0.5rem;
}

.chart-wrapper {
  flex: 1;
  position: relative;
  min-height: 300px;
  display: flex;
  align-items: center;
  justify-content: center;
}

.chart-legend {
  margin-top: 1rem;
  padding-top: 1rem;
  border-top: 1px solid #eee;
}

.legend-info {
  margin: 0;
  font-size: 0.9rem;
  color: #666;
  text-align: center;
}
</style>

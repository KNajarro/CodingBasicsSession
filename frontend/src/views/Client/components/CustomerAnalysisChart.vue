<template>
  <div class="chart-container">
    <h3 class="chart-title">Customer Analysis by Person Type</h3>
    <div class="chart-wrapper">
      <Bar :data="data" :options="options" />
    </div>
    <div class="chart-legend">
      <p class="legend-info">Total {{ totalPeople }} people across {{ data.labels.length }} categories</p>
    </div>
  </div>
</template>

<script>
import { Bar } from 'vue-chartjs'
import { Chart as ChartJS, CategoryScale, LinearScale, BarElement, Title, Tooltip, Legend } from 'chart.js'

ChartJS.register(CategoryScale, LinearScale, BarElement, Title, Tooltip, Legend)

export default {
  name: 'CustomerAnalysisChart',
  components: {
    Bar
  },
  props: {
    data: {
      type: Object,
      required: true
    }
  },
  computed: {
    totalPeople() {
      return this.data.datasets[0].data.reduce((sum, val) => sum + val, 0)
    },
    options() {
      return {
        responsive: true,
        maintainAspectRatio: true,
        plugins: {
          legend: {
            display: false
          },
          tooltip: {
            backgroundColor: 'rgba(0, 0, 0, 0.8)',
            padding: 12,
            titleFont: { size: 14, weight: 'bold' },
            bodyFont: { size: 13 },
            borderColor: '#ddd',
            borderWidth: 1,
            callbacks: {
              label: function(context) {
                return 'Count: ' + context.parsed.y
              }
            }
          }
        },
        scales: {
          y: {
            beginAtZero: true,
            grid: {
              color: 'rgba(0, 0, 0, 0.05)',
              drawBorder: false
            },
            ticks: {
              color: '#666',
              font: {
                size: 12
              }
            }
          },
          x: {
            grid: {
              display: false,
              drawBorder: false
            },
            ticks: {
              color: '#666',
              font: {
                size: 11
              }
            }
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

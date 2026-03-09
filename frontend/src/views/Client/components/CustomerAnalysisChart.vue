<template>
  <div class="chart-container">
    <h3 class="chart-title">Customer Analysis by Person Type</h3>
    <p class="chart-subtitle">Distribution of people across different person types</p>
    <div class="chart-wrapper">
      <Bar :data="data" :options="options" />
    </div>
    <div class="chart-legend">
      <p class="legend-info">Total <strong>{{ totalPeople }}</strong> people across <strong>{{ data.labels.length }}</strong> categories</p>
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
  },
  methods: {
    getBarColors() {
      return [
        '#3498db', // Store Contact - Blue
        '#e74c3c', // Individual - Red
        '#2ecc71', // Sales Person - Green
        '#f39c12', // Employee - Orange
        '#9b59b6', // Vendor Contact - Purple
        '#1abc9c'  // General Contact - Teal
      ]
    }
  },
  watch: {
    data: {
      handler() {
        // Update colors when data changes
        if (this.data.datasets && this.data.datasets[0]) {
          this.data.datasets[0].backgroundColor = this.getBarColors()
          this.data.datasets[0].borderColor = this.getBarColors().map(color => this.darkenColor(color))
        }
      },
      immediate: true,
      deep: true
    }
  },
  mounted() {
    // Set colors on mount
    if (this.data.datasets && this.data.datasets[0]) {
      this.data.datasets[0].backgroundColor = this.getBarColors()
      this.data.datasets[0].borderColor = this.getBarColors().map(color => this.darkenColor(color))
    }
  },
  methods: {
    getBarColors() {
      return [
        '#3498db', // Store Contact - Blue
        '#e74c3c', // Individual - Red
        '#2ecc71', // Sales Person - Green
        '#f39c12', // Employee - Orange
        '#9b59b6', // Vendor Contact - Purple
        '#1abc9c'  // General Contact - Teal
      ]
    },
    darkenColor(hex) {
      // Darken color by 20% for border
      const num = parseInt(hex.replace('#',''), 16)
      const amt = Math.round(2.55 * -20)
      const R = (num >> 16) + amt
      const G = (num >> 8 & 0x00FF) + amt
      const B = (num & 0x0000FF) + amt
      return "#" + (0x1000000 + (R<255?R<1?0:R:255)*0x10000 +
        (G<255?G<1?0:G:255)*0x100 + (B<255?B<1?0:B:255))
        .toString(16).slice(1)
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
  margin: 0 0 0.5rem 0;
  font-size: 1.1rem;
  font-weight: 600;
  color: #2c3e50;
  border-bottom: 2px solid #3498db;
  padding-bottom: 0.5rem;
}

.chart-subtitle {
  margin: 0 0 1rem 0;
  font-size: 0.9rem;
  color: #7f8c8d;
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

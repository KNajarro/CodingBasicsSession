<template>
  <div class="chart-container">
    <h5 class="chart-title">{{ title }}</h5>
    <div ref="chartElement" id="customer-analysis-chart"></div>
    <div v-if="loading" class="text-center py-4">
      <div class="spinner-border text-primary" role="status">
        <span class="visually-hidden">Loading...</span>
      </div>
    </div>
    <div v-if="error" class="alert alert-danger mt-3">{{ error }}</div>
  </div>
</template>

<script>
import ApexCharts from 'apexcharts'

export default {
  name: 'CustomerAnalysisChart',
  props: {
    people: {
      type: Array,
      default: () => []
    },
    title: {
      type: String,
      default: 'Customer Analysis by Type'
    }
  },
  data() {
    return {
      chart: null,
      loading: false,
      error: null
    }
  },
  computed: {
    personTypeDistribution() {
      const distribution = {}
      this.people.forEach(person => {
        const type = person.personType || 'Unknown'
        distribution[type] = (distribution[type] || 0) + 1
      })
      return distribution
    },
    chartData() {
      const types = Object.keys(this.personTypeDistribution)
      const counts = Object.values(this.personTypeDistribution)
      
      return {
        categories: types,
        series: [{
          name: 'Count',
          data: counts
        }]
      }
    }
  },
  watch: {
    people: {
      handler() {
        if (this.people.length > 0) {
          this.initChart()
        }
      },
      deep: true
    }
  },
  mounted() {
    if (this.people.length > 0) {
      this.initChart()
    }
  },
  methods: {
    initChart() {
      this.error = null

      const options = {
        chart: {
          type: 'bar',
          height: '100%',
          toolbar: {
            show: true
          }
        },
        colors: ['#3b82f6'],
        plotOptions: {
          bar: {
            columnWidth: '50%',
            dataLabels: {
              position: 'top'
            }
          }
        },
        dataLabels: {
          enabled: true,
          offsetY: -20,
          style: {
            fontSize: '12px',
            colors: ['#304758']
          }
        },
        xaxis: {
          categories: this.chartData.categories,
          position: 'bottom',
          axisBorder: {
            show: true
          },
          axisTicks: {
            show: true
          },
          crosshairs: {
            fill: {
              type: 'gradient',
              gradient: {
                colorFrom: '#D8E3F0',
                colorTo: '#BED1E6'
              }
            }
          }
        },
        yaxis: {
          axisBorder: {
            show: false
          },
          axisTicks: {
            show: false
          },
          labels: {
            show: true
          }
        },
        title: {
          text: '',
          offsetX: 0,
          offsetY: 0,
          floating: false
        },
        grid: {
          row: {
            colors: ['#f3f3f3', 'transparent'],
            opacity: 0.5
          }
        },
        series: this.chartData.series,
        responsive: [{
          breakpoint: 480,
          options: {
            plotOptions: {
              bar: {
                columnWidth: '80%'
              }
            }
          }
        }]
      }

      if (this.chart) {
        this.chart.destroy()
      }

      this.chart = new ApexCharts(this.$refs.chartElement, options)
      this.chart.render()
    }
  },
  beforeUnmount() {
    if (this.chart) {
      this.chart.destroy()
    }
  }
}
</script>

<style scoped>
.chart-container {
  background: white;
  border-radius: 8px;
  padding: 20px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
}

.chart-title {
  font-weight: 600;
  margin-bottom: 15px;
  color: #1a1a1a;
}
</style>

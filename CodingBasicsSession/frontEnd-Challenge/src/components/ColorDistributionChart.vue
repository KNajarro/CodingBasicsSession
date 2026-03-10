<template>
  <div class="chart-container">
    <h5 class="chart-title">{{ title }}</h5>
    <div ref="chartElement" id="color-distribution-chart"></div>
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
  name: 'ColorDistributionChart',
  props: {
    products: {
      type: Array,
      default: () => []
    },
    title: {
      type: String,
      default: 'Distribution by Color'
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
    colorDistribution() {
      const distribution = {}
      this.products.forEach(product => {
        const color = product.color || 'N/A'
        distribution[color] = (distribution[color] || 0) + 1
      })
      return distribution
    },
    chartData() {
      return {
        labels: Object.keys(this.colorDistribution),
        series: Object.values(this.colorDistribution)
      }
    }
  },
  watch: {
    products: {
      handler() {
        if (this.products.length > 0) {
          this.initChart()
        }
      },
      deep: true
    }
  },
  mounted() {
    if (this.products.length > 0) {
      this.initChart()
    }
  },
  methods: {
    initChart() {
      this.error = null
      
      const options = {
        chart: {
          type: 'donut',
          height: '100%',
          toolbar: {
            show: true
          }
        },
        labels: this.chartData.labels,
        series: this.chartData.series,
        colors: ['#1f77b4', '#ff7f0e', '#2ca02c', '#d62728', '#9467bd', '#8c564b', '#e377c2', '#7f7f7f', '#bcbd22', '#17becf'],
        plotOptions: {
          pie: {
            donut: {
              size: '65%'
            }
          }
        },
        dataLabels: {
          enabled: true,
          formatter: (val) => Math.round(val) + '%'
        },
        legend: {
          position: 'bottom',
          fontSize: '12px'
        },
        tooltip: {
          y: {
            formatter: (val) => val + ' products'
          }
        },
        responsive: [{
          breakpoint: 480,
          options: {
            legend: {
              position: 'bottom'
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

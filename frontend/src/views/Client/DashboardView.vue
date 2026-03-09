<template>
  <div class="dashboard-view">
    <h2>Dashboard</h2>

    <div v-if="error" class="error-message">{{ error }}</div>
    
    <div v-if="loading" class="loading-spinner">
      <div class="spinner"></div>
      <p>Loading dashboard data...</p>
    </div>

    <div v-else class="dashboard-content">
      <!-- KPI Cards Section -->
      <div class="kpi-section">
        <InventoryValueCard :totalValue="inventoryValue" />
      </div>

      <!-- Charts Section -->
      <div class="charts-section">
        <div class="chart-wrapper">
          <DistributionByColorChart :data="colorDistributionData" />
        </div>
        <div class="chart-wrapper">
          <CustomerAnalysisChart :data="personTypeData" />
        </div>
      </div>

      <!-- Stock Management Section -->
      <div class="stock-section">
        <StockManagementTable :products="lowStockProducts" />
      </div>
    </div>
  </div>
</template>

<script>
import { ref, onMounted, computed } from 'vue'
import productsService from '../../services/productsService'
import peopleService from '../../services/peopleService'
import InventoryValueCard from './components/InventoryValueCard.vue'
import DistributionByColorChart from './components/DistributionByColorChart.vue'
import CustomerAnalysisChart from './components/CustomerAnalysisChart.vue'
import StockManagementTable from './components/StockManagementTable.vue'

export default {
  name: 'DashboardView',
  components: {
    InventoryValueCard,
    DistributionByColorChart,
    CustomerAnalysisChart,
    StockManagementTable
  },
  setup() {
    const loading = ref(true)
    const error = ref(null)
    const products = ref([])
    const people = ref([])

    const inventoryValue = computed(() => {
      return products.value.reduce((sum, product) => {
        return sum + (product.listPrice || 0)
      }, 0)
    })

    const colorDistributionData = computed(() => {
      const colorMap = {}
      
      products.value.forEach(product => {
        const color = product.color || 'No Color'
        colorMap[color] = (colorMap[color] || 0) + 1
      })

      return {
        labels: Object.keys(colorMap),
        datasets: [
          {
            label: 'Products by Color',
            data: Object.values(colorMap),
            backgroundColor: generateColors(Object.keys(colorMap).length),
            borderColor: '#fff',
            borderWidth: 2
          }
        ]
      }
    })

    const personTypeData = computed(() => {
      const typeMap = {
        'SC': 0, // Store Contact
        'IN': 0, // Individual
        'SP': 0, // Sales Person
        'EM': 0, // Employee
        'VC': 0, // Vendor Contact
        'GC': 0  // General Contact
      }

      people.value.forEach(person => {
        if (person.personType in typeMap) {
          typeMap[person.personType]++
        }
      })

      const labels = [
        'Store Contact',
        'Individual',
        'Sales Person',
        'Employee',
        'Vendor Contact',
        'General Contact'
      ]

      const colors = [
        '#3498db', // Store Contact - Blue
        '#e74c3c', // Individual - Red
        '#2ecc71', // Sales Person - Green
        '#f39c12', // Employee - Orange
        '#9b59b6', // Vendor Contact - Purple
        '#1abc9c'  // General Contact - Teal
      ]

      return {
        labels,
        datasets: [
          {
            label: 'Number of People',
            data: Object.values(typeMap),
            backgroundColor: colors,
            borderColor: colors,
            borderWidth: 1
          }
        ]
      }
    })

    const lowStockProducts = computed(() => {
      // Return all products - let StockManagementTable handle filtering for better UX
      return products.value
    })

    const loadDashboardData = async () => {
      loading.value = true
      error.value = null
      try {
        // Load all products by fetching all pages
        let allProducts = []
        let currentPage = 1
        let hasMore = true
        const pageSize = 500

        while (hasMore) {
          const response = await productsService.getAllWithPagination(currentPage, pageSize)
          allProducts = allProducts.concat(response.items || [])
          
          // Check if we've loaded all items
          if (allProducts.length >= response.totalCount) {
            hasMore = false
          } else {
            currentPage++
          }
        }
        products.value = allProducts

        // Load all people by fetching all pages
        let allPeople = []
        currentPage = 1
        hasMore = true

        while (hasMore) {
          const response = await peopleService.getAllWithPagination(currentPage, pageSize)
          allPeople = allPeople.concat(response.items || [])
          
          // Check if we've loaded all items
          if (allPeople.length >= response.totalCount) {
            hasMore = false
          } else {
            currentPage++
          }
        }
        people.value = allPeople
      } catch (err) {
        error.value = `Error loading dashboard data: ${err.message}`
        console.error('Dashboard error:', err)
      } finally {
        loading.value = false
      }
    }

    const generateColors = (count) => {
      const colors = [
        '#FF6384', '#36A2EB', '#FFCE56', '#4BC0C0', '#9966FF',
        '#FF9F40', '#FF6384', '#C9CBCF', '#4BC0C0', '#FF6384',
        '#36A2EB', '#FFCE56', '#FF9F40', '#FF6384', '#C9CBCF'
      ]
      const result = []
      for (let i = 0; i < count; i++) {
        result.push(colors[i % colors.length])
      }
      return result
    }

    onMounted(() => {
      loadDashboardData()
    })

    return {
      loading,
      error,
      inventoryValue,
      colorDistributionData,
      personTypeData,
      lowStockProducts
    }
  }
}
</script>

<style scoped>
.dashboard-view {
  padding: 0;
}

.dashboard-view h2 {
  margin-bottom: 2rem;
  color: #2c3e50;
}

.error-message {
  background: #fee;
  color: #c00;
  padding: 1rem;
  border-radius: 8px;
  margin-bottom: 1rem;
  border-left: 4px solid #c00;
}

.loading-spinner {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  padding: 4rem 2rem;
  color: #666;
}

.spinner {
  width: 40px;
  height: 40px;
  border: 4px solid #f3f3f3;
  border-top: 4px solid #3498db;
  border-radius: 50%;
  animation: spin 1s linear infinite;
  margin-bottom: 1rem;
}

@keyframes spin {
  0% { transform: rotate(0deg); }
  100% { transform: rotate(360deg); }
}

.dashboard-content {
  display: flex;
  flex-direction: column;
  gap: 2rem;
}

.kpi-section {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
  gap: 1.5rem;
}

.charts-section {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(400px, 1fr));
  gap: 2rem;
}

.chart-wrapper {
  background: white;
  padding: 1.5rem;
  border-radius: 8px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
  display: flex;
  flex-direction: column;
}

.stock-section {
  background: white;
  padding: 1.5rem;
  border-radius: 8px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
}

/* Responsive adjustments */
@media (max-width: 1024px) {
  .charts-section {
    grid-template-columns: 1fr;
  }

  .dashboard-header {
    margin: -2rem -2rem 1.5rem -2rem;
    padding: 1.5rem;
  }

  .dashboard-header h1 {
    font-size: 2rem;
  }
}

@media (max-width: 768px) {
  .kpi-section {
    grid-template-columns: 1fr;
  }

  .dashboard-header h1 {
    font-size: 1.5rem;
  }

  .subtitle {
    font-size: 1rem;
  }
}
</style>

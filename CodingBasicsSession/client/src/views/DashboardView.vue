<template>
  <div class="dashboard">
    <div v-if="loading" class="text-center py-5">
      <div class="spinner-border text-primary" role="status">
        <span class="visually-hidden">Loading...</span>
      </div>
      <p class="mt-3">Loading dashboard data...</p>
    </div>

    <div v-else-if="error" class="alert alert-danger" role="alert">
      <i class="bi bi-exclamation-triangle"></i>
      <strong>Error:</strong> {{ error }}
      <button @click="loadData" class="btn btn-sm btn-outline-danger mt-2">
        <i class="bi bi-arrow-clockwise"></i> Retry
      </button>
    </div>

    <div v-else class="dashboard-content">
      <!-- KPI Cards Row -->
      <div class="row g-3 mb-4">
        <div class="col-lg-4 col-md-6">
          <KPICard
            title="Total Inventory Value"
            :value="totalInventoryValue"
            format="currency"
            icon="box2-heart"
            iconBgColor="#d4edda"
            subtitle="Sum of all product list prices"
          />
        </div>
        <div class="col-lg-4 col-md-6">
          <KPICard
            title="Total Products"
            :value="products.length"
            format="number"
            icon="archive"
            iconBgColor="#cfe2ff"
            :subtitle="`${products.length} SKUs in catalog`"
          />
        </div>
        <div class="col-lg-4 col-md-6">
          <KPICard
            title="Total Customers"
            :value="people.length"
            format="number"
            icon="people"
            iconBgColor="#fff3cd"
            :subtitle="`${people.length} registered people`"
          />
        </div>
      </div>

      <!-- Charts Row -->
      <div class="row g-3 mb-4">
        <div class="col-lg-6">
          <ColorDistributionChart :products="products" />
        </div>
        <div class="col-lg-6">
          <CustomerAnalysisChart :people="people" />
        </div>
      </div>

      <!-- Stock Management Table Row -->
      <div class="row g-3">
        <div class="col-12">
          <StockManagementTable :products="products" />
        </div>
      </div>

      <!-- Data Status Row -->
      <div class="row g-3 mt-4">
        <div class="col-12">
          <div class="card bg-light">
            <div class="card-body text-muted small">
              <i class="bi bi-info-circle"></i>
              <strong>Data Status:</strong> 
              Products: {{ products.length }} loaded | 
              People: {{ people.length }} loaded | 
              Backend: {{ backendStatus }}
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import KPICard from '../components/KPICard.vue'
import ColorDistributionChart from '../components/ColorDistributionChart.vue'
import CustomerAnalysisChart from '../components/CustomerAnalysisChart.vue'
import StockManagementTable from '../components/StockManagementTable.vue'
import { productsService } from '../services/productsService'
import { peopleService } from '../services/peopleService'

export default {
  name: 'DashboardView',
  components: {
    KPICard,
    ColorDistributionChart,
    CustomerAnalysisChart,
    StockManagementTable
  },
  data() {
    return {
      products: [],
      people: [],
      loading: true,
      error: null,
      backendStatus: 'Connecting...'
    }
  },
  computed: {
    totalInventoryValue() {
      return this.products.reduce((sum, product) => sum + (product.listPrice || 0), 0)
    }
  },
  async mounted() {
    await this.loadData()
  },
  methods: {
    async loadData() {
      this.loading = true
      this.error = null
      this.backendStatus = 'Connecting...'

      try {
        // Load products
        console.log('Fetching products from backend...')
        const productsData = await productsService.getAll(1000)
        console.log('Products fetched:', productsData.length, 'items')
        this.products = productsData

        // Load people
        console.log('Fetching people from backend...')
        const peopleData = await peopleService.getAll(1000)
        console.log('People fetched:', peopleData.length, 'items')
        this.people = peopleData

        this.backendStatus = '✅ Connected'
      } catch (err) {
        console.error('Error loading dashboard data:', err)
        this.error = `Failed to load data: ${err.message}. Make sure the backend API is running on http://localhost:5261`
        this.backendStatus = '❌ Connection failed'
      } finally {
        this.loading = false
      }
    }
  }
}
</script>

<style scoped>
.dashboard {
  min-height: 100vh;
  padding-bottom: 40px;
}

.dashboard-content {
  animation: fadeIn 0.3s ease-in;
}

@keyframes fadeIn {
  from {
    opacity: 0;
    transform: translateY(10px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

.card {
  border: none;
  border-radius: 8px;
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.05);
}
</style>

<template>
  <div class="analytics-page">
    <header class="page-header">
      <div class="header-label">AdventureWorks</div>
      <h1 class="page-title">Analytics Dashboard</h1>
      <p class="page-desc">Real-time KPIs, charts, and inventory intelligence</p>
    </header>

    <!-- KPI Row -->
    <section class="kpi-row">
      <InventoryKpiCard :total="inventoryValue" />
    </section>

    <!-- Charts Row -->
    <section class="charts-grid">
      <ColorDistributionChart :distribution="colorDistribution" />
      <CustomerAnalysisChart :distribution="customerAnalysis" />
    </section>

    <!-- Low Stock Table -->
    <section class="table-section">
      <LowStockTable />
    </section>

    <!-- Error banner -->
    <div v-if="error" class="error-banner">
      {{ error }}
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { dashboardService } from '../services/dashboardService'
import InventoryKpiCard      from '../components/dashboard/InventoryKpiCard.vue'
import ColorDistributionChart from '../components/dashboard/ColorDistributionChart.vue'
import CustomerAnalysisChart  from '../components/dashboard/CustomerAnalysisChart.vue'
import LowStockTable          from '../components/dashboard/LowStockTable.vue'

const inventoryValue    = ref(0)
const colorDistribution = ref([])
const customerAnalysis  = ref([])
const error             = ref(null)

onMounted(async () => {
  try {
    const [invRes, colorRes, custRes] = await Promise.all([
      dashboardService.getInventoryValue(),
      dashboardService.getColorDistribution(),
      dashboardService.getCustomerAnalysis()
    ])

    inventoryValue.value    = invRes.data.totalInventoryValue
    colorDistribution.value = colorRes.data
    customerAnalysis.value  = custRes.data
  } catch (err) {
    error.value = 'Failed to load dashboard data. Make sure the API is running.'
    console.error(err)
  }
})
</script>

<style scoped>
.analytics-page {
  min-height: 100vh;
  background: #0a0f1e;
  padding: 2rem 2.5rem 4rem;
  font-family: 'Inter', system-ui, sans-serif;
}

.page-header {
  margin-bottom: 2.5rem;
}

.header-label {
  font-size: 0.7rem;
  font-weight: 700;
  letter-spacing: 0.18em;
  text-transform: uppercase;
  color: #63d2ff;
  margin-bottom: 0.4rem;
}

.page-title {
  margin: 0 0 0.35rem;
  font-size: 2rem;
  font-weight: 800;
  color: #f1f5f9;
  letter-spacing: -0.03em;
}

.page-desc {
  margin: 0;
  font-size: 0.85rem;
  color: rgba(255,255,255,0.35);
}

.kpi-row {
  margin-bottom: 1.75rem;
}

.charts-grid {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 1.25rem;
  margin-bottom: 1.75rem;
}

@media (max-width: 768px) {
  .analytics-page {
    padding: 1.25rem 1rem 3rem;
  }

  .charts-grid {
    grid-template-columns: 1fr;
  }
}

.table-section {
  /* full-width table */
}

.error-banner {
  margin-top: 1.5rem;
  background: rgba(248, 113, 113, 0.1);
  border: 1px solid rgba(248, 113, 113, 0.25);
  border-radius: 10px;
  color: #f87171;
  padding: 0.85rem 1.25rem;
  font-size: 0.85rem;
}
</style>

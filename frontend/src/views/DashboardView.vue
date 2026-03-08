<template>
  <div class="dashboard">
    <section class="section kpi-section">
      <InventoryKPI />
    </section>

    <section class="section charts-wrapper">
      <div class="chart-section">
        <ColorDistributionChart />
      </div>
      <div class="chart-section">
        <CustomerAnalysisChart />
      </div>
    </section>

    <section class="section table-section">
      <LowStockTable />
    </section>
  </div>
</template>

<script>
import InventoryKPI from '../components/InventoryKPI.vue'
import ColorDistributionChart from '../components/ColorDistributionChart.vue'
import CustomerAnalysisChart from '../components/CustomerAnalysisChart.vue'
import LowStockTable from '../components/LowStockTable.vue'

export default {
  name: 'DashboardView',
  components: {
    InventoryKPI,
    ColorDistributionChart,
    CustomerAnalysisChart,
    LowStockTable
  }
}
</script>

<style scoped>
.dashboard {
  display: grid;
  grid-template-columns: 1fr;
  grid-auto-rows: auto;
  gap: 2rem;
  padding: 1rem 0; /* only vertical padding, use full width */
}
.section {
  width: 100%;
}
.kpi-section {
  display: flex;
  justify-content: center;
}
.chart-section {
  display: flex;
  justify-content: stretch; /* stretch child to fill column */
  align-items: center;
  min-height: 360px;
  width: 100%;
}
.charts-wrapper {
  display: flex;
  flex-direction: column;
  gap: 2rem;
  align-items: center; /* center the pair of charts */
  width: 100%;
  max-width: 1200px; /* match table max width */
  margin: 0 auto;
}
.table-section {
  overflow-x: auto;
  max-width: 1200px;
  margin: 0 auto;
}
@media (min-width: 768px) {
  .dashboard {
    padding: 1rem 1rem;
    /* two columns: table spans both, charts share second row */
    grid-template-columns: 1fr 1fr;
    grid-template-areas:
      "kpi kpi"
      "table table"
      "color customer";
  }
  .kpi-section { grid-area: kpi; }
  .table-section { grid-area: table; }
  .charts-wrapper { grid-column: 1 / -1; }
  .chart-section {
    display: flex;
    justify-content: center;
    align-items: center;
    min-height: 500px;
    width: 100%;
  }
  .table-section {
    max-height: calc(100vh - 200px);
    overflow-y: auto;
  }

  /* extra-wide: keep charts side by side and centered */
  @media (min-width: 1200px) {
    .charts-wrapper {
      flex-direction: row;
      justify-content: center;
      align-items: center;
    }
    .chart-section {
      flex: 0 0 50%; /* exactly half of wrapper */
      max-width: 50%;
    }
  }
}
</style>
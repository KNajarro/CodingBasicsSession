<template>
  <section class="dashboard">
    <div v-if="loading" class="state-card">Loading dashboard...</div>
    <div v-else-if="error" class="state-card error">
      <p>{{ error }}</p>
      <button type="button" @click="reload">Retry</button>
    </div>
    <template v-else>
      <div class="kpi-grid">
        <KpiCard
          label="Total Inventory Value"
          :value="inventoryValue"
          hint="Calculated from list price and estimated stock quantity"
        />
        <article class="stat-card">
          <p>Products tracked</p>
          <h3>{{ stockRows.length }}</h3>
        </article>
        <article class="stat-card warn">
          <p>Low stock products</p>
          <h3>{{ lowStockCount }}</h3>
          <small>Threshold <= {{ lowStockThreshold }}</small>
        </article>
      </div>

      <DashboardCharts
        :products-by-color="productsByColor"
        :people-by-type="peopleByType"
      />

      <StockTable :rows="stockRows" :low-stock-threshold="lowStockThreshold" />
    </template>
  </section>
</template>

<script setup>
import DashboardCharts from '../components/DashboardCharts.vue'
import KpiCard from '../components/KpiCard.vue'
import StockTable from '../components/StockTable.vue'
import { useDashboardData } from '../composables/useDashboardData'

const {
  loading,
  error,
  inventoryValue,
  stockRows,
  productsByColor,
  peopleByType,
  lowStockCount,
  lowStockThreshold,
  reload
} = useDashboardData()
</script>

<style scoped>
.dashboard {
  display: grid;
  gap: 1rem;
}

.kpi-grid {
  display: grid;
  grid-template-columns: 2fr 1fr 1fr;
  gap: 1rem;
}

.stat-card {
  background: var(--panel);
  border: 1px solid var(--line);
  border-radius: var(--radius);
  box-shadow: var(--shadow);
  padding: 1rem;
}

.stat-card p {
  margin: 0;
  color: var(--muted);
}

.stat-card h3 {
  margin: 0.35rem 0 0;
  font-size: 2rem;
  color: var(--brand);
}

.stat-card.warn h3 {
  color: var(--warn);
}

.state-card {
  background: var(--panel);
  border: 1px solid var(--line);
  border-radius: var(--radius);
  padding: 1.1rem;
}

.state-card.error {
  border-color: #d57d50;
}

.state-card button {
  border: 0;
  background: var(--brand);
  color: white;
  border-radius: 10px;
  padding: 0.45rem 0.75rem;
  cursor: pointer;
}

@media (max-width: 980px) {
  .kpi-grid {
    grid-template-columns: 1fr;
  }
}
</style>

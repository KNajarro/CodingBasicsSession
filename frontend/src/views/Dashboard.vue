<template>
  <div class="dashboard-grid">
    <h2>Dashboard</h2>
    <div class="grid-element">
      <InventoryValue />
    </div>
    <div class="grid-element">
      <DistributionByColor />
    </div>
  <div class="grid-element">
      <CustomerAnalysis />
    </div>
    <div class="grid-element">
      <StockManagement />
    </div>
  </div>
</template>

<script setup>
import InventoryValue from '../components/InventoryValue.vue';
import DistributionByColor from '../components/DistributionByColor.vue';
import CustomerAnalysis from '../components/CustomerAnalysis.vue';
import StockManagement from '../components/StockManagement.vue';
</script>

<style scoped>
.dashboard-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(400px, 1fr));
  gap: 2rem;
}
.grid-element {
  max-height: 400px;
}

</style>


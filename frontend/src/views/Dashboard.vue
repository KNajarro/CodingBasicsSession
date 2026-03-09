<template>
  <div class="dashboard">
    <div class="dashboard-tabs">
      <button
        type="button"
        class="tab"
        :class="{ active: activeTab === 'people' }"
        @click="activeTab = 'people'"
      >
        Dashboard de People
      </button>
      <button
        type="button"
        class="tab"
        :class="{ active: activeTab === 'products' }"
        @click="activeTab = 'products'"
      >
        Dashboard de Products
      </button>
    </div>
    <div class="dashboard-content">
      <PeopleView v-if="activeTab === 'people'" :key="'people'" />
      <ProductsView v-else-if="activeTab === 'products'" :key="'products'" />
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue'
import PeopleView from '../components/People/PeopleView.vue'
import ProductsView from '../components/Products/ProductsView.vue'

const activeTab = ref('people')
</script>

<style scoped>
.dashboard {
  max-width: 1200px;
  margin: 0 auto;
}

.dashboard-tabs {
  display: flex;
  gap: 0.5rem;
  margin-bottom: 1.5rem;
  border-bottom: 2px solid #e2e8f0;
  padding-bottom: 0;
}

.tab {
  padding: 0.75rem 1.5rem;
  font-size: 1rem;
  font-weight: 600;
  color: #64748b;
  background: transparent;
  border: none;
  border-bottom: 3px solid transparent;
  margin-bottom: -2px;
  cursor: pointer;
  transition: color 0.2s, border-color 0.2s;
}

.tab:hover {
  color: #334155;
}

.tab.active {
  color: #0ea5e9;
  border-bottom-color: #0ea5e9;
}

.dashboard-content {
  min-height: 400px;
}
</style>

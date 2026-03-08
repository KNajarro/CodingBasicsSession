<template>
  <div class="kpi-card">
    <h3>Inventory Value</h3>
    <p class="value">{{ formattedValue }}</p>
  </div>
</template>

<script setup>
import { ref, onMounted, computed } from 'vue'
import { productsService } from '../services/productsService'

const total = ref(0)

const formattedValue = computed(() => {
  return new Intl.NumberFormat('en-US', {
    style: 'currency',
    currency: 'USD'
  }).format(total.value)
})

async function loadInventory() {
  try {
    const products = await productsService.getAll()
    // API returns PascalCase property names
    total.value = products.reduce((sum, p) => sum + (p.listPrice || 0), 0)
  } catch (err) {
    console.error('Failed to load inventory', err)
  }
}

onMounted(loadInventory)
</script>

<style scoped>
.kpi-card {
  background: white;
  border-radius: 8px;
  padding: 1.5rem;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  text-align: center;
}
.kpi-card h3 {
  margin: 0;
  color: #2c3e50;
  font-size: 1.25rem;
}
.kpi-card .value {
  margin-top: 0.5rem;
  font-size: 2rem;
  font-weight: bold;
  color: #27ae60;
}
</style>
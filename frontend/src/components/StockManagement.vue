<template>
  <div class="stock-management">
    <div class="controls">
      <label>
        <input type="checkbox" v-model="showOnlyLow" />
        Show only low stock
      </label>
      <label>
        Low stock threshold
        <input type="number" v-model.number="threshold" min="0" />
      </label>
    </div>

    <table>
      <thead>
        <tr>
          <th>Product</th>
          <th>Stock Level</th>
        </tr>
      </thead>
      <tbody>
        <tr
          v-for="p in displayedProducts"
          :key="p.id"
          :class="{ 'low-stock': p.safetyStockLevel < threshold }"
        >
          <td>{{ p.name }}</td>
          <td>{{ p.safetyStockLevel }}</td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue'
import api from '../services/api'

const products = ref([])
const threshold = ref(50)
const showOnlyLow = ref(true)

const displayedProducts = computed(() => {
  const list = products.value
  if (showOnlyLow.value) {
    return list.filter((p) => p.safetyStockLevel < threshold.value)
  }
  return list
})

onMounted(async () => {
  const { data } = await api.get('/products')
  products.value = data
  console.log({data})
})
</script>

<style scoped>
.stock-management .controls {
  display: flex;
  align-items: center;
  gap: 1rem;
  margin-bottom: 0.75rem;
}

.stock-management .controls label {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  font-size: 0.9rem;
}

.low-stock {
  background: rgba(231, 76, 60, 0.15);
}
</style>

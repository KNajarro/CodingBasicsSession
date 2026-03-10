<template>
  <div class="card">
    <h3>Inventory Value</h3>
    <p>{{ totalValue }}</p>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import api from '../services/api';

const totalValue = ref(0);

onMounted(async () => {
  const { data } = await api.get('/products');
  totalValue.value = data.reduce((sum, p) => sum + p.listPrice, 0);
});
</script>

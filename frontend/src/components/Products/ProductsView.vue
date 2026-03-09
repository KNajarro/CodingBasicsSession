<template>
  <div class="products-view">
    <div class="filters card">
      <div class="form-row">
        <div class="form-group">
          <label for="products-name">Name</label>
          <input
            id="products-name"
            v-model="searchName"
            type="text"
            placeholder="Search by name..."
            class="input"
          />
        </div>
        <div class="form-group">
          <label for="products-category">Category</label>
          <select id="products-category" v-model="searchCategory" class="input">
            <option value="">All Categories</option>
            <option v-for="c in categories" :key="c" :value="c">{{ c }}</option>
          </select>
        </div>
        <button type="button" class="btn btn-primary" @click="handleSearch">
          Buscar
        </button>
      </div>
    </div>

    <div v-if="error" class="message error">{{ error }}</div>
    <div v-if="loading" class="loading">Loading...</div>
    <template v-else>
      <DataTable
        :columns="productColumns"
        :items="items"
        empty-message="No hay productos. Usa Buscar o recarga la vista."
      >
        <template #listPrice="{ value }">
          {{ formatPrice(value) }}
        </template>
      </DataTable>
    </template>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import DataTable from '../shared/DataTable.vue'
import { useProducts } from '../../composables/useProducts'

const {
  items,
  categories,
  loading,
  error,
  search,
  init
} = useProducts()

const searchName = ref('')
const searchCategory = ref('')

const productColumns = [
  { key: 'productID', label: 'ID' },
  { key: 'name', label: 'Name' },
  { key: 'productNumber', label: 'Number' },
  { key: 'color', label: 'Color' },
  { key: 'listPrice', label: 'Price' },
  { key: 'categoryName', label: 'Category' },
  { key: 'subcategoryName', label: 'Subcategory' }
]

function formatPrice(value) {
  if (value == null || value === '') return '-'
  return new Intl.NumberFormat('en-US', {
    style: 'currency',
    currency: 'USD'
  }).format(value)
}

function handleSearch() {
  search(searchName.value, searchCategory.value)
}

onMounted(() => {
  init()
})
</script>

<style scoped>
.products-view {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.filters.card {
  background: white;
  border-radius: 8px;
  padding: 1rem 1.25rem;
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.08);
}

.form-row {
  display: flex;
  flex-wrap: wrap;
  align-items: flex-end;
  gap: 1rem;
}

.form-group {
  display: flex;
  flex-direction: column;
  gap: 0.35rem;
}

.form-group label {
  font-weight: 600;
  color: #475569;
  font-size: 0.875rem;
}

.input {
  padding: 0.5rem 0.75rem;
  border: 1px solid #e2e8f0;
  border-radius: 6px;
  font-size: 1rem;
  min-width: 200px;
}

.input:focus {
  outline: none;
  border-color: #0ea5e9;
  box-shadow: 0 0 0 2px rgba(14, 165, 233, 0.2);
}

.btn {
  padding: 0.5rem 1rem;
  border: none;
  border-radius: 6px;
  font-size: 1rem;
  font-weight: 500;
  cursor: pointer;
}

.btn-primary {
  background: #0ea5e9;
  color: white;
}

.btn-primary:hover {
  background: #0284c7;
}

.message {
  padding: 1rem;
  border-radius: 6px;
}

.message.error {
  background: #fef2f2;
  color: #b91c1c;
}

.loading {
  text-align: center;
  padding: 2rem;
  color: #64748b;
}
</style>

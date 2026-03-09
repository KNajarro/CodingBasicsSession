<template>
  <div class="people-view">
    <div class="filters card">
      <div class="form-row">
        <div class="form-group">
          <label for="people-name">Name</label>
          <input
            id="people-name"
            v-model="searchName"
            type="text"
            placeholder="Search by name..."
            class="input"
          />
        </div>
        <div class="form-group">
          <label for="people-type">Person Type</label>
          <select id="people-type" v-model="searchType" class="input">
            <option value="">All Types</option>
            <option v-for="t in types" :key="t" :value="t">{{ t }}</option>
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
        :columns="peopleColumns"
        :items="items"
        empty-message="No hay personas. Usa Buscar o recarga la vista."
      />
    </template>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import DataTable from '../shared/DataTable.vue'
import { usePeople } from '../../composables/usePeople'

const {
  items,
  types,
  loading,
  error,
  loadAll,
  search,
  init
} = usePeople()

const searchName = ref('')
const searchType = ref('')

const peopleColumns = [
  { key: 'businessEntityID', label: 'ID' },
  { key: 'personType', label: 'Type' },
  { key: 'title', label: 'Title' },
  { key: 'firstName', label: 'First Name' },
  { key: 'middleName', label: 'Middle Name' },
  { key: 'lastName', label: 'Last Name' },
  { key: 'suffix', label: 'Suffix' },
  { key: 'emailPromotion', label: 'Email Promo' }
]

function handleSearch() {
  search(searchName.value, searchType.value)
}

onMounted(() => {
  init()
})
</script>

<style scoped>
.people-view {
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

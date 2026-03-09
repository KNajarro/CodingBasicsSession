<template>
  <div class="data-table-wrapper">
    <table class="data-table">
      <thead>
        <tr>
          <th v-for="col in columns" :key="col.key">{{ col.label }}</th>
        </tr>
      </thead>
      <tbody>
        <tr v-if="items.length === 0">
          <td :colspan="columns.length" class="empty-row">{{ emptyMessage }}</td>
        </tr>
        <tr v-else v-for="(item, i) in items" :key="getRowKey(item, i)">
          <td v-for="col in columns" :key="col.key">
            <slot :name="col.key" :item="item" :value="item[col.key]">
              {{ item[col.key] ?? '-' }}
            </slot>
          </td>
        </tr>
      </tbody>
    </table>
    <p v-if="items.length > 0" class="record-count">Total: {{ items.length }}</p>
  </div>
</template>

<script setup>
/**
 * Tabla genérica reutilizable.
 * Props: columns [{ key, label }], items [], emptyMessage
 * Slots: #<col.key>="{ item, value }" para formatear celdas (ej. #listPrice="{ value }")
 */
defineProps({
  columns: { type: Array, required: true },
  items: { type: Array, default: () => [] },
  emptyMessage: { type: String, default: 'No data available' }
})

function getRowKey(item, index) {
  return item?.businessEntityID ?? item?.productID ?? index
}
</script>

<style scoped>
.data-table-wrapper {
  background: white;
  border-radius: 8px;
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.08);
  overflow-x: auto;
}
.data-table {
  width: 100%;
  border-collapse: collapse;
}
.data-table th,
.data-table td {
  padding: 0.75rem 1rem;
  text-align: left;
  border-bottom: 1px solid #e2e8f0;
}
.data-table th {
  background: #1e293b;
  color: #f8fafc;
  font-weight: 600;
}
.data-table tbody tr:hover {
  background: #f8fafc;
}
.empty-row {
  text-align: center;
  color: #64748b;
  font-style: italic;
  padding: 2rem !important;
}
.record-count {
  padding: 1rem 1rem 0;
  color: #64748b;
  text-align: right;
  margin: 0;
  font-size: 0.875rem;
}
</style>

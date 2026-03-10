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
        <tr v-else v-for="(item, i) in items" :key="i">
          <td v-for="col in columns" :key="col.key">
            <slot :name="col.key" :item="item" :value="item[col.key]">
              {{ item[col.key] ?? '-' }}
            </slot>
          </td>
        </tr>
      </tbody>
    </table>
    <p v-if="items.length > 0" class="record-count">Total records: {{ items.length }}</p>
  </div>
</template>
<script>
/**
 * Reusable DataTable component.
 * Props:
 *   columns      Array<{ key: string, label: string }>   -  column definitions
 *   items        Array<Object>                           -  data rows
 *   emptyMessage string                                  -  shown when items is empty
 *
 * Usage:
 *   <DataTable :columns="cols" :items="rows" empty-message="No data">
 *     <template #listPrice="{ value }">{{ formatCurrency(value) }}</template>
 *   </DataTable>
 */
export default {
  name: 'DataTable',
  props: {
    columns:      { type: Array,  required: true },
    items:        { type: Array,  default: () => [] },
    emptyMessage: { type: String, default: 'No data available' }
  }
}
</script>
<style scoped>
.data-table-wrapper { background: white; border-radius: 8px; box-shadow: 0 2px 4px rgba(0,0,0,.1); overflow-x: auto; }
.data-table { width: 100%; border-collapse: collapse; }
.data-table th, .data-table td { padding: .75rem 1rem; text-align: left; border-bottom: 1px solid #eee; }
.data-table th { background: #2c3e50; color: white; }
.data-table tbody tr:hover { background: #f5f5f5; }
.empty-row { text-align: center; color: #999; font-style: italic; padding: 2rem !important; }
.record-count { padding: 1rem; color: #666; text-align: right; margin: 0; }
</style>
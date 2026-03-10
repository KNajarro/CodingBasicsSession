import { computed, onMounted, ref } from 'vue'
import { getDashboardData } from '../services/dashboardService'

const LOW_STOCK_THRESHOLD = 20

export function useDashboardData() {
  const loading = ref(false)
  const error = ref('')
  const inventoryValue = ref(0)
  const stockRows = ref([])
  const productsByColor = ref({})
  const peopleByType = ref({})

  const lowStockCount = computed(() =>
    stockRows.value.filter(row => row.stock <= LOW_STOCK_THRESHOLD).length
  )

  async function load() {
    loading.value = true
    error.value = ''

    try {
      const data = await getDashboardData()
      inventoryValue.value = data.inventoryValue
      stockRows.value = data.stockRows
      productsByColor.value = data.productsByColor
      peopleByType.value = data.peopleByType
    } catch (err) {
      error.value = err?.message || 'Failed to load dashboard data.'
    } finally {
      loading.value = false
    }
  }

  onMounted(load)

  return {
    loading,
    error,
    inventoryValue,
    stockRows,
    productsByColor,
    peopleByType,
    lowStockCount,
    lowStockThreshold: LOW_STOCK_THRESHOLD,
    reload: load
  }
}

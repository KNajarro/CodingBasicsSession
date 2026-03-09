import { ref } from 'vue'
import productsService from '../services/productsService'

/**
 * Composable para la vista de Products: datos, categorías, carga inicial y búsqueda.
 * Al montar el componente que lo use, carga categorías y lista inicial.
 */
export function useProducts() {
  const items = ref([])
  const categories = ref([])
  const loading = ref(false)
  const error = ref(null)

  async function loadCategories() {
    try {
      const data = await productsService.getCategories()
      categories.value = Array.isArray(data) ? data : []
    } catch (e) {
      console.error('Error loading product categories:', e)
      categories.value = []
    }
  }

  async function loadAll() {
    loading.value = true
    error.value = null
    try {
      const data = await productsService.getAll(1, 20)
      items.value = data?.items ?? []
    } catch (e) {
      error.value = e.response?.data?.message || e.message || 'Error al cargar productos'
      items.value = []
    } finally {
      loading.value = false
    }
  }

  async function search(name, categoryName) {
    loading.value = true
    error.value = null
    try {
      const data = await productsService.search(name || null, categoryName || null)
      items.value = Array.isArray(data) ? data : []
    } catch (e) {
      error.value = e.response?.data?.message || e.message || 'Error en la búsqueda'
      items.value = []
    } finally {
      loading.value = false
    }
  }

  /** Llamar al montar la vista para cargar categorías y lista por defecto */
  function init() {
    loadCategories()
    loadAll()
  }

  return {
    items,
    categories,
    loading,
    error,
    loadAll,
    search,
    loadCategories,
    init
  }
}

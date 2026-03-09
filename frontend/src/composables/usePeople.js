import { ref, onMounted } from 'vue'
import peopleService from '../services/peopleService'

/**
 * Composable para la vista de People: datos, tipos, carga inicial y búsqueda.
 * Al montar el componente que lo use, carga tipos y lista inicial.
 */
export function usePeople() {
  const items = ref([])
  const types = ref([])
  const loading = ref(false)
  const error = ref(null)

  async function loadTypes() {
    try {
      const data = await peopleService.getTypes()
      types.value = Array.isArray(data) ? data : []
    } catch (e) {
      console.error('Error loading people types:', e)
      types.value = []
    }
  }

  async function loadAll() {
    loading.value = true
    error.value = null
    try {
      const data = await peopleService.getAll(1, 20)
      items.value = data?.items ?? []
    } catch (e) {
      error.value = e.response?.data?.message || e.message || 'Error al cargar personas'
      items.value = []
    } finally {
      loading.value = false
    }
  }

  async function search(name, personType) {
    loading.value = true
    error.value = null
    try {
      const data = await peopleService.search(name || null, personType || null)
      items.value = Array.isArray(data) ? data : []
    } catch (e) {
      error.value = e.response?.data?.message || e.message || 'Error en la búsqueda'
      items.value = []
    } finally {
      loading.value = false
    }
  }

  /** Llamar al montar la vista para cargar tipos y lista por defecto */
  function init() {
    loadTypes()
    loadAll()
  }

  return {
    items,
    types,
    loading,
    error,
    loadAll,
    search,
    loadTypes,
    init
  }
}

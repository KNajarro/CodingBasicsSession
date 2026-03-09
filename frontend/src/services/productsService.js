import api from './api'

/**
 * Products Service — centraliza las peticiones al backend de Products.
 * La API devuelve { page, pageSize, totalCount, items } en getAll.
 * GET /api/products/categories devuelve un array de nombres de categoría.
 * GET /api/products/search devuelve un array de ProductDto.
 */
export const productsService = {
  /** GET /api/products — devuelve { page, pageSize, totalCount, items } */
  async getAll(page = 1, pageSize = 20) {
    const { data } = await api.get('/products', { params: { page, pageSize } })
    return data
  },

  /** GET /api/products/categories — devuelve array de categorías para el combobox */
  async getCategories() {
    const { data } = await api.get('/products/categories')
    return data
  },

  /** GET /api/products/search?name=&categoryName= — devuelve array de productos */
  async search(name, categoryName) {
    const params = {}
    if (name != null && name !== '') params.name = name
    if (categoryName != null && categoryName !== '') params.categoryName = categoryName
    const { data } = await api.get('/products/search', { params })
    return data
  }
}

export default productsService

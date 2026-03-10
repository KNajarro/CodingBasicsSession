import api from './api'
export const getProducts = () => api.get('/products');
/**
 * Products Service
 Implement full CRUD methods to call the .NET API.
 */
export const productsService = {
  /** GET /api/products - returns all ProductDto records */
  async getAll() {
          const response = await api.get('/products')
          return response.data
      },

  /** GET /api/products/search?name=&categoryName= */
  async search(name, categoryName) {
          const params = {}
          if (name)         params.name         = name
          if (categoryName) params.categoryName = categoryName
          const response = await api.get('/products/search', { params })
          return response.data
      },

  /** POST /api/products - create a new product */
  async create(product) {
          const response = await api.post('/products', product)
          return response.data
      },

  /** PUT /api/products/{id} - update a product */
  async update(id, product) {
          const response = await api.put(`/products/${id}`, product)
          return response.data
      },

  /** DELETE /api/products/{id} - delete a product */
  async delete(id) {
          await api.delete(`/products/${id}`)
      }
}

export default productsService
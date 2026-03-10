import api from './api'

export const productsService = {
  async getAll(pageSize = 1000) {
    try {
      const response = await api.get('/api/products', {
        params: {
          page: 1,
          pageSize: pageSize
        }
      })
      return response.data.items || []
    } catch (error) {
      console.error('Error fetching products:', error)
      throw error
    }
  },

  async search(name, categoryName) {
    try {
      const response = await api.get('/api/products/search', {
        params: {
          name,
          categoryName,
          page: 1,
          pageSize: 1000
        }
      })
      return response.data.items || []
    } catch (error) {
      console.error('Error searching products:', error)
      throw error
    }
  }
}

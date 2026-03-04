import api from './api'

/**
 * Products Service
 * TODO (Workshop): Implement the two methods below to call the .NET API.
 */
export const productsService = {
  /** GET /api/products  -  returns all ProductDto records */
  async getAll() {
    // TODO: const response = await api.get('/products')
    //       return response.data
    throw new Error('Workshop: Implement getAll()')
  },

  /** GET /api/products/search?name=&categoryName= */
  async search(name, categoryName) {
    // TODO: const params = {}
    //       if (name)         params.name         = name
    //       if (categoryName) params.categoryName = categoryName
    //       const response = await api.get('/products/search', { params })
    //       return response.data
    throw new Error('Workshop: Implement search()')
  }
}

export default productsService
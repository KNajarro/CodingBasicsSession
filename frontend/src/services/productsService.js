import api from './api'

/**
 * Products Service
 * Implements full CRUD methods to call the .NET API endpoints for Product management.
 * Handles all interactions with the AdventureWorks API for products data.
 */
export const productsService = {
  /**
   * GET /api/products - Retrieves all products with pagination
   * @param {number} page - Page number (default 1)
   * @param {number} pageSize - Number of items per page (default 20)
   * @returns {Promise<Array>} Array of ProductDto records from the items property
   */
  async getAll(page = 1, pageSize = 20) {
    const response = await api.get('/products', {
      params: { page, pageSize }
    })
    return response.data.items || []
  },

  /**
   * GET /api/products/by-name/{name} or /api/products/by-category/{categoryName}
   * Searches for products by name, category, or both
   * @param {string|null} name - Optional product name to search for
   * @param {string|null} categoryName - Optional category name to filter by
   * @param {number} page - Page number (default 1)
   * @param {number} pageSize - Number of items per page (default 20)
   * @returns {Promise<Array>} Array of matching ProductDto records
   */
  async search(name = null, categoryName = null, page = 1, pageSize = 20) {
    const params = { page, pageSize }

    // Use specific endpoints based on provided parameters
    if (name && categoryName) {
      // If both are provided, search by name and filter by category on the client
      // Note: API doesn't have a combined endpoint, so we fetch by name and filter client-side
      const response = await api.get(`/products/by-name/${encodeURIComponent(name)}`, { params })
      const items = response.data.items || []
      return items.filter(p => p.categoryName && p.categoryName.toLowerCase() === categoryName.toLowerCase())
    } else if (name) {
      // Only name provided
      const response = await api.get(`/products/by-name/${encodeURIComponent(name)}`, { params })
      return response.data.items || []
    } else if (categoryName) {
      // Only category provided
      const response = await api.get(`/products/by-category/${encodeURIComponent(categoryName)}`, { params })
      return response.data.items || []
    } else {
      // Neither provided, get all products
      const response = await api.get('/products', { params })
      return response.data.items || []
    }
  },

  /**
   * GET /api/products/by-name/{name} - Retrieves products by their name
   * @param {string} name - The product name to search for
   * @param {number} page - Page number (default 1)
   * @param {number} pageSize - Number of items per page (default 20)
   * @returns {Promise<Array>} Array of matching ProductDto records
   */
  async getProductByName(name, page = 1, pageSize = 20) {
    const response = await api.get(`/products/by-name/${encodeURIComponent(name)}`, {
      params: { page, pageSize }
    })
    return response.data.items || []
  },

  /**
   * GET /api/products/by-category/{categoryName} - Retrieves products by their category
   * @param {string} categoryName - The category name (Bikes, Components, Clothing, Accessories)
   * @param {number} page - Page number (default 1)
   * @param {number} pageSize - Number of items per page (default 20)
   * @returns {Promise<Array>} Array of matching ProductDto records
   */
  async getProductsByCategoryType(categoryName, page = 1, pageSize = 20) {
    const response = await api.get(`/products/by-category/${encodeURIComponent(categoryName)}`, {
      params: { page, pageSize }
    })
    return response.data.items || []
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
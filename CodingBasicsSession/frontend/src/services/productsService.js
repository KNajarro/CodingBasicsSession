import api from './api'

/**
 * Products Service
 * TODO (Workshop): Implement full CRUD methods to call the .NET API.
 */
export const productsService = {
  /** GET /api/products - returns all ProductDto records */
  async getAll(page = 1, pageSize = 20) {
    const response = await api.get('/products', { params: { page, pageSize } });
    return response.data; // { page, pageSize, totalCount, items }
  },

  async search(name, categoryName) {
    const params = {};
    if (name) params.name = name;
    if (categoryName) params.categoryName = categoryName;
    const response = await api.get('/products/search', { params });
    return response.data;
  },

  async create(product) {
    const response = await api.post('/products', product);
    return response.data;
  },

  async update(id, product) {
    const response = await api.put(`/products/${id}`, product);
    return response.data;
  },

  async delete(id) {
    await api.delete(`/products/${id}`);
  }
}

export default productsService
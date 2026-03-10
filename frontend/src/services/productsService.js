import api from './api'

export const productsService = {
  async getAll(page = 1, pageSize = 20) {
    const res = await api.get('/products', { params: { page, pageSize } })
    return res.data // { page, pageSize, totalCount, items }
  },

  async search(name, categoryName) {
    const params = {}
    if (name)         params.name         = name
    if (categoryName) params.categoryName = categoryName
    const res = await api.get('/products/search', { params })
    return res.data // ProductDto[]
  },

  async create(product) {
    const res = await api.post('/products', product)
    return res.data
  },

  async update(id, product) {
    const res = await api.put(`/products/${id}`, product)
    return res.data
  },

  async delete(id) {
    await api.delete(`/products/${id}`)
  }
}

export default productsService

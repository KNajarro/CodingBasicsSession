import api from './api'

export const productsService = {
  async getAll(page = 1, pageSize = 500) {
    const { data } = await api.get('/products', { params: { page, pageSize } })
    return data.items || data
  }
}

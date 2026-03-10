import api from './api'

export const peopleService = {
  async getAll(page = 1, pageSize = 500) {
    const { data } = await api.get('/people', { params: { page, pageSize } })
    return data.items || data
  }
}

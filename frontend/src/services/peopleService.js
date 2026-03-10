import api from './api'

export const peopleService = {
  async getAll(page = 1, pageSize = 20) {
    const res = await api.get('/people', { params: { page, pageSize } })
    return res.data // { page, pageSize, totalCount, items }
  },

  async search(name, personType) {
    const params = {}
    if (name)       params.name       = name
    if (personType) params.personType = personType
    const res = await api.get('/people/search', { params })
    return res.data // PersonDto[]
  },

  async create(person) {
    const res = await api.post('/people', person)
    return res.data
  },

  async update(id, person) {
    const res = await api.put(`/people/${id}`, person)
    return res.data
  },

  async delete(id) {
    await api.delete(`/people/${id}`)
  }
}

export default peopleService

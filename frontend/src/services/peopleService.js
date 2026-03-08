import api from './api'

/**
 * People Service - Full CRUD implementation
 */
export const peopleService = {
  /** GET /api/people - returns all PersonDto records */
  async getAll(page = 1, pageSize = 100) {
    const response = await api.get('/people', { params: { page, pageSize } })
    return response.data.items || response.data
  },

  /** GET /api/people/search?name=&personType= */
  async search(name, personType) {
    const params = {}
    if (name) params.name = name
    if (personType) params.personType = personType
    const response = await api.get('/people/search', { params })
    return response.data
  },

  /** POST /api/people - create a new person */
  async create(person) {
    const response = await api.post('/people', person)
    return response.data
  },

  /** PUT /api/people/{id} - update a person */
  async update(id, person) {
    const response = await api.put(`/people/${id}`, person)
    return response.data
  },

  /** DELETE /api/people/{id} - delete a person */
  async delete(id) {
    await api.delete(`/people/${id}`)
  }
}

export default peopleService
import api from './api'

/**
 * People Service
 * TODO (Workshop): Implement full CRUD methods to call the .NET API.
 */
export const peopleService = {
  /** GET /api/people - returns all PersonDto records */
  async getAll() {
    // TODO: const response = await api.get('/people')
    //       return response.data
    throw new Error('Workshop: Implement getAll()')
  },

  /** GET /api/people/search?name=&personType= */
  async search(name, personType) {
    // TODO: const params = {}
    //       if (name)       params.name       = name
    //       if (personType) params.personType = personType
    //       const response = await api.get('/people/search', { params })
    //       return response.data
    throw new Error('Workshop: Implement search()')
  },

  /** POST /api/people - create a new person */
  async create(person) {
    // TODO: const response = await api.post('/people', person)
    //       return response.data
    throw new Error('Workshop: Implement create()')
  },

  /** PUT /api/people/{id} - update a person */
  async update(id, person) {
    // TODO: const response = await api.put(`/people/${id}`, person)
    //       return response.data
    throw new Error('Workshop: Implement update()')
  },

  /** DELETE /api/people/{id} - delete a person */
  async delete(id) {
    // TODO: await api.delete(`/people/${id}`)
    throw new Error('Workshop: Implement delete()')
  }
}

export default peopleService
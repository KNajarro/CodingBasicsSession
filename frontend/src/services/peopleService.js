import api from './api'

/**
 * People Service
 * TODO (Workshop): Implement the two methods below to call the .NET API.
 */
export const peopleService = {
  /** GET /api/people  -  returns all PersonDto records */
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
  }
}

export default peopleService
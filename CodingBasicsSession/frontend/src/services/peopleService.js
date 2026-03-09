import api from './api'

/**
 * People Service
 * TODO (Workshop): Implement full CRUD methods to call the .NET API.
 */
export const peopleService = {
  /** GET /api/people - returns all PersonDto records */
  async getAll(page = 1, pageSize = 20) {
    const response = await api.get('/people', { params: { page, pageSize } });
    return response.data; // { page, pageSize, totalCount, items }
  },

  /** GET /api/people/search?name=&personType= */
  async search(name, personType) {
    const params = {};
    if (name) params.name = name;
    if (personType) params.personType = personType;
    const response = await api.get('/people/search', { params });
    return response.data;
  },

  async create(person) {
    const response = await api.post('/people', person);
    return response.data;
  },

  async update(id, person) {
    const response = await api.put(`/people/${id}`, person);
    return response.data;
  },

  async delete(id) {
    await api.delete(`/people/${id}`);
  }
}

export default peopleService
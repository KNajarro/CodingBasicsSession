import api from './api'

/**
 * People Service — centraliza las peticiones al backend de People.
 * La API devuelve { page, pageSize, totalCount, items } en getAll.
 * GET /api/people/types devuelve un array de strings.
 * GET /api/people/search devuelve un array de PersonDto.
 */
export const peopleService = {
  /** GET /api/people — devuelve { page, pageSize, totalCount, items } */
  async getAll(page = 1, pageSize = 20) {
    const { data } = await api.get('/people', { params: { page, pageSize } })
    return data
  },

  /** GET /api/people/types — devuelve array de tipos para el combobox */
  async getTypes() {
    const { data } = await api.get('/people/types')
    return data
  },

  /** GET /api/people/search?name=&personType= — devuelve array de personas */
  async search(name, personType) {
    const params = {}
    if (name != null && name !== '') params.name = name
    if (personType != null && personType !== '') params.personType = personType
    const { data } = await api.get('/people/search', { params })
    return data
  }
}

export default peopleService

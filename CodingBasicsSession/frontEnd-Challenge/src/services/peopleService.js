import api from './api'

export const peopleService = {
  async getAll(pageSize = 1000) {
    try {
      const response = await api.get('/api/people', {
        params: {
          page: 1,
          pageSize: pageSize
        }
      })
      return response.data.items || []
    } catch (error) {
      console.error('Error fetching people:', error)
      throw error
    }
  },

  async search(name, personType) {
    try {
      const response = await api.get('/api/people/search', {
        params: {
          name,
          personType,
          page: 1,
          pageSize: 1000
        }
      })
      return response.data.items || []
    } catch (error) {
      console.error('Error searching people:', error)
      throw error
    }
  }
}

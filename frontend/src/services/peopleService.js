import api from './api'

/**
 * People Service
 * Implements full CRUD methods to call the .NET API endpoints for Person management.
 * Handles all interactions with the AdventureWorks API for people data.
 */
export const peopleService = {
  /**
   * GET /api/people - Retrieves all people with pagination
   * @param {number} page - Page number (default 1)
   * @param {number} pageSize - Number of items per page (default 20)
   * @returns {Promise<Array>} Array of PersonDto records from the items property
   */
  async getAll(page = 1, pageSize = 20) {
    const response = await api.get('/people', {
      params: { page, pageSize }
    })
    return response.data.items || []
  },

  /**
   * GET /api/people/search or by-name or by-type or by-name-and-type
   * Searches for people by name, personType, or both
   * @param {string|null} name - Optional name to search for
   * @param {string|null} personType - Optional person type to filter by
   * @param {number} page - Page number (default 1)
   * @param {number} pageSize - Number of items per page (default 20)
   * @returns {Promise<Array>} Array of matching PersonDto records
   */
  async search(name = null, personType = null, page = 1, pageSize = 20) {
    // Determine which endpoint to use based on provided parameters
    let endpoint = '/people/search'
    const params = { page, pageSize }

    // Use specific endpoints for better performance when possible
    if (name && personType) {
      // Both name and type provided
      endpoint = `/people/by-name-and-type/${encodeURIComponent(name)}/${encodeURIComponent(personType)}`
    } else if (name) {
      // Only name provided
      endpoint = `/people/by-name/${encodeURIComponent(name)}`
    } else if (personType) {
      // Only person type provided
      endpoint = `/people/by-type/${encodeURIComponent(personType)}`
    } else {
      // Neither provided, use general search
      endpoint = '/people/search'
    }

    const response = await api.get(endpoint, { params })
    return response.data.items || []
  },

  /**
   * GET /api/people/by-name/{name} - Retrieves people by their name
   * @param {string} name - The name to search for
   * @param {number} page - Page number (default 1)
   * @param {number} pageSize - Number of items per page (default 20)
   * @returns {Promise<Array>} Array of matching PersonDto records
   */
  async getPersonByName(name, page = 1, pageSize = 20) {
    const response = await api.get(`/people/by-name/${encodeURIComponent(name)}`, {
      params: { page, pageSize }
    })
    return response.data.items || []
  },

  /**
   * GET /api/people/by-type/{personType} - Retrieves people by their person type
   * @param {string} personType - The person type code (SC, IN, SP, EM, VC, GC)
   * @param {number} page - Page number (default 1)
   * @param {number} pageSize - Number of items per page (default 20)
   * @returns {Promise<Array>} Array of matching PersonDto records
   */
  async getPersonByPersonType(personType, page = 1, pageSize = 20) {
    const response = await api.get(`/people/by-type/${encodeURIComponent(personType)}`, {
      params: { page, pageSize }
    })
    return response.data.items || []
  },

  /**
   * GET /api/people/by-name-and-type/{name}/{personType} - Retrieves people by both name and type
   * @param {string} name - The name to search for
   * @param {string} personType - The person type code
   * @param {number} page - Page number (default 1)
   * @param {number} pageSize - Number of items per page (default 20)
   * @returns {Promise<Array>} Array of matching PersonDto records
   */
  async getPersonByNameAndPersonType(name, personType, page = 1, pageSize = 20) {
    const response = await api.get(
      `/people/by-name-and-type/${encodeURIComponent(name)}/${encodeURIComponent(personType)}`,
      { params: { page, pageSize } }
    )
    return response.data.items || []
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
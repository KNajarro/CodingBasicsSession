import api from "./api";

/**
 * People Service
 * TODO (Workshop): Implement full CRUD methods to call the .NET API.
 */
export const peopleService = {
  /** GET /api/people - returns all PersonDto records */
  async getAll(page = 1, pageSize = 20) {
    const response = await api.get("/people", {
      params: { page, pageSize },
    });
    return response.data;
  },

  async search(name, personType) {
    const params = {};

    if (name && name.trim()) {
      params.name = name.trim();
    }

    if (personType && personType.trim()) {
      params.personType = personType.trim();
    }

    const response = await api.get("/people/search", { params });
    return response.data;
  },

  /** POST /api/people - create a new person */
  async create(person) {
    // TODO: const response = await api.post('/people', person)
    //       return response.data
    throw new Error("Workshop: Implement create()");
  },

  /** PUT /api/people/{id} - update a person */
  async update(id, person) {
    // TODO: const response = await api.put(`/people/${id}`, person)
    //       return response.data
    throw new Error("Workshop: Implement update()");
  },

  /** DELETE /api/people/{id} - delete a person */
  async delete(id) {
    // TODO: await api.delete(`/people/${id}`)
    throw new Error("Workshop: Implement delete()");
  },
};

export default peopleService;

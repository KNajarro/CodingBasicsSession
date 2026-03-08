import api from "./api";

/**
 * People Service
 */
export const peopleService = {
  /** GET /api/people?page=&pageSize= */
  async getAll(page = 1, pageSize = 20) {
    const response = await api.get("/people", {
      params: { page, pageSize },
    });
    return response.data;
  },

  /** GET /api/people/search?name=&personType= */
  async search(name, personType) {
    const params = {};
    if (name) params.name = name;
    if (personType) params.personType = personType;

    const response = await api.get("/people/search", { params });
    return response.data;
  },

  /** POST /api/people - skipped for now */
  async create() {
    throw new Error("Create person is skipped for now.");
  },

  /** PUT /api/people/{id} */
  async update(id, person) {
    const response = await api.put(`/people/${id}`, person);
    return response.data;
  },

  /** DELETE /api/people/{id} */
  async delete(id) {
    await api.delete(`/people/${id}`);
  },
};

export default peopleService;

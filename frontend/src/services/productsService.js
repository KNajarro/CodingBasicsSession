import api from "./api";

/**
 * Products Service
 * TODO (Workshop): Implement full CRUD methods to call the .NET API.
 */
export const productsService = {
  /** GET /api/products - returns all ProductDto records */
  async getAll() {
    const response = await api.get("/products");
    return response.data;
  },

  async search(name, categoryName) {
    const params = {};

    if (name && name.trim()) {
      params.name = name.trim();
    }

    if (categoryName && categoryName.trim()) {
      params.categoryName = categoryName.trim();
    }

    const response = await api.get("/products/search", { params });
    return response.data;
  },

  /** POST /api/products - create a new product */
  async create(product) {
    // TODO: const response = await api.post('/products', product)
    //       return response.data
    throw new Error("Workshop: Implement create()");
  },

  /** PUT /api/products/{id} - update a product */
  async update(id, product) {
    // TODO: const response = await api.put(`/products/${id}`, product)
    //       return response.data
    throw new Error("Workshop: Implement update()");
  },

  /** DELETE /api/products/{id} - delete a product */
  async delete(id) {
    // TODO: await api.delete(`/products/${id}`)
    throw new Error("Workshop: Implement delete()");
  },
};

export default productsService;

import api from "./api"

/**
 * Dashboard Service
 * Consumes dashboard endpoints from the .NET API
 */
export const dashboardService = {

  /** GET /api/dashboard/inventory-value */
  async getInventoryValue() {
    const response = await api.get("/dashboard/inventory-value")
    return response.data
  },

  /** GET /api/dashboard/products-by-color */
  async getProductsByColor() {
    const response = await api.get("/dashboard/products-by-color")
    return response.data
  },

  /** GET /api/dashboard/people-by-type */
  async getPeopleByType() {
    const response = await api.get("/dashboard/people-by-type")
    return response.data
  },

  /** GET /api/dashboard/low-stock */
  async getLowStockProducts() {
    const response = await api.get("/dashboard/low-stock")
    return response.data
  }

}

export default dashboardService
import api from './api'

export const dashboardService = {
  /** GET /api/dashboard/inventory-value → { totalInventoryValue: number } */
  getInventoryValue: (ct) =>
    api.get('/dashboard/inventory-value', { signal: ct }),

  /** GET /api/dashboard/color-distribution → ColorCountDto[] */
  getColorDistribution: (ct) =>
    api.get('/dashboard/color-distribution', { signal: ct }),

  /** GET /api/dashboard/customer-analysis → PersonTypeCountDto[] */
  getCustomerAnalysis: (ct) =>
    api.get('/dashboard/customer-analysis', { signal: ct }),

  /** GET /api/dashboard/low-stock → PagedResult<LowStockProductDto> */
  getLowStock: (page = 1, threshold = 100, ct) =>
    api.get('/dashboard/low-stock', { params: { page, threshold }, signal: ct })
}

<template>
  <div class="products-view">
    <h2>Products</h2>
    <div class="search-form">
      <div class="form-group">
        <label>Product Name:</label>
        <input
          type="text"
          v-model="searchName"
          placeholder="Search by name..."
        />
      </div>
      <div class="form-group">
        <label>Category:</label>
        <select v-model="searchCategory">
          <option value="">All Categories</option>
          <option value="Bikes">Bikes</option>
          <option value="Components">Components</option>
          <option value="Clothing">Clothing</option>
          <option value="Accessories">Accessories</option>
        </select>
      </div>
      <button @click="handleSearch" class="btn btn-primary">Search</button>
      <button @click="handleLoadAll" class="btn btn-secondary">Load All</button>
    </div>
    <div v-if="error" class="error-message">{{ error }}</div>
    <div v-if="loading" class="loading">Loading...</div>
    <div v-else-if="products.length > 0" class="table-container">
      <table class="data-table">
        <thead>
          <tr>
            <th>ID</th>
            <th>Name</th>
            <th>Number</th>
            <th>Color</th>
            <th>Price</th>
            <th>Category</th>
            <th>Subcategory</th>
          </tr>
        </thead>
         <tbody>
           <tr v-for="product in products" :key="product.productID">
             <td>{{ product.productID }}</td>
             <td>{{ product.name }}</td>
             <td>{{ product.productNumber }}</td>
             <td>{{ product.color || '-' }}</td>
             <td>{{ formatPrice(product.listPrice) }}</td>
             <td>{{ product.categoryName || '-' }}</td>
             <td>{{ product.subcategoryName || '-' }}</td>
           </tr>
         </tbody>
       </table>
       <div class="pagination-info">
         <div class="info-text">
           <span>Page <strong>{{ currentPage }}</strong> of <strong>{{ totalPages }}</strong></span>
           <span class="separator">•</span>
           <span>Showing <strong>{{ products.length }}</strong> of <strong>{{ totalCount }}</strong> records</span>
         </div>
         <div class="pagination-controls">
           <button 
             @click="previousPage" 
             class="btn btn-pagination"
             :disabled="currentPage === 1 || loading"
           >
             ← Previous
           </button>
           <div class="page-size-selector">
             <label>Page Size:</label>
             <select v-model.number="pageSize" @change="resetAndLoad" :disabled="loading">
               <option value="10">10</option>
               <option value="20">20</option>
               <option value="50">50</option>
               <option value="100">100</option>
             </select>
           </div>
           <button 
             @click="nextPage" 
             class="btn btn-pagination"
             :disabled="currentPage >= totalPages || loading"
           >
             Next →
           </button>
         </div>
       </div>
    </div>
    <div v-else class="empty-state">
      <p>No data. Click Load All or Search.</p>
    </div>
    <!-- TODO (Workshop): Add UI and methods for Create, Update, and Delete product records -->
  </div>
</template>
<script>
import productsService from "../services/productsService";
export default {
  name: "ProductsView",
  data() {
    return {
      products: [],
      searchName: "",
      searchCategory: "",
      loading: false,
      error: null,
      currentPage: 1,
      pageSize: 20,
      totalCount: 0,
      isSearchActive: false,
    };
  },
  computed: {
    totalPages() {
      return Math.ceil(this.totalCount / this.pageSize) || 1;
    },
  },
  methods: {
    formatPrice(p) {
      return new Intl.NumberFormat("en-US", {
        style: "currency",
        currency: "USD",
      }).format(p);
    },
    async loadProductsData() {
      this.loading = true;
      this.error = null;
      try {
        let response;
        if (this.isSearchActive) {
          // Apply search with current filters and pagination
          response = await productsService.searchWithPagination(
            this.searchName || null,
            this.searchCategory || null,
            this.currentPage,
            this.pageSize
          );
        } else {
          // Load all with pagination
          response = await productsService.getAllWithPagination(this.currentPage, this.pageSize);
        }
        this.products = response.items;
        this.totalCount = response.totalCount;
      } catch (err) {
        this.error = `Error loading products: ${err.message}`;
        this.products = [];
        this.totalCount = 0;
      } finally {
        this.loading = false;
      }
    },
    async handleLoadAll() {
      this.isSearchActive = false;
      this.currentPage = 1;
      await this.loadProductsData();
    },
    async handleSearch() {
      this.isSearchActive = true;
      this.currentPage = 1;
      await this.loadProductsData();
    },
    async nextPage() {
      if (this.currentPage < this.totalPages) {
        this.currentPage++;
        await this.loadProductsData();
        this.scrollToTop();
      }
    },
    async previousPage() {
      if (this.currentPage > 1) {
        this.currentPage--;
        await this.loadProductsData();
        this.scrollToTop();
      }
    },
    async resetAndLoad() {
      this.currentPage = 1;
      await this.loadProductsData();
    },
    scrollToTop() {
      window.scrollTo({ top: 0, behavior: 'smooth' });
    },
  },
};
</script>
<style scoped>
h2 {
  margin-bottom: 1.5rem;
  color: #2c3e50;
}
.search-form {
  display: flex;
  gap: 1rem;
  align-items: flex-end;
  flex-wrap: wrap;
  margin-bottom: 1.5rem;
  padding: 1rem;
  background: white;
  border-radius: 8px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
}
.form-group {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}
.form-group label {
  font-weight: 600;
  color: #555;
}
.form-group input,
.form-group select {
  padding: 0.5rem;
  border: 1px solid #ddd;
  border-radius: 4px;
  font-size: 1rem;
  min-width: 200px;
}
.btn {
  padding: 0.5rem 1rem;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-size: 1rem;
}
.btn-primary {
  background: #3498db;
  color: white;
}
.btn-primary:hover {
  background: #2980b9;
}
.btn-secondary {
  background: #95a5a6;
  color: white;
}
.btn-secondary:hover {
  background: #7f8c8d;
}
.error-message {
  background: #fee;
  color: #c00;
  padding: 1rem;
  border-radius: 4px;
  margin-bottom: 1rem;
}
.loading {
  text-align: center;
  padding: 2rem;
  color: #666;
}
.table-container {
  background: white;
  border-radius: 8px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  overflow-x: auto;
}
.data-table {
  width: 100%;
  border-collapse: collapse;
}
.data-table th,
.data-table td {
  padding: 0.75rem 1rem;
  text-align: left;
  border-bottom: 1px solid #eee;
}
.data-table th {
  background: #2c3e50;
  color: white;
}
.data-table tbody tr:hover {
  background: #f5f5f5;
}
.placeholder {
  text-align: center;
  color: #999;
  font-style: italic;
  padding: 2rem !important;
}
.record-count {
  padding: 1rem;
  color: #666;
  text-align: right;
}
.empty-state {
  text-align: center;
  padding: 3rem;
  color: #666;
  background: white;
  border-radius: 8px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
}
.pagination-info {
  background: white;
  padding: 1rem;
  border-radius: 8px;
  margin-top: 1rem;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  display: flex;
  justify-content: space-between;
  align-items: center;
  flex-wrap: wrap;
  gap: 1.5rem;
}
.info-text {
  display: flex;
  gap: 1rem;
  align-items: center;
  color: #666;
  font-size: 0.95rem;
}
.separator {
  color: #ccc;
}
.pagination-controls {
  display: flex;
  gap: 1rem;
  align-items: center;
  flex-wrap: wrap;
}
.btn-pagination {
  padding: 0.5rem 1rem;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-size: 0.95rem;
  background: #3498db;
  color: white;
  transition: background 0.3s ease;
}
.btn-pagination:hover:not(:disabled) {
  background: #2980b9;
}
.btn-pagination:disabled {
  background: #bdc3c7;
  cursor: not-allowed;
  opacity: 0.6;
}
.page-size-selector {
  display: flex;
  align-items: center;
  gap: 0.5rem;
}
.page-size-selector label {
  font-weight: 600;
  color: #555;
  white-space: nowrap;
}
.page-size-selector select {
  padding: 0.5rem;
  border: 1px solid #ddd;
  border-radius: 4px;
  font-size: 0.95rem;
  cursor: pointer;
}
.page-size-selector select:disabled {
  background: #f5f5f5;
  cursor: not-allowed;
  opacity: 0.6;
}
</style>

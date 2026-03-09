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
      <p class="record-count">Total: {{ totalCount }} | Page {{ currentPage }} of {{ totalPages }}</p>
      <div class="pagination" v-if="!isSearchMode">
        <button @click="goToPage(1)" :disabled="currentPage === 1" class="btn btn-page">«</button>
        <button @click="goToPage(currentPage - 1)" :disabled="currentPage === 1" class="btn btn-page">‹</button>
        <span class="page-info">{{ currentPage }} / {{ totalPages }}</span>
        <button @click="goToPage(currentPage + 1)" :disabled="currentPage === totalPages" class="btn btn-page">›</button>
        <button @click="goToPage(totalPages)" :disabled="currentPage === totalPages" class="btn btn-page">»</button>
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
      isSearchMode: false,
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
    async handleLoadAll() {
      this.isSearchMode = false;
      this.currentPage = 1;
      await this.fetchPage();
    },
    async fetchPage() {
      this.loading = true;
      this.error = null;
      try {
        if (this.isSearchMode) {
          const result = await productsService.search(
            this.searchName || null,
            this.searchCategory || null
          );
          this.products = result;
          this.totalCount = result.length;
        } else {
          const result = await productsService.getAll(this.currentPage, this.pageSize);
          this.products = result.items;
          this.totalCount = result.totalCount;
        }
      } catch (err) {
        this.error = err.message;
      } finally {
        this.loading = false;
      }
    },
    async handleSearch() {
      this.isSearchMode = true;
      this.currentPage = 1;
      await this.fetchPage();
    },
    async goToPage(page) {
      if (page < 1 || page > this.totalPages) return;
      this.currentPage = page;
      await this.fetchPage();
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
.pagination {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 0.5rem;
  padding: 1rem;
  border-top: 1px solid #eee;
}
.btn-page {
  background: #ecf0f1;
  color: #2c3e50;
  min-width: 2rem;
  font-size: 1rem;
}
.btn-page:hover:not(:disabled) {
  background: #3498db;
  color: white;
}
.btn-page:disabled {
  opacity: 0.4;
  cursor: not-allowed;
}
.page-info {
  font-weight: 600;
  color: #2c3e50;
  min-width: 5rem;
  text-align: center;
}
.empty-state {
  text-align: center;
  padding: 3rem;
  color: #666;
  background: white;
  border-radius: 8px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
}
</style>

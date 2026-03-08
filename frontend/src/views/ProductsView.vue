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
      <p class="record-count">Total: {{ products.length }}</p>
    </div>

    <div v-else class="empty-state">
      <p>No data. Click Load All or Search.</p>
    </div>
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
    };
  },
  methods: {
    formatPrice(p) {
      return new Intl.NumberFormat("en-US", {
        style: "currency",
        currency: "USD",
      }).format(p);
    },

    async handleLoadAll() {
      this.loading = true;
      this.error = null;

      try {
        const response = await productsService.getAll();
        this.products = response || [];
      } catch (err) {
        this.error =
          err.response?.data?.message ||
          err.message ||
          "Error loading products";
      } finally {
        this.loading = false;
      }
    },

    async handleSearch() {
      this.loading = true;
      this.error = null;

      try {
        const response = await productsService.search(
          this.searchName || null,
          this.searchCategory || null
        );
        this.products = response || [];
      } catch (err) {
        this.error =
          err.response?.data?.message ||
          err.message ||
          "Error searching products";
      } finally {
        this.loading = false;
      }
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
</style>
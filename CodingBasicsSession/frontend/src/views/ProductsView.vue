<template>
  <div class="products-view">
    <h2>Products</h2>

    <div class="search-form">
      <div class="form-group">
        <label>Product Name:</label>
        <input type="text" v-model="searchName" placeholder="Search by name..." />
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

    <div class="crud-form">
      <h3>{{ editingId ? 'Edit Product' : 'Create Product' }}</h3>
      <div class="form-grid">
        <div class="form-group">
          <label>Name:</label>
          <input type="text" v-model="form.name" />
        </div>
        <div class="form-group">
          <label>Product Number:</label>
          <input type="text" v-model="form.productNumber" />
        </div>
        <div class="form-group">
          <label>Color:</label>
          <input type="text" v-model="form.color" />
        </div>
        <div class="form-group">
          <label>List Price:</label>
          <input type="number" min="0" step="0.01" v-model.number="form.listPrice" />
        </div>
      </div>
      <div class="actions-row">
        <button @click="handleSave" class="btn btn-primary">{{ editingId ? 'Update' : 'Create' }}</button>
        <button v-if="editingId" @click="resetForm" class="btn btn-secondary">Cancel</button>
      </div>
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
            <th>Actions</th>
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
            <td>
              <button @click="editProduct(product)" class="btn btn-sm btn-primary">Edit</button>
              <button @click="deleteProduct(product.productID)" class="btn btn-sm btn-danger">Delete</button>
            </td>
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
      editingId: null,
      form: {
        name: "",
        productNumber: "",
        color: "",
        listPrice: 0,
      },
    };
  },
  methods: {
    formatPrice(value) {
      return new Intl.NumberFormat("en-US", {
        style: "currency",
        currency: "USD",
      }).format(Number(value || 0));
    },
    mapError(err) {
      return err?.friendlyMessage || err?.response?.data?.message || err?.response?.data?.title || err?.message || "Request failed";
    },
    normalizeProductPayload() {
      return {
        name: this.form.name,
        productNumber: this.form.productNumber,
        color: this.form.color || null,
        listPrice: Number(this.form.listPrice || 0),
      };
    },
    resetForm() {
      this.editingId = null;
      this.form = {
        name: "",
        productNumber: "",
        color: "",
        listPrice: 0,
      };
    },
    async refreshCurrentList() {
      if (this.searchName || this.searchCategory) {
        this.products = await productsService.search(this.searchName || null, this.searchCategory || null);
        return;
      }
      this.products = await productsService.getAll();
    },
    async handleLoadAll() {
      this.loading = true;
      this.error = null;
      try {
        this.products = await productsService.getAll();
      } catch (err) {
        this.error = this.mapError(err);
      } finally {
        this.loading = false;
      }
    },
    async handleSearch() {
      this.loading = true;
      this.error = null;
      try {
        this.products = await productsService.search(this.searchName || null, this.searchCategory || null);
      } catch (err) {
        this.error = this.mapError(err);
      } finally {
        this.loading = false;
      }
    },
    editProduct(product) {
      this.editingId = product.productID;
      this.form = {
        name: product.name || "",
        productNumber: product.productNumber || "",
        color: product.color || "",
        listPrice: Number(product.listPrice || 0),
      };
    },
    async handleSave() {
      if (!this.form.name || !this.form.productNumber) {
        this.error = "Name y Product Number son obligatorios.";
        return;
      }

      this.loading = true;
      this.error = null;
      try {
        const payload = this.normalizeProductPayload();
        if (this.editingId) {
          await productsService.update(this.editingId, payload);
        } else {
          await productsService.create(payload);
        }
        this.resetForm();
        await this.refreshCurrentList();
      } catch (err) {
        this.error = this.mapError(err);
      } finally {
        this.loading = false;
      }
    },
    async deleteProduct(id) {
      if (!confirm(`Delete product ${id}?`)) return;

      this.loading = true;
      this.error = null;
      try {
        await productsService.delete(id);
        await this.refreshCurrentList();
      } catch (err) {
        this.error = this.mapError(err);
      } finally {
        this.loading = false;
      }
    },
  },
  mounted() {
    this.handleLoadAll();
  },
};
</script>

<style scoped>
h2 {
  margin-bottom: 1.5rem;
  color: #2c3e50;
}
.search-form,
.crud-form {
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
.crud-form {
  display: block;
}
.crud-form h3 {
  margin: 0 0 1rem;
  color: #2c3e50;
}
.form-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
  gap: 1rem;
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
  min-width: 160px;
}
.actions-row {
  margin-top: 1rem;
  display: flex;
  gap: 0.75rem;
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
.btn-danger {
  background: #e74c3c;
  color: white;
}
.btn-danger:hover {
  background: #c0392b;
}
.btn-sm {
  padding: 0.35rem 0.65rem;
  font-size: 0.85rem;
  margin-right: 0.35rem;
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

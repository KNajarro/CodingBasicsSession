<template>
  <div class="products-view">
    <h2>Products Management</h2>
    
    <!-- Search and Actions Bar -->
    <div class="search-form">
      <div class="form-group">
        <label>Product Name:</label>
        <input
          type="text"
          v-model="searchName"
          placeholder="Search by name..."
          @keyup.enter="handleSearch"
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
      <button @click="openCreateModal" class="btn btn-success">+ New Product</button>
    </div>

    <!-- Messages -->
    <div v-if="error" class="error-message">{{ error }}</div>
    <div v-if="success" class="success-message">{{ success }}</div>
    <div v-if="loading" class="loading">Loading...</div>

    <!-- Data Table -->
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
            <td class="actions">
              <button @click="openEditModal(product)" class="btn-action btn-edit" title="Edit">✏️</button>
              <button @click="confirmDelete(product)" class="btn-action btn-delete" title="Delete">🗑️</button>
            </td>
          </tr>
        </tbody>
      </table>
      <p class="record-count">Total: {{ products.length }} records</p>
    </div>

    <!-- Empty State -->
    <div v-else class="empty-state">
      <p>No data. Click Load All or Search.</p>
    </div>

    <!-- Create/Edit Modal -->
    <div v-if="showModal" class="modal-overlay" @click.self="closeModal">
      <div class="modal">
        <h3>{{ isEditing ? 'Edit Product' : 'Create New Product' }}</h3>
        <form @submit.prevent="handleSave">
          <div class="form-row">
            <div class="form-group">
              <label>Product Name: <span class="required">*</span></label>
              <input type="text" v-model="formData.name" required />
            </div>
            <div class="form-group">
              <label>Product Number: <span class="required">*</span></label>
              <input type="text" v-model="formData.productNumber" required />
            </div>
          </div>
          
          <div class="form-row">
            <div class="form-group">
              <label>Color:</label>
              <input type="text" v-model="formData.color" />
            </div>
            <div class="form-group">
              <label>List Price: <span class="required">*</span></label>
              <input type="number" v-model.number="formData.listPrice" step="0.01" min="0" required />
            </div>
          </div>

          <p class="form-note">* Category and Subcategory are read-only and set by the database</p>

          <div class="modal-actions">
            <button type="submit" class="btn btn-primary">{{ isEditing ? 'Update' : 'Create' }}</button>
            <button type="button" @click="closeModal" class="btn btn-secondary">Cancel</button>
          </div>
        </form>
      </div>
    </div>

    <!-- Delete Confirmation Modal -->
    <div v-if="showDeleteModal" class="modal-overlay" @click.self="showDeleteModal = false">
      <div class="modal modal-small">
        <h3>Confirm Delete</h3>
        <p>Are you sure you want to delete <strong>{{ productToDelete?.name }}</strong>?</p>
        <div class="modal-actions">
          <button @click="handleDelete" class="btn btn-danger">Delete</button>
          <button @click="showDeleteModal = false" class="btn btn-secondary">Cancel</button>
        </div>
      </div>
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
      success: null,
      showModal: false,
      showDeleteModal: false,
      isEditing: false,
      productToDelete: null,
      formData: {
        productID: 0,
        name: "",
        productNumber: "",
        color: "",
        listPrice: 0
      }
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
      this.success = null;
      try {
        this.products = await productsService.getAll();
      } catch (err) {
        this.error = err.response?.data?.message || err.message;
      } finally {
        this.loading = false;
      }
    },

    async handleSearch() {
      this.loading = true;
      this.error = null;
      this.success = null;
      try {
        this.products = await productsService.search(
          this.searchName || null,
          this.searchCategory || null
        );
      } catch (err) {
        this.error = err.response?.data?.message || err.message;
      } finally {
        this.loading = false;
      }
    },

    openCreateModal() {
      this.isEditing = false;
      this.formData = {
        productID: 0,
        name: "",
        productNumber: "",
        color: "",
        listPrice: 0
      };
      this.showModal = true;
      this.error = null;
      this.success = null;
    },

    openEditModal(product) {
      this.isEditing = true;
      this.formData = {
        productID: product.productID,
        name: product.name,
        productNumber: product.productNumber,
        color: product.color || "",
        listPrice: product.listPrice
      };
      this.showModal = true;
      this.error = null;
      this.success = null;
    },

    closeModal() {
      this.showModal = false;
      this.formData = {
        productID: 0,
        name: "",
        productNumber: "",
        color: "",
        listPrice: 0
      };
    },

    async handleSave() {
      this.error = null;
      this.success = null;
      try {
        if (this.isEditing) {
          await productsService.update(this.formData.productID, this.formData);
          this.success = "Product updated successfully!";
        } else {
          await productsService.create(this.formData);
          this.success = "Product created successfully!";
        }
        this.closeModal();
        await this.handleLoadAll();
      } catch (err) {
        this.error = err.response?.data?.message || err.message;
      }
    },

    confirmDelete(product) {
      this.productToDelete = product;
      this.showDeleteModal = true;
      this.error = null;
      this.success = null;
    },

    async handleDelete() {
      this.error = null;
      this.success = null;
      try {
        await productsService.delete(this.productToDelete.productID);
        this.success = "Product deleted successfully!";
        this.showDeleteModal = false;
        this.productToDelete = null;
        await this.handleLoadAll();
      } catch (err) {
        this.error = err.response?.data?.message || err.message;
        this.showDeleteModal = false;
      }
    }
  },
  mounted() {
    this.handleLoadAll();
  }
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
  transition: background 0.3s;
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
.btn-success {
  background: #27ae60;
  color: white;
}
.btn-success:hover {
  background: #229954;
}
.btn-danger {
  background: #e74c3c;
  color: white;
}
.btn-danger:hover {
  background: #c0392b;
}
.error-message {
  background: #fee;
  color: #c00;
  padding: 1rem;
  border-radius: 4px;
  margin-bottom: 1rem;
}
.success-message {
  background: #d4edda;
  color: #155724;
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
.actions {
  display: flex;
  gap: 0.5rem;
}
.btn-action {
  border: none;
  background: none;
  cursor: pointer;
  font-size: 1.2rem;
  padding: 0.25rem;
  transition: transform 0.2s;
}
.btn-action:hover {
  transform: scale(1.2);
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

/* Modal Styles */
.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: rgba(0, 0, 0, 0.5);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 1000;
}
.modal {
  background: white;
  padding: 2rem;
  border-radius: 8px;
  max-width: 600px;
  width: 90%;
  max-height: 90vh;
  overflow-y: auto;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.3);
}
.modal-small {
  max-width: 400px;
}
.modal h3 {
  margin-top: 0;
  margin-bottom: 1.5rem;
  color: #2c3e50;
}
.form-row {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 1rem;
  margin-bottom: 1rem;
}
.modal .form-group {
  margin-bottom: 1rem;
}
.modal .form-group input,
.modal .form-group select {
  width: 100%;
}
.required {
  color: #e74c3c;
}
.form-note {
  color: #666;
  font-size: 0.9rem;
  font-style: italic;
  margin-bottom: 1rem;
}
.modal-actions {
  display: flex;
  gap: 1rem;
  justify-content: flex-end;
  margin-top: 1.5rem;
}
</style>

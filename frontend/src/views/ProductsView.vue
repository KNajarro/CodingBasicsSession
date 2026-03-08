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

    <div v-if="visibleProducts.length > 0" class="table-container">
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
          <tr v-for="product in visibleProducts" :key="product.productID">
            <td>{{ product.productID }}</td>
            <td>{{ product.name }}</td>
            <td>{{ product.productNumber }}</td>
            <td>{{ product.color || "-" }}</td>
            <td>{{ formatPrice(product.listPrice) }}</td>
            <td>{{ product.categoryName || "-" }}</td>
            <td>{{ product.subcategoryName || "-" }}</td>
            <td class="actions">
              <button
                @click="startEdit(product)"
                class="btn btn-warning btn-sm"
              >
                Edit
              </button>
              <button
                @click="handleDelete(product.productID)"
                class="btn btn-danger btn-sm"
              >
                Delete
              </button>
            </td>
          </tr>
        </tbody>
      </table>

      <div class="pagination">
        <button
          class="btn btn-secondary btn-sm"
          @click="goToPrevPage"
          :disabled="currentPage === 1 || loading"
        >
          Prev
        </button>

        <span class="page-info">
          Page {{ currentPage }} of {{ totalPages }}
        </span>

        <button
          class="btn btn-secondary btn-sm"
          @click="goToNextPage"
          :disabled="currentPage === totalPages || loading"
        >
          Next
        </button>
      </div>

      <p class="record-count">
        Showing {{ visibleProducts.length }} of {{ totalCount }}
      </p>
    </div>

    <div v-else-if="!loading" class="empty-state">
      <p>No data. Click Load All or Search.</p>
    </div>

    <!-- Edit modal -->
    <div v-if="editingProduct" class="modal-overlay" @click.self="cancelEdit">
      <div class="modal-panel">
        <div class="modal-header">
          <h3>Edit Product</h3>
          <button class="close-btn" @click="cancelEdit">×</button>
        </div>

        <div class="edit-form">
          <div class="form-group">
            <label>Name</label>
            <input type="text" v-model="editingProduct.name" />
          </div>

          <div class="form-group">
            <label>Product Number</label>
            <input type="text" v-model="editingProduct.productNumber" />
          </div>

          <div class="form-group">
            <label>Color</label>
            <input type="text" v-model="editingProduct.color" />
          </div>

          <div class="form-group">
            <label>List Price</label>
            <input
              type="number"
              step="0.01"
              v-model.number="editingProduct.listPrice"
            />
          </div>

          <div class="form-group">
            <label>Make Flag</label>
            <select v-model="editingProduct.makeFlag">
              <option :value="true">True</option>
              <option :value="false">False</option>
            </select>
          </div>

          <div class="form-group">
            <label>Finished Goods Flag</label>
            <select v-model="editingProduct.finishedGoodsFlag">
              <option :value="true">True</option>
              <option :value="false">False</option>
            </select>
          </div>

          <div class="form-group">
            <label>Safety Stock Level</label>
            <input
              type="number"
              v-model.number="editingProduct.safetyStockLevel"
            />
          </div>

          <div class="form-group">
            <label>Reorder Point</label>
            <input type="number" v-model.number="editingProduct.reorderPoint" />
          </div>

          <div class="form-group">
            <label>Standard Cost</label>
            <input
              type="number"
              step="0.01"
              v-model.number="editingProduct.standardCost"
            />
          </div>

          <div class="form-group">
            <label>Size</label>
            <input type="text" v-model="editingProduct.size" />
          </div>

          <div class="form-group">
            <label>Size Unit Measure Code</label>
            <input type="text" v-model="editingProduct.sizeUnitMeasureCode" />
          </div>

          <div class="form-group">
            <label>Weight Unit Measure Code</label>
            <input type="text" v-model="editingProduct.weightUnitMeasureCode" />
          </div>

          <div class="form-group">
            <label>Weight</label>
            <input
              type="number"
              step="0.01"
              v-model.number="editingProduct.weight"
            />
          </div>

          <div class="form-group">
            <label>Days To Manufacture</label>
            <input
              type="number"
              v-model.number="editingProduct.daysToManufacture"
            />
          </div>

          <div class="form-group">
            <label>Product Line</label>
            <input type="text" v-model="editingProduct.productLine" />
          </div>

          <div class="form-group">
            <label>Class</label>
            <input type="text" v-model="editingProduct.class" />
          </div>

          <div class="form-group">
            <label>Style</label>
            <input type="text" v-model="editingProduct.style" />
          </div>

          <div class="form-group">
            <label>Product Subcategory ID</label>
            <input
              type="number"
              v-model.number="editingProduct.productSubcategoryID"
            />
          </div>

          <div class="form-group">
            <label>Product Model ID</label>
            <input
              type="number"
              v-model.number="editingProduct.productModelID"
            />
          </div>

          <div class="form-group">
            <label>Sell Start Date</label>
            <input
              type="datetime-local"
              v-model="editingProduct.sellStartDate"
            />
          </div>
        </div>

        <div class="edit-actions">
          <button @click="handleUpdate" class="btn btn-primary">Save</button>
          <button @click="cancelEdit" class="btn btn-secondary">Cancel</button>
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
      editingProduct: null,
      currentPage: 1,
      pageSize: 20,
      totalCount: 0,
    };
  },
  computed: {
    totalPages() {
      return Math.max(1, Math.ceil(this.totalCount / this.pageSize));
    },
    visibleProducts() {
      const start = (this.currentPage - 1) * this.pageSize;
      const end = start + this.pageSize;
      return this.products.slice(start, end);
    },
  },
  mounted() {
    this.handleLoadAll();
  },
  methods: {
    toNullIfEmpty(value) {
      return value === "" || value === undefined ? null : value;
    },

    toIsoDate(value) {
      if (!value) return null;
      const date = new Date(value);
      return isNaN(date.getTime()) ? null : date.toISOString();
    },

    normalizeProduct(product) {
      return {
        productID: 0,
        name: product.name || "",
        productNumber: product.productNumber || "",
        color: this.toNullIfEmpty(product.color),
        listPrice: Number(product.listPrice || 0),
        makeFlag: !!product.makeFlag,
        finishedGoodsFlag: !!product.finishedGoodsFlag,
        safetyStockLevel: Number(product.safetyStockLevel || 0),
        reorderPoint: Number(product.reorderPoint || 0),
        standardCost: Number(product.standardCost || 0),
        size: this.toNullIfEmpty(product.size),
        sizeUnitMeasureCode: this.toNullIfEmpty(product.sizeUnitMeasureCode),
        weightUnitMeasureCode: this.toNullIfEmpty(
          product.weightUnitMeasureCode,
        ),
        weight:
          product.weight === "" ||
          product.weight === null ||
          product.weight === undefined
            ? null
            : Number(product.weight),
        daysToManufacture: Number(product.daysToManufacture || 0),
        productLine: this.toNullIfEmpty(product.productLine),
        class: this.toNullIfEmpty(product.class),
        style: this.toNullIfEmpty(product.style),
        productSubcategoryID:
          product.productSubcategoryID === "" ||
          product.productSubcategoryID === null ||
          product.productSubcategoryID === undefined
            ? null
            : Number(product.productSubcategoryID),
        productModelID:
          product.productModelID === "" ||
          product.productModelID === null ||
          product.productModelID === undefined
            ? null
            : Number(product.productModelID),
        sellStartDate: this.toIsoDate(product.sellStartDate),
        sellEndDate: null,
        discontinuedDate: null,
        categoryName: null,
        subcategoryName: null,
      };
    },

    formatPrice(price) {
      return new Intl.NumberFormat("en-US", {
        style: "currency",
        currency: "USD",
      }).format(price || 0);
    },

    async handleLoadAll() {
      this.loading = true;
      this.error = null;
      this.currentPage = 1;

      try {
        const data = await productsService.getAll();
        this.products = data || [];
        this.totalCount = this.products.length;
      } catch (err) {
        this.error =
          err?.response?.data?.message ||
          err?.message ||
          "Failed to load products.";
      } finally {
        this.loading = false;
      }
    },

    async handleSearch() {
      this.loading = true;
      this.error = null;
      this.currentPage = 1;

      try {
        const data = await productsService.search(
          this.searchName || null,
          this.searchCategory || null,
        );
        this.products = data || [];
        this.totalCount = this.products.length;
      } catch (err) {
        this.error =
          err?.response?.data?.message ||
          err?.message ||
          "Failed to search products.";
      } finally {
        this.loading = false;
      }
    },

    goToPrevPage() {
      if (this.currentPage > 1) {
        this.currentPage--;
      }
    },

    goToNextPage() {
      if (this.currentPage < this.totalPages) {
        this.currentPage++;
      }
    },

    startEdit(product) {
      this.editingProduct = {
        ...product,
        sellStartDate: product.sellStartDate
          ? new Date(product.sellStartDate).toISOString().slice(0, 16)
          : "",
      };
    },

    cancelEdit() {
      this.editingProduct = null;
    },

    async handleUpdate() {
      if (!this.editingProduct) return;

      this.loading = true;
      this.error = null;

      try {
        const payload = this.normalizeProduct(this.editingProduct);
        const updated = await productsService.update(
          this.editingProduct.productID,
          payload,
        );

        const index = this.products.findIndex(
          (p) => p.productID === updated.productID,
        );

        if (index !== -1) {
          this.products.splice(index, 1, updated);
        }

        this.editingProduct = null;
      } catch (err) {
        this.error =
          err?.response?.data?.message ||
          err?.message ||
          "Failed to update product.";
      } finally {
        this.loading = false;
      }
    },

    async handleDelete(id) {
      const confirmed = window.confirm(
        "Are you sure you want to delete this product?",
      );
      if (!confirmed) return;

      this.loading = true;
      this.error = null;

      try {
        await productsService.delete(id);
        this.products = this.products.filter((p) => p.productID !== id);
        this.totalCount = this.products.length;

        if (this.currentPage > this.totalPages) {
          this.currentPage = this.totalPages;
        }

        if (this.editingProduct && this.editingProduct.productID === id) {
          this.editingProduct = null;
        }
      } catch (err) {
        this.error =
          err?.response?.data?.message ||
          err?.message ||
          "Failed to delete product.";
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

h3 {
  margin: 0;
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

.btn-warning {
  background: #f39c12;
  color: white;
}

.btn-warning:hover {
  background: #d68910;
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
  font-size: 0.875rem;
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

.actions {
  display: flex;
  gap: 0.5rem;
}

.pagination {
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 1rem;
  padding: 1rem 0 0;
}

.page-info {
  color: #555;
  font-weight: 600;
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

.modal-overlay {
  position: fixed;
  inset: 0;
  background: rgba(0, 0, 0, 0.45);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 1000;
  padding: 1rem;
}

.modal-panel {
  width: min(1000px, 95vw);
  max-height: 90vh;
  overflow-y: auto;
  padding: 1rem;
  background: white;
  border-radius: 10px;
  box-shadow: 0 10px 35px rgba(0, 0, 0, 0.25);
}

.modal-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 1rem;
}

.close-btn {
  border: none;
  background: transparent;
  font-size: 2rem;
  line-height: 1;
  cursor: pointer;
  color: #555;
}

.edit-form {
  display: flex;
  gap: 1rem;
  flex-wrap: wrap;
}

.edit-actions {
  display: flex;
  gap: 1rem;
  margin-top: 1rem;
}
</style>

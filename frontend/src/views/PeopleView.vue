<template>
  <div class="people-view">
    <h2>People</h2>

    <div class="search-form">
      <div class="form-group">
        <label>Name:</label>
        <input
          type="text"
          v-model="searchName"
          placeholder="Search by name..."
        />
      </div>

      <div class="form-group">
        <label>Person Type:</label>
        <select v-model="searchType">
          <option value="">All Types</option>
          <option value="SC">SC - Store Contact</option>
          <option value="IN">IN - Individual</option>
          <option value="SP">SP - Sales Person</option>
          <option value="EM">EM - Employee</option>
          <option value="VC">VC - Vendor Contact</option>
          <option value="GC">GC - General Contact</option>
        </select>
      </div>

      <button @click="handleSearch" class="btn btn-primary">Search</button>
      <button @click="handleLoadAll(1)" class="btn btn-secondary">
        Load All
      </button>
      <button class="btn btn-disabled" disabled>Create</button>
    </div>

    <div v-if="error" class="error-message">{{ error }}</div>
    <div v-if="loading" class="loading">Loading...</div>

    <div v-if="visiblePeople.length > 0" class="table-container">
      <table class="data-table">
        <thead>
          <tr>
            <th>ID</th>
            <th>Type</th>
            <th>Title</th>
            <th>First Name</th>
            <th>Middle Name</th>
            <th>Last Name</th>
            <th>Suffix</th>
            <th>Email Promo</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="person in visiblePeople" :key="person.businessEntityID">
            <td>{{ person.businessEntityID }}</td>
            <td>{{ person.personType }}</td>
            <td>{{ person.title }}</td>
            <td>{{ person.firstName }}</td>
            <td>{{ person.middleName }}</td>
            <td>{{ person.lastName }}</td>
            <td>{{ person.suffix }}</td>
            <td>{{ person.emailPromotion }}</td>
            <td class="actions">
              <button @click="startEdit(person)" class="btn btn-warning btn-sm">
                Edit
              </button>
              <button
                @click="handleDelete(person.businessEntityID)"
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
        Showing {{ visiblePeople.length }} of {{ totalCount }}
      </p>
    </div>

    <div v-else-if="!loading" class="empty-state">
      <p>No data. Click Load All or Search.</p>
    </div>

    <div v-if="editingPerson" class="modal-overlay" @click.self="cancelEdit">
      <div class="edit-panel">
        <div class="modal-header">
          <h3>Edit Person</h3>
          <button class="close-btn" @click="cancelEdit">×</button>
        </div>

        <div class="edit-form">
          <div class="form-group">
            <label>Person Type</label>
            <select v-model="editingPerson.personType">
              <option value="SC">SC</option>
              <option value="IN">IN</option>
              <option value="SP">SP</option>
              <option value="EM">EM</option>
              <option value="VC">VC</option>
              <option value="GC">GC</option>
            </select>
          </div>

          <div class="form-group">
            <label>Title</label>
            <input type="text" v-model="editingPerson.title" />
          </div>

          <div class="form-group">
            <label>First Name</label>
            <input type="text" v-model="editingPerson.firstName" />
          </div>

          <div class="form-group">
            <label>Middle Name</label>
            <input type="text" v-model="editingPerson.middleName" />
          </div>

          <div class="form-group">
            <label>Last Name</label>
            <input type="text" v-model="editingPerson.lastName" />
          </div>

          <div class="form-group">
            <label>Suffix</label>
            <input type="text" v-model="editingPerson.suffix" />
          </div>

          <div class="form-group">
            <label>Email Promotion</label>
            <input
              type="number"
              v-model.number="editingPerson.emailPromotion"
            />
          </div>

          <div class="form-group">
            <label>Additional Contact Info</label>
            <input type="text" v-model="editingPerson.additionalContactInfo" />
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
import peopleService from "../services/peopleService";

export default {
  name: "PeopleView",
  data() {
    return {
      people: [],
      searchName: "",
      searchType: "",
      loading: false,
      error: null,
      editingPerson: null,
      currentPage: 1,
      pageSize: 20,
      totalCount: 0,
      isSearchMode: false,
    };
  },
  computed: {
    totalPages() {
      return Math.max(1, Math.ceil(this.totalCount / this.pageSize));
    },
    visiblePeople() {
      if (!this.isSearchMode) {
        return this.people;
      }

      const start = (this.currentPage - 1) * this.pageSize;
      const end = start + this.pageSize;
      return this.people.slice(start, end);
    },
  },
  mounted() {
    this.handleLoadAll(1);
  },
  methods: {
    async handleLoadAll(page = 1) {
      this.loading = true;
      this.error = null;
      this.isSearchMode = false;

      try {
        const response = await peopleService.getAll(page, this.pageSize);
        this.people = response.items || [];
        this.currentPage = response.page || page;
        this.pageSize = response.pageSize || 20;
        this.totalCount = response.totalCount || 0;
      } catch (err) {
        this.error = err?.message || "Failed to load people.";
      } finally {
        this.loading = false;
      }
    },

    async handleSearch() {
      this.loading = true;
      this.error = null;
      this.isSearchMode = true;
      this.currentPage = 1;

      try {
        const results = await peopleService.search(
          this.searchName || null,
          this.searchType || null,
        );
        this.people = results || [];
        this.totalCount = this.people.length;
      } catch (err) {
        this.error = err?.message || "Failed to search people.";
      } finally {
        this.loading = false;
      }
    },

    goToPrevPage() {
      if (this.currentPage === 1 || this.loading) return;

      if (this.isSearchMode) {
        this.currentPage--;
      } else {
        this.handleLoadAll(this.currentPage - 1);
      }
    },

    goToNextPage() {
      if (this.currentPage === this.totalPages || this.loading) return;

      if (this.isSearchMode) {
        this.currentPage++;
      } else {
        this.handleLoadAll(this.currentPage + 1);
      }
    },

    startEdit(person) {
      this.editingPerson = { ...person };
    },

    cancelEdit() {
      this.editingPerson = null;
    },

    async handleUpdate() {
      if (!this.editingPerson) return;

      this.loading = true;
      this.error = null;

      try {
        const updated = await peopleService.update(
          this.editingPerson.businessEntityID,
          this.editingPerson,
        );

        if (this.isSearchMode) {
          const index = this.people.findIndex(
            (p) => p.businessEntityID === updated.businessEntityID,
          );
          if (index !== -1) this.people.splice(index, 1, updated);
        } else {
          const index = this.people.findIndex(
            (p) => p.businessEntityID === updated.businessEntityID,
          );
          if (index !== -1) this.people.splice(index, 1, updated);
        }

        this.editingPerson = null;
      } catch (err) {
        this.error = err?.message || "Failed to update person.";
      } finally {
        this.loading = false;
      }
    },

    async handleDelete(id) {
      const confirmed = window.confirm(
        "Are you sure you want to delete this person?",
      );
      if (!confirmed) return;

      this.loading = true;
      this.error = null;

      try {
        await peopleService.delete(id);

        this.people = this.people.filter((p) => p.businessEntityID !== id);
        this.totalCount = Math.max(0, this.totalCount - 1);

        if (this.currentPage > this.totalPages) {
          this.currentPage = this.totalPages;
        }

        if (
          !this.isSearchMode &&
          this.people.length === 0 &&
          this.currentPage > 1
        ) {
          await this.handleLoadAll(this.currentPage - 1);
          return;
        }

        if (this.editingPerson && this.editingPerson.businessEntityID === id) {
          this.editingPerson = null;
        }
      } catch (err) {
        this.error = err?.message || "Failed to delete person.";
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

.btn-disabled {
  background: #dcdcdc;
  color: #777;
  cursor: not-allowed;
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

.edit-panel {
  width: min(900px, 95vw);
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

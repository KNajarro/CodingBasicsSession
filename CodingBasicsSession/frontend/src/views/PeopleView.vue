<template>
  <div class="people-view">
    <h2>People</h2>

    <div class="search-form">
      <div class="form-group">
        <label>Name:</label>
        <input type="text" v-model="searchName" placeholder="Search by name..." />
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
      <button @click="handleLoadAll" class="btn btn-secondary">Load All</button>
    </div>

    <div class="crud-form">
      <h3>{{ editingId ? 'Edit Person' : 'Create Person' }}</h3>
      <div class="form-grid">
        <div class="form-group">
          <label>Person Type:</label>
          <select v-model="form.personType">
            <option value="SC">SC - Store Contact</option>
            <option value="IN">IN - Individual</option>
            <option value="SP">SP - Sales Person</option>
            <option value="EM">EM - Employee</option>
            <option value="VC">VC - Vendor Contact</option>
            <option value="GC">GC - General Contact</option>
          </select>
        </div>
        <div class="form-group">
          <label>Title:</label>
          <input type="text" v-model="form.title" />
        </div>
        <div class="form-group">
          <label>First Name:</label>
          <input type="text" v-model="form.firstName" />
        </div>
        <div class="form-group">
          <label>Middle Name:</label>
          <input type="text" v-model="form.middleName" />
        </div>
        <div class="form-group">
          <label>Last Name:</label>
          <input type="text" v-model="form.lastName" />
        </div>
        <div class="form-group">
          <label>Suffix:</label>
          <input type="text" v-model="form.suffix" />
        </div>
        <div class="form-group">
          <label>Email Promotion:</label>
          <input type="number" min="0" max="2" v-model.number="form.emailPromotion" />
        </div>
      </div>
      <div class="actions-row">
        <button @click="handleSave" class="btn btn-primary">{{ editingId ? 'Update' : 'Create' }}</button>
        <button v-if="editingId" @click="resetForm" class="btn btn-secondary">Cancel</button>
      </div>
    </div>

    <div v-if="error" class="error-message">{{ error }}</div>
    <div v-if="loading" class="loading">Loading...</div>

    <div v-else-if="people.length > 0" class="table-container">
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
          <tr v-for="person in people" :key="person.businessEntityID">
            <td>{{ person.businessEntityID }}</td>
            <td>{{ person.personType }}</td>
            <td>{{ person.title || '-' }}</td>
            <td>{{ person.firstName }}</td>
            <td>{{ person.middleName || '-' }}</td>
            <td>{{ person.lastName }}</td>
            <td>{{ person.suffix || '-' }}</td>
            <td>{{ person.emailPromotion }}</td>
            <td>
              <button @click="editPerson(person)" class="btn btn-sm btn-primary">Edit</button>
              <button @click="deletePerson(person.businessEntityID)" class="btn btn-sm btn-danger">Delete</button>
            </td>
          </tr>
        </tbody>
      </table>
      <p class="record-count">Total: {{ people.length }}</p>
    </div>

    <div v-else class="empty-state">
      <p>No data. Click Load All or Search.</p>
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
      editingId: null,
      form: {
        personType: "IN",
        title: "",
        firstName: "",
        middleName: "",
        lastName: "",
        suffix: "",
        emailPromotion: 0,
      },
    };
  },
  methods: {
    mapError(err) {
      return err?.friendlyMessage || err?.response?.data?.message || err?.response?.data?.title || err?.message || "Request failed";
    },
    normalizePersonPayload() {
      return {
        personType: this.form.personType,
        title: this.form.title || null,
        firstName: this.form.firstName,
        middleName: this.form.middleName || null,
        lastName: this.form.lastName,
        suffix: this.form.suffix || null,
        emailPromotion: Number(this.form.emailPromotion || 0),
      };
    },
    resetForm() {
      this.editingId = null;
      this.form = {
        personType: "IN",
        title: "",
        firstName: "",
        middleName: "",
        lastName: "",
        suffix: "",
        emailPromotion: 0,
      };
    },
    async refreshCurrentList() {
      if (this.searchName || this.searchType) {
        this.people = await peopleService.search(this.searchName || null, this.searchType || null);
        return;
      }
      this.people = await peopleService.getAll();
    },
    async handleLoadAll() {
      this.loading = true;
      this.error = null;
      try {
        this.people = await peopleService.getAll();
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
        this.people = await peopleService.search(this.searchName || null, this.searchType || null);
      } catch (err) {
        this.error = this.mapError(err);
      } finally {
        this.loading = false;
      }
    },
    editPerson(person) {
      this.editingId = person.businessEntityID;
      this.form = {
        personType: person.personType || "IN",
        title: person.title || "",
        firstName: person.firstName || "",
        middleName: person.middleName || "",
        lastName: person.lastName || "",
        suffix: person.suffix || "",
        emailPromotion: Number(person.emailPromotion || 0),
      };
    },
    async handleSave() {
      if (!this.form.personType || !this.form.firstName || !this.form.lastName) {
        this.error = "Person Type, First Name y Last Name son obligatorios.";
        return;
      }

      this.loading = true;
      this.error = null;
      try {
        const payload = this.normalizePersonPayload();
        if (this.editingId) {
          await peopleService.update(this.editingId, payload);
        } else {
          await peopleService.create(payload);
        }
        this.resetForm();
        await this.refreshCurrentList();
      } catch (err) {
        this.error = this.mapError(err);
      } finally {
        this.loading = false;
      }
    },
    async deletePerson(id) {
      if (!confirm(`Delete person ${id}?`)) return;

      this.loading = true;
      this.error = null;
      try {
        await peopleService.delete(id);
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

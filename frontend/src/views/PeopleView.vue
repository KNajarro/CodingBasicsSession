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
      <button @click="handleLoadAll" class="btn btn-secondary">Load All</button>
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
          </tr>
        </thead>
        <tbody>
          <tr v-for="person in people" :key="person.businessEntityID">
            <td>{{ person.businessEntityID }}</td>
            <td>{{ person.personType }}</td>
            <td>{{ person.title }}</td>
            <td>{{ person.firstName }}</td>
            <td>{{ person.middleName }}</td>
            <td>{{ person.lastName }}</td>
            <td>{{ person.suffix }}</td>
            <td>{{ person.emailPromotion }}</td>
          </tr>
        </tbody>
      </table>
      <p class="record-count">Total: {{ people.length }}</p>
    </div>
    <div v-else class="empty-state">
      <p>No data. Click Load All or Search.</p>
    </div>
    <!-- TODO (Workshop): Add UI and methods for Create, Update, and Delete person records -->
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
    };
  },
  methods: {
    async handleLoadAll() {
      this.loading = true
      this.error = null
      try {
        this.people = await peopleService.getAll()
      } catch (err) {
        this.error = err.message
      } finally {
        this.loading = false
      }
    },
    async handleSearch() {
      this.loading = true
      this.error = null
      try {
        this.people = await peopleService.search(this.searchName || null, this.searchType || null)
      } catch (err) {
        this.error = err.message
      } finally {
        this.loading = false
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
</style>

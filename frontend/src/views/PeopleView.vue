<template>
  <div class="view-wrap">

    <!-- ── Header ── -->
    <div class="view-header">
      <div>
        <h2 class="view-title">People</h2>
        <p class="view-sub">{{ isSearch ? searchResults.length + ' search results' : totalCount.toLocaleString() + ' total records' }}</p>
      </div>
      <button class="rsm-btn rsm-btn-primary" @click="openCreate">
        <svg viewBox="0 0 24 24" width="14" height="14" fill="currentColor"><path d="M19 13h-6v6h-2v-6H5v-2h6V5h2v6h6v2z"/></svg>
        Add Person
      </button>
    </div>

    <!-- ── Toolbar ── -->
    <div class="rsm-card toolbar-card">
      <div class="rsm-card-body" style="padding:0.9rem 1.25rem">
        <div class="toolbar">
          <div class="search-field">
            <svg viewBox="0 0 24 24" width="15" height="15" fill="none" stroke="currentColor" stroke-width="2"><circle cx="11" cy="11" r="8"/><line x1="21" y1="21" x2="16.65" y2="16.65"/></svg>
            <input v-model="filterName" @keyup.enter="doSearch" placeholder="Search by name…" />
          </div>
          <select v-model="filterType" class="select-field">
            <option value="">All Types</option>
            <option value="SC">SC – Store Contact</option>
            <option value="IN">IN – Individual</option>
            <option value="SP">SP – Sales Person</option>
            <option value="EM">EM – Employee</option>
            <option value="VC">VC – Vendor Contact</option>
            <option value="GC">GC – General Contact</option>
          </select>
          <button class="rsm-btn rsm-btn-primary" @click="doSearch" :disabled="loading">Search</button>
          <button class="rsm-btn rsm-btn-secondary" @click="clearSearch" :disabled="loading">Clear</button>
          <div class="toolbar-spacer"></div>
          <select v-model.number="pageSize" @change="currentPage=1;loadPage(1)" class="select-field select-sm" :disabled="isSearch">
            <option :value="10">10 / page</option>
            <option :value="20">20 / page</option>
            <option :value="50">50 / page</option>
          </select>
        </div>
      </div>
    </div>

    <!-- ── Error ── -->
    <div v-if="error" class="err-banner">{{ error }}</div>

    <!-- ── Table Card ── -->
    <div class="rsm-card">

      <!-- Loading overlay -->
      <div v-if="loading" class="table-loading">
        <svg class="spin" viewBox="0 0 24 24" width="24" height="24" fill="none" stroke="#1e3a5f" stroke-width="2.5"><circle cx="12" cy="12" r="10" stroke-opacity="0.2"/><path d="M12 2a10 10 0 0 1 10 10"/></svg>
        <span>Loading…</span>
      </div>

      <div style="overflow-x:auto">
        <table class="rsm-table">
          <thead>
            <tr>
              <th>ID</th>
              <th>Type</th>
              <th>Full Name</th>
              <th>Email Promo</th>
              <th style="text-align:right">Actions</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="p in rows" :key="p.businessEntityID">
              <td><span class="id-cell">{{ p.businessEntityID }}</span></td>
              <td>
                <span :class="typeBadgeClass(p.personType)" class="badge">{{ p.personType }}</span>
              </td>
              <td style="font-weight:500">{{ p.fullName }}</td>
              <td>
                <span :class="promoBadgeClass(p.emailPromotion)" class="badge">{{ promoLabel(p.emailPromotion) }}</span>
              </td>
              <td>
                <div class="action-btns">
                  <button class="rsm-btn rsm-btn-ghost" @click="openEdit(p)" title="Edit">
                    <svg viewBox="0 0 24 24" width="14" height="14" fill="currentColor"><path d="M3 17.25V21h3.75L17.81 9.94l-3.75-3.75L3 17.25zM20.71 7.04a1 1 0 000-1.41l-2.34-2.34a1 1 0 00-1.41 0l-1.83 1.83 3.75 3.75 1.83-1.83z"/></svg>
                    Edit
                  </button>
                  <button class="rsm-btn rsm-btn-ghost act-del" @click="doDelete(p)" title="Delete">
                    <svg viewBox="0 0 24 24" width="14" height="14" fill="currentColor"><path d="M6 19c0 1.1.9 2 2 2h8c1.1 0 2-.9 2-2V7H6v12zm3-6V9h2v4H9zm4 0V9h2v4h-2zM15.5 4l-1-1h-5l-1 1H5v2h14V4z"/></svg>
                    Delete
                  </button>
                </div>
              </td>
            </tr>
            <tr v-if="!loading && rows.length === 0">
              <td colspan="5" class="empty-row">No records found.</td>
            </tr>
          </tbody>
        </table>
      </div>

      <!-- Pagination footer -->
      <div v-if="!isSearch && totalCount > 0" class="pagination-bar">
        <span class="pg-info">
          Showing {{ showFrom }}–{{ showTo }} of {{ totalCount.toLocaleString() }}
        </span>
        <div class="pg-controls">
          <button class="rsm-btn rsm-btn-secondary pg-btn" @click="loadPage(currentPage-1)" :disabled="currentPage<=1 || loading">&lt;</button>
          <button
            v-for="n in visiblePages" :key="n"
            :class="['rsm-btn','pg-btn', n===currentPage ? 'pg-active' : 'rsm-btn-secondary']"
            @click="loadPage(n)" :disabled="loading"
          >{{ n }}</button>
          <button class="rsm-btn rsm-btn-secondary pg-btn" @click="loadPage(currentPage+1)" :disabled="currentPage>=totalPages || loading">&gt;</button>
        </div>
      </div>
    </div>

    <!-- ── Modal ── -->
    <div v-if="showModal" class="modal-backdrop" @click.self="showModal=false">
      <div class="modal-box">
        <div class="modal-head">
          <h3>{{ modalMode==='create' ? 'Add Person' : 'Edit Person' }}</h3>
          <button class="modal-close" @click="showModal=false">
            <svg viewBox="0 0 24 24" width="18" height="18" fill="currentColor"><path d="M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z"/></svg>
          </button>
        </div>
        <form @submit.prevent="saveForm" class="modal-body">
          <div class="form-row">
            <div class="form-field">
              <label>Person Type <span class="req">*</span></label>
              <select v-model="form.personType" required>
                <option value="SC">SC – Store Contact</option>
                <option value="IN">IN – Individual Customer</option>
                <option value="SP">SP – Sales Person</option>
                <option value="EM">EM – Employee</option>
                <option value="VC">VC – Vendor Contact</option>
                <option value="GC">GC – General Contact</option>
              </select>
            </div>
            <div class="form-field">
              <label>Title</label>
              <input type="text" v-model="form.title" placeholder="Mr. / Ms. / Dr." />
            </div>
          </div>
          <div class="form-row">
            <div class="form-field">
              <label>First Name <span class="req">*</span></label>
              <input type="text" v-model="form.firstName" required placeholder="First name" />
            </div>
            <div class="form-field">
              <label>Middle Name</label>
              <input type="text" v-model="form.middleName" placeholder="Middle name" />
            </div>
            <div class="form-field">
              <label>Last Name <span class="req">*</span></label>
              <input type="text" v-model="form.lastName" required placeholder="Last name" />
            </div>
          </div>
          <div class="form-row">
            <div class="form-field">
              <label>Suffix</label>
              <input type="text" v-model="form.suffix" placeholder="Jr. / Sr. / III" />
            </div>
            <div class="form-field">
              <label>Email Promotion <span class="req">*</span></label>
              <select v-model.number="form.emailPromotion" required>
                <option :value="0">No Promotions</option>
                <option :value="1">Occasional</option>
                <option :value="2">Frequent Emails</option>
              </select>
            </div>
          </div>
          <div v-if="modalError" class="modal-err">{{ modalError }}</div>
          <div class="modal-footer">
            <button type="button" class="rsm-btn rsm-btn-secondary" @click="showModal=false">Cancel</button>
            <button type="submit" class="rsm-btn rsm-btn-primary" :disabled="saving">
              {{ saving ? 'Saving…' : (modalMode==='create' ? 'Create' : 'Save Changes') }}
            </button>
          </div>
        </form>
      </div>
    </div>

  </div>
</template>

<script>
import peopleService from '../services/peopleService'

const EMPTY = { businessEntityID: 0, personType: 'IN', title: '', firstName: '', middleName: '', lastName: '', suffix: '', emailPromotion: 0 }

export default {
  name: 'PeopleView',
  data() {
    return {
      rows: [], totalCount: 0, currentPage: 1, pageSize: 20,
      filterName: '', filterType: '',
      isSearch: false, searchResults: [],
      loading: false, error: null,
      showModal: false, modalMode: 'create', saving: false, modalError: null,
      form: { ...EMPTY }
    }
  },
  computed: {
    totalPages() { return Math.ceil(this.totalCount / this.pageSize) },
    showFrom() { return Math.min((this.currentPage-1)*this.pageSize+1, this.totalCount) },
    showTo()   { return Math.min(this.currentPage*this.pageSize, this.totalCount) },
    visiblePages() {
      const r=2, tp=this.totalPages, cp=this.currentPage, pages=[]
      for (let i=Math.max(1,cp-r); i<=Math.min(tp,cp+r); i++) pages.push(i)
      return pages
    }
  },
  methods: {
    typeBadgeClass(t) { return { SC:'badge-gray', IN:'badge-blue', SP:'badge-green', EM:'badge-navy', VC:'badge-yellow', GC:'badge-gray' }[t] || 'badge-gray' },
    promoBadgeClass(n) { return [,'badge-yellow','badge-green'][n] || 'badge-gray' },
    promoLabel(n)      { return ['None','Occasional','Frequent'][n] ?? String(n) },

    async loadPage(page) {
      this.loading = true; this.error = null; this.isSearch = false
      try {
        const d = await peopleService.getAll(page, this.pageSize)
        this.rows = d.items; this.totalCount = d.totalCount; this.currentPage = page
      } catch (e) { this.error = e.message }
      finally { this.loading = false }
    },

    async doSearch() {
      if (!this.filterName && !this.filterType) return this.loadPage(1)
      this.loading = true; this.error = null
      try {
        const d = await peopleService.search(this.filterName||null, this.filterType||null)
        this.searchResults = d; this.rows = d; this.isSearch = true
      } catch (e) { this.error = e.message }
      finally { this.loading = false }
    },

    clearSearch() { this.filterName=''; this.filterType=''; this.isSearch=false; this.loadPage(1) },

    openCreate() { this.form={...EMPTY}; this.modalMode='create'; this.modalError=null; this.showModal=true },
    openEdit(p)  { this.form={...p};    this.modalMode='edit';   this.modalError=null; this.showModal=true },

    async saveForm() {
      this.saving=true; this.modalError=null
      try {
        if (this.modalMode==='create') await peopleService.create(this.form)
        else                           await peopleService.update(this.form.businessEntityID, this.form)
        this.showModal=false
        if (this.isSearch) this.doSearch(); else this.loadPage(this.currentPage)
      } catch (e) { this.modalError = e.response?.data?.message || e.message }
      finally { this.saving=false }
    },

    async doDelete(p) {
      if (!window.confirm(`Delete ${p.fullName}? This action cannot be undone.`)) return
      this.loading=true; this.error=null
      try {
        await peopleService.delete(p.businessEntityID)
        if (this.isSearch) this.doSearch(); else this.loadPage(this.currentPage)
      } catch (e) { this.error = e.response?.data?.message || e.message; this.loading=false }
    }
  },
  mounted() { this.loadPage(1) }
}
</script>

<style scoped>
/* ── Page layout ────────────────────────────────────────── */
.view-wrap { display:flex; flex-direction:column; gap:1rem; }
.view-header { display:flex; justify-content:space-between; align-items:center; }
.view-title { font-size:1.35rem; font-weight:700; color:var(--text); }
.view-sub   { font-size:0.8rem; color:var(--slate-lg); margin-top:0.1rem; }

/* ── Toolbar ────────────────────────────────────────────── */
.toolbar { display:flex; flex-wrap:wrap; align-items:center; gap:0.6rem; }
.toolbar-spacer { flex:1; }
.search-field {
  display:flex; align-items:center; gap:0.5rem;
  border:1.5px solid var(--border); border-radius:var(--radius);
  padding:0.4rem 0.75rem; background:var(--bg); transition:border-color 0.15s;
  min-width:220px;
}
.search-field:focus-within { border-color:var(--accent); background:#fff; }
.search-field svg { color:var(--slate-lg); flex-shrink:0; }
.search-field input { border:none; background:transparent; font-size:0.85rem; color:var(--text); outline:none; width:100%; }
.search-field input::placeholder { color:var(--slate-lg); }
.select-field { border:1.5px solid var(--border); border-radius:var(--radius); padding:0.4rem 0.65rem; font-size:0.82rem; color:var(--text); background:var(--bg); cursor:pointer; outline:none; }
.select-field:focus { border-color:var(--accent); }
.select-sm { font-size:0.75rem; }

/* ── Table ──────────────────────────────────────────────── */
.table-loading { display:flex; align-items:center; justify-content:center; gap:0.5rem; padding:2.5rem; color:var(--slate-lg); }
.id-cell { font-size:0.78rem; color:var(--slate-lg); font-family:monospace; }
.empty-row { text-align:center; padding:3rem!important; color:var(--slate-lg); font-size:0.875rem; }
.action-btns { display:flex; justify-content:flex-end; gap:0.3rem; }
.act-del:hover { color:#dc2626!important; border-color:#dc2626!important; }

/* ── Pagination ─────────────────────────────────────────── */
.pagination-bar {
  display:flex; align-items:center; justify-content:space-between;
  padding:0.85rem 1.25rem; border-top:1px solid var(--border);
  flex-wrap:wrap; gap:0.5rem;
}
.pg-info { font-size:0.8rem; color:var(--slate-lg); }
.pg-controls { display:flex; gap:0.3rem; }
.pg-btn { padding:0.3rem 0.6rem; font-size:0.78rem; min-width:32px; justify-content:center; }
.pg-active { background:var(--accent)!important; color:#fff!important; border-color:var(--accent)!important; }

/* ── Error ──────────────────────────────────────────────── */
.err-banner { background:#fee2e2; color:#991b1b; border:1px solid #fca5a5; border-radius:var(--radius); padding:0.75rem 1.1rem; font-size:0.85rem; }

/* ── Modal ──────────────────────────────────────────────── */
.modal-backdrop { position:fixed; inset:0; background:rgba(0,0,0,0.45); z-index:999; display:flex; align-items:center; justify-content:center; padding:1rem; }
.modal-box { background:var(--card); border-radius:10px; width:100%; max-width:560px; box-shadow:0 20px 25px rgba(0,0,0,0.15); overflow:hidden; }
.modal-head { display:flex; align-items:center; justify-content:space-between; padding:1rem 1.5rem; border-bottom:1px solid var(--border); }
.modal-head h3 { font-size:1rem; font-weight:600; color:var(--text); }
.modal-close { background:none; border:none; cursor:pointer; color:var(--slate-lg); display:flex; align-items:center; padding:0.2rem; border-radius:4px; }
.modal-close:hover { background:var(--bg); color:var(--text); }
.modal-body { padding:1.25rem 1.5rem; }
.form-row { display:flex; gap:0.9rem; margin-bottom:0.9rem; flex-wrap:wrap; }
.form-field { display:flex; flex-direction:column; gap:0.3rem; flex:1; min-width:140px; }
.form-field label { font-size:0.78rem; font-weight:600; color:var(--slate); }
.req { color:#dc2626; }
.form-field input, .form-field select {
  border:1.5px solid var(--border); border-radius:var(--radius); padding:0.45rem 0.7rem;
  font-size:0.85rem; color:var(--text); background:var(--card); outline:none; transition:border-color 0.15s;
}
.form-field input:focus, .form-field select:focus { border-color:var(--accent); }
.modal-err { background:#fee2e2; color:#991b1b; border:1px solid #fca5a5; border-radius:var(--radius); padding:0.6rem 0.9rem; font-size:0.82rem; margin-bottom:0.9rem; }
.modal-footer { display:flex; justify-content:flex-end; gap:0.6rem; padding-top:0.5rem; border-top:1px solid var(--border); margin-top:0.5rem; }

@media (max-width:600px) {
  .toolbar { flex-direction:column; align-items:stretch; }
  .toolbar-spacer { display:none; }
  .action-btns { justify-content:flex-start; }
}
</style>

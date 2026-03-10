<template>
  <div class="dash">

    <!-- ── KPI Row ── -->
    <div class="kpi-row">
      <div class="kpi-card kpi-1">
        <div class="kpi-top">
          <span class="kpi-lbl">TOTAL PEOPLE</span>
          <svg class="kpi-icon" viewBox="0 0 24 24"><path d="M16 11c1.66 0 3-1.34 3-3s-1.34-3-3-3-3 1.34-3 3 1.34 3 3 3zm-8 0c1.66 0 3-1.34 3-3S9.66 5 8 5 5 6.34 5 8s1.34 3 3 3zm0 2c-2.33 0-7 1.17-7 3.5V19h14v-2.5c0-2.33-4.67-3.5-7-3.5zm8 0c-.29 0-.62.02-.97.05 1.16.84 1.97 1.97 1.97 3.45V19h6v-2.5c0-2.33-4.67-3.5-7-3.5z"/></svg>
        </div>
        <div class="kpi-val">{{ loading ? '—' : totalPeople.toLocaleString() }}</div>
        <div class="kpi-meta">{{ loading ? '' : individualsCount.toLocaleString() + ' individual customers' }}</div>
      </div>
      <div class="kpi-card kpi-2">
        <div class="kpi-top">
          <span class="kpi-lbl">TOTAL PRODUCTS</span>
          <svg class="kpi-icon" viewBox="0 0 24 24"><path d="M20 6h-2.18c.07-.44.18-.87.18-1a3 3 0 00-6 0c0 .13.11.56.18 1H4a2 2 0 00-2 2v12a2 2 0 002 2h16a2 2 0 002-2V8a2 2 0 00-2-2zm-2 13H6V9h12v10z"/></svg>
        </div>
        <div class="kpi-val">{{ loading ? '—' : totalProducts.toLocaleString() }}</div>
        <div class="kpi-meta">{{ loading ? '' : totalCategories + ' categories' }}</div>
      </div>
      <div class="kpi-card kpi-3">
        <div class="kpi-top">
          <span class="kpi-lbl">AVG LIST PRICE</span>
          <svg class="kpi-icon" viewBox="0 0 24 24"><path d="M11.8 10.9c-2.27-.59-3-1.2-3-2.15 0-1.09 1.01-1.85 2.7-1.85 1.78 0 2.44.85 2.5 2.1h2.21c-.07-1.72-1.12-3.3-3.21-3.81V3h-3v2.16c-1.94.42-3.5 1.68-3.5 3.61 0 2.31 1.91 3.46 4.7 4.13 2.5.6 3 1.48 3 2.41 0 .69-.49 1.79-2.7 1.79-2.06 0-2.87-.92-2.98-2.1h-2.2c.12 2.19 1.76 3.42 3.68 3.83V21h3v-2.15c1.95-.37 3.5-1.5 3.5-3.55 0-2.84-2.43-3.81-4.7-4.4z"/></svg>
        </div>
        <div class="kpi-val">{{ loading ? '—' : fmt(avgListPrice) }}</div>
        <div class="kpi-meta">{{ loading ? '' : fmt(maxPrice) + ' max price' }}</div>
      </div>
      <div class="kpi-card kpi-4">
        <div class="kpi-top">
          <span class="kpi-lbl">TOP CATEGORY</span>
          <svg class="kpi-icon" viewBox="0 0 24 24"><path d="M12 2l-5.5 9h11L12 2zm0 3.84L14.6 10h-5.2L12 5.84zM17.5 13c-2.49 0-4.5 2.01-4.5 4.5S15.01 22 17.5 22s4.5-2.01 4.5-4.5S19.99 13 17.5 13zm0 7c-1.38 0-2.5-1.12-2.5-2.5S16.12 15 17.5 15s2.5 1.12 2.5 2.5S18.88 20 17.5 20zM3 21.5h8v-8H3v8zm2-6h4v4H5v-4z"/></svg>
        </div>
        <div class="kpi-val" style="font-size:1.5rem">{{ loading ? '—' : topCategoryName }}</div>
        <div class="kpi-meta">{{ loading ? '' : topCategoryCount + ' products' }}</div>
      </div>
    </div>

    <!-- ── Global Search ── -->
    <div class="rsm-card search-card">
      <div class="rsm-card-body" style="padding:1rem 1.25rem">
        <div class="search-wrap">
          <svg class="search-ico" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><circle cx="11" cy="11" r="8"/><line x1="21" y1="21" x2="16.65" y2="16.65"/></svg>
          <input
            v-model="globalQuery"
            @input="onSearch"
            class="search-input"
            placeholder="Search people and products..."
            autocomplete="off"
          />
          <button v-if="globalQuery" @click="clearSearch" class="search-clear">
            <svg viewBox="0 0 24 24" width="14" height="14" fill="currentColor"><path d="M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z"/></svg>
          </button>
          <span v-if="searchLoading" class="search-spinner">
            <svg class="spin" viewBox="0 0 24 24" width="16" height="16" fill="none" stroke="currentColor" stroke-width="2.5"><circle cx="12" cy="12" r="10" stroke-opacity="0.25"/><path d="M12 2a10 10 0 0 1 10 10" stroke-opacity="1"/></svg>
          </span>
        </div>

        <!-- Results panel -->
        <div v-if="showResults" class="results-panel">
          <div class="results-col">
            <div class="results-title">
              People
              <span class="results-count">{{ searchPeople.length }} found</span>
            </div>
            <div v-if="searchPeople.length === 0" class="results-empty">No matches</div>
            <div
              v-for="p in searchPeople" :key="p.businessEntityID"
              class="result-row"
              @click="$router.push('/people')"
            >
              <span class="badge badge-navy result-badge">{{ p.personType }}</span>
              <div class="result-info">
                <span class="result-name">{{ p.fullName }}</span>
                <span class="result-sub">{{ typeLabels[p.personType] || p.personType }}</span>
              </div>
            </div>
          </div>
          <div class="results-div"></div>
          <div class="results-col">
            <div class="results-title">
              Products
              <span class="results-count">{{ searchProducts.length }} found</span>
            </div>
            <div v-if="searchProducts.length === 0" class="results-empty">No matches</div>
            <div
              v-for="p in searchProducts" :key="p.productID"
              class="result-row"
              @click="$router.push('/products')"
            >
              <span class="badge badge-blue result-badge" style="font-size:0.6rem">{{ (p.categoryName||'—').slice(0,3).toUpperCase() }}</span>
              <div class="result-info">
                <span class="result-name">{{ p.name }}</span>
                <span class="result-sub">{{ fmt(p.listPrice) }} · {{ p.categoryName || 'Uncategorized' }}</span>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- Loading / Error -->
    <div v-if="error" class="err-banner">{{ error }}</div>
    <div v-if="loading" class="loading-row">
      <svg class="spin" viewBox="0 0 24 24" width="22" height="22" fill="none" stroke="#1e3a5f" stroke-width="2.5"><circle cx="12" cy="12" r="10" stroke-opacity="0.2"/><path d="M12 2a10 10 0 0 1 10 10"/></svg>
      <span>Loading analytics…</span>
    </div>

    <template v-if="!loading && !error">
      <!-- ── Charts Row ── -->
      <div class="charts-row">

        <!-- People by Type -->
        <div class="rsm-card chart-card">
          <div class="rsm-card-head">
            <h3>Personnel Distribution</h3>
            <span class="card-label">By Person Type</span>
          </div>
          <div class="rsm-card-body">
            <div class="bar-chart">
              <div v-for="(n, type) in peopleByType" :key="type" class="bar-row">
                <div class="bar-lbl">
                  <span class="badge badge-navy">{{ type }}</span>
                  <span class="bar-name">{{ typeLabels[type] || type }}</span>
                </div>
                <div class="bar-track">
                  <div class="bar-fill fill-navy" :style="{ width: pct(n, maxPeopleType) + '%' }"></div>
                </div>
                <div class="bar-val">{{ n.toLocaleString() }}</div>
              </div>
            </div>
          </div>
        </div>

        <!-- Products by Category -->
        <div class="rsm-card chart-card">
          <div class="rsm-card-head">
            <h3>Product Catalog Distribution</h3>
            <span class="card-label">By Category</span>
          </div>
          <div class="rsm-card-body">
            <div class="bar-chart">
              <div v-for="(n, cat) in productsByCategory" :key="cat" class="bar-row">
                <div class="bar-lbl bar-lbl-plain">{{ cat }}</div>
                <div class="bar-track">
                  <div class="bar-fill fill-accent" :style="{ width: pct(n, maxCategory) + '%' }"></div>
                </div>
                <div class="bar-val">{{ n.toLocaleString() }}</div>
              </div>
            </div>
          </div>
        </div>

      </div>

      <!-- ── Second Charts Row ── -->
      <div class="charts-row">

        <!-- Email Promo -->
        <div class="rsm-card chart-card">
          <div class="rsm-card-head">
            <h3>Email Promotion Level</h3>
            <span class="card-label">Opted-in: {{ optedInPct }}%</span>
          </div>
          <div class="rsm-card-body">
            <div class="bar-chart">
              <div v-for="(n, level) in emailPromotion" :key="level" class="bar-row">
                <div class="bar-lbl bar-lbl-plain">{{ promoLabels[level] }}</div>
                <div class="bar-track">
                  <div class="bar-fill fill-teal" :style="{ width: pct(n, maxEmailPromo) + '%' }"></div>
                </div>
                <div class="bar-val">{{ n.toLocaleString() }}</div>
              </div>
            </div>
          </div>
        </div>

        <!-- Price Range -->
        <div class="rsm-card chart-card">
          <div class="rsm-card-head">
            <h3>Price Range Distribution</h3>
            <span class="card-label">Max: {{ fmt(maxPrice) }}</span>
          </div>
          <div class="rsm-card-body">
            <div class="bar-chart">
              <div v-for="(n, range) in priceRanges" :key="range" class="bar-row">
                <div class="bar-lbl bar-lbl-plain">{{ range }}</div>
                <div class="bar-track">
                  <div class="bar-fill fill-slate" :style="{ width: pct(n, maxPriceRange) + '%' }"></div>
                </div>
                <div class="bar-val">{{ n.toLocaleString() }}</div>
              </div>
            </div>
          </div>
        </div>

      </div>

      <!-- ── Top 10 Table ── -->
      <div class="rsm-card">
        <div class="rsm-card-head">
          <h3>Top 10 Most Expensive Products</h3>
          <span class="card-label">Sorted by List Price</span>
        </div>
        <div style="overflow-x:auto">
          <table class="rsm-table">
            <thead>
              <tr>
                <th>#</th>
                <th>Product Name</th>
                <th>Number</th>
                <th>Category</th>
                <th>Subcategory</th>
                <th style="text-align:right">List Price</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="(p, i) in topProducts" :key="p.productID">
                <td><span class="rank">{{ i + 1 }}</span></td>
                <td style="font-weight:500">{{ p.name }}</td>
                <td><span class="mono-sm">{{ p.productNumber }}</span></td>
                <td>{{ p.categoryName || '—' }}</td>
                <td>{{ p.subcategoryName || '—' }}</td>
                <td style="text-align:right"><span class="price-val">{{ fmt(p.listPrice) }}</span></td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>

    </template>
  </div>
</template>

<script>
import api from '../services/api'

export default {
  name: 'DashboardView',
  data() {
    return {
      loading: true,
      error: null,
      peopleData: [],
      productsData: [],
      totalPeople: 0,
      totalProducts: 0,
      // search
      globalQuery: '',
      searchTimer: null,
      searchLoading: false,
      searchPeople: [],
      searchProducts: [],
      showResults: false,
      typeLabels: { SC:'Store Contact', IN:'Individual Customer', SP:'Sales Person', EM:'Employee', VC:'Vendor Contact', GC:'General Contact' },
      promoLabels: { 0:'No Promotions', 1:'Occasional', 2:'Frequent Emails' }
    }
  },
  computed: {
    individualsCount() { return this.peopleData.filter(p => p.personType === 'IN').length },
    peopleByType() {
      const c = {}
      for (const p of this.peopleData) c[p.personType] = (c[p.personType] || 0) + 1
      return Object.fromEntries(Object.entries(c).sort((a,b) => b[1]-a[1]))
    },
    maxPeopleType() { return Math.max(...Object.values(this.peopleByType), 1) },
    emailPromotion() {
      const c = { 0: 0, 1: 0, 2: 0 }
      for (const p of this.peopleData) if (p.emailPromotion in c) c[p.emailPromotion]++
      return c
    },
    maxEmailPromo() { return Math.max(...Object.values(this.emailPromotion), 1) },
    optedInPct() {
      if (!this.totalPeople) return 0
      return Math.round(((this.emailPromotion[1]||0)+(this.emailPromotion[2]||0)) / this.totalPeople * 100)
    },
    productsByCategory() {
      const c = {}
      for (const p of this.productsData) { const k = p.categoryName||'Uncategorized'; c[k]=(c[k]||0)+1 }
      return Object.fromEntries(Object.entries(c).sort((a,b)=>b[1]-a[1]))
    },
    maxCategory() { return Math.max(...Object.values(this.productsByCategory), 1) },
    totalCategories() { return Object.keys(this.productsByCategory).filter(k=>k!=='Uncategorized').length },
    topCategoryName() {
      const e = Object.entries(this.productsByCategory)[0]; return e ? e[0] : '—'
    },
    topCategoryCount() {
      const e = Object.entries(this.productsByCategory)[0]; return e ? e[1] : 0
    },
    priceRanges() {
      const r = { '$0': 0, '$1–$99': 0, '$100–$499': 0, '$500–$999': 0, '$1,000+': 0 }
      for (const p of this.productsData) {
        const x = p.listPrice
        if (x===0) r['$0']++
        else if (x<100) r['$1–$99']++
        else if (x<500) r['$100–$499']++
        else if (x<1000) r['$500–$999']++
        else r['$1,000+']++
      }
      return r
    },
    maxPriceRange() { return Math.max(...Object.values(this.priceRanges), 1) },
    avgListPrice() {
      if (!this.productsData.length) return 0
      return this.productsData.reduce((a,p)=>a+p.listPrice,0) / this.productsData.length
    },
    maxPrice() { return this.productsData.length ? Math.max(...this.productsData.map(p=>p.listPrice)) : 0 },
    topProducts() { return [...this.productsData].sort((a,b)=>b.listPrice-a.listPrice).slice(0,10) }
  },
  methods: {
    pct(n, max) { return max ? Math.round(n/max*100) : 0 },
    fmt(v) { return new Intl.NumberFormat('en-US',{style:'currency',currency:'USD',minimumFractionDigits:0,maximumFractionDigits:2}).format(v) },
    onSearch() {
      clearTimeout(this.searchTimer)
      if (!this.globalQuery.trim()) { this.showResults = false; return }
      this.searchTimer = setTimeout(async () => {
        this.searchLoading = true
        try {
          const [pr, ar] = await Promise.all([
            api.get('/people/search',   { params: { name: this.globalQuery } }),
            api.get('/products/search', { params: { name: this.globalQuery } })
          ])
          this.searchPeople   = Array.isArray(pr.data)  ? pr.data.slice(0,8)  : []
          this.searchProducts = Array.isArray(ar.data) ? ar.data.slice(0,8) : []
          this.showResults = true
        } catch { /* ignore search errors */ }
        finally { this.searchLoading = false }
      }, 450)
    },
    clearSearch() { this.globalQuery=''; this.showResults=false; clearTimeout(this.searchTimer) },
    async loadData() {
      this.loading = true; this.error = null
      try {
        const [pr, ar] = await Promise.all([
          api.get('/people',   { params: { page:1, pageSize:5000 } }),
          api.get('/products', { params: { page:1, pageSize:5000 } })
        ])
        this.peopleData   = pr.data.items;  this.totalPeople   = pr.data.totalCount
        this.productsData = ar.data.items;  this.totalProducts = ar.data.totalCount
      } catch (e) {
        this.error = 'Could not load data. Make sure the backend is running on http://localhost:5261.'
      } finally { this.loading = false }
    }
  },
  mounted() { this.loadData() }
}
</script>

<style scoped>
/* ── Dashboard Page ─────────────────────────────────────── */
.dash { display: flex; flex-direction: column; gap: 1.25rem; }

/* ── KPI Row ────────────────────────────────────────────── */
.kpi-row { display: grid; grid-template-columns: repeat(auto-fit, minmax(200px, 1fr)); gap: 1rem; }

.kpi-card {
  background: var(--card);
  border-radius: var(--radius);
  padding: 1.25rem 1.5rem;
  box-shadow: var(--shadow);
  border-top: 4px solid transparent;
  position: relative;
}
.kpi-1 { border-top-color: #1e3a5f; }
.kpi-2 { border-top-color: #0066cc; }
.kpi-3 { border-top-color: #0d803f; }
.kpi-4 { border-top-color: #553e7a; }

.kpi-top { display: flex; justify-content: space-between; align-items: flex-start; margin-bottom: 0.6rem; }
.kpi-lbl { font-size: 0.62rem; font-weight: 700; letter-spacing: 0.09em; color: var(--slate-lg); }
.kpi-icon { width: 20px; height: 20px; fill: var(--border); flex-shrink: 0; }

.kpi-val { font-size: 2rem; font-weight: 700; color: var(--text); line-height: 1; margin-bottom: 0.3rem; }
.kpi-meta { font-size: 0.75rem; color: var(--slate-lg); }

/* ── Search ─────────────────────────────────────────────── */
.search-card { }
.search-wrap {
  display: flex; align-items: center; gap: 0.6rem;
  border: 1.5px solid var(--border); border-radius: var(--radius);
  padding: 0.5rem 0.9rem; background: var(--bg); transition: border-color 0.15s;
}
.search-wrap:focus-within { border-color: var(--accent); background: #fff; }
.search-ico { width: 16px; height: 16px; color: var(--slate-lg); flex-shrink: 0; }
.search-input { flex:1; border:none; background:transparent; font-size:0.9rem; color:var(--text); outline:none; }
.search-input::placeholder { color: var(--slate-lg); }
.search-clear { background:none; border:none; cursor:pointer; color:var(--slate-lg); display:flex; align-items:center; padding:0; }
.search-clear:hover { color:var(--text); }
.search-spinner { display:flex; align-items:center; color:var(--accent); }

.results-panel { display:grid; grid-template-columns:1fr 1px 1fr; gap:1rem; margin-top:1rem; padding-top:1rem; border-top:1px solid var(--border); }
.results-div { background:var(--border); }
.results-col { }
.results-title { font-size:0.75rem; font-weight:700; color:var(--text); margin-bottom:0.5rem; display:flex; align-items:center; gap:0.4rem; }
.results-count { font-size:0.68rem; background:var(--accent-bg); color:var(--accent); padding:0.1rem 0.45rem; border-radius:999px; font-weight:600; }
.results-empty { font-size:0.8rem; color:var(--slate-lg); padding:0.5rem 0; }
.result-row { display:flex; align-items:center; gap:0.6rem; padding:0.4rem 0.3rem; border-radius:6px; cursor:pointer; transition:background 0.1s; }
.result-row:hover { background:var(--accent-bg); }
.result-badge { font-size:0.6rem; flex-shrink:0; }
.result-info { display:flex; flex-direction:column; min-width:0; }
.result-name { font-size:0.82rem; font-weight:600; color:var(--text); white-space:nowrap; overflow:hidden; text-overflow:ellipsis; }
.result-sub  { font-size:0.72rem; color:var(--slate-lg); }

/* ── Charts ─────────────────────────────────────────────── */
.charts-row { display:grid; grid-template-columns:repeat(auto-fit, minmax(320px,1fr)); gap:1rem; }
.chart-card { }

.bar-chart { display:flex; flex-direction:column; gap:0.55rem; }
.bar-row { display:grid; grid-template-columns:160px 1fr 50px; align-items:center; gap:0.5rem; }
.bar-lbl { display:flex; align-items:center; gap:0.4rem; font-size:0.8rem; color:var(--slate); overflow:hidden; }
.bar-lbl-plain { font-size:0.8rem; color:var(--slate); white-space:nowrap; overflow:hidden; text-overflow:ellipsis; }
.bar-name { font-size:0.75rem; color:var(--slate-lg); white-space:nowrap; overflow:hidden; text-overflow:ellipsis; }
.bar-track { background:#e9edf3; border-radius:4px; height:18px; overflow:hidden; }
.bar-fill  { height:100%; border-radius:4px; min-width:3px; transition:width 0.5s ease; }
.fill-navy   { background:linear-gradient(90deg,#1e3a5f,#2a4f7c); }
.fill-accent { background:linear-gradient(90deg,#0052a3,#0066cc); }
.fill-teal   { background:linear-gradient(90deg,#0d6e6e,#0e9090); }
.fill-slate  { background:linear-gradient(90deg,#374151,#4a5568); }
.bar-val { font-size:0.75rem; font-weight:600; color:var(--text); text-align:right; }

/* ── Top Table ──────────────────────────────────────────── */
.rank { display:inline-flex; align-items:center; justify-content:center; width:22px; height:22px; background:var(--border); border-radius:50%; font-size:0.72rem; font-weight:700; color:var(--slate); }
.mono-sm { font-family:monospace; font-size:0.8rem; color:var(--slate-lg); }
.price-val { font-weight:700; color:#0d803f; }

/* ── Error / Loading ────────────────────────────────────── */
.err-banner { background:#fee2e2; color:#991b1b; border:1px solid #fca5a5; border-radius:var(--radius); padding:0.85rem 1.25rem; font-size:0.85rem; }
.loading-row { display:flex; align-items:center; gap:0.6rem; color:var(--slate-lg); padding:3rem 0; justify-content:center; font-size:0.9rem; }
</style>

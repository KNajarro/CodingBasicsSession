<template>
  <div id="app" :class="{ 'mob-open': mobileOpen }">

    <!-- Mobile overlay -->
    <div class="sidebar-overlay" @click="mobileOpen = false"></div>

    <!-- ── Sidebar ── -->
    <aside class="rsm-sidebar">
      <div class="sidebar-brand">
        <div class="brand-logo">AW</div>
        <div class="brand-text-wrap">
          <span class="brand-name">AdventureWorks</span>
          <span class="brand-sub">Workshop</span>
        </div>
      </div>

      <nav class="sidebar-nav">
        <span class="nav-section">MENU</span>

        <router-link to="/" exact-active-class="nav-active" class="nav-item" @click="mobileOpen = false">
          <svg class="nav-icon" viewBox="0 0 24 24"><path d="M10 20v-6h4v6h5v-8h3L12 3 2 12h3v8z"/></svg>
          <span>Home</span>
        </router-link>

        <router-link to="/dashboard" active-class="nav-active" class="nav-item" @click="mobileOpen = false">
          <svg class="nav-icon" viewBox="0 0 24 24"><path d="M3 13h8V3H3v10zm0 8h8v-6H3v6zm10 0h8V11h-8v10zm0-18v6h8V3h-8z"/></svg>
          <span>Dashboard</span>
        </router-link>

        <router-link to="/people" active-class="nav-active" class="nav-item" @click="mobileOpen = false">
          <svg class="nav-icon" viewBox="0 0 24 24"><path d="M16 11c1.66 0 3-1.34 3-3s-1.34-3-3-3-3 1.34-3 3 1.34 3 3 3zm-8 0c1.66 0 3-1.34 3-3S9.66 5 8 5 5 6.34 5 8s1.34 3 3 3zm0 2c-2.33 0-7 1.17-7 3.5V19h14v-2.5c0-2.33-4.67-3.5-7-3.5zm8 0c-.29 0-.62.02-.97.05 1.16.84 1.97 1.97 1.97 3.45V19h6v-2.5c0-2.33-4.67-3.5-7-3.5z"/></svg>
          <span>People</span>
        </router-link>

        <router-link to="/products" active-class="nav-active" class="nav-item" @click="mobileOpen = false">
          <svg class="nav-icon" viewBox="0 0 24 24"><path d="M20 6h-2.18c.07-.44.18-.87.18-1a3 3 0 00-6 0c0 .13.11.56.18 1H4a2 2 0 00-2 2v12a2 2 0 002 2h16a2 2 0 002-2V8a2 2 0 00-2-2zm-2 13H6V9h12v10z"/></svg>
          <span>Products</span>
        </router-link>
      </nav>

      <div class="sidebar-footer">
        <svg viewBox="0 0 24 24" width="12" height="12" fill="currentColor"><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z"/></svg>
        AdventureWorks DB
      </div>
    </aside>

    <!-- ── Main ── -->
    <div class="main-wrapper">
      <header class="rsm-topbar">
        <button class="hamburger" @click="mobileOpen = !mobileOpen" aria-label="Menu">
          <span></span><span></span><span></span>
        </button>
        <h1 class="page-title">{{ pageTitle }}</h1>
        <div class="topbar-badge">AdventureWorks</div>
      </header>
      <main class="page-body">
        <router-view />
      </main>
    </div>

  </div>
</template>

<script>
export default {
  name: 'App',
  data() { return { mobileOpen: false } },
  computed: {
    pageTitle() {
      return { home: 'Home', dashboard: 'Analytics Dashboard', people: 'People', products: 'Products' }[this.$route.name] || 'AdventureWorks'
    }
  }
}
</script>

<style>
/* ── Design Tokens ──────────────────────────────────────── */
:root {
  --sw: 240px;
  --th: 60px;
  --navy:        #1e3a5f;
  --navy-dark:   #152c47;
  --navy-hover:  #243f68;
  --accent:      #0066cc;
  --accent-bg:   #e8f0fe;
  --slate:       #4a5568;
  --slate-lg:    #718096;
  --bg:          #f0f4f8;
  --card:        #ffffff;
  --border:      #e2e8f0;
  --text:        #1a202c;
  --radius:      8px;
  --shadow:      0 1px 3px rgba(0,0,0,0.1), 0 1px 2px rgba(0,0,0,0.06);
  --shadow-md:   0 4px 6px rgba(0,0,0,0.07), 0 2px 4px rgba(0,0,0,0.06);
}

*, *::before, *::after { box-sizing: border-box; margin: 0; padding: 0; }

html { height: 100%; width: 100%; }

body {
  font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, 'Helvetica Neue', Arial, sans-serif;
  background: var(--bg);
  color: var(--text);
  font-size: 14px;
  line-height: 1.5;
  width: 100%;
  min-height: 100%;
}

/* ── App Shell ──────────────────────────────────────────── */
#app { display: flex; min-height: 100vh; width: 100%; }

/* ── Sidebar ────────────────────────────────────────────── */
.rsm-sidebar {
  width: var(--sw);
  background: var(--navy);
  color: rgba(255,255,255,0.85);
  flex-shrink: 0;
  display: flex;
  flex-direction: column;
  position: sticky;
  top: 0;
  height: 100vh;
  overflow-y: auto;
  z-index: 100;
}

.sidebar-brand {
  display: flex;
  align-items: center;
  gap: 0.75rem;
  padding: 1.25rem;
  border-bottom: 1px solid rgba(255,255,255,0.08);
}
.brand-logo {
  width: 36px; height: 36px;
  background: var(--accent);
  border-radius: 6px;
  display: flex; align-items: center; justify-content: center;
  font-weight: 800; font-size: 0.78rem; color: #fff; flex-shrink: 0;
  letter-spacing: 0.05em;
}
.brand-text-wrap { display: flex; flex-direction: column; line-height: 1.2; }
.brand-name { font-size: 0.8rem; font-weight: 700; color: #fff; }
.brand-sub  { font-size: 0.68rem; color: rgba(255,255,255,0.45); }

.sidebar-nav {
  flex: 1;
  padding: 1rem 0;
  display: flex;
  flex-direction: column;
}
.nav-section {
  font-size: 0.62rem;
  font-weight: 700;
  letter-spacing: 0.1em;
  color: rgba(255,255,255,0.28);
  padding: 0.5rem 1.25rem 0.25rem;
}
.nav-item {
  display: flex;
  align-items: center;
  gap: 0.72rem;
  padding: 0.65rem 1.25rem;
  color: rgba(255,255,255,0.68);
  text-decoration: none;
  font-size: 0.875rem;
  font-weight: 500;
  border-left: 3px solid transparent;
  transition: background 0.15s, color 0.15s, border-color 0.15s;
}
.nav-item:hover { background: rgba(255,255,255,0.07); color: #fff; border-left-color: rgba(255,255,255,0.25); }
.nav-item.nav-active { background: rgba(0,102,204,0.22); color: #fff; border-left-color: var(--accent); }
.nav-icon { width: 17px; height: 17px; fill: currentColor; flex-shrink: 0; }

.sidebar-footer {
  padding: 0.85rem 1.25rem;
  border-top: 1px solid rgba(255,255,255,0.08);
  display: flex; align-items: center; gap: 0.4rem;
  font-size: 0.68rem; color: rgba(255,255,255,0.3);
}

/* ── Main Wrapper ───────────────────────────────────────── */
.main-wrapper { flex: 1; min-width: 0; display: flex; flex-direction: column; }

.rsm-topbar {
  height: var(--th);
  background: var(--card);
  border-bottom: 1px solid var(--border);
  display: flex;
  align-items: center;
  padding: 0 1.5rem;
  gap: 0.75rem;
  position: sticky;
  top: 0;
  z-index: 50;
  box-shadow: var(--shadow);
}
.hamburger { display: none; flex-direction: column; gap: 4px; background: none; border: none; cursor: pointer; padding: 4px; }
.hamburger span { display: block; width: 20px; height: 2px; background: var(--slate); border-radius: 2px; }
.page-title { font-size: 1rem; font-weight: 600; color: var(--text); flex: 1; }
.topbar-badge { font-size: 0.72rem; background: var(--accent-bg); color: var(--accent); padding: 0.18rem 0.65rem; border-radius: 999px; font-weight: 600; }

.page-body { flex: 1; padding: 1.75rem 2rem; overflow-x: hidden; }

/* ── Global utility classes (used by child views) ───────── */
.rsm-card      { background: var(--card); border: 1px solid var(--border); border-radius: var(--radius); box-shadow: var(--shadow); }
.rsm-card-body { padding: 1.25rem 1.5rem; }
.rsm-card-head { padding: 1rem 1.5rem; border-bottom: 1px solid var(--border); display: flex; align-items: center; justify-content: space-between; }
.rsm-card-head h3 { font-size: 0.875rem; font-weight: 600; color: var(--text); }
.rsm-card-head .card-label { font-size: 0.68rem; font-weight: 700; letter-spacing: 0.08em; text-transform: uppercase; color: var(--slate-lg); }

.rsm-btn {
  display: inline-flex; align-items: center; gap: 0.35rem;
  padding: 0.45rem 0.9rem; border-radius: var(--radius);
  font-size: 0.8rem; font-weight: 600; border: none; cursor: pointer;
  transition: opacity 0.15s, background 0.15s;
}
.rsm-btn:disabled { opacity: 0.55; cursor: not-allowed; }
.rsm-btn-primary  { background: var(--accent); color: #fff; }
.rsm-btn-primary:hover:not(:disabled)  { background: #0052a3; }
.rsm-btn-secondary { background: var(--card); color: var(--slate); border: 1px solid var(--border); }
.rsm-btn-secondary:hover:not(:disabled) { background: var(--bg); }
.rsm-btn-danger   { background: #dc2626; color: #fff; }
.rsm-btn-danger:hover:not(:disabled)   { background: #b91c1c; }
.rsm-btn-ghost    { background: transparent; color: var(--slate); border: 1px solid transparent; padding: 0.35rem 0.6rem; }
.rsm-btn-ghost:hover { background: var(--bg); border-color: var(--border); }

.badge {
  display: inline-block;
  padding: 0.15rem 0.55rem;
  border-radius: 999px;
  font-size: 0.68rem; font-weight: 700; letter-spacing: 0.02em;
}
.badge-navy   { background: var(--navy);  color: #fff; }
.badge-blue   { background: #dbeafe; color: #1d4ed8; }
.badge-green  { background: #dcfce7; color: #166534; }
.badge-yellow { background: #fef9c3; color: #854d0e; }
.badge-gray   { background: #f1f5f9; color: var(--slate); }
.badge-red    { background: #fee2e2; color: #991b1b; }

/* ── Table base (used in PeopleView / ProductsView) ─────── */
.rsm-table { width: 100%; border-collapse: collapse; font-size: 0.85rem; }
.rsm-table thead tr { background: #f8fafc; }
.rsm-table th {
  padding: 0.7rem 1rem; text-align: left;
  font-size: 0.68rem; font-weight: 700; letter-spacing: 0.07em; text-transform: uppercase;
  color: var(--slate-lg); border-bottom: 2px solid var(--border); white-space: nowrap;
}
.rsm-table td { padding: 0.7rem 1rem; border-bottom: 1px solid var(--border); color: var(--text); }
.rsm-table tbody tr:hover td { background: var(--accent-bg); }
.rsm-table tbody tr:last-child td { border-bottom: none; }

.spin { animation: spin 0.8s linear infinite; }
@keyframes spin { to { transform: rotate(360deg); } }

/* ── Mobile responsive ──────────────────────────────────── */
.sidebar-overlay { display: none; }

@media (max-width: 900px) {
  .rsm-sidebar {
    position: fixed;
    left: 0; top: 0; bottom: 0;
    transform: translateX(-100%);
    transition: transform 0.25s ease;
  }
  #app.mob-open .rsm-sidebar { transform: translateX(0); }
  .sidebar-overlay {
    display: block; position: fixed; inset: 0;
    background: rgba(0,0,0,0.45); z-index: 99;
    opacity: 0; pointer-events: none; transition: opacity 0.25s;
  }
  #app.mob-open .sidebar-overlay { opacity: 1; pointer-events: all; }
  .hamburger { display: flex; }
  .page-body { padding: 1rem; }
}
</style>

<template>
  <div id="app">
    <nav class="navbar">
      <div class="navbar-brand"><h1>AdventureWorks Workshop</h1></div>
      <div class="navbar-menu">
        <router-link to="/" class="nav-link">Home</router-link>
        <router-link to="/people" class="nav-link">People</router-link>
        <router-link to="/products" class="nav-link">Products</router-link>
        <router-link to="/dashboard" class="nav-link">Dashboard</router-link>
      </div>
    </nav>
    <main class="container">
      <router-view />
    </main>
  </div>
</template>

<script>
export default { name: "App" };
</script>

<style>
.navbar {
  background-color: #2c3e50;
  padding: 1rem 2rem;
  display: flex;
  justify-content: space-between;
  align-items: center;
}
.navbar-brand h1 {
  color: white;
  font-size: 1.5rem;
  margin: 0;
}
.navbar-menu {
  display: flex;
  gap: 1rem;
}
.nav-link {
  color: #ecf0f1;
  text-decoration: none;
  padding: 0.5rem 1rem;
  border-radius: 4px;
  transition: background-color 0.3s;
}
.nav-link:hover,
.nav-link.router-link-active {
  background-color: #34495e;
}
.container {
  max-width: 1200px;
  margin: 0 auto;
  padding: 2rem;
}
</style>

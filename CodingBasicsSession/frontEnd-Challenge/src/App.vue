<template>
  <div id="app">
    <nav class="navbar navbar-dark bg-dark">
      <div class="container-fluid">
        <span class="navbar-brand mb-0 h1">
          <i class="bi bi-graph-up"></i> Adventure Works Dashboard
        </span>
      </div>
    </nav>
    <main class="container-fluid py-4">
      <router-view />
    </main>
  </div>
</template>

<script>
export default {
  name: 'App'
}
</script>

<style scoped>
#app {
  min-height: 100vh;
  background-color: #f5f5f5;
}

.navbar {
  box-shadow: 0 2px 4px rgba(0,0,0,0.1);
}
</style>

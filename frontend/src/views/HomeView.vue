<template>
  <div class="home">
    <h2>Welcome to AdventureWorks Workshop</h2>
    <div class="info-cards">
      <div class="card">
        <h3>Workshop Goals</h3>
        <ul>
          <li>Connect Vue frontend to .NET API</li>
          <li>Implement service methods with Axios</li>
          <li>Display data in tables</li>
          <li>Add search functionality</li>
        </ul>
      </div>
      <div class="card">
        <h3>API Endpoints</h3>
        <ul>
          <li><code>GET /api/people</code></li>
          <li><code>GET /api/people/search?name=&amp;personType=</code></li>
          <li><code>GET /api/products</code></li>
          <li><code>GET /api/products/search?name=&amp;categoryName=</code></li>
        </ul>
      </div>
      <div class="card">
        <h3>Person Types</h3>
        <ul>
          <li><strong>SC</strong>  -  Store Contact</li>
          <li><strong>IN</strong>  -  Individual</li>
          <li><strong>SP</strong>  -  Sales Person</li>
          <li><strong>EM</strong>  -  Employee</li>
          <li><strong>VC</strong>  -  Vendor Contact</li>
          <li><strong>GC</strong>  -  General Contact</li>
        </ul>
      </div>
    </div>
  </div>
</template>
<script>
export default { name: 'HomeView' }
</script>
<style scoped>
.home h2 { margin-bottom: 2rem; color: #2c3e50; }
.info-cards { display: grid; grid-template-columns: repeat(auto-fit, minmax(300px, 1fr)); gap: 1.5rem; }
.card { background: white; border-radius: 8px; padding: 1.5rem; box-shadow: 0 2px 4px rgba(0,0,0,.1); }
.card h3 { color: #2c3e50; margin-bottom: 1rem; padding-bottom: .5rem; border-bottom: 2px solid #3498db; }
.card ul { list-style: none; padding: 0; }
.card li { padding: .5rem 0; border-bottom: 1px solid #eee; }
.card li:last-child { border-bottom: none; }
code { background: #f4f4f4; padding: .2rem .5rem; border-radius: 4px; font-size: .9rem; }
</style>
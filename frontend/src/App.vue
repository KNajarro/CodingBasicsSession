<template>
  <div class="container">
    <h1>Coding Session 2 (Workshop)</h1>

    <section>
      <h2>Person Filter</h2>
      <input v-model="filterPerson.name" placeholder="Search by Name">
      <input v-model="filterPerson.type" placeholder="Search by Type">
      <button @click="fetchPersons">Filter</button>

      <ul>
        <li v-for="person in persons" :key="person.businessEntityId">
          {{ person.firstName }} {{ person.lastName }} - <strong>{{ person.personType }}</strong>
        </li>
      </ul>
    </section>

    <hr>

    <section>
      <h2>Product Filter</h2>
      <input v-model="filterProduct.name" placeholder="Product Name">
      <button @click="fetchProducts">Filter</button>

      <ul>
        <li v-for="product in products" :key="product.productId">
          {{ product.name }} - {{ product.productNumber }}
        </li>
      </ul>
    </section>
  </div>
</template>

<script setup>
import { ref } from 'vue';

const API_URL = "http://localhost:5261/api";

// 1. IMPORTANTE: Declarar loading
const loading = ref(false); 

const persons = ref([]);
const filterPerson = ref({ name: '', type: '' });

const products = ref([]);
const filterProduct = ref({ name: '' });

const fetchPersons = async () => {
  loading.value = true;
  
  // 2. URL base corregida, 
  let url = `${API_URL}/people`; 

  if (filterPerson.value.name || filterPerson.value.type) {
    const params = new URLSearchParams();
    if (filterPerson.value.name) params.append('name', filterPerson.value.name);
    if (filterPerson.value.type) params.append('personType', filterPerson.value.type);
    
    url = `${API_URL}/people/search?${params.toString()}`;
  }

  try {
    const response = await fetch(url);
    if (!response.ok) throw new Error("Server Error");
    
    const data = await response.json();
    
    // 3. Validar si los datos vienen en .items o directo
    persons.value = data.items ? data.items : data;

  } catch (error) {
    console.error("Error:", error);
    persons.value = [];
  } finally {
    loading.value = false;
  }
};
// Lógica para Productos

const fetchProducts = async () => {
  loading.value = true;
    
  let url = `${API_URL}/products`;

  if (filterProduct.value.name) {
    const params=new URLSearchParams();
    params.append('name',filterProduct.value.name);
    url = `${API_URL}/products/search?name=${encodeURIComponent(filterProduct.value.name)}`;
  }

  try {
    const response = await fetch(url);
    if (!response.ok) throw new Error("Error trying to obtain Products");
    
    // This will be the complete array, no items involved
    const data= await response.json();

    //This way we ensure a response when whe have an array (data) or
    //specific items data.items

    products.value=data.items?data.items:data;


  } catch (error) {
    console.error("Error in products:", error);
    products.value = [];
  } finally {
    loading.value = false;
  }
};
</script>

<style scoped>
.container { font-family: sans-serif; padding: 20px; }
section { margin-bottom: 30px; }
input { margin-right: 10px; padding: 5px; }
button { padding: 5px 15px; cursor: pointer; }
ul { margin-top: 15px; }
</style>
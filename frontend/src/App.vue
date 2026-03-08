<template>
  <div class="container">
    <h1>Coding Session 2 (Workshop)</h1>

    <section>
      <h2>Person Filter</h2>
      <input v-model="filterPerson.name" placeholder="Buscar por nombre">
      <input v-model="filterPerson.type" placeholder="Buscar por tipo (ej: EM, SP)">
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
      <input v-model="filterProduct.name" placeholder="Nombre del producto">
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

// API URL 
const API_URL = "https://localhost:5261/api";  // The port reflected locally is 5261 , change it accordly

const persons = ref([]);
const filterPerson = ref({ name: '', type: '' });

const products = ref([]);
const filterProduct = ref({ name: '' });

// Lógica para Personas
const fetchPersons = async () => {
  loading.value = true; // Recomendado para feedback visual
  
  let url = `${API_URL}/people`; 

  
  if (filterPerson.value.name || filterPerson.value.type) {
    const params = new URLSearchParams();
    if (filterPerson.value.name) params.append('name', filterPerson.value.name);
    if (filterPerson.value.type) params.append('personType', filterPerson.value.type);
    
    url = `${API_URL}/people/search?${params.toString()}`;
  }

  try {
    const response = await fetch(url);
    if (!response.ok) throw new Error("Error en la respuesta del servidor");
    
    const data = await response.json();

   
    if (data.items) {
      persons.value = data.items; // Para el endpoint /api/people
    } else {
      persons.value = data; // Para el endpoint /api/people/search
    }

  } catch (error) {
    console.error("Error al conectar con la API:", error);
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
    // waiting for name parameter
    url = `${API_URL}/products/search?name=${encodeURIComponent(filterProduct.value.name)}`;
  }

  try {
    const response = await fetch(url);
    if (!response.ok) throw new Error("Error al obtener productos");
    
    // returns an array
    products.value = await response.json();
  } catch (error) {
    console.error("Error en productos:", error);
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
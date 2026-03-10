<template>
  <nav class="p-4 bg-gray-800 text-white flex gap-4">
    <router-link to="/">Dashboard</router-link>
    <router-link to="/products">Products List</router-link>
    <router-link to="/people">People List</router-link>
  </nav>

  <router-view />
</template>
import { createRouter, createWebHistory } from 'vue-router'
import HomeView             from '../views/HomeView.vue'
import PeopleView           from '../views/PeopleView.vue'
import ProductsView         from '../views/ProductsView.vue'
import InventoryDashboard   from '../views/InventoryDashboard.vue'

const routes = [
  { path: '/',           name: 'home',      component: HomeView           },
  { path: '/people',     name: 'people',    component: PeopleView         },
  { path: '/products',   name: 'products',  component: ProductsView       },
  { path: '/dashboard',  name: 'dashboard', component: InventoryDashboard }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

export default router
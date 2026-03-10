import { createRouter, createWebHistory } from 'vue-router'
import Dashboard from '../views/Dashboard.vue'
import DashboardView from '../views/DashboardView.vue'

const routes = [
  { path: '/', name: 'dashboard', component: Dashboard },
  { path: '/dashboard', name: 'analytics', component: DashboardView }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

export default router

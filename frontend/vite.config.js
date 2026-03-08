import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'

export default defineConfig({
  plugins: [vue()],
  server: {
    port: 3000,
    proxy: {
      // All requests starting with /api are forwarded to the .NET API
      '/api': {
        target: 'http://localhost:5261',
        changeOrigin: true
      }
    }
  }
})

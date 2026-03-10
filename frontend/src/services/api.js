import axios from 'axios'

// Shared Axios instance. All requests use /api as the base URL.
// The Vite dev server proxy forwards these to http://localhost:5000.
const api = axios.create({
  baseURL: 'http://localhost:5261',  //changed to match the one found in C:\CodingBasicsSession\src\CodingBasics.Api\Properties\launchSettings.json, make sure CORS is enable in Program.cs
  timeout: 10000,
  headers: { 'Content-Type': 'application/json' }
})

api.interceptors.response.use(
  response => response,
  error => {
    console.error('API Error:', error.response?.data || error.message)
    return Promise.reject(error)
  }
)

export default api
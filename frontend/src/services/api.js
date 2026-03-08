import axios from "axios";

// Shared Axios instance. All requests use /api as the base URL.
// The Vite dev server proxy forwards these to http://localhost:5000.
const api = axios.create({
  baseURL: "/api",
  timeout: 30000,
  headers: { "Content-Type": "application/json" },
});

api.interceptors.response.use(
  (response) => response,
  (error) => {
    console.error("API Error:", error.response?.data || error.message);
    return Promise.reject(error);
  },
);

export default api;

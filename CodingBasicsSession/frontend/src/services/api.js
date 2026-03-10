import axios from 'axios'

function getFriendlyErrorMessage(error) {
  const status = error?.response?.status
  const payload = error?.response?.data
  const payloadText = typeof payload === 'string'
    ? payload
    : JSON.stringify(payload || {})

  if (status === 404) return 'El registro no existe o fue eliminado.'
  if (status === 400) return 'La solicitud es inválida. Revisa los campos enviados.'

  if (payloadText.includes('Cannot insert the value NULL into column')) {
    return 'No se pudo crear el registro por una restricción de base de datos (campo requerido en backend).'
  }

  if (payloadText.includes('target table has database triggers') || payloadText.includes('OUTPUT clause')) {
    return 'No se pudo guardar por una configuración de triggers en SQL Server.'
  }

  if (payloadText.includes('DbUpdateException')) {
    return 'No se pudieron guardar los cambios en base de datos.'
  }

  if (status && status >= 500) {
    return 'Error interno del servidor. Intenta de nuevo en unos segundos.'
  }

  return 'No se pudo completar la operación. Verifica el backend y vuelve a intentar.'
}

// Shared Axios instance. All requests use /api as the base URL.
// The Vite dev server proxy forwards these to http://localhost:5000.
const api = axios.create({
  baseURL: '/api',
  timeout: 10000,
  headers: { 'Content-Type': 'application/json' }
})

api.interceptors.response.use(
  response => response,
  error => {
    error.friendlyMessage = getFriendlyErrorMessage(error)
    console.error('API Error:', error.response?.data || error.message)
    return Promise.reject(error)
  }
)

export default api
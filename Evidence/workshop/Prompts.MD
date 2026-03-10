# localhost:3000
# Primer Prompt, luego ejecución del PLANNING.MD:

ok necesito que me hagas un planning para hacer las operaciones CRUD de la entidad Person y Products. Aparte de las operaciones básicas del CRUD, necesito que hagas el planning para agregar estos endpoints que están acá en forma de método:

Person:
GetAll

GetPersonByName (string name)

GetPersonByPersonType (string personType)

GetPersonByNameAqndPersonType(string name, string personType)

Products:

GetAll

GetProductByName (string name)

GetProductsByCategoryType (string categoryType)

Hazlo siguiendo buenas prácticas y principios SOLID. Pon el planning en un markdown.



# Segundo Prompt:

PARTE 1: BACKEND (.NET)
Actualmente tengo endpoints en Program.cs para obtener Products y People (y sus búsquedas), pero necesito endpoints para llenar los comboboxes del frontend.
Escribe el código necesario (indicando qué interfaces de la capa Application y qué repositorios de Infrastructure modificar, además de las rutas en Program.cs) para crear:

Un endpoint GET /api/products/categories que devuelva una lista de los nombres de categorías (categoryName) únicos y válidos desde la base de datos.
Un endpoint GET /api/people/types que devuelva una lista de los tipos de persona (personType) únicos y válidos desde la base de datos.


# Tercer Prompt:

PARTE 2: FRONTEND (Vue.js)

Actúa como un Senior Frontend Developer experto en Vue.js 3 (usando Composition API y `<script setup>`). Necesito que construyas un Dashboard tipo Single Page Application (SPA) para consumir una API construida en ASP.NET Core Minimal APIs.

### Requerimientos generales del Dashboard:

1. **Vista Unificada con Tabs/Botones:** Todo debe estar en una sola página principal (`Dashboard.vue`). Debe haber un control (botones o pestañas) para alternar dinámicamente entre el "Dashboard de People" y el "Dashboard de Products".
2. **Arquitectura de Componentes:** Prohibido poner todo el código en un solo archivo. Debes crear una estructura escalable, limpia y mantenible. Te sugiero la siguiente estructura mínima:
   - `views/Dashboard.vue` (Contenedor principal y lógica de pestañas)
   - `components/People/PeopleView.vue` (Vista de personas)
   - `components/Products/ProductsView.vue` (Vista de productos)
   - `components/shared/DataTable.vue` (Componente genérico reutilizable para las tablas)
   - `services/api.js` o composables (`usePeople.js`, `useProducts.js`) para centralizar las peticiones `fetch` o `axios` al backend.
3. **Carga Inicial (Get All):** Al montar o activar cada vista (People o Products), se debe llamar inmediatamente a su respectivo endpoint principal para llenar la tabla con los datos por defecto.

### Requerimientos específicos por entidad (Solo Lectura):

#### Sección People:
- **Tabla de datos:** Debe consumir `GET /api/people` (retorna `{ page, pageSize, totalCount, items }`, muestra el array `items`).
- **Filtros de Búsqueda:** Encima de la tabla debe haber un formulario con:
  - Un `input` de texto para el campo `name`.
  - Un `select` (combobox) para `personType`. Este combobox debe llenarse dinámicamente consumiendo `GET /api/people/types` al cargar la vista.
  - Un botón "Buscar" que, al hacer clic, consuma `GET /api/people/search?name={valor}&personType={valor}` y actualice la tabla con esos resultados.

#### Sección Products:
- **Tabla de datos:** Debe consumir `GET /api/products` (retorna `{ page, pageSize, totalCount, items }`, muestra el array `items`).
- **Filtros de Búsqueda:** Encima de la tabla debe haber un formulario con:
  - Un `input` de texto para el campo `name`.
  - Un `select` (combobox) para `categoryName`. Este combobox debe llenarse dinámicamente consumiendo `GET /api/products/categories` al cargar la vista.
  - Un botón "Buscar" que, al hacer clic, consuma `GET /api/products/search?name={valor}&categoryName={valor}` y actualice la tabla con los resultados.

### Consideraciones de Código:
- Usa estilos limpios y modernos (Tailwind CSS o CSS puro bien estructurado).
- Maneja los estados de carga (`loading`) y posibles errores de red de forma amigable.
- Asegúrate de que los componentes sean legibles, con responsabilidades únicas.
- Entrégame el código paso a paso para los archivos clave mencionados, explicando brevemente cómo se conectan.
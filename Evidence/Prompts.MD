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


# Prompt 4, prompt para el challenge técnico del Dashboard:

#codebase #file:SKILL.md 

# Contexto General
Actúa como un Senior Full-Stack Developer experto en ASP.NET Core (Minimal APIs, Entity Framework Core) y Vue 3 (Composition API). 
Tu tarea es implementar la "Parte 2" de un desafío técnico: un Dashboard para la base de datos de AdventureWorks. 
Debes generar todo el código necesario respetando estrictamente la arquitectura actual del proyecto. No generes código espagueti; modulariza los componentes y servicios.

El desafío tiene 4 requerimientos para el Dashboard:
1. Inventory Value: Un KPI card con la suma total de ListPrice de todos los productos.
2. Distribution by Color: Un gráfico de dona/pastel mostrando la cantidad de productos por color.
3. Customer Analysis: Un gráfico de barras mostrando la cantidad de personas agrupadas por PersonType.
4. Stock Management: Una tabla que resalte o filtre productos con un SafetyStockLevel bajo. Esta tabla debe estar paginada de 20 en 20.

Divide tu respuesta y la generación de código en las siguientes dos fases:

## Fase 1: Backend (ASP.NET Core)
El proyecto usa Minimal APIs en Program.cs e inyecta servicios (IProductService, IPersonService).
Genera el código para agregar 4 nuevos endpoints optimizados para el Dashboard. La lógica de base de datos no debe ir en Program.cs, sino en las interfaces e implementaciones de los servicios.

1. Escribe el código para agregar las firmas de estos 4 métodos en IProductService e IPersonService dentro de la capa de Dominio/Aplicación.
2. Escribe la implementación de estos métodos en las clases de servicio correspondientes usando Entity Framework Core de forma asíncrona (SumAsync, GroupBy, Where). Presta especial atención al método de bajo stock: debe soportar paginación (recibiendo `page` y devolviendo un tamaño de página de 20 elementos junto con el total de registros para el frontend).
3. Escribe el bloque de código para registrar los 4 nuevos Minimal APIs en Program.cs (bajo un comentario // DASHBOARD ENDPOINTS), inyectando los servicios correspondientes y retornando Results.Ok().

## Fase 2: Frontend (Vue 3)
El proyecto en la carpeta frontend/ ya cuenta con Vite, Vue Router y Axios. Las dependencias `chart.js` y `vue-chartjs` **ya están instaladas**, por lo que solo debes importarlas y usarlas.

1. Servicios: Crea un archivo src/services/dashboardService.js que utilice la instancia de Axios existente para consumir los 4 endpoints creados en la Fase 1.
2. Componentes Modulares: Genera el código para 4 componentes separados dentro de src/components/dashboard/:
   - InventoryKpiCard.vue (Recibe el total por props y lo formatea como moneda).
   - ColorDistributionChart.vue (Importa y utiliza el DoughnutChart de vue-chartjs).
   - CustomerAnalysisChart.vue (Importa y utiliza el BarChart de vue-chartjs).
   - LowStockTable.vue (Debe mostrar los datos consumiendo la API paginada, incluir controles de paginación para avanzar o retroceder de 20 en 20, y aplicar un estilo CSS condicional para resaltar las filas con bajo stock).
3. Vista Principal: Genera el código para src/views/DashboardView.vue usando el <script setup> (Composition API). Esta vista debe importar los 4 componentes, llamar a dashboardService en el hook onMounted, manejar estados reactivos (ref) para los datos, y pasar estos datos a los componentes mediante props. Usa CSS Grid o Flexbox para un diseño limpio y moderno.
4. Enrutamiento: Muestra cómo registrar esta nueva vista en src/router/index.js bajo la ruta /dashboard.

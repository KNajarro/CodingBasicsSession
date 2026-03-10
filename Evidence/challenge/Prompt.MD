# Prompt para el challenge técnico del Dashboard: direccion: localhost:3000/dashboard

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

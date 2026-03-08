# Planning CRUD — Person & Products

## Resumen del Estado Actual

| Capa | Person | Products |
|------|--------|----------|
| **Entidades** (Domain) | ✅ Completa | ✅ Completa |
| **DTOs** (Domain) | ✅ Completa | ✅ Completa |
| **Contratos** (Domain) | ⚠️ Solo `GetAll` + `Search` definidos, CRUD comentado | ⚠️ Solo `GetAll` + `Search` definidos, CRUD comentado |
| **Repositorio** (Infrastructure) | ⚠️ Solo `GetAll` implementado, `Search` lanza excepción | ❌ Todo lanza excepción |
| **Servicio** (Application) | ⚠️ Solo `GetAll` implementado, `Search` lanza excepción | ❌ Sin inyección de repositorio, todo lanza excepción |
| **DI Registration** | ✅ Person registrado | ❌ Product comentado en ambas capas |
| **Endpoints API** | ⚠️ Solo `GET /api/people` funcional, search/CRUD comentado | ❌ Todo comentado |

---

## Arquitectura del Proyecto (Clean Architecture)

```
Api (Presentación) → Application (Lógica de Negocio) → Domain (Contratos/Entidades)
                                                          ↑
Infrastructure (Acceso a Datos) ──────────────────────────┘
```

- **Domain**: Entidades, DTOs, interfaces (contratos)
- **Application**: Implementación de servicios (lógica de negocio)
- **Infrastructure**: Implementación de repositorios (EF Core), DbContext
- **Api**: Minimal API endpoints

---

## Plan de Implementación

### Fase 1 — Person: Completar Operaciones de Lectura

#### Paso 1.1: Implementar `SearchAsync` en `PersonRepository`
> **Archivo**: `src/CodingBasics.Infrastructure/Repositories/Repositories.cs`
>
> **Qué hacer**: Descomentar e implementar la lógica de `SearchAsync` en `PersonRepository`. Construir una query dinámica con filtros opcionales por `name` y `personType`:
> - Si `name` no es nulo, filtrar donde `FirstName` o `LastName` contengan el valor.
> - Si `personType` no es nulo, filtrar donde `PersonType` sea igual al valor.
> - Proyectar a `PersonDto` y retornar.
>
> **Principio SOLID aplicado**: SRP — El repositorio solo se encarga de acceso a datos, la lógica de negocio queda en el servicio.

#### Paso 1.2: Implementar `SearchAsync` en `PersonService`
> **Archivo**: `src/CodingBasics.Application/Services/Services.cs`
>
> **Qué hacer**: Reemplazar el `throw NotImplementedException` por la llamada al repositorio:
> ```csharp
> return await _repository.SearchAsync(name, personType, ct);
> ```
>
> **Principio SOLID aplicado**: DIP — El servicio depende de la abstracción `IPersonRepository`, no de la implementación concreta.

#### Paso 1.3: Habilitar los endpoints de búsqueda en la API
> **Archivo**: `src/CodingBasics.Api/Program.cs`
>
> **Qué hacer**: Descomentar y configurar los siguientes endpoints que cubren los métodos requeridos:
>
> | Endpoint | Método cubierto |
> |----------|----------------|
> | `GET /api/people` | `GetAll` (ya funcional) |
> | `GET /api/people/search?name={name}` | `GetPersonByName` |
> | `GET /api/people/search?personType={personType}` | `GetPersonByPersonType` |
> | `GET /api/people/search?name={name}&personType={personType}` | `GetPersonByNameAndPersonType` |
>
> Un solo endpoint `search` con query params opcionales cubre los 3 métodos de búsqueda, evitando duplicación y siguiendo el principio DRY.

---

### Fase 2 — Person: Operaciones de Escritura (Create, Update, Delete)

#### Paso 2.1: Agregar métodos CRUD a los contratos
> **Archivo**: `src/CodingBasics.Domain/Contracts/Contracts.cs`
>
> **Qué hacer**: Descomentar los métodos de CRUD en `IPersonService` e `IPersonRepository`:
> ```csharp
> Task<PersonDto?> GetByIdAsync(int id, CancellationToken ct = default);
> Task<PersonDto> CreateAsync(PersonDto dto, CancellationToken ct = default);
> Task<PersonDto> UpdateAsync(int id, PersonDto dto, CancellationToken ct = default);
> Task DeleteAsync(int id, CancellationToken ct = default);
> ```
>
> **Nota**: Se agrega también `GetByIdAsync` ya que es necesario para las operaciones Update y Delete (verificar existencia).
>
> **Principio SOLID aplicado**: ISP — Las interfaces definen contratos específicos por entidad, no una interfaz genérica gigante.

#### Paso 2.2: Implementar CRUD en `PersonRepository`
> **Archivo**: `src/CodingBasics.Infrastructure/Repositories/Repositories.cs`
>
> **Qué hacer**: Implementar los siguientes métodos:
>
> - **`GetByIdAsync`**: Buscar persona por `BusinessEntityID`, retornar `PersonDto` o `null`.
> - **`CreateAsync`**: Crear nueva entidad `Person` a partir del DTO. Asignar `rowguid = Guid.NewGuid()` y `ModifiedDate = DateTime.UtcNow`. Llamar `_context.People.Add()` y `SaveChangesAsync()`.
> - **`UpdateAsync`**: Buscar la entidad existente por ID, actualizar propiedades desde el DTO, llamar `SaveChangesAsync()`. Lanzar excepción si no existe.
> - **`DeleteAsync`**: Buscar la entidad por ID, llamar `_context.People.Remove()` y `SaveChangesAsync()`. Lanzar excepción si no existe.
>
> **Principio SOLID aplicado**: SRP — Cada método tiene una única responsabilidad de acceso a datos.

#### Paso 2.3: Implementar CRUD en `PersonService`
> **Archivo**: `src/CodingBasics.Application/Services/Services.cs`
>
> **Qué hacer**: Implementar los métodos delegando al repositorio. Agregar validaciones de negocio donde sea necesario:
> - Validar que campos requeridos (`FirstName`, `LastName`, `PersonType`) no estén vacíos en Create/Update.
> - Validar que `PersonType` sea un valor válido (EM, SP, SC, IN, VC, GC).
>
> **Principio SOLID aplicado**: SRP — Las validaciones de negocio viven en el servicio, no en el repositorio ni en el controller.

#### Paso 2.4: Crear endpoints CRUD de Person en la API
> **Archivo**: `src/CodingBasics.Api/Program.cs`
>
> **Qué hacer**: Agregar los endpoints:
>
> | Método HTTP | Ruta | Descripción |
> |-------------|------|-------------|
> | `GET` | `/api/people/{id}` | Obtener persona por ID |
> | `POST` | `/api/people` | Crear nueva persona |
> | `PUT` | `/api/people/{id}` | Actualizar persona existente |
> | `DELETE` | `/api/people/{id}` | Eliminar persona |
>
> Cada endpoint debe manejar respuestas HTTP apropiadas:
> - `200 OK` para lectura/actualización exitosa
> - `201 Created` para creación exitosa
> - `204 No Content` para eliminación exitosa
> - `404 Not Found` si la entidad no existe
> - `400 Bad Request` si los datos de entrada son inválidos

---

### Fase 3 — Products: Operaciones de Lectura

#### Paso 3.1: Implementar `GetAllAsync` en `ProductRepository`
> **Archivo**: `src/CodingBasics.Infrastructure/Repositories/Repositories.cs`
>
> **Qué hacer**: Descomentar la query con LEFT JOINs que ya está como guía en el código:
> - `Product` → `ProductSubcategory` (por `ProductSubcategoryID`)
> - `ProductSubcategory` → `ProductCategory` (por `ProductCategoryID`)
> - Proyectar a `ProductDto` incluyendo `CategoryName` y `SubcategoryName`.

#### Paso 3.2: Implementar `SearchAsync` en `ProductRepository`
> **Archivo**: `src/CodingBasics.Infrastructure/Repositories/Repositories.cs`
>
> **Qué hacer**: Implementar búsqueda con los mismos JOINs de `GetAllAsync`, agregando filtros opcionales:
> - Si `name` no es nulo, filtrar donde `Product.Name` contenga el valor.
> - Si `categoryName` no es nulo, filtrar donde `ProductCategory.Name` sea igual al valor.
>
> | Endpoint | Método cubierto |
> |----------|----------------|
> | `GET /api/products` | `GetAll` |
> | `GET /api/products/search?name={name}` | `GetProductByName` |
> | `GET /api/products/search?categoryName={categoryType}` | `GetProductsByCategoryType` |

#### Paso 3.3: Implementar `ProductService`
> **Archivo**: `src/CodingBasics.Application/Services/Services.cs`
>
> **Qué hacer**:
> - Descomentar la inyección de `IProductRepository` en el constructor.
> - Implementar `GetAllAsync` delegando al repositorio.
> - Implementar `SearchAsync` delegando al repositorio.
>
> **Principio SOLID aplicado**: DIP — El servicio depende de `IProductRepository` (abstracción), no de `ProductRepository` (implementación concreta).

#### Paso 3.4: Registrar Product en Dependency Injection
> **Archivos**:
> - `src/CodingBasics.Infrastructure/DependencyInjection.cs` — Descomentar `AddScoped<IProductRepository, ProductRepository>()`
> - `src/CodingBasics.Application/DependencyInjection.cs` — Descomentar `AddScoped<IProductService, ProductService>()`
>
> **Principio SOLID aplicado**: DIP — El registro de DI conecta abstracciones con implementaciones en la raíz de composición.

#### Paso 3.5: Habilitar endpoints de lectura de Products en la API
> **Archivo**: `src/CodingBasics.Api/Program.cs`
>
> **Qué hacer**: Descomentar y configurar:
> - `GET /api/products` — Listar todos los productos con paginación.
> - `GET /api/products/search` — Buscar por nombre y/o categoría.

---

### Fase 4 — Products: Operaciones de Escritura (Create, Update, Delete)

#### Paso 4.1: Agregar métodos CRUD a los contratos de Product
> **Archivo**: `src/CodingBasics.Domain/Contracts/Contracts.cs`
>
> **Qué hacer**: Descomentar los métodos de CRUD en `IProductService` e `IProductRepository`:
> ```csharp
> Task<ProductDto?> GetByIdAsync(int id, CancellationToken ct = default);
> Task<ProductDto> CreateAsync(ProductDto dto, CancellationToken ct = default);
> Task<ProductDto> UpdateAsync(int id, ProductDto dto, CancellationToken ct = default);
> Task DeleteAsync(int id, CancellationToken ct = default);
> ```

#### Paso 4.2: Implementar CRUD en `ProductRepository`
> **Archivo**: `src/CodingBasics.Infrastructure/Repositories/Repositories.cs`
>
> **Qué hacer**: Implementar los métodos:
>
> - **`GetByIdAsync`**: Buscar producto por `ProductID` con JOINs para incluir categoría. Retornar `ProductDto` o `null`.
> - **`CreateAsync`**: Crear nueva entidad `Product` a partir del DTO. Asignar campos requeridos por BD (`rowguid`, `ModifiedDate`, `SellStartDate`). Llamar `Add()` y `SaveChangesAsync()`.
> - **`UpdateAsync`**: Buscar entidad existente por ID, actualizar propiedades, `SaveChangesAsync()`.
> - **`DeleteAsync`**: Buscar entidad por ID, `Remove()` y `SaveChangesAsync()`.

#### Paso 4.3: Implementar CRUD en `ProductService`
> **Archivo**: `src/CodingBasics.Application/Services/Services.cs`
>
> **Qué hacer**: Implementar los métodos delegando al repositorio con validaciones:
> - Validar campos requeridos (`Name`, `ProductNumber`, `ListPrice >= 0`, etc.).
>
> **Principio SOLID aplicado**: OCP — Si en el futuro se necesitan más validaciones, se pueden agregar sin modificar el repositorio.

#### Paso 4.4: Crear endpoints CRUD de Products en la API
> **Archivo**: `src/CodingBasics.Api/Program.cs`
>
> **Qué hacer**: Agregar los endpoints:
>
> | Método HTTP | Ruta | Descripción |
> |-------------|------|-------------|
> | `GET` | `/api/products/{id}` | Obtener producto por ID |
> | `POST` | `/api/products` | Crear nuevo producto |
> | `PUT` | `/api/products/{id}` | Actualizar producto existente |
> | `DELETE` | `/api/products/{id}` | Eliminar producto |

---

### Fase 5 — Validación y Pruebas

#### Paso 5.1: Verificar compilación
> Ejecutar `dotnet build` desde la raíz de la solución y confirmar que no hay errores de compilación.

#### Paso 5.2: Probar endpoints con Swagger
> Levantar la API con `dotnet run` y abrir Swagger UI para validar todos los endpoints:
>
> **Person:**
> - `GET /api/people` — Listar todos
> - `GET /api/people/search?name=Ken` — Buscar por nombre
> - `GET /api/people/search?personType=EM` — Buscar por tipo
> - `GET /api/people/search?name=Ken&personType=EM` — Buscar por nombre y tipo
> - `GET /api/people/{id}` — Obtener por ID
> - `POST /api/people` — Crear
> - `PUT /api/people/{id}` — Actualizar
> - `DELETE /api/people/{id}` — Eliminar
>
> **Products:**
> - `GET /api/products` — Listar todos
> - `GET /api/products/search?name=Mountain` — Buscar por nombre
> - `GET /api/products/search?categoryName=Bikes` — Buscar por categoría
> - `GET /api/products/{id}` — Obtener por ID
> - `POST /api/products` — Crear
> - `PUT /api/products/{id}` — Actualizar
> - `DELETE /api/products/{id}` — Eliminar

---

## Resumen de Archivos a Modificar

| # | Archivo | Cambios |
|---|---------|---------|
| 1 | `Domain/Contracts/Contracts.cs` | Descomentar y agregar `GetByIdAsync`, `CreateAsync`, `UpdateAsync`, `DeleteAsync` en las 4 interfaces |
| 2 | `Infrastructure/Repositories/Repositories.cs` | Implementar todos los métodos en `PersonRepository` y `ProductRepository` |
| 3 | `Application/Services/Services.cs` | Implementar todos los métodos en `PersonService` y completar `ProductService` |
| 4 | `Infrastructure/DependencyInjection.cs` | Descomentar registro de `IProductRepository` |
| 5 | `Application/DependencyInjection.cs` | Descomentar registro de `IProductService` |
| 6 | `Api/Program.cs` | Descomentar y agregar todos los endpoints CRUD para ambas entidades |

## Principios SOLID Aplicados

| Principio | Aplicación |
|-----------|-----------|
| **S** — Single Responsibility | Cada capa tiene una responsabilidad clara: Repositorio = datos, Servicio = negocio, API = presentación |
| **O** — Open/Closed | Las interfaces permiten extender comportamiento sin modificar código existente |
| **L** — Liskov Substitution | Las implementaciones concretas son intercambiables a través de sus interfaces |
| **I** — Interface Segregation | Interfaces separadas por entidad (`IPersonRepository`, `IProductRepository`) en vez de un `IGenericRepository` |
| **D** — Dependency Inversion | Todas las dependencias fluyen hacia abstracciones (interfaces en Domain), nunca hacia implementaciones concretas |

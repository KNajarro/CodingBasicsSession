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



Segundo Prompt:

PARTE 1: BACKEND (.NET)
Actualmente tengo endpoints en Program.cs para obtener Products y People (y sus búsquedas), pero necesito endpoints para llenar los comboboxes del frontend.
Escribe el código necesario (indicando qué interfaces de la capa Application y qué repositorios de Infrastructure modificar, además de las rutas en Program.cs) para crear:

Un endpoint GET /api/products/categories que devuelva una lista de los nombres de categorías (categoryName) únicos y válidos desde la base de datos.
Un endpoint GET /api/people/types que devuelva una lista de los tipos de persona (personType) únicos y válidos desde la base de datos.


Tercer Prompt:

PARTE 2: FRONTEND (Vue.js)

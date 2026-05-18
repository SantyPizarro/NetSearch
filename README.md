# SearchEngine

API .NET 8 para crear documentos, indexarlos y buscarlos con tokenizacion, operadores `And`/`Or`, ranking TF-IDF, filtros por autor/categoria y paginado.

## Estado actual

Funcional:

- `POST /api/documents`: crea documento y actualiza el indice.
- `GET /api/documents`: lista documentos.
- `GET /api/documents/{id}`: obtiene un documento por id.
- `PUT /api/documents/{id}`: actualiza documento y reindexa sus terminos.
- `DELETE /api/documents/{id}`: elimina documento y entradas del indice.
- `POST /api/search`: busca por texto, operador, autor, categoria, pagina y tamano de pagina.
- Swagger UI en desarrollo: `/swagger`.
- Tests unitarios para tokenizacion, busqueda y reindexado.

Pendiente o dependiente del entorno:

- La API usa SQL Server con la cadena `DefaultConnection` de `SearchEngine.Api/appsettings.json`.
- Antes de probar endpoints reales hay que tener SQL Server disponible y aplicar migraciones.

## Ejecutar

```
dotnet restore
dotnet build SearchEngine.sln
dotnet ef database update --project SearchEngine.Infrastructure --startup-project SearchEngine.Api
dotnet run --project SearchEngine.Api
```

Luego abrir:

```text
http://localhost:5015/swagger
```

Si el puerto cambia, revisar `SearchEngine.Api/Properties/launchSettings.json` o la salida de `dotnet run`.

## Probar

Tests automatizados:

```
dotnet test SearchEngine.sln
```

Pruebas manuales:

- Usar Swagger UI en `/swagger`.
- Usar `SearchEngine.Api/SearchEngine.Api.http` desde Visual Studio, Rider o VS Code REST Client.
- Crear primero un documento, copiar el `id` devuelto y reemplazar `@DocumentId` en el archivo `.http` para probar `GET`, `PUT` y `DELETE`.

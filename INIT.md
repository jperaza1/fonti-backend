# INIT — Cómo funciona este backend

Guía de onboarding del API **Fonti / Buffeteleden**. Si solo necesitas levantar el entorno, usa [README.md](README.md). Este documento explica **cómo está armado el sistema y cómo se recorre un request de punta a punta**.

Solución: `cod.base.template.sln`  
Runtime: **ASP.NET Core 5.0** + **SQL Server** + **EF Core 5** + **Identity** + **JWT**  
IoC: **StructureMap** (convenciones) + registro extra en `Startup`

---

## 1. Qué hace el sistema

Es un backend de operaciones para rutas / cafeterías. El cliente web está en otro proyecto: **[FRONTEND.md](FRONTEND.md)** (`fonti-app` en WSL). Habla con dos hosts:

| Host | Proyecto | Puerto local | Rol |
|------|----------|--------------|-----|
| Auth | `cod.auth` | **5000** | Login, usuarios, tenants, roles, configuración |
| Data | `cod.data` | **5001** | Negocio: ventas, gastos, productos, control de salidas, facturas, RRHH, catálogos |

Ambos leen la misma base (`ConnectionStrings:Default`) y validan el mismo JWT (`JWT:Secret`, issuer, audience).

El dominio gira alrededor de **rutas** (`Route`). Sobre una ruta se registran:

- **Ventas** (`Sale` + `SaleDetail`) — contado o crédito; crédito exige cliente
- **Gastos** (`Expense`) — categoría, subcategoría, proveedor, opcionalmente factura
- **Control de salidas** (`DispatchControl` + `DispatchControlDetail` + `ProductMovement`) — productos de venta que salen / recargan / regresan. La materia prima no se lista

Catálogos de soporte: productos, precios por ruta, clientes, cafeterías, cajas (`Register`), correlativos, proveedores, empleados.

---

## 2. Capas (de afuera hacia adentro)

```
Cliente
   │  HTTP  /api/v2/{Controller}/{Action}
   ▼
cod.data  /  cod.auth          ← hosts web (controllers, JWT, CORS, Swagger)
   │  inyectan interfaces I*Command / I*Query
   ▼
cod.application                ← casos de uso, DTOs, validación
   │  IRepository + AuthService
   ▼
cod.domain                     ← entidades, enums, AuthService (claims/headers)
   ▲
cod.persistence                ← EF Core: Repository (DbContext), migraciones
cod.infraestructure            ← PDF, Excel, email, SFTP, HTTP, etc.
```

Regla práctica:

- **Controller** no toca EF ni reglas de negocio. Recibe DTO, llama `Execute`, envuelve errores.
- **Command / Query** sí habla con `IRepository` y arma el modelo de respuesta.
- **Entidad de dominio** vive en `cod.domain` y se mapea a tablas SQL por atributos `[Table]` + `DbSet` en `Repository`.
- **No hay MediatR.** Cada caso de uso es una clase concreta + su interfaz. StructureMap las resuelve por convención (`ICreateSaleCommand` → `CreateSaleCommand`).

### Proyectos

| Proyecto | Qué contiene |
|----------|----------------|
| `cod.domain` | Entidades (`Entity` / `EntityTenantable`), enums (`DomainEnums`), `AuthService`, extensiones HTTP |
| `cod.application` | Commands, Queries, Models/DTOs, `IRepository`, factories, excepciones |
| `cod.persistence` | `Repository` (IdentityDbContext), `EntityRegister/*`, migraciones |
| `cod.infraestructure` | Servicios transversales (PDF, Excel, email, Twilio, SFTP…) |
| `cod.data` | API de negocio |
| `cod.auth` | API de autenticación y administración de usuarios |

---

## 3. Cómo entra un request

### Ruteo

`BaseController` fija el contrato de la API:

```
[Route("api/v2/[controller]/[action]")]
[Authorize]
```

Ejemplos reales:

- `POST /api/v2/Sale/Create`
- `POST /api/v2/Sale/GetAll`
- `GET  /api/v2/Sale/GetById?id=12`
- `POST /api/v2/Auth/authenticate`  (en `cod.auth`, sin `[Authorize]`)

El nombre del método **es** el `action`. No hay `[HttpGet("ruta-custom")]` salvo excepciones.

### Pipeline (`Startup.Configure`)

1. CORS (`App:WebApplication`, orígenes separados por coma)
2. HTTPS redirection
3. Routing
4. Authentication (JWT Bearer)
5. Authorization
6. Endpoints de controllers
7. Swagger (`/swagger`, documento `v2`)

### Headers que el backend espera

`AuthService` lee el `HttpContext` en cada request:

| Header | Uso |
|--------|-----|
| `Authorization: Bearer {token}` | Usuario autenticado (`User.Identity.Name`) |
| `App.TenantId` | Tenant actual; filtra datos multi-tenant |
| `Timezone` | Zona horaria del cliente (default `America/Tegucigalpa`) |
| `App.CarrierId` / `App.CompanyId` | Contexto extra (legado del template) |

El JWT **no** lleva el tenant. El front lo manda en `App.TenantId`. El repositorio usa ese valor para filtrar e insertar.

---

## 4. Autenticación

Flujo de login (`cod.auth` → `AuthenticateCommand`):

1. `UserManager.FindByNameAsync` + `CheckPasswordAsync`
2. Carga `ApplicationUserProfile` (puede estar inactivo)
3. Valida el tenant: activo, y si es demo que no haya vencido
4. Arma claims (`Name`, roles) y firma un JWT (expira en 1 año)
5. Devuelve `Token`, `Expiration` y `UserAuth` (nombre, tenant, `UserType`, permisos y `routes`)

Identity está relajado a propósito: password mínimo 4 caracteres, sin mayúsculas/dígitos/símbolos obligatorios. Lockout a 5 intentos.

`cod.data` y `cod.auth` configuran el **mismo** esquema JWT. Un token emitido por auth sirve para llamar data.

Usuarios: `ApplicationUser` (Identity) + `ApplicationUserProfile` (datos de negocio, `UserType`). Roles/permisos viven en `cod.domain/Authorization`.

`UserType`: `SuperAdmin` = 1, `Administrativo` = 2, `General` = 3, `Vendor` = 4. El miembro va en inglés y el `[Description]` en español: en pantalla `Vendor` se lee **Vendedor**.

Si el tipo es Vendedor, el alta y la edición exigen al menos una ruta activa. Puede tener varias. Los ids se guardan en `ApplicationUserProfile.Routes`, separados por coma (`"3,8"`). Otro tipo deja ese campo vacío. El login parte el texto y lo manda en `userAuth.routes`. No hay selector de ruta en la pantalla de login. El front lo copia a `currentUser.routes` y lo reenvía en el header `App.Routes`. `Route/GetAllActive` hace lo mismo que `Cafeteria/GetAllActive` en Buffete: si el header trae ids, devuelve solo esas rutas activas; si viene vacío, devuelve todas las activas.

---

## 5. El patrón Command / Query

Cada feature en `cod.application` se organiza así:

```
cod.application/{Feature}/
  Common/                 ← DTOs (SaleModel, ExpenseFilterModel…)
  Commands/{Accion}/      ← ICreateXCommand + CreateXCommand
  Queries/{Accion}/       ← IGetXQuery + GetXQuery
```

Convenciones:

- Escrituras → `*Command` : heredan `BaseCommand` (`IRepository` + `IExceptionFactory`)
- Lecturas → `*Query` : heredan `BaseQuery` (`IRepository`)
- Método público siempre: `Execute(...)`
- El controller inyecta la **interfaz**, nunca la clase

Ejemplo de escritura (`CreateSaleCommand`):

1. Ajusta la fecha con `DateService.GetCLientLocalTime` + timezone del header
2. Valida el modelo; si falla, `IExceptionFactory` acumula errores y lanza `ApplicationException`
3. Mapea DTO → entidad de dominio
4. `_repository.InsertAsync` + `_repository.CommitAsync`

Ejemplo de lectura (`GetSalesQuery`):

1. Parte de `_repository.Query<Sale>()` (sin tracking, ya filtrado por tenant)
2. Encadena `Expression<Func<…>>` opcionales (fecha, tipo, ruta)
3. Proyecta a `SaleModel` y hace `ToListAsync`

### Errores

El controller **no** devuelve 4xx/5xx de negocio. Captura la excepción y responde `200 OK` con:

```json
{ "success": false, "messageResponse": "CANCELED", "title": "VENTAS", "errors": { "route": "…" } }
```

Eso lo arma `BaseController.ProcessException`. Éxito suele ser `{ "success": true, "messageResponse": "SUCCESS" }`.

Soft-delete: no se borra la fila. `DisableSync` pone `IsActive = false` + `DisableUser` / `DisableDateTime`. `EnableSync` revierte.

---

## 6. Persistencia y multi-tenant

`Repository` es a la vez:

- `IdentityDbContext<ApplicationUser>`
- implementación de `IRepository`
- clase **partial**: los `DbSet` están partidos por módulo en `cod.persistence/EntityRegister/`

| Archivo | Schema / módulo |
|---------|------------------|
| `DataRegister` | `data`: rutas, productos, unidades de medida, recetas, clientes, cafeterías, categorías, correlativos |
| `OperationRegister` | `operation`: ventas, gastos, facturas, control de salidas, stock, kardex, turnos de producción |
| `RrhhRegister` | empleados, vacaciones |
| `SalePointRegister` | platos / cafetería |
| `AuthorizationRegister` | tenants, perfiles, roles |
| `AlertRegister` / `GeneralRegister` | alertas, documentación, logs |

### Lo que hace `InsertAsync` / `WhereAsync` por ti

Al insertar una `IEntity`:

- `CreationTime = Clock.Now`
- `CreatorUser` = usuario del JWT
- `IsActive = true`
- si es `IEntityTenantable`, `TenantId` = header `App.TenantId`

Al consultar (`WhereAsync`, `AllAsync`, `Query`):

- aplica filtro de tenant salvo `DisableTenantFilter = true`
- `WhereActive` / `QueryActive` agregan `IsActive`

`OnConfiguring` activa **lazy loading proxies**. Evita navegar colecciones sin pensar: puede disparar N+1. Las queries de listado suelen proyectar con `Select`.

Migraciones: `cod.persistence/Migrations`. Aplicar:

```bash
dotnet ef database update --project cod.persistence --startup-project cod.data
```

Nueva migración:

```bash
dotnet ef migrations add nombre-descriptivo --project cod.persistence --startup-project cod.data
```

---

## 7. Entidad base

Casi todo hereda `Entity`:

| Campo | Significado |
|-------|-------------|
| `Id` | PK int |
| `IsActive` | vivo / deshabilitado |
| `CreatorUser` / `CreationTime` | alta |
| `LastModifiedUser` / `LastModificationTime` | última edición |
| `DisableUser` / `DisableDateTime` | baja lógica |
| `EnableUser` / `EnableDateTime` | reactivación |

`EntityTenantable` agrega `TenantId` → `ApplicationTenant`. Úsalo cuando el dato es por compañía. Varias entidades operativas actuales (`Sale`, `Expense`, `DispatchControl`) heredan solo `Entity` (sin tenant en la tabla).

Tablas usan schema SQL: `data.*`, `operation.*`, etc.

---

## 8. Mapa funcional

### Catálogos (`cod.domain/Data`, API en `cod.data`)

- **Route** — eje operativo; tiene `ProductRoutePrice` (precio del producto en esa ruta). `DeliversRegalia` indica si entrega regalía. Con el campo en verdadero, el precio de un producto puede ser 0; en falso, el precio tiene que ser mayor a 0. En pantalla es el switch **Entrega regalía** y el listado muestra la columna Regalía (Sí / No). Columna `data.Routes.DeliversRegalia` (`bit` no nulo, default falso); la migración la genera quien mantiene la base
- **Product** — nombre, `ProductType` (`Sale` = 1, `Purchase` = 2, `RawMaterial` = 3), `Tax`. Materia prima exige `UnitOfMeasureId` y al menos un `ProductVendor` activo (proveedor, unidad, unidades por medida, costo, costo unitario y descuento). `Product.Cost` guarda el costo unitario del primer proveedor activo y es el que usan inventario, producción y kardex. Venta y compra no guardan proveedores ni costo
- **UnitOfMeasure** — unidad de la materia prima (`data.UnitOfMeasures`)
- **ProductRecipe / ProductRecipeDetail** — receta del producto de venta. Cabecera: `UnitsPerPackage`. Línea: materia prima, `Quantity`, `ProducedUnits`, `PerPackage`
- **Customer** — clientes; crédito en venta
- **Cafeteria** — sucursal / punto; correlativos de OC, inventario, etc.
- **Register** — cajas por cafetería
- **Vendor** — proveedores (gastos / compras)
- **Category / SubCategory** — clasificación de gastos
- **Correlative** — numeración de documentos

### Operaciones (`cod.domain/Operation`)

- **Sale / SaleDetail** — venta de productos en una ruta. `SaleType`: `Cash` o `Credit`
- **Expense / ExpenseInvoice** — gasto de ruta; puede colgarse de una factura y un `DocumentId` (archivo)
- **DispatchControl / DispatchControlDetail / ProductMovement** — control de salida de mercadería. Status `Open` / `Close`. Salida y recarga descuentan el producto terminado; el retorno lo suma. No explota la receta. El formulario pide `Product/GetAllActive` con `ProductType.Sale` (1). `GetDispatchControl` devuelve solo movimientos de ese tipo, así que un control ya guardado tampoco muestra materia prima ni compra
- **ProductStock** — saldo único de la empresa por producto (`operation.ProductStock`)
- **InventoryKardex** — historial. `MovementType`: Entrada, Salida, Recarga, Retorno, Producción, Mala calidad, Pérdida. Puede apuntar a un despacho (`DispatchControlId`) o a una línea de producción (`ProductionOrderId`)
- **ProductionShift** — un registro por fecha y tipo de turno. Índice único `(ProductionDate, ShiftTypeId)`. El tipo sale de `data.Types` del grupo `PRODUCTION_SHIFT` (Turno A y Turno B). `IsClosed`: abierto, cerrado o anulado (`IsActive` en falso)
- **ProductionOrder** — línea de producto dentro del turno (`ProductionShiftId`, producto de venta, cantidad, nota)
- **Invoice / Payment / PurchaseOrder** — facturación y órdenes de compra. La orden es solo proveedor y materia prima: no lleva cafetería, ruta ni envío a Kova. Al pasar a revisión entra el inventario. Si `gen.Configurations` tiene `PURCHASE_ORDER_PRICE_APPROVAL` en `true`, un cambio de costo abre una solicitud y el costo del proveedor no cambia hasta aprobarla en `PriceApproval/Resolve`. Si la clave no existe o vale `false`, el costo se actualiza al recibir. La clave se crea en `false` la primera vez que se evalúa
- **Inventory / OfficeRequisition / SpecialService** — inventario, requisiciones, servicios especiales

### Inventario y producción

El saldo es de toda la empresa, no por ruta ni cafetería.

La cantidad que se escribe en producción son unidades del producto terminado. La receta descuenta materia prima solo al guardar una línea. El redondeo es a 6 decimales.

- Sin `PerPackage`: `unidades * Quantity * UnitsPerPackage / ProducedUnits`. Una bolsa con cantidad 1, que produce 1, y 10 unidades por paquete pide 10 bolsas por unidad. 250 unidades piden 2500 bolsas.
- Con `PerPackage`: `unidades * Quantity`. La cantidad ya es por cada unidad capturada y no se vuelve a multiplicar por las unidades del paquete. Cantidad 1 marcada así pide 250 bolsas para 250 unidades.
- La bobina se carga sin marcar, cantidad 1 que produce 230.

Además de la receta se puede descontar materia prima arruinada en esa misma línea (pérdida). En la pantalla solo aparecen las materias primas de la receta, con su unidad y la cantidad de pérdida. Si queda en cero, no se registra.

Solo puede haber un turno abierto en toda la planta (`IsActive` y `IsClosed` en falso). Mientras sigue abierto se le agregan líneas; no se abre otro. `Production/Create` acepta `productionShiftId` para colgar la línea de ese turno. Si no viene el id, busca la misma fecha y el mismo tipo; si ese tipo ya está abierto en otra fecha, la línea cae ahí. Cerrar (`Production/CloseShift`) deja el inventario como está y permite abrir el siguiente. Anular el turno revierte las líneas vigentes y lo desactiva; un turno cerrado no se anula. Un turno anulado no se vuelve a abrir en esa fecha y ese tipo. Anular una línea revierte solo ese producto. Si no hay stock, la operación no se guarda.

El listado de turnos muestra fecha, turno y estado (Abierto, Cerrado, Anulado). Los productos se ven al abrir el turno, no en la grilla.

Entradas y salidas manuales viven en Inventario. La salida de una materia prima queda en el kardex como **Mala calidad**. El kardex es el histórico, filtrado por fecha de inicio y fin; al abrir, ambas fechas son el día de hoy. Producción usa el mismo filtro.

El menú Inventario y Camiones (dentro de Operaciones) piden permiso. El código está en `page-menus.ts` del frontend y la fila vive solo en `AspNetRoles`.

| Pantalla | Ver | Acciones |
|----------|-----|----------|
| Inventario | `OPERATION.INVENTORY` | `ENTRY` entrada, `EXIT` salida |
| Producción | `OPERATION.PRODUCTION` | `CREATE` abrir turno y agregar líneas, `CLOSE` cerrar, `CANCEL` anular turno, `CANCEL_LINE` anular línea. Abrir un turno ya creado usa el permiso de ver |
| Kardex | `OPERATION.KARDEX` | Solo consulta |
| Recetas | `OPERATION.PRODUCT_RECIPE` | `CREATE`, `UPDATE` |
| Unidades de medida | `OPERATION.UNIT_OF_MEASURE` | `CREATE`, `UPDATE`, `DISABLE`, `ENABLE` |
| Camiones | `DATA.TRUCK` | `CREATE`, `UPDATE`, `DISABLE`, `ENABLE` |
| Ordenes de compra | `OPERATION.PURCHASE_ORDER` | `CREATE`, `ADD_PRODUCT`, `APPROVE`, `UPDATE_INVOICE_TYPE`, `PROCESS`, `REVIEW`, `CANCEL`, `CLOSE`, `REJECT`, `RECALCULATE`, `APPROVE_PRICE`, `REJECT_PRICE` |
| Reporte de órdenes de compra | `OPERATION.PURCHASE_ORDER.REPORT` | Solo consulta |
| Reporte de gastos de compra | `OPERATION.PURCHASE_ORDER.EXPENSE_REPORT` | Solo consulta |

**Ordenes de compra** es un ítem de primer nivel. Los dos reportes cuelgan de **Reportes**. La solicitud de precios no va en el menú: se abre en `/purchase-order/price-approval/:token` y sus botones usan `APPROVE_PRICE` y `REJECT_PRICE`. La referencia de la orden es `OC-#####`. Al pasar a revisión entra el inventario (`cantidad recibida × unidades de la presentación`). Con `PURCHASE_ORDER_PRICE_APPROVAL` en `true`, el costo del proveedor espera la aprobación; si la clave no existe o vale `false`, el costo se actualiza al recibir y la clave se crea en `false` la primera vez.

La tablet (`fonti-app` en Windows) tiene Inventario, Kardex y Producción; las recetas se editan en el frontend web.

Turnos de producción: `20261027000012_adding-production-kardex`, `20261027002424_adding-Change-production` y `20261027160000_adding-close-production-shift` (`operation.ProductionShifts.IsClosed`, `bit` no nulo, default falso; los turnos que ya estaban activos quedan abiertos). Rutas del vendedor: `20261027045938_adding-route-userprofile` agrega `auth.ApplicationUserProfiles.Routes` (`nvarchar(max)`, nullable).

### Otros

- **Rrhh**: empleados, vacaciones, PDF de solicitud
- **SalePoint**: platos y acompañamientos por cafetería
- **Alerts**: configuraciones y factories de datos para avisos
- **EnumDefinition**: endpoints que exponen enums al front (`/api/v2/EnumDefinition/...`)
- **Users / Tenant / Role**: solo en `cod.auth`. El formulario de usuario pide rutas cuando el tipo es Vendedor (`views/auth/user`)

### Infraestructura usada por application

`cod.infraestructure`: PDF (`DinkToPdf` / Syncfusion / iText), Excel (`EPPlus`), email, Twilio, SFTP, HTTP client. Los reports (ej. control de salidas) suelen devolver Excel en **base64**.

---

## 9. Inyección de dependencias

En `ConfigureServices`:

1. Se registran a mano queries genéricas de enums y el conversor PDF
2. Controllers, cache, Swagger, CORS, DbContext, Identity, JWT
3. StructureMap escanea ensamblados cuyo nombre contiene `"cod"` y aplica **default conventions** (`IFoo` → `Foo`)
4. `StructureMapRegistry` fija `IRepository → Repository` y `IHttpContextAccessor`

Por eso **no** registras cada command/query en `Startup`: basta con que la interfaz y la clase compartan el nombre (`IGetSaleQuery` / `GetSaleQuery`) y vivan en un ensamblado `cod.*`.

Excepción: `IGetEnumDefinitionValuesQuery<TEnum>` se registra a mano porque es genérico.

---

## 10. Cómo agregar un feature nuevo

Checklist del patrón existente (copia `Sale` o `Expense`):

1. **Dominio** — entidad en `cod.domain/{Modulo}/NuevaEntidad.cs` con `[Table("...", Schema = "...")]` heredando `Entity` o `EntityTenantable`.
2. **Persistencia** — `DbSet<NuevaEntidad>` en el `EntityRegister` que corresponda.
3. **Migración** — la hace quien mantiene la base. El cambio de modelo se queda en dominio, `DbSet` y `OnModelCreating`. No se generan archivos en `cod.persistence/Migrations` salvo que se pida. Comandos, cuando toque aplicarlos:

```bash
dotnet ef migrations add nombre-descriptivo --project cod.persistence --startup-project cod.data
dotnet ef database update --project cod.persistence --startup-project cod.data
```
4. **Application**
   - DTO en `Common/`
   - `ICreate…Command` + clase que hereda `BaseCommand`
   - `IGet…Query` + clase que hereda `BaseQuery`
   - Validar con `IExceptionFactory`; persistir con `InsertAsync` / `UpdateSync` / `DisableSync` + `CommitAsync`
5. **API** — controller en `cod.data/Controllers` (o `cod.auth` si es identidad) que herede `BaseController` e inyecte las interfaces.
6. **Enums al front** — si hay enum nuevo, query en `EnumDefinition` + registro en `Startup.ConfigureServices`.

No hace falta tocar StructureMap si respetas `INombre` / `Nombre`.

---

## 11. Flujo típico (venta)

```
POST /api/v2/Sale/Create
Authorization: Bearer …
App.TenantId: 1
Timezone: America/Tegucigalpa
Body: SaleModel { routeId, saleType, saleDate, amount, customerId?, saleDetails[] }

SaleController.Create
  → CreateSaleCommand.Execute
      → DateService (timezone)
      → ValidateModel (ruta, tipo, monto, fecha; cliente si Credit)
      → new Sale + SaleDetails
      → Repository.InsertAsync  (IsActive, CreatorUser, CreationTime)
      → CommitAsync
  → 200 { success: true, messageResponse: "SUCCESS" }
```

Listado: `POST /api/v2/Sale/GetAll` con `FilterSaleDto` → `GetSalesQuery` proyecta a `SaleModel`.

---

## 12. Configuración

Archivos: `cod.data/appsettings*.json` y `cod.auth/appsettings*.json`.

| Clave | Para qué |
|-------|----------|
| `ConnectionStrings:Default` | SQL Server |
| `JWT:Secret` / `ValidIssuer` / `ValidAudience` | Firma y validación del token |
| `App:WebApplication` | Orígenes CORS (coma-separados) |
| `App:Pdfs` | Carpeta de PDFs en el servidor |
| `PORT` (env) | Puerto de escucha (Railway). Default 5000 auth / 5001 data |

No commitees secretos reales. En local usa `appsettings.Development.json` o User Secrets.

---

## 13. Convenciones que evitan sorpresas

- Soft-delete, no `DELETE` físico (salvo casos puntuales).
- Fechas: el cliente manda UTC o local; el command las pasa por `DateService` + header `Timezone`.
- Respuestas de mutación: siempre `200` + `ResponseModel`, incluso en error de validación.
- Nombres en inglés en código; mensajes de validación a menudo en español (`"Ruta"`, `"Monto"`). En un enum nuevo, el miembro va en inglés y el `[Description]` en español (`Vendor` / «Vendedor»). El listado de usuarios muestra esa descripción.
- El template original era de transporte (`Carrier`, `Company`, muchos enums de trucks). Parte de eso sigue en `cod.domain/Features/Enums` y no se usa en Fonti.
- `cod.application` referencia solo `cod.domain`. `cod.persistence` referencia `cod.application` (el `IRepository` vive en application).
- Lazy loading está ON. En queries de lista, proyecta; no materialices entidades completas si no hace falta.

---

## 14. Arranque rápido

```bash
dotnet restore cod.base.template.sln
dotnet ef database update --project cod.persistence --startup-project cod.data
dotnet run --project cod.auth/cod.auth.csproj     # :5000  Swagger Authorize
dotnet run --project cod.data/cod.data.csproj     # :5001  Swagger Data
```

1. `POST /api/v2/Auth/authenticate` `{ "username", "password" }`
2. Usar el `token` + header `App.TenantId` contra `cod.data`

Detalle de puertos, IIS y OpenTelemetry: [README.md](README.md).

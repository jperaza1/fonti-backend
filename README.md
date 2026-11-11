# Backend — API (Fonti / Buffeteleden)

API REST en **ASP.NET Core 5.0** para operaciones de cafeterías, ventas, productos, inventario, producción, órdenes de compra, facturación, empleados y catálogos. Incluye un servicio separado de **autenticación** (`cod.auth`) y la API principal (`cod.data`).

Cómo está armado el sistema (capas, request, commands/queries, tenant, cómo agregar un feature): **[INIT.md](INIT.md)**.

El frontend Angular está en otro repo. Ruta y cómo se conecta: **[FRONTEND.md](FRONTEND.md)**.

## Requisitos

- [.NET 5 SDK](https://dotnet.microsoft.com/download/dotnet/5.0) (el proyecto usa `net5.0`; conviene planear migración a una versión LTS soportada).
- **SQL Server** accesible desde la cadena de conexión configurada.
- Windows recomendado si usas IIS Express o rutas locales de PDFs según `appsettings`.

## Estructura de la solución

| Proyecto | Rol |
|----------|-----|
| `cod.domain` | Entidades y enums de dominio |
| `cod.persistence` | EF Core, `DbContext`, migraciones |
| `cod.application` | Casos de uso (commands/queries), DTOs |
| `cod.infraestructure` | Servicios transversales (p. ej. PDF, imágenes) |
| `cod.data` | **Host web principal**: controladores, Swagger, JWT, OpenTelemetry |
| `cod.auth` | **Host de autenticación** (JWT; despliegue compatible con variable `PORT`, p. ej. Railway) |

Patrón: API → aplicación → dominio; persistencia e infraestructura inyectadas vía **StructureMap** y registro adicional en `Startup`.

## Cómo ejecutar en local

1. **Restaurar dependencias**

   ```bash
   dotnet restore cod.base.template.sln
   ```

2. **Configurar secretos y cadena de conexión**  
   Copia valores de ejemplo en `cod.data/appsettings.Development.json` y `cod.auth/appsettings.Development.json` (o usa [User Secrets](https://learn.microsoft.com/es-es/aspnet/core/security/app-secrets) / variables de entorno). No subas contraseñas reales al repositorio.

   Claves habituales:

   - `ConnectionStrings:Default` — base de datos SQL Server  
   - `JWT:Secret`, `JWT:ValidIssuer`, `JWT:ValidAudience` — tokens  
   - `App:WebApplication`, `App:Pdfs` — URLs y ruta de PDFs (ajustar a tu máquina)

3. **Aplicar migraciones** (si la base está vacía o desactualizada)

   ```bash
   dotnet ef database update --project cod.persistence --startup-project cod.data
   ```

4. **Levantar los servicios**

   Por defecto en código, si no existe la variable de entorno `PORT`:

   - **API principal** (`cod.data`): escucha en el puerto **5001** (`Program.cs`).
   - **Auth** (`cod.auth`): escucha en el puerto **5000** (`Program.cs`).

   ```bash
   dotnet run --project cod.data/cod.data.csproj
   dotnet run --project cod.auth/cod.auth.csproj
   ```

   En **Visual Studio**, los perfiles `sos.data` / `sos.auth` en `launchSettings.json` abren Swagger; revisa que los puertos no choquen con lo anterior.

5. **Swagger**

   Con el API principal en marcha: `http://localhost:5001/swagger` (o el puerto que uses en `launchSettings` / IIS Express).

## Compilación

```bash
dotnet build cod.base.template.sln
```

Si falla la copia de DLLs a `bin\Debug` con errores `MSB3027` / archivo en uso, detén procesos en depuración (`cod.data`, `cod.auth`) o compila en otra configuración: `dotnet build -c Release`.

## Observabilidad

`cod.data` registra **OpenTelemetry** (trazas y métricas ASP.NET Core, HTTP, EF Core). Ajusta exportadores y endpoints en `Startup` según tu entorno.

## Dominio funcional (resumen)

Los controladores en `cod.data/Controllers` cubren entre otros: cafeterías, clientes, empleados, productos, rutas y precios por ruta (una ruta con regalía admite precio 0; la ruta guarda el nombre del vendedor y el switch **Da crédito**), correlativos, ventas (copian ese vendedor al guardarse; al abrirlas, `GetById` resume los botellones y demás productos vendidos según el control de salida de la misma ruta y fecha; **Sacar botes** resta cantidades de varias ventas al contado sin factura y deja una venta nueva ya facturada), facturas, pagos, programación de pagos a proveedores, órdenes de compra (con facturas de la orden cuando está en proceso; el alta manda el comprobante y crea el gasto en la ruta Planta), proveedores, inventario y kardex, producción (un solo turno abierto; `CloseShift` lo cierra sin revertir stock), control de salidas (solo productos de venta; en el retorno los botes faltantes pueden ser 0; una ruta abierta puede pasar botellones a otra ruta abierta del mismo día), platos y acompañamientos, alertas (facturas pendientes y correlativos por vencer), documentación y catálogos vía `EnumDefinitionController`. Reportes de ventas: `POST Sale/GetChartReport` (vendedores, productos y contado/crédito) y `POST Sale/GetRouteQuantityReport` (cantidades por ruta y fecha, sin precio). El detalle de recetas, turnos, regalía, crédito de la ruta, vendedor, reportes, programación de pagos, la edición de ventas y permisos de Inventario y Camiones está en [INIT.md](INIT.md).

Usuarios y login viven en `cod.auth`. El tipo `Vendor` (Vendedor) guarda una o más rutas en el perfil y el login las devuelve en `userAuth.routes`.

## Licencias de terceros

- **EPPlus**: en `appsettings` aparece contexto de licencia; respeta la licencia elegida en tu despliegue.

---

*Nombre interno de solución: `cod.base.template.sln`.*

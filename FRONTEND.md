# Frontend de este backend

El cliente web de Fonti **no vive en este repo**. Está en otro proyecto (Angular).

## Dónde está

| Desde | Ruta |
|-------|------|
| **WSL Ubuntu-22.04** | `/root/proyectos/robert/fonti/fonti-app` |
| **Windows (UNC)** | `\\wsl.localhost\Ubuntu-22.04\root\proyectos\robert\fonti\fonti-app` |

Repo / carpeta: `fonti-app`.  
Guía inversa (este backend visto desde el front): `BACKEND.md` dentro de ese proyecto.

## Cómo se habla con este API

El front lee `src/assets/config.local.json` (`environment.ts` → `configUrl`).

| Clave | Host local | Proyecto |
|-------|------------|----------|
| `auth` | `https://localhost:44390/api/v2/` (IIS) o `http://localhost:5000/api/v2/` | `cod.auth` |
| `data` | `https://localhost:44367/api/v2/` (IIS) o `http://localhost:5001/api/v2/` | `cod.data` |

La app Angular corre en **http://localhost:4200** (`App:WebApplication` en `cod.data` / `cod.auth`).

Contrato: `api/v2/{Controller}/{Action}`. Ejemplo: `Distributor/GetAll` ↔ `src/app/views/data/distributor/`.

## Módulos que se espejan

| Backend (`cod.data` / `cod.application`) | Frontend (`src/app/views`) |
|------------------------------------------|----------------------------|
| `Route` | `data/route` |
| `Distributor` | `data/distributor` |
| `Product`, `Vendor`, `Customer`, `Truck` | `data/product`, `vendor`, `customer`, `truck` |
| `UnitOfMeasure`, `ProductRecipe` | `operation/unit-of-measure`, `operation/product-recipe` |
| `Kardex`, `Production` | `operation/inventory`, `operation/kardex`, `operation/production` |
| `Sale`, `Expense`, `Invoice`, `Payment`, `PaymentSchedule` | `operation/sale`, `expense`, `invoice`, `payment`, `payment-schedule` |
| Users (`cod.auth`) | `auth/user` |

Menú **Inventario** y **Camiones** (`page-menus.ts`) piden permiso. El mismo código está en `AspNetRoles`. Inventario muestra saldos y las acciones de entrada y salida. Kardex es el histórico del día y solo consulta. Recetas permite crear y editar. Unidades de medida y Camiones permiten crear, editar, desactivar y activar.

| Pantalla | Ver | Acciones |
|----------|-----|----------|
| Inventario | `OPERATION.INVENTORY` | `ENTRY`, `EXIT` |
| Producción | `OPERATION.PRODUCTION` | `CREATE`, `CLOSE`, `CANCEL`, `CANCEL_LINE` |
| Kardex | `OPERATION.KARDEX` | — |
| Recetas | `OPERATION.PRODUCT_RECIPE` | `CREATE`, `UPDATE` |
| Unidades de medida | `OPERATION.UNIT_OF_MEASURE` | `CREATE`, `UPDATE`, `DISABLE`, `ENABLE` |
| Camiones | `DATA.TRUCK` | `CREATE`, `UPDATE`, `DISABLE`, `ENABLE` |
| Ordenes de compra | `OPERATION.PURCHASE_ORDER` | `CREATE`, `ADD_PRODUCT`, `APPROVE`, `UPDATE_INVOICE_TYPE`, `PROCESS`, `REVIEW`, `CANCEL`, `CLOSE`, `REJECT`, `RECALCULATE`, `APPROVE_PRICE`, `REJECT_PRICE` |
| Ventas | `OPERATION.SALE` | `CREATE`, `VIEW`, `UPDATE`, `DISABLE`, `ENABLE`, `CONSOLIDATE` |
| Programación de pagos | `OPERATION.PAYMENT_SCHEDULE` | `CREATE`, `VIEW`, `PARTIAL`, `FINISH` |
| Reporte de órdenes de compra | `OPERATION.PURCHASE_ORDER.REPORT` | — |
| Reporte de gastos de compra | `OPERATION.PURCHASE_ORDER.EXPENSE_REPORT` | — |
| Ranking de vendedores | `OPERATION.SALE.VENDOR_REPORT` | — |
| Productos vendidos | `OPERATION.SALE.PRODUCT_REPORT` | — |
| Contado y crédito | `OPERATION.SALE.PAYMENT_TYPE_REPORT` | — |
| Cantidades por ruta | `OPERATION.SALE.ROUTE_QUANTITY_REPORT` | — |

El turno usa el grupo de tipos `PRODUCTION_SHIFT`. Solo puede haber un turno abierto: se cierra con **Cerrar turno** (el inventario se queda) y después se abre otro. Mientras está abierto, las líneas nuevas se agregan a ese turno (`productionShiftId` en `Production/Create`). Anular revierte el inventario. El listado muestra fecha, turno y estado; la columna de productos no va en la grilla. Abrir un turno ya creado usa `OPERATION.PRODUCTION`.

En Ventas, el lápiz usa `OPERATION.SALE.VIEW` y abre la venta. Si ya existe, ruta, tipo, cliente, fecha y cantidades se habilitan solo con `OPERATION.SALE.UPDATE`. Una venta con factura se puede ver, no editar. Al abrirla se muestran las líneas guardadas; un precio 0 se lee **Regalía**. Debajo del detalle, **Según control de salida** lista los botellones y los demás productos vendidos de ese control (salida + recargas + traspasos recibidos − retorno − traspasos entregados). Si no hay control de esa ruta en la fecha, se avisa. El vendedor es un campo bloqueado: al elegir la ruta se copia `vendor` y se guarda en la venta. El listado tiene la columna Vendedor. **Facturar al Contado** (`OPERATION.SALE.CONSOLIDATE`) abre la ruta y el rango del filtro, marca las ventas al contado sin factura y pide cuántos botes sacar de cada precio mayor a 0, más el cliente y la fecha. Guarda con `Sale/Consolidate` y refresca el listado. El permiso está en `AspNetRoles` y no queda asignado a un rol.

En Rutas, el formulario y el listado tienen **Vendedor** (`vendor`), un texto libre.

En Reportes, además de facturas pendientes y los reportes de compra, están Ranking de vendedores (`sale/vendor-report`), Productos vendidos (`sale/product-report`), Contado y crédito (`sale/payment-type-report`) y Cantidades por ruta (`sale/route-quantity-report`). En productos, un clic abre la cantidad por precio. El servicio llama a `Sale/GetChartReport`. Cantidades por ruta llama a `Sale/GetRouteQuantityReport`: una fila por ruta y vendedor, con la cantidad de cada producto en el rango, sin partir por fecha, y una fila Total. Arriba se listan lunes a domingo de cada semana que entra en el rango. El filtro abre en la semana actual. El botón de la grilla exporta eso a Excel.

En el usuario, las alertas del inicio se marcan en la grilla. **Correlativos por vencer** avisa 30 días antes de la fecha límite o cuando quedan 100 números o menos. El tipo `Vendor` se muestra como Vendedor. Ese tipo abre un selector múltiple de rutas activas y exige al menos una. Al iniciar sesión, `userAuth.routes` queda en `currentUser.routes` y el interceptor lo manda en `App.Routes`. `Route/GetAllActive` devuelve solo esas rutas si el header trae ids; si viene vacío, devuelve todas las activas. Ventas, gastos, facturas de compra y camiones piden esa lista. El catálogo de rutas sigue en `Route/GetAll`.

En Rutas (`data/route`), el formulario tiene el switch **Entrega regalía** (`deliversRegalia`). Encendido, un producto del talonario puede quedar en precio 0. Apagado, el precio tiene que ser mayor a 0. El listado muestra la columna Regalía (Sí / No). También tiene el switch **Da crédito** (`givesCredit`): se envía al crear y al editar, y `Route/GetById` lo regresa para dejar el switch como quedó guardado. El listado muestra la columna Crédito (Sí / No). Con el switch encendido, cada producto del talonario tiene **Precio crédito** (`creditPrice`). Es el precio del botellón fiado y no cambia el precio de contado. Al agregar se puede dejar vacío. En una fila ya guardada, vacío no borra el precio de crédito que ya tenía. `Route/GetById` y `Route/GetProductRoutePrices` lo devuelven. La columna es `data.ProductRoutePrices.CreditPrice` (`float`, nullable). La migración `20261112223919_adding-Route-Product-Credit-Price` la crea.

En Productos, el bloque de proveedores y costo solo se muestra cuando el tipo es Materia prima. Cada línea pide proveedor, unidad, unidades por medida, costo, costo unitario y descuento. El listado muestra el costo unitario que usa el inventario.

La orden de compra no pide cafetería ni ruta. El menú **Ordenes de Compra** abre el listado; los reportes están bajo **Reportes**. Los productos salen de `Product/GetByVendor` y cada línea guarda el `productVendorId` de esa materia prima. La solicitud de cambio de precios vive en `/purchase-order/price-approval/:token` y solo se genera si la configuración `PURCHASE_ORDER_PRICE_APPROVAL` está en `true`. Aprobar y rechazar esa solicitud usan `OPERATION.PURCHASE_ORDER.APPROVE_PRICE` y `OPERATION.PURCHASE_ORDER.REJECT_PRICE`. Con la orden guardada y en estado Procesando, Pendiente de revisión o Completado, la pestaña **Facturas** lista las de esa orden. **Agregar Factura** (`OPERATION.INVOICE.CREATE`) solo aparece en Procesando. El alta pide el comprobante y lo envía en el mismo `Invoice/AddInvoice`. El gasto de esa factura queda en la ruta Planta, con categoría Producción y subcategoría Compra de suministros. El formulario no pide la ruta. Al reabrir con el lápiz se ve la imagen. Borrar usa `OPERATION.INVOICE.DELETE`.

**Programación de pagos** está en Finanzas (`operation/payment-schedule`). El listado es `/payment-schedule`, el alta `/payment-schedule/create` y el detalle `/payment-schedule/view/:id`. El alta trae los saldos sin pagar y deja agregar proveedores. En la programación más reciente se agregan líneas y se guarda el real; **Pagado** lo deja en el monto a pagar. Cada línea tiene descripción. El detalle también tiene la pestaña Documentos (`PAYMENT_SCHEDULE`). Quien tenga `CREATE`, `VIEW`, `PARTIAL` o `FINISH` también necesita `OPERATION.PAYMENT_SCHEDULE` para ver el menú.

El control de salida (`operation/dispatch-control`) arma salida, recarga y retorno con `Product/GetAllActive?productType=1`. Al guardar uno nuevo, si esa ruta ya tiene un control activo en esa fecha, se avisa y no se crea. Uno desactivado no bloquea. `DispatchControl/GetAll` devuelve un arreglo. El listado web filtra por ruta: la primera opción es **Todas** (0) y Planta no aparece. La tablet no manda `routeId`. En cada línea, los productos de ese control muestran despachado y retornado. Vendido sale solo cuando esa línea está cerrada, con el retorno ingresado. No hay un total del rango. Solo productos de venta. Al abrir un control ya guardado, los movimientos que vuelven son de ese mismo tipo. Al elegir la ruta, el camión y el vendedor se llenan solos y se muestran en el mismo recuadro de solo lectura. En el retorno se capturan los botes faltantes y las cestas; ambos pueden quedar en 0. Con el control abierto, permiso de editar y la configuración `DISPATCH_TRANSFER` en `true`, **Pasar botellones** elige la ruta que recibe, el producto y la cantidad. La otra ruta tiene que tener el control abierto el mismo día. En quien entrega se lee **Entregado a {ruta}** y en quien recibe **Recibido de {ruta}**. Anular lo quita de las dos si siguen abiertas. Si la clave no existe, se crea en `false` al abrir el control. El Excel lleva la columna Traspasos solo con la clave encendida. La app de la tablet llama a `DispatchControl/GetTransferSetting` y muestra el mismo botón solo con la clave en `true`.

Pantallas y menú: `src/app/config/page-menus.ts`.  
Línea gráfica: `docs/design-line.md` en el front.

## Levantar el front (desde WSL)

```bash
cd /root/proyectos/robert/fonti/fonti-app
npm install
ng serve
```

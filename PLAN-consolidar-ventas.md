# Facturar al contado con botes de varias ventas

Implementado en `Sale/GetConsolidateAvailability`, `Sale/Consolidate` y el botón **Facturar al Contado** del listado. El permiso `OPERATION.SALE.CONSOLIDATE` está en `AspNetRoles` y no queda asignado a un rol.

De una ruta se eligen ventas al contado, activas y sin factura. Se indica cuántos botes se quieren de cada producto con precio mayor a 0 (por ejemplo 120 a 27 y 90 a 25). Esa cantidad se reparte entre las ventas marcadas, se les resta, se recalcula el total y se crea una venta nueva con esos botes. En el mismo paso se pide cliente y fecha, se genera la factura de contado y se le asigna esa venta.

La regalía (precio 0) no se toca. El control de salida no se mueve.

## Cómo se usa

En Ventas, el botón **Facturar al Contado**:

1. Toma la ruta y el rango que ya están en el filtro. Se pueden cambiar y volver a buscar.
2. Lista solo ventas activas, al contado y sin factura. Vienen marcadas; se puede quitar alguna.
3. Muestra cada producto y precio mayor a 0, con la cantidad disponible. Ahí se escribe cuántos botes sacar.
4. Se elige el cliente y la fecha de la venta nueva y de la factura.
5. Al guardar se descuentan los botes, se crea la venta y queda facturada.

## Reglas

- Al menos una venta de esa ruta, activa, al contado y sin factura.
- Solo productos con precio mayor a 0. Si el mismo producto y precio se pide dos veces, se suma.
- Los botes se reparten entre todas las ventas marcadas que tengan ese producto y ese precio. Si la cantidad pedida alcanza para todas, cada una aporta al menos uno y el resto se reparte según lo que les queda. Si no alcanza para todas, se toma uno de las que más tienen, hasta completar.
- Si no alcanzan los botes, no se guarda nada y se avisa cuántos hay y cuántos se pidieron.
- El total de cada línea queda en precio por cantidad. El monto de la venta es la suma de las líneas que siguen con cantidad.
- Si una venta se queda sin líneas con cantidad, se desactiva. Si solo le queda regalía, sigue activa.
- La venta nueva es al contado, con el vendedor de la ruta, el cliente y la fecha elegidos.
- La factura es de contado, pagada, a nombre de ese cliente, con la misma fecha, impuesto del 15% sobre lo gravado, y el correlativo vigente. La venta nueva queda ligada a esa factura.
- Si la factura no se puede crear, se revierten también los descuentos.

## Backend

- `POST Sale/GetConsolidateAvailability` con ruta e ids. Devuelve producto, precio y cantidad disponible.
- `POST Sale/Consolidate` con ruta, cliente, fecha, ids y las líneas (producto, precio, cantidad).
- No agrega columnas. La migración de esta función no hace falta.

Al facturar al contado por el flujo que ya existía, si todas las ventas del lote tienen el mismo cliente se usa ese cliente. Si no, sigue **Consumidor final**.

## Frontend

En el listado de ventas, **Facturar al Contado** abre el modal y llama a esos dos endpoints.

Permiso de la acción: `OPERATION.SALE.CONSOLIDATE`, en el menú de Ventas, en el botón, y en `AspNetRoles`. No se asigna a un rol.

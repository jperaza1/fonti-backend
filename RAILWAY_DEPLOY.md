# Guía de despliegue en Railway

## Resumen

Este proyecto tiene **dos APIs** que se despliegan por separado:
- **cod.auth**: API de autenticación (JWT, Identity)
- **cod.data**: API principal de datos

Ambas usan **SQL Server** como base de datos.

---

## 1. SQL Server en Railway

Railway ofrece SQL Server como servicio. Opciones:

- **SQL Server 2025**: https://railway.com/deploy/microsoft-sql-server-2025
- **SQL Server 2017**: https://railway.com/template/aNgXG5

### Variables requeridas para SQL Server
| Variable | Valor |
|----------|-------|
| `ACCEPT_EULA` | `Y` |
| `MSSQL_SA_PASSWORD` | Tu contraseña segura |

### Cadena de conexión

Railway usa **coma** (`,`) para separar host y puerto, no dos puntos:

```
Server=<HOST>,<PUERTO>;Database=tu_base_datos;User Id=sa;Password=<MSSQL_SA_PASSWORD>;TrustServerCertificate=True;
```

Tras desplegar SQL Server, Railway te mostrará las variables de conexión. Puedes crear una variable compartida en el proyecto para usarla en ambos servicios.

---

## 2. Desplegar cod.auth y cod.data

### Opción A: Desde el dashboard de Railway

1. **Crear un nuevo proyecto** en Railway
2. **Añadir SQL Server** como servicio (desde templates)
3. **Añadir servicio vacío** para cod.auth:
   - Conectar el repositorio
   - Root Directory: `/` (o la ruta al backend)
   - Builder: Dockerfile
   - Dockerfile Path: `cod.auth/Dockerfile`
   - Start Command: `dotnet cod.auth.dll`
4. **Repetir** para cod.data con `cod.data/Dockerfile`
5. **Generar dominios** para cada API

### Opción B: Con Railway CLI

```bash
# Instalar Railway CLI
npm i -g @railway/cli

# Login
railway login

# En la carpeta backend, para cod.auth:
railway link   # Vincular al proyecto
railway up --service cod-auth   # O crear el servicio primero desde el dashboard
```

---

## 3. Variables de entorno (cod.auth y cod.data)

Configura estas variables en **cada servicio** (Variables tab en Railway):

| Variable | Descripción | Ejemplo |
|----------|-------------|---------|
| `ConnectionStrings__Default` | Cadena de conexión SQL Server | `Server=...,1433;Database=...;User Id=sa;Password=...;TrustServerCertificate=True` |
| `JWT__ValidAudience` | Audiencia del token | `https://tu-frontend.railway.app` |
| `JWT__ValidIssuer` | Emisor del token | `https://cod-auth.railway.app` |
| `JWT__Secret` | Clave secreta JWT | (misma en ambos para que los tokens sean válidos) |
| `App__WebApplication` | Orígenes CORS (separados por coma) | `https://tu-app.railway.app,https://localhost:3000` |

> **Nota**: En .NET, `__` (doble guión bajo) reemplaza `:` en la jerarquía de configuración.

### Referenciar SQL Server desde otro servicio

Si SQL Server está en el mismo proyecto Railway, puedes usar:

```
ConnectionStrings__Default=${{SqlServer.MSSQL_CONNECTION_STRING}}
```

(Ajusta el nombre del servicio y la variable según lo que exponga Railway)

---

## 4. Migraciones de base de datos

Las migraciones de Entity Framework deben ejecutarse contra la base de datos. Opciones:

### A) Ejecutar migraciones localmente apuntando a Railway

```bash
# Obtener la cadena de conexión de Railway y usarla localmente
dotnet ef database update --project cod.persistence --startup-project cod.data -- --connection "Server=..."
```

### B) Añadir un job/script de despliegue

Puedes crear un servicio temporal o un script que ejecute `dotnet ef database update` al desplegar.

### C) Migraciones automáticas al arrancar (solo para desarrollo)

En `Program.cs` o `Startup.cs` puedes aplicar migraciones pendientes al iniciar (no recomendado en producción sin control).

---

## 5. Consideraciones adicionales

### libwkhtmltox.dll (cod.data)
El proyecto referencia `libwkhtmltox.dll` para generación de PDFs. Si lo usas en producción:
- La imagen base es Linux; wkhtmltopdf requiere dependencias específicas
- Considera alternativas como Puppeteer/Playwright o servicios externos de PDF

### HTTPS
Railway termina HTTPS en su proxy. La app escucha HTTP en el puerto `PORT`; no hace falta configurar certificados.

### appsettings.json
Las credenciales en `appsettings.json` se sobrescriben con variables de entorno en Railway. Para desarrollo local, mantén `appsettings.Development.json` sin subirlo a Git.

---

## 6. Estructura de archivos creados

```
backend/
├── cod.auth/
│   ├── Dockerfile
│   └── Program.cs  (modificado: UseUrls con PORT)
├── cod.data/
│   ├── Dockerfile
│   └── Program.cs  (modificado: UseUrls con PORT)
└── RAILWAY_DEPLOY.md  (esta guía)
```

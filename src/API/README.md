# 🌐 API

Proyecto de backend desarrollado con **ASP.NET Core 8**, que expone una API RESTful y sirve como backend para el frontend del proyecto. Utiliza **PostgreSQL** como base de datos, junto con **Entity Framework Core 7** como ORM. Actualmente, la autorización es manual, con vistas a migrar a **JWT Bearer Authentication** próximamente.

---

## 🗂️ Estructura del proyecto

```
API/
├── Controllers/          # Controladores que expone los endpoints HTTP
├── Models/               # Clases de modelo de dominio
├── Requests/             # DTOs para peticiones (Request Models)
├── Resources/            # Recursos compartidos (por ejemplo, textos o configuraciones)
├── Services/             # Lógica de negocio y servicios internos
├── Utils/                # Utilidades y helpers comunes
├── ApplicationDbContext.cs  # DbContext de Entity Framework Core
├── appsettings.json      # Configuración de entorno
├── Program.cs            # Punto de entrada de la aplicación
└── .github/workflows/    # Pipeline de integración continua (CI)
    └── run-tests.yml
```

---

## 🔐 Autenticación y Seguridad

- Actualmente los endpoints controlan el acceso de forma **manual**.
- Está planificada la incorporación de **autenticación JWT Bearer** para gestionar usuarios y roles de forma más segura y escalable.

---

## 🗃️ Base de datos

- **Sistema gestor**: PostgreSQL
- **ORM**: Entity Framework Core 7
- `ApplicationDbContext` define las entidades y relaciones.
- Usa migraciones para gestionar cambios en el esquema de datos.

---

## 🚀 Integración continua (GitHub Actions)

Este proyecto incluye una **pipeline CI/CD** definida en `.github/workflows/run-tests.yml`, que:

1. Se ejecuta en cada `push` o `pull_request` hacia la rama `develop`.
2. Hace checkout del repositorio actual (API) y del repositorio de tests (`DaveNetDeveloper/Test`).
3. Instala .NET 8 y la CLI de Playwright.
4. Ejecuta los tests del proyecto de test externo en modo Release.

---

## ▶️ Ejecución local

### Requisitos
- .NET 8 SDK
- PostgreSQL en ejecución
- Visual Studio 2022+ o VS Code

### Comandos básicos
```bash
dotnet restore
dotnet build
dotnet ef database update  # Aplica migraciones
dotnet run
```

---

## 📦 Paquetes NuGet utilizados (principales)
- `Microsoft.EntityFrameworkCore`
- `Microsoft.EntityFrameworkCore.Design`
- `Npgsql.EntityFrameworkCore.PostgreSQL`
- `Microsoft.AspNetCore.Authentication.JwtBearer` (planificado)

---

## 📌 Notas finales

- El proyecto de **Tests** depende de esta API.
- Las llamadas desde el **frontend** se hacen directamente mediante JavaScript (fetch/AJAX).
- La autenticación y gestión de usuarios será mejorada en próximas iteraciones.

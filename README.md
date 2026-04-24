# Sistema Gestor Inventario — Backend

API REST desarrollada con **ASP.NET Core Web API** para la gestión integral de una PYME del sector LEDs. Incluye módulos de inventario, ventas, adquisiciones y administración de usuarios con control de acceso basado en roles.

**Deploy:** Azure app services  
**Frontend repo:** [https://github.com/david011902/sistema-inventario]

---

## Tecnologías

| Capa          | Tecnología                   |
| ------------- | ---------------------------- |
| Framework     | ASP.NET Core Web API         |
| ORM           | Entity Framework Core + LINQ |
| Base de datos | PostgreSQL (Supabase)        |
| Autenticación | JWT (JSON Web Tokens)        |
| Mapeo         | AutoMapper                   |
| Deploy        | Azure App Service (Plan F1)  |

---

El proyecto sigue una **arquitectura en capas** inspirada en Clean Architecture:

```
SistemaGestorInventario/
├── Domain/                  # Entidades, abstracciones e interfaces
│   ├── Abstractions/
│   ├── Entities/
│   └── Enums/
├── Application/             # Casos de uso, servicios y DTOs
│   ├── DTOs/
│   ├── Services/
│   └── UseCases/
├── Data/                    # Acceso a datos, repositorios y migraciones
│   ├── Migrations/
│   ├── Persistence/
│   ├── Repositories/
│   └── Services/
└── SistemaGestorInventario/ # Punto de entrada — Endpoints y configuración
    ├── Endpoints/
    └── Program.cs
```

---

## Estilo de API

Los endpoints están implementados con **Minimal APIs** de .NET 10, organizados
por módulo en la carpeta `Endpoints/`. Este enfoque reduce el boilerplate
frente a los controladores tradicionales manteniendo la separación de responsabilidades.

### Rate Limiting

La API incluye límite de peticiones por IP usando el middleware nativo de
ASP.NET Core, para proteger los endpoints públicos de ataques de fuerza bruta.

## Documentación de la API

La API expone una interfaz interactiva con **Scalar** disponible en `/scalar` al correr el proyecto localmente.

## Base de Datos

Se utilizó la filosofía **Code First** con Entity Framework Core: el esquema
de la base de datos se define desde las entidades en C# y se gestiona
mediante migraciones.

## Funcionalidades Principales

- **Control de Inventario:** Seguimiento en tiempo real de productos LED, categorías y stock, asi como busqueda por SKU.
- **Ventas y Compras:** Registro histórico de movimientos que impactan el inventario.
- **Seguridad Robusta:** Implementación de JWT para proteger los endpoints según el rol del usuario.
- **Persistencia Confiable:** Uso de PostgreSQL con migraciones gestionadas por EF Core.

## Variables de entorno

---

## Compilar el proyecto

Crea un archivo `appsettings.json` en la raíz del proyecto con la siguiente estructura:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=...;Database=...;Username=...;Password=..."
  },
  "Jwt": {
    "AccessSecret": "tu_clave_secreta",
    "RefreshSecret": "tu_clave_secreta"
  }
}
```

---

## Instalación local

```bash
# 1. Clonar el repositorio
git clone https://github.com/david011902/SistemaGestorInventario
cd sistema-gestor-backend

# 2. Configurar variables de entorno
# Crea el archivo appsettings.Development.json como se indica arriba

# 3. Aplicar migraciones
dotnet ef database update

# 4. Ejecutar el proyecto
dotnet run
```

---

## Autenticación

La API usa **JWT Bearer Tokens**. Para acceder a los endpoints protegidos:

1. Realiza un `POST /api/auth/login` con tus credenciales
2. Copia el token de la respuesta
3. Inclúyelo en el header de las siguientes peticiones:
   ```
   Authorization: Bearer <token>
   ```

### Roles disponibles

| Rol           | Acceso                            |
| ------------- | --------------------------------- |
| Administrator | Acceso completo                   |
| Employee      | Ventas, Inventario, Adquisiciones |

---

## Autor

**[David Acosta]**  
Ingeniero en Sistemas Computacionales  
[GitHub](https://github.com/david011902)

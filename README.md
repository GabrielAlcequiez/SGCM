# SGCM - Sistema de Gestión de Clínica/Médica

SGCM es un sistema integral para la gestión de clínicas y consultorios médicos, desarrollado con arquitectura limpia (Clean Architecture) y las mejores prácticas de desarrollo de software.

## Características Principales

- **Gestión de Pacientes**: Registro, actualización, eliminación (lógica) y consulta de pacientes
- **Gestión de Citas**: Programación, reagendación y seguimiento de citas médicas
- **Gestión de Médicos**: Administración de profesionales de la salud y sus especialidades
- **Especialidades Médicas**: Catálogo de especialidades disponibles
- **Disponibilidad**: Control de horarios disponibles por médico
- **Proveedores**: Gestión de proveedores de seguros médicos
- **Autenticación y Autorización**: Sistema seguro con JWT
- **Auditoría**: Registro completo de todas las operaciones del sistema
- **Notificaciones**: Envío de notificaciones por Email, SMS y Push

## Arquitectura

El proyecto implementa **Arquitectura Limpia (Clean Architecture)** con una separación clara de responsabilidades:

```
┌─────────────────────────────────────────────────────────────┐
│                     PRESENTATION LAYER                      │
│  ┌─────────────┐                  ┌─────────────────────┐  │
│  │   SGCM.Web  │                  │   SGCM.Desktop      │  │
│  │   (MVC)     │                  │   (Windows Forms)    │  │
│  └─────────────┘                  └─────────────────────┘  │
├─────────────────────────────────────────────────────────────┤
│                        API LAYER                            │
│  ┌───────────────────────────────────────────────────────┐  │
│  │                    SGCM.API                           │  │
│  │         Controllers │ Middleware │ Swagger            │  │
│  └───────────────────────────────────────────────────────┘  │
├─────────────────────────────────────────────────────────────┤
│                    APPLICATION LAYER                       │
│  ┌───────────────────────────────────────────────────────┐  │
│  │                  SGCM.Application                      │  │
│  │    App Services │ DTOs │ Interfaces │ Logging         │  │
│  └───────────────────────────────────────────────────────┘  │
├─────────────────────────────────────────────────────────────┤
│                      DOMAIN LAYER                           │
│  ┌───────────────────────────────────────────────────────┐  │
│  │                    SGCM.Domain                        │  │
│  │  Entities │ Domain Services │ Exceptions │ Validations │  │
│  └───────────────────────────────────────────────────────┘  │
├─────────────────────────────────────────────────────────────┤
│                   INFRASTRUCTURE LAYER                      │
│  ┌──────────────────┐          ┌──────────────────────────┐ │
│  │ SGCM.Infraestructure │      │    SGCM.Persistence       │ │
│  │ JWT │ Notifications  │      │ EF Core │ Repositories    │ │
│  └──────────────────┘          └──────────────────────────┘ │
└─────────────────────────────────────────────────────────────┘
```

### Capas del Proyecto

| Capa | Proyecto | Descripción |
|------|----------|-------------|
| **Domain** | `SGCM.Domain` | Entidades del negocio, reglas de negocio, excepciones personalizadas, validaciones, interfaces de repositorio |
| **Application** | `SGCM.Application` | Servicios de aplicación, DTOs, interfaces de servicios, logging de auditoría |
| **Infrastructure** | `SGCM.Infraestructure` | Servicios externos: JWT, notificaciones (Email, SMS, Push) |
| **Persistence** | `SGCM.Persistence` | Entity Framework Core, repositorios concretos, Unit of Work, contexto de base de datos |
| **API** | `SGCM.API` | Controladores REST, middleware de excepciones, configuración de dependencias, Swagger |
| **Web** | `SGCM.Web` | Frontend web usando ASP.NET Core MVC |
| **Desktop** | `SGCM.Desktop` | Aplicación de escritorio para Windows |

## Patrones y Principios Implementados

### Domain-Driven Design (DDD)
- **Entidades** con identidad propia y validación intrínseca
- **Value Objects** para conceptos del dominio
- **Domain Services** para reglas de negocio complejas
- **Repository Pattern** con interfaces genéricas

### Patrones de Diseño
- **Repository Pattern**: Abstracción del acceso a datos
- **Unit of Work**: Gestión de transacciones
- **Dependency Injection**: Inyección de dependencias nativa de .NET
- **Middleware**: Manejo centralizado de excepciones

### Características Avanzadas
- **Soft Deletes**: Eliminación lógica con filtros globales de Entity Framework
- **Validación en Entidades**: Método `ValidarEntradaDatos()` en cada entidad
- **Auditoría Completa**: Registro de todas las operaciones con usuario, fecha y detalles

## Estructura de Carpetas

```
SGCM/
├── SGCM.slnx                     # Solución
├── README.md                     # Este archivo
│
├── SGCM.API/                     # API REST
│   ├── Controllers/              # Controladores
│   ├── Dependencies/             # Inyección de dependencias
│   ├── Middleware/                # Middleware personalizado
│   └── Program.cs
│
├── SGCM.Application/             # Capa de aplicación
│   ├── Base/                      # Clases base
│   ├── Dtos/                      # Data Transfer Objects
│   ├── Interfaces/               # Interfaces de servicios
│   ├── Logger/                   # Sistema de auditoría
│   └── Services/                  # Servicios de aplicación
│
├── SGCM.Domain/                  # Capa de dominio
│   ├── Base/                      # Entidades base
│   ├── Entities/                  # Entidades del negocio
│   │   ├── Citas_Agenda/
│   │   ├── Pacientes/
│   │   └── Seguridad_Usuarios/
│   ├── Exceptions/               # Excepciones personalizadas
│   ├── Repository/               # Interfaces de repositorio
│   ├── Services/                 # Servicios de dominio
│   └── Validaciones/             # Clases de validación
│
├── SGCM.Infraestructure/         # Infraestructura
│   ├── Notifications/           # Servicios de notificación
│   └── Services/                 # Servicios JWT
│
├── SGCM.Persistence/             # Persistencia
│   ├── Context/                  # DbContext
│   ├── Repositories/             # Repositorios concretos
│   └── UnitOfWork.cs
│
├── SGCM.Web/                     # Frontend Web (MVC)
│   ├── Controllers/
│   ├── Views/
│   ├── wwwroot/
│   └── appsettings.json
│
└── SGCM.Desktop/                 # Aplicación de escritorio
    └── Form1.cs
```

## Tecnologías Utilizadas

- **.NET 8.0** - Framework principal
- **Entity Framework Core 8** - ORM para acceso a datos
- **SQL Server** - Base de datos relacional
- **JWT Bearer** - Autenticación y autorización
- **Swagger/OpenAPI** - Documentación de API (Swashbuckle)
- **Windows Forms** - Aplicación de escritorio
- **Bootstrap 5** - Framework CSS para el frontend web
- **jQuery** - Biblioteca JavaScript

## Requisitos

- **.NET 8.0 SDK** o superior
- **SQL Server** (Local, Express o Azure)
- **Visual Studio 2022** o **VS Code** (recomendado)

## Instalación y Ejecución

### 1. Clonar el repositorio

```bash
git clone <url-del-repositorio>
cd SGCM
```

### 2. Configurar la base de datos

Editar el archivo `SGCM.API/appsettings.json` o `SGCM.Web/appsettings.json` con la cadena de conexión:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=TU_SERVIDOR;Database=SGCM;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

### 3. Restaurar paquetes

```bash
dotnet restore
```

### 4. Ejecutar migraciones (opcional)

```bash
cd SGCM.Persistence
dotnet ef database update
```

### 5. Ejecutar el proyecto

#### API
```bash
cd SGCM.API
dotnet run
```

#### Web
```bash
cd SGCM.Web
dotnet run
```

La API estará disponible en: `https://localhost:5001`
La documentación Swagger en: `https://localhost:5001/swagger`

## Endpoints Principales

### Autenticación
- `POST /api/auth/login` - Iniciar sesión
- `POST /api/auth/register` - Registrar usuario

### Pacientes
- `GET /api/paciente` - Listar pacientes
- `GET /api/paciente/{id}` - Obtener paciente por ID
- `POST /api/paciente` - Crear paciente
- `PUT /api/paciente/{id}` - Actualizar paciente
- `DELETE /api/paciente/{id}` - Eliminar paciente

### Citas
- `GET /api/cita` - Listar citas
- `GET /api/cita/{id}` - Obtener cita por ID
- `POST /api/cita` - Crear cita
- `PUT /api/cita/{id}` - Actualizar cita
- `DELETE /api/cita/{id}` - Cancelar cita

### Médicos
- `GET /api/medico` - Listar médicos
- `GET /api/medico/{id}` - Obtener médico por ID
- `POST /api/medico` - Crear médico
- `PUT /api/medico/{id}` - Actualizar médico
- `DELETE /api/medico/{id}` - Eliminar médico

### Especialidades
- `GET /api/especialidades` - Listar especialidades
- `POST /api/especialidades` - Crear especialidad

### Disponibilidad
- `GET /api/disponibilidad` - Listar disponibilidad
- `POST /api/disponibilidad` - Crear disponibilidad

### Usuarios
- `GET /api/usuario` - Listar usuarios
- `PUT /api/usuario/cambiar-password` - Cambiar contraseña

### Auditoría
- `GET /api/auditoria` - Ver registros de auditoría

## Licencia

Este proyecto está bajo desarrollo.

## Sistema de Gestión y Control de Acceso de Empleados

API REST para administrar empleados, controlar sus credenciales de acceso, registrar entradas y salidas en los puntos de acceso de la sede y consultar el historial de movimientos. Caso de estudio de Programación Distribuida.

Construido con Clean Architecture, DDD y CQRS, sobre SQL Server con Entity Framework Core.

## Arquitectura

| Proyecto | Responsabilidad |
| --- | --- |
| `ControlAcceso.Dominio` | Entidades, objetos de valor y reglas de negocio. No depende de nada. |
| `ControlAcceso.Aplicacion` | Casos de uso separados en comandos y consultas, contratos de repositorios, mediador y validaciones. |
| `ControlAcceso.Persistencia` | Implementa los contratos con Entity Framework Core sobre SQL Server. |
| `ControlAcceso.Api` | Controladores REST y traducción de errores a códigos HTTP. |

Los comandos (`CasosDeUso/*/Comandos`) modifican el estado y confirman con la unidad de trabajo. Las consultas (`CasosDeUso/*/Consultas`) solo leen y devuelven DTOs. Un mediador propio resuelve el manejador de cada petición y ejecuta su validador antes de llegar al caso de uso.

## Requisitos

- .NET SDK 10.0
- SQL Server local, Express o LocalDB
- `dotnet tool install --global dotnet-ef`

## Ejecutar

1. Ajustar `ConexionPredeterminada` en `ControlAcceso.Api/appsettings.json`:

```json
"ConnectionStrings": {
  "ConexionPredeterminada": "Server=(localdb)\\MSSQLLocalDB;Database=ControlAccesoDb;Trusted_Connection=True;TrustServerCertificate=True"
}
```

2. Compilar, generar la migración y ejecutar:

```bash
dotnet build
dotnet ef migrations add Inicial --project ControlAcceso.Persistencia --startup-project ControlAcceso.Api --output-dir Migraciones
dotnet run --project ControlAcceso.Api
```

Al arrancar aplica las migraciones pendientes y carga los datos iniciales: seis áreas organizacionales y cuatro puntos de acceso. La API queda en `http://localhost:5296`.

## Endpoints

### RF-01. Gestión de empleados

| Método | Ruta | Descripción |
| --- | --- | --- |
| `POST` | `/api/empleados` | Registra un empleado. |
| `GET` | `/api/empleados/{id}` | Consulta un empleado por identificador. |
| `GET` | `/api/empleados` | Lista paginada con filtros `areaId` y `estado`. |
| `PUT` | `/api/empleados/{id}` | Actualiza los datos de un empleado activo. |
| `PATCH` | `/api/empleados/{id}/desactivar` | Desactiva el vínculo laboral. |

La identificación y el correo son únicos. La identificación no se modifica después del registro y un empleado inactivo no admite actualizaciones.

### RF-02. Gestión de credenciales

| Método | Ruta | Descripción |
| --- | --- | --- |
| `POST` | `/api/credenciales` | Asigna una credencial a un empleado. |
| `GET` | `/api/credenciales/codigo/{codigo}` | Consulta el estado de una credencial. |
| `GET` | `/api/credenciales/empleado/{empleadoId}` | Lista las credenciales de un empleado. |
| `PATCH` | `/api/credenciales/{id}/bloquear` | Bloquea una credencial activa. |
| `PATCH` | `/api/credenciales/{id}/revocar` | Revoca una credencial. |

El código es único y se guarda en mayúsculas. Solo un empleado activo recibe credenciales, solo una credencial activa se bloquea, y una revocada no admite cambios.

### RF-03. Registro de entradas y salidas

| Método | Ruta | Descripción |
| --- | --- | --- |
| `POST` | `/api/accesos` | Registra un movimiento con el código de la credencial, el punto de acceso y el tipo de movimiento. |

Todo movimiento queda registrado, autorizado o rechazado. Se rechaza cuando la credencial está bloqueada o revocada, cuando el empleado no tiene vínculo laboral activo, cuando el punto de acceso está deshabilitado, cuando se pide una entrada y el empleado ya registra una entrada sin salida, o cuando se pide una salida sin entrada previa. Cada rechazo guarda su motivo en el historial.

Una credencial o un punto de acceso inexistentes devuelven 404, porque sin ellos no hay movimiento que registrar.

### RF-04. Consulta del historial de accesos

| Método | Ruta | Descripción |
| --- | --- | --- |
| `GET` | `/api/accesos` | Historial paginado con filtros `empleadoId`, `desde`, `hasta`, `puntoAccesoId` y `resultado`. |

El tamaño de página admite entre 1 y 50 registros y de forma predeterminada devuelve 15.

### RF-06. Administración de puntos de acceso

| Método | Ruta | Descripción |
| --- | --- | --- |
| `POST` | `/api/puntos-acceso` | Registra un punto de acceso físico. |
| `GET` | `/api/puntos-acceso/{id}` | Consulta un punto de acceso. |
| `GET` | `/api/puntos-acceso` | Lista paginada con filtros `habilitado` y `tipo`. |
| `PATCH` | `/api/puntos-acceso/{id}/habilitar` | Habilita el punto de acceso. |
| `PATCH` | `/api/puntos-acceso/{id}/deshabilitar` | Deshabilita el punto de acceso. |

Los tipos son `Recepcion`, `Parqueadero`, `Bodega` y `AreaRestringida`.

### Apoyo

| Método | Ruta | Descripción |
| --- | --- | --- |
| `GET` | `/api/areas` | Lista las áreas organizacionales necesarias para registrar empleados. |

## Formato de respuesta

Todas las respuestas comparten la misma estructura. Los errores devuelven 400 por validación, 404 por recurso inexistente, 409 por regla de negocio incumplida y 500 por error no controlado:

```json
{
  "exitoso": false,
  "mensaje": "La operación no cumple una regla de negocio",
  "errores": [
    {
      "codigo": "Negocio.ReglaViolada",
      "descripcion": "No se puede asignar una credencial a un empleado inactivo.",
      "tipo": "ReglaNegocio"
    }
  ]
}
```

## Evidencias

`ControlAcceso.Api/ControlAcceso.Api.http` contiene el recorrido de verificación agrupado por requerimiento, con los casos correctos y los de error de cada regla. Las peticiones se ejecutan en orden desde `listarAreas`, porque cada una reutiliza los identificadores que devolvieron las anteriores.

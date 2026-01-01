# WebSystem

![.NET](https://img.shields.io/badge/.NET-8.0-512bd4)
![CI](https://img.shields.io/badge/CI-GitHub%20Actions-success)
![License](https://img.shields.io/badge/License-CC%20BY--NC%204.0-lightgrey)

## 📌 Descripción del Proyecto

**WebSystem** es una solución web desarrollada con **.NET 8** orientada a la gestión y estructuración de lógica empresarial mediante una arquitectura modular, mantenible y preparada para entornos profesionales.

El proyecto se concibe como un **trabajo de carácter profesional y académico**, con especial atención a la separación de responsabilidades, la estabilidad del código y la preparación para despliegues en la nube, sin introducir dependencias innecesarias ni complejidad artificial.

---

## 🧱 Arquitectura y Estructura de la Solución

La solución sigue una **arquitectura multicapa (N-Tier)** con separación clara de responsabilidades, alineada con principios de **Clean Architecture**:

- **WebSystem.Api**  
  API REST desarrollada con ASP.NET Core. Actúa como punto de entrada al sistema y coordina la lógica de aplicación.

- **WebSystem.Web**  
  Cliente web basado en **HTML, JavaScript nativo y CSS**, encargado de la interacción con el usuario.

- **WebSystem.Domain**  
  Núcleo del dominio. Contiene las entidades, reglas de negocio y lógica pura, sin dependencias externas.

- **WebSystem.Application**  
  Capa de aplicación que orquesta los casos de uso, DTOs y contratos, actuando como intermediaria entre la API y el dominio.

- **WebSystem.Infrastructure**  
  Implementación de la persistencia y dependencias externas, incluyendo **Entity Framework Core** y acceso a datos.

- **WebSystem.Utilities**  
  Conjunto de utilidades y componentes transversales reutilizables dentro de la solución.

- **WebSystem.WorkerService**  
  Servicio de segundo plano basado en **Worker Service**, destinado a la ejecución de procesos asíncronos y tareas no interactivas.

- **WebSystem.Tests**  
  Proyecto de pruebas unitarias y de integración para validar la estabilidad del sistema.

Esta organización favorece el desacoplamiento, la mantenibilidad y la evolución controlada del proyecto.

---

## 🛠️ Tecnologías Utilizadas

- **Lenguaje:** C# 12
- **Framework:** .NET 8 / ASP.NET Core
- **Frontend:** HTML, JavaScript nativo, CSS
- **Persistencia:** Entity Framework Core
- **Testing:** xUnit
- **CI:** GitHub Actions
- **Plataforma Cloud:** Microsoft Azure (preparado para despliegue)

---

## 🔄 Integración Continua (CI)

El repositorio cuenta con **Integración Continua mediante GitHub Actions**, configurada para:

- Ejecutar compilación y pruebas automáticamente.
- Garantizar la estabilidad del código antes de permitir merges.
- Proteger la rama principal mediante validaciones automáticas.

Este enfoque asegura un flujo de trabajo controlado y acorde a prácticas profesionales.

---

## ☁️ Despliegue y Configuración

El proyecto está **preparado para desplegarse en Microsoft Azure** directamente desde Visual Studio.

### Gestión de Secretos

Por diseño, **no se almacenan claves ni secretos en los archivos de configuración** (`appsettings.json`). En su lugar:

- En entorno local se utiliza el **Windows Secret Store**.
- En entornos cloud se contempla el uso de **Azure (Key Vault / servicios equivalentes)**.

Esto mantiene los archivos de configuración limpios y evita la exposición de información sensible.

---

## ⚙️ Requisitos

- SDK **.NET 8.0**
- Visual Studio 2022 (17.12+) o compatible
- Motor de base de datos compatible con EF Core (según configuración)

---

## 🧪 Ejecución de Pruebas

Para ejecutar las pruebas automatizadas:

```bash
dotnet test
```

---

## 📈 Enfoque Profesional

WebSystem prioriza:

- Claridad arquitectónica
- Código mantenible y testeable
- Flujo de trabajo seguro mediante CI
- Preparación realista para despliegue en la nube

El proyecto evita promesas técnicas no implementadas y refleja fielmente su estado actual.

---

## 👤 Autor

**David (DaveNetDeveloper)**  
Proyecto .NET de carácter profesional y académico


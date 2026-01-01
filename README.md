# WebSystem

![.NET](https://img.shields.io/badge/.NET-8.0-512bd4)
![CI](https://img.shields.io/badge/CI-GitHub%20Actions-success)
![License](https://img.shields.io/badge/License-CC%20BY--NC%204.0-lightgrey)

## ğŸ§© DescripciÃ³n del Proyecto

**WebSystem** es una solución web desarrollada con **.NET 8**, orientada a la construcción de aplicaciones profesionales mediante una **arquitectura limpia, modular y mantenible**.

El proyecto tiene un **enfoque profesional y académico**, priorizando buenas prácticas reales del sector: separación de responsabilidades, testabilidad, integración continua y preparación para despliegues en entornos cloud modernos.

No se trata de un proyecto “demo”, sino de una base sólida pensada para **evolucionar, mantenerse y desplegarse en producción**.

---

## ğŸ—ï¸ Arquitectura y Estructura de la SoluciÃ³n

La solución sigue una **arquitectura multicapa (N-Tier)** alineada con principios de **Clean Architecture**, garantizando bajo acoplamiento y alta cohesión.

### Proyectos principales

- **WebSystem.Api**  
  API REST desarrollada con **ASP.NET Core**, responsable de exponer los endpoints y coordinar los casos de uso.

- **WebSystem.Web**  
  Cliente web basado en **HTML, JavaScript nativo y CSS**, desacoplado de la API y consumiendo datos vía HTTP.

- **WebSystem.Domain**  
  Núcleo del dominio. Contiene entidades y reglas de negocio puras, sin dependencias externas.

- **WebSystem.Application**  
  Capa de aplicación que define casos de uso, DTOs y contratos. Actúa como intermediaria entre la API y el dominio.

- **WebSystem.Infrastructure**  
  Implementación de la persistencia y dependencias externas, incluyendo **Entity Framework Core** y acceso a datos.

- **WebSystem.Utilities**  
  Componentes y utilidades transversales reutilizables en toda la solución.

- **WebSystem.WorkerService**  
  Servicio en segundo plano basado en **Worker Service**, orientado a tareas asíncronas, procesos programados o trabajos no interactivos.

- **WebSystem.Tests**  
  Proyecto de **pruebas unitarias e integración**, enfocado a validar la estabilidad y el comportamiento del sistema.

Esta estructura favorece la mantenibilidad, la escalabilidad y un desarrollo alineado con estándares profesionales.

---

## ğŸ› ï¸ TecnologÃ­as Utilizadas

- **Lenguaje:** C# 12  
- **Framework:** .NET 8 / ASP.NET Core  
- **Frontend:** HTML, JavaScript nativo, CSS  
- **Persistencia:** Entity Framework Core  
- **Testing:** xUnit  
- **CI:** GitHub Actions  
- **Cloud & Hosting:**  
  - **Microsoft Azure**  
  - **Railway**  
- **Publicación:** Perfiles de publicación de **Visual Studio**

---

## ğŸ”„ IntegraciÃ³n Continua (CI)

El repositorio cuenta con **Integración Continua mediante GitHub Actions**, configurada para:

- Compilar la solución automáticamente.
- Ejecutar las pruebas.
- Validar cambios antes de permitir merges a ramas protegidas.

Este flujo garantiza estabilidad y reduce errores en fases avanzadas del desarrollo.

---

## ğŸš€ Despliegue y PublicaciÃ³n

El proyecto está **preparado para despliegues reales en la nube**, utilizando distintas estrategias según el entorno.

### Publicación desde Visual Studio

- Se emplean **perfiles de publicación de Visual Studio**, permitiendo:
  - Despliegues directos a **Microsoft Azure App Service**.
  - Configuración clara por entorno (Development / Production).
  - Separación entre código y configuración.

### Plataformas Cloud soportadas

- **Microsoft Azure**  
  Despliegue de API y Web mediante App Services, con configuración por entorno.

- **Railway**  
  Alternativa ligera para despliegues rápidos, especialmente útil en entornos de pruebas o demostración.

La arquitectura del proyecto permite cambiar de proveedor cloud sin modificaciones estructurales relevantes.

---

## ğŸ” GestiÃ³n de ConfiguraciÃ³n y Secretos

Por diseño, **no se almacenan claves ni secretos en el repositorio**.

- En entorno local:
  - Uso de **Secret Manager (User Secrets)** de .NET.
- En entornos cloud:
  - Configuración mediante variables de entorno.
  - Preparado para integración con **Azure Key Vault** u otros servicios equivalentes.

Esto evita la exposición de información sensible y mantiene los archivos de configuración limpios.

---

## ğŸ“‹ Requisitos

- **.NET SDK 8.0**
- Visual Studio 2022 (17.12 o superior) o compatible
- Motor de base de datos compatible con Entity Framework Core

---

## ğŸ§ª EjecuciÃ³n de Pruebas

Para ejecutar las pruebas automatizadas:

```bash
dotnet test


## ğŸ’¼ Enfoque Profesional

WebSystem prioriza:
- Arquitectura clara y desacoplada
- Código mantenible y testeable
- Flujo de trabajo controlado mediante CI
- Despiegues en la nube (Azure / Railway)
- Uso responsable de configuraciones y secretos
El proyecto refleja fielmente su estado actual, sin promesas técnicas no implementadas.

## ğŸ‘¤ Autor

David Martínez Gómez
Proyecto .NET de carácter académico y personal
# WebSystem

![.NET](https://img.shields.io/badge/.NET-8.0-512bd4)
![CI](https://img.shields.io/badge/CI-GitHub%20Actions-success)
![License](https://img.shields.io/badge/License-CC%20BY--NC%204.0-lightgrey)

## 🧩 Descripci�n del Proyecto

**WebSystem** es una soluci�n web desarrollada con **.NET 8**, orientada a la construcci�n de aplicaciones profesionales mediante una **arquitectura limpia, modular y mantenible**.

El proyecto tiene un **enfoque profesional y acad�mico**, priorizando buenas pr�cticas reales del sector: separaci�n de responsabilidades, testabilidad, integraci�n continua y preparaci�n para despliegues en entornos cloud modernos.

No se trata de un proyecto �demo�, sino de una base s�lida pensada para **evolucionar, mantenerse y desplegarse en producci�n**.

--- 

## 🧩 Arquitectura y Estructura de la Soluci�n

La soluci�n sigue una **arquitectura multicapa (N-Tier)** alineada con principios de **Clean Architecture**, garantizando bajo acoplamiento y alta cohesi�n.

### Proyectos principales

- **WebSystem.Api**  
  API REST desarrollada con **ASP.NET Core**, responsable de exponer los endpoints y coordinar los casos de uso.

- **WebSystem.Web**  
  Cliente web basado en **HTML, JavaScript nativo y CSS**, desacoplado de la API y consumiendo datos v�a HTTP.

- **WebSystem.Domain**  
  N�cleo del dominio. Contiene entidades y reglas de negocio puras, sin dependencias externas.

- **WebSystem.Application**  
  Capa de aplicaci�n que define casos de uso, DTOs y contratos. Act�a como intermediaria entre la API y el dominio.

- **WebSystem.Infrastructure**  
  Implementaci�n de la persistencia y dependencias externas, incluyendo **Entity Framework Core** y acceso a datos.

- **WebSystem.Utilities**  
  Componentes y utilidades transversales reutilizables en toda la soluci�n.

- **WebSystem.WorkerService**  
  Servicio en segundo plano basado en **Worker Service**, orientado a tareas as�ncronas, procesos programados o trabajos no interactivos.

- **WebSystem.Tests**  
  Proyecto de **pruebas unitarias e integraci�n**, enfocado a validar la estabilidad y el comportamiento del sistema.

Esta estructura favorece la mantenibilidad, la escalabilidad y un desarrollo alineado con est�ndares profesionales.

---

## 🧩 Tecnolog�as Utilizadas

- **Lenguaje:** C# 12  
- **Framework:** .NET 8 / ASP.NET Core  
- **Frontend:** HTML, JavaScript nativo, CSS  
- **Persistencia:** Entity Framework Core  
- **Testing:** xUnit  
- **CI:** GitHub Actions  
- **Cloud & Hosting:**  
  - **Microsoft Azure**  
  - **Railway**  
- **Publicaci�n:** Perfiles de publicaci�n de **Visual Studio**

---

## 🔄 Integraci�n Continua (CI)

El repositorio cuenta con **Integraci�n Continua mediante GitHub Actions**, configurada para:

- Compilar la soluci�n autom�ticamente.
- Ejecutar las pruebas.
- Validar cambios antes de permitir merges a ramas protegidas.

Este flujo garantiza estabilidad y reduce errores en fases avanzadas del desarrollo.

---

## 🚀 Despliegue y Publicaci�n

El proyecto est� **preparado para despliegues reales en la nube**, utilizando distintas estrategias seg�n el entorno.

### Publicaci�n desde Visual Studio

- Se emplean **perfiles de publicaci�n de Visual Studio**, permitiendo:
  - Despliegues directos a **Microsoft Azure App Service**.
  - Configuraci�n clara por entorno (Development / Production).
  - Separaci�n entre c�digo y configuraci�n.

### Plataformas Cloud soportadas

- **Microsoft Azure**  
  Despliegue de API y Web mediante App Services, con configuraci�n por entorno.

- **Railway**  
  Alternativa ligera para despliegues r�pidos, especialmente �til en entornos de pruebas o demostraci�n.

La arquitectura del proyecto permite cambiar de proveedor cloud sin modificaciones estructurales relevantes.

---

## 🔐 Gesti�n de Configuraci�n y Secretos

Por dise�o, **no se almacenan claves ni secretos en el repositorio**.

- En entorno local:
  - Uso de **Secret Manager (User Secrets)** de .NET.
- En entornos cloud:
  - Configuraci�n mediante variables de entorno.
  - Preparado para integraci�n con **Azure Key Vault** u otros servicios equivalentes.

Esto evita la exposici�n de informaci�n sensible y mantiene los archivos de configuraci�n limpios.

---

## 📋 Requisitos

- **.NET SDK 8.0**
- Visual Studio 2022 (17.12 o superior) o compatible
- Motor de base de datos compatible con Entity Framework Core

---

## 🧪 Ejecuci�n de Pruebas

Para ejecutar las pruebas automatizadas:

```bash
dotnet test


## Enfoque Profesional

WebSystem prioriza:
- Arquitectura clara y desacoplada
- C�digo mantenible y testeable
- Flujo de trabajo controlado mediante CI
- Despiegues en la nube (Azure / Railway)
- Uso responsable de configuraciones y secretos
El proyecto refleja fielmente su estado actual, sin promesas t�cnicas no implementadas.

## 👤 Autor

David Mart�nez G�mez
Proyecto .NET de car�cter acad�mico y personal
# 🧪 Tests

Este proyecto contiene todos los tests automatizados de la solución, organizados por tipo de prueba. Está construido con **NUnit**, **SpecFlow** y **Playwright para .NET**, y centraliza los diferentes niveles de validación del sistema (desde tests unitarios hasta pruebas funcionales de UI).

---

## 📁 Estructura de carpetas y tipos de test

```
Test/
├── Integration/          # Pruebas de integración entre componentes reales
│   ├── AuthHelper.cs
│   ├── CustomWebApplicationFactory.cs
│   └── UsuariosControllerTests.cs
├── Performance/          # (Vacío actualmente, reservado para tests de rendimiento)
├── Security/             # (Vacío actualmente, reservado para tests de seguridad)
├── UI/                   # Pruebas funcionales y de UI automatizadas
│   ├── Features/
│   │   └── Login.feature
│   ├── StepsDefinitions/
│   │   └── LoginSteps.cs
│   ├── TestData/
│   │   └── logins.json
│   └── UsuariosTests.cs
├── UnitTest/             # Pruebas unitarias puras (sin dependencias externas)
│   └── UsuariosControllerTests.cs
├── appsettings.json      # Configuración para entornos de prueba
└── GlobalUsings.cs       # Usings globales para simplificar el código de test
```

---

## 🧪 Tipos de pruebas incluidas

### ✅ Pruebas Unitarias (`/UnitTest`)
- Validan la lógica de negocio de forma aislada.
- Sin dependencias externas (base de datos, red, etc.).
- Tecnología usada: **NUnit**.

### 🔗 Pruebas de Integración (`/Integration`)
- Verifican el comportamiento de componentes reales interactuando entre sí.
- Usan `CustomWebApplicationFactory` para montar un entorno de pruebas.
- Tecnología usada: **NUnit**, `Microsoft.AspNetCore.Mvc.Testing`.

### 🧑‍💻 Pruebas Funcionales / UI (`/UI`)
- Pruebas de extremo a extremo usando Gherkin + SpecFlow.
- Automación de interfaz de usuario.
- Manejo de datos de prueba en JSON.
- Tecnologías usadas:
  - **SpecFlow** (BDD + Gherkin)
  - **Playwright para .NET** (automatización del navegador)
  - **NUnit** (como runner base)

### 🚀 Futuras categorías (vacías por ahora)
- **Performance**: para incluir pruebas de rendimiento con herramientas como BenchmarkDotNet o JMeter.
- **Security**: para validaciones automáticas relacionadas con autenticación/autorización o análisis de vulnerabilidades.

---

## ⚙️ Ejecución de tests

### Ejecutar todos los tests:
```bash
dotnet test
```

### Ejecutar por categoría (por ejemplo, integración):
```bash
dotnet test --filter "TestCategory=Integration"
```

> Nota: asegúrate de usar `[Category("...")]` en los tests NUnit para poder filtrarlos.

---

## 📝 Requisitos

- [.NET 7 SDK o superior](https://dotnet.microsoft.com/)
- NUnit
- SpecFlow for .NET
- Playwright for .NET (`Microsoft.Playwright.NUnit`)

---

## 🔧 Notas adicionales

- Si usas SpecFlow, asegúrate de que la generación automática de código está correctamente configurada con MSBuild.
- Usa `CustomWebApplicationFactory` para montar entornos controlados en pruebas de integración.

---

## 📌 Pendiente

- Implementar tests reales de rendimiento y seguridad.
- Centralizar configuración sensible de entornos en variables de entorno o archivos protegidos.

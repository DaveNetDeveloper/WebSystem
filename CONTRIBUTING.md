# Guía de Contribución

Este proyecto sigue estándares profesionales de ingeniería de software para asegurar la calidad y mantenibilidad del código.

## 1. Flujo de Trabajo (Gitflow)
Se utiliza el modelo de ramificación **Gitflow**:
* **master**: Rama principal y protegida. Contiene el código en estado de producción.
* **develop**: Rama de integración para el desarrollo.
* **feature/**: Ramas temporales para el desarrollo de nuevas funcionalidades.
* **release/**: Ramas de preparación para lanzamientos de versiones.

## 2. Integración Continua (CI)
El repositorio cuenta con **GitHub Actions** configurados para ejecutar la suite de tests automáticos ante cada evento de `Push` o `Pull Request`.
* La integración en la rama `master` está bloqueada si los tests automáticos no finalizan con éxito.

## 3. Estándares de Código
Se deben seguir las convenciones oficiales de Microsoft para C#:
* **PascalCase** para nombres de clases, métodos y propiedades.
* **camelCase** para variables locales y parámetros.
* Uso intensivo de **Interfaces** para garantizar el desacoplamiento y la inyección de dependencias.

## 4. Calidad de Commits (Conventional Commits)
Se requiere que los mensajes de commit sigan el siguiente estándar descriptivo:
* `feat:` Nuevas funcionalidades.
* `fix:` Corrección de errores.
* `docs:` Cambios en la documentación.
* `test:` Adición o modificación de pruebas.
* `chore:` Tareas de mantenimiento (build, configuración, dependencias).
* `style:` Cambios de formato o estilo (espacios, puntos y coma, etc., sin afectar la lógica).
* `refactor:` Refactorización de código que no cambia el comportamiento externo.

## 5. Proceso de Pull Request
* Ningún cambio entra en `master` sin una revisión previa.
* Es obligatorio que los tests automáticos pasen antes de completar la fusión.

## 6. Proceso de Despliegue (CD)
El despliegue se realiza de forma manual mediante la publicación directa desde **Visual Studio** hacia **Microsoft Azure**, asegurando la validación previa en el entorno local.
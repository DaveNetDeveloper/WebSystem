# UI

Este proyecto representa la interfaz de usuario del sistema. Está compuesto por páginas HTML estáticas que interactúan con la API mediante JavaScript.

---

## ⚙️ Tecnologías utilizadas

- HTML5, CSS3 y JavaScript
- No se utiliza ningún framework de frontend (como React o Angular)
- Las peticiones a la API se hacen mediante `fetch`

---

## 🔒 Seguridad

Actualmente la autenticación es manual. En futuras versiones se integrará **autenticación con JWT Bearer**, que se gestionará desde la UI y se enviará en las cabeceras a los endpoints protegidos.

---

## 🔗 Conexión con la API

El frontend realiza peticiones a la API expuesta por el proyecto `API`. La URL base se configura manualmente (por ejemplo, mediante `appsettings.json` o una variable global JS en producción).

---

## 📁 Estructura de carpetas

UI/
│
├───assets/
│ ├───css/
│ └───img/
│
├───components/
│ ├───header/
│ └───footer/
│
├───pages/
│ └───(archivos HTML)
│
├───scripts/
│ ├───api/
│ ├───auth/
│ └───utils/
│
└───README_UI.md


---

## 🧪 Tests

Este proyecto no contiene tests propios. Las pruebas funcionales y de UI están centralizadas en el repositorio de tests (`Test`), donde se utilizan:

- **NUnit** para pruebas unitarias y de integración
- **SpecFlow** para pruebas BDD
- **Playwright** para pruebas E2E sobre la UI

---

## 📦 Deploy

Este frontend puede desplegarse como aplicación estática en:

- GitHub Pages
- Netlify
- Azure Static Web Apps
- Cualquier servidor web (Apache, Nginx…)

---

## 📝 Notas

- Cabecera (`header`) y pie de página (`footer`) se incluyen dinámicamente mediante JavaScript
- La estructura modular permite una fácil escalabilidad futura

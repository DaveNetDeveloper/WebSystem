// 
// CONFIGURACIÓNES GLOBALES
// 

// Variables globales
export let baseUrl = null;
export let port = null;
export let accessTokenCookieName = null;
export let refreshTokenCookieName = null;
export let roleCookieName = null;
export let profileCookieName = null;

export let authControllerName = null;
export let usuariosControllerName = null;
export let perfilesControllerName = null;
export let rolesControllerName = null;
export let notificationsControllerName = null;
export let emailTokensControllerName = null;
export let inAppNotificationControllerName = null;
export let loginsControllerName = null;
export let productosControllerName = null;

export let loginType_Web = 'Web';
export let loginType_Admin = 'Admin';
 
function getCookie(name) {
    const value = `; ${document.cookie}`;
    const parts = value.split(`; ${name}=`);
    if (parts.length === 2) {
        return parts.pop().split(";").shift();
    }
    return null;
}

export async function loadGlobalConfigurationFromConfig() {

    baseUrl = window.location.hostname === "localhost"
        ? "https://localhost"
        : "https://websystem-api-prod.azurewebsites.net";


    port = = window.location.hostname === "localhost"
        ? "44311"
        : ""; 
}

//
// Carga la configuración global desde la API nusando el token guardado en las cookies.
// 
export async function loadGlobalConfiguration() {
    try {
        const token = getCookie("app-access-token");
        if (!token) {
            console.error("No se pudo obtener el access_token desde la cookie.");
            return;
        }

        const baseUrl = window.location.hostname === "localhost"
            ? "https://localhost:44311"
            : "https://websystem-api-prod-ctgrd4gfc2e6dudb.westeurope-01.azurewebsites.net";

        const response = await fetch(`${baseUrl}/AppConfiguration/GetConfiguration`, {
            method: "GET",
            headers: {
                "Authorization": `Bearer ${token}`,
                "Content-Type": "application/json"
            }
        });

        if (!response.ok) {
            console.error("Error al llamar al endpoint GetConfiguration:", response.statusText);
            return;
        }
        const data = await response.json();
        console.log("JSON recibido de API:", data);

        // Asignar variables globales
        baseUrl = data.baseUrl;
        port = data.port;
        accessTokenCookieName = data.accessTokenCookieName;
        refreshTokenCookieName = data.refreshTokenCookieName;
        roleCookieName = data.roleCookieName;
        profileCookieName = data.profileCookieName;

        authControllerName = data.controllerName.authController;
        usuariosControllerName = data.controllerName.usuariosController;;
        perfilesControllerName = data.controllerName.perfilesController;
        rolesControllerName = data.controllerName.rolesController;
        notificationsControllerName = data.controllerName.notificationsController;
        emailTokensControllerName = data.controllerName.emailTokensController;
        inAppNotificationControllerName = data.controllerName.inAppNotificationController;
        loginsControllerName = data.controllerName.loginsController;
        productosControllerName = data.controllerName.productosController;

        console.log("Configuración cargada correctamente:", { baseUrl, port, accessTokenCookieName, refreshTokenCookieName, roleCookieName, profileCookieName, authControllerName });
    }
    catch (error) {
        console.error("Error al obtener configuración global:", error);
    }
}
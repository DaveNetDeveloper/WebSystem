async function fetchWithAuth(url, options = {}) {

    // Añadimos el token a los headers
    const token = getCookie('app-access-token');
    options.headers = {
        'Content-Type': 'application/json',
        ...options.headers,                  // mantiene headers que se pasen
        ...(token ? { 'Authorization': `Bearer ${token}` } : {}) // añade token
    };

    try {
        let response = await fetch(url, options);

        // Si es 401, intentar regenerar el token
        if (response.status === 401) {
            console.warn('Token expirado, intentando renovar...');
            //alert('Token expirado, intentando renovar...');

            var data = await refreshToken();
             
            if (data == null) throw new Error('No se pudo renovar el token');

            //const expiracionEnDias = 7;
            //const fechaExpiracionRT = new Date();
            //fechaExpiracionRT.setDate(fechaExpiracionRT.getDate() + expiracionEnDias);

            setCookie("app-access-token", data.access_token, data.expires_at);
            //setCookie("app-refresh-token", data.refresh_token, fechaExpiracionRT);
            setCookie("app-role", data.role, data.expires_at);

            // Reintentamos la petición original con el nuevo token
            options.headers['Authorization'] = `Bearer ${data.access_token}`;
            response = await fetch(url, options);
        }

        // Si sigue siendo error, lanzamos excepción para capturarlo fuera
        if (!response.ok) {
            console.error("Sigue siendo error, lanzamos excepción para capturarlo fuera");
            const errorText = await response.text();
            throw new Error(errorText || response.statusText);
        }

        // Intentamos parsear JSON
        const contentType = response.headers.get('content-type');
        if (contentType && contentType.includes('application/json')) {
            //alert("response es json");
            return response;  
        }

        // Si no es JSON, devolvemos el texto
        return response.text();

    } catch (err) {
        //alert("Error en fetchWithAuth");
        console.error('Error en fetchWithAuth:', err);
        throw err; 
    }
}

async function refreshToken() {

    try {
        const baseUrl = `https://localhost`;
        const port = `44311`;
        const controllerName = `auth`;
        const apiUrl = `${baseUrl}:${port}/${controllerName}`;

        const apiMethod = `refreshToken`;
        const response = await fetch(`${apiUrl}/${apiMethod}`,
        {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify( {
                RefreshToken: getCookie("app-refresh-token")
            }) 
        });

        if (!response.ok) return null;

        const data = await response.json();

        return data;
    } catch (err) {
        console.error("error en refreshToken()");
        return null;
    }
}

// Función para establecer una cookie
function setCookie(nombre, valor, expiracionEnDias) {

    //const fechaExpiracion = new Date();

    //if (expiracionEnDias == 0) {
    //    fechaExpiracion.setMinutes(fechaExpiracion.getMinutes() + 5);
    //}
    //else {
    //    fechaExpiracion.setDate(fechaExpiracion.getDate() + expiracionEnDias);
    //}

    // Convertir a objeto Date de JavaScript
    const date = new Date(expiracionEnDias);

    // Convertir a formato GMT para cookies
    const expires = date;// date.toUTCString();

    //const cookieValor = encodeURIComponent(valor) + "; expires=" + fechaExpiracion.toUTCString() + "; path=/";
    const cookieValor = encodeURIComponent(valor) + "; expires=" + expires; + "; path=/";

    document.cookie = nombre + "=" + cookieValor;
}

function getCookie(nombre) {
    const nombreEQ = nombre + "=";
    const cookies = document.cookie.split(';');
    for (let i = 0; i < cookies.length; i++) {
        let cookie = cookies[i];
        while (cookie.charAt(0) === ' ') {
            cookie = cookie.substring(1, cookie.length);
        }
        if (cookie.indexOf(nombreEQ) === 0) {
            return decodeURIComponent(cookie.substring(nombreEQ.length, cookie.length));
        }
    }
    return null;
}
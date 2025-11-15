
// Encriptar
export async function calcularHash(cadena) {
     
    // Convierte la cadena en un array de bytes (codificación UTF-8)
    const buffer = new TextEncoder().encode(cadena);

    // Calcula el hash utilizando el algoritmo SHA-256
    const hashBuffer = await crypto.subtle.digest('SHA-256', buffer);

    // Convierte el resultado del hash a una cadena hexadecimal
    const hashArray = Array.from(new Uint8Array(hashBuffer));
    const hashHex = hashArray.map(byte => byte.toString(16).padStart(2, '0')).join('');

    return hashHex;
}

//
// COOKIES
//

// Función para establecer una cookie
export function setCookie(nombre, valor, expiracionEnDias) {

    alert("setCookie");

    const fechaExpiracion = new Date();
    fechaExpiracion.setDate(fechaExpiracion.getDate() + expiracionEnDias);
    const cookieValor = encodeURIComponent(valor) + "; expires=" + fechaExpiracion.toUTCString() + "; path=/";

    document.cookie = nombre + "=" + cookieValor;
}

// Función para obtener el valor de una cookie
export function getCookie(nombre) {
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

// Función para cargar contenido desde un archivo
export async function cargarContenidoHtml(url, contenedorId) {
    const response = await fetch(url);
    const contenido = await response.text();
    document.getElementById(contenedorId).innerHTML = contenido;
}

//
export function getInputValueByName(name) {
    const input = document.querySelector('input[name="' + name + '"]');
    return input.value;
}

export function getCheckboxValueByName(name) {
    const input = document.querySelector('input[name="' + name + '"]');
    return input.checked;
}

//
export function setInputValueByName(name, value) {
    const input = document.querySelector('input[name="' + name + '"]');
    input.value = value;
}

export function setInputHtmlByName(name, value) {
    const input = document.getElementsByName(name);
    input[0].innerHTML = value;
}

export function setBackgroundImageByName(name, value) {
    const input = document.getElementsByName(name);
    input[0].style.backgroundImage = "url(img/entidades/" + value + ")";
}

// para navegación a una página
export function redirectToPage(page) {
    window.location.assign(page);
}

// para recargar una página
export function refreshPage() {
    window.location.href = getCurrentPage();
}

//
export function getCurrentPage() {
    var rutaActual = window.location.pathname;
    return rutaActual.substring(rutaActual.lastIndexOf('/') + 1);
}

//
export function getCurrentDateTime() {
    var fechaHoraActual = new Date();

    var año = fechaHoraActual.getFullYear();
    var mes = fechaHoraActual.getMonth() + 1;
    var dia = fechaHoraActual.getDate();
    var horas = fechaHoraActual.getHours();
    var minutos = fechaHoraActual.getMinutes();
    var segundos = fechaHoraActual.getSeconds();

    var fechaFormateada = año + '-' + mes + '-' + dia;
    var horaFormateada = horas + ':' + minutos + ':' + segundos;

    //console.log('Fecha actual:', fechaFormateada);
    //console.log('Hora actual:', horaFormateada);

    return fechaFormateada + ' ' + horaFormateada;
}

//
export class Filtros {
    constructor() {
        this.elementos = [];  
    }
    agregarElemento(clave, valor) { 
        this.elementos.push([clave, valor]);
    }
    obtenerValor(clave) {
        const elementoEncontrado = this.elementos.find(elemento => elemento[0] === clave);
        return elementoEncontrado ? elementoEncontrado[1] : undefined;
    }
    imprimirElementos() {
        this.elementos.forEach(elemento => {
            console.log(`Clave: ${elemento[0]}, Valor: ${elemento[1]}`);
        });
    }
};

//
export function isExpiredDate(date) {
    const fecha = new Date(date);
    const fechaActual = new Date();
    return fecha < fechaActual;
}

//
export function replaceTextByTagId(tagId, tagText, newText) {
    var element = document.getElementById(tagId);
    element.textContent = element.textContent.replace(tagText, newText);
} 
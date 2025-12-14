//
// Constantes
//

//TODO: refactorizar las variables globales para la URL

let baseUrl4 = `https://localhost`;
let controllerName4 = `Productos`;
let port4 = `44311`;
let apiUrl4 = `${baseUrl4}:${port4}/${controllerName4}/`;
 
class Producto {
    constructor(id, idEntidad, nombre, imagen, descripcion, puntos, precio, activo, descuento, popularidad, descripcionCorta, disponible, informacioExtra, linkInstagram, linkFacebook, linkYoutube) {
        this.id = id,
        this.idEntidad = idEntidad,
        this.nombre = nombre,
        this.imagen = imagen,
        this.descripcion = descripcion,
        this.puntos = puntos,
        this.precio = precio,
        this.activo = activo,
        this.descuento = descuento,
        this.popularidad = popularidad,
        this.descripcionCorta = descripcionCorta,
        this.disponible = disponible,
        this.informacioExtra = informacioExtra,
        this.linkInstagram = linkInstagram
        this.linkFacebook = linkFacebook,
        this.linkYoutube = linkYoutube
    }
    metodo() {
        //console.log(`Producto ${this.nombre} con ${this.puntos} puntos.`);
    }
} 

//
// Proxies con los web services del controlador 
//
function GetProducto(id) {

    let nameMethod = "ObtenerProducto";
    let requestOptions = {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        }
    };

    let urlConParametro = `${apiUrl4}${nameMethod}/${id}`; 

    return fetch(urlConParametro, requestOptions)
        .then(response => {
            if (!response.ok) {
                throw new Error(`Error en la solicitud: ${response.status}`);
            }
            return response.json();
        })
        .then(entidad => {
            return entidad;
        })
        .catch(error => {
            console.error('Error al recuperar datos:', error.message);
        }); 
}

function GetProductos() {

    let requestOptions = {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        }
    };

    let url = `${apiUrl4}ObtenerProductos`;
    return fetch(url, requestOptions)
        .then(response => {
            if (!response.ok) {
                throw new Error(`Error en la solicitud: ${response.status}`);
            }
            return response.json();
        })
        .then(data => {
            return data;
        })
        .catch(error => {
            console.error('Error al recuperar datos:', error.message);
            throw error;
        });
}

function GetProductoByFilters(nameMethod, pFiltros) {

    let requestOptions = {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        }
    };

    let urlConFiltros = `${apiUrl4}${nameMethod}`;
    let isFirstParam = true;
    pFiltros.elementos.forEach(filtro => {
        if (isFirstParam) urlConFiltros = `${urlConFiltros}?`;
        else urlConFiltros = `${urlConFiltros}&`;
        urlConFiltros = `${urlConFiltros}${filtro[0]}=${encodeURIComponent(filtro[1])}`;
        isFirstParam = false;
    });

    return fetch(urlConFiltros, requestOptions)
        .then(response => {
            if (!response.ok) {
                throw new Error(`Error en la solicitud: ${response.status}`);
            }
            return response.json();
        })
        .then(data => {
            return data;
        })
        .catch(error => {
            console.error('Error al recuperar datos:', error.message);
        });
}

function PatchProductoByFilters(nameMethod, pFiltros) {

    let requestOptions = {
        method: 'PATCH',
        headers: {
            'Content-Type': 'application/json'
        }
    }; 

    let urlConFiltros = `${apiUrl4}${nameMethod}`;

    alert(urlConFiltros);

    let isFirstParam = true;
    pFiltros.elementos.forEach(filtro => {
        if (isFirstParam) urlConFiltros = `${urlConFiltros}?`; 
        else urlConFiltros = `${urlConFiltros}&`; 
        urlConFiltros = `${urlConFiltros}${filtro[0]}=${encodeURIComponent(filtro[1])}`;
        isFirstParam = false;
    });

    alert(urlConFiltros);

    return fetch(urlConFiltros, requestOptions)
        .then(response => {
            if (!response.ok) {
                alert('ko: ' + response.status);
                throw new Error(`Error en la solicitud: ${response.status}`);
            }
            alert('ok');
            return response.json();
        })
        .then(data => {
            alert('data ok');
            return data;
        })
        .catch(error => {
            //console.error('Error al recuperar datos:', error.message);
            alert('catch error');
            //throw error;
            return error;
        }); 
}

function DeleteProducto(id) {

    let urlConParametro = `${apiUrl4}Eliminar/${id}`;
    return fetch(urlConParametro, {
        method: 'DELETE',
        headers: {
            'Content-Type': 'application/json'
        }
    })
    .then(response => {
        if (!response.ok) {
            throw new Error(`Error en la solicitud: ${response.status}`);
        }
        return response.json();
    })
    .catch(error => {
        console.error('Error al realizar la solicitud DELETE:', error.message);
    });
}
 
function PostProducto(nameMethod, entidad) {
  
    let url = `${apiUrl4}${nameMethod}`;
    return fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(entidad)
        })
        .then(response => {
            if (!response.ok) {
                throw new Error(`Error en la solicitud: ${response.status}`);
            } 
            return response.json();
        })
        .then(data => { 
            return data;
        })
        .catch(error => { 
            console.error('Error al realizar la solicitud POST:', error.message);
        });
}

function PutProducto(nameMethod, entidad) {

    let url = `${apiUrl4}${nameMethod}`;
    return fetch(url, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(entidad)
        })
        .then(response => {
            if (!response.ok) {
                throw new Error(`Error en la solicitud: ${response.status}`);
            } 
            return response.json();
        })
        .then(data => { 
            return data;
        })
        .catch(error => {
            console.error('Error al realizar la solicitud PUT:', error.message);
        });
} 
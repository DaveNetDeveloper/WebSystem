//
// Constantes
//
const baseUrl2 = `https://localhost`;
const controllerName2 = `Transacciones`;
const port2 = `7161`;
const apiUrl2 = `${baseUrl2}:${port2}/${controllerName2}/`;

//var userToken = "qAGlDm9o9oS1Ir+xNlWk3XXHkJy/+nJmBy3KUPoms2w="; // TODO: cambiar por valor de la cookie
 
//
// Clases
//
class Transaccion {

    constructor(id, nombre, idUsuario, fecha, puntos, idProducto) {
        this.id = id;
        this.nombre = nombre;
        this.idUsuario = idUsuario;
        this.fecha = fecha,
            this.puntos = puntos,
            this.idProducto = idProducto
    }
    metodo() {
        //console.log(`Transacción ${this.nombre} con ${this.puntos} puntos.`);
    }
} 

//
// Proxies con los web services del controlador 
//
function GetTransaccion(id) {

    var nameMethod = "ObtenerTransaccion";
 
    const requestOptions = {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${userToken}` 
        }, 
    };

    const urlConParametro = `${apiUrl}${nameMethod}/${id}`; 

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

function GetTransacciones() {
    const requestOptions = {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${userToken}`
        },
    };
    const url = `${apiUrl}ObtenerTransacciones`;
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

function GetTransaccionByFilters(nameMethod, pFiltros) {

    const requestOptions = {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${userToken}`
        },
    };

    var urlConFiltros = `${apiUrl}${nameMethod}`;
    var isFirstParam = true;
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

function PatchTransaccionByFilters(nameMethod, pFiltros) {

    const requestOptions = {
        method: 'PATCH',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer qAGlDm9o9oS1Ir+xNlWk3XXHkJy/+nJmBy3KUPoms2w=`
        },
    }; 
    var urlConFiltros = `${apiUrl}${nameMethod}`;
    var isFirstParam = true;
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
                //throw error;
                return error;
            }); 
}

function DeleteTransaccion(id) {

    var urlConParametro = `${apiUrl}Eliminar/${id}`;
    return fetch(urlConParametro, {
        method: 'DELETE',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${userToken}`
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
 
function PostTransaccion(nameMethod, entidad) {
  
    var url2 = `${apiUrl2}${nameMethod}`;
    return fetch(url2, {
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

function PutTransaccion(nameMethod, entidad) {

    var url = `${apiUrl}${nameMethod}`;
    return fetch(url, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${userToken}`
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
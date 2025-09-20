//
// Constantes
//

//TODO: refactorizar las variables globales para la URL

const baseUrl3 = `https://localhost`;
const controllerName3 = `QRs`;
const port3 = `7161`;
const apiUrl3 = `${baseUrl3}:${port3}/${controllerName3}/`;

//var userToken = "qAGlDm9o9oS1Ir+xNlWk3XXHkJy/+nJmBy3KUPoms2w="; // TODO: cambiar por valor de la cookie
 
class QR {
    constructor(id, idProducto, activo, multicliente, qrCode, consumido, fechaExpiracion) {
        this.id = id,
        this.idProducto = idProducto,
        this.activo = activo,
        this.multicliente = multicliente,
        this.qrCode = qrCode,
        this.consumido = consumido,
        this.fechaExpiracion = fechaExpiracion
    }
    metodo() {
        //console.log(`Transacción ${this.qrCode} para el producto ${this.idProducto}.`);
    }
} 

//
// Proxies con los web services del controlador 
//
function GetQR(id) {

    let nameMethod = "ObtenerQR";
    let requestOptions = {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        }
    };

    let urlConParametro = `${apiUrl3}${nameMethod}/${id}`; 

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

function GetQRs() {

    let requestOptions = {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        }
    };

    let url = `${apiUrl3}ObtenerQRs`;
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

function GetQRByFilters(nameMethod, pFiltros) {

    let requestOptions = {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        }
    };

    let urlConFiltros = `${apiUrl3}${nameMethod}`;
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

function PatchQRByFilters(nameMethod, pFiltros) {

    let requestOptions = {
        method: 'PATCH',
        headers: {
            'Content-Type': 'application/json'
        }
    }; 

    let urlConFiltros = `${apiUrl3}${nameMethod}`;

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

function DeleteQR(id) {

    let urlConParametro = `${apiUrl3}Eliminar/${id}`;
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
 
function PostQR(nameMethod, entidad) {
  
    let url = `${apiUrl3}${nameMethod}`;
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

function PutQR(nameMethod, entidad) {

    let url = `${apiUrl3}${nameMethod}`;
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
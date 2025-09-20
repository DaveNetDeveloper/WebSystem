//
// Constantes
//

//TODO: refactorizar las variables globales para la URL

let baseUrl5 = `https://localhost`;
let controllerName5 = `Entidades`;
let port5 = `7161`;
let apiUrl5 = `${baseUrl5}:${port5}/${controllerName5}/`;

//var userToken = "qAGlDm9o9oS1Ir+xNlWk3XXHkJy/+nJmBy3KUPoms2w="; 
 
class Entidad { 

    constructor(id, idTipoEntidad, nombre, descripcion, popularidad, activo, ubicacion, fechaAlta, imagen) {
        this.id = id, 
        this.nombre = nombre,
        this.ubicacion = ubicacion,
        this.descripcion = descripcion,
        this.fechaAlta = fechaAlta, 
        this.popularidad = popularidad, 
        this.activo = activo,
        this.idTipoEntidad = idTipoEntidad,
        this.imagen = imagen
       
    }
    metodo() {
        //console.log(`Producto ${this.nombre} con ${this.puntos} puntos.`);
    }
} 

//
// Proxies con los web services del controlador 
//
function GetEntidad(id) {

    let nameMethod = "ObtenerEntidad";
    let requestOptions = {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        }
    };

    let urlConParametro = `${apiUrl5}${nameMethod}/${id}`; 

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

function GetEntidades() {

    let requestOptions = {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        }
    };

    let url = `${apiUrl5}ObtenerEntidades`;
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

function GetEntidadByFilters(nameMethod, pFiltros) {

    let requestOptions = {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        }
    };

    let urlConFiltros = `${apiUrl5}${nameMethod}`;
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

function PatchEntidadesByFilters(nameMethod, pFiltros) {

    let requestOptions = {
        method: 'PATCH',
        headers: {
            'Content-Type': 'application/json'
        }
    }; 

    let urlConFiltros = `${apiUrl5}${nameMethod}`;

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

function DeleteEntidad(id) {

    let urlConParametro = `${apiUrl5}Eliminar/${id}`;
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
 
function PostEntidad(nameMethod, entidad) {
  
    let url = `${apiUrl5}${nameMethod}`;
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

function PutEntidad(nameMethod, entidad) {

    let url = `${apiUrl5}${nameMethod}`;
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
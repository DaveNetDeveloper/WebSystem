//
// Constantes
//
const baseUrl = `https://localhost`;
const controllerName = `Usuarios`;
const port = `7161`;
const apiUrl = `${baseUrl}:${port}/${controllerName}/`;

var userToken = "qAGlDm9o9oS1Ir+xNlWk3XXHkJy/+nJmBy3KUPoms2w=";


const controllerActions = [
    'ObtenerUsuarioById',
    'CrearUsuario',
    'ObtenerUsuarioByEmail',
    'ObtenerUsuarios',
    'ActualizarUsuario',
    'Login',
    'CambiarContraseña',
    'ValidarCuenta',
    'ActivacionSuscripcion',
    'Eliminar'
];
 
//
// Clases
//
class Usuario {

    constructor(id, nombre, apellidos, correo, activo, contraseña, fechaNacimiento, suscrito, fechaCreacion, puntos) {
        this.id = id;
        this.nombre = nombre;
        this.apellidos = apellidos;
        this.correo = correo, 
        this.activo = activo,
        this.contraseña = contraseña,
        this.fechaNacimiento = fechaNacimiento,
        this.suscrito = suscrito,
        this.fechaCreacion = fechaCreacion,
        this.ultimaConexion = null,
        this.puntos = puntos,
        this.token = null,
        this.expiracionToken = null
    }
    metodo() {
        //console.log(`Hola, soy ${this.nombre} y tengo ${this.puntos} puntos.`);
    }
}
 
//
// Proxis con los web services del controlador 
//
function Get(id) {

    var nameMethod = "ObtenerUsuario";
 
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
            const listaUsuarios = document.getElementById('lista-usuarios');
            const listItem = document.createElement('li');
            listItem.textContent = `${entidad.id}: ${entidad.nombre}`;
            listaUsuarios.appendChild(listItem);

            return entidad;
        })
        .catch(error => {
            console.error('Error al recuperar datos:', error.message);
        }); 
}

function GetAll() {
    const requestOptions = {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${userToken}`
        },
    };
    const url = `${apiUrl}ObtenerUsuarios`;
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

function GetByFilters(nameMethod, pFiltros) {

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

function PatchByFilters(nameMethod, pFiltros) {

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

function Delete(id) {

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
 
function Post(nameMethod, entidad) {
  
    var url = `${apiUrl}${nameMethod}`;
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

function Put(nameMethod, entidad) {

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

function CloseSession() {

    // borrar la cookie de sesión de usuario

    // redirigir a la Home Pública

}
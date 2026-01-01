

document.addEventListener("DOMContentLoaded", function () {
    const rol = getCookie("app-role");
    LoadMenuOptions(rol);
});

function getCookie(name) {
    const value = `; ${document.cookie}`;
    const parts = value.split(`; ${name}=`);
    if (parts.length === 2) return parts.pop().split(";").shift();
}

async function LoadMenuOptions(rol) {
    try {
        alert(window.location.hostname);

        const baseUrl = window.location.hostname === "localhost"
            ? "https://localhost"
            : "https://websystem-api-prod-ctgrd4gfc2e6dudb.westeurope-01.azurewebsites.net";

        const port = window.location.hostname === "localhost"
            ? "44311"
            : "";
        const controllerName = `Permisos`;

        const apiUrl = `${baseUrl}:${port}/${controllerName}`; 
        const apiMethod = `ObtenerOpcionesMenu`;

        const response = await fetch(`${apiUrl}/${apiMethod}?rol=${rol}`, {
            method: "GET",
            headers: {
                "Authorization": "Bearer " + getCookie("app-access-token")
            }
        });

        if (!response.ok) {
            console.error("Error al obtener las opciones de menu");
            return;
        }

        const opciones = await response.json();
        mostrarMenu(opciones);

    } catch (error) {
        console.error("Error en la llamada:", error);
    }
}

function mostrarMenu(opciones) {
    const menuContainer = document.getElementById("menu");
    menuContainer.innerHTML = "";

    opciones.forEach(op => {
        const li = document.createElement("li");
        li.textContent = `${op.nivel}. ${op.nombre}`;
        li.onclick = () => window.location.href = op.path;
        menuContainer.appendChild(li);
    });
}
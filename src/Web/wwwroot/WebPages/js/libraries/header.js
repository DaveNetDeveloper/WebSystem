 
async function renderHeader(paginaActiva, headerId) {

    const header = document.getElementById(headerId);
    if (!header) return;

    header.innerHTML = `
        <div class="header-inner"> 
            <div class="container">
                <div class="inner">
                    <div class="row">
                        <div class="col-lg-3 col-md-3 col-12">
                            <div class="logo">
                                <a href="home-privada.html"><img src="img/logo.jpg" alt="#"></a>
                            </div>
                            <div class="mobile-nav"></div>
                        </div>
                        <div class="col-lg-7 col-md-9 col-12">
                            <div class="main-menu">
                                <nav class="navigation">
                                    <ul class="nav menu">
                                        <li class="${paginaActiva === 'home' ? 'active' : ''}">
                                            <a href="home-privada.html">Inicio</a>
                                        </li>
                                        <li class="${paginaActiva === 'explorar' ? 'active' : ''}">
                                            <a href="explorar.html">Explorar</a>
                                        </li>
                                        <li class="${paginaActiva === 'recompensas' ? 'active' : ''}">
                                            <a href="#" style="cursor: not-allowed;">Recompensas</a>
                                        </li>
                                        <li class="${paginaActiva === 'como-ganar' ? 'active' : ''}">
                                            <a href="#" style="cursor: not-allowed;">¿Cómo ganar puntos?</a>
                                        </li>
                                    </ul>
                                </nav>
                            </div>
                        </div>
                        <div class="col-lg-1 col-12">
                            <div class="get-quote">
                                <a id="infoUsuario" style="background: goldenrod;     margin-left: -135%; padding-top: 15px;" href="#" class="btn">
                                    Hola {nombre}. Tienes {puntos} pts.
                                </a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>`;
 }
 
async function setUserData() {

    //localStorage.setItem("IdUsuario", 15);
    const idUsuario = localStorage.getItem("idUsuario");

    const baseUrl = window.location.hostname === "localhost"
        ? "https://localhost"
        : "https://websystem-api-prod-ctgrd4gfc2e6dudb.westeurope-01.azurewebsites.net";

    const port = window.location.hostname === "localhost"
        ? "44311"
        : "";
    const controllerName = `Usuarios`;
    //const port = `44311`;
    const apiUrl = `${baseUrl}:${port}/${controllerName}/FiltrarUsuarios`;

    const url = `${apiUrl}?filters.Id=${idUsuario}&filters.Activo=true`;

    const response = await fetch(url, {
        cache: 'no-store',
        method: "GET",
        //redentials: "include",
        headers: { "Content-Type": "application/json" }
    });

    if (!response.ok) {
        console.error("Error al obtener el usuario", response.status);
    }

    const data = await response.json(); 
    if (data != null && data.length > 0 && data[0] != null) {

        localStorage.setItem("suscripcion", data[0].suscrito); 

        var infoUsuarioElement = document.getElementById("infoUsuario");
        infoUsuarioElement.innerHTML = infoUsuarioElement.innerHTML.replace("{nombre}", data[0].nombre).replace("{puntos}", data[0].puntos);
          
    }
}
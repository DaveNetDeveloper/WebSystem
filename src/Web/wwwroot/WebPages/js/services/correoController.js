
//
// Proxis con los web services del controlador
//
function enviarCorreo(email, tipoEnvio) {

    const baseUrl = window.location.hostname === "localhost"
        ? "https://localhost"
        : "https://websystem-api-prod-ctgrd4gfc2e6dudb.westeurope-01.azurewebsites.net";

    const port = window.location.hostname === "localhost"
        ? "44311"
        : "";
    const controllerName = `Correo`;
    const action = "Enviar";
    //const port = `7161`;
    const url = `${baseUrl}:${port}/${controllerName}/${action}`;
     
    const data = {
        Destinatario: email,
        TipoEnvio: tipoEnvio,
        Asunto: "",
        Cuerpo: ""
    };

    const requestOptions = {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data),
    };

    fetch(url, requestOptions)
        .then(response => response)
        .then(data => {
            console.log(data);
        })
        .catch(error => {
            console.error('Error al llamar al servicio web ' + controllerName + '/' + action + ': ', error);
            throw error;
        });
}
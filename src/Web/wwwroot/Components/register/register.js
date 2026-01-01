import * as UserController from '../../WebPages/js/services/usuariosController.js';
//import * as Utilities from '../../WebPages/js/libraries/utilities.js';

const css = await fetch('../Components/register/register.css').then(r => r.text());
const html = await fetch('../Components/register/register.html').then(r => r.text());

// Properties
const subscribe_PropName = "subscribe";
class AppRegister extends HTMLElement {

    constructor() {
        super();
        this.attachShadow({ mode: 'open' });  // Activamos Shadow DOM 

        this.shadowRoot.innerHTML = `

	        <link href="https://fonts.googleapis.com/css?family=Poppins:200i,300,300i,400,400i,500,500i,600,600i,700,700i,800,800i,900,900i&display=swap" rel="stylesheet">
	        <link rel="stylesheet" href="../WebPages/css/bootstrap.min.css">
	        <link rel="stylesheet" href="../WebPages/css/nice-select.css">
	        <link rel="stylesheet" href="../WebPages/css/font-awesome.min.css">
	        <link rel="stylesheet" href="../WebPages/css/icofont.css">
	        <link rel="stylesheet" href="../WebPages/css/slicknav.min.css">
	        <link rel="stylesheet" href="../WebPages/css/owl-carousel.css">
	        <link rel="stylesheet" href="../WebPages/css/datepicker.css">
	        <link rel="stylesheet" href="../WebPages/css/animate.min.css"> 

            <link rel="stylesheet" href="../WebPages/css/normalize.css">
    	    <link rel="stylesheet" href="../WebPages/css/style.css">
	        <link rel="stylesheet" href="../WebPages/css/responsive.css">

            <style>${css}</style>
            
            ${html} 
        `;
    }

    static get observedAttributes() {
        return [subscribe_PropName];
    }

    attributeChangedCallback(name, oldValue, newValue) {
        if (name === subscribe_PropName) {
            this.subscribe = newValue;
        }
    }

    connectedCallback() {

        /*Validaciones de los campos del formulario*/
        function validateUser(newUser) {

            return true;

            if (!validateName(newUser.nombre)) return false;
            if (!validateSurname(newUser.apellidos)) return false;
            if (!validateEmail(newUser.email)) return false;
            if (!validatePassword(newUser.contrasena)) return false;
            if (!validateDate(newUser.fechaNacimiento)) return false;
            if (!validateGender(newUser.genero)) return false;

            return true;
        }

        function isTextProvided(value) {
            return typeof value === 'string' && value.trim().length > 0;
        }

        function validateGender(genero) {
            return isTextProvided(genero);
        }

        function validatePassword(password) {
            return isTextProvided(password);
        }

        function validateDate(date) {
            return true;
            //return isTextProvided(date);
            //return typeof date !== 'string';
        }

        function validateEmail(email) {
            return isTextProvided(email);
        }

        function validateName(name) {
            return isTextProvided(name);
        }

        function validateSurname(surname) {
            return isTextProvided(surname);
        }

        function validateTermsAndConditions(terms) {
            return terms == true;
        }
        
        const savedKeepSubscribed = localStorage.getItem("keepSubscribed");
        if (savedKeepSubscribed !== null) {
            this.shadowRoot.querySelector("#subscribe").checked = savedKeepSubscribed === "true";
        }
         
        this.subscribe = this.shadowRoot.querySelector('#subscribe').checked;

        const script = document.createElement('script');
        script.src = '../WebPages/js/main.js'; 
        document.head.appendChild(script); 

        const form = this.shadowRoot.querySelector('#registerForm');
        const errorMsg = this.shadowRoot.querySelector('#errorMsg');

        //
        form.addEventListener('submit', async (e) => {
            e.preventDefault();

            const _terminos = this.shadowRoot.querySelector("#terminos").checked; 

            if (!validateTermsAndConditions(_terminos)) { alert("Es obligatorio aceptar los Términos y Condiciones."); }

                const _nombre = this.shadowRoot.querySelector('#name').value.trim();
                const _apellidos = this.shadowRoot.querySelector('#surname').value.trim();
                const _correo = this.shadowRoot.querySelector('#email').value.trim();
                const _contrasena = this.shadowRoot.querySelector('#password').value.trim();
                const _fechaNacimiento = this.shadowRoot.querySelector('#date').value.trim();
                const _suscrito = this.shadowRoot.querySelector("#subscribe").checked; 

                const select = this.shadowRoot.querySelector('#genero');
                //const generoId = select.value;
                const _genero = select.options[select.selectedIndex].text;

                const _codigoRecomendacion = this.shadowRoot.querySelector("#codigoRecomendacion").value.trim();
                
                // Usuario tipado
                var newUser = new UserController.Usuario(
                    null,
                    _nombre,
                    _apellidos,
                    _correo,
                    false,
                    _contrasena,
                    _fechaNacimiento,
                    _suscrito,
                    new Date().toISOString(), 
                    _genero,
                    null);

                const _idPerfil = 'c930d0e1-143d-4cd1-9881-f8355a505158';

                //newUser.codigoRecomendacionRef = _codigoRecomendacion;

                if (!validateUser(newUser)) { alert("Errores de validación en los datos introducidos."); }

                // Proceso de registro
                try { 
                    const usuarioDTO = {
                        nombre: _nombre,
                        apellidos: _apellidos,
                        correo: _correo,
                        contrasena: _contrasena,
                        activo: false,
                        fechaNacimiento: new Date(_fechaNacimiento).toISOString(),
                        suscrito: _suscrito, 
                        fechaCreacion: new Date().toISOString(),
                        genero: _genero,
                        puntos: 0,
                        codigoRecomendacionRef: _codigoRecomendacion,
                        idPerfil: _idPerfil
                    };

                    const baseUrl = window.location.hostname === "localhost"
                        ? "https://localhost"
                        : "https://websystem-api-prod-ctgrd4gfc2e6dudb.westeurope-01.azurewebsites.net";

                    const port = window.location.hostname === "localhost"
                        ? "44311"
                        : "";
                    const controllerName = `auth`;
                    const apiUrl = `${baseUrl}:${port}/${controllerName}`;

                    const apiMethod = `register`;

                    const res = await fetch(`${apiUrl}/${apiMethod}`,
                    {
                        method: 'POST',
                        headers: {
                            'Content-Type': 'application/json'
                        },
                        body: JSON.stringify(usuarioDTO)
                    });

                    if (!res.ok) throw new Error('Datos incorrectos');

                    const data = await res.json();
                    console.info("result: " + data);

                    if (data == true) {
                        this.dispatchEvent(new CustomEvent('register-success', { detail: _correo }));
                        errorMsg.textContent = '';
                    }
                }
                catch (err) {
                    errorMsg.textContent = err.message || 'Error durante el proceso de registro';
                }
        });

        const checkbox = this.shadowRoot.querySelector("#subscribe"); 
        checkbox.addEventListener("change", () => {
            localStorage.setItem("keepSubscribed", checkbox.checked);
        });

        //var linkTerminos = this.shadowRoot.querySelector('#terminos');
        //linkTerminos.addEventListener('click', function (event) {
        //    event.preventDefault();

        //    var urlPDF = '../Resources/documents/Terminos y Condiciones.pdf';
        //    window.open(urlPDF, '_blank'); 
        //});

    }
}

// Registrar el nuevo elemento personalizado
customElements.define('app-register', AppRegister);

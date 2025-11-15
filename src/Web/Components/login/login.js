//import * as UserController from './js/services/usuariosController.js';

import * as Utilities from '../../WebPages/js/libraries/utilities.js';

const css = await fetch('../Components/login/login.css').then(r => r.text());
const html = await fetch('../Components/login/login.html').then(r => r.text());

// Properties
const redirectURL_PropName = "redirect-url";
const session_PropName = "keep-session";

class AppLogin extends HTMLElement {

    constructor() {
        super();
        this.attachShadow({ mode: 'open' }); // Activamos Shadow DOM 

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
        return [redirectURL_PropName];
    }

    attributeChangedCallback(name, oldValue, newValue) {
        if (name === redirectURL_PropName) {
            this.redirectUrl = newValue; 
        }
        else if (name === session_PropName) {
            this.keepSession = newValue;
        }
    }

    connectedCallback() {

        const saved = localStorage.getItem("keepSession");
        alert(saved);
        if (saved !== null) {
            this.shadowRoot.querySelector("#session").checked = saved === "true";
            //alert(saved);
        }

        //alert(this.redirectUrl);
        this.keepSession = this.shadowRoot.querySelector('#session').checked;
        //alert(this.keepSession);

        // Cargar el script dinámicamente
        const script = document.createElement('script');
        script.src = '../WebPages/js/main.js';
        //script.onload = () => { };
        document.head.appendChild(script); 

        const form = this.shadowRoot.querySelector('#loginForm');
        const errorMsg = this.shadowRoot.querySelector('#errorMsg');

        form.addEventListener('submit', async (e) => {
            e.preventDefault();

            const username = this.shadowRoot.querySelector('#username').value.trim();
            const password = this.shadowRoot.querySelector('#password').value.trim();

            try { 
                //var hashedPassword = Utilities.calcularHash(password);

                const baseUrl = `https://localhost`;
                const port = `44311`;
                const controllerName = `auth`;
                const apiUrl = `${baseUrl}:${port}/${controllerName}`;

                const apiMethod = `login`;
                const res = await fetch(`${apiUrl}/${apiMethod}`,
                {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json' 
                    },
                    body: JSON.stringify({
                        UserName: username,
                        Password: password
                    })
                });

                if (!res.ok) throw new Error('Credenciales incorrectas');

                const data = await res.json();

                alert("access_token: " + data.access_token);
                alert("token_type: " + data.token_type);
                alert("expires_at_utc: " + data.expires_at_utc);
                alert("role: " + data.role);

                const expiracionEnDias = 30;
                Utilities.setCookie("app-token", data.access_token, expiracionEnDias);
                 
                // Se emite evento personalizado para que el resto de la app reaccione
                this.dispatchEvent(new CustomEvent('login-success', { detail: data }));

                errorMsg.textContent = '';

            } catch (err) {
                errorMsg.textContent = err.message || 'Error durante el proceso de Login';
            }
        });


        const checkbox = this.shadowRoot.querySelector("#session"); 
        checkbox.addEventListener("change", () => {
            localStorage.setItem("keepSession", checkbox.checked);
            //alert(checkbox.checked.toString());
        });

        var linkPassword = this.shadowRoot.querySelector('#linkChangePassword');
        linkPassword.addEventListener('click', function (event) {

            event.preventDefault();
            var url = '../WebPages/changePassword.html?email='; 
            window.location.href = url;
        });

    }
}

// Registrar el nuevo elemento personalizado
customElements.define('app-login', AppLogin);

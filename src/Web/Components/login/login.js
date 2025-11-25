import * as Utilities from '../../WebPages/js/libraries/utilities.js';

const css = await fetch('../Components/login/login.css').then(r => r.text());
const html = await fetch('../Components/login/login.html').then(r => r.text());

// Component properties
const loginType_PropName = "login-type";
const session_PropName = "keep-session";
class AppLogin extends HTMLElement {

    constructor() {
        super();
        this.attachShadow({ mode: 'open' }); // Activamos Shadow DOM
         
        //console.log(html);

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
        return [loginType_PropName];
    }

    attributeChangedCallback(name, oldValue, newValue) {
        if (name === loginType_PropName) {
            this.loginType = newValue; 
        }
        else if (name === session_PropName) {
            this.keepSession = newValue;
        }
    }

    connectedCallback() {
       
        const saved = localStorage.getItem("keepSession");
        //alert(saved);
        if (saved !== null) {
            this.shadowRoot.querySelector("#session").checked = saved === "true";
            //alert(saved);
        }

        //alert("loginType: " + this.loginType);
        this.keepSession = this.shadowRoot.querySelector('#session').checked;
        //alert(this.keepSession);

        // Cargar el script dinámicamente
        const script = document.createElement('script');
        script.src = '../WebPages/js/main.js';
        document.head.appendChild(script); 

        const form = this.shadowRoot.querySelector('#loginForm');
        const errorMsg = this.shadowRoot.querySelector('#errorMsg');

        form.addEventListener('submit', async (e) => {
            e.preventDefault();

            const email = this.shadowRoot.querySelector('#email').value.trim();
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
                        Email: email,
                        Password: password,
                        LoginType: this.loginType,
                    })
                });

                if (!res.ok) throw new Error('Credenciales incorrectas');

                const data = await res.json();

                alert("access_token: " + data.access_token);
                alert("token_type: " + data.token_type);
                alert("expires_at: " + data.expires_at);
                alert("role: " + data.role);
                alert("profile: " + data.profile);

                const expiracionEnDias = 7;
                const fechaExpiracionRT = new Date();
                fechaExpiracionRT.setDate(fechaExpiracionRT.getDate() + expiracionEnDias);

                Utilities.setCookie("app-access-token", data.access_token, data.expires_at);
                Utilities.setCookie("app-refresh-token", data.refresh_token, fechaExpiracionRT);
                Utilities.setCookie("app-role", data.role, data.expires_at);
                Utilities.setCookie("app-profile", data.profile, data.expires_at);

                // Se emite evento personalizado para que el resto de la app reaccione
                this.dispatchEvent(new CustomEvent('login-success', { detail: data }));

                errorMsg.textContent = '';

            } catch (err) {
                errorMsg.textContent = err.message || 'Error durante el proceso de login';
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
         
        var subTitleContent = "";
        if (this.loginType === "Web") {
            subTitleContent = "Introduce tus datos para acceder a la plataforma.";
        }
        else if (this.loginType === "Admin") {
            subTitleContent = "Introduce tus datos para acceder a la zona de administración.";
        }
        this.shadowRoot.querySelector('#loginSubTitle').textContent = subTitleContent;
    }
}

// Registrar el nuevo elemento personalizado
customElements.define('app-login', AppLogin);

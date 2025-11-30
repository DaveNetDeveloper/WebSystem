import * as Utilities from '../../WebPages/js/libraries/utilities.js';

//import { baseUrl, port, authControllerName, accessTokenCookieName, loginType_Web, loginType_Admin, loadGlobalConfiguration }
//    from "../../WebPages/js/libraries/appConfiguration.js";

//const css = await fetch('../Components/login/login.css').then(r => r.text());
//const html = await fetch('../Components/login/login.html').then(r => r.text());

// Component properties
const loginType_PropName = "login-type";
const session_PropName = "keep-session";

var loginType_Admin = "Admin";
var loginType_Web = "Web";

class AppLogin extends HTMLElement {

    constructor() {
        super();
        this.attachShadow({ mode: 'open' }); // Activamos Shadow DOM

        //console.log(html);
        //loadGlobalConfiguration();

        this.shadowRoot.innerHTML = `
            <style>
                .skeleton {
                    animation: pulse 1.5s infinite ease-in-out;
                    background: #e2e2e2;
                    border-radius: 4px;
                    height: 16px;
                }
                .skeleton.title { width: 150px; height: 28px; margin-bottom: 20px; }
                .skeleton.input { width: 100%; height: 38px; margin-bottom: 15px; }
                .skeleton.button { width: 40%; height: 40px; margin-top: 20px; }

                @keyframes pulse {
                    0% { opacity: 0.6; }
                    50% { opacity: 1; }
                    100% { opacity: 0.6; }
                }

                .fade-in {
                    opacity: 0;
                    animation: fadeIn 0.4s ease forwards;
                }

                @keyframes fadeIn {
                    0% { opacity: 0; }
                    100% { opacity: 1; }
                }

            </style>
            
            <div id="container">
                <div class="skeleton title"></div>
                <div class="skeleton input"></div>
                <div class="skeleton input"></div>
                <div class="skeleton button"></div>
            </div>
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

    async connectedCallback() {

        //await new Promise(requestAnimationFrame);
        await new Promise(res => setTimeout(res, 200));

        const shadow = this.shadowRoot;
        const cssFiles = [
            "../WebPages/css/bootstrap.min.css",
            "../WebPages/css/nice-select.css",
            "../WebPages/css/font-awesome.min.css",
            "../WebPages/css/icofont.css",
            "../WebPages/css/slicknav.min.css",
            "../WebPages/css/owl-carousel.css",
            "../WebPages/css/datepicker.css",
            "../WebPages/css/animate.min.css",
            "../WebPages/css/normalize.css",
            "../WebPages/css/style.css",
            "../WebPages/css/responsive.css"
        ];

        const linkPromises = cssFiles.map(url => {
            return new Promise(resolve => {
                const link = document.createElement("link");
                link.rel = "stylesheet";
                link.href = url;
                link.onload = resolve;
                shadow.appendChild(link);
            });
        });

        const css = await fetch('../Components/login/login.css').then(r => r.text());
        const html = await fetch('../Components/login/login.html').then(r => r.text());

        await Promise.all(linkPromises);

        const container = shadow.getElementById("container");

        container.innerHTML = `
        <div class="fade-in">
            <style>

                ${css}
                
                /* button color */
                :host([login-type="Admin"]) #btnLogin {
                    background-color: #ecab0f;
                    color: white;
                    font-family: 'Poppins', sans - serif;
                    font-weight: 600;
                }
                :host([login-type="Web"]) #btnLogin {
                    background-color: #0066cc;
                    color: white;
                    font - family: 'Poppins', sans - serif;
                    font - weight: 500;
                }

                /* underline color */
                :host([login-type="Admin"]) h2::before {
                    background-color: #ecab0f !important;
                }
                :host([login-type="Web"]) h2::before {
                    background-color: #0066cc !important;
                } 
           
            </style>

            ${html}

           </div>
        `;
          
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
                alert("entidades: " + data.entidades);

                const expiracionEnDias = 7;
                const fechaExpiracionRT = new Date();
                fechaExpiracionRT.setDate(fechaExpiracionRT.getDate() + expiracionEnDias);

                Utilities.setCookie("app-access-token", data.access_token, data.expires_at);
                Utilities.setCookie("app-refresh-token", data.refresh_token, fechaExpiracionRT);
                Utilities.setCookie("app-role", data.role, data.expires_at);
                Utilities.setCookie("app-profile", data.profile, data.expires_at);

                if (this.loginType == loginType_Admin) { 
                    Utilities.setCookie("app-entities", data.entidades, data.expires_at);
                    console.info("Se han cargado la cookie con la entidades o entidades adminsitradas por el usuario.");
                }

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
        const emailValue = this.shadowRoot.querySelector('#email').value.trim();

        linkPassword.addEventListener('click', function (event) {

            event.preventDefault();
            var url = '../WebPages/changePassword.html?email=' + emailValue; 
            window.location.href = url;
        });
         
        var subTitleContent = "";
        if (this.loginType == loginType_Web) {
            subTitleContent = "Introduce tus datos para acceder a la plataforma.";
        }
        else if (this.loginType == loginType_Admin) {
            subTitleContent = "Introduce tus datos para acceder a la zona de administración.";
        }
        this.shadowRoot.querySelector('#loginSubTitle').textContent = subTitleContent;
    }
}

// Registrar el nuevo elemento personalizado
customElements.define('app-login', AppLogin);

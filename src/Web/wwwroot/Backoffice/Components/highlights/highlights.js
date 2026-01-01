class AppHighlights extends HTMLElement {

    constructor() {
        super();
        this.attachShadow({ mode: "open" }); 
        this.data = []; 
    }

    async connectedCallback() {
        const css = await fetch("/Backoffice/Components/highlights/highlights.css").then(r => r.text());
        const html = await fetch("/Backoffice/Components/highlights/highlights.html").then(r => r.text());

        this.shadowRoot.innerHTML = `
            <style>
            
            .single-counter {
                padding: 30px;
                position: relative;
                background: #fff;
            }

            .counter-box {
                display: flex;
                flex-wrap: wrap; 
                font-family: sans-serif; 
            }
             
            ${css}

            </style>
            ${html}
        `;

        await this.getDashboardTotales();

        this.renderItems();
    }

    async getDashboardTotales() {
        try {
             
            const endpoint = window.location.hostname === "localhost"
                ? "https://localhost:44311/Dashboard/ObtenerDatosTotales"
                : "https://websystem-api-prod-ctgrd4gfc2e6dudb.westeurope-01.azurewebsites.net/Dashboard/ObtenerDatosTotales";
                 

            const response = await fetch(endpoint, {
                method: "GET",
                headers: {
                    "Content-Type": "application/json"
                }
            });

            if (!response.ok) {
                throw new Error("Error al obtener los datos del dashboard");
            }

            const globalData = await response.json();

            this.data = [
                { icon: "fas fa-user", value: globalData.usuarios, label: "Usuarios" },
                { icon: "fas fa-clock", value: globalData.entidades, label: "Entidades" },
                { icon: "fas fa-check", value: globalData.transacciones, label: "Transacciones" },
                { icon: "fas fa-times", value: globalData.visitas, label: "Visitas" }
            ];

            this.renderItems();

        } catch (error) {
            console.error("Dashboard error:", error);
        }
    } 
     
    renderItems() {
        if (!this.shadowRoot) return;

        const container = this.shadowRoot.querySelector(".counter-box");
        if (!container) return;

        container.innerHTML = "";

        this.data.forEach(item => {
            const block = document.createElement("div");
            block.className = "col-xl-3 col-lg-6 col-md-6";

            block.innerHTML = `
                <div class="single-counter d-flex justify-content-between flex-wrap align-items-center mb-30">
                    <i class="${item.icon}"></i>
                    <div class="c-number">
                        <h3 class="counter">${item.value}</h3>
                        <span>${item.label}</span>
                    </div>
                </div>
            `;

            container.appendChild(block);
        });
    }
}

customElements.define("app-highlights", AppHighlights);

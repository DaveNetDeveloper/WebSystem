class AppHighlights extends HTMLElement {

    constructor() {
        super();
        this.attachShadow({ mode: "open" });

        // DataSource que podrás reemplazar dinámicamente
        this.data = [
            { icon: "fas fa-signal", value: "41,456", label: "Usuarios" },
            { icon: "far fa-lightbulb", value: "62,236", label: "Visitas" },
            { icon: "fas fa-chart-line", value: "29,423", label: "Transacciones" },
            { icon: "far fa-comments", value: "32,220", label: "Contactos" }
        ];
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

        this.renderItems();
    }

    /** Permite cambiar los datos desde fuera */
    set dataSource(value) {
        this.data = value;
        this.renderItems();
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

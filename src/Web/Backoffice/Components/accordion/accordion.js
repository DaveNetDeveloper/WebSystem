
class AccordionComp extends HTMLElement {

    static get observedAttributes() {
        return ["entidad-id", "titulo"];
    }

    constructor() {
        super();
        this.attachShadow({ mode: "open" });
    }

    async connectedCallback() {
        const css = await fetch("Components/accordion/accordion.css").then(r => r.text());
        const html = await fetch("Components/accordion/accordion.html").then(r => r.text());

        this.shadowRoot.innerHTML = `
            <style>${css}</style>
            ${html}
        `;

        this.renderTitle();
        this.loadData();
    }

    attributeChangedCallback() {
        if (this.shadowRoot) {
            this.renderTitle();
        }
    }

    renderTitle() {
        const title = this.getAttribute("titulo");
        const h4 = this.shadowRoot.querySelector("#accordion-title");
        if (h4) h4.textContent = title;
    }

    async loadData() {
        const id = this.getAttribute("entidad-id");

        if (!id) return;

        const baseUrl = `https://localhost`;
        const port = `44311`;
        const controller = `Entidades`;
        const method = `ObtenerEntidad`;
        const url = `${baseUrl}:${port}/${controller}/${method}/${id}`;

        const response = await fetch(url);
        const entidad = await response.json();

        this.renderAccordion(entidad);
        this.iniciarAcordeon();
    }

    async iniciarAcordeon() {
        const root = this.shadowRoot;

        const items = root.querySelectorAll(".card");

        items.forEach(card => {
            const btn = card.querySelector(".btn-link");
            const collapse = card.querySelector(".collapse");

            btn.addEventListener("click", evt => {
                evt.preventDefault();

                const isOpen = collapse.classList.contains("show");

                // Cerrar todos los demás
                root.querySelectorAll(".collapse.show").forEach(openItem => {
                    if (openItem !== collapse) {
                        openItem.classList.remove("show");
                    }
                });

                // Abrir/cerrar el actual
                collapse.classList.toggle("show", !isOpen);
            });
        });
    }


    async renderAccordion(entidad) {
        const root = this.shadowRoot.querySelector("#accordion-root");
        root.innerHTML = ""; // clean

        // generamos un único apartado basado en los datos recibidos
        const cardId = `accordion-${entidad.id}`;
        const collapseId = `collapse-${entidad.id}`;

        root.innerHTML = `
            <div class="card">
                <div class="card-header" id="${cardId}">
                    <h5 class="mb-0">
                        <a class="btn btn-link" 
                           data-toggle="collapse" 
                           data-target="#${collapseId}" 
                           aria-expanded="true" 
                           aria-controls="${collapseId}">
                            ${entidad.nombre}
                        </a>
                    </h5>
                </div>

                <div id="${collapseId}" 
                     class="collapse show" 
                     aria-labelledby="${cardId}" 
                     data-parent="#accordion-root">
                     
                    <div class="card-body">
                       ${ (entidad.descripcion && entidad.descripcion.length > 500)
                            ? entidad.descripcion.substring(0, 500) + '...'
                            : entidad.descripcion ?? "Sin descripción" }
                    </div>
                </div>

            </div>

        `;
    }
}

customElements.define("app-accordion", AccordionComp);


//class AccordionComponent extends HTMLElement {
//    constructor() {
//        super();
//        this.attachShadow({ mode: "open" });

//        this.baseUrl = `https://localhost`;
//        this.port = `44311`;
//        this.controller = `Entidades`;
//        this.method = `ObtenerEntidad`;
//    }

//    async connectedCallback() {
//        await this.loadTemplate();
//        const entidadId = this.getAttribute("entidad-id");

//        if (!entidadId) {
//            console.error("No se ha especificado 'entidad-id' en <app-accordion>");
//            return;
//        }

//        const entidad = await this.obtenerEntidad(entidadId);
//        if (entidad) {
//            this.renderAccordion(entidad);
//            this.initAccordionListeners();
//        }
//    }

//    async loadTemplate() {
//        const html = await fetch("Components/accordion/accordion.html").then(r => r.text());
//        const css = await fetch("Components/accordion/accordion.css").then(r => r.text());

//        this.shadowRoot.innerHTML = `
//            <style>${css}</style>
//            ${html}
//        `;
//    }

//    async obtenerEntidad(id) {
//        try {
//            const url = `${this.baseUrl}:${this.port}/${this.controller}/${this.method}/${id}`;
//            const response = await fetch(url);

//            if (!response.ok) throw new Error("Error al obtener entidad");

//            return await response.json();
//        } catch (err) {
//            console.error("Error al llamar al endpoint:", err);
//            return null;
//        }
//    }

//    renderAccordion(entidad) {
//        const root = this.shadowRoot.querySelector("#accordion-root");
//        const titulo = this.shadowRoot.querySelector("#titulo-accordion");

//        titulo.textContent = `Información de ${entidad.nombre}`;

//        const html = `
//            <div class="card">
//                <div class="card-header">
//                    <h5>
//                        <a class="accordion-toggle" data-target="item1">
//                            ${entidad.nombre}
//                        </a>
//                    </h5>
//                </div>

//                <div id="item1" class="collapse show">
//                    <div class="card-body">
//                        ${entidad.descripcion ?? "Sin descripción disponible."}
//                    </div>
//                </div>
//            </div>

//        `;

//        root.innerHTML = html;
//    }

//    initAccordionListeners() {
//        const shadow = this.shadowRoot;

//        shadow.querySelectorAll(".accordion-toggle").forEach(btn => {
//            btn.addEventListener("click", () => {
//                const targetId = btn.getAttribute("data-target");
//                const panel = shadow.querySelector(`#${targetId}`);
//                const isOpen = panel.classList.contains("show");

//                // Ocultar todos
//                shadow.querySelectorAll(".collapse").forEach(p => p.classList.remove("show"));

//                // Mostrar solo si estaba cerrado
//                if (!isOpen) panel.classList.add("show");
//            });
//        });
//    }
//}

//customElements.define("app-accordion", AccordionComponent);

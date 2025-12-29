class LightBoxComponent extends HTMLElement {
    constructor() {
        super();
        this.attachShadow({ mode: "open" });
    }

    async connectedCallback() {
        const css = await preloadCss('./Components/lightbox/lightbox.css');
        const html = await fetch('./Components/lightbox/lightbox.html').then(r => r.text());
         
        this.shadowRoot.innerHTML = ` 
            <style>

                .heading-border {
                    border-bottom: 1px solid #eaeaea;
                }
                .panel-heading {
                    padding: 20px 30px;
                }
                .panel-body {
                    padding: 30px;
                }
                .panel-heading h4 {
                    font-size: 16px;
                    font-weight: 700;
                    line-height: 1;
                    margin: 0;
                }


                /* --- Flechas Magnific Popup --- */
                .mfp-arrow {
                    opacity: 1;
                    margin: 0;
                    top: 50%;
                    width: 90px;
                    height: 110px;
                    -webkit-tap-highlight-color: transparent;
                    position: absolute;
                    cursor: pointer;
                    background: transparent;
                    border: none;
                    outline: none;
                    transform: translateY(-50%);
                }

                .mfp-arrow:before,
                .mfp-arrow:after {
                    font-family: Arial, sans-serif;
                    font-size: 50px;
                    line-height: 1;
                    color: white;
                }

                .mfp-arrow-left {
                    left: 0;
                }

                .mfp-arrow-left:before {
                    content: "<";
                }

                .mfp-arrow-right {
                    right: 0;
                }

                .mfp-arrow-right:before {
                    content: ">";
                }

                ${css}

            </style>
            ${html}
        `;

        this.titulo = this.getAttribute("titulo"); // || "Galería";
        const tituloEl = this.shadowRoot.querySelector("#titulo-galeria");
        if (tituloEl) tituloEl.textContent = this.titulo;

        const entidadId = this.getAttribute("entidad-id");
        if (!entidadId) {
            console.error("LightBoxComponent: Falta el atributo entidad-id");
            return;
        }

        const data = await this.obtenerEntidad(entidadId);
        if (!data || !data.imagen) return;

        this.renderGaleria(data);
        this.iniciarLightbox();
    }

    async obtenerEntidad(id) {
        try {
            const baseUrl = `https://localhost`;
            const port = `44311`;
            const controllerName = `Entidades`;
            const apiMethod = `ObtenerEntidad`;

            const url = `${baseUrl}:${port}/${controllerName}/${apiMethod}`;

            const response = await fetch(`${url}/${id}`);
            if (!response.ok) throw new Error("API error");
            return await response.json();
        } catch (e) {
            console.error("Error obteniendo entidad:", e);
            return null;
        }
    }

    async renderGaleria(entidad) {

        const container = this.shadowRoot.querySelector("#galleryContainer");
        const imgPath = "/Resources/entidades";

        container.innerHTML = `
            <div class="col-lg-3 col-md-6 mb-30">
                <a class="popup-image-gallery" href="${imgPath}/${entidad.imagen}">
                    <img style="height: 165px;" src="${imgPath}/${entidad.imagen}" alt="${entidad.nombre}">
                </a>
            </div>
             <div class="col-lg-3 col-md-6 mb-30">
                <a class="popup-image-gallery" href="${imgPath}/Bostik.jpg">
                    <img  style="height: 165px;" src="${imgPath}/Bostik.jpg" alt="Imgen Nau Bostik">
                </a>
            </div>
             <div class="col-lg-3 col-md-6 mb-30">
               <a class="popup-image-gallery" href="${imgPath}/EspaiGrafic.jpg">
                    <img  style="height: 165px;" src="${imgPath}/EspaiGrafic.jpg" alt="Espai Grafic">
                </a>
            </div>
             <div class="col-lg-3 col-md-6 mb-30">
               <a class="popup-image-gallery" href="${imgPath}/CalTon.jpg">
                    <img  style="height: 165px;" src="${imgPath}/CalTon.jpg" alt="Cal Ton">
                </a>
            </div>  
        `;
    }
      
    async iniciarLightbox() {
        setTimeout(() => {

            const anchors = Array.from(
                this.shadowRoot.querySelectorAll(".popup-image-gallery")
            );

            // Crear lista completa de imágenes para la galería
            const items = anchors.map(a => ({
                src: a.getAttribute("href"),
                type: "image"
            }));

            anchors.forEach((a, index) => {
                a.addEventListener("click", e => {
                    e.preventDefault();

                    $.magnificPopup.open({
                        items: items,
                        type: "image",
                        gallery: {
                            enabled: true
                        }
                    }, index); 
                });
            });  
        }, 100);
    } 
} 
customElements.define("app-lightbox", LightBoxComponent);

class ModalPanel extends HTMLElement {
    constructor() {
        super();
        const shadow = this.attachShadow({ mode: 'open' });
        fetch('../Components/modalPanel/modalPanel.html')
            .then(response => response.text())
            .then(html => {
                const parser = new DOMParser();
                const doc = parser.parseFromString(html, 'text/html');
                const template = doc.getElementById('modal-template');
                 
                if (template == null) {
                    console.error("Template no encontrado en modalPanel.html");
                    return;
                }
                shadow.appendChild(template.content.cloneNode(true));
                this.initEvents();
            });
    }

    initEvents() {

        const closeBtn = this.shadowRoot.querySelector('.cerrar'); 

        if (closeBtn) {
            closeBtn.addEventListener('click', () => { this.ocultar(); });
        }

        this.shadowRoot.addEventListener('click', (e) => {
            if (e.target.classList.contains("modal")) { this.ocultar();}
        });

        document.addEventListener('keydown', (e) => {
            if (e.key === "Escape") {
                this.ocultar();
            }
        });

    }

    setContent(title, message) {
        this.shadowRoot.querySelector('h3').textContent = title;
        this.shadowRoot.querySelector('p').textContent = message;
    }

    mostrar() {
        const modal = this.shadowRoot.querySelector('.modal');
        modal.classList.add("mostrar");
    }

    ocultar() {
        const modal = this.shadowRoot.querySelector('.modal');
        modal.classList.remove("mostrar");
    }
}

customElements.define('modal-panel', ModalPanel); 
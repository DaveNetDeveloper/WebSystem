class ModalPanel extends HTMLElement {
    constructor() {
        super();
        const shadow = this.attachShadow({ mode: 'open' });
        fetch('/Components/modalPanel/modalPanel.html')
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
        const btnCancel = this.shadowRoot.querySelector('.cancel');
        const btnConfirm = this.shadowRoot.querySelector('.confirm');

        // Cerrar modal con "x"
        if (closeBtn) {
            closeBtn.addEventListener('click', () => this.ocultar());
        }

        // Botón Cancelar
        if (btnCancel) {
            btnCancel.addEventListener('click', () => {
                this.dispatchEvent(new CustomEvent('cancel', { bubbles: true, composed: true }));
                this.ocultar();
            });
        }

        // Botón Confirmar
        if (btnConfirm) {
            btnConfirm.addEventListener('click', () => {
                this.dispatchEvent(new CustomEvent('confirm', { bubbles: true, composed: true }));
                this.ocultar();
            });
        }

        // Clic fuera de la ventana modal
        this.shadowRoot.addEventListener('click', (e) => {
            if (e.target.classList.contains("modal")) { this.ocultar(); }
        });

        // Escape para cerrar modal
        document.addEventListener('keydown', (e) => {
            if (e.key === "Escape") { this.ocultar(); }
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

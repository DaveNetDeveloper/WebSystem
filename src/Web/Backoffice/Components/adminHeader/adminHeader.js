let templatePath = "../Backoffice/Components/adminHeader";
const rol_PropName = "rol";
const isHome_PropName = "is-home";

class AdminMenu extends HTMLElement {
    constructor() {
        super();
        this.attachShadow({ mode: "open" });
        this.rol = null;
        this.isHome = null;
        this._meanResizeHandler = null;
        this.mobileMenuOpen = false;

        this.shadowRoot.innerHTML = `<style>:host { visibility: hidden; }</style>`;
    }

    static get observedAttributes() {
        return [rol_PropName, isHome_PropName];
    }

    attributeChangedCallback(name, oldValue, newValue) {
        if (name === rol_PropName) {
            this.rol = newValue;
        }
        if (name === isHome_PropName) {
            this.isHome = newValue;
        }
    }

    async connectedCallback() {
        await this.loadTemplate();

        if (!this.rol) {
            this.rol = await this.getCookie("app-role");
        } 
        if (!this.rol) {
            console.error("No se encontró el rol en atributo ni en cookie.");
            return;
        } 
        await this.loadMenuFromServer();
    }

    async disconnectedCallback() {
        if (this._meanResizeHandler) window.removeEventListener('resize', this._meanResizeHandler);
    }

    async loadTemplate() {

        if (this.isHome == true || this.isHome == 'true') {

            templatePath = '../Backoffice/Components/adminHeader';
        }
        else {
            templatePath = '../Components/adminHeader';
        }

        const html = await fetch(`${templatePath}/adminHeader.html`).then(r => r.text());
        const css = await fetch(`${templatePath}/adminHeader.css`).then(r => r.text());

        let prefixCss = '../';
        if (this.isHome == true || this.isHome == 'true') {

            prefixCss = '';
        }  

        // Carga global sin 'link' 
        const bootstrap = await preloadCss(prefixCss + "css/bootstrap.min.css");
        const animate = await preloadCss(prefixCss + "css/animate.min.css");
        const fontawesome = await preloadCss(prefixCss + "css/fontawesome-all.min.css");
        const etline = await preloadCss(prefixCss + "css/et-line-fonts.css");
        const meanmenu = await preloadCss(prefixCss + "css/meanmenu.css");
        const defaultCss = await preloadCss(prefixCss + "css/default.css");
        const style = await preloadCss(prefixCss + "css/style.css");
        const responsive = await preloadCss(prefixCss + "css/responsive.css");

        const template = document.createElement("template");
        template.innerHTML = ` 
            <style>
                ${bootstrap}
                ${animate}
                ${fontawesome}
                ${etline}
                ${meanmenu}
                ${defaultCss}
                ${style}
                ${responsive} 
               
                ${css}

                .active-menu > a {
                    color: #e0a800 !important;
                    font-weight: 600;
                }
                 
                #mobileMenuList li.active-menu > a:hover {
                   
                    background-color: rgba(255, 152, 0, 0.15);
                    color: white;
                }

            </style>

            ${html}
        `;

        this.shadowRoot.appendChild(template.content.cloneNode(true));

        requestAnimationFrame(() => {
            this.shadowRoot.host.style.visibility = "visible";
        });
    }

    async getCookie(name) {
        const value = `; ${document.cookie}`;
        const parts = value.split(`; ${name}=`);
        if (parts.length === 2) return parts.pop().split(";").shift();
        return null;
    }

    async loadMenuFromServer() {
        try {
            const baseUrl = `https://localhost`;
            const port = `44311`;
            const controllerName = `Permisos`;
            const apiMethod = `ObtenerOpcionesMenu`;

            const url = `${baseUrl}:${port}/${controllerName}/${apiMethod}?rol=${this.rol}`;

            const response = await fetch(url, {
                method: "GET",
                headers: {
                    "Authorization": "Bearer " + this.getCookie("app-access-token")
                }
            });

            if (!response.ok) {
                console.error("Error al obtener opciones de menú");
                return;
            }

            const opciones = await response.json();
            this.renderMenu(opciones);

        } catch (error) {
            console.error("Error cargando menú:", error);
        }
    }

    getActiveMenuFromUrl() {
        const path = window.location.pathname.toLowerCase();
        const segments = path.split("/").filter(Boolean);
        return segments.length > 0 ? segments[segments.length-2] : null;
    }

    getActivePageFromUrl() {
        const path = window.location.pathname.toLowerCase();
        const segments = path.split("/").filter(Boolean);
        return segments.length > 0 ? segments[segments.length - 1] : null; 
    }

    cortarHastaGuion(texto) {
        if (!texto) return "";

        const index = texto.indexOf("-");
        return index === -1 ? texto : texto.substring(0, index);
    }

    // Render dinámico del menú desktop
    async renderMenu(opciones) {
        const ulMenu = this.shadowRoot.querySelector("#ulMenuOptions");
        if (!ulMenu) {
            console.error("No se encontró #ulMenuOptions");
            return;
        }
        ulMenu.innerHTML = "";
         
        const nivel1 = opciones.filter(o => o.nivel === 1);
        const nivel2 = opciones.filter(o => o.nivel === 2);

        let activeMenu = this.getActiveMenuFromUrl();
        let activePage = this.cortarHastaGuion(this.getActivePageFromUrl());
         
        if (activeMenu == 'backoffice') activeMenu = activePage;

        nivel1.forEach(op => {
            const li = document.createElement("li");
            const hijos = nivel2.filter(h => h.parent === op.nombre);
              
            if (op.nombre.toLowerCase().includes(activeMenu.toLowerCase())) {
                li.classList.add("active-menu");
            }  

            if (hijos.length === 0) {
                li.innerHTML = `<a href="${op.path}">${op.nombre}</a>`;
            } else {
                li.innerHTML = `
                    <a href="${op.path}">${op.nombre}</a>
                    <ul class="submenu"></ul>
                `;
                const subUl = li.querySelector("ul.submenu");
                hijos.forEach(sub => {
                    const subLi = document.createElement("li");
                    subLi.innerHTML = `<a href="${sub.path}">${sub.nombre}</a>`;

                    if (sub.nombre.toLowerCase().includes(activePage.toLowerCase())) {
                        subLi.classList.add("active-menu");
                    }   
                    subUl.appendChild(subLi);
                });
            } 
            ulMenu.appendChild(li);
        });

        // Generar menú mobile
        this.renderMobileMenu(opciones);

        // Inicialitzar la versió vanilla de MeanMenu
        this.initMeanMenuVanilla({
            meanScreenWidth: 991,
            meanRevealOpenText: "☰",
            meanRevealCloseText: "X",
            meanRevealFontSize: "18px",
            animationDuration: 300,
            meanExpandText: "+",
            meanContractText: "-"
        });
    }

    // Crea el HTML del menú mobile dins del #mobileMenuContainer (shadowRoot)
    async renderMobileMenu(opciones) {
        const mobileList = this.shadowRoot.querySelector("#mobileMenuList");
        if (!mobileList) {
            console.error("No se encontró #mobileMenuList dentro del ShadowRoot");
            return;
        }

        mobileList.innerHTML = "";
         
        const nivel1 = opciones.filter(o => o.nivel === 1);
        const nivel2 = opciones.filter(o => o.nivel === 2);

        let activeMenu = this.getActiveMenuFromUrl();
        let activePage = this.cortarHastaGuion(this.getActivePageFromUrl());

        if (activeMenu == 'backoffice') activeMenu = activePage;

        nivel1.forEach(op => {
            const li = document.createElement("li");
            const hijos = nivel2.filter(h => h.parent === op.nombre);
             
            if (op.nombre.toLowerCase().includes(activeMenu.toLowerCase())) { 
                li.classList.add("active-menu");
            } 

            if (hijos.length === 0) {
                li.innerHTML = `<a href="${op.path}">${op.nombre}</a>`;
            } else {
                li.innerHTML = `
                    <a href="${op.path}">${op.nombre}</a>
                    <ul class="submenu" style="display: none;"></ul>
                `;

                const subUl = li.querySelector(".submenu");
                hijos.forEach(sub => {
                    const subLi = document.createElement("li");
                    subLi.innerHTML = `<a href="${sub.path}">${sub.nombre}</a>`;

                    if (sub.nombre.toLowerCase().includes(activePage.toLowerCase())) {
                        subLi.classList.add("active-menu");
                    }  
                    subUl.appendChild(subLi);
                });
            } 
            mobileList.appendChild(li);
        });

        const mainUl = this.shadowRoot.querySelector(".mean-nav > ul");
        if (mainUl) {
            mainUl.style.display = "none";
            mainUl.style.maxHeight = null;
        }
    }

    // replica les funcionalitats clau de meanmenu però sense jQuery.
    async initMeanMenuVanilla(options = {}) {
        const opts = Object.assign({
            meanScreenWidth: 991,
            meanRevealOpenText: "☰",
            meanRevealCloseText: "X",
            meanRevealFontSize: "18px",
            animationDuration: 300,
            meanExpandText: "+",
            meanContractText: "-",
            meanShowChildren: true,
            meanExpandableChildren: true
        }, options);

        // helpers slideDown/slideUp amb max-height i transition
        const slideDown = (el, duration = opts.animationDuration) => {
            if (!el) return;
            el.style.removeProperty('display');
            let display = window.getComputedStyle(el).display;
            if (display === 'none') display = 'block';
            el.style.display = display;
            const height = el.scrollHeight;
            el.style.overflow = 'hidden';
            el.style.maxHeight = '0px';
            el.offsetHeight; // reflow
            el.style.transition = `max-height ${duration}ms ease`;
            requestAnimationFrame(() => el.style.maxHeight = height + 'px');
            const done = () => {
                el.style.removeProperty('max-height');
                el.style.removeProperty('overflow');
                el.style.removeProperty('transition');
                el.removeEventListener('transitionend', done);
            };
            el.addEventListener('transitionend', done);
        };

        const slideUp = (el, duration = opts.animationDuration) => {
            if (!el) return;
            el.style.overflow = 'hidden';
            el.style.maxHeight = el.scrollHeight + 'px';
            el.offsetHeight; // reflow
            el.style.transition = `max-height ${duration}ms ease`;
            requestAnimationFrame(() => el.style.maxHeight = '0px');
            const done = () => {
                el.style.display = 'none';
                el.style.removeProperty('max-height');
                el.style.removeProperty('overflow');
                el.style.removeProperty('transition');
                el.removeEventListener('transitionend', done);
            };
            el.addEventListener('transitionend', done);
        };

        // Elements dins del shadowRoot
        const mobileContainer = this.shadowRoot.querySelector('#mobileMenuContainer');
        const mobileNav = this.shadowRoot.querySelector('.mean-nav');
        const desktopWrapper = this.shadowRoot.querySelector('.main-menu');
        const desktopUl = this.shadowRoot.querySelector('#ulMenuOptions');

        if (!mobileContainer || !mobileNav || !desktopWrapper || !desktopUl) {
            console.warn('MeanMenuVanilla: elements missing in shadowRoot.');
            return;
        }

        // Botó reveal ja està dins de l'HTML: .meanmenu-reveal
        let revealBtn = mobileContainer.querySelector('.meanmenu-reveal');
        const navUl = mobileNav.querySelector('ul');

        // assegurar estat inicial
        if (navUl) {
            navUl.style.display = 'none';
            navUl.style.maxHeight = null;
        }

        // si no existeix revealBtn (per si el template canvia), el creem
        if (!revealBtn) {
            revealBtn = document.createElement('a');
            revealBtn.className = 'meanmenu-reveal';
            revealBtn.href = '#nav';
            revealBtn.style.fontSize = opts.meanRevealFontSize;
            revealBtn.style.textAlign = 'center';
            revealBtn.style.textIndent = '0';
            revealBtn.innerHTML = opts.meanRevealOpenText;
            const bar = mobileContainer.querySelector('.mean-bar') || mobileContainer;
            bar.insertBefore(revealBtn, mobileNav);
        }

        // Afegir expanders a submenus i llistar listeners
        const addExpanders = () => {
            const subMenus = mobileNav.querySelectorAll('ul.submenu');
            subMenus.forEach(sub => {
                const parentLi = sub.parentElement;
                if (!parentLi) return;

                // evitar duplicates
                if (!parentLi.querySelector('.mean-expand')) {
                    const exp = document.createElement('a');
                    exp.className = 'mean-expand';
                    exp.href = '#';
                    exp.style.fontSize = opts.meanRevealFontSize;
                    exp.textContent = opts.meanExpandText;
                    parentLi.appendChild(exp);

                    // col·locar estat inicial (tancat)
                    sub.style.display = 'none';

                    exp.addEventListener('click', (ev) => {
                        ev.preventDefault();
                        const isOpen = exp.classList.contains('mean-clicked');
                        if (isOpen) {
                            exp.classList.remove('mean-clicked');
                            exp.textContent = opts.meanExpandText;
                            slideUp(sub, opts.animationDuration);
                        } else {
                            exp.classList.add('mean-clicked');
                            exp.textContent = opts.meanContractText;
                            slideDown(sub, opts.animationDuration);
                        }
                    });
                }
            });
        };

        // Toggle principal (revealBtn)
        const onRevealClick = (e) => {
            e.preventDefault();
            const isOpen = revealBtn.classList.contains('meanclose');

            if (isOpen) {
                // close
                revealBtn.classList.remove('meanclose');
                revealBtn.innerHTML = opts.meanRevealOpenText;
                if (navUl) slideUp(navUl, opts.animationDuration);
                this.mobileMenuOpen = false;
            } else {
                // open
                revealBtn.classList.add('meanclose');
                revealBtn.innerHTML = opts.meanRevealCloseText;
                if (navUl) slideDown(navUl, opts.animationDuration);
                this.mobileMenuOpen = true;
            }
        };

        // ensure single handler
        revealBtn.removeEventListener('click', onRevealClick);
        revealBtn.addEventListener('click', onRevealClick);

        // Comportament responsive: mostrar mobile <= breakpoint
        const checkBreakpoint = () => {
            const width = window.innerWidth || document.documentElement.clientWidth;
            if (width <= opts.meanScreenWidth) {
                mobileContainer.style.display = 'block';
                // ocultar desktop menu visualment
                if (desktopWrapper) desktopWrapper.style.display = 'none';
                // preparar expanders
                addExpanders();
            } else {
                mobileContainer.style.display = 'none';
                if (desktopWrapper) desktopWrapper.style.display = '';
                // tancar navUl si està obert
                if (navUl) {
                    // tancar amb slideUp si està obert
                    if (this.mobileMenuOpen) {
                        slideUp(navUl, opts.animationDuration);
                        this.mobileMenuOpen = false;
                    }
                    navUl.style.removeProperty('max-height');
                    navUl.style.display = 'none';
                }
                // restaurar reveals i expanders (no esborrem elements, només restablim estats)
                revealBtn.classList.remove('meanclose');
                revealBtn.innerHTML = opts.meanRevealOpenText;
            }
        };

        // bind resize (debounce)
        checkBreakpoint();
        let resizeTO;
        const onResize = () => {
            clearTimeout(resizeTO);
            resizeTO = setTimeout(checkBreakpoint, 120);
        };
        // neteja handler anterior si existeix
        if (this._meanResizeHandler) window.removeEventListener('resize', this._meanResizeHandler);
        this._meanResizeHandler = onResize;
        window.addEventListener('resize', this._meanResizeHandler);
    }
}

customElements.define("admin-menu", AdminMenu); 
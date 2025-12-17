//import * as Utilities from '/WebPages/js/libraries/utilities.js';

import * as globalAuth from '/WebPages/js/libraries/globalAuth.js'; 

const dataSource_PropName = "data-source";
const showExport_PropName = "show-export";
const showActions_PropName = "show-actions";
const controllerName_PropName = "controller";


class BOReadOnlyList extends HTMLElement {
    constructor() {
        super();
        this.attachShadow({ mode: "open" });

        this.fullData = [];         // Datos completos
        this.filteredData = [];     // Datos filtrados por búsqueda
        this.pageSize = 10;         // Tamaño de página
        this.searchText = "";       // Texto del filtro
        this.currentPage = 1;       // Página actual
    }

    static get observedAttributes() {
        return [dataSource_PropName, showExport_PropName, showActions_PropName, controllerName_PropName];
    }

    attributeChangedCallback(name, oldValue, newValue) {
        if (name === dataSource_PropName) {
            this.dataSource = newValue;
        }
        if (name === showExport_PropName) {
            this.showExport = newValue === "true" || newValue === "";
        }
        if (name === showActions_PropName) {
            this.showActions = newValue === "true" || newValue === "";
        }
        if (name == controllerName_PropName) {
            this.controller = newValue;
        } 
    }

    async connectedCallback() {

        const basePath = '/Backoffice/Components/readOnlyList/';

        const css = await fetch(basePath + 'readOnlyList.css').then(r => r.text());
        const html = await fetch(basePath + 'readOnlyList.html').then(r => r.text());

        //const css = await fetch('/Backoffice/Components/readOnlyList/readOnlyList.css').then(r => r.text());
        //const html = await fetch('/Backoffice/Components/readOnlyList/readOnlyList.html').then(r => r.text());


        this.shadowRoot.innerHTML = `

            <link rel="stylesheet" href="/Backoffice/css/bootstrap.min.css">
            <link rel="stylesheet" href="/Backoffice/css/et-line-fonts.css">
            <link rel="stylesheet" href="/Backoffice/css/fontawesome-all.min.css">
            <link rel="stylesheet" href="/Backoffice/css/themify-icons.css">
            <link rel="stylesheet" href="/Backoffice/css/default.css">
            <link rel="stylesheet" href="/Backoffice/css/style.css">
            <link rel="stylesheet" href="/Backoffice/css/responsive.css">
            <link rel="stylesheet" href="https://cdn.datatables.net/1.10.19/css/dataTables.bootstrap4.min.css">
             
            <style> 
                ${css} 
                
                [class^="ti-"], [class*=" ti-"] {
	                font-family: "themify";
                    src: url('../fonts/themify.woff') format('woff');
                }

                @font-face {
                    font-family: 'themify';
                    src: url('../fonts/themify.woff') format('woff');
                    
                }

                .ti-close::before {
                    content: "\\e646" !important;
                    font-family: 'themify' !important;
                }
                 
                :host table.dataTable thead th.sorting::before {
                    content: "↑";
                    position: absolute;
                    right: 1em;
                    opacity: 0.3;
                }

                :host table.dataTable thead th.sorting::after {
                    content: "↓";
                    position: absolute;
                    right: 0.5em;
                    opacity: 0.3;
                }

                :host table.dataTable thead th.sorting_asc::before {
                    opacity: 1;
                }

                :host table.dataTable thead th.sorting_desc::after {
                    opacity: 1;
                }

                /*table.dataTable thead th {
                    position: relative !important;
                }*/

                :host table#table_name tbody tr:hover {
                    background-color: #ffffde;
                    /*cursor: pointer;  */
                } 
            </style>

            ${html}
        `;

        await this.loadData();
        this.setupEventHandlers();
        this.applyFilters();
         
        var btnCrear = this.shadowRoot.querySelector('#btnCrear'); 
        btnCrear.addEventListener('click', (e) => {
            e.preventDefault();
            window.location.href = "../admin/qr-form-new.html";
        });

        var btnExport = this.shadowRoot.querySelector('#btnExport');  
        btnExport.hidden = !this.showExport; 

        this.initExportDropdowns();  
    }

    initExportDropdowns() {
        this.shadowRoot.addEventListener('click', (e) => {
            const target = e.target;

            if (target.id === 'exportExcel') {
                e.preventDefault();
                this.exportData("Excel");
            }
            else if (target.id === 'exportPdf') {
                e.preventDefault();
                this.exportData("Pdf");
            }

            // dropdown toggle
            const group = target.closest(".btn-group");
            if (group && target.classList.contains("export-btn")) {
                e.stopPropagation();
                const menu = group.querySelector(".dropdown-menu");
                const isOpen = group.classList.contains("show");
                this.closeAllDropdowns();
                if (!isOpen) {
                    group.classList.add("show");
                    menu.classList.add("show");
                }
            }
        });

        // click fuera del shadow para cerrar dropdowns
        this.addEventListener("click", (e) => {
            if (!this.shadowRoot.contains(e.target)) {
                this.closeAllDropdowns();
            }
        });
    }

    closeAllDropdowns() {
        const openGroups = this.shadowRoot.querySelectorAll(".btn-group.show");
        const openMenus = this.shadowRoot.querySelectorAll(".dropdown-menu.show");

        openGroups.forEach(g => g.classList.remove("show"));
        openMenus.forEach(m => m.classList.remove("show"));
    } 

    async exportData(exportType) {

        const baseUrl = 'https://localhost';
        //const controllerName = 'DataQuery';
        const port = '44311';
        const apiMethod = 'Exportar';
        const envioEmail = false;

        const apiUrl =
            `${baseUrl}:${port}/${this.controller}/${apiMethod}` +
            `?formato=${exportType}&envioEmail=${envioEmail}`;
             
        const token = this.getCookie('app-access-token');

        const response = await fetch(apiUrl, {
            method: "GET",
            credentials: "include",
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
        });

        //const response = await fetch(apiUrl);

        if (!response.ok) throw new Error("Error al exportar los datos");

        const blob = await response.blob();
        const url = window.URL.createObjectURL(blob);

        const a = document.createElement("a");
        a.href = url;
        a.download = `export_${this.controller}.${exportType === "Excel" ? "xlsx" : "pdf"}`;
        a.click();

        window.URL.revokeObjectURL(url);
    }

    // TODO
    //async exportData(exportType) {

    //    const baseUrl = 'https://localhost';
    //    const controllerName = 'DataQuery';
    //    const port = '44311';
    //    const apiMethod = 'Exportar';

    //    // params
    //    const dataQueryType = 'UsuariosIdiomas'; // TODO!
    //    const envioEmail = false;

    //    const apiUrl =
    //        `${baseUrl}:${port}/${controllerName}/${apiMethod}` +
    //        `?dataQueryType=${dataQueryType}&formato=${exportType}&envioEmail=${envioEmail}`;

    //    const response = await fetch(apiUrl);
    //    if (!response.ok) throw new Error("Error al exportar los datos");

    //    const blob = await response.blob();
    //    const url = window.URL.createObjectURL(blob);

    //    const a = document.createElement("a");
    //    a.href = url;
    //    a.download = `export_${dataQueryType}.${exportType === "Excel" ? "xlsx" : "pdf"}`;
    //    a.click();

    //    window.URL.revokeObjectURL(url);
    //}

    async getCookie(nombre) {
        const nombreEQ = nombre + "=";
        const cookies = document.cookie.split(';');
        for (let i = 0; i < cookies.length; i++) {
            let cookie = cookies[i];
            while (cookie.charAt(0) === ' ') {
                cookie = cookie.substring(1, cookie.length);
            }
            if (cookie.indexOf(nombreEQ) === 0) {
                return decodeURIComponent(cookie.substring(nombreEQ.length, cookie.length));
            }
        }
        return null;
    }

    // Cargar datos de la API
    async loadData() {
        try {
            const baseUrl = 'https://localhost';
            //const controllerName = 'DataQuery'; // exponer desde atributo de componente
            const port = '44311';
            const apiMethod = `${this.controller}/${this.dataSource}`;
            //const apiUrl = `${baseUrl}:${port}/${this.controllerName}/${this.dataSource}`;
            const apiUrl = `${baseUrl}:${port}/${apiMethod}`;

            const token = this.getCookie('app-access-token');
             
            const response = await fetch(apiUrl, {
                method: "GET",         
                credentials: "include",
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`
                },
            });

            if (!response.ok) throw new Error("Error al obtener los datos");

            const data = await response.json();
            this.fullData = data;
            this.filteredData = [...data];

            this.buildHeader(data);
        }
        catch (err) {
            console.error("Error cargando datos:", err);
            const tbody = this.shadowRoot.querySelector("#table-body");
            tbody.innerHTML = `<tr><td colspan="100%" class="text-center text-danger">Error al cargar los datos</td></tr>`;
        }
    }

    // Crear cabecera dinámica
    buildHeader(data) {
        if (!data || data.length === 0) return;

        const headers = Object.keys(data[0]);
        const theadRow = this.shadowRoot.querySelector("#thead tr");

        theadRow.innerHTML = headers
            .map(h => `
        <th style="text-align:center; color: black;"
            class="sorting"
            aria-controls="table_name"
            aria-label="${h}: activate to sort column descending"
            aria-sort="none"
            data-column="${h}">
            ${h}
        </th>`
            )
            .join('');

        // Agregamos columna de acciones
        if (this.showActions) {
            theadRow.innerHTML += `
            <th style="width:130px; text-align:center;"></th>
        `;
        }

        const thElements = theadRow.querySelectorAll("th");
        thElements.forEach((th, index) => {
            th.dataset.index = index;

            let thLimpio = th.textContent.replace(/[\r\n]+/g, '').replace(/\s/g, '');
            th.textContent = this.capitalizarPrimeraLetra(thLimpio);

            th.addEventListener("click", () => {
                this.sortByColumn(index, th);
            });
        });
    }

    // Modificar nombre de la columna
    capitalizarPrimeraLetra(text)
    {
        if (!text || typeof text !== 'string') {
            return text;
        }
        const primeraLetra = text.charAt(0).toUpperCase();
        const restoDeLaCadena = text.slice(1).toLowerCase();
        return primeraLetra + restoDeLaCadena;
    }

    // Odenación por desde header columnas
    sortByColumn(columnIndex, thElement) {

        const theadRow = this.shadowRoot.querySelector("#thead tr");

        // Borrar clases de otros th
        theadRow.querySelectorAll("th").forEach(th => {
            th.classList.remove("sorting_asc", "sorting_desc");
            th.classList.add("sorting");  // reset
        });

        // Alternar ASC/DESC
        const currentSort = thElement.getAttribute("aria-sort");
        const newSort = currentSort === "ascending" ? "descending" : "ascending";

        thElement.setAttribute("aria-sort", newSort);

        // Aplicar la clase CSS correcta
        if (newSort === "ascending") {
            thElement.classList.remove("sorting_desc");
            thElement.classList.add("sorting_asc");
        } else {
            thElement.classList.remove("sorting_asc");
            thElement.classList.add("sorting_desc");
        }

        // Ordenar los datos
        this.filteredData.sort((a, b) => {
            const aVal = Object.values(a)[columnIndex];
            const bVal = Object.values(b)[columnIndex];

            if (aVal < bVal) return newSort === "ascending" ? -1 : 1;
            if (aVal > bVal) return newSort === "ascending" ? 1 : -1;
            return 0;
        });

        this.renderTable();
    }

    // Listeners del buscador y selector de tamaño
    setupEventHandlers() {
        // Input BUSCAR
        const searchInput = this.shadowRoot.querySelector('#table_name_filter input');
        searchInput.addEventListener('input', (e) => {
            this.searchText = e.target.value.toLowerCase();
            this.currentPage = 1;
            this.applyFilters();
        });

        // Selector "Mostrar X"
        const pageSizeSelect = this.shadowRoot.querySelector('#table_name_length select');
        pageSizeSelect.addEventListener('change', (e) => {
            this.pageSize = parseInt(e.target.value, 10);
            this.currentPage = 1;
            this.applyFilters();
        });

        // BOTONES PAGINACIÓN
        this.shadowRoot.querySelector("#table_name_previous a")
            .addEventListener("click", (e) => {
                e.preventDefault();
                if (this.currentPage > 1) {
                    this.currentPage--;
                    this.renderTable();
                }
            });

        this.shadowRoot.querySelector("#table_name_next a")
            .addEventListener("click", (e) => {
                e.preventDefault();
                const totalPages = Math.ceil(this.filteredData.length / this.pageSize);
                if (this.currentPage < totalPages) {
                    this.currentPage++;
                    this.renderTable();
                }
            });
    }

    // Aplicar filtros y reiniciar paginación
    applyFilters() {
        let data = [...this.fullData];

        // Filtro por texto
        if (this.searchText.trim() !== "") {
            data = data.filter(row =>
                Object.values(row).some(value =>
                    String(value ?? "").toLowerCase().includes(this.searchText)
                )
            );
        }

        this.filteredData = data;
        this.currentPage = 1;

        this.renderTable();
    }

    // Renderizar tabla + info + paginación
    renderTable() {
        const tbody = this.shadowRoot.querySelector("#table-body");
        tbody.innerHTML = "";

        const total = this.filteredData.length;
        const totalPages = Math.ceil(total / this.pageSize);

        const start = (this.currentPage - 1) * this.pageSize;
        const end = Math.min(start + this.pageSize, total);

        const pageData = this.filteredData.slice(start, end);

        pageData.forEach(row => {
            const tr = document.createElement("tr");

            tr.innerHTML = Object.values(row)
                .map(v => {
                    const text = (v ?? "").toString();
                    const short = text.length > 50 ? text.substring(0, 50) + "…" : text;
                    return `<td title="${text}">${short}</td>`;
                })
                .join('');

            // Agregar columna de acciones
            if (this.showActions) {
                tr.innerHTML += `
                <td style="text-align:center;">

                    <button type="button" class="btn btn-sm btn-default md-btn-flat btn-edit" >
                        View
                    </button>

                    <button type="button" class="btn icon-btn btn-sm btn-default md-btn-flat btn-delete">
                        <span>X</span>
                    </button>

                </td>
            `;
            }
            tbody.appendChild(tr);

            // --- Asignar listeners de acciones por fila ---
            const _id = Object.values(row)[0];
            const _nombre = Object.values(row)[1];

            if (this.showActions) {
                const btnEdit = tr.querySelector(".btn-edit");
                const btnDelete = tr.querySelector(".btn-delete");

                // EDITAR → redirección
                btnEdit.addEventListener("click", () => {
                    window.location.href = `editForm.html?id=${_id}`;
                });

                // ELIMINAR → disparar evento custom
                btnDelete.addEventListener("click", () => {
                    this.dispatchEvent(new CustomEvent("delete-request", {
                        detail: { id: _id, nombre: _nombre, controller: this.controllerName },
                        bubbles: true,
                        composed: true
                    }));
                });
            }
             
        });

        // Actualizar info y paginación (sin cambios)
        const info = this.shadowRoot.querySelector("#table_name_info");
        if (total === 0)
            info.textContent = "Mostrando 0 a 0 de 0 registros";
        else
            info.textContent = `Mostrando ${start + 1} a ${end} de ${total} registros`;

        const prevLi = this.shadowRoot.querySelector("#table_name_previous");
        const nextLi = this.shadowRoot.querySelector("#table_name_next");

        if (this.currentPage === 1) prevLi.classList.add("disabled");
        else prevLi.classList.remove("disabled");

        if (this.currentPage === totalPages || totalPages === 0) nextLi.classList.add("disabled");
        else nextLi.classList.remove("disabled");

        this.renderPageNumbers(totalPages);
    }
    
    // Crear <li> dinámicos con números de página
    renderPageNumbers(totalPages) {
        const ul = this.shadowRoot.querySelector("#table_name_paginate ul");

        // borrar números antiguos
        ul.querySelectorAll("li.page-number").forEach(li => li.remove());

        const nextBtn = this.shadowRoot.querySelector("#table_name_next");

        for (let i = 1; i <= totalPages; i++) {
            const li = document.createElement("li");
            li.className = `paginate_button page-item page-number ${i === this.currentPage ? "active" : ""}`;

            li.innerHTML = `<a href="#" class="page-link">${i}</a>`;

            li.querySelector("a").addEventListener("click", (e) => {
                e.preventDefault();
                this.currentPage = i;
                this.renderTable();
            });

            nextBtn.before(li);
        }
    }
      
} 
customElements.define('bo-readonlylist', BOReadOnlyList);

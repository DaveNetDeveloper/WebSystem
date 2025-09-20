//
//
//

//
document.addEventListener('DOMContentLoaded', function () {
	var abrirModal = document.getElementById('abrirModal');
	var miModal = document.getElementById('miModal');
	var cerrarModal = document.getElementById('cerrarModal');

	abrirModal.addEventListener('click', function () {
		miModal.style.display = 'block';
	});

	cerrarModal.addEventListener('click', function () {
		miModal.style.display = 'none';
	});

	window.addEventListener('click', function (event) {
		if (event.target === miModal) {
			miModal.style.display = 'none';
		}
	});
});

//
function cargarContenidoModal(titulo, mensaje) {

	var pElement = document.getElementById('pModal');
	var h3Element = document.getElementById('h3Modal');

	pElement.textContent = mensaje;
	h3Element.textContent = titulo;


}
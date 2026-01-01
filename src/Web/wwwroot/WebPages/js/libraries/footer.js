
async function renderFooter(footerId) {

    const footer = document.getElementById(footerId);
    if (!footer) return;

    footer.innerHTML = `
         <div class="footer-top">
            <div class="container">
                <div class="row">
                    <div class="col-lg-5 col-md-6 col-12">
                        <div class="single-footer">
                            <h2>Sobre Nosotros</h2>
                            <p>Descubre más acerca de Appropat y conoce todo lo que puedes hacer en la plataforma.</p>
                            <ul class="social">
                                <li><a href="#"><i class="icofont-facebook"></i></a></li>
                                <li><a href="#"><i class="icofont-google-plus"></i></a></li>
                                <li><a href="#"><i class="icofont-instagram"></i></a></li>
                            </ul>
                        </div>
                    </div>
                    <div class="col-lg-3 col-md-6 col-12">
                        <div class="single-footer f-link">
                            <h2>Enlaces</h2> 
                                <div class="col-lg-12 col-md-12 col-12">
                                    <ul>
                                        <li><a href="home-privada.html"><i class="fa fa-caret-right" aria-hidden="true"></i> Inicio</a></li>
                                        <li><a href="explorar.html"><i class="fa fa-caret-right" aria-hidden="true"></i> Explorar</a></li>
                                        <li><a href="#"><i class="fa fa-caret-right" aria-hidden="true"></i> Recompensas</a></li>
                                        <li><a href="#"><i class="fa fa-caret-right" aria-hidden="true"></i> ¿Cómo ganar puntos?</a></li> 
                                    </ul>
                               </div>  
                        </div>
                    </div>
                    <div class="col-lg-4 col-md-6 col-12">
                        <div class="single-footer">
                            <h2>Newsletter</h2>
                            <p>Suscríbete para recibir todas nuestras noticias en tu bandeja de entrada y estar a la última.</p>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="copyright">
            <div class="container">
                <div class="row">
                    <div class="col-lg-12 col-md-12 col-12">
                        <div class="copyright-content">
                            <p>
                                © 2026 David Martínez ·
                                Esta obra está bajo una licencia
                                <a rel="license"
                                   href="https://creativecommons.org/licenses/by-sa/4.0/"
                                   target="_blank">
                                   Creative Commons Atribución-CompartirIgual 4.0 Internacional
                                </a>.
                              </p>
                        </div>
                    </div>
                </div>
            </div>
        </div> 
        `;
} 
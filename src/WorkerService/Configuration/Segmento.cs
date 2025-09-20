using WorkerService.Common;

namespace WorkerService.Configuration
{
    public class Segmento
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public TipoSegmento TipoSegmentacion { get; set; }
        public ReglaSegmento Regla { get; set; }
    } 

}
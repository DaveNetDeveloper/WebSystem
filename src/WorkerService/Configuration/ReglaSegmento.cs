namespace WorkerService.Configuration
{
    public class ReglaSegmento
    {
        public string Campo { get; set; } 
        public string Operador { get; set; }
        public string Valor { get; set; }
    }

    public enum OperadorReglaSegmentacion
    {
        Between,
        HigtherOrEqual,
        LowerOrEqual,
        Higther,
        Lower,
        In,
        NotIn,
        Equal,
        Distinct,
        Contains,
        NotContains
    }
}
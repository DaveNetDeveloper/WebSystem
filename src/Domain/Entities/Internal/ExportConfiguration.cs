namespace Domain.Entities
{
    public class ExportConfiguration
    {
        public string ExcelContentType { get; set; }
        //public string WordContentType { get; set; }
        public string PdfContentType { get; set; }
        public string PdfExtension{ get; set; }
        public string ExcelExtension { get; set; }
        public string CorreoAdmin { get; set; }
    }
}
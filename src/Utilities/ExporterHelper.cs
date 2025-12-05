using System.Data;
using System.Text;

namespace Utilities
{
    public static class ExporterHelper {

        public enum ExportFormat { Excel, Pdf }


        public static void ExportManager(DataTable sourceDataTable, object Response)
        {
            //GridView gridToExport = new GridView
            //{
            //    DataSource = sourceDataTable
            //};
            //gridToExport.DataBind();

            //foreach (TableCell cell in gridToExport.HeaderRow.Cells)
            //{
            //    cell.BackColor = Color.DeepSkyBlue;
            //}
            //gridToExport.EnableViewState = false;

            //StringBuilder sb = new StringBuilder();
            //StringWriter sw = new StringWriter(sb);
            //Page pagina = new Page();
            //pagina.EnableEventValidation = false;
            //pagina.DesignerInitialize();
            //HtmlForm form = new HtmlForm();
            //pagina.Controls.Add(form);
            //form.Controls.Add(gridToExport);
            //HtmlTextWriter htw = new HtmlTextWriter(sw);
            //pagina.RenderControl(htw);

            //Response.Clear();
            //Response.Buffer = true;
            //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //Response.AddHeader("Content-Disposition", "attachment;filename=DocumentsList.xls");
            //Response.Charset = "UTF-8";
            //Response.ContentEncoding = Encoding.Default;
            //Response.Write(sb.ToString());
            //Response.End();
        }
        public static void ExportPdf(DataTable sourceDataTable, object Response)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<html><head></head><body><table><tr><td><img src=\"http://gps.geoconecta.com/gps/imagenes/integradores/geoConecta/geoConectaLogoAccesoWeb.png\"></td></tr></table><br/><br/><br/><br/><br/>");
            sb.Append("<table border=\"2px\">");
            sb.Append("<tr>");
            foreach (System.Data.DataColumn dc in sourceDataTable.Columns)
            {
                sb.Append("<td>" + dc.ColumnName + "</td>");
            }
            sb.Append("</tr>");

            foreach (System.Data.DataRow dr in sourceDataTable.Rows)
            {
                sb.Append("<tr>");
                foreach (System.Data.DataColumn dc in sourceDataTable.Columns)
                {
                    sb.Append("<td>" + dr[dc].ToString() + "</td>");
                }
                sb.Append("</tr>");
            }
            sb.Append("</table></body></html>");

            ////var pdfBytes = (new NReco.PdfGenerator.HtmlToPdfConverter()).GeneratePdf("nombreABC");

            //Response.AddHeader("Content-Disposition", "attachment; filename=nombreABC" + "_" + DateTime.Now.ToShortDateString() + ".pdf");
            //Response.ContentType = "application/pdf";

            //Response.Clear();
            //Response.Buffer = true;
            ////return File.Open(pdfBytes, "application/pdf"));

            //FileInfo file = new System.IO.FileInfo("C:/TEMP/nombreABC.pdf");
            //Response.Write(file.Create());
            //Response.End();
        }
    }
}
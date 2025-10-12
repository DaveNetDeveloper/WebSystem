using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;

using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Text;
using Utilities;

using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Collections;
using System.Reflection;

namespace Application.Services
{
    public class PdfExporter : IExporter
    {
        public byte[] ExportDynamic(IEnumerable data, Type entityType)
        {
            var method = typeof(PdfExporter).GetMethod(nameof(Export));
            var genericMethod = method!.MakeGenericMethod(entityType);
            return (byte[])genericMethod.Invoke(this, new object[] { data, entityType.Name })!;
        }

        public byte[] Export<T>(IEnumerable<T> data, string title)
        {
            var items = data.Cast<object>().ToList();
            if (!items.Any())
                return Array.Empty<byte>();

            var props = items.First().GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);
                    page.Size(PageSizes.A4);
                    page.Header()
                        .Text(title)
                        .SemiBold().FontSize(16).AlignCenter();

                    page.Content().Table(table =>
                    {
                        // Encabezados
                        table.ColumnsDefinition(c =>
                        {
                            foreach (var _ in props)
                                c.RelativeColumn();
                        });

                        table.Header(header =>
                        {
                            foreach (var p in props)
                                header.Cell().Text(p.Name).Bold().FontSize(10);
                        });

                        // Datos
                        foreach (var item in items)
                        {
                            foreach (var p in props)
                            {
                                var value = p.GetValue(item)?.ToString() ?? "";
                                table.Cell().Text(value).FontSize(9);
                            }
                        }
                    });

                    page.Footer().AlignCenter().Text(txt =>
                    {
                        txt.Span("Generado el ");
                        txt.Span($"{DateTime.Now:dd/MM/yyyy HH:mm}");
                    });
                });
            });

            byte[] pdfBytes =  document.GeneratePdf();
            return pdfBytes;
        }
    }
}
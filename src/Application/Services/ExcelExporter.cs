using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;

using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Text;
using Utilities;

using ClosedXML.Excel;
using System.Reflection;
using System.Collections;

namespace Application.Services
{
    public class ExcelExporter : IExcelExporter
    {
        public byte[] ExportToExcelDynamic(IEnumerable data, Type entityType)
        {
            var method = typeof(ExcelExporter).GetMethod(nameof(ExportToExcel));
            var genericMethod = method!.MakeGenericMethod(entityType);
            return (byte[])genericMethod.Invoke(this, new object[] { data, entityType.Name })!;
        }

        public byte[] ExportToExcel<T>(IEnumerable<T> data, string sheetName)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add(sheetName);

            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            // Cabeceras
            for (int i = 0; i < properties.Length; i++)
            {
                worksheet.Cell(1, i + 1).Value = properties[i].Name;
                worksheet.Cell(1, i + 1).Style.Font.Bold = true;
            }

            // Datos
            int row = 2;
            foreach (var item in data)
            {
                for (int col = 0; col < properties.Length; col++)
                {
                    object? val = properties[col].GetValue(item, null);

                    if (val == null)
                    {
                        worksheet.Cell(row, col + 1).Value = "";
                        continue;
                    }

                    switch (Type.GetTypeCode(val.GetType()))
                    {
                        case TypeCode.Int32:
                        case TypeCode.Int64:
                        case TypeCode.Decimal:
                        case TypeCode.Double:
                        case TypeCode.Single:
                            worksheet.Cell(row, col + 1).Value = Convert.ToDouble(val);
                            break;

                        case TypeCode.Boolean:
                            worksheet.Cell(row, col + 1).Value = (bool)val;
                            break;

                        case TypeCode.DateTime:
                            worksheet.Cell(row, col + 1).Value = (DateTime)val;
                            worksheet.Cell(row, col + 1).Style.DateFormat.Format = "yyyy-MM-dd HH:mm:ss";
                            break;

                        case TypeCode.String:
                            worksheet.Cell(row, col + 1).Value = val.ToString();
                            break;

                        default:
                            // Para Guid y cualquier otro tipo
                            worksheet.Cell(row, col + 1).Value = val.ToString();
                            break;
                    }
                }
                row++;
            }
            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }
    }
}
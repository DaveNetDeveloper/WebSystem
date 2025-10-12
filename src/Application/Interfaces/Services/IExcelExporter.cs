using Application.DTOs.Filters;
using Application.Interfaces.Common;
using Application.Services;
using Domain.Entities;

using System.Collections;
namespace Application.Interfaces.Services
{
    public interface IExcelExporter
    {
        byte[] ExportToExcelDynamic(IEnumerable data, Type entityType);
        byte[] ExportToExcel<T>(IEnumerable<T> data, string sheetName);
    }
}
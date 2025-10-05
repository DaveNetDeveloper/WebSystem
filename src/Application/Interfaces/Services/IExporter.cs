using Application.DTOs.Filters;
using Application.Interfaces.Common;
using Application.Services;
using Domain.Entities;

using System.Collections;
namespace Application.Interfaces.Services
{
    public interface IExporter
    {
        byte[] ExportDynamic(IEnumerable data, Type entityType);
        byte[] Export<T>(IEnumerable<T> data, string sheetName);
    }
}
using Application.DTOs.Filters;
using Application.Interfaces.Common;
using Application.Services;
using Domain.Entities;
using static Utilities.ExporterHelper;
using static Application.Services.DataQueryService;

namespace Application.Interfaces.Services
{
    public interface IExportService
    {
        Task<byte[]> ExportarAsync(DataQueryType staQuery, ExportFormat formato);
        Task<byte[]> ExportarAsync(ExportFormat formato);
    }
}
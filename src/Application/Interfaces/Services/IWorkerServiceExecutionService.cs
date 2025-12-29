using Application.Common;
using Application.DTOs.Filters;
using Application.DTOs.Responses; 
using Application.Interfaces.Common;
using Domain.Entities;
using static Utilities.ExporterHelper;

namespace Application.Interfaces.Services
{
    public interface IWorkerServiceExecutionService : IService<WorkerServiceExecution, Guid>
    {

        Task<byte[]> ExportarAsync(ExportFormat formato);
    }
}
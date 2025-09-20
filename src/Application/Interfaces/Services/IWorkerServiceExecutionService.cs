using Application.Common;
using Application.DTOs.Filters;
using Application.DTOs.Responses; 
using Application.Interfaces.Common;
using Domain.Entities;

namespace Application.Interfaces.Services
{
    public interface IWorkerServiceExecutionService : IService<WorkerServiceExecution, Guid>
    {
         
    }
}
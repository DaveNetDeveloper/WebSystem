﻿using Application.DTOs.Filters;
using Application.DTOs.Responses;
using Application.Interfaces.Common;
using Domain.Entities;

namespace Application.Interfaces.Services
{
    public interface ICampanaExecutionService : IService<CampanaExecution, Guid>
    {
        Task<IEnumerable<CampanaExecution>> GetByFiltersAsync(CampanaExecutionFilters filters,
                                                              IQueryOptions<CampanaExecution>? queryOptions = null); 
    }
}
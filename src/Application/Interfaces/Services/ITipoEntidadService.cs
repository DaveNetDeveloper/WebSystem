﻿using Application.DTOs.Filters;
using Application.Interfaces.Common;
using Domain.Entities;

namespace Application.Interfaces.Services
{
    public interface ITipoEntidadService : IService<TipoEntidad, Guid>
    {
        Task<IEnumerable<TipoEntidad>> GetByFiltersAsync(TipoEntidadFilters filters,
                                                     IQueryOptions<TipoEntidad>? queryOptions = null);
    }
}
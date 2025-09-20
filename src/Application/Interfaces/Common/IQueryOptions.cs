using Application;
using Application.Interfaces;
using Application.Interfaces.Common;
using Domain.Entities; 

namespace Application.Interfaces.Common
{
    public interface IQueryOptions<TEntity>
    {
        int? Page { get; }
        int? PageSize { get; }
        string? OrderBy { get; }
        bool Descending { get; }

        bool HasPagination { get; }
        bool HasOrdering { get; }
    }
}
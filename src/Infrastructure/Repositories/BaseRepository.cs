using Application.Interfaces.Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class BaseRepository<TEntity>
    {    
        public IQueryable<TEntity> ApplyOrdening(IQueryable<TEntity> query, IQueryOptions<TEntity>? options) 
        { 
            // Ordenación
            if (options?.HasOrdering == true) {
                query = options.Descending
                    ? query.OrderByDescending(e => EF.Property<object>(e, options.OrderBy!))
                    : query.OrderBy(e => EF.Property<object>(e, options.OrderBy!));
            }
            return query;
        }

        public IQueryable<TEntity> ApplyPagination(IQueryable<TEntity> query, IQueryOptions<TEntity>? options)
        {
            //New

            // aplicar paginación (solo si options está informado)
            if (options != null && options.Page > 0 && options.PageSize > 0) {
                query = query
                    .Skip((options.Page.Value - 1) * options.PageSize.Value)
                    .Take(options.PageSize.Value);
            }   
            return query;
        }
    }
} 
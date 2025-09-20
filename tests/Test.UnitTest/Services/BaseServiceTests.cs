using Application.Common;
using Application.Interfaces.Services;
using Application.Interfaces.Common;

namespace Test.UnitTest.Services
{
    public class BaseServiceTests<TEntity>
    {
        protected IQueryOptions<TEntity> GetQueryOptions(int? page, int? pageSize, string? orderBy, bool descending = false)
        {
            return new QueryOptions<TEntity>
            {
                Page = page,
                PageSize = pageSize,
                OrderBy = orderBy,
                Descending = descending
            };
        } 
    }
}
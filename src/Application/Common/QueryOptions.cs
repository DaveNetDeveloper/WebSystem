using Application.Interfaces.Common;

namespace Application.Common
{
    public class QueryOptions<TEntity> : IQueryOptions<TEntity>
    {
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public string? OrderBy { get; set; }
        public bool Descending { get; set; } = false;

        public bool HasPagination => Page.HasValue && PageSize.HasValue;
        public bool HasOrdering => !string.IsNullOrEmpty(OrderBy);
    }
}
using Application.Common;
using Application.DTOs.Filters;
using Application.Interfaces.Common;
using Domain.Entities;

namespace Application.Interfaces.Services
{
    public interface ILoginService : IService<Login, Guid>
    {
        static class LoginType
        {
            public const string Web = "Web";
            public const string Admin = "Admin";
        }

        Task<IEnumerable<Login>> GetByFiltersAsync(LoginFilters filters,
                                                   IQueryOptions<Login>? queryOptions = null);
    }
}
using Application.Common;
using Application.DTOs.Filters;
using Application.Interfaces.Common;
using Domain.Entities;
using static Utilities.ExporterHelper;

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
        Task<byte[]> ExportarAsync(ExportFormat formato);
    }
}
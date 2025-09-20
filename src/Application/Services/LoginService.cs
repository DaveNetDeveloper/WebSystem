using Application.DTOs.Filters;
using Application.Interfaces;
using Application.Interfaces.Common;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;

namespace Application.Services
{
    public class LoginService : ILoginService
    {
        private readonly ILoginRepository _repo;
        
        public LoginService(ILoginRepository repo)
        {
            _repo = repo;
        }

        public Task<IEnumerable<Login>> GetByFiltersAsync(LoginFilters filters,
                                                          IQueryOptions<Login>? queryOptions = null)
         => _repo.GetByFiltersAsync(filters, queryOptions);

        public Task<Login?> GetByIdAsync(Guid id)
            => _repo.GetByIdAsync(id);

        public Task<IEnumerable<Login>> GetAllAsync()
            => _repo.GetAllAsync(); 

        public Task<bool> AddAsync(Login login)
            => _repo.AddAsync(login);

        public Task<bool> UpdateAsync(Login login)
            => _repo.UpdateAsync(login);

        public Task<bool> Remove(Guid id)
              => _repo.Remove(id); 
    }
}
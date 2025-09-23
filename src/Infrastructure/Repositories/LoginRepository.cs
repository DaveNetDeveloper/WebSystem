using Application.DTOs.Filters;
using Application.Interfaces.Common;
using Application.Interfaces.DTOs.Filters;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence;

using LinqKit;
using Microsoft.EntityFrameworkCore;
using System.Linq;


namespace Infrastructure.Repositories
{
    public class LoginRepository : BaseRepository<Login>, ILoginRepository
    {
        private readonly ApplicationDbContext _context;

        public LoginRepository(ApplicationDbContext context) {
            _context = context;
        }

        public async Task<IEnumerable<Login>> GetByFiltersAsync(IFilters<Login> filters,
                                                               IQueryOptions<Login>? options = null)
        {
            LoginFilters _filters = ((LoginFilters)filters);

            var predicate = PredicateBuilder.New<Login>(true);

            if (_filters.Id.HasValue)
                predicate = predicate.And(u => u.id == _filters.Id.Value);

            if (_filters.IdUsuario.HasValue)
                predicate = predicate.And(u => u.idUsuario == _filters.IdUsuario);

            var query = _context.Logins
                            .AsExpandable()
                            .Where(predicate);

            query = ApplyOrdening(query, options);
            query = ApplyPagination(query, options);
            return await query.ToListAsync();
        }

        public async Task<Login?> GetByIdAsync(Guid id) =>
            await _context.Logins.FindAsync(id);

        public async Task<IEnumerable<Login>> GetAllAsync() =>
            await _context.Logins.ToListAsync();

        public async Task<bool> AddAsync(Login login) {
             
            var nuevoLogin = new Login {
                id = Guid.NewGuid(),
                idUsuario = login.idUsuario,
                fecha = login.fecha,
                plataforma = login.plataforma,
                tipoDispositivo = login.tipoDispositivo,
                modeloDispositivo = login.modeloDispositivo,
                sistemaOperativo = login.sistemaOperativo,
                browser = login.browser,
                ip = login.ip,
                pais = login.pais,
                region = login.region,
                idiomaNavegador = login.idiomaNavegador
            };
             
            await _context.Logins.AddAsync(nuevoLogin);
            await _context.SaveChangesAsync();
            return true;
        } 

        public async Task<bool> UpdateAsync(Login login)
        {
            var updatedLogin = await _context.Logins.Where(r => r.id == login.id).SingleOrDefaultAsync();
            if (updatedLogin == null)
                return false;

            updatedLogin.idUsuario = login.idUsuario; 

            await _context.SaveChangesAsync();
            return true;
        } 

        public async Task<bool> Remove(Guid id)
        {
            var loginToDelete = await _context.Logins.FindAsync(id);

            if (loginToDelete == null)
                return false;

            _context.Logins.Remove(loginToDelete);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

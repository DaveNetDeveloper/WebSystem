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
    public class FAQRepository : BaseRepository<FAQ>, IFAQRepository
    {
        private readonly ApplicationDbContext _context;
         
        public FAQRepository(ApplicationDbContext context) {
            _context = context;
        }

        public async Task<IEnumerable<FAQ>> GetAllAsync() =>
           await _context.FAQS.ToListAsync();

        public async Task<IEnumerable<FAQ>> GetByFiltersAsync(IFilters<FAQ> filters,
                                                                  IQueryOptions<FAQ>? options = null)
        {
            FAQFilters _filters = ((FAQFilters)filters);

            var predicate = PredicateBuilder.New<FAQ>(true);

            if (_filters.Id.HasValue)
                predicate = predicate.And(u => u.id == _filters.Id.Value);

            var query = _context.FAQS
                            .AsExpandable()
                            .Where(predicate);

            query = ApplyOrdening(query, options);
            query = ApplyPagination(query, options);
            return await query.ToListAsync();
        }

        public async Task<FAQ?> GetByIdAsync(Guid id) =>
            await _context.FAQS.FindAsync(id);

        public async Task<bool> AddAsync(FAQ faq)
        {
            var nuevaFaq = new FAQ {
                id =Guid.NewGuid(),
                orden = faq.orden,
                pregunta = faq.pregunta,
                respuesta = faq.respuesta
            };

            await _context.FAQS.AddAsync(nuevaFaq);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(FAQ faq)
        {
            var updatedFaq = await _context.FAQS.Where(c => c.id == faq.id).SingleOrDefaultAsync();
            if (updatedFaq == null)
                return false;

            updatedFaq.orden = faq.orden;
            updatedFaq.pregunta = faq.pregunta;
            updatedFaq.respuesta = faq.respuesta;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Remove(Guid id)
        { 
            var faqToDelete = await _context.FAQS.FindAsync(id);

            if (faqToDelete == null)
                return false;

            _context.FAQS.Remove(faqToDelete);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

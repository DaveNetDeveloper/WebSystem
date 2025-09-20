using Application.Common;
using Application.DTOs.Filters;
using Application.Interfaces.Common;
using Application.Interfaces.DTOs.Filters;
using Application.Interfaces.Repositories;
using Infrastructure.Persistence;
using Domain.Entities;
 
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using LinqKit;

namespace Infrastructure.Repositories
{
    public class WorkerServiceExecutionRepository : BaseRepository<WorkerServiceExecution>, IWorkerServiceExecutionRepository
    {
        private readonly ApplicationDbContext _context;

        public WorkerServiceExecutionRepository(ApplicationDbContext context) {
            _context = context;
        } 

        public async Task<WorkerServiceExecution?> GetByIdAsync(Guid id) =>
            await _context.WorkerServiceExecutions.FindAsync(id);
         
        public async Task<IEnumerable<WorkerServiceExecution>> GetAllAsync() =>
            await _context.WorkerServiceExecutions.ToListAsync();

        public async Task<bool> AddAsync(WorkerServiceExecution workerServiceExecution)
        {
            var _workerServiceExecution = new WorkerServiceExecution {
                id = new Guid(),
                workerService = workerServiceExecution.workerService,
                result = workerServiceExecution.result,
                resultDetailed = workerServiceExecution.resultDetailed,
                executionTime = workerServiceExecution.executionTime
            };

            await _context.WorkerServiceExecutions.AddAsync(_workerServiceExecution);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateAsync(WorkerServiceExecution workerServiceExecution)
        {
            var _workerServiceExecution = await _context.WorkerServiceExecutions.Where(a => a.id == workerServiceExecution.id).SingleOrDefaultAsync();
            if (_workerServiceExecution == null)
                return false;

            _workerServiceExecution.workerService = workerServiceExecution.workerService;
            _workerServiceExecution.result = workerServiceExecution.result;
            _workerServiceExecution.resultDetailed = workerServiceExecution.resultDetailed;
            _workerServiceExecution.executionTime = workerServiceExecution.executionTime;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Remove(Guid id) {

            var workerServiceToDelete = await _context.WorkerServiceExecutions.FindAsync(id);

            if (workerServiceToDelete == null)
                return false;

            _context.WorkerServiceExecutions.Remove(workerServiceToDelete);
            await _context.SaveChangesAsync();
            return true;
        }
          
    }
} 
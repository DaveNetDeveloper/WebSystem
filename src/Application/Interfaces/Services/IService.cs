using Domain.Entities;
using static Utilities.ExporterHelper;

namespace Application.Interfaces.Services
{
    /// <summary>
    /// Interfaz generica para todos los servicios
    /// </summary>
    public interface IService<TEntity, TKey>
    {
        //Task<IEnumerable<TEntity>> GetByFiltersAsync(IFilters<TEntity> filters);
        
        /// <summary>
        /// Devuelve todas las entidades
        /// </summary> 
        Task<IEnumerable<TEntity>> GetAllAsync(); 

        /// <summary>
        /// Devuelve la entidad con el Id especificado
        /// </summary> 
        Task<TEntity?> GetByIdAsync(TKey id);

        /// <summary>
        /// Crea una nueva entidad 
        /// </summary> 
        Task<bool> AddAsync(TEntity entidad);

        /// <summary>
        /// Actualiza la entidad afectada
        /// </summary> 
        Task<bool> UpdateAsync(TEntity entidad);

        /// <summary>
        /// Elimina la entidad con el Id especificado
        /// </summary> 
        Task<bool> Remove(TKey id);

        
    }
}
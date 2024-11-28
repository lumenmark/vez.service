using Microsoft.EntityFrameworkCore;
using UsaWeb.Service.Data;
using UsaWeb.Service.Features.QrtCaseMeetingOfiFeature.Abstractions;
using UsaWeb.Service.Models;
using UsaWeb.Service.ViewModels;

namespace UsaWeb.Service.Features.QrtCaseMeetingFeature.Implementations
{
    /// <summary>
    /// Qrt Case Meeting Repository.
    /// </summary>
    public class QrtCaseMeetingOfiRepository(Usaweb_DevContext context) : IQrtCaseMeetingOfiRepository
    {
        /// <summary>
        /// The context.
        /// </summary>
        private readonly Usaweb_DevContext _context = context;

        /// <summary>
        /// Creates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public async Task<QrtCaseMeetingOfi> Create(QrtCaseMeetingOfi entity)
        {
            await _context.QrtCaseMeetingOfis.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }


        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public async Task Delete(QrtCaseMeetingOfi entity)
        {
            _context.Remove(entity);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        public async Task<IEnumerable<QrtCaseMeetingOfi>> GetAllAsync()
        {
            return await _context.QrtCaseMeetingOfis.ToListAsync();
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<QrtCaseMeetingOfi> GetAsync(int id)
        {
            return await _context.QrtCaseMeetingOfis.FirstOrDefaultAsync(_ => _.QrtCaseMeetingOfiId == id);
        }

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public async Task<QrtCaseMeetingOfi> Update(QrtCaseMeetingOfi entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}

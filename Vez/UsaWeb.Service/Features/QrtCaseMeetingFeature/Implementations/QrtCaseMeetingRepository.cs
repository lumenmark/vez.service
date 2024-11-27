using Microsoft.EntityFrameworkCore;
using UsaWeb.Service.Data;
using UsaWeb.Service.Features.QrtCaseMeetingFeature.Abstractions;
using UsaWeb.Service.Models;
using UsaWeb.Service.ViewModels;

namespace UsaWeb.Service.Features.QrtCaseMeetingFeature.Implementations
{
    /// <summary>
    /// Qrt Case Meeting Repository.
    /// </summary>
    public class QrtCaseMeetingRepository(Usaweb_DevContext context) : IQrtCaseMeetingRepository
    {
        /// <summary>
        /// The context.
        /// </summary>
        private readonly Usaweb_DevContext _context = context;

        /// <summary>
        /// Creates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public async Task<QrtCaseMeeting> Create(QrtCaseMeeting entity)
        {
            await _context.QrtCaseMeetings.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public async Task Delete(QrtCaseMeeting entity)
        {
            _context.Remove(entity);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        public async Task<IEnumerable<QrtCaseMeeting>> GetAllAsync()
        {
            return await _context.QrtCaseMeetings.OrderByDescending(_ => _.MeetingDt).ToListAsync();
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<QrtCaseMeeting> GetAsync(int id)
        {
            return await _context.QrtCaseMeetings.FirstOrDefaultAsync(_ => _.QrtCaseMeetingId == id);
        }

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public async Task<QrtCaseMeeting> Update(QrtCaseMeeting entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}

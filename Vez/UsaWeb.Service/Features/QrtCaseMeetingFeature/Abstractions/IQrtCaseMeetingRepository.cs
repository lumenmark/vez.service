using UsaWeb.Service.Models;
using UsaWeb.Service.ViewModels;

namespace UsaWeb.Service.Features.QrtCaseMeetingFeature.Abstractions
{
    public interface IQrtCaseMeetingRepository
    {
        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        Task<QrtCaseMeeting> GetAsync(int id);

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        Task<IEnumerable<QrtCaseMeeting>> GetAllAsync();

        /// <summary>
        /// Creates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        Task<QrtCaseMeeting> Create(QrtCaseMeeting entity);

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        Task<QrtCaseMeeting> Update(QrtCaseMeeting entity);

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        Task Delete(QrtCaseMeeting entity);

    }
}

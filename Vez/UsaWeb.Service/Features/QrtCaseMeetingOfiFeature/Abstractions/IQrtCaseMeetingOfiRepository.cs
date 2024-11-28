using UsaWeb.Service.Models;
using UsaWeb.Service.ViewModels;

namespace UsaWeb.Service.Features.QrtCaseMeetingOfiFeature.Abstractions
{
    public interface IQrtCaseMeetingOfiRepository
    {
        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        Task<QrtCaseMeetingOfi> GetAsync(int id);

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        Task<IEnumerable<QrtCaseMeetingOfi>> GetAllAsync();

        /// <summary>
        /// Creates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        Task<QrtCaseMeetingOfi> Create(QrtCaseMeetingOfi entity);

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        Task<QrtCaseMeetingOfi> Update(QrtCaseMeetingOfi entity);

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        Task Delete(QrtCaseMeetingOfi entity);
    }
}

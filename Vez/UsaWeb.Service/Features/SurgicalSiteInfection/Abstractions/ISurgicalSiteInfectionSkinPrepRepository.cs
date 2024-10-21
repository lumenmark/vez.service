using UsaWeb.Service.Models;

namespace UsaWeb.Service.Features.SurgicalSiteInfection.Abstractions
{
    /// <summary>
    /// Surgical Site Infection Skin Prep Repository Interface.
    /// </summary>
    public interface ISurgicalSiteInfectionSkinPrepRepository
    {
        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        Task<IEnumerable<SurgicalSiteInfectionSkinPrep>> GetAllAsync();

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        Task<SurgicalSiteInfectionSkinPrep> GetByIdAsync(int id);

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        Task AddAsync(SurgicalSiteInfectionSkinPrep entity);

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        Task UpdateAsync(SurgicalSiteInfectionSkinPrep entity);

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        Task DeleteAsync(int id);

        /// <summary>
        /// Gets the by surgical site infection identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<List<SurgicalSiteInfectionSkinPrep>> GetBySurgicalSiteInfectionIdAsync(int id);
    }
}

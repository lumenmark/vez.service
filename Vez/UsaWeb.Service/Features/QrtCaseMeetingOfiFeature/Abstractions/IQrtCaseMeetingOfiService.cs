using Microsoft.AspNetCore.JsonPatch;
using UsaWeb.Service.Models;
using UsaWeb.Service.ViewModels;

namespace UsaWeb.Service.Features.QrtCaseMeetingOfiFeature.Abstractions
{
    public interface IQrtCaseMeetingOfiService
    {
        /// <summary>
        /// Gets the by parameters.
        /// </summary>
        /// <param name="qrtCaseMeetingId">The QRT case meeting identifier.</param>
        /// <param name="qrtCaseMeetingOfiId">The QRT case meeting ofi identifier.</param>
        Task<IEnumerable<QrtCaseMeetingOfi>> GetByParams(int? qrtCaseMeetingId, int? qrtCaseMeetingOfiId);

        /// <summary>
        /// Creates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        Task<QrtCaseMeetingOfi> Create(QrtCaseMeetingOfiVM entity);

        /// <summary>
        /// Update a specified entity.
        /// </summary>
        Task<QrtCaseMeetingOfi> Update(QrtCaseMeetingOfiVM entity, int id);

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        Task<QrtCaseMeetingOfi> Delete(int id);

        /// <summary>
        /// Patches the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="patchDoc">The patch document.</param>
        Task<QrtCaseMeetingOfi> Patch(int id, JsonPatchDocument<QrtCaseMeetingOfiVM> patchDoc);
    }
}

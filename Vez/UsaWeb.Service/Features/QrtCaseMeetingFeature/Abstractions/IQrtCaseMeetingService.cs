using Microsoft.AspNetCore.JsonPatch;
using UsaWeb.Service.Models;
using UsaWeb.Service.ViewModels;

namespace UsaWeb.Service.Features.QrtCaseMeetingFeature.Abstractions
{
    public interface IQrtCaseMeetingService
    {
        /// <summary>
        /// Gets the by parameters.
        /// </summary>
        /// <param name="qrtCaseMeetingId">The QRT case meeting identifier.</param>
        /// <param name="qrtCaseId">The QRT case identifier.</param>
        Task<IEnumerable<QrtCaseMeeting>> GetByParams(int? qrtCaseMeetingId, int? qrtCaseId);

        /// <summary>
        /// Creates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        Task<QrtCaseMeeting> Create(QrtCaseMeetingVM entity);

        /// <summary>
        /// Update a specified entity.
        /// </summary>
        Task<QrtCaseMeeting> Update(QrtCaseMeetingVM entity, int id);

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        Task<QrtCaseMeeting> Delete(int id);

        /// <summary>
        /// Patches the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="patchDoc">The patch document.</param>
        Task<QrtCaseMeeting> Patch(int id, JsonPatchDocument<QrtCaseMeetingVM> patchDoc);
    }
}

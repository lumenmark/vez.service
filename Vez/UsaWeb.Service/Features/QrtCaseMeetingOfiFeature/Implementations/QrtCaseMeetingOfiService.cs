using Microsoft.AspNetCore.JsonPatch;
using UsaWeb.Service.Features.Extensions;
using UsaWeb.Service.Features.QrtCaseMeetingFeature.Abstractions;
using UsaWeb.Service.Features.QrtCaseMeetingOfiFeature.Abstractions;
using UsaWeb.Service.Models;
using UsaWeb.Service.ViewModels;

namespace UsaWeb.Service.Features.QrtCaseMeetingFeature.Implementations
{
    /// <summary>
    /// Qrt Case Meeting Service.
    /// </summary>
    public class QrtCaseMeetingOfiService(IQrtCaseMeetingOfiRepository qrtCaseMeetingRepository) : IQrtCaseMeetingOfiService
    {
        /// <summary>
        /// The QRT case meeting repository.
        /// </summary>
        private readonly IQrtCaseMeetingOfiRepository _qrtCaseMeetingOfiRepository = qrtCaseMeetingRepository;

        /// <summary>
        /// Creates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public async Task<QrtCaseMeetingOfi> Create(QrtCaseMeetingOfiVM entity)
        {
            var qtrCaseMeeting = entity.ToEntity();
            var createdEntity = await _qrtCaseMeetingOfiRepository.Create(qtrCaseMeeting);
            return createdEntity;
        }

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="id"></param>
        public async Task<QrtCaseMeetingOfi> Delete(int id)
        {
            var entity = await _qrtCaseMeetingOfiRepository.GetAsync(id);
            if (entity == null)
            {
                return null;
            }

            await _qrtCaseMeetingOfiRepository.Delete(entity);
            return entity;
        }

        /// <summary>
        /// Gets the by parameters.
        /// </summary>
        /// <param name="qrtCaseMeetingId">The QRT case meeting identifier.</param>
        /// <param name="qrtCaseMeetingOfiId">The QRT case meeting ofi identifier.</param>
        public async Task<IEnumerable<QrtCaseMeetingOfi>> GetByParams(int? qrtCaseMeetingId, int? qrtCaseMeetingOfiId)
        {
            var qrtMeetings = await _qrtCaseMeetingOfiRepository.GetAllAsync();

            if (qrtCaseMeetingId.HasValue)
            {
                qrtMeetings = qrtMeetings.Where(_ => _.QrtCaseMeetingId == qrtCaseMeetingId).ToList();
            }

            if (qrtCaseMeetingOfiId.HasValue)
            {
                qrtMeetings = qrtMeetings.Where(_ => _.QrtCaseMeetingOfiId == qrtCaseMeetingOfiId).ToList();
            }

            return qrtMeetings;
        }

        /// <summary>
        /// Patches the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="patchDoc">The patch document.</param>
        public async Task<QrtCaseMeetingOfi> Patch(int id, JsonPatchDocument<QrtCaseMeetingOfiVM> patchDoc)
        {
            ArgumentNullException.ThrowIfNull(patchDoc);

            var entity = await _qrtCaseMeetingOfiRepository.GetAsync(id);
            if (entity == null)
                return null;

            var model = entity.ToViewModel();
            patchDoc.ApplyTo(model);

            entity.UpdateEntity(model);
            var updatedEntity = await _qrtCaseMeetingOfiRepository.Update(entity);
            return updatedEntity;
        }

        /// <summary>
        /// Update a specified entity.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="id"></param>
        public async Task<QrtCaseMeetingOfi> Update(QrtCaseMeetingOfiVM entity, int id)
        {
            var qrtCaseMeetingOfi = await _qrtCaseMeetingOfiRepository.GetAsync(id);
            if (qrtCaseMeetingOfi == null)
            {
                return null;
            }

            qrtCaseMeetingOfi.UpdateEntity(entity);
            var updatedEntity = await _qrtCaseMeetingOfiRepository.Update(qrtCaseMeetingOfi);
            return updatedEntity;
        }
    }
}

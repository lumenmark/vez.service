using Microsoft.AspNetCore.JsonPatch;
using UsaWeb.Service.Features.Extensions;
using UsaWeb.Service.Features.QrtCaseMeetingFeature.Abstractions;
using UsaWeb.Service.Models;
using UsaWeb.Service.ViewModels;

namespace UsaWeb.Service.Features.QrtCaseMeetingFeature.Implementations
{
    /// <summary>
    /// Qrt Case Meeting Service.
    /// </summary>
    public class QrtCaseMeetingService(IQrtCaseMeetingRepository qrtCaseMeetingRepository) : IQrtCaseMeetingService
    {
        /// <summary>
        /// The QRT case meeting repository.
        /// </summary>
        private readonly IQrtCaseMeetingRepository _qrtCaseMeetingRepository = qrtCaseMeetingRepository;

        /// <summary>
        /// Creates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public async Task<QrtCaseMeeting> Create(QrtCaseMeetingVM entity)
        {
            var qtrCaseMeeting = entity.ToEntity();
            var createdEntity = await _qrtCaseMeetingRepository.Create(qtrCaseMeeting);
            return createdEntity;
        }

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="id"></param>
        public async Task<QrtCaseMeeting> Delete(int id)
        {
            var entity = await _qrtCaseMeetingRepository.GetAsync(id);
            if (entity == null)
            {
                return null;
            }

            await _qrtCaseMeetingRepository.Delete(entity);
            return entity;
        }

        /// <summary>
        /// Gets the by parameters.
        /// </summary>
        /// <param name="qrtCaseMeetingId">The QRT case meeting identifier.</param>
        /// <param name="qrtCaseId">The QRT case identifier.</param>
        public async Task<IEnumerable<QrtCaseMeeting>> GetByParams(int? qrtCaseMeetingId, int? qrtCaseId)
        {
            var qrtMeetings = await _qrtCaseMeetingRepository.GetAllAsync();

            if (qrtCaseMeetingId.HasValue)
            {
                qrtMeetings =qrtMeetings.Where(_ => _.QrtCaseMeetingId == qrtCaseMeetingId).ToList();
            }

            if (qrtCaseId.HasValue)
            {
                qrtMeetings = qrtMeetings.Where(_ => _.QrtCaseId == qrtCaseId).ToList();
            }

            return qrtMeetings;
        }

        /// <summary>
        /// Patches the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="patchDoc">The patch document.</param>
        public async Task<QrtCaseMeeting> Patch(int id, JsonPatchDocument<QrtCaseMeetingVM> patchDoc)
        {
            ArgumentNullException.ThrowIfNull(patchDoc);

            var entity = await _qrtCaseMeetingRepository.GetAsync(id);
            if (entity == null)
                return null;

            var model = entity.ToViewModel();
            patchDoc.ApplyTo(model);

            entity.UpdateFromViewModel(model);
            var updatedEntity = await _qrtCaseMeetingRepository.Update(entity);
            return updatedEntity;
        }

        /// <summary>
        /// Update a specified entity.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="id"></param>
        public async Task<QrtCaseMeeting> Update(QrtCaseMeetingVM entity, int id)
        {
            var qrtCaseMeeting = await _qrtCaseMeetingRepository.GetAsync(id);
            if (qrtCaseMeeting == null)
            {
                return null;
            }

            qrtCaseMeeting.UpdateFromViewModel(entity);
            var updatedEntity = await _qrtCaseMeetingRepository.Update(qrtCaseMeeting);
            return updatedEntity;
        }
    }
}

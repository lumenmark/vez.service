using Microsoft.AspNetCore.JsonPatch;
using UsaWeb.Service.Features.Extensions;
using UsaWeb.Service.Features.Requests;
using UsaWeb.Service.Features.Responses;
using UsaWeb.Service.Features.SurgicalSiteInfection.Abstractions;
using UsaWeb.Service.ViewModels;

namespace UsaWeb.Service.Features.SurgicalSiteInfection.Implementations
{
    public class SurgicalSiteInfectionService : ISurgicalSiteInfectionService
    {
        private readonly ISurgicalSiteInfectionRepository _repository;

        public SurgicalSiteInfectionService(ISurgicalSiteInfectionRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<SurgicalSiteInfectionResponse>> GetSurgicalSiteInfectionsAsync(SurgicalSiteInfectionRequest request)
        {
            return await _repository.GetSurgicalSiteInfectionsAsync(request);
        }

        public async Task<Models.SurgicalSiteInfection> CreateSurgicalSiteInfectionAsync(SurgicalSiteInfectionViewModel model)
        {
            if(model == null)
            {
                throw new ArgumentException("Please fill the form.");
            }

            var entity = await _repository.CreateSurgicalSiteInfectionAsync(model);
            return entity;
        }

        public async Task<Models.SurgicalSiteInfection> UpdateSurgicalSiteInfectionAsync(int id, SurgicalSiteInfectionViewModel model)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid ID");


            var existingEntity = await _repository.GetSurgicalSiteInfectionByIdAsync(id);
            if (existingEntity == null)
                return null;

            existingEntity.UpdateFromViewModel(model);

            var updatedEntity = await _repository.UpdateSurgicalSiteInfectionAsync(existingEntity);
            return updatedEntity;
        }

        public async Task<SurgicalSiteInfectionViewModel> PatchSurgicalSiteInfectionAsync(int id, JsonPatchDocument<SurgicalSiteInfectionViewModel> patchDoc)
        {
            ArgumentNullException.ThrowIfNull(patchDoc);

            var entity = await _repository.GetSurgicalSiteInfectionByIdAsync(id);
            if (entity == null)
                return null;

            var model = entity.ToViewModel();
            patchDoc.ApplyTo(model);

            entity.UpdateFromViewModel(model);
            var updatedEntity = await _repository.UpdateSurgicalSiteInfectionAsync(entity);
            return updatedEntity.ToViewModel();
        }
    }
}

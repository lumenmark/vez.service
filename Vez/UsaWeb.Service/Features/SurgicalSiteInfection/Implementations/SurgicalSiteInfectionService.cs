using Microsoft.AspNetCore.JsonPatch;
using UsaWeb.Service.Data;
using UsaWeb.Service.Features.Extensions;
using UsaWeb.Service.Features.Requests;
using UsaWeb.Service.Features.Responses;
using UsaWeb.Service.Features.SurgicalSiteInfection.Abstractions;
using UsaWeb.Service.Models;
using UsaWeb.Service.ViewModels;
using static Amazon.S3.Util.S3EventNotification;

namespace UsaWeb.Service.Features.SurgicalSiteInfection.Implementations
{
    public class SurgicalSiteInfectionService(ISurgicalSiteInfectionRepository repository,
        ISurgicalSiteInfectionSkinPrepRepository siteInfectionSkinPrepRepository) : ISurgicalSiteInfectionService
    {
        private readonly ISurgicalSiteInfectionRepository _repository = repository;
        private readonly ISurgicalSiteInfectionSkinPrepRepository _siteInfectionSkinPrepRepository = siteInfectionSkinPrepRepository;

        public async Task<IEnumerable<SurgicalSiteInfectionResponse>> GetSurgicalSiteInfectionsAsync(SurgicalSiteInfectionRequest request)
        {
            var surgicalSiteInfectionResponses =  await _repository.GetSurgicalSiteInfectionsAsync(request);

            foreach (var item in surgicalSiteInfectionResponses)
            {
                var skinPrep = await _siteInfectionSkinPrepRepository.GetBySurgicalSiteInfectionIdAsync(item.SurgicalSiteInfectionId);
                item.SurgicalSiteInfectionSkinPrep = string.Join(',', skinPrep.Select(sp => sp.SkinPrep));
            }

            return surgicalSiteInfectionResponses;
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

        public async Task<IEnumerable<NhsnProcedureCategory>> GetNhsnProcedureCategories()
        {
            return await _repository.GetNhsnProcedureCategories();
        }
    }
}
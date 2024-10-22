using UsaWeb.Service.Data;
using UsaWeb.Service.Features.Enums;
using UsaWeb.Service.Features.Responses;
using UsaWeb.Service.Features.SurgicalSiteInfection.Abstractions;
using UsaWeb.Service.Models;
using UsaWeb.Service.ViewModels;

namespace UsaWeb.Service.Features.SurgicalSiteInfection.Implementations
{
    public class SurgicalSiteInfectionSkinPrepService(ISurgicalSiteInfectionSkinPrepRepository repository) : ISurgicalSiteInfectionSkinPrepService
    {
        private readonly ISurgicalSiteInfectionSkinPrepRepository _repository = repository;

        public async Task<bool> CreateSkinPrepAsync(SkinPrepViewModel model)
        {
            try
            {
                var skinPrepsToDelete = await _repository.GetBySurgicalSiteInfectionIdAsync(model.SurgicalSiteInfectionId);
                foreach (var skinPrepToDelete in skinPrepsToDelete)
                {
                    await _repository.DeleteAsync(skinPrepToDelete.SurgicalSiteInfectionSkinPrepId);
                }

                if (string.IsNullOrEmpty(model.SkinPrep))
                {
                    return false;
                }

                var skinPreps = model.SkinPrep.Split(',').Select(ct => ct.Trim()).ToArray();
                foreach (var skinPrep in skinPreps)
                {
                    if (!Enum.TryParse<SkinPrepOptions>(skinPrep, true, out _))
                    {
                        continue;
                    }

                    var entity = new SurgicalSiteInfectionSkinPrep
                    {
                        SkinPrep = skinPrep,
                        CreateTs = DateTime.UtcNow,
                        SurgicalSiteInfectionId = model.SurgicalSiteInfectionId
                    };

                    await _repository.AddAsync(entity);
                }

                return true;
            }
            catch (Exception ex)
            {
                DBHelper.LogError("An error occurred while saving the data."+ ex.Message+ " InnerException " + ex.InnerException);
                return false;
            }
        }

        public Task<SurgicalSiteInfectionResponse> GetSkinPrepByIdAsync(int surgicalSiteInfectionId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SurgicalSiteInfectionResponse>> GetSkinPrepsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateSkinPrepAsync(SkinPrepViewModel model)
        {
            try
            {
                var skinPrepsToDelete = await _repository.GetBySurgicalSiteInfectionIdAsync(model.SurgicalSiteInfectionId);
                foreach (var skinPrepToDelete in skinPrepsToDelete)
                {
                    await _repository.DeleteAsync(skinPrepToDelete.SurgicalSiteInfectionSkinPrepId);
                }

                if (string.IsNullOrEmpty(model.SkinPrep))
                {
                    return false;
                }

                var skinPreps = model.SkinPrep.Split(',').Select(ct => ct.Trim()).ToArray();
                foreach (var skinPrep in skinPreps)
                {
                    if (!Enum.TryParse<SkinPrepOptions>(skinPrep, true, out _))
                    {
                        continue;
                    }

                    var entity = new SurgicalSiteInfectionSkinPrep
                    {
                        SkinPrep = skinPrep,
                        CreateTs = DateTime.UtcNow,
                        SurgicalSiteInfectionId = model.SurgicalSiteInfectionId
                    };

                    await _repository.AddAsync(entity);
                }

                return true;
            }
            catch (Exception ex)
            {
                DBHelper.LogError("An error occurred while saving the data." + ex.Message + " InnerException " + ex.InnerException);
                return false;
            }
        }
    }
}

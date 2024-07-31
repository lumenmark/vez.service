using Microsoft.AspNetCore.JsonPatch;
using UsaWeb.Service.Features.Requests;
using UsaWeb.Service.Features.Responses;
using UsaWeb.Service.ViewModels;

namespace UsaWeb.Service.Features.SurgicalSiteInfection.Abstractions
{
    public interface ISurgicalSiteInfectionService
    {
        Task<IEnumerable<SurgicalSiteInfectionResponse>> GetSurgicalSiteInfectionsAsync(SurgicalSiteInfectionRequest request);
        Task<Models.SurgicalSiteInfection> CreateSurgicalSiteInfectionAsync(SurgicalSiteInfectionViewModel model);
        Task<Models.SurgicalSiteInfection> UpdateSurgicalSiteInfectionAsync(int id, SurgicalSiteInfectionViewModel model);
        Task<SurgicalSiteInfectionViewModel> PatchSurgicalSiteInfectionAsync(int id, JsonPatchDocument<SurgicalSiteInfectionViewModel> patchDoc);
    }
}

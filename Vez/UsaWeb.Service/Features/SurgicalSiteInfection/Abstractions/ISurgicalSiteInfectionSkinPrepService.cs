using Microsoft.AspNetCore.JsonPatch;
using UsaWeb.Service.Features.Requests;
using UsaWeb.Service.Features.Responses;
using UsaWeb.Service.Models;
using UsaWeb.Service.ViewModels;

namespace UsaWeb.Service.Features.SurgicalSiteInfection.Abstractions
{
    public interface ISurgicalSiteInfectionSkinPrepService
    {
        Task<SurgicalSiteInfectionResponse> GetSkinPrepByIdAsync(int surgicalSiteInfectionId);
        Task<IEnumerable<SurgicalSiteInfectionResponse>> GetSkinPrepsAsync();
        Task<bool> CreateSkinPrepAsync(SkinPrepViewModel model);
        Task<bool> UpdateSkinPrepAsync(SkinPrepViewModel model);
    }
}
using UsaWeb.Service.Features.Requests;
using UsaWeb.Service.Features.Responses;
using UsaWeb.Service.Models;
using UsaWeb.Service.ViewModels;
namespace UsaWeb.Service.Features.SurgicalSiteInfection.Abstractions
{
    public interface ISurgicalSiteInfectionRepository
    {
        Task<IEnumerable<SurgicalSiteInfectionResponse>> GetSurgicalSiteInfectionsAsync(SurgicalSiteInfectionRequest request);
        Task<IEnumerable<Models.SurgicalSiteInfection>> GetAllSurgicalSiteInfectionsAsync();
        Task<Models.SurgicalSiteInfection> GetSurgicalSiteInfectionByIdAsync(int id);
        Task<Models.SurgicalSiteInfection> CreateSurgicalSiteInfectionAsync(SurgicalSiteInfectionViewModel model);
        Task<Models.SurgicalSiteInfection> UpdateSurgicalSiteInfectionAsync(Models.SurgicalSiteInfection entity);
        Task<bool> DeleteSurgicalSiteInfectionAsync(int id);
        Task<IEnumerable<NhsnProcedureCategory>> GetNhsnProcedureCategories();
    }
}

using UsaWeb.Service.Features.Requests;
using UsaWeb.Service.ViewModels;
namespace UsaWeb.Service.Features.SurgicalSiteInfection.Abstractions
{
    public interface ISurgicalSiteInfectionRepository
    {
        Task<IEnumerable<Models.SurgicalSiteInfection>> GetSurgicalSiteInfectionsAsync(SurgicalSiteInfectionRequest request);
        Task<IEnumerable<Models.SurgicalSiteInfection>> GetAllSurgicalSiteInfectionsAsync();
        Task<Models.SurgicalSiteInfection> GetSurgicalSiteInfectionByIdAsync(int id);
        Task<Models.SurgicalSiteInfection> CreateSurgicalSiteInfectionAsync(SurgicalSiteInfectionViewModel model);
        Task<Models.SurgicalSiteInfection> UpdateSurgicalSiteInfectionAsync(Models.SurgicalSiteInfection entity);
        Task<bool> DeleteSurgicalSiteInfectionAsync(int id);
    }
}

using UsaWeb.Service.Models;
using UsaWeb.Service.ViewModels;

namespace UsaWeb.Service.Features.Extensions
{
    public static class SurgicalSiteInfectionSkinPrepExtensions
    {
        // Convert from Entity to ViewModel
        public static SurgicalSiteInfectionSkinPrepViewModel ToViewModel(this SurgicalSiteInfectionSkinPrep entity)
        {
            if (entity == null) return null;

            return new SurgicalSiteInfectionSkinPrepViewModel
            {
                SurgicalSiteInfectionSkinPrepId = entity.SurgicalSiteInfectionSkinPrepId,
                SurgicalSiteInfectionId = entity.SurgicalSiteInfectionId,
                SkinPrep = entity.SkinPrep,
                CreateTs = entity.CreateTs
            };
        }

        // Convert from ViewModel to Entity
        public static SurgicalSiteInfectionSkinPrep ToEntity(this SurgicalSiteInfectionSkinPrepViewModel viewModel)
        {
            if (viewModel == null) return null;

            return new SurgicalSiteInfectionSkinPrep
            {
                SurgicalSiteInfectionSkinPrepId = viewModel.SurgicalSiteInfectionSkinPrepId,
                SurgicalSiteInfectionId = viewModel.SurgicalSiteInfectionId,
                SkinPrep = viewModel.SkinPrep,
                CreateTs = viewModel.CreateTs
            };
        }
    }

}

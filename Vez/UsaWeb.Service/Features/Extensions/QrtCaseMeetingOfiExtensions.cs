using UsaWeb.Service.Models;
using UsaWeb.Service.ViewModels;

namespace UsaWeb.Service.Features.Extensions
{
    public static class QrtCaseMeetingOfiExtensions
    {
        // Convert ViewModel to Entity
        public static QrtCaseMeetingOfi ToEntity(this QrtCaseMeetingOfiVM viewModel)
        {
            return new QrtCaseMeetingOfi
            {
                QrtCaseMeetingId = viewModel.QrtCaseMeetingId,
                OfiType = viewModel.OfiType,
                Notes = viewModel.Notes,
                ProviderNpi1 = viewModel.ProviderNpi1,
                ProviderNpi2 = viewModel.ProviderNpi2,
                CreateTs = viewModel.CreateTs
            };
        }

        // Convert Entity to ViewModel
        public static QrtCaseMeetingOfiVM ToViewModel(this QrtCaseMeetingOfi entity)
        {
            return new QrtCaseMeetingOfiVM
            {
                QrtCaseMeetingId = entity.QrtCaseMeetingId,
                OfiType = entity.OfiType,
                Notes = entity.Notes,
                ProviderNpi1 = entity.ProviderNpi1,
                ProviderNpi2 = entity.ProviderNpi2,
                CreateTs = entity.CreateTs
            };
        }

        // Update Entity from ViewModel
        public static void UpdateEntity(this QrtCaseMeetingOfi entity, QrtCaseMeetingOfiVM viewModel)
        {
            entity.QrtCaseMeetingId = viewModel.QrtCaseMeetingId;
            entity.OfiType = viewModel.OfiType;
            entity.Notes = viewModel.Notes;
            entity.ProviderNpi1 = viewModel.ProviderNpi1;
            entity.ProviderNpi2 = viewModel.ProviderNpi2;
            entity.CreateTs = viewModel.CreateTs;
        }
    }
}

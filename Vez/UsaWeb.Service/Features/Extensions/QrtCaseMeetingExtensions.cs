using UsaWeb.Service.Models;
using UsaWeb.Service.ViewModels;

namespace UsaWeb.Service.Features.Extensions
{
    /// <summary>
    /// Qrt Case Meeting Extensions.
    /// </summary>
    public static class QrtCaseMeetingExtensions
    {
        /// <summary>
        /// Converts to viewmodel.
        /// </summary>
        /// <param name="meeting">The meeting.</param>
        public static QrtCaseMeetingVM ToViewModel(this QrtCaseMeeting meeting)
        {
            if (meeting == null) return null;

            return new QrtCaseMeetingVM
            {
                QrtCaseId = meeting.QrtCaseId,
                Department = meeting.Department,
                MeetingDt = meeting.MeetingDt,
                ReasonForReview = meeting.ReasonForReview,
                AttendingNpi1 = meeting.AttendingNpi1,
                IsOfi = meeting.IsOfi,
                IsDeviationFromSoC = meeting.IsDeviationFromSoC,
                Notes = meeting.Notes,
                IsReferToProcessImprove = meeting.IsReferToProcessImprove,
                ReferToProcessImproveNotes = meeting.ReferToProcessImproveNotes,
                CreateTs = meeting.CreateTs,
                DeleteTs = meeting.DeleteTs
            };
        }

        /// <summary>
        /// Converts to entity.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        public static QrtCaseMeeting ToEntity(this QrtCaseMeetingVM viewModel)
        {
            if (viewModel == null) return null;

            return new QrtCaseMeeting
            {
                QrtCaseId = viewModel.QrtCaseId,
                Department = viewModel.Department,
                MeetingDt = viewModel.MeetingDt,
                ReasonForReview = viewModel.ReasonForReview,
                AttendingNpi1 = viewModel.AttendingNpi1,
                IsOfi = viewModel.IsOfi,
                IsDeviationFromSoC = viewModel.IsDeviationFromSoC,
                Notes = viewModel.Notes,
                IsReferToProcessImprove = viewModel.IsReferToProcessImprove,
                ReferToProcessImproveNotes = viewModel.ReferToProcessImproveNotes,
                CreateTs = viewModel.CreateTs,
                DeleteTs = viewModel.DeleteTs
            };
        }

        /// <summary>
        /// Updates from view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="viewModel">The view model.</param>
        public static void UpdateFromViewModel(this QrtCaseMeeting entity, QrtCaseMeetingVM viewModel)
        {
            if (entity == null || viewModel == null) return;

            entity.QrtCaseId = viewModel.QrtCaseId;
            entity.Department = viewModel.Department;
            entity.MeetingDt = viewModel.MeetingDt;
            entity.ReasonForReview = viewModel.ReasonForReview;
            entity.AttendingNpi1 = viewModel.AttendingNpi1;
            entity.IsOfi = viewModel.IsOfi;
            entity.IsDeviationFromSoC = viewModel.IsDeviationFromSoC;
            entity.Notes = viewModel.Notes;
            entity.IsReferToProcessImprove = viewModel.IsReferToProcessImprove;
            entity.ReferToProcessImproveNotes = viewModel.ReferToProcessImproveNotes;
            entity.DeleteTs = viewModel.DeleteTs;
        }
    }
}

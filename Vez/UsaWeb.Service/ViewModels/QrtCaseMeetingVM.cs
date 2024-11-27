using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UsaWeb.Service.ViewModels
{
    /// <summary>
    /// Qrt CaseMeeting View Model.
    /// </summary>
    public class QrtCaseMeetingVM
    {
        /// <summary>
        /// Gets or sets the QRT case identifier.
        /// </summary>
        /// <value>
        /// The QRT case identifier.
        /// </value>
        public int QrtCaseId { get; set; }

        /// <summary>
        /// Gets or sets the department.
        /// </summary>
        /// <value>
        /// The department.
        /// </value>
        public string Department { get; set; }

        /// <summary>
        /// Gets or sets the meeting dt.
        /// </summary>
        /// <value>
        /// The meeting dt.
        /// </value>
        public DateOnly? MeetingDt { get; set; }

        /// <summary>
        /// Gets or sets the reason for review.
        /// </summary>
        /// <value>
        /// The reason for review.
        /// </value>
        public string ReasonForReview { get; set; }

        /// <summary>
        /// Gets or sets the attending npi1.
        /// </summary>
        /// <value>
        /// The attending npi1.
        /// </value>
        public int? AttendingNpi1 { get; set; }

        /// <summary>
        /// Gets or sets the is ofi.
        /// </summary>
        /// <value>
        /// The is ofi.
        /// </value>
        public bool? IsOfi { get; set; }

        /// <summary>
        /// Gets or sets the is deviation from so c.
        /// </summary>
        /// <value>
        /// The is deviation from so c.
        /// </value>
        public bool? IsDeviationFromSoC { get; set; }

        /// <summary>
        /// Gets or sets the notes.
        /// </summary>
        /// <value>
        /// The notes.
        /// </value>
        public string Notes { get; set; }

        /// <summary>
        /// Gets or sets the is refer to process improve.
        /// </summary>
        /// <value>
        /// The is refer to process improve.
        /// </value>
        public bool? IsReferToProcessImprove { get; set; }

        /// <summary>
        /// Gets or sets the refer to process improve notes.
        /// </summary>
        /// <value>
        /// The refer to process improve notes.
        /// </value>
        public string ReferToProcessImproveNotes { get; set; }

        /// <summary>
        /// Gets or sets the create ts.
        /// </summary>
        /// <value>
        /// The create ts.
        /// </value>
        public DateTime CreateTs { get; set; }

        /// <summary>
        /// Gets or sets the delete ts.
        /// </summary>
        /// <value>
        /// The delete ts.
        /// </value>
        public DateTime? DeleteTs { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UsaWeb.Service.ViewModels
{
    /// <summary>
    /// QrtCaseMeetingOfi View Model.
    /// </summary>
    public class QrtCaseMeetingOfiVM
    {
        /// <summary>
        /// Gets or sets the QRT case meeting identifier.
        /// </summary>
        /// <value>
        /// The QRT case meeting identifier.
        /// </value>
        public int QrtCaseMeetingId { get; set; }

        /// <summary>
        /// comm, doc, system, nursing, provider, dept, other
        /// </summary>
        public string OfiType { get; set; }

        /// <summary>
        /// Gets or sets the notes.
        /// </summary>
        /// <value>
        /// The notes.
        /// </value>
        public string Notes { get; set; }

        /// <summary>
        /// Gets or sets the provider npi1.
        /// </summary>
        /// <value>
        /// The provider npi1.
        /// </value>
        public int? ProviderNpi1 { get; set; }

        /// <summary>
        /// Gets or sets the provider npi2.
        /// </summary>
        /// <value>
        /// The provider npi2.
        /// </value>
        public int? ProviderNpi2 { get; set; }

        /// <summary>
        /// Gets or sets the create ts.
        /// </summary>
        /// <value>
        /// The create ts.
        /// </value>
        public DateTime CreateTs { get; set; }
    }
}

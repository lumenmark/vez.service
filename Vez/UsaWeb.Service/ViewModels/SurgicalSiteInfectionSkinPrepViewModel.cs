namespace UsaWeb.Service.ViewModels
{
    /// <summary>
    /// Surgical Site Infection Skin Prep ViewModel
    /// </summary>
    public class SurgicalSiteInfectionSkinPrepViewModel
    {
        /// <summary>
        /// Gets or sets the surgical site infection skin prep identifier.
        /// </summary>
        /// <value>
        /// The surgical site infection skin prep identifier.
        /// </value>
        public int SurgicalSiteInfectionSkinPrepId { get; set; }

        /// <summary>
        /// Gets or sets the surgical site infection identifier.
        /// </summary>
        /// <value>
        /// The surgical site infection identifier.
        /// </value>
        public int? SurgicalSiteInfectionId { get; set; }

        /// <summary>
        /// Gets or sets the skin prep.
        /// </summary>
        /// <value>
        /// The skin prep.
        /// </value>
        public string SkinPrep { get; set; }

        /// <summary>
        /// Gets or sets the create ts.
        /// </summary>
        /// <value>
        /// The create ts.
        /// </value>
        public DateTime CreateTs { get; set; }
    }
}

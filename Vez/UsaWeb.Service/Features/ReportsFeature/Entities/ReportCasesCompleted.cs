namespace UsaWeb.Service.Features.ReportsFeature.Entities
{
    /// <summary>
    /// Report Cases Completed.
    /// </summary>
    public class ReportCasesCompleted
    {
        /// <summary>
        /// Gets or sets the year.
        /// </summary>
        /// <value>
        /// The year.
        /// </value>
        public int Year { get; set; }

        /// <summary>
        /// Gets or sets the month.
        /// </summary>
        /// <value>
        /// The month.
        /// </value>
        public int Month { get; set; }

        /// <summary>
        /// Gets or sets the mort count.
        /// </summary>
        /// <value>
        /// The mort count.
        /// </value>
        public int MortCount { get; set; }

        /// <summary>
        /// Gets or sets the morb count.
        /// </summary>
        /// <value>
        /// The morb count.
        /// </value>
        public int MorbCount { get; set; }
    }
}

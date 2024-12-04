using UsaWeb.Service.Features.ReportsFeature.Entities;

namespace UsaWeb.Service.Features.ReportsFeature.Abstractions
{
    /// <summary>
    /// Report Repository Interface.
    /// </summary>
    public interface IReportRepository
    {
        /// <summary>
        /// Gets the cases count.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="reportType">Type of the report.</param>
        /// <param name="source">The source.</param>
        ValueTask<int> GetCasesCount(DateTime? startDate, DateTime? endDate, string reportType,string source);

        /// <summary>
        /// Gets the report mort morb count.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        Task<IEnumerable<ReportMortMorbCount>> GetReportMortMorbCount(DateTime? startDate, DateTime? endDate);

        /// <summary>
        /// Gets the report cases completed.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        Task<IEnumerable<ReportCasesCompleted>> GetReportCasesCompleted(DateTime? startDate, DateTime? endDate);
    }
}

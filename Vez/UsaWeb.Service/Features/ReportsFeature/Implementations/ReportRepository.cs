using Microsoft.EntityFrameworkCore;
using UsaWeb.Service.Data;
using UsaWeb.Service.Features.ReportsFeature.Abstractions;
using UsaWeb.Service.Features.ReportsFeature.Entities;

namespace UsaWeb.Service.Features.ReportsFeature.Implementations
{
    /// <summary>
    /// Report Repository.
    /// </summary>
    public class ReportRepository(Usaweb_DevContext context) : IReportRepository
    {
        /// <summary>
        /// The context
        /// </summary>
        private readonly Usaweb_DevContext _context = context;

        /// <summary>
        /// Gets the cases count.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="reportType">Type of the report.</param>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public async ValueTask<int> GetCasesCount(DateTime? startDate, DateTime? endDate, string reportType, string source)
        {
            var query = _context.QrtCase
                .Join(_context.QrtCaseExtended,
                    qc => qc.QrtCaseId,
                    qce => qce.QrtCaseId,
                    (qc, qce) => new { qc, qce })
                .AsQueryable();

            if (startDate.HasValue && endDate.HasValue)
            {
                query = query.Where(_ => _.qc.DischargeDt >= startDate.Value && _.qc.DischargeDt < endDate);
            }

            if (string.IsNullOrWhiteSpace(source) || source.Equals("qrt"))
            {
                query = query.Where(_ => !_.qc.Source.Equals("Mory")
                || _.qc.Source == null);
            }
            else if(source.Equals("Mory"))
            {
                query = query.Where(_ => _.qc.Source.Equals("Mory"));
            }

            switch (reportType)
            {
                case "withNotes": 
                    query = query.Where(_ => _.qce.Note != null);
                    break;
                case "needingPeerReview":
                    query = query.Where(_ => _.qce.Pr_IsNeeded.Value);
                    break;
                case "hasOfi":
                    query = query.Where(_ => _.qce.Outcome_IsOpportunityForImprovement.Value);
                    break;
                case "hasOfiProviderConcern":
                    query = query.Where(_ => _.qce.Outcome_IsProviderConcern.Value);
                    break;
                case "hasOfiNursingConcern":
                    query = query.Where(_ => _.qce.Outcome_IsNursingConcern.Value);
                    break;
                case "hasOfiSystemConcern":
                    query = query.Where(_ => _.qce.Outcome_IsSystemConcern.Value);
                    break;
            }

            return await query.CountAsync();
        }

        /// <summary>
        /// Gets the report cases completed.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        public async Task<IEnumerable<ReportCasesCompleted>> GetReportCasesCompleted(DateTime? startDate, DateTime? endDate)
        {
            var result = await _context.QrtCase
                .Join(
                    _context.QrtCaseExtended,
                    c => c.QrtCaseId,
                    e => e.QrtCaseId,
                    (c, e) => new { Case = c, Extended = e }
                )
                .Where(x => x.Extended.Status == "COMPLETE" &&
                            (!startDate.HasValue || x.Extended.CreateTs >= startDate) &&
                            (!endDate.HasValue || x.Extended.CreateTs < endDate))
                .GroupBy(x => new
                {
                    Year = x.Extended.CreateTs.HasValue ? x.Extended.CreateTs.Value.Year : 0,
                    Month = x.Extended.CreateTs.HasValue ? x.Extended.CreateTs.Value.Month : 0
                })
                .Select(g => new ReportCasesCompleted
                { 
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    MortCount = g.Count(x => x.Case.CaseType == "MORT"),
                    MorbCount = g.Count(x => x.Case.CaseType == "MORB")
                })
                .OrderBy(x => x.Year)
                .ThenBy(x => x.Month)
                .ToListAsync();

            return result;
        }

        /// <summary>
        /// Gets the report mort morb count.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        public async Task<IEnumerable<ReportMortMorbCount>> GetReportMortMorbCount(DateTime? startDate, DateTime? endDate)
        {
            var query = _context.QrtCase
                            .Where(q => q.DischargeDt >= (startDate ?? DateTime.MinValue) &&
                                        q.DischargeDt < (endDate ?? DateTime.UtcNow))
                            .GroupBy(q => new { q.DischargeDt.Value.Year, q.DischargeDt.Value.Month })
                            .Select(g => new ReportMortMorbCount
                            {
                                Year = g.Key.Year,
                                Month = g.Key.Month,
                                MortCount = g.Count(q => q.CaseType == "MORT"),
                                MorbCount = g.Count(q => q.CaseType == "MORB")
                            })
                            .OrderBy(g => g.Year)
                            .ThenBy(g => g.Month);

            return await query.ToListAsync();
        }
    }
}

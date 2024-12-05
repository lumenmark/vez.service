using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UsaWeb.Service.Data;
using UsaWeb.Service.Features.ReportsFeature.Abstractions;

namespace UsaWeb.Service.Controllers
{
    /// <summary>
    /// Report Controller.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Route("[controller]")]
    [ApiController]
    public class ReportsController(IReportRepository reportRepository) : ControllerBase
    {
        /// <summary>
        /// The report repository.
        /// </summary>
        private readonly IReportRepository _reportRepository = reportRepository;

        /// <summary>
        /// Gets the QRT case count.
        /// </summary>
        /// <param name="dateStart">The date start.</param>
        /// <param name="dateEnd">The date end.</param>
        /// <param name="reportType">Type of the report.</param>
        /// <param name="source">The source.</param>
        [HttpGet("/report1")]
        public async Task<IActionResult> GetQrtCaseCount(DateTime? dateStart, 
            DateTime? dateEnd, 
            string reportType,
            string source)
        {
            try
            {
                var result = await _reportRepository.GetCasesCount(dateStart, dateEnd, reportType, source);
                return Ok(result);
            }
            catch (Exception ex)
            {
                DBHelper.LogError($"{DateTime.Now} Error in ReportsController.GetQrtCaseCount: {ex.Message} {ex.InnerException} ");
                return StatusCode(500, $"An error occurred while processing your operation.");
            }
        }

        /// <summary>
        /// Gets the report mort morb count.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        [HttpGet("/qrt/reportMortMorbCount")]
        public async Task<IActionResult> GetReportMortMorbCount(DateTime? startDate, DateTime? endDate)
        {
            try
            {
                var result = await _reportRepository.GetReportMortMorbCount(startDate, endDate);
                return Ok(result);
            }
            catch (Exception ex)
            {
                DBHelper.LogError($"{DateTime.Now} Error in ReportsController.GetReportMortMorbCount: {ex.Message} {ex.InnerException} ");
                return StatusCode(500, $"An error occurred while processing your operation.");
            }
        }

        /// <summary>
        /// Gets the report cases completed.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        [HttpGet("/qrt/reportCasesCompleted")]
        public async Task<IActionResult> GetReportCasesCompleted(DateTime? dateStart, DateTime? dateEnd)
        {
            try
            {
                var result = await _reportRepository.GetReportCasesCompleted(dateStart, dateEnd);
                return Ok(result);
            }
            catch (Exception ex)
            {
                DBHelper.LogError($"{DateTime.Now} Error in ReportsController.GetReportCasesCompleted: {ex.Message} {ex.InnerException} ");
                return StatusCode(500, $"An error occurred while processing your operation.");
            }
        }
    }
}

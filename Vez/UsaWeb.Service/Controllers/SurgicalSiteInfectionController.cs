using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using UsaWeb.Service.Data;
using UsaWeb.Service.Features.Requests;
using UsaWeb.Service.Features.SurgicalSiteInfection.Abstractions;
using UsaWeb.Service.Models;
using UsaWeb.Service.ViewModels;

namespace UsaWeb.Service.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SurgicalSiteInfectionController : ControllerBase
    {
        private readonly ISurgicalSiteInfectionService _service;

        public SurgicalSiteInfectionController(ISurgicalSiteInfectionService service)
        {
            _service = service;
        }

        /// <summary>
        /// Retrieves a list of Surgical Site Infections based on the provided filters.
        /// </summary>
        [HttpGet("/surgical_site_infection")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<SurgicalSiteInfection>>> GetAll(
            [FromQuery] int? surgicalSiteInfectionId = null,
            [FromQuery] string status = null,
            [FromQuery] DateTime? eventDtStart = null,
            [FromQuery] DateTime? eventDtEnd = null,
            [FromQuery] string providerName = null,
            [FromQuery] string surgery = null,
            [FromQuery] string woundClassifications = null,
            [FromQuery] string patient = null,
            [FromQuery] string surgeon = null
            )
        {
            try
            {
                var statusList = !string.IsNullOrEmpty(status) ? [.. status.Split(',')] : new List<string>();
                var surgeryList = !string.IsNullOrEmpty(surgery) ? [.. surgery.Split(',')] : new List<string>();
                var woundClassificationList = !string.IsNullOrEmpty(woundClassifications) ? [.. woundClassifications.Split(',')] : new List<string>();

                var request = new SurgicalSiteInfectionRequest
                {
                    SurgicalSiteInfectionId = surgicalSiteInfectionId,
                    StatusList = statusList,
                    EventDtStart = eventDtStart,
                    EventDtEnd = eventDtEnd,
                    ProviderName = providerName,
                    SurgeryList = surgeryList,
                    WoundClassificationList = woundClassificationList,
                    Patient = patient,
                    Surgeon = surgeon
                };

                var result = await _service.GetSurgicalSiteInfectionsAsync(request);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                DBHelper.LogError($"SurgicalSiteInfectionController.GetAll {DateTime.UtcNow} - {ex.Message} - InnerException : {ex.InnerException} ");
                return StatusCode(500, new { message = "An error occurred while processing your request." });
            }
        }

        /// <summary>
        /// Creates a new Surgical Site Infection record.
        /// </summary>
        [HttpPost("/surgical_site_infection")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<SurgicalSiteInfection>> Create(SurgicalSiteInfectionViewModel model)
        {
            try
            {
                var result = await _service.CreateSurgicalSiteInfectionAsync(model);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                DBHelper.LogError($"SurgicalSiteInfectionController.Create {DateTime.UtcNow} - {ex.Message} - InnderException : {ex.InnerException} ");
                return StatusCode(500, new { message = "An error occurred while processing your request."});
            }
        }

        /// <summary>
        /// Updates an existing Surgical Site Infection record.
        /// </summary>
        [HttpPut("/surgical_site_infection/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<SurgicalSiteInfection>> Update(int id, SurgicalSiteInfectionViewModel model)
        {
            try
            {
                var result = await _service.UpdateSurgicalSiteInfectionAsync(id, model);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                DBHelper.LogError($"SurgicalSiteInfectionController.Update {DateTime.UtcNow} - {ex.Message} - InnerException : {ex.InnerException} ");
                return StatusCode(500, new { message = "An error occurred while processing your request." });
            }
        }

        /// <summary>
        /// Partially updates an existing Surgical Site Infection record.
        /// </summary>
        [HttpPatch("/surgical_site_infection/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<SurgicalSiteInfectionViewModel>> Patch(int id, [FromBody] JsonPatchDocument<SurgicalSiteInfectionViewModel> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            try
            {
                var result = await _service.PatchSurgicalSiteInfectionAsync(id, patchDoc);
                if (result == null)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                DBHelper.LogError($"SurgicalSiteInfectionController.Patch {DateTime.UtcNow} - {ex.Message} - InnerException : {ex.InnerException} ");
                return StatusCode(500, new { message = "An error occurred while processing your request." });
            }
        }
    }
}
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using UsaWeb.Service.Data;
using UsaWeb.Service.Features.Extensions;
using UsaWeb.Service.Features.Requests;
using UsaWeb.Service.Features.Responses;
using UsaWeb.Service.Features.SurgicalSiteInfection.Abstractions;
using UsaWeb.Service.Features.SurgicalSiteInfection.Implementations;
using UsaWeb.Service.Models;
using UsaWeb.Service.ViewModels;

namespace UsaWeb.Service.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SurgicalSiteInfectionController(ISurgicalSiteInfectionService service, ISurgicalSiteInfectionSkinPrepService skinPrepService) : ControllerBase
    {
        private readonly ISurgicalSiteInfectionService _service = service;
        private readonly ISurgicalSiteInfectionSkinPrepService _skinPrepService = skinPrepService;

        [HttpGet("/nhsn_procedure_categories")]
        public async Task<ActionResult<IEnumerable<NhsnProcedureCategory>>> GetAllNhsnProcedureCategories()
        {
            try
            {
                var result = await _service.GetNhsnProcedureCategories();
                return Ok(result);
            }
            catch (Exception ex)
            {
                DBHelper.LogError($"SurgicalSiteInfectionController.GetAllNhsnProcedureCategories {DateTime.UtcNow} - {ex.Message} - InnerException : {ex.InnerException} ");
                return StatusCode(500, new { message = "An error occurred while processing your request." });
            }
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
        public async Task<IActionResult> Create(SurgicalSiteInfectionViewModel model)
        {
            try
            {
                var surgicalSiteInfection = await _service.CreateSurgicalSiteInfectionAsync(model);
                var skiPrepModel = new SkinPrepViewModel
                {
                    SurgicalSiteInfectionId = surgicalSiteInfection.SurgicalSiteInfectionId,
                    SkinPrep = model.SurgicalSiteInfectionSkinPrep
                };

                var response = surgicalSiteInfection.ToResponse();

                var isSkinPrepCreated = await _skinPrepService.CreateSkinPrepAsync(skiPrepModel);
                if (isSkinPrepCreated)
                {
                    response.SurgicalSiteInfectionSkinPrep = model.SurgicalSiteInfectionSkinPrep;
                }

                return Ok(response);
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
        public async Task<ActionResult> Update(int id, SurgicalSiteInfectionViewModel model)
        {
            try
            {
                var result = await _service.UpdateSurgicalSiteInfectionAsync(id, model);
                if (result == null)
                {
                    return NotFound();
                }

                var response = result.ToResponse();
                var skiPrepModel = new SkinPrepViewModel
                {
                    SurgicalSiteInfectionId = id,
                    SkinPrep = model.SurgicalSiteInfectionSkinPrep
                };

                var isSkinPrepCreated = await _skinPrepService.UpdateSkinPrepAsync(skiPrepModel);
                if (isSkinPrepCreated)
                {
                    response.SurgicalSiteInfectionSkinPrep = model.SurgicalSiteInfectionSkinPrep;
                }
                return Ok(response);
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
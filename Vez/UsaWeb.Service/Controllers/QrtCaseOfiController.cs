using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using System.Text.Json;
using UsaWeb.Service.Helper;
using UsaWeb.Service.Data;
using UsaWeb.Service.Models;
using UsaWeb.Service.ViewModels;
using Microsoft.AspNetCore.JsonPatch;
using UsaWeb.Service.Features.QrtCaseMeetingOfiFeature.Abstractions;
using UsaWeb.Service.Features.QrtCaseMeetingFeature.Abstractions;

namespace UsaWeb.Service.Controllers
{
    /// <summary>
    /// Qrt Case Controller.
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class QrtCaseOfiController(IQrtCaseMeetingOfiService service) : ControllerBase
    {
        /// <summary>
        /// The service.
        /// </summary>
        private readonly IQrtCaseMeetingOfiService _service = service;

        /// <summary>
        /// Gets the QRT case meeting.
        /// </summary>
        /// <param name="qrtCaseMeetingId">The QRT case meeting identifier.</param>
        /// <param name="qrtCaseMeetingOfiId">The QRT case meeting ofi identifier.</param>
        [HttpGet("/qrt/caseMeetingOfi")]
        public async Task<IActionResult> GetQrtCaseMeeting(int? qrtCaseMeetingId, int? qrtCaseMeetingOfiId)
        {
            try
            {
                var qrtMeetings = await _service.GetByParams(qrtCaseMeetingId, qrtCaseMeetingOfiId);
                return Ok(qrtMeetings);
            }
            catch (Exception ex)
            {
                DBHelper.LogError($"{DateTime.Now} Error in QrtCaseController.GetQrtCaseMeeting: {ex.Message} ");
                return StatusCode(500, $"An error occurred while processing your operation ");
            }
        }

        /// <summary>
        /// Posts the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        [HttpPost("/qrt/caseMeetingOfi")]
        public async Task<IActionResult> Post(QrtCaseMeetingOfiVM model)
        {
            try
            {
                var result = await _service.Create(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                DBHelper.LogError($"{DateTime.Now} Error in QrtCaseController.Post: {ex.Message} ");
                return StatusCode(500, $"An error occurred while processing your operation ");
            }
        }

        /// <summary>
        /// Puts the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="model">The model.</param>
        [HttpPut("/qrt/caseMeetingOfi/{id}")]
        public async Task<IActionResult> Put(int id, QrtCaseMeetingOfiVM model)
        {
            try
            {
                var result = await _service.Update(model, id);
                if(result == null)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                DBHelper.LogError($"{DateTime.Now} Error in QrtCaseController.Put: {ex.Message} ");
                return StatusCode(500, $"An error occurred while processing your operation ");
            }
        }

        [HttpDelete("/qrt/caseMeetingOfi/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _service.Delete(id);
                if (result == null)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                DBHelper.LogError($"{DateTime.Now} Error in QrtCaseController.Delete: {ex.Message} ");
                return StatusCode(500, $"An error occurred while processing your operation ");
            }
        }

        /// <summary>
        /// Patches the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="patchDoc">The patch document.</param>
        [HttpPatch("/qrt/caseMeetingOfi/{id}")]
        public async Task<IActionResult> Patch(int id, JsonPatchDocument<QrtCaseMeetingOfiVM> patchDoc)
        {
            try
            {
                var result = await _service.Patch(id, patchDoc);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                DBHelper.LogError($"{DateTime.Now} Error in QrtCaseController.Delete: {ex.Message} ");
                return StatusCode(500, $"An error occurred while processing your operation ");
            }
        }
    }
}

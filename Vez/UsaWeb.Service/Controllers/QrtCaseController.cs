using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using System.Text.Json;
using UsaWeb.Service.Features.QrtCaseMeetingFeature.Abstractions;
using UsaWeb.Service.Helper;
using UsaWeb.Service.Data;
using UsaWeb.Service.Models;
using UsaWeb.Service.ViewModels;
using Microsoft.AspNetCore.JsonPatch;

namespace UsaWeb.Service.Controllers
{
    /// <summary>
    /// Qrt Case Controller.
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class QrtCaseController(IQrtCaseMeetingService service) : ControllerBase
    {
        /// <summary>
        /// The service.
        /// </summary>
        private readonly IQrtCaseMeetingService _service = service;

        /// <summary>
        /// Gets the QRT case meeting.
        /// </summary>
        /// <param name="qrtCaseMeetingId">The QRT case meeting identifier.</param>
        /// <param name="qrtCaseId">The QRT case identifier.</param>
        [HttpGet("/qrt/caseMeeting")]
        public async Task<IActionResult> GetQrtCaseMeeting(int? qrtCaseMeetingId, int? qrtCaseId)
        {
            try
            {
                var qrtMeetings = await _service.GetByParams(qrtCaseMeetingId, qrtCaseId);
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
        [HttpPost("/qrt/caseMeeting")]
        public async Task<IActionResult> Post(QrtCaseMeetingVM model)
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
        [HttpPut("/qrt/caseMeeting/{id}")]
        public async Task<IActionResult> Put(int id, QrtCaseMeetingVM model)
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

        [HttpDelete("/qrt/caseMeeting/{id}")]
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
        [HttpPatch("/qrt/caseMeeting/{id}")]
        public async Task<IActionResult> Patch(int id, JsonPatchDocument<QrtCaseMeetingVM> patchDoc)
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

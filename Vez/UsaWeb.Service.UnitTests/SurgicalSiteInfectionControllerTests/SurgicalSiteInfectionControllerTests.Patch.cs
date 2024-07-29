using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsaWeb.Service.ViewModels;
using Xunit;

namespace UsaWeb.Service.UnitTests.SurgicalSiteInfectionControllerTests
{
    public partial class SurgicalSiteInfectionControllerTests
    {
        [Fact]
        public async Task Patch_ReturnsOkResult_WithExpectedData()
        {
            // Arrange
            var patchDoc = new JsonPatchDocument<SurgicalSiteInfectionViewModel>();
            patchDoc.Replace(e => e.PatientFirstName, "John");

            var expectedData = new SurgicalSiteInfectionViewModel { PatientFirstName = "John", PatientLastName = "Doe" };

            MockService.Setup(service => service.PatchSurgicalSiteInfectionAsync(1, patchDoc))
                       .ReturnsAsync(expectedData);

            // Act
            var result = await _controller.Patch(1, patchDoc);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(expectedData, okResult.Value);
        }

        [Fact]
        public async Task Patch_ReturnsBadRequest_OnArgumentException()
        {
            // Arrange
            var patchDoc = new JsonPatchDocument<SurgicalSiteInfectionViewModel>();
            patchDoc.Replace(e => e.PatientFirstName, "John");

            var errorMessage = "Invalid argument";
            MockService.Setup(service => service.PatchSurgicalSiteInfectionAsync(1, patchDoc))
                       .ThrowsAsync(new ArgumentException(errorMessage));

            // Act
            var result = await _controller.Patch(1, patchDoc);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(400, badRequestResult.StatusCode);

            // Assert the message in the anonymous type
            var value = badRequestResult.Value;
            var messageProperty = value?.GetType().GetProperty("message");
            Assert.NotNull(messageProperty);
            Assert.Equal(errorMessage, messageProperty.GetValue(value));
        }

        [Fact]
        public async Task Patch_ReturnsNotFound_OnNullResult()
        {
            // Arrange
            var patchDoc = new JsonPatchDocument<SurgicalSiteInfectionViewModel>();
            patchDoc.Replace(e => e.PatientFirstName, "John");

            MockService.Setup(service => service.PatchSurgicalSiteInfectionAsync(1, patchDoc))
                       .ReturnsAsync((SurgicalSiteInfectionViewModel)null);

            // Act
            var result = await _controller.Patch(1, patchDoc);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result.Result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task Patch_ReturnsBadRequest_OnNullPatchDoc()
        {
            // Act
            var result = await _controller.Patch(1, null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestResult>(result.Result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task Patch_ReturnsInternalServerError_OnException()
        {
            // Arrange
            var patchDoc = new JsonPatchDocument<SurgicalSiteInfectionViewModel>();
            patchDoc.Replace(e => e.PatientFirstName, "John");

            MockService.Setup(service => service.PatchSurgicalSiteInfectionAsync(1, patchDoc))
                       .ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.Patch(1, patchDoc);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, statusCodeResult.StatusCode);

            // Assert the message in the anonymous type
            var value = statusCodeResult.Value;
            var messageProperty = value?.GetType().GetProperty("message");
            Assert.NotNull(messageProperty);
            Assert.Equal("An error occurred while processing your request.", messageProperty.GetValue(value));
        }
    }
}

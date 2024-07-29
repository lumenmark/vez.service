using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsaWeb.Service.Models;
using UsaWeb.Service.ViewModels;
using Xunit;

namespace UsaWeb.Service.UnitTests.SurgicalSiteInfectionControllerTests
{
    public partial class SurgicalSiteInfectionControllerTests
    {
        [Fact]
        public async Task Update_ReturnsOkResult_WithExpectedData()
        {
            // Arrange
            var model = new SurgicalSiteInfectionViewModel { PatientFirstName = "John", PatientLastName = "Doe" };
            var expectedData = new SurgicalSiteInfection { SurgicalSiteInfectionId = 1, PatientFirstName = "John", PatientLastName = "Doe" };

            MockService.Setup(service => service.UpdateSurgicalSiteInfectionAsync(1, model))
                       .ReturnsAsync(expectedData);

            // Act
            var result = await _controller.Update(1, model);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(expectedData, okResult.Value);
        }

        [Fact]
        public async Task Update_ReturnsBadRequest_OnArgumentException()
        {
            // Arrange
            var model = new SurgicalSiteInfectionViewModel { PatientFirstName = "John", PatientLastName = "Doe" };
            var errorMessage = "Invalid argument";
            MockService.Setup(service => service.UpdateSurgicalSiteInfectionAsync(1, model))
                       .ThrowsAsync(new ArgumentException(errorMessage));

            // Act
            var result = await _controller.Update(1, model);

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
        public async Task Update_ReturnsNotFound_OnNullResult()
        {
            // Arrange
            var model = new SurgicalSiteInfectionViewModel { PatientFirstName = "John", PatientLastName = "Doe" };

            MockService.Setup(service => service.UpdateSurgicalSiteInfectionAsync(1, model))
                       .ReturnsAsync((SurgicalSiteInfection)null);

            // Act
            var result = await _controller.Update(1, model);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result.Result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task Update_ReturnsInternalServerError_OnException()
        {
            // Arrange
            var model = new SurgicalSiteInfectionViewModel { PatientFirstName = "John", PatientLastName = "Doe" };
            MockService.Setup(service => service.UpdateSurgicalSiteInfectionAsync(1, model))
                       .ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.Update(1, model);

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

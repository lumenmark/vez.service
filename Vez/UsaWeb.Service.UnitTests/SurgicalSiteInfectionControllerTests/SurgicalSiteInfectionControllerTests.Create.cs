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
        public async Task Create_ReturnsOkResult_WithExpectedData()
        {
            // Arrange
            var model = new SurgicalSiteInfectionViewModel { PatientFirstName = "John", PatientLastName = "Doe" };
            var expectedData = new SurgicalSiteInfection { SurgicalSiteInfectionId = 1, PatientFirstName = "John", PatientLastName = "Doe" };

            MockService.Setup(service => service.CreateSurgicalSiteInfectionAsync(model))
                       .ReturnsAsync(expectedData);

            // Act
            var result = await _controller.Create(model);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(expectedData, okResult.Value);
        }

        [Fact]
        public async Task Create_ReturnsBadRequest_OnArgumentException()
        {
            // Arrange
            var model = new SurgicalSiteInfectionViewModel { PatientFirstName = "John", PatientLastName = "Doe" };
            var errorMessage = "Invalid argument";
            MockService.Setup(service => service.CreateSurgicalSiteInfectionAsync(model))
                       .ThrowsAsync(new ArgumentException(errorMessage));

            // Act
            var result = await _controller.Create(model);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);

            // Assert the message in the anonymous type
            var value = badRequestResult.Value;
            var messageProperty = value?.GetType().GetProperty("message");
            Assert.NotNull(messageProperty);
            Assert.Equal(errorMessage, messageProperty.GetValue(value));
        }

        [Fact]
        public async Task Create_ReturnsInternalServerError_OnException()
        {
            // Arrange
            var model = new SurgicalSiteInfectionViewModel { PatientFirstName = "John", PatientLastName = "Doe" };
            MockService.Setup(service => service.CreateSurgicalSiteInfectionAsync(model))
                       .ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.Create(model);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);

            // Assert the message in the anonymous type
            var value = statusCodeResult.Value;
            var messageProperty = value?.GetType().GetProperty("message");
            Assert.NotNull(messageProperty);
            Assert.Equal("An error occurred while processing your request.", messageProperty.GetValue(value));
        }
    }
}

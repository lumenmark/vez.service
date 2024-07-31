using Microsoft.AspNetCore.Mvc;
using Moq;
using UsaWeb.Service.Features.Requests;
using UsaWeb.Service.Features.Responses;
using UsaWeb.Service.Models;
using Xunit;

namespace UsaWeb.Service.UnitTests.SurgicalSiteInfectionControllerTests
{
    public partial class SurgicalSiteInfectionControllerTests
    {
        [Fact]
        public async Task GetAll_ReturnsOkResult_WithExpectedData()
        {
            // Arrange
            var expectedData = new List<SurgicalSiteInfectionResponse>
            {
                new() { SurgicalSiteInfectionId = 1, PatientFirstName = "LJ", PatientLastName = "LI" },
                new() { SurgicalSiteInfectionId = 2, PatientFirstName = "Abdelilah", PatientLastName = "Ben" }
            };

            MockService.Setup(service => service.GetSurgicalSiteInfectionsAsync(It.IsAny<SurgicalSiteInfectionRequest>()))
                       .ReturnsAsync(expectedData);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(expectedData, okResult.Value);
        }

        [Fact]
        public async Task GetAll_ReturnsBadRequest_OnArgumentException()
        {
            // Arrange
            var errorMessage = "Invalid argument";
            MockService.Setup(service => service.GetSurgicalSiteInfectionsAsync(It.IsAny<SurgicalSiteInfectionRequest>()))
                       .ThrowsAsync(new ArgumentException(errorMessage));

            // Act
            var result = await _controller.GetAll();

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
        public async Task GetAll_ReturnsInternalServerError_OnException()
        {
            // Arrange
            MockService.Setup(service => service.GetSurgicalSiteInfectionsAsync(It.IsAny<SurgicalSiteInfectionRequest>()))
                       .ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.GetAll();

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

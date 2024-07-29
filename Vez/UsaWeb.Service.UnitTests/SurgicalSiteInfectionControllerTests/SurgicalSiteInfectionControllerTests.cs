using Moq;
using UsaWeb.Service.Controllers;
using UsaWeb.Service.Features.SurgicalSiteInfection.Abstractions;

namespace UsaWeb.Service.UnitTests.SurgicalSiteInfectionControllerTests
{
    public partial class SurgicalSiteInfectionControllerTests
    {
        private readonly Mock<ISurgicalSiteInfectionService> MockService;
        private readonly SurgicalSiteInfectionController _controller;

        public SurgicalSiteInfectionControllerTests()
        {
            MockService = new Mock<ISurgicalSiteInfectionService>();
            _controller = new SurgicalSiteInfectionController(MockService.Object);
        }
    }
}
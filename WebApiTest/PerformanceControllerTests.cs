using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Moq;
using SoftServe.ITAcademy.BackendDubbingProject.Administration.Core;
using SoftServe.ITAcademy.BackendDubbingProject.Administration.Core.DTOs;
using SoftServe.ITAcademy.BackendDubbingProject.Web.ApiControllers;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Xunit;

namespace SoftServe.ITAcademy.BackendDubbingProject.WebApiTest
{
    [ExcludeFromCodeCoverage]
    public class PerformanceControllerTests
    {
        [Fact]
        public async Task TestGetAll_200OK()
        {
            // Arrange
            var mock = new Mock<IAdministrationService>();
            mock.Setup(service => service.GetAllPerformancesAsync()).Returns(async () => { return DullData.GetAllPerfomanceDTOs(); });
            var controller = new PerformanceController(mock.Object);

            // Act
            var result = (await controller.GetAll()).Result as ObjectResult;
            var value = result.Value as List<PerformanceDTO>;

            // Assert
            Assert.NotEmpty(value);
            Assert.Equal(3, value.Count);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

        [Fact]
        public async Task TestGetById_200OK()
        {
            // Arrange
            var mock = new Mock<IAdministrationService>();
            int id = 1;
            mock.Setup(service => service.GetPerformanceByIdAsync(id)).Returns(async () => {
                return new PerformanceDTO { Id = 1, Title = "Perfomance1", Description = "Description of perfomance1" };
            });
            var controller = new PerformanceController(mock.Object);

            // Act
            var result = (await controller.GetById(id)).Result as ObjectResult;
            var value = result.Value as PerformanceDTO;

            // Assert
            Assert.NotNull(value);
            Assert.Equal(id, value.Id);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

        [Fact]
        public async Task TestGetById_404NotFound()
        {
            // Arrange
            var mock = new Mock<IAdministrationService>();
            int id = 1;
            mock.Setup(service => service.GetPerformanceByIdAsync(id)).Returns(async () => { return null; });
            var controller = new LanguageController(mock.Object);

            // Act
            var result = (await controller.GetById(id)).Result as IStatusCodeActionResult;

            // Assert
            Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
        }

        [Fact]
        public async Task TestCreate_201Created()
        {
            // Arrange
            var mock = new Mock<IAdministrationService>();
            var controller = new PerformanceController(mock.Object);
            PerformanceDTO performance = new PerformanceDTO { Id = 1, Title = "Perfomance1", Description = "Description of perfomance1" };

            // Act
            var result = (await controller.Create(performance)).Result as ObjectResult;
            var value = result.Value as PerformanceDTO;

            // Assert
            Assert.NotNull(value);
            Assert.Equal(performance.Id, value.Id);
            Assert.Equal(StatusCodes.Status201Created, result.StatusCode);
        }

        [Fact]
        public async Task TestUpdate_204NoContent()
        {
            // Arrange
            var mock = new Mock<IAdministrationService>();
            var controller = new PerformanceController(mock.Object);
            var id = 1;
            PerformanceDTO performance = new PerformanceDTO { Id = 1, Title = "Perfomance1", Description = "Description of perfomance1" };

            // Act
            var result = await controller.Update(id, performance) as IStatusCodeActionResult;

            // Assert
            Assert.Equal(StatusCodes.Status204NoContent, result.StatusCode);
        }

        [Fact]
        public async Task TestUpdate_400BadRequest()
        {
            // Arrange
            var mock = new Mock<IAdministrationService>();
            var controller = new PerformanceController(mock.Object);
            var id = 2;
            PerformanceDTO performance = new PerformanceDTO { Id = 1, Title = "Perfomance1", Description = "Description of perfomance1" };

            // Act
            var result = await controller.Update(id, performance) as IStatusCodeActionResult;

            // Assert
            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task TestDelete_204NoContent()
        {
            // Arrange
            var mock = new Mock<IAdministrationService>();
            var controller = new PerformanceController(mock.Object);
            var id = 2;

            // Act
            var result = await controller.Delete(id) as IStatusCodeActionResult;

            // Assert
            Assert.Equal(StatusCodes.Status204NoContent, result.StatusCode);
        }

        [Fact]
        public async Task TestGetByIdWithChildren_200OK()
        {
            // Arrange
            var mock = new Mock<IAdministrationService>();
            int id = 1;
            mock.Setup(service => service.GetSpeechesByPerformanceIdAsync(id)).Returns(async () => { return DullData.GetAllSpeechDTOs(); });
            var controller = new PerformanceController(mock.Object);

            // Act
            var result = (await controller.GetByIdWithChildren(id)).Result as ObjectResult;
            var value = result.Value as List<SpeechDTO>;

            // Assert
            Assert.NotEmpty(value);
            Assert.Equal(3, value.Count);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

        [Fact]
        public async Task TestGetByIdWithChildren_400BadRequest()
        {
            // Arrange
            var mock = new Mock<IAdministrationService>();
            int id = 1;
            mock.Setup(service => service.GetSpeechesByPerformanceIdAsync(id)).Returns(async () => { return null; });
            var controller = new PerformanceController(mock.Object);

            // Act
            var result = (await controller.GetByIdWithChildren(id)).Result as IStatusCodeActionResult;

            // Assert
            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task TestGetByIdLanguages_200OK()
        {
            // Arrange
            var mock = new Mock<IAdministrationService>();
            int id = 1;
            mock.Setup(service => service.GetLanguagesByPerformanceIdAsync(id)).Returns(async () => { return DullData.GetAllLanguageDTOs(); });
            var controller = new PerformanceController(mock.Object);

            // Act
            var result = (await controller.GetByIdLanguages(id)).Result as ObjectResult;
            var value = result.Value as List<LanguageDTO>;

            // Assert
            Assert.NotEmpty(value);
            Assert.Equal(3, value.Count);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }
    }
}

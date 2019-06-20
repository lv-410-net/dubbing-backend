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
    public class SpeechControllerTests
    {
        [Fact]
        public async Task TestGetAll_200OK()
        {
            // Arrange
            var mock = new Mock<IAdministrationService>();
            mock.Setup(service => service.GetAllSpeechesAsync()).Returns(async () => { return DullData.GetAllSpeechDTOs(); });
            var controller = new SpeechController(mock.Object);

            // Act
            var result = (await controller.GetAll()).Result as ObjectResult;
            var value = result.Value as List<SpeechDTO>;

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
            mock.Setup(service => service.GetSpeechByIdAsync(id)).Returns(async () => {
                return new SpeechDTO { Id = 1, Order = 1, Text = "Speech1", Duration = 120, PerformanceId = 1 };
            });
            var controller = new SpeechController(mock.Object);

            // Act
            var result = (await controller.GetById(id)).Result as ObjectResult;
            var value = result.Value as SpeechDTO;

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
            mock.Setup(service => service.GetSpeechByIdAsync(id)).Returns(async () => { return null; });
            var controller = new SpeechController(mock.Object);

            // Act
            var result = (await controller.GetById(id)).Result as IStatusCodeActionResult;

            // Assert
            Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
        }

        [Fact]
        public async Task TestCreate_201Created()
        {
            // Arrange
            SpeechDTO speech = new SpeechDTO { Id = 1, Order = 1, Text = "Speech1", Duration = 120, PerformanceId = 1 };
            var mock = new Mock<IAdministrationService>();
            var controller = new SpeechController(mock.Object);

            // Act
            var result = (await controller.Create(speech)).Result as ObjectResult;
            var value = result.Value as SpeechDTO;

            // Assert
            Assert.NotNull(value);
            Assert.Equal(speech.Id, value.Id);
            Assert.Equal(StatusCodes.Status201Created, result.StatusCode);
        }

        [Fact]
        public async Task TestUpdate_204NoContent()
        {
            // Arrange
            var mock = new Mock<IAdministrationService>();
            var controller = new SpeechController(mock.Object);
            var id = 1;
            SpeechDTO speech = new SpeechDTO { Id = 1, Order = 1, Text = "Speech1", Duration = 120, PerformanceId = 1 };

            // Act
            var result = await controller.Update(id, speech) as IStatusCodeActionResult;

            // Assert
            Assert.Equal(StatusCodes.Status204NoContent, result.StatusCode);
        }

        [Fact]
        public async Task TestUpdate_400BadRequest()
        {
            // Arrange
            var mock = new Mock<IAdministrationService>();
            var controller = new SpeechController(mock.Object);
            var id = 2;
            SpeechDTO speech = new SpeechDTO { Id = 1, Order = 1, Text = "Speech1", Duration = 120, PerformanceId = 1 };

            // Act
            var result = await controller.Update(id, speech) as IStatusCodeActionResult;

            // Assert
            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task TestDelete_204NoContent()
        {
            // Arrange
            var mock = new Mock<IAdministrationService>();
            var controller = new SpeechController(mock.Object);
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
            mock.Setup(service => service.GetAudiosBySpeechIdAsync(id)).Returns(async () => { return DullData.GetAllAudioDTOs(); });
            var controller = new SpeechController(mock.Object);

            // Act
            var result = (await controller.GetByIdWithChildren(id)).Result as ObjectResult;
            var value = result.Value as List<AudioDTO>;

            // Assert
            Assert.NotEmpty(value);
            Assert.Equal(5, value.Count);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

        [Fact]
        public async Task TestGetByIdWithChildren_400BadRequest()
        {
            // Arrange
            var mock = new Mock<IAdministrationService>();
            int id = 1;
            mock.Setup(service => service.GetAudiosBySpeechIdAsync(id)).Returns(async () => { return null; });
            var controller = new PerformanceController(mock.Object);

            // Act
            var result = (await controller.GetByIdWithChildren(id)).Result as IStatusCodeActionResult;

            // Assert
            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        }
    }
}

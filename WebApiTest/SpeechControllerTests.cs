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
        private const int SomeId = 1;
        private const int OtherId = 2;
        private const int SomeOrder = 1;
        private const string SomeText = "Speech1";
        private const int SomeDuration = 120;
        private const int SomePerfomanceId = 1;

        private static SpeechDTO SomeSpeech = new SpeechDTO
        {
            Id = SomeId,
            Order = SomeOrder,
            Text = SomeText,
            Duration = SomeDuration,
            PerformanceId = SomePerfomanceId
        };

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
            mock.Setup(service => service.GetSpeechByIdAsync(SomeId)).Returns(async () => { return SomeSpeech; });
            var controller = new SpeechController(mock.Object);

            // Act
            var result = (await controller.GetById(SomeId)).Result as ObjectResult;
            var value = result.Value as SpeechDTO;

            // Assert
            Assert.NotNull(value);
            Assert.Equal(SomeId, value.Id);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

        [Fact]
        public async Task TestGetById_404NotFound()
        {
            // Arrange
            var mock = new Mock<IAdministrationService>();
            mock.Setup(service => service.GetSpeechByIdAsync(SomeId)).Returns(async () => { return null; });
            var controller = new SpeechController(mock.Object);

            // Act
            var result = (await controller.GetById(SomeId)).Result as IStatusCodeActionResult;

            // Assert
            Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
        }

        [Fact]
        public async Task TestCreate_201Created()
        {
            // Arrange
            var mock = new Mock<IAdministrationService>();
            var controller = new SpeechController(mock.Object);

            // Act
            var result = (await controller.Create(SomeSpeech)).Result as ObjectResult;
            var value = result.Value as SpeechDTO;

            // Assert
            Assert.NotNull(value);
            Assert.Equal(SomeSpeech.Id, value.Id);
            Assert.Equal(StatusCodes.Status201Created, result.StatusCode);
        }

        [Fact]
        public async Task TestUpdate_204NoContent()
        {
            // Arrange
            var mock = new Mock<IAdministrationService>();
            var controller = new SpeechController(mock.Object);

            // Act
            var result = await controller.Update(SomeId, SomeSpeech) as IStatusCodeActionResult;

            // Assert
            Assert.Equal(StatusCodes.Status204NoContent, result.StatusCode);
        }

        [Fact]
        public async Task TestUpdate_400BadRequest()
        {
            // Arrange
            var mock = new Mock<IAdministrationService>();
            var controller = new SpeechController(mock.Object);

            // Act
            var result = await controller.Update(OtherId, SomeSpeech) as IStatusCodeActionResult;

            // Assert
            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task TestDelete_204NoContent()
        {
            // Arrange
            var mock = new Mock<IAdministrationService>();
            var controller = new SpeechController(mock.Object);

            // Act
            var result = await controller.Delete(SomeId) as IStatusCodeActionResult;

            // Assert
            Assert.Equal(StatusCodes.Status204NoContent, result.StatusCode);
        }

        [Fact]
        public async Task TestGetByIdWithChildren_200OK()
        {
            // Arrange
            var mock = new Mock<IAdministrationService>();
            mock.Setup(service => service.GetAudiosBySpeechIdAsync(SomeId)).Returns(async () => { return DullData.GetAllAudioDTOs(); });
            var controller = new SpeechController(mock.Object);

            // Act
            var result = (await controller.GetByIdWithChildren(SomeId)).Result as ObjectResult;
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
            mock.Setup(service => service.GetAudiosBySpeechIdAsync(SomeId)).Returns(async () => { return null; });
            var controller = new PerformanceController(mock.Object);

            // Act
            var result = (await controller.GetByIdWithChildren(SomeId)).Result as IStatusCodeActionResult;

            // Assert
            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        }
    }
}

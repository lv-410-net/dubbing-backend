using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Moq;
using SoftServe.ITAcademy.BackendDubbingProject.Administration.Core;
using SoftServe.ITAcademy.BackendDubbingProject.Administration.Core.DTOs;
using SoftServe.ITAcademy.BackendDubbingProject.Web.ApiControllers;
using Xunit;

namespace SoftServe.ITAcademy.BackendDubbingProject.WebApiTest
{
    [ExcludeFromCodeCoverage]
    public class AudioControllerTests
    {
        private const int SomeId = 1;
        private const int OtherId = 2;
        private const string SomeFileName = "File1";
        private const string SomeOriginalText = "Text1";
        private const int SomeSpeechId = 1;
        private const int SomeLanguageId = 1;

        private static string[] SomeFiles = new string[] { "File1", "File2", "File3" };
        private static AudioDTO SomeAudio = new AudioDTO
        {
            Id = SomeId,
            FileName = SomeFileName,
            OriginalText = SomeOriginalText,
            SpeechId = SomeSpeechId,
            LanguageId = SomeLanguageId
        };

        [Fact]
        public async Task TestGetAll_200OK()
        {
            // Arrange
            var mock = new Mock<IAdministrationService>();
            mock.Setup(service => service.GetAllAudiosAsync()).Returns(async () => { return DullData.GetAllAudioDTOs(); });
            var controller = new AudioController(mock.Object);

            // Act
            var result = (await controller.GetAll()).Result as ObjectResult;
            var value = result.Value as List<AudioDTO>;

            // Assert
            Assert.NotNull(value);
            Assert.NotEmpty(value);
            Assert.Equal(5, value.Count);
            Assert.Equal(value[0].Id, SomeAudio.Id);
            Assert.Equal("File5", value[value.Count-1].FileName);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

        [Fact]
        public async Task TestGetById_200OK()
        {
            // Arrange
            var mock = new Mock<IAdministrationService>();
            mock.Setup(service => service.GetAudioByIdAsync(SomeId)).Returns(async () => { return SomeAudio; });
            var controller = new AudioController(mock.Object);

            // Act
            var result = (await controller.GetById(SomeId)).Result as ObjectResult;
            var value = result.Value as AudioDTO;

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
            mock.Setup(service => service.GetAudioByIdAsync(SomeId)).Returns(async () => { return null; });
            var controller = new AudioController(mock.Object);

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
            var controller = new AudioController(mock.Object);

            // Act
            var result = (await controller.Create(SomeAudio)).Result as ObjectResult;
            var value = result.Value as AudioDTO;

            // Assert
            Assert.NotNull(value);
            Assert.Equal(SomeAudio.Id, value.Id);
            Assert.Equal(StatusCodes.Status201Created, result.StatusCode);
        }

        [Fact]
        public async Task TestUpdate_204NoContent()
        {
            // Arrange
            var mock = new Mock<IAdministrationService>();
            var controller = new AudioController(mock.Object);

            // Act
            var result = (await controller.Update(SomeId, SomeAudio)) as IStatusCodeActionResult;

            // Assert
            Assert.Equal(StatusCodes.Status204NoContent, result.StatusCode);
        }

        [Fact]
        public async Task TestUpdate_400BadRequest()
        {
            // Arrange
            var mock = new Mock<IAdministrationService>();
            var controller = new AudioController(mock.Object);

            // Act
            var result = (await controller.Update(OtherId, SomeAudio)) as IStatusCodeActionResult;

            // Assert
            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        }

        [Fact]
        public void TestUnload_204NoContent()
        {
            // Arrange
            var mock = new Mock<IAdministrationService>();
            var controller = new AudioController(mock.Object);

            // Act
            var result = controller.Delete(SomeFiles) as IStatusCodeActionResult;

            // Assert
            Assert.Equal(StatusCodes.Status204NoContent, result.StatusCode);
        }

        [Fact]
        public void TestDelete_204NoContent()
        {
            // Arrange
            var mock = new Mock<IAdministrationService>();
            var controller = new AudioController(mock.Object);

            // Act
            var result = controller.Delete(SomeId) as IStatusCodeActionResult;

            // Assert
            Assert.Equal(StatusCodes.Status204NoContent, result.StatusCode);
        }
    }
}

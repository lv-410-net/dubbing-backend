using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;
using SoftServe.ITAcademy.BackendDubbingProject.Web.ApiControllers;
using Moq;
using SoftServe.ITAcademy.BackendDubbingProject.Administration.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using SoftServe.ITAcademy.BackendDubbingProject.Administration.Core.DTOs;
using System.IO;
using Microsoft.AspNetCore.Http.Internal;
using System.Text;
using System.Web;

namespace SoftServe.ITAcademy.BackendDubbingProject.WebApiTest
{
    [ExcludeFromCodeCoverage]
    public class AudioControllerTests
    {
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
            Assert.NotEmpty(value);
            Assert.Equal(5, value.Count);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

        [Fact]
        public async Task TestGetById_200OK()
        {
            // Arrange
            var mock = new Mock<IAdministrationService>();
            int id = 1;
            mock.Setup(service => service.GetAudioByIdAsync(id)).Returns(async () => {
                return new AudioDTO { Id = 1, FileName = "File1", OriginalText = "Text1", SpeechId = 1, LanguageId = 1 };
            });
            var controller = new AudioController(mock.Object);

            // Act
            var result = (await controller.GetById(id)).Result as ObjectResult;
            var value = result.Value as AudioDTO;

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
            mock.Setup(service => service.GetAudioByIdAsync(id)).Returns(async () => { return null; });
            var controller = new AudioController(mock.Object);

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
            var controller = new AudioController(mock.Object);
            AudioDTO audio = new AudioDTO { Id = 1, FileName = "File1", OriginalText = "Text1", SpeechId = 1, LanguageId = 1 };

            // Act
            var result = (await controller.Create(audio)).Result as ObjectResult;
            var value = result.Value as AudioDTO;

            // Assert
            Assert.NotNull(value);
            Assert.Equal(audio.Id, value.Id);
            Assert.Equal(StatusCodes.Status201Created, result.StatusCode);
        }

        [Fact]
        public async Task TestUpdate_204NoContent()
        {
            // Arrange
            var mock = new Mock<IAdministrationService>();
            var controller = new AudioController(mock.Object);
            var id = 2;
            AudioDTO audio = new AudioDTO { Id = 2, FileName = "File1", OriginalText = "Text1", SpeechId = 1, LanguageId = 1 };

            // Act
            var result = (await controller.Update(id, audio)) as IStatusCodeActionResult;

            // Assert
            Assert.Equal(StatusCodes.Status204NoContent, result.StatusCode);
        }

        [Fact]
        public async Task TestUpdate_400BadRequest()
        {
            // Arrange
            var mock = new Mock<IAdministrationService>();
            var controller = new AudioController(mock.Object);
            var id = 1;
            AudioDTO audio = new AudioDTO { Id = 2, FileName = "File1", OriginalText = "Text1", SpeechId = 1, LanguageId = 1 };

            // Act
            var result = (await controller.Update(id, audio)) as IStatusCodeActionResult;

            // Assert
            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        }

        [Fact]
        public void TestUnload_204NoContent()
        {
            // Arrange
            var mock = new Mock<IAdministrationService>();
            var controller = new AudioController(mock.Object);
            string[] files = new string[] { "File1", "File2", "File3" };

            // Act
            var result = controller.Delete(files) as IStatusCodeActionResult;

            // Assert
            Assert.Equal(StatusCodes.Status204NoContent, result.StatusCode);
        }

        [Fact]
        public void TestDelete_204NoContent()
        {
            // Arrange
            var mock = new Mock<IAdministrationService>();
            var controller = new AudioController(mock.Object);
            int id = 1;

            // Act
            var result = controller.Delete(id) as IStatusCodeActionResult;

            // Assert
            Assert.Equal(StatusCodes.Status204NoContent, result.StatusCode);
        }
    }
}

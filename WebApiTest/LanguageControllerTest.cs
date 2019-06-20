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
    public class LanguageControllerTest
    {
        [Fact]
        public async Task TestGetAll_200OK()
        {
            // Arrange
            var mock = new Mock<IAdministrationService>();
            mock.Setup(service => service.GetAllLanguagesAsync()).Returns(async () => { return DullData.GetAllLanguageDTOs(); });
            var controller = new LanguageController(mock.Object);

            // Act
            var result = (await controller.GetAll()).Result as ObjectResult;
            var value = result.Value as List<LanguageDTO>;

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
            mock.Setup(service => service.GetLanguageByIdAsync(id)).Returns(async () => {
                return new LanguageDTO { Id = 1, Name = "English" };
            });
            var controller = new LanguageController(mock.Object);

            // Act
            var result = (await controller.GetById(id)).Result as ObjectResult;
            var value = result.Value as LanguageDTO;

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
            mock.Setup(service => service.GetLanguageByIdAsync(id)).Returns(async () => { return null; });
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
            LanguageDTO language = new LanguageDTO { Id = 2, Name = "English" };
            var mock = new Mock<IAdministrationService>();
            var controller = new LanguageController(mock.Object);

            // Act
            var result = (await controller.Create(language)).Result as ObjectResult;
            var value = result.Value as LanguageDTO;

            // Assert
            Assert.NotNull(value);
            Assert.Equal(language.Id, value.Id);
            Assert.Equal(StatusCodes.Status201Created, result.StatusCode);
        }

        [Fact]
        public async Task TestUpdate_204NoContent()
        {
            // Arrange
            var mock = new Mock<IAdministrationService>();
            var controller = new LanguageController(mock.Object);
            var id = 1;
            LanguageDTO language = new LanguageDTO { Id = 1, Name = "English" };

            // Act
            var result = await controller.Update(id, language) as IStatusCodeActionResult;

            // Assert
            Assert.Equal(StatusCodes.Status204NoContent, result.StatusCode);
        }

        [Fact]
        public async Task TestUpdate_400BadRequest()
        {
            // Arrange
            var mock = new Mock<IAdministrationService>();
            var controller = new LanguageController(mock.Object);
            var id = 2;
            LanguageDTO language = new LanguageDTO { Id = 1, Name = "English" };

            // Act
            var result = await controller.Update(id, language) as IStatusCodeActionResult;

            // Assert
            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task TestDelete_204NoContent()
        {
            // Arrange
            var mock = new Mock<IAdministrationService>();
            var controller = new LanguageController(mock.Object);
            var id = 2;

            // Act
            var result = await controller.Delete(id) as IStatusCodeActionResult;

            // Assert
            Assert.Equal(StatusCodes.Status204NoContent, result.StatusCode);
        }
    }
}

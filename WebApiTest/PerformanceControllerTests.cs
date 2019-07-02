﻿using Microsoft.AspNetCore.Http;
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
        private const int SomeId = 1;
        private const int OtherId = 2;
        private const string SomeTitle = "Performance1";
        private const string SomeDescription = "Description of perfomance1";
        private const string SomeName = "English";
        private const int SomeOrder = 1;
        private const string SomeText = "Speech1";
        private const int SomeDuration = 120;
        private const int SomePerfomanceId = 1;

        private static PerformanceDTO SomePerformance = new PerformanceDTO
        {
            Id = SomeId,
            Title = SomeTitle,
            Description = SomeDescription
        };

        private static SpeechDTO SomeSpeech = new SpeechDTO
        {
            Id = SomeId,
            Order = SomeOrder,
            Text = SomeText,
            Duration = SomeDuration,
            PerformanceId = SomePerfomanceId
        };

        private static LanguageDTO SomeLanguage = new LanguageDTO
        {
            Id = SomeId,
            Name = SomeName
        };


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
            Assert.NotNull(value);
            Assert.NotEmpty(value);
            Assert.Equal(3, value.Count);
            Assert.Equal(value[0].Id, SomePerformance.Id);
            Assert.Equal("Performance3", value[value.Count - 1].Title);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

        [Fact]
        public async Task TestGetById_200OK()
        {
            // Arrange
            var mock = new Mock<IAdministrationService>();
            mock.Setup(service => service.GetPerformanceByIdAsync(SomeId)).Returns(async () => { return SomePerformance; });
            var controller = new PerformanceController(mock.Object);

            // Act
            var result = (await controller.GetById(SomeId)).Result as ObjectResult;
            var value = result.Value as PerformanceDTO;

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
            mock.Setup(service => service.GetPerformanceByIdAsync(SomeId)).Returns(async () => { return null; });
            var controller = new LanguageController(mock.Object);

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
            var controller = new PerformanceController(mock.Object);

            // Act
            var result = (await controller.Create(SomePerformance)).Result as ObjectResult;
            var value = result.Value as PerformanceDTO;

            // Assert
            Assert.NotNull(value);
            Assert.Equal(SomePerformance.Id, value.Id);
            Assert.Equal(StatusCodes.Status201Created, result.StatusCode);
        }

        [Fact]
        public async Task TestUpdate_204NoContent()
        {
            // Arrange
            var mock = new Mock<IAdministrationService>();
            var controller = new PerformanceController(mock.Object);

            // Act
            var result = await controller.Update(SomeId, SomePerformance) as IStatusCodeActionResult;

            // Assert
            Assert.Equal(StatusCodes.Status204NoContent, result.StatusCode);
        }

        [Fact]
        public async Task TestUpdate_400BadRequest()
        {
            // Arrange
            var mock = new Mock<IAdministrationService>();
            var controller = new PerformanceController(mock.Object);

            // Act
            var result = await controller.Update(OtherId, SomePerformance) as IStatusCodeActionResult;

            // Assert
            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task TestDelete_204NoContent()
        {
            // Arrange
            var mock = new Mock<IAdministrationService>();
            var controller = new PerformanceController(mock.Object);

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
            mock.Setup(service => service.GetSpeechesByPerformanceIdAsync(SomeId)).Returns(async () => { return DullData.GetAllSpeechDTOs(); });
            var controller = new PerformanceController(mock.Object);

            // Act
            var result = (await controller.GetByIdWithChildren(SomeId)).Result as ObjectResult;
            var value = result.Value as List<SpeechDTO>;

            // Assert
            Assert.NotNull(value);
            Assert.NotEmpty(value);
            Assert.Equal(3, value.Count);
            Assert.Equal(value[0].Id, SomeSpeech.Id);
            Assert.Equal("Speech3", value[value.Count - 1].Text);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

        [Fact]
        public async Task TestGetByIdWithChildren_400BadRequest()
        {
            // Arrange
            var mock = new Mock<IAdministrationService>();
            mock.Setup(service => service.GetSpeechesByPerformanceIdAsync(SomeId)).Returns(async () => { return null; });
            var controller = new PerformanceController(mock.Object);

            // Act
            var result = (await controller.GetByIdWithChildren(SomeId)).Result as IStatusCodeActionResult;

            // Assert
            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task TestGetByIdLanguages_200OK()
        {
            // Arrange
            var mock = new Mock<IAdministrationService>();
            mock.Setup(service => service.GetLanguagesByPerformanceIdAsync(SomeId)).Returns(async () => { return DullData.GetAllLanguageDTOs(); });
            var controller = new PerformanceController(mock.Object);

            // Act
            var result = (await controller.GetByIdLanguages(SomeId)).Result as ObjectResult;
            var value = result.Value as List<LanguageDTO>;

            // Assert
            Assert.NotNull(value);
            Assert.NotEmpty(value);
            Assert.Equal(3, value.Count);
            Assert.Equal(value[0].Id, SomeLanguage.Id);
            Assert.Equal("Spanish", value[value.Count - 1].Name);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }
    }
}
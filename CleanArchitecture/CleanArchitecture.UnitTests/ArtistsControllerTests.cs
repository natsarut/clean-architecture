using AutoMapper;
using CleanArchitecture.ApplicationCore;
using CleanArchitecture.ApplicationCore.Entities;
using CleanArchitecture.ApplicationCore.Interfaces;
using CleanArchitecture.ApplicationCore.Models;
using CleanArchitecture.ApplicationCore.Services;
using CleanArchitecture.WebApi;
using CleanArchitecture.WebApi.Controllers;
using CleanArchitecture.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.UnitTests
{
    public class ArtistsControllerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<ILogger<GenericService<Artist>>> _genericServiceLoggerMock = new();
        private readonly Mock<ILogger<ArtistsController>> _controllerLoggerMock = new();
        private readonly Guid _id = Guid.NewGuid();

        [Fact]
        public async Task GetArtist_Returns_SingleObject()
        {
            // Arrange
            _unitOfWorkMock.Setup(x => x.Repository<Artist>().GetByIdAsync(_id, default)).ReturnsAsync(new Artist { ArtistId = _id,Name="BNK48" });
            var service = new ArtistService(_unitOfWorkMock.Object, _genericServiceLoggerMock.Object);

            // Act
            var controller = new ArtistsController(service, AutoMapperProfile.CreateMapper(), _controllerLoggerMock.Object);
            IActionResult result = await controller.GetArtist(_id);

            // Assert
            OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
            ArtistDto item = Assert.IsType<ArtistDto>(okResult.Value);
            Assert.Equal(_id, item.ArtistId);
        }

        [Fact]
        public async Task GetArtist_Returns_NotFound()
        {
            // Arrange
            _unitOfWorkMock.Setup(x => x.Repository<Artist>().GetByIdAsync(_id, default)).ReturnsAsync((Artist?)null);
            var service = new ArtistService(_unitOfWorkMock.Object, _genericServiceLoggerMock.Object);

            // Act
            var controller = new ArtistsController(service, AutoMapperProfile.CreateMapper(), _controllerLoggerMock.Object);
            IActionResult result = await controller.GetArtist(_id);

            // Assert
            NotFoundObjectResult notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal($"Artist with ArtistId {_id} not found.", notFoundResult.Value);
        }

        [Fact]
        public async Task GetArtists_Returns_Collection()
        {
            // Arrange
            var artists = new List<Artist> { new() { ArtistId = Guid.NewGuid(),Name="BNK48" }, new() { ArtistId = Guid.NewGuid(),Name="JKT48" } };
            _unitOfWorkMock.Setup(x => x.Repository<Artist>().GetAllAsync(null, string.Empty, false, default)).ReturnsAsync(artists);
            var service = new ArtistService(_unitOfWorkMock.Object, _genericServiceLoggerMock.Object);

            // Act
            var controller = new ArtistsController(service, AutoMapperProfile.CreateMapper(), _controllerLoggerMock.Object);
            IActionResult result = await controller.GetArtists();

            // Assert
            OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
            IEnumerable<ArtistDto> items = Assert.IsAssignableFrom<IEnumerable<ArtistDto>>(okResult.Value);
        }

        [Fact]
        public async Task GetPaginatedArtists_Returns_PaginatedList()
        {
            // Arrange
            List<Artist> items = [new() { ArtistId = Guid.NewGuid(), Name = "BNK48" }, new() { ArtistId = Guid.NewGuid(), Name = "JKT48" }];
            var queryStringParameters = new QueryStringParameters { PageNumber = 1, PageSize = 10 };
            var paginatedArtists = new PaginatedList<Artist>(items, items.Count, queryStringParameters.PageNumber, queryStringParameters.PageSize);
            _unitOfWorkMock.Setup(x => x.Repository<Artist>().GetPaginatedAllAsync(queryStringParameters, string.Empty, false, default)).ReturnsAsync(paginatedArtists);
            var service = new ArtistService(_unitOfWorkMock.Object, _genericServiceLoggerMock.Object);

            // Act
            var controller = new ArtistsController(service, AutoMapperProfile.CreateMapper(), _controllerLoggerMock.Object);
            IActionResult result = await controller.GetPaginatedArtists(queryStringParameters);

            // Assert
            OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
            PaginatedList<ArtistDto> paginatedList = Assert.IsType<PaginatedList<ArtistDto>>(okResult.Value);
            Assert.Equal(items.Count, paginatedList.TotalCount);
        }

        [Fact]
        public async Task PostArtist_Creates_Artist()
        {
            // Arrange
            var artistDto = new ArtistDto { ArtistId = _id, Name = "Test Artist" };
            _unitOfWorkMock.Setup(x => x.Repository<Artist>().AddAsync(It.IsAny<Artist>(), default)).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync(default)).Returns(Task.CompletedTask);
            var service = new ArtistService(_unitOfWorkMock.Object, _genericServiceLoggerMock.Object);

            // Act
            var controller = new ArtistsController(service, AutoMapperProfile.CreateMapper(), _controllerLoggerMock.Object);
            IActionResult result = await controller.PostArtist(artistDto);

            // Assert
            CreatedAtActionResult createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.NotNull(createdResult.Value); // Ensure the value is not null.
            Assert.Equal(nameof(ArtistsController.GetArtist), createdResult.ActionName);
            var createdArtistDto = Assert.IsType<ArtistDto>(createdResult.Value);
            Assert.Equal(_id, createdArtistDto.ArtistId);
        }

        [Fact]
        public async Task PutArtist_Updates_Artist()
        {
            // Arrange
            var artistDto = new ArtistForUpdateDto { ArtistId = _id, Name = "Updated Artist" };
            _unitOfWorkMock.Setup(x => x.Repository<Artist>().GetByIdAsync(_id, default)).ReturnsAsync(new Artist { ArtistId = _id,Name = "BNK48" });
            _unitOfWorkMock.Setup(x => x.Repository<Artist>().Update(It.IsAny<Artist>()));
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync(default)).Returns(Task.CompletedTask);
            var service = new ArtistService(_unitOfWorkMock.Object, _genericServiceLoggerMock.Object);

            // Act
            var controller = new ArtistsController(service, AutoMapperProfile.CreateMapper(), _controllerLoggerMock.Object);
            IActionResult result = await controller.PutArtist(_id, artistDto);

            // Assert
            NoContentResult noContentResult = Assert.IsType<NoContentResult>(result);
            _unitOfWorkMock.Verify(x => x.Repository<Artist>().Update(It.IsAny<Artist>()), Times.Once);
        }

        [Fact]
        public async Task DeleteArtist_Removes_Artist()
        {
            // Arrange
            _unitOfWorkMock.Setup(x => x.Repository<Artist>().GetByIdAsync(_id, default)).ReturnsAsync(new Artist { ArtistId = _id , Name = "BNK48" });
            _unitOfWorkMock.Setup(x => x.Repository<Artist>().Remove(It.IsAny<Artist>()));
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync(default)).Returns(Task.CompletedTask);
            var service = new ArtistService(_unitOfWorkMock.Object, _genericServiceLoggerMock.Object);

            // Act
            var controller = new ArtistsController(service, AutoMapperProfile.CreateMapper(), _controllerLoggerMock.Object);
            IActionResult result = await controller.DeleteArtist(_id);

            // Assert
            NoContentResult noContentResult = Assert.IsType<NoContentResult>(result);
            _unitOfWorkMock.Verify(x => x.Repository<Artist>().Remove(It.IsAny<Artist>()), Times.Once);
        }

    }
}

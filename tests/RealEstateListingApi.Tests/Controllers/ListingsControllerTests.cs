using Microsoft.AspNetCore.Mvc;
using Moq;
using RealEstateListingApi.Controllers;
using RealEstateListingApi.Models;
using RealEstateListingApi.Repo;
using RealEstateListingApi.Tests.Factory;
using RealEstateListingApi.UnitOfWork;
using Xunit;

namespace RealEstateListingApi.Tests.Controllers
{
    public class ListingsControllerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IListingRepo> _mockListingRepo;
        private readonly ListingsController _controller;

        public ListingsControllerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockListingRepo = new Mock<IListingRepo>();
            _mockUnitOfWork.Setup(u => u.Listings).Returns(_mockListingRepo.Object);
            _controller = new ListingsController(_mockUnitOfWork.Object);
        }

        [Fact]
        public void DeleteListing_WhenListingDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var unitOfWorkMock = UnitOfWorkMockFactory.Create(null);
            var controller = new ListingsController(unitOfWorkMock.Object);

            // Act
            var result = controller.DeleteListing(Guid.NewGuid());

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void DeleteListing_WhenListingExists_ReturnsNoContent()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var listing = new Listing { Id = guid, Title = "Test", Price = 100 };
            var unitOfWorkMock = UnitOfWorkMockFactory.Create(listing);
            var controller = new ListingsController(unitOfWorkMock.Object);

            // Act
            var result = controller.DeleteListing(guid);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void GetAllListings_ReturnsOkWithListings()
        {
            // Arrange
            var listings = new List<Listing>
            {
                new Listing { Id = Guid.NewGuid(), Title = "Test 1", Price = 100000 },
                new Listing { Id = Guid.NewGuid(), Title = "Test 2", Price = 200000 }
            };
            _mockListingRepo.Setup(r => r.GetAll()).Returns(listings);

            // Act
            var result = _controller.GetAllListings();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<Listing>>(okResult.Value);
            Assert.Equal(2, ((List<Listing>)returnValue).Count);
        }

        [Fact]
        public void AddListing_ValidListing_ReturnsCreatedAtAction()
        {
            // Arrange
            var listing = new Listing { Id = Guid.NewGuid(), Title = "Test Listing", Price = 150000 };
            _mockListingRepo.Setup(r => r.Add(It.IsAny<Listing>()));
            _mockUnitOfWork.Setup(u => u.Commit()).Returns(1);

            // Act
            var result = _controller.AddListing(listing);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnValue = Assert.IsType<Listing>(createdAtActionResult.Value);
            Assert.Equal(listing.Id, returnValue.Id);
            Assert.Equal("GetListingById", createdAtActionResult.ActionName);
        }

        [Fact]
        public void AddListing_InvalidModel_ReturnsBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("Title", "Required");
            var listing = new Listing();

            // Act
            var result = _controller.AddListing(listing);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public void GetListingById_ListingExists_ReturnsOkWithListing()
        {
            // Arrange
            var id = Guid.NewGuid();
            var listing = new Listing { Id = id, Title = "Test", Price = 12345 };
            _mockListingRepo.Setup(r => r.GetById(id)).Returns(listing);

            // Act
            var result = _controller.GetListingById(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<Listing>(okResult.Value);
            Assert.Equal(id, returnValue.Id);
        }

        [Fact]
        public void GetListingById_ListingDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockListingRepo.Setup(r => r.GetById(id)).Returns((Listing)null);

            // Act
            var result = _controller.GetListingById(id);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }
    }
}
using System.Security.Claims;
using Database.Controllers;
using Database.DTOs.Property;
using Database.Models;
using Database.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace DatabaseTest.Controllers;

public class PropertyControllerTests
{
    private readonly Mock<IPropertyRepository> _mockRepo;
    private readonly PropertyController _controller;

    private readonly Guid _testLandlordId = Guid.NewGuid();

    public PropertyControllerTests()
    {
        _mockRepo = new Mock<IPropertyRepository>();
        _controller = new PropertyController(_mockRepo.Object);

        // moq the logged in user claims
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, _testLandlordId.ToString())
        }, "mock"));

        _controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext() { User = user }
        };
    }

    [Fact]
    public async Task GetProperties_ShouldReturnAll()
    {
        // Arrange
        var list = new List<Property> { new Property(), new Property() };
        _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(list);

        // Act
        var result = await _controller.GetProperties();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnList = Assert.IsAssignableFrom<IEnumerable<Property>>(okResult.Value);
        Assert.Equal(2, returnList.Count());
    }

    [Fact]
    public async Task PostProperty_ShouldSetLandlordId_AndReturnCreated()
    {
        // Arrange
        var dto = new CreatePropertyDto
        {
            AddressLine1 = "test",
            RentPrice = 1000
        };

        // Act
        var result = await _controller.PostProperty(dto);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var createdProp = Assert.IsType<Property>(createdResult.Value);
        Assert.Equal(_testLandlordId, createdProp.LandlordId);
        Assert.True(createdProp.IsAvailable);

        _mockRepo.Verify(r => r.AddAsync(It.IsAny<Property>()), Times.Once);
        _mockRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteProperty_ShouldReturnConflict_IfNotAvailable()
    {
        // Arrange
        var id = Guid.NewGuid();
        var occupiedProp = new Property { Id = id, IsAvailable = false };

        _mockRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(occupiedProp);

        // Act
        var result = await _controller.DeleteProperty(id);

        // Assert
        var conflictResult = Assert.IsType<ConflictObjectResult>(result);
        Assert.Equal("Property cannot be deleted while occupied", conflictResult.Value);
        _mockRepo.Verify(r => r.DeleteAsync(It.IsAny<Property>()), Times.Never);
    }

    [Fact]
    public async Task DeleteProperty_ShouldReturnNoContent_IfAvailable()
    {
        // Arrange
        var id = Guid.NewGuid();
        var emptyProp = new Property { Id = id, IsAvailable = true };

        _mockRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(emptyProp);

        // Act
        var result = await _controller.DeleteProperty(id);

        // Assert
        Assert.IsType<NoContentResult>(result);
        _mockRepo.Verify(r => r.DeleteAsync(emptyProp), Times.Once);
        _mockRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
    }
}
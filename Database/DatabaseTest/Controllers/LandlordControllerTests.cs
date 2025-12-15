using System.Security.Claims;
using Database.Controllers;
using Database.DTOs.Landlord;
using Database.Models;
using Database.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace DatabaseTest.Controllers;

public class LandlordControllerTests
{
    private readonly Mock<ILandlordRepository> _mockLandlordRepo;
    private readonly Mock<IPropertyRepository> _mockPropertyRepo;
    private readonly Mock<IRentalRequestRepository> _mockRequestRepo;

    private readonly LandlordController _controller;

    private readonly Guid _testLandlordId = Guid.NewGuid();

    public LandlordControllerTests()
    {
        _mockLandlordRepo = new Mock<ILandlordRepository>();
        _mockPropertyRepo = new Mock<IPropertyRepository>();
        _mockRequestRepo = new Mock<IRentalRequestRepository>();

        _controller = new LandlordController(
            _mockLandlordRepo.Object,
            _mockPropertyRepo.Object,
            _mockRequestRepo.Object
        );

        // mock the logged in user for claims
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, _testLandlordId.ToString()),
            new Claim(ClaimTypes.Name, "Test Landlord")
        }, "mock"));

        _controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext() { User = user }
        };
    }

    [Fact]
    public async Task GetLandlord_ShouldReturnOk_WhenFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        var landlord = new Landlord { Id = id, FirstName = "john", LastName = "test", Email = "john@test.com" };

        _mockLandlordRepo.Setup(x => x.GetByIdAsync(id))
            .ReturnsAsync(landlord);

        // Act
        var result = await _controller.GetLandlord(id);

        // Assert
        var okResult = Assert.IsType<LandlordDto>(result.Value);
        Assert.Equal("john test", okResult.FullName);
    }

    [Fact]
    public async Task GetLandlord_ShouldReturnNotFound_WhenMissing()
    {
        // Arrange
        var id = Guid.NewGuid();
        _mockLandlordRepo.Setup(x => x.GetByIdAsync(id))
            .ReturnsAsync((Landlord?)null);

        // Act
        var result = await _controller.GetLandlord(id);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task CreateLandlord_ShouldReturnCreated()
    {
        // Arrange
        var dto = new CreateLandlordDto { FirstName = "john", LastName = "test", Email = "jane@test.com" };

        // Act
        var result = await _controller.CreateLandlord(dto);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var returnDto = Assert.IsType<LandlordDto>(createdResult.Value);
        Assert.Equal("john test", returnDto.FullName);
        _mockLandlordRepo.Verify(x => x.AddAsync(It.IsAny<Landlord>()), Times.Once);
        _mockLandlordRepo.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task GetMyProperties_ShouldReturnOnlyMyProperties()
    {
        // Arrange
        var myProperties = new List<Property>
        {
            new Property { Id = Guid.NewGuid(), LandlordId = _testLandlordId, AddressLine1 = "test" }
        };

        _mockPropertyRepo.Setup(x => x.GetByLandlordIdAsync(_testLandlordId))
            .ReturnsAsync(myProperties);

        // Act
        var result = await _controller.GetMyProperties();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var dtos = Assert.IsAssignableFrom<IEnumerable<Database.DTOs.Property.PropertyDto>>(okResult.Value);
        Assert.Single(dtos);
        Assert.Equal("test", dtos.First().AddressLine1);
    }

    [Fact]
    public async Task ApproveRequest_ShouldSucceed_WhenValid()
    {
        // Arrange
        var requestId = Guid.NewGuid();
        var propId = Guid.NewGuid();
        var tenantId = Guid.NewGuid();

        var request = new RentalRequest
        {
            Id = requestId,
            PropertyId = propId,
            TenantId = tenantId,
            Status = "Pending"
        };

        var property = new Property
        {
            Id = propId,
            LandlordId = _testLandlordId,
            IsAvailable = true
        };

        // Setup Mocks
        _mockRequestRepo.Setup(x => x.GetByIdAsync(requestId)).ReturnsAsync(request);
        _mockPropertyRepo.Setup(x => x.GetByIdAsync(propId)).ReturnsAsync(property);
        _mockPropertyRepo.Setup(x => x.IsTenantRentingAnyPropertyAsync(tenantId)).ReturnsAsync(false);
        _mockRequestRepo.Setup(x => x.GetOtherRequestsForPropertyAsync(propId, requestId))
            .ReturnsAsync(new List<RentalRequest>());

        // Act
        var result = await _controller.ApproveRequest(requestId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Request approved.", okResult.Value);
        Assert.Equal("Approved", request.Status);
        Assert.Equal(tenantId, property.TenantId);
        Assert.False(property.IsAvailable);

        _mockRequestRepo.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task ApproveRequest_ShouldFail_IfTenantAlreadyRenting()
    {
        // Arrange
        var requestId = Guid.NewGuid();
        var tenantId = Guid.NewGuid();
        var request = new RentalRequest { Id = requestId, PropertyId = Guid.NewGuid(), TenantId = tenantId, Status = "Pending" };
        var property = new Property { Id = request.PropertyId, LandlordId = _testLandlordId };

        _mockRequestRepo.Setup(x => x.GetByIdAsync(requestId)).ReturnsAsync(request);
        _mockPropertyRepo.Setup(x => x.GetByIdAsync(request.PropertyId)).ReturnsAsync(property);
        _mockPropertyRepo.Setup(x => x.IsTenantRentingAnyPropertyAsync(tenantId)).ReturnsAsync(true);

        // Act
        var result = await _controller.ApproveRequest(requestId);

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("This tenant is already actively renting another property.", badRequest.Value);
        // ensure that the request is set to declined
        Assert.Equal("Declined", request.Status);
    }
}
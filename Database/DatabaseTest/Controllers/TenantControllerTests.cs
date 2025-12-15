using System.Security.Claims;
using Database.Controllers;
using Database.DTOs.Property;
using Database.DTOs.Request;
using Database.Models;
using Database.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace DatabaseTest.Controllers;

public class TenantControllerTests
{
    private readonly Mock<ITenantRepository> _mockTenantRepo;
    private readonly Mock<IPropertyRepository> _mockPropertyRepo;
    private readonly Mock<IRentalRequestRepository> _mockRequestRepo;
    
    private readonly TenantController _controller;
    
    private readonly Guid _testTenantId = Guid.NewGuid();

    public TenantControllerTests()
    {
        _mockTenantRepo = new Mock<ITenantRepository>();
        _mockPropertyRepo = new Mock<IPropertyRepository>();
        _mockRequestRepo = new Mock<IRentalRequestRepository>();

        _controller = new TenantController(
            _mockTenantRepo.Object,
            _mockPropertyRepo.Object,
            _mockRequestRepo.Object
        );

        // mock the logged in user for http context
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, _testTenantId.ToString()),
            new Claim(ClaimTypes.Name, "Test Tenant")
        }, "mock"));

        _controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext() { User = user }
        };
    }

    [Fact]
    public async Task GetTenant_ShouldReturnOk_WhenFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        var tenant = new Tenant { Id = id, FirstName = "Test" };
        _mockTenantRepo.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(tenant);

        // Act
        var result = await _controller.GetTenant(id);

        // Assert
        Assert.Equal(tenant, result.Value);
    }

    [Fact]
    public async Task CreateRentalRequest_ShouldReturnBadRequest_IfAlreadyRenting()
    {
        // Arrange
        var propId = Guid.NewGuid();
        
        _mockPropertyRepo.Setup(x => x.IsTenantRentingAnyPropertyAsync(_testTenantId))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.CreateRentalRequest(propId);

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("You are currently renting a property and cannot request a new one.", badRequest.Value);
    }

    [Fact]
    public async Task CreateRentalRequest_ShouldReturnBadRequest_IfPropertyOccupied()
    {
        // Arrange
        var propId = Guid.NewGuid();
        
        _mockPropertyRepo.Setup(x => x.IsTenantRentingAnyPropertyAsync(_testTenantId)).ReturnsAsync(false);
        // moq returning property to simulate occupied 
        var prop = new Property { Id = propId, TenantId = Guid.NewGuid() };
        _mockPropertyRepo.Setup(x => x.GetByIdAsync(propId)).ReturnsAsync(prop);

        // Act
        var result = await _controller.CreateRentalRequest(propId);

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("Property not available.", badRequest.Value);
    }

    [Fact]
    public async Task CreateRentalRequest_ShouldReturnBadRequest_IfRequestAlreadyPending()
    {
        // Arrange
        var propId = Guid.NewGuid();
        
        _mockPropertyRepo.Setup(x => x.IsTenantRentingAnyPropertyAsync(_testTenantId)).ReturnsAsync(false);
        _mockPropertyRepo.Setup(x => x.GetByIdAsync(propId)).ReturnsAsync(new Property { Id = propId, TenantId = null });
        // ensure has pending request returns true
        _mockRequestRepo.Setup(x => x.HasPendingRequestAsync(_testTenantId, propId)).ReturnsAsync(true);

        // Act
        var result = await _controller.CreateRentalRequest(propId);

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("You already have a pending request for this property.", badRequest.Value);
    }

    [Fact]
    public async Task GetMyRental_ShouldReturnDto_WhenRenting()
    {
        // Arrange
        var myProp = new Property 
        { 
            Id = Guid.NewGuid(), 
            TenantId = _testTenantId, 
            AddressLine1 = "test",
            City = "test",
            County = "test"
        };
        
        _mockPropertyRepo.Setup(x => x.GetPropertyByTenantIdAsync(_testTenantId))
            .ReturnsAsync(myProp);

        // Act
        var result = await _controller.GetMyRental();

        // Assert
        var okResult = Assert.IsType<PropertyDto>(result.Value);
        Assert.Equal("test", okResult.AddressLine1);
    }
}
using Database.Controllers;
using Database.DTOs.Auth;
using Database.Models;
using Database.Repositories;
using Database.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace DatabaseTest.Controllers;

public class AuthControllerTests
    {
        private readonly Mock<IAuthenticationRepository> _mockAuthRepo;
        private readonly Mock<ILandlordRepository> _mockLandlordRepo;
        private readonly Mock<ITenantRepository> _mockTenantRepo;
        private readonly Mock<IJwtService> _mockJwtService; 
        
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _mockAuthRepo = new Mock<IAuthenticationRepository>();
            _mockLandlordRepo = new Mock<ILandlordRepository>();
            _mockTenantRepo = new Mock<ITenantRepository>();
            _mockJwtService = new Mock<IJwtService>();
            _controller = new AuthController(
                _mockAuthRepo.Object, 
                _mockLandlordRepo.Object, 
                _mockTenantRepo.Object, 
                _mockJwtService.Object);
            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
            };
            
            _mockJwtService.Setup(s => s.GenerateToken(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns("TEST-jwt-token-test");
        }

        [Fact]
        public async Task Signup_ShouldReturnConflict_WhenEmailExists()
        {
            // Arrange
            var dto = new SignUpDto { Email = "test@test.com", Password = "Password123", Role = "landlord" };
            
            _mockAuthRepo.Setup(repo => repo.GetByEmailAsync(dto.Email))
                .ReturnsAsync(new Authentication { Email = dto.Email });

            // Act
            var result = await _controller.Signup(dto);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result.Result);
            Assert.Equal("Email already in use.", conflictResult.Value);
        }

        [Fact]
        public async Task Signup_ShouldReturnOk_WhenEmailIsNew()
        {
            // Arrange
            var dto = new SignUpDto { 
                Email = "new@test.com", 
                Password = "Password123", 
                Role = "landlord",
                FirstName = "test",
                LastName = "test"
            };
            
            _mockAuthRepo.Setup(repo => repo.GetByEmailAsync(dto.Email))
                .ReturnsAsync((Authentication)null);

            // Act
            var result = await _controller.Signup(dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var response = Assert.IsType<AuthResponseDto>(okResult.Value);
            Assert.Equal(dto.Email, response.Email);
            _mockAuthRepo.Verify(repo => repo.AddAsync(It.IsAny<Authentication>()), Times.Once);
            _mockLandlordRepo.Verify(repo => repo.AddAsync(It.IsAny<Landlord>()), Times.Once);
        }
    }
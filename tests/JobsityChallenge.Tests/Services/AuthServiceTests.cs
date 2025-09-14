using JobsityChallenge.Core.DTOs;
using JobsityChallenge.Core.Interfaces.Services;
using JobsityChallenge.Core.Results;
using JobsityChallenge.Core.Services;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace JobsityChallenge.Tests.Services;

public class AuthServiceTests
{
    private readonly Mock<UserManager<IdentityUser>> _userManagerMock;
    private readonly Mock<IJwtTokenGenerator> _jwtTokenGeneratorMock;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        var store = new Mock<IUserStore<IdentityUser>>();
        _userManagerMock = new Mock<UserManager<IdentityUser>>(
            store.Object, null, null, null, null, null, null, null, null
        );

        _jwtTokenGeneratorMock = new Mock<IJwtTokenGenerator>();
        _authService = new AuthService(_userManagerMock.Object, _jwtTokenGeneratorMock.Object);
    }

    [Fact]
    public async Task RegisterAsync_ShouldCallUserManagerCreateAsync()
    {
        // Arrange
        var dto = new RegisterUserDto { UserName = "test", Email = "test@test.com", Password = "P@ss123" };
        _userManagerMock.Setup(m => m.CreateAsync(It.IsAny<IdentityUser>(), dto.Password))
                        .ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _authService.RegisterAsync(dto);

        // Assert
        Assert.True(result.Succeeded);
        _userManagerMock.Verify(m => m.CreateAsync(It.Is<IdentityUser>(u => u.UserName == dto.UserName && u.Email == dto.Email), dto.Password), Times.Once);
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnError_WhenUserNotFound()
    {
        // Arrange
        var dto = new LoginDto { UserName = "unknown", Password = "123" };
        _userManagerMock.Setup(m => m.FindByNameAsync(dto.UserName))
                        .ReturnsAsync((IdentityUser)null!);

        // Act
        var result = await _authService.LoginAsync(dto);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(Errors.InvalidUserOrPassword, result);
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnError_WhenPasswordInvalid()
    {
        // Arrange
        var dto = new LoginDto { UserName = "test", Password = "wrong" };
        var user = new IdentityUser { UserName = "test" };

        _userManagerMock.Setup(m => m.FindByNameAsync(dto.UserName)).ReturnsAsync(user);
        _userManagerMock.Setup(m => m.CheckPasswordAsync(user, dto.Password)).ReturnsAsync(false);

        // Act
        var result = await _authService.LoginAsync(dto);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(Errors.InvalidUserOrPassword, result);
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnToken_WhenCredentialsAreValid()
    {
        // Arrange
        var dto = new LoginDto { UserName = "test", Password = "valid" };
        var user = new IdentityUser { Id = "1", UserName = "test" };

        _userManagerMock.Setup(m => m.FindByNameAsync(dto.UserName)).ReturnsAsync(user);
        _userManagerMock.Setup(m => m.CheckPasswordAsync(user, dto.Password)).ReturnsAsync(true);
        _jwtTokenGeneratorMock.Setup(m => m.GenerateToken(user.Id, user.UserName!))
                              .Returns("jwt-token");

        // Act
        var result = await _authService.LoginAsync(dto);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("jwt-token", result.Value);
    }
}

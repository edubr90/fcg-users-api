using MassTransit;
using Moq;
using NUnit.Framework;
using FCG.Shared.Events;
using Users.Application.DTOs;
using Users.Application.Interfaces;
using Users.Application.Services;
using Users.Domain.Entities;
using Users.Domain.Interfaces;

namespace Users.UnitTests;

[TestFixture]
public class UserServiceTests
{
    private Mock<IUserRepository> _repoMock = null!;
    private Mock<IUnitOfWork> _uowMock = null!;
    private Mock<IJwtService> _jwtMock = null!;
    private Mock<IPublishEndpoint> _publishMock = null!;
    private UserService _service = null!;

    [SetUp]
    public void SetUp()
    {
        _repoMock = new Mock<IUserRepository>();
        _uowMock = new Mock<IUnitOfWork>();
        _jwtMock = new Mock<IJwtService>();
        _publishMock = new Mock<IPublishEndpoint>();
        _service = new UserService(_repoMock.Object, _uowMock.Object, _jwtMock.Object, _publishMock.Object);
    }

    [Test]
    public async Task RegisterAsync_NewUser_ShouldPublishUserCreatedEvent()
    {
        _repoMock.Setup(r => r.ExistsByEmailAsync("new@test.com", default)).ReturnsAsync(false);
        _jwtMock.Setup(j => j.GenerateToken(It.IsAny<User>())).Returns("fake-token");
        _publishMock.Setup(p => p.Publish(It.IsAny<UserCreatedEvent>(), default)).Returns(Task.CompletedTask);

        var request = new RegisterUserRequest("New User", "new@test.com", "Pass@123");
        var result = await _service.RegisterAsync(request);

        _publishMock.Verify(p => p.Publish(It.Is<UserCreatedEvent>(e => e.Email == "new@teste.com"), default), Times.Once);
        Assert.That(result.Token, Is.EqualTo("fake-token"));
    }

    [Test]
    public async Task RegisterAsync_DuplicateEmail_ShouldThrowArgumentException()
    {
        _repoMock.Setup(r => r.ExistsByEmailAsync("dup@test.com", default)).ReturnsAsync(true);

        var request = new RegisterUserRequest("User", "dup@test.com", "Pass@123");
        Assert.ThrowsAsync<ArgumentException>(() => _service.RegisterAsync(request));
    }

    [Test]
    public async Task LoginAsync_InvalidPassword_ShouldThrow()
    {
        var user = new User("Test", "test@test.com", BCrypt.Net.BCrypt.HashPassword("Correct@1"));
        _repoMock.Setup(r => r.GetByEmailAsync("test@test.com", default)).ReturnsAsync(user);

        var request = new LoginRequest("test@test.com", "Wrong@123");
        Assert.ThrowsAsync<ArgumentException>(() => _service.LoginAsync(request));
    }

    [Test]
    public async Task LoginAsync_InactiveUser_ShouldThrow()
    {
        var user = new User("Test", "test@test.com", BCrypt.Net.BCrypt.HashPassword("Pass@123"));
        user.Deactivate();
        _repoMock.Setup(r => r.GetByEmailAsync("test@test.com", default)).ReturnsAsync(user);

        var request = new LoginRequest("test@test.com", "Pass@123");
        Assert.ThrowsAsync<ArgumentException>(() => _service.LoginAsync(request));
    }

    [Test]
    public async Task GetByIdAsync_NotFound_ShouldThrow()
    {
        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), default)).ReturnsAsync((User?)null);
        Assert.ThrowsAsync<KeyNotFoundException>(() => _service.GetByIdAsync(Guid.NewGuid()));
    }

    [Test]
    public async Task DeleteAsync_ShouldDeactivateUser()
    {
        var user = new User("Test", "test@test.com", "hash");
        _repoMock.Setup(r => r.GetByIdAsync(user.Id, default)).ReturnsAsync(user);

        await _service.DeleteAsync(user.Id);

        Assert.That(user.IsActive, Is.False);
        _repoMock.Verify(r => r.UpdateAsync(user, default), Times.Once);
        _uowMock.Verify(u => u.CommitAsync(default), Times.Once);
    }

    [Test]
    public async Task RegisterAsync_WeakPassword_ShouldThrow()
    {
        var request = new RegisterUserRequest("User", "user@test.com", "weakpwd");
        Assert.ThrowsAsync<ArgumentException>(() => _service.RegisterAsync(request));
    }
}

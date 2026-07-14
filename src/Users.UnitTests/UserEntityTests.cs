using NUnit.Framework;
using Users.Domain.Entities;
using Users.Domain.Enums;

namespace Users.UnitTests;

[TestFixture]
public class UserEntityTests
{
    [Test]
    public void Constructor_ShouldInitializeDefaultValues()
    {
        var user = new User("John", "john@test.com", "hashedpwd");

        Assert.That(user.Name, Is.EqualTo("John"));
        Assert.That(user.Email, Is.EqualTo("john@test.com"));
        Assert.That(user.Role, Is.EqualTo(UserRole.User));
        Assert.That(user.IsActive, Is.True);
        Assert.That(user.Id, Is.Not.EqualTo(Guid.Empty));
    }

    [Test]
    public void SetEmail_InvalidFormat_ShouldThrow()
    {
        var user = new User("John", "john@test.com", "hash");
        Assert.Throws<ArgumentException>(() => user.SetEmail("not-an-email"));
    }

    [Test]
    public void SetName_EmptyString_ShouldThrow()
    {
        var user = new User("John", "john@test.com", "hash");
        Assert.Throws<ArgumentException>(() => user.SetName(""));
    }

    [Test]
    public void PromoteToAdmin_ShouldChangeRole()
    {
        var user = new User("John", "john@test.com", "hash");
        user.PromoteToAdmin();
        Assert.That(user.Role, Is.EqualTo(UserRole.Admin));
    }

    [Test]
    public void Deactivate_ShouldSetIsActiveFalse()
    {
        var user = new User("John", "john@test.com", "hash");
        user.Deactivate();
        Assert.That(user.IsActive, Is.False);
    }

    [Test]
    public void Activate_ShouldSetIsActiveTrue()
    {
        var user = new User("John", "john@test.com", "hash");
        user.Deactivate();
        user.Activate();
        Assert.That(user.IsActive, Is.True);
    }

    [Test]
    public void UpdatePassword_Empty_ShouldThrow()
    {
        var user = new User("John", "john@test.com", "hash");
        Assert.Throws<ArgumentException>(() => user.UpdatePassword(""));
    }
}

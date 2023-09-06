using InvesTime.BackEnd.Data.Repositories;
using InvesTime.BackEnd.Helpers;
using InvesTime.BackEnd.Models;
using InvesTime.BackEnd.Models.DTO;
using InvesTime.BackEnd.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Text;

namespace InvesTime.Tests;

[TestClass]
public class UserServiceTests
{
    private UserService _userService;
    private Mock<IUserRepository> _userRepositoryMock;
    private Mock<IMeetingService> _meetingServiceMock;
    private Mock<IUserHelper> _userHelperMock;
    private Mock<IUserStatisticsRepository> _userStatisticsRepositoryMock;
    private Mock<IConfiguration> _configurationMock;

    [TestInitialize]
    public void Initialize()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _meetingServiceMock = new Mock<IMeetingService>();
        _userHelperMock = new Mock<IUserHelper>();
        _userStatisticsRepositoryMock = new Mock<IUserStatisticsRepository>();
        _configurationMock = new Mock<IConfiguration>();

        _userService = new UserService(
            _configurationMock.Object,
            _userHelperMock.Object,
            _userRepositoryMock.Object,
            _meetingServiceMock.Object,
            _userStatisticsRepositoryMock.Object
        );
    }

    [TestMethod]
    public void CreateUserWithDefaultPassword_ValidInput_ReturnsUser()
    {
        // Arrange
        var registrationRequest = new RegistrationRequest
        {
            FirstName = "John",
            LastName = "Doe",
            Username = "johndoe",
            ManagerUsername = "manager",
            Email = "johndoe@example.com"
        };

        _userRepositoryMock.Setup(repo => repo.CreateUser(It.IsAny<User>())).Returns(new User());

        // Act
        var result = _userService.CreateUserWithDefaultPassword(registrationRequest);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(User));
    }


    [TestMethod]
    public void DeleteConsultant_AdminDeletesConsultant_ReturnsTrue()
    {
        // Arrange
        var username = "consultant1";
        var user = new User
        {
            Username = username,
            IsAdmin = false
        };

        _userRepositoryMock.Setup(repo => repo.GetUserByUsername(username)).Returns(user);
        _meetingServiceMock.Setup(service => service.DeleteAllMeetingsOfUserId(user.Id)).Returns(true);
        _userStatisticsRepositoryMock.Setup(repo => repo.DeleteUserStatistics(username)).Returns(true);
        _userRepositoryMock.Setup(repo => repo.DeleteUser(user.Id)).Returns(true);

        // Act
        var result = _userService.DeleteConsultant(username);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void CreateToken_UserIsAdmin_CreatesTokenWithAdminRole()
    {
        // Arrange
        var user = new User
        {
            Username = "admin",
            IsAdmin = true
        };

        _configurationMock.Setup(config => config.GetSection("Authentication:Token").Value).Returns("YourSecretKey");

        // Act
        var token = _userService.CreateToken(user);

        // Assert
        Assert.IsNotNull(token);
    }

    [TestMethod]
    public void GetAllConsultantsUnderManager_AdminUser_ReturnsConsultants()
    {
        // Arrange
        var adminUsername = "admin";
        _userHelperMock.Setup(helper => helper.IsCurrentUserAdmin()).Returns(true);
        _userHelperMock.Setup(helper => helper.GetCurrentUserUsername()).Returns(adminUsername);

        var consultants = new List<User>
    {
        new User { Username = "consultant1" },
        new User { Username = "consultant2" }
    };

        _userRepositoryMock.Setup(repo => repo.GetAllConsultantUsernamesUnderManager(adminUsername)).Returns(consultants);

        // Act
        var result = _userService.GetAllConsultantsUnderManager();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(2, result.Count);
    }
}

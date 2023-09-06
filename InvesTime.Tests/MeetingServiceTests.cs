using InvesTime.BackEnd.Data.Repositories;
using InvesTime.BackEnd.Helpers;
using InvesTime.BackEnd.Models.DTO;
using InvesTime.BackEnd.Models;
using InvesTime.BackEnd.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.IdentityModel.Tokens;

namespace InvesTime.Tests;

[TestClass]
public class MeetingServiceTests
{
    [TestMethod]
    public void GetMeetings_ReturnsListOfMeetings()
    {
        // Arrange
        var userId = "userId";
        var userHelperMock = new Mock<IUserHelper>();
        userHelperMock.Setup(helper => helper.GetCurrentUserId()).Returns(userId);

        var repositoryMock = new Mock<IMeetingRepository>();
        repositoryMock.Setup(repo => repo.GetMeetingsByUserId(userId)).Returns(new List<Meeting>());

        var service = new MeetingService(repositoryMock.Object, Mock.Of<IUserRepository>(), userHelperMock.Object, Mock.Of<IUserStatisticsService>());

        // Act
        var result = service.GetMeetings();

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(IList<Meeting>));
    }

    [TestMethod]
    public void GetFirstThreeUpcomingMeetings_ReturnsUpcomingMeetings()
    {
        // Arrange
        var userId = "userId";
        var userHelperMock = new Mock<IUserHelper>();
        userHelperMock.Setup(helper => helper.GetCurrentUserId()).Returns(userId);

        var repositoryMock = new Mock<IMeetingRepository>();
        repositoryMock.Setup(repo => repo.GetMeetingsByUserId(userId)).Returns(new List<Meeting>
        {
            new Meeting { Date = DateTime.Now.AddHours(1) },
            new Meeting { Date = DateTime.Now.AddHours(2) },
            new Meeting { Date = DateTime.Now.AddHours(3) },
            new Meeting { Date = DateTime.Now.AddHours(-1) }, // Past meeting
        });

        var service = new MeetingService(repositoryMock.Object, Mock.Of<IUserRepository>(), userHelperMock.Object, Mock.Of<IUserStatisticsService>());

        // Act
        var result = service.GetFirstThreeUpcomingMeetings();

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(IList<Meeting>));
        Assert.AreEqual(3, result.Count);
    }

    [TestMethod]
    public void GetFirstThreeUpcomingMeetings_NoUpcomingMeetings_ReturnsEmptyList()
    {
        // Arrange
        var userId = "userId";
        var userHelperMock = new Mock<IUserHelper>();
        userHelperMock.Setup(helper => helper.GetCurrentUserId()).Returns(userId);

        var repositoryMock = new Mock<IMeetingRepository>();
        repositoryMock.Setup(repo => repo.GetMeetingsByUserId(userId)).Returns(new List<Meeting>
        {
            new Meeting { Date = DateTime.Now.AddHours(-1) },
            new Meeting { Date = DateTime.Now.AddHours(-2) },
        });

        var service = new MeetingService(repositoryMock.Object, Mock.Of<IUserRepository>(), userHelperMock.Object, Mock.Of<IUserStatisticsService>());

        // Act
        var result = service.GetFirstThreeUpcomingMeetings();

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(IList<Meeting>));
        Assert.AreEqual(0, result.Count);
    }

    [TestMethod]
    public async Task AddMeeting_ValidMeetingDto_AddsMeeting()
    {
        // Arrange
        var meetingDto = new MeetingDto { /* initialize with valid data */ };
        var userHelperMock = new Mock<IUserHelper>();
        userHelperMock.Setup(helper => helper.GetCurrentUserId()).Returns("userId");
        userHelperMock.Setup(helper => helper.IsCurrentUserAdmin()).Returns(true);
        userHelperMock.Setup(helper => helper.GetCurrentUserUsername()).Returns("admin");

        var userRepositoryMock = new Mock<IUserRepository>();

        var users = new List<User>
        {
            new User { Username = "consultant1" },
            new User { Username = "consultant2" }
        };

        userRepositoryMock.Setup(repo => repo.GetAllConsultantUsernamesUnderManager("admin"))
            .Returns(users);

        var repositoryMock = new Mock<IMeetingRepository>();
        repositoryMock.Setup(repo => repo.AddMeeting(It.IsAny<Meeting>())).Returns(Task.CompletedTask);

        var statisticsServiceMock = new Mock<IUserStatisticsService>();

        var service = new MeetingService(repositoryMock.Object, userRepositoryMock.Object, userHelperMock.Object, statisticsServiceMock.Object);

        // Act
        var result = await service.AddMeeting(meetingDto);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(Meeting));
    }


    [TestMethod]
    public void GetMeetingById_NullId_ThrowsArgumentException()
    {
        // Arrange
        var service = new MeetingService(Mock.Of<IMeetingRepository>(), Mock.Of<IUserRepository>(), Mock.Of<IUserHelper>(), Mock.Of<IUserStatisticsService>());

        // Act & Assert
        Assert.ThrowsException<ArgumentException>(() => service.GetMeetingById(null));
    }


    [TestMethod]
    public void DeleteMeeting_InvalidMeetingId_ThrowsArgumentException()
    {
        // Arrange
        var service = new MeetingService(Mock.Of<IMeetingRepository>(), Mock.Of<IUserRepository>(), Mock.Of<IUserHelper>(), Mock.Of<IUserStatisticsService>());

        // Act & Assert
        Assert.ThrowsException<ArgumentException>(() => service.DeleteMeeting(null));
    }

    [TestMethod]
    public void DeleteMeeting_DifferentUserId_ThrowsSecurityTokenException()
    {
        // Arrange
        var meetingId = "validMeetingId";
        var userHelperMock = new Mock<IUserHelper>();
        userHelperMock.Setup(helper => helper.GetCurrentUserId()).Returns("userId");

        var repositoryMock = new Mock<IMeetingRepository>();
        repositoryMock.Setup(repo => repo.GetMeetingById(meetingId)).Returns(new Meeting { UserId = "differentUserId" });

        var service = new MeetingService(repositoryMock.Object, Mock.Of<IUserRepository>(), userHelperMock.Object, Mock.Of<IUserStatisticsService>());

        // Act & Assert
        Assert.ThrowsException<SecurityTokenException>(() => service.DeleteMeeting(meetingId));
    }

    [TestMethod]
    public void DeleteAllMeetingsOfUserId_ValidUserId_DeletesMeetings()
    {
        // Arrange
        var userId = "validUserId";
        var repositoryMock = new Mock<IMeetingRepository>();
        repositoryMock.Setup(repo => repo.DeleteAllMeetingsOfUserId(userId)).Returns(true);

        var service = new MeetingService(repositoryMock.Object, Mock.Of<IUserRepository>(), Mock.Of<IUserHelper>(), Mock.Of<IUserStatisticsService>());

        // Act
        var result = service.DeleteAllMeetingsOfUserId(userId);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void DeleteAllMeetingsOfUserId_InvalidUserId_ThrowsArgumentException()
    {
        // Arrange
        var service = new MeetingService(Mock.Of<IMeetingRepository>(), Mock.Of<IUserRepository>(), Mock.Of<IUserHelper>(), Mock.Of<IUserStatisticsService>());

        // Act & Assert
        Assert.ThrowsException<ArgumentException>(() => service.DeleteAllMeetingsOfUserId(null));
    }
}
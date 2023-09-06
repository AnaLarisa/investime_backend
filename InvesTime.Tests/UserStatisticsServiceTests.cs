using InvesTime.BackEnd.Data.Repositories;
using InvesTime.BackEnd.Helpers;
using InvesTime.BackEnd.Models;
using InvesTime.BackEnd.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvesTime.Tests;

[TestClass]
public class UserStatisticsServiceTests
{
    private Mock<IUserStatisticsRepository> _userStatisticsRepositoryMock;
    private Mock<IMeetingRepository> _meetingRepositoryMock;
    private Mock<IUserHelper> _userHelperMock;
    private Mock<IUserRepository> _userRepositoryMock;
    private UserStatisticsService _userStatisticsService;

    [TestInitialize]
    public void Initialize()
    {
        _userStatisticsRepositoryMock = new Mock<IUserStatisticsRepository>();
        _meetingRepositoryMock = new Mock<IMeetingRepository>();
        _userHelperMock = new Mock<IUserHelper>();
        _userRepositoryMock = new Mock<IUserRepository>();

        _userStatisticsService = new UserStatisticsService(
            _userStatisticsRepositoryMock.Object,
            _meetingRepositoryMock.Object,
            _userHelperMock.Object,
            _userRepositoryMock.Object);
    }

    [TestMethod]
    public void GetGoalsListsForCurrentUser_ReturnsGoalsList()
    {
        // Arrange
        var goalsList = new List<string> { "Goal1", "Goal2" };
        _userStatisticsRepositoryMock.Setup(repo => repo.GetUserStatisticsByUsername(It.IsAny<string>()))
            .Returns(new UserStatistics { GoalsList = goalsList });

        // Act
        var result = _userStatisticsService.GetGoalsListsForCurrentUser();

        // Assert
        CollectionAssert.AreEqual(goalsList, result.ToList());
    }

    [TestMethod]
    public void AddGoalToList_AddsGoalAndUpdateUserStatistics()
    {
        // Arrange
        var goalsList = new List<string> { "Goal1" };
        var newUserStatistics = new UserStatistics { GoalsList = goalsList };
        _userStatisticsRepositoryMock.Setup(repo => repo.GetUserStatisticsByUsername(It.IsAny<string>()))
            .Returns(newUserStatistics);
        _userStatisticsRepositoryMock.Setup(repo => repo.UpdateUserStatistics(It.IsAny<UserStatistics>()))
            .Callback<UserStatistics>(updatedUserStatistics =>
            {
                newUserStatistics = updatedUserStatistics;
            });

        // Act
        _userStatisticsService.AddGoalToList("NewGoal");

        // Assert
        Assert.AreEqual(2, newUserStatistics.GoalsList.Count);
        Assert.IsTrue(newUserStatistics.GoalsList.Contains("NewGoal"));
    }

    [TestMethod]
    public void RemoveGoalFromList_RemovesGoalAndUpdateUserStatistics()
    {
        // Arrange
        var goalsList = new List<string> { "Goal1", "Goal2" };
        var newUserStatistics = new UserStatistics { GoalsList = goalsList };
        _userStatisticsRepositoryMock.Setup(repo => repo.GetUserStatisticsByUsername(It.IsAny<string>()))
            .Returns(newUserStatistics);
        _userStatisticsRepositoryMock.Setup(repo => repo.UpdateUserStatistics(It.IsAny<UserStatistics>()))
            .Callback<UserStatistics>(updatedUserStatistics =>
            {
                newUserStatistics = updatedUserStatistics;
            });

        // Act
        _userStatisticsService.RemoveGoalFromList("Goal1");

        // Assert
        Assert.AreEqual(1, newUserStatistics.GoalsList.Count);
        Assert.IsFalse(newUserStatistics.GoalsList.Contains("Goal1"));
    }

    [TestMethod]
    public void SetTargetNrOfClientsPerYear_SetsTargetNrOfClientsAndUpdatesUserStatistics()
    {
        // Arrange
        var newUserStatistics = new UserStatistics();
        _userStatisticsRepositoryMock.Setup(repo => repo.GetUserStatisticsByUsername(It.IsAny<string>()))
            .Returns(newUserStatistics);
        _userStatisticsRepositoryMock.Setup(repo => repo.UpdateUserStatistics(It.IsAny<UserStatistics>()))
            .Callback<UserStatistics>(updatedUserStatistics =>
            {
                newUserStatistics = updatedUserStatistics;
            });

        // Act
        _userStatisticsService.SetTargetNrOfClientsPerYear(100);

        // Assert
        Assert.AreEqual(100, newUserStatistics.TargetNrOfClientsPerYear);
    }

    [TestMethod]
    public void IncreaseNrOfClientsCount_IncreasesClientsCountAndUpdatesUserStatistics()
    {
        // Arrange
        var newUserStatistics = new UserStatistics { ClientsCount = 50 };
        _userStatisticsRepositoryMock.Setup(repo => repo.GetUserStatisticsByUsername(It.IsAny<string>()))
            .Returns(newUserStatistics);
        _userStatisticsRepositoryMock.Setup(repo => repo.UpdateUserStatistics(It.IsAny<UserStatistics>()))
            .Callback<UserStatistics>(updatedUserStatistics =>
            {
                newUserStatistics = updatedUserStatistics;
            });

        // Act
        _userStatisticsService.IncreaseNrOfClientsCount();

        // Assert
        Assert.AreEqual(51, newUserStatistics.ClientsCount);
    }

    [TestMethod]
    public void DecreaseNrOfContractsSignedPerYear_DecreasesContractsSignedAndUpdatesUserStatistics()
    {
        // Arrange
        var newUserStatistics = new UserStatistics { ContractsSigned = 10 };
        _userStatisticsRepositoryMock.Setup(repo => repo.GetUserStatisticsByUsername(It.IsAny<string>()))
            .Returns(newUserStatistics);
        _userStatisticsRepositoryMock.Setup(repo => repo.UpdateUserStatistics(It.IsAny<UserStatistics>()))
            .Callback<UserStatistics>(updatedUserStatistics =>
            {
                newUserStatistics = updatedUserStatistics;
            });

        // Act
        _userStatisticsService.DecreaseNrOfContractsSignedPerYear();

        // Assert
        Assert.AreEqual(9, newUserStatistics.ContractsSigned);
    }

    [TestMethod]
    public void DecreaseNrOfClientsCount_DecreasesClientsCountAndUpdatesUserStatistics()
    {
        // Arrange
        var newUserStatistics = new UserStatistics { ClientsCount = 30 };
        _userStatisticsRepositoryMock.Setup(repo => repo.GetUserStatisticsByUsername(It.IsAny<string>()))
            .Returns(newUserStatistics);
        _userStatisticsRepositoryMock.Setup(repo => repo.UpdateUserStatistics(It.IsAny<UserStatistics>()))
            .Callback<UserStatistics>(updatedUserStatistics =>
            {
                newUserStatistics = updatedUserStatistics;
            });

        // Act
        _userStatisticsService.DecreaseNrOfClientsCount();

        // Assert
        Assert.AreEqual(29, newUserStatistics.ClientsCount);
    }
}
using InvesTime.BackEnd.Data.Repositories;
using InvesTime.BackEnd.Models;
using InvesTime.BackEnd.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace InvesTime.Tests;

[TestClass]
public class ArticleFromManagerServiceTests
{
    [TestMethod]
    public void GetArticles_ReturnsListOfArticles()
    {
        // Arrange
        var repositoryMock = new Mock<IArticleFromManagerRepository>();
        repositoryMock.Setup(repo => repo.GetArticles()).Returns(new List<ArticleFromManager>());

        var service = new ArticleFromManagerService(repositoryMock.Object);

        // Act
        var result = service.GetArticles();

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(IEnumerable<ArticleFromManager>));
    }

    [TestMethod]
    public void GetArticleById_ValidId_ReturnsArticle()
    {
        // Arrange
        var articleId = "validId";
        var repositoryMock = new Mock<IArticleFromManagerRepository>();
        repositoryMock.Setup(repo => repo.GetArticleById(articleId)).Returns(new ArticleFromManager());

        var service = new ArticleFromManagerService(repositoryMock.Object);

        // Act
        var result = service.GetArticleById(articleId);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(ArticleFromManager));
    }

    [TestMethod]
    public void GetArticleById_NullId_ThrowsArgumentException()
    {
        // Arrange
        var service = new ArticleFromManagerService(Mock.Of<IArticleFromManagerRepository>());

        // Act & Assert
        Assert.ThrowsException<ArgumentException>(() => service.GetArticleById(null));
    }

    // Similar tests for other methods...

    [TestMethod]
    public void SearchArticles_ValidTitle_ReturnsMatchingArticles()
    {
        // Arrange
        var title = "searchTerm";
        var articles = new List<ArticleFromManager>
        {
            new ArticleFromManager { Title = "Article1" },
            new ArticleFromManager { Title = "Article2" },
            new ArticleFromManager { Title = "searchTermArticle" },
        };

        var repositoryMock = new Mock<IArticleFromManagerRepository>();
        repositoryMock.Setup(repo => repo.GetArticles()).Returns(articles);

        var service = new ArticleFromManagerService(repositoryMock.Object);

        // Act
        var result = service.SearchArticles(title);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(IEnumerable<ArticleFromManager>));
        Assert.AreEqual(1, result.Count());
        Assert.AreEqual("searchTermArticle", result.First().Title);
    }
}
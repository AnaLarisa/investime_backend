using InvesTime.BackEnd.Data.Repositories;
using InvesTime.BackEnd.Models;
using InvesTime.BackEnd.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace InvesTime.Tests;

[TestClass]
public class NewsServiceTests
{
    [TestMethod]
    public async Task GetTopBusinessHeadlines_SuccessfulResponse_ReturnsNewsList()
    {
        // Arrange
        var httpClientMock = new Mock<IHttpClientFactory>();
        var apiRepositoryMock = new Mock<INewsApiRepository>();

        // Mock the HTTP client and API repository
        var httpClient = new HttpClient();
        httpClientMock.Setup(factory => factory.CreateClient(It.IsAny<string>())).Returns(httpClient);
        apiRepositoryMock.Setup(repo => repo.GetTopBusinessHeadlines()).ReturnsAsync(GetSampleNewsResponse());

        var service = new NewsService(apiRepositoryMock.Object);

        // Act
        var newsList = await service.GetTopBusinessHeadlines();

        // Assert
        Assert.IsNotNull(newsList);
        Assert.AreEqual(2, newsList.Count); // Assuming your sample response contains 2 news articles
    }

    [TestMethod]
    public async Task GetTopBusinessHeadlines_EmptyResponse_ReturnsEmptyList()
    {
        // Arrange
        var httpClientMock = new Mock<IHttpClientFactory>();
        var apiRepositoryMock = new Mock<INewsApiRepository>();

        // Mock the HTTP client and API repository
        var httpClient = new HttpClient();
        httpClientMock.Setup(factory => factory.CreateClient(It.IsAny<string>())).Returns(httpClient);
        apiRepositoryMock.Setup(repo => repo.GetTopBusinessHeadlines()).ReturnsAsync(new List<NewsModel>());

        var service = new NewsService(apiRepositoryMock.Object);

        // Act
        var newsList = await service.GetTopBusinessHeadlines();

        // Assert
        Assert.IsNotNull(newsList);
        Assert.AreEqual(0, newsList.Count); // No news articles in the response
    }

    [TestMethod]
    public async Task GetTopBusinessHeadlines_ValidResponse_ReturnsNewsList()
    {
        // Arrange
        var httpClientMock = new Mock<IHttpClientFactory>();
        var apiRepositoryMock = new Mock<INewsApiRepository>();

        // Create a sample list of news articles as a valid response
        var validResponse = new List<NewsModel>
        {
            new NewsModel
            {
                Title = "Sample News 1",
                Author = "Author 1",
                Url = "https://example.com/news1",
                UrlToImage = "https://example.com/image1.jpg"
            },
            new NewsModel
            {
                Title = "Sample News 2",
                Author = "Author 2",
                Url = "https://example.com/news2",
                UrlToImage = "https://example.com/image2.jpg"
            }
        };

        // Mock the HTTP client to return a successful response
        var httpClient = new HttpClient();
        httpClientMock.Setup(factory => factory.CreateClient(It.IsAny<string>())).Returns(httpClient);

        // Mock the API repository to return the valid response
        apiRepositoryMock.Setup(repo => repo.GetTopBusinessHeadlines()).ReturnsAsync(validResponse);

        var service = new NewsService(apiRepositoryMock.Object);

        // Act
        var newsList = await service.GetTopBusinessHeadlines();

        // Assert
        Assert.IsNotNull(newsList);
        Assert.AreEqual(2, newsList.Count); // Valid response should contain 2 news articles
    }


    private List<NewsModel> GetSampleNewsResponse()
    {
        // Create a sample news response with two articles
        return new List<NewsModel>
            {
                new NewsModel
                {
                    Title = "Sample News 1",
                    Author = "Author 1",
                    Url = "https://example.com/news1",
                    UrlToImage = "https://example.com/image1.jpg"
                },
                new NewsModel
                {
                    Title = "Sample News 2",
                    Author = "Author 2",
                    Url = "https://example.com/news2",
                    UrlToImage = "https://example.com/image2.jpg"
                }
            };
    }
}
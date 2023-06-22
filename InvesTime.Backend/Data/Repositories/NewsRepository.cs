using InvesTime.BackEnd.Models;
using MongoDB.Bson.IO;
using System.Text.Json;

namespace InvesTime.BackEnd.Data.Repositories;

public class NewsApiRepository : INewsApiRepository
{
    private readonly HttpClient _httpClient;
    private readonly string _apiUrl;
    private readonly string _apiKey;

    public NewsApiRepository(IConfiguration configuration)
    {
        _httpClient = new HttpClient();
        _apiUrl = configuration["NewsApi:Url"]!;
        _apiKey = configuration["NewsApi:ApiKey"]!;
    }

    public async Task<List<NewsModel>> GetTopBusinessHeadlines()
    {
        var requestUrl = $"{_apiUrl}?country=ro&category=business&apiKey={_apiKey}";

        try
        {
            var response = await _httpClient.GetAsync(requestUrl);
            response.EnsureSuccessStatusCode();
            string responseContent = await response.Content.ReadAsStringAsync();
            var newsResponse = JsonSerializer.Deserialize<NewsResponseModel>(responseContent);

            return newsResponse?.Articles?.Select(a => new NewsModel
            {
                Title = a.Title,
                Author = a.Author,
                Url = a.Url,
                UrlToImage = a.UrlToImage
            }).ToList() ?? new List<NewsModel>();
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"An error occurred while calling the News API: {ex.Message}");
            return new List<NewsModel>();
        }
    }
}
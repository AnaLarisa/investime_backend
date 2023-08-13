using InvesTime.BackEnd.Helpers;
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
        var requestUrl = $"{_apiUrl}?country=us&category=business&apiKey={_apiKey}";

        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            request.Headers.Add("User-Agent", "invesTime");

            var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            string responseContent = await response.Content.ReadAsStringAsync();
            var responseObject = JsonSerializer.Deserialize<JsonElement>(responseContent);

            if (responseObject.TryGetProperty("articles", out var articlesElement) && articlesElement.ValueKind == JsonValueKind.Array)
            {
                var newsList = new List<NewsModel>();

                foreach (var article in articlesElement.EnumerateArray())
                {
                    var newsModel = new NewsModel
                    {
                        Title = JsonHelper.GetStringOrDefault(article, "title"),
                        Author = JsonHelper.GetStringOrDefault(article, "author"),
                        Url = JsonHelper.GetStringOrDefault(article, "url"),
                        UrlToImage = JsonHelper.GetStringOrDefault(article, "urlToImage")
                    };

                    newsList.Add(newsModel);
                }

                return newsList;
            }
            else
            {
                Console.WriteLine("News API response does not contain valid article data.");
                return new List<NewsModel>();
            }
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"An error occurred while calling the News API: {ex.Message}");
            return new List<NewsModel>();
        }
    }

}
﻿using InvesTime.BackEnd.Helpers;
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
        _apiUrl = Environment.GetEnvironmentVariable("NewsApiUrl")!;
        _apiKey = Environment.GetEnvironmentVariable("NewsApiKey")!;
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
                    var title = JsonHelper.GetStringOrDefault(article, "title");
                    var author = JsonHelper.GetStringOrDefault(article, "author");
                    var url = JsonHelper.GetStringOrDefault(article, "url");
                    var urlToImage = JsonHelper.GetStringOrDefault(article, "urlToImage");

                    var newsModel = CreateNewsModel(title, author, url, urlToImage);

                    if (newsModel != null)
                    {
                        newsList.Add(newsModel);
                    }
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

    public async Task<NewsModel> GetOneTopBusinessHeadline()
    {
        var newsList = await GetTopBusinessHeadlines();
        return newsList.FirstOrDefault() ?? throw new InvalidOperationException("No news returned from the API.");
    }

    private NewsModel CreateNewsModel(string title, string author, string url, string urlToImage)
    {
        if (title.Contains("[Removed]"))
        {
            return null;
        }

        if (string.IsNullOrWhiteSpace(title) && string.IsNullOrWhiteSpace(author) && string.IsNullOrWhiteSpace(urlToImage))
        {
            return null;
        }

        if (string.IsNullOrWhiteSpace(url))
        {
            return null;
        }

        return new NewsModel
        {
            Title = title,
            Author = author,
            Url = url,
            UrlToImage = urlToImage
        };
    }
}
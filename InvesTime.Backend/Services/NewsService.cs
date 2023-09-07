using InvesTime.BackEnd.Data.Repositories;
using InvesTime.BackEnd.Models;

namespace InvesTime.BackEnd.Services;

public class NewsService : INewsService
{
    private readonly INewsApiRepository _newsApiRepository;

    public NewsService(INewsApiRepository newsApiRepository)
    {
        _newsApiRepository = newsApiRepository;
    }

    public Task<List<NewsModel>> GetTopBusinessHeadlines()
    {
        return _newsApiRepository.GetTopBusinessHeadlines();
    }

    public Task<NewsModel> GetOneTopBusinessHeadline()
    {
        return _newsApiRepository.GetOneTopBusinessHeadline();
    }
}
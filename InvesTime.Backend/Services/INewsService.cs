using InvesTime.BackEnd.Models;

namespace InvesTime.BackEnd.Services;

public interface INewsService
{
    Task<List<NewsModel>> GetTopBusinessHeadlines();
    Task<NewsModel> GetOneTopBusinessHeadline();

}
using InvesTime.BackEnd.Models;

namespace InvesTime.BackEnd.Data.Repositories;

public interface INewsApiRepository
{
    Task<List<NewsModel>> GetTopBusinessHeadlines();
    Task<NewsModel> GetOneTopBusinessHeadline();
}
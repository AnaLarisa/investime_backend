using InvesTime.BackEnd.Models;
using InvesTime.BackEnd.Models.DTO;

namespace InvesTime.BackEnd.Services;

public interface IArticleFromManagerService
{
    ArticleFromManager AddArticle(ArticleFromManagerDto articleDto);
    bool DeleteArticle(string id);
    ArticleFromManager? GetArticleById(string id);
    IEnumerable<ArticleFromManager> GetArticles();
    IEnumerable<ArticleFromManager> SearchArticles(string title);
}
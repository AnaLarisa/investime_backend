using InvesTime.BackEnd.Models;
using InvesTime.BackEnd.Models.DTO;

namespace InvesTime.BackEnd.Services;

public interface IArticleFromManagerService
{
    ArticleFromManager AddArticle(ArticleFromManagerDto articleDto);
    bool UpdateArticle(string id, ArticleFromManagerDto updatedArticleDto);
    bool DeleteArticle(string id);
    ArticleFromManager? GetArticleById(string id);
    IEnumerable<ArticleFromManager> GetArticles();
    ArticleFromManager? GetLastArticle();
    IEnumerable<ArticleFromManager> SearchArticles(string title);
}
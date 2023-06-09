using backend.Models;
using backend.Models.DTO;

namespace backend.Services
{
    public interface IArticleFromManagerService
    {
        ArticleFromManager AddArticle(ArticleFromManagerDto articleDto);
        bool DeleteArticle(string id);
        ArticleFromManager? GetArticleById(string id);
        IEnumerable<ArticleFromManager> GetArticles();
        IEnumerable<ArticleFromManager> SearchArticles(string title);
    }
}
using InvesTime.BackEnd.Models;

namespace InvesTime.BackEnd.Data.Repositories;

public interface IArticleFromManagerRepository
{
    void AddArticle(ArticleFromManager article);
    bool UpdateArticle(ArticleFromManager updatedArticle);
    bool DeleteArticle(string id);
    ArticleFromManager GetArticleById(string id);
    IEnumerable<ArticleFromManager> GetArticles();
}
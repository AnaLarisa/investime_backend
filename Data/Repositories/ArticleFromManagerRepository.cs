using InvesTime.BackEnd.Models;
using MongoDB.Driver;

namespace InvesTime.BackEnd.Data.Repositories;

public class ArticleFromManagerRepository : IArticleFromManagerRepository
{
    private readonly IMongoCollection<ArticleFromManager> _articles;

    public ArticleFromManagerRepository(IMongoDatabase database)
    {
        _articles = database.GetCollection<ArticleFromManager>("articles");
    }

    public IEnumerable<ArticleFromManager> GetArticles()
    {
        return _articles.Find(_ => true).ToList();
    }

    public ArticleFromManager GetArticleById(string id)
    {
        return _articles.Find(article => article.Id == id).FirstOrDefault();
    }

    public void AddArticle(ArticleFromManager article)
    {
        _articles.InsertOne(article);
    }

    public bool DeleteArticle(string id)
    {
        var result = _articles.DeleteOne(article => article.Id == id);
        return result.DeletedCount > 0;
    }
}

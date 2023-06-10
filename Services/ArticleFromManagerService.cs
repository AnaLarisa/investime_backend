using InvesTime.BackEnd.Data.Repositories;
using InvesTime.BackEnd.Helpers;
using InvesTime.BackEnd.Models;
using InvesTime.BackEnd.Models.DTO;

namespace InvesTime.BackEnd.Services;

public class ArticleFromManagerService : IArticleFromManagerService
{
    private readonly IArticleFromManagerRepository _repository;

    public ArticleFromManagerService(IArticleFromManagerRepository repository)
    {
        _repository = repository;
    }

    public IEnumerable<ArticleFromManager> GetArticles()
    {
        return _repository.GetArticles();
    }

    public ArticleFromManager? GetArticleById(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new ArgumentException("Article ID is required.");
        }

        return _repository.GetArticleById(id);
    }

    public ArticleFromManager AddArticle(ArticleFromManagerDto articleDto)
    {
        var article = ObjectConverter.Convert<ArticleFromManagerDto, ArticleFromManager>(articleDto);
        _repository.AddArticle(article);

        return article;
    }

    public bool DeleteArticle(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new ArgumentException("Article ID is required.");
        }

        return _repository.DeleteArticle(id);
    }

    public IEnumerable<ArticleFromManager> SearchArticles(string title)
    {
        if (string.IsNullOrEmpty(title))
        {
            throw new ArgumentException("Title is required for article search.");
        }

        return _repository.GetArticles().Where(article => article.Title.Contains(title, StringComparison.OrdinalIgnoreCase));
    }
}

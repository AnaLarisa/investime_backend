using backend.Models.DTO;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[Route("api/[controller]")]
[ApiController, Authorize]
public class ArticleFromManagerController : Controller
{
    private IArticleFromManagerService _articleFromManagerService;
    public ArticleFromManagerController(IArticleFromManagerService articleFromManagerService)
    {
        _articleFromManagerService = articleFromManagerService;
    }

    [HttpGet(Name = "GetAllArticles")]
    public IActionResult GetAllArticles()
    {
        var articles = _articleFromManagerService.GetArticles();
        return Ok(articles);
    }

    [HttpGet("{id}", Name = "GetArticleById")]
    public IActionResult GetArticleById(string id)
    {
        var article = _articleFromManagerService.GetArticleById(id);
        if (article is null)
        {
            return BadRequest($"The article with id = {id} was not found");
        }
        return Ok(article);
    }

    [HttpPost(Name = "AddArticle")]
    [Authorize(Roles = "Admin")]
    public IActionResult AddArticle(ArticleFromManagerDto articleDto)
    {
        if (!ModelState.IsValid)
            return BadRequest($"One or more fields are not correct \n{articleDto}");

        var article = _articleFromManagerService.AddArticle(articleDto);
        return Ok(article);
    }

    [HttpDelete("{id}", Name = "DeleteArticle")]
    [Authorize(Roles = "Admin")]
    public IActionResult DeleteArticle(string id)
    {
        var result = _articleFromManagerService.DeleteArticle(id);
        if (result == false)
        {
            return BadRequest($"Deletion operation failed for article with id: {id}");
        }

        return Ok($"Deleted article with id = {id}");
    }

    [HttpGet("search", Name = "SearchArticles")]
    public IActionResult SearchArticles([FromQuery] string title)
    {
        var articles = _articleFromManagerService.SearchArticles(title);
        return Ok(articles);
    }
}

﻿using InvesTime.BackEnd.Models.DTO;
using InvesTime.BackEnd.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InvesTime.BackEnd.Controllers;

[Route("articlesFromManager")]
[ApiController]
[Authorize]
public class ArticleFromManagerController : Controller
{
    private readonly IArticleFromManagerService _articleFromManagerService;

    public ArticleFromManagerController(IArticleFromManagerService articleFromManagerService)
    {
        _articleFromManagerService = articleFromManagerService;
    }

    /// <summary>
    /// Gets a list of all the articles posted by the manager.
    /// </summary>
    [HttpGet(Name = "GetAllArticles")]
    public IActionResult GetAllArticles()
    {
        var articles = _articleFromManagerService.GetArticles();
        return Ok(articles);
    }


    /// <summary>
    /// Gets one of the articles posted by the manager.
    /// </summary>
    [HttpGet("last", Name = "GetLastArticle")]
    public IActionResult GetLastArticle()
    {
        var article = _articleFromManagerService.GetLastArticle();
        return Ok(article);
    }


    /// <summary>
    /// Gets an article by its Id in the database.
    /// </summary>
    /// <param name="id"></param>
    [HttpGet("{id}", Name = "GetArticleById")]
    public IActionResult GetArticleById(string id)
    {
        var article = _articleFromManagerService.GetArticleById(id);
        if (article is null) return BadRequest($"The article with id = {id} was not found");
        return Ok(article);
    }


    /// <summary>
    /// Search through articles by title.
    /// </summary>
    /// <param name="title"></param>
    [HttpGet("search", Name = "SearchArticles")]
    public IActionResult SearchArticles([FromQuery] string title)
    {
        var articles = _articleFromManagerService.SearchArticles(title);
        if (articles is null) 
            return BadRequest($"No articles found with title = {title}");

        return Ok(articles);
    }


    /// <summary>
    /// Admin - add a new article to the database
    /// </summary>
    /// <param name="articleDto"></param>
    [HttpPost(Name = "AddArticle")]
    [Authorize(Roles = "Admin")]
    public IActionResult AddArticle(ArticleFromManagerDto articleDto)
    {
        if (!ModelState.IsValid)
            return BadRequest($"One or more fields are not correct \n{articleDto}");

        var article = _articleFromManagerService.AddArticle(articleDto);
        return Ok(article);
    }


    /// <summary>
    /// Admin - Update one article by Id.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="updatedArticleDto"></param>
    /// <returns></returns>
    [HttpPut("{id}", Name = "UpdateArticle")]
    [Authorize(Roles = "Admin")]
    public IActionResult UpdateArticle(string id, [FromBody] ArticleFromManagerDto updatedArticleDto)
    {
        if (!ModelState.IsValid)
            return BadRequest($"One or more fields are not correct \n{updatedArticleDto}");

        var updated = _articleFromManagerService.UpdateArticle(id, updatedArticleDto);
        if (updated is false) 
            return BadRequest($"The article with id = {id} was not found");

        return Ok($"The article \"{updatedArticleDto.Title}\" has been updated successfully.");
    }


    /// <summary>
    /// Admin - Delete one article.
    /// </summary>
    /// <param name="id"></param>
    [HttpDelete("{id}", Name = "DeleteArticle")]
    [Authorize(Roles = "Admin")]
    public IActionResult DeleteArticle(string id)
    {
        var result = _articleFromManagerService.DeleteArticle(id);
        if (result == false) 
            return BadRequest($"Deletion operation FAILED for article with id: {id}");

        return Ok($"Deleted article with id = {id}");
    }
}
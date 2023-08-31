using InvesTime.BackEnd.Models;
using InvesTime.BackEnd.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InvesTime.BackEnd.Controllers;

[ApiController]
[Route("news")]
[Authorize]
public class NewsController : ControllerBase
{
    private readonly INewsService _newsService;

    public NewsController(INewsService newsService)
    {
        _newsService = newsService;
    }


    /// <summary>
    /// Gets the top business headlines
    /// </summary>
    [HttpGet("business")]
    public async Task<ActionResult<List<NewsModel>>> GetTopBusinessHeadlines()
    {
        var headlines = await _newsService.GetTopBusinessHeadlines();
        return Ok(headlines);
    }
}
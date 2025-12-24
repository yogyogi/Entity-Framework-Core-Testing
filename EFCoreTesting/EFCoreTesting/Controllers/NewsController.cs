using EFCoreTesting.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

[ApiController]
[Route("[controller]")]
public class NewsController : ControllerBase
{
    private readonly NewsContext _context;

    public NewsController(NewsContext context)
        => _context = context;

    [HttpGet]
    public async Task<ActionResult<News>> GetNews(string name)
    {
        var news = await _context.News.FirstOrDefaultAsync(b => b.Name == name);
        return news is null ? NotFound() : news;
    }

    [HttpGet]
    [Route("GetAllNews")]
    public IAsyncEnumerable<News> GetAllNews()
        => _context.News.OrderBy(b => b.Name).AsAsyncEnumerable();

    [HttpPost]
    public async Task<ActionResult> AddNews(string name, string url)
    {
        _context.News.Add(new News { Name = name, Url = url });
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpPost]
    public async Task<ActionResult> UpdateNewsUrl(string name, string url)
    {
        // Note: it isn't usually necessary to start a transaction for updating. This is done here for illustration purposes only.
        await using var transaction = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable);

        var news = await _context.News.FirstOrDefaultAsync(b => b.Name == name);
        if (news is null)
        {
            return NotFound();
        }

        news.Url = url;
        await _context.SaveChangesAsync();

        await transaction.CommitAsync();
        return Ok();
    }
}

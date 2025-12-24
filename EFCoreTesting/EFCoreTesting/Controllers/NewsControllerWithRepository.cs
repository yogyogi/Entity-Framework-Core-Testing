using EFCoreTesting.Models;
using Microsoft.AspNetCore.Mvc;

namespace EFCoreTesting.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NewsControllerWithRepository : ControllerBase
    {
        private readonly INewsRepository _repository;

        public NewsControllerWithRepository(INewsRepository repository)
            => _repository = repository;

        [HttpGet]
        public async Task<News> GetNews(string name)
            => await _repository.GetNewsByNameAsync(name);

        [HttpGet]
        public IAsyncEnumerable<News> GetAllNews()
            => _repository.GetAllNewsAsync();

        [HttpPost]
        public async Task AddNews(string name, string url)
        {
            _repository.AddNews(new News { Name = name, Url = url });
            await _repository.SaveChangesAsync();
        }

        [HttpPost]
        public async Task<ActionResult> UpdateNewsUrl(string name, string url)
        {
            var news = await _repository.GetNewsByNameAsync(name);
            if (news is null)
            {
                return NotFound();
            }

            news.Url = url;
            await _repository.SaveChangesAsync();

            return Ok();
        }
    }
}

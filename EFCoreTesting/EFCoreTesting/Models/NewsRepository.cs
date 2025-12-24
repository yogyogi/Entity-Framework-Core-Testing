using Microsoft.EntityFrameworkCore;

namespace EFCoreTesting.Models
{
    public class NewsRepository : INewsRepository
    {
        private readonly NewsContext _context;

        public NewsRepository(NewsContext context)
            => _context = context;

        public async Task<News> GetNewsByNameAsync(string name)
            => await _context.News.FirstOrDefaultAsync(b => b.Name == name);

        public IAsyncEnumerable<News> GetAllNewsAsync()
            => _context.News.AsAsyncEnumerable();

        public void AddNews(News news)
            => _context.Add(news);

        public async Task SaveChangesAsync()
            => await _context.SaveChangesAsync();
    }
}

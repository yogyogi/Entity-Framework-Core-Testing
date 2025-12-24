namespace EFCoreTesting.Models
{
    public interface INewsRepository
    {
        Task<News> GetNewsByNameAsync(string name);

        IAsyncEnumerable<News> GetAllNewsAsync();

        void AddNews(News news);

        Task SaveChangesAsync();
    }
}

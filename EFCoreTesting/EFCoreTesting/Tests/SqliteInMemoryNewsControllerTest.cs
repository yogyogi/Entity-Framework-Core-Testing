using EFCoreTesting.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using Xunit;

namespace EFCoreTesting.Tests
{
    public class SqliteInMemoryNewsControllerTest : IDisposable
    {
        private readonly DbConnection _connection;
        private readonly DbContextOptions<NewsContext> _contextOptions;

        public SqliteInMemoryNewsControllerTest()
        {
            // Create and open a connection. This creates the SQLite in-memory database, which will persist until the connection is closed
            // at the end of the test (see Dispose below).
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();

            // These options will be used by the context instances in this test suite, including the connection opened above.
            _contextOptions = new DbContextOptionsBuilder<NewsContext>()
                .UseSqlite(_connection)
                .Options;

            // Create the schema and seed some data
            using var context = new NewsContext(_contextOptions);

            if (context.Database.EnsureCreated())
            {
                using var viewCommand = context.Database.GetDbConnection().CreateCommand();
                viewCommand.CommandText = @"
CREATE VIEW AllResources AS
SELECT Url
FROM News;";
                viewCommand.ExecuteNonQuery();
            }

            context.AddRange(
                new News { Name = "Donald Trump wins 2025 USA Presidential Election", Url = "https://newsabc.com/usa-2025-donald-trump/" },
                new News { Name = "Elon Musk worth soared to $1 trillion", Url = "https://newsabc.com/elon-musk-trillionaire/" });
            context.SaveChanges();
        }

        NewsContext CreateContext() => new NewsContext(_contextOptions);

        public void Dispose() => _connection.Dispose();

        [Fact]
        public async Task GetNews()
        {
            using var context = CreateContext();
            var controller = new NewsController(context);

            var news = (await controller.GetNews("Elon Musk worth soared to $1 trillion")).Value;

            Assert.Equal("https://newsabc.com/elon-musk-trillionaire/", news.Url);
        }

        [Fact]
        public async Task GetAllNews()
        {
            using var context = CreateContext();
            var controller = new NewsController(context);

            var news = await controller.GetAllNews().ToListAsync();

            Assert.Collection(
                news,
                b => Assert.Equal("Donald Trump wins 2025 USA Presidential Election", b.Name),
                b => Assert.Equal("Elon Musk worth soared to $1 trillion", b.Name));
        }

        [Fact]
        public async Task AddNews()
        {
            using var context = CreateContext();
            var controller = new NewsController(context);

            await controller.AddNews("Bitcoin set to reach $1 million", "https://newsabc.com/btc-one-million/");

            var news = await context.News.SingleAsync(b => b.Name == "Bitcoin set to reach $1 million");
            Assert.Equal("https://newsabc.com/btc-one-million/", news.Url);
        }

        [Fact]
        public async Task UpdateNewsUrl()
        {
            using var context = CreateContext();
            var controller = new NewsController(context);

            await controller.UpdateNewsUrl("Elon Musk worth soared to $1 trillion", "https://newsabc.com/elon-musk-trillionaire-news-updated/");

            var news = await context.News.SingleAsync(b => b.Name == "Elon Musk worth soared to $1 trillion");
            Assert.Equal("https://newsabc.com/elon-musk-trillionaire-news-updated/", news.Url);
        }
    }
}

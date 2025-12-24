using EFCoreTesting.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Xunit;

namespace EFCoreTesting.Tests
{
    public class InMemoryNewsControllerTest
    {
        private readonly DbContextOptions<NewsContext> _contextOptions;

        public InMemoryNewsControllerTest()
        {
            _contextOptions = new DbContextOptionsBuilder<NewsContext>()
                .UseInMemoryDatabase("NewsControllerTest")
                .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            using var context = new NewsContext(_contextOptions);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.AddRange(
                new News { Name = "Donald Trump wins 2025 USA Presidential Election", Url = "https://newsabc.com/usa-2025-donald-trump/" },
                new News { Name = "Elon Musk worth soared to $1 trillion", Url = "https://newsabc.com/elon-musk-trillionaire/" });

            context.SaveChanges();
        }
        
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

        NewsContext CreateContext() => new NewsContext(_contextOptions, (context, modelBuilder) =>
        {
            modelBuilder.Entity<UrlResource>()
                .ToInMemoryQuery(() => context.News.Select(b => new UrlResource { Url = b.Url }));
        });
    }
}
using EFCoreTesting.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EFCoreTesting.Tests
{
    public class NewsControllerTest : IClassFixture<TestDatabaseFixture>
    {
        public NewsControllerTest(TestDatabaseFixture fixture)
        => Fixture = fixture;

        public TestDatabaseFixture Fixture { get; }

        [Fact]
        public async Task GetNews()
        {
            using var context = Fixture.CreateContext();
            var controller = new NewsController(context);

            var news = (await controller.GetNews("Elon Musk worth soared to $1 trillion")).Value;

            Assert.Equal("https://newsabc.com/elon-musk-trillionaire/", news.Url);
        }

        [Fact]
        public async Task GetAllNews()
        {
            using var context = Fixture.CreateContext();
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
            using var context = Fixture.CreateContext();
            context.Database.BeginTransaction();

            var controller = new NewsController(context);
            await controller.AddNews("Bitcoin set to reach $1 million", "https://newsabc.com/btc-one-million/");

            context.ChangeTracker.Clear();

            var news = await context.News.SingleAsync(b => b.Name == "Bitcoin set to reach $1 million");
            Assert.Equal("https://newsabc.com/btc-one-million/", news.Url);
        }
    }
}

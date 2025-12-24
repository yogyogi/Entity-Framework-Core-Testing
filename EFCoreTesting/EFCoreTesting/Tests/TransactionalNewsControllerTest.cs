using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EFCoreTesting.Tests
{
    [Collection("TransactionalTests")]
    public class TransactionalNewsControllerTest: IDisposable
    {
        public TransactionalNewsControllerTest(TransactionalTestDatabaseFixture fixture)
        => Fixture = fixture;

        public TransactionalTestDatabaseFixture Fixture { get; }

        [Fact]
        public async Task UpdateNewsUrl()
        {
            using (var context = Fixture.CreateContext())
            {
                var controller = new NewsController(context);
                await controller.UpdateNewsUrl("Elon Musk worth soared to $1 trillion", "https://newsabc.com/elon-musk-trillionaire-news-updated/");
            }

            using (var context = Fixture.CreateContext())
            {
                var news = await context.News.SingleAsync(b => b.Name == "Elon Musk worth soared to $1 trillion");
                Assert.Equal("https://newsabc.com/elon-musk-trillionaire-news-updated/", news.Url);
            }
        }
        
        public void Dispose()
            => Fixture.Cleanup();
    }
}

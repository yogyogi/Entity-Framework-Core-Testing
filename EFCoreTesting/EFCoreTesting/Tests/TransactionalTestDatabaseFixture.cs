using EFCoreTesting.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EFCoreTesting.Tests
{
    public class TransactionalTestDatabaseFixture
    {
        private const string ConnectionString = @"Server=(localdb)\mssqllocaldb;Database=NewsAgency;Trusted_Connection=True;ConnectRetryCount=0";

        public NewsContext CreateContext()
            => new NewsContext(
                new DbContextOptionsBuilder<NewsContext>()
                    .UseSqlServer(ConnectionString)
                    .Options);

        public TransactionalTestDatabaseFixture()
        {
            using var context = CreateContext();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            Cleanup();
        }

        public void Cleanup()
        {
            using var context = CreateContext();

            context.News.RemoveRange(context.News);

            context.AddRange(
                new News { Name = "Donald Trump wins 2025 USA Presidential Election", Url = "https://newsabc.com/usa-2025-donald-trump/" },
                new News { Name = "Elon Musk worth soared to $1 trillion", Url = "https://newsabc.com/elon-musk-trillionaire/" });
            context.SaveChanges();
        }
    }

    [CollectionDefinition("TransactionalTests")]
    public class TransactionalTestsCollection : ICollectionFixture<TransactionalTestDatabaseFixture>
    {
    }
}

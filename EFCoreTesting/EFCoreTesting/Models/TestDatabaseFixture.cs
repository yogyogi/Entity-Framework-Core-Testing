using Microsoft.EntityFrameworkCore;

namespace EFCoreTesting.Models
{
    #region TestDatabaseFixture
    public class TestDatabaseFixture
    {
        private const string ConnectionString = @"Server=(localdb)\mssqllocaldb;Database=NewsAgency;Trusted_Connection=True;ConnectRetryCount=0";

        private static readonly object _lock = new();
        private static bool _databaseInitialized;

        public TestDatabaseFixture()
        {
            lock (_lock)
            {
                if (!_databaseInitialized)
                {
                    using (var context = CreateContext())
                    {
                        context.Database.EnsureDeleted();
                        context.Database.EnsureCreated();

                        context.AddRange(
                            new News { Name = "Donald Trump wins 2025 USA Presidential Election", Url = "https://newsabc.com/usa-2025-donald-trump/" },
                            new News { Name = "Elon Musk worth soared to $1 trillion", Url = "https://newsabc.com/elon-musk-trillionaire/" });
                        context.SaveChanges();
                    }

                    _databaseInitialized = true;
                }
            }
        }

        public NewsContext CreateContext()
            => new NewsContext(
                new DbContextOptionsBuilder<NewsContext>()
                    .UseSqlServer(ConnectionString)
                    .Options);
    }
    #endregion
}

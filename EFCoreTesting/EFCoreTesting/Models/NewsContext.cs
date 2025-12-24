using Microsoft.EntityFrameworkCore;

namespace EFCoreTesting.Models
{
    public class NewsContext : DbContext
    {
        private readonly Action<NewsContext, ModelBuilder> _modelCustomizer;

        #region Constructors
        public NewsContext()
        {
        }

        public NewsContext(DbContextOptions<NewsContext> options, Action<NewsContext, ModelBuilder> modelCustomizer = null)
            : base(options)
        {
            _modelCustomizer = modelCustomizer;
        }
        #endregion

        public DbSet<News> News => Set<News>();
        public DbSet<UrlResource> UrlResources => Set<UrlResource>();

        #region OnConfiguring
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UrlResource>().HasNoKey()
                .ToView("AllResources");

            if (_modelCustomizer is not null)
            {
                _modelCustomizer(this, modelBuilder);
            }
        }
    }
}

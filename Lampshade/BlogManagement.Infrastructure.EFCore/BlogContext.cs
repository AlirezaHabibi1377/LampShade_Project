using BlogManagement.Infrastructure.EFCore.Mapping;
using Microsoft.EntityFrameworkCore;

namespace BlogManagement.Infrastructure.EFCore
{
    public class BlogContext : DbContext
    {
        public DbSet<Domain.ArticleCategoryAgg.ArticleCategory> ArticleCategories { get; set; }
        
        public BlogContext(DbContextOptions<BlogContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var assemble = typeof(ArticleCategoryMapping).Assembly;
            modelBuilder.ApplyConfigurationsFromAssembly(assemble);
            base.OnModelCreating(modelBuilder);
        }
    }
}

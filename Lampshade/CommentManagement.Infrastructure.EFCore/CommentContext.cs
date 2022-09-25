using CommentManagement.Domain.CommentAgg;
using Microsoft.EntityFrameworkCore;
using CommentManagement.Infrastructure.EFCore.Mapping;


namespace CommentManagement.Infrastructure.EFCore
{
    public class CommentContext : DbContext
    {
        public DbSet<Comment> Comments { get; set; }
        public CommentContext(DbContextOptions<CommentContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var assemble = typeof(CommentMapping).Assembly;
            modelBuilder.ApplyConfigurationsFromAssembly(assemble);
            base.OnModelCreating(modelBuilder);
        }
    }
}

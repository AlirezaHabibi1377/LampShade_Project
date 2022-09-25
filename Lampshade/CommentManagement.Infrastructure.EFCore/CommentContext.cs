using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommentManagement.Infrastructure.EFCore.Mapping;
using CommentManagement.Domain.CommentAgg;

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

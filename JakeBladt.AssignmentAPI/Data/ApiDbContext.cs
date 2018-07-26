using System.Collections.Generic;
using System.Linq;

using Microsoft.EntityFrameworkCore;

using JakeBladt.AssignmentAPI.Domain;

namespace JakeBladt.AssignmentAPI.Data
{
    public class ApiDbContext : DbContext
    {
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Assignment> Assignments { get; set; }

        public DbSet<AssignmentTag> AssignmentTags { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Program.DBConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Assignment>().HasKey(a => a.AssignmentId);
            modelBuilder.Entity<Tag>().HasKey(t => t.TagId);
            modelBuilder.Entity<AssignmentTag>().HasKey(at => new { at.AssignmentId, at.TagId });
            base.OnModelCreating(modelBuilder);
        }

        public IList<Assignment> AssignmentsByTag(string tag)
        {
            return Assignments
              .Include(a => a.AssignmentTags)
              .ThenInclude(ast => ast.Tag)
              .Where(asn => asn.AssignmentTags.Any(atag => atag.Tag.Name == tag))
              .ToList();
        }

    }
}

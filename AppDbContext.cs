using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Fakebook.Models;

namespace Fakebook.Services
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }

        public DbSet<Person> Persons { get; set; }
        public DbSet<TimelinePost> TimelinePosts { get; set; }
        public DbSet<ReplyPost> ReplyPosts { get; set; }
    }
}

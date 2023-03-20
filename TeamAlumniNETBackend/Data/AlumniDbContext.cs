using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using TeamAlumniNETBackend.Models;

namespace TeamAlumniNETBackend.Data
{
    public class AlumniDbContext : DbContext
    {
        public DbSet<User>? Users { get; set; }
        public DbSet<Group>? Groups { get; set; }
        public DbSet<Post>? Posts { get; set; }
        public DbSet<Topic>? Topics { get; set; }
        public DbSet<Event>? Events { get; set; }
        public DbSet<Rsvp>? Rsvps { get; set; }

        public AlumniDbContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Group>().HasData(DataSeeder.GetGroups());
            modelBuilder.Entity<Topic>().HasData(DataSeeder.GetTopics());
            modelBuilder.Entity<Event>().HasData(DataSeeder.GetEvents());
            modelBuilder.Entity<Post>().HasData(DataSeeder.GetPosts());
            modelBuilder.Entity<User>().HasData(DataSeeder.GetUsers());
        }
    }
}

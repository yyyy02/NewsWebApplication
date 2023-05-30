using System;
using Microsoft.EntityFrameworkCore;
using NewsWebApplication.Models;

namespace EntityFrameworkDemo.DB;

public class DBContext : DbContext
{
    public DbSet<User> User { get; set; }
    public DbSet<New> New { get; set; }
    public DbSet<Comment> Comment { get; set; }

    public DbSet<Feedback> Feedback { get; set; }

    public DbSet<Recommend> Recommend { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Data Source=localhost;Database=NewApplication;User ID=sa;Password=123456;TrustServerCertificate=true");
    }
}

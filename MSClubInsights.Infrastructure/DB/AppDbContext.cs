using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MSClubInsights.Domain.Entities;
using MSClubInsights.Domain.Entities.Identity;

namespace MSClubInsights.Infrastructure.DB;

public class AppDbContext : IdentityDbContext<AppUser> // Adding Identity Tables.
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<AppUser> users { get; set; }
    public DbSet<Category> categories { get; set; }

    public DbSet<Tag> tags { get; set; }

    public DbSet<Article> articles { get; set; }

    public DbSet<ArticleTag> articlesTags { get; set; }

    public DbSet<City> cities { get; set; }
    
    public DbSet<Comment> comments { get; set; }

    public DbSet<Rating> ratings { get; set; }

    public DbSet<Like> likes { get; set; }

    public DbSet<Univeristy> Univeristy { get; set; }

    public DbSet<College> College { get; set; }

   
}
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MSClubInsights.Domain.Entities.Identity;

namespace MSClubInsights.Infrastructure.DB;

public class AppDbContext : IdentityDbContext<AppUser> // Adding Identity Tables.
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    
    
    
}
using Microsoft.AspNetCore.Identity;

namespace MSClubInsights.Domain.Entities.Identity;

public class AppUser : IdentityUser
{
    public string PhoneNumber { get; set; } = string.Empty;
    
    // TODO: Add City_ID HERE
    
}
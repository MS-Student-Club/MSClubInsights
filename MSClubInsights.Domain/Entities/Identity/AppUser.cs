using Microsoft.AspNetCore.Identity;

namespace MSClubInsights.Domain.Entities.Identity;

public class AppUser : IdentityUser
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public bool gender { get; set; }

    public DateOnly DateOfBirth { get; set; }

    public string PhoneNumber { get; set; }
    
    public int CityId { get; set; }

    public City City { get; set; }

}
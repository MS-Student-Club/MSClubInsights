using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace MSClubInsights.Domain.Entities.Identity;

public class AppUser : IdentityUser
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public bool gender { get; set; }

    public DateOnly DateOfBirth { get; set; }

    public string PhoneNumber { get; set; }

    [ForeignKey("City")]
    public int CityId { get; set; }

    public City City { get; set; }

    [ForeignKey("College")]

    public int collegeId { get; set; }

    public College College { get; set; }

    [ForeignKey("Univeristy")]
    public int univeristyId { get; set; }

    public Univeristy Univeristy { get; set; }

}
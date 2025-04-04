using System.ComponentModel.DataAnnotations;


namespace MSClubInsights.Shared.DTOs.City
{
    public class CityUpdateDTO
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
    }
}

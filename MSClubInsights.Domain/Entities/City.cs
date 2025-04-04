using System.ComponentModel.DataAnnotations;


namespace MSClubInsights.Domain.Entities
{
    public class City
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)] 
        public string Name { get; set; }
    }
}

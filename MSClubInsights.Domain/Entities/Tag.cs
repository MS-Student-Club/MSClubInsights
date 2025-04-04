using System.ComponentModel.DataAnnotations;


namespace MSClubInsights.Domain.Entities
{
    public class Tag
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)] 
        public string Name { get; set; }
    }
}

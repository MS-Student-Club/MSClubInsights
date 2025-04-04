using System.ComponentModel.DataAnnotations;


namespace MSClubInsights.Shared.DTOs.Tag
{
    public class TagCreateDTO
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

    }
}

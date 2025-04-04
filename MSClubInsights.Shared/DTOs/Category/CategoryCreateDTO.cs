using System.ComponentModel.DataAnnotations;

namespace MSClubInsights.Shared.DTOs.Category
{
    public class CategoryCreateDTO
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

    }
}

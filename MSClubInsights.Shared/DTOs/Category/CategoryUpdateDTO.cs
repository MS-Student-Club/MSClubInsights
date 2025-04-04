using System.ComponentModel.DataAnnotations;

namespace MSClubInsights.Shared.DTOs.Category
{
    public class CategoryUpdateDTO
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

    }
}

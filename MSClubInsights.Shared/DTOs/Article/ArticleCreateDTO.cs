using System.ComponentModel.DataAnnotations;

namespace MSClubInsights.Shared.DTOs.Article
{
    public class ArticleCreateDTO
    {
        [Required]

        public string ImageUrl { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

        [Required]

        public string Content { get; set; }

        [Required]

        public int CategoryId { get; set; }

    }
}

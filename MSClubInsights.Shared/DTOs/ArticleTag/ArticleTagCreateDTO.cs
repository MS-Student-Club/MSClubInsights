using System.ComponentModel.DataAnnotations;

namespace MSClubInsights.Shared.DTOs.ArticleTag
{
    public class ArticleTagCreateDTO
    {
        [Required]
        public int ArticleId { get; set; }

        [Required]
        public int TagId { get; set; }


    }
}

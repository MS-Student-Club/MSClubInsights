using System.ComponentModel.DataAnnotations;


namespace MSClubInsights.Shared.DTOs.Like
{
    public class LikeCreateDTO
    {
        [Required]
        public int ArticleId { get; set; }


    }
}

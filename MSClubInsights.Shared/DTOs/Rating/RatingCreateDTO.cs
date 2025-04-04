using System.ComponentModel.DataAnnotations;


namespace MSClubInsights.Shared.DTOs.Rating
{
    public class RatingCreateDTO
    {
        [Required]
        [Range(1, 5)] // Adjust range based on business logic
        public int Value { get; set; }

        [Required]
        public int ArticleId { get; set; }

    }
}

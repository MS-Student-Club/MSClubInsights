using System.ComponentModel.DataAnnotations;


namespace MSClubInsights.Shared.DTOs.Comment
{
    public class CommentUpdateDTO
    {
        [Required]
        [MaxLength(500)]
        public string Content { get; set; }

        [Required]
        public int ArticleId { get; set; }

    }
}

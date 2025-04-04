using MSClubInsights.Domain.Entities.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MSClubInsights.Domain.Entities
{
    public class Comment
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(500)] 
        public string Content { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;

        [ForeignKey("Article")]
        public int ArticleId { get; set; }

        public Article Article { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public AppUser User { get; set; }
    }
}

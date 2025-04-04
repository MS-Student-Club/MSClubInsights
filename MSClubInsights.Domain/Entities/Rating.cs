using MSClubInsights.Domain.Entities.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MSClubInsights.Domain.Entities
{
    public class Rating
    {
       public int Id { get; set; }

        [Range(1, 5)] // Adjust range based on business logic
        public int Value { get; set; }

        [ForeignKey("Article")]
        public int ArticleId { get; set; }
        public Article Article { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public AppUser User { get; set; }
    }
}

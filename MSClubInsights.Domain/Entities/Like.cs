using MSClubInsights.Domain.Entities.Identity;
using System.ComponentModel.DataAnnotations.Schema;


namespace MSClubInsights.Domain.Entities
{
    public class Like
    {
        public int Id { get; set; }

        [ForeignKey("Article")]
        public int ArticleId { get; set; }
        public Article Article { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public AppUser User { get; set; }
    }
}

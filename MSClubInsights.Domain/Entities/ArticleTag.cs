using System.ComponentModel.DataAnnotations.Schema;


namespace MSClubInsights.Domain.Entities
{
    public class ArticleTag
    {
        public int Id { get; set; }

        [ForeignKey("Article")]
        public int ArticleId { get; set; }
        public Article Article { get; set; }

        [ForeignKey("tag")]
        public int TagId { get; set; }
        public Tag Tag { get; set; }
    }
}

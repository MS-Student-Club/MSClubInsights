using System.ComponentModel.DataAnnotations;


namespace MSClubInsights.Domain.Entities
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}

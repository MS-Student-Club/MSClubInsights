using MSClubInsights.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSClubInsights.Domain.Entities
{
    public class Article
    {
        public int Id { get; set; }

        public string ImageUrl { get; set; }

        [Required]
        [MaxLength(255)] 
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;

        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        public Category Category { get; set; }

        [ForeignKey("Author")]
        public string AuthorId { get; set; }

        public AppUser Author { get; set; }

    }
}

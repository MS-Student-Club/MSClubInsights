using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSClubInsights.Shared.DTOs.Comment
{
    public class CommentCreateDTO
    {
        [Required]
        [MaxLength(500)]
        public string Content { get; set; }

        [Required]
        public int ArticleId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSClubInsights.Shared.DTOs.ArticleTag
{
    public class ArticleTagUpdateDTO
    {
        [Required]
        public int ArticleId { get; set; }

        [Required]
        public int TagId { get; set; }

    }
}

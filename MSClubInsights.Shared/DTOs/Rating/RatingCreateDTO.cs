using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

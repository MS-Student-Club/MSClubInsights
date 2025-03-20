using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSClubInsights.Shared.DTOs.Tag
{
    public class TagCreateDTO
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSClubInsights.Shared.DTOs.Auth
{
    public class RegisterRequestDTO
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string PhoneNumber { get; set; }

        public DateOnly DateOfBirth { get; set; }

        public bool gender { get; set; }

        public int CityId { get; set; }
    }
}

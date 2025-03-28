using MSClubInsights.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSClubInsights.Application.ServiceInterfaces
{
    public interface ITokenService
    {
        Task<string> GenerateJwtToken(AppUser user);
    }
}

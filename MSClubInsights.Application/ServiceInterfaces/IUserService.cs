using MSClubInsights.Shared.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSClubInsights.Application.ServiceInterfaces
{
    public interface IUserService
    {
        Task<LoginResponseDTO> Register(RegisterRequestDTO request);
    }
}

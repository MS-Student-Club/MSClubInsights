using MSClubInsights.Shared.DTOs.Auth;

namespace MSClubInsights.Application.ServiceInterfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDTO> Login(LoginRequestDTO request);
    }
}

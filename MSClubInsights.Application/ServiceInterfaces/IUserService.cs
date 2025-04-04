using MSClubInsights.Shared.DTOs.Auth;


namespace MSClubInsights.Application.ServiceInterfaces
{
    public interface IUserService
    {
        Task<LoginResponseDTO> Register(RegisterRequestDTO request);
    }
}

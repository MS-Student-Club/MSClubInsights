using MSClubInsights.Domain.Entities.Identity;


namespace MSClubInsights.Application.ServiceInterfaces
{
    public interface ITokenService
    {
        Task<string> GenerateJwtToken(AppUser user);
    }
}

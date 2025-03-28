using MSClubInsights.Application.ServiceInterfaces;
using MSClubInsights.Domain.RepoInterfaces;
using MSClubInsights.Shared.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSClubInsights.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        public AuthService(IUserRepository userRepository , ITokenService tokenService)
        {
           _userRepository = userRepository;
           _tokenService = tokenService;
        }
        public async Task<LoginResponseDTO> Login(LoginRequestDTO request)
        {
            var user = await _userRepository.GetUserByUserName(request.Username);

            bool isValid = await _userRepository.CheckPassword(user, request.Password);

            if (user == null && isValid == false)
            {
                return new LoginResponseDTO()
                {
                    Token = {},
                    User = null
                };
            }

            var token = await _tokenService.GenerateJwtToken(user);

            UserDTO userDTO = new()
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                FullName = user.FirstName + user.LastName,
            };

            return new LoginResponseDTO()
            {
                Token = token,
                User = userDTO
            };
        }
    }
}

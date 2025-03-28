using MSClubInsights.Application.ServiceInterfaces;
using MSClubInsights.Domain.Entities.Identity;
using MSClubInsights.Domain.RepoInterfaces;
using MSClubInsights.Shared.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSClubInsights.Application.Services
{
    public class UserService : IUserService
    {
        private readonly iUserRepository _userRepository;
        public readonly ITokenService _tokenService;

        public UserService(iUserRepository userRepository, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        public async Task<LoginResponseDTO> Register(RegisterRequestDTO request)
        {
           var user = new AppUser()
           {
               UserName = request.Email,
               Email = request.Email,
               FirstName = request.FirstName,
               LastName = request.LastName,
               CityId = request.CityId,
               gender = request.gender,
               DateOfBirth = request.DateOfBirth,
               PhoneNumber = request.PhoneNumber,
           };

            var result = await _userRepository.CreateUser(user, request.Password);

            if (result.Succeeded)
            {
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

            return new LoginResponseDTO()
            {
                Token = null,
                User = null
            };
        }
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MSClubInsights.Domain.Entities.Identity;
using MSClubInsights.Infrastructure.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSClubInsights.Infrastructure.Persistence.Repositories
{
    public class UserRepository
    {
        private readonly UserManager<AppUser> _userManager;

        public UserRepository(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<AppUser> GetUserByEmail(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<AppUser> GetUserById(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task CreateUser(AppUser user , string password)
        {
            await _userManager.CreateAsync(user , password);
        }

        public async Task<bool> CheckPassword(AppUser user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<IList<string>> GetUserRoles(AppUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }
    }
}

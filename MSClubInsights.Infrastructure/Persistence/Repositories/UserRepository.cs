using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MSClubInsights.Domain.Entities.Identity;
using MSClubInsights.Domain.RepoInterfaces;
using MSClubInsights.Infrastructure.DB;
using MSClubInsights.Shared.Utitlites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSClubInsights.Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserRepository(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<AppUser> GetUserByEmail(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<AppUser> GetUserById(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<bool> CheckPassword(AppUser user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<IList<string>> GetUserRoles(AppUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<AppUser> GetUserByUserName(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            return user;
        }

        public async Task<IdentityResult> CreateUser(AppUser user, string password)
        {
            if (!_roleManager.RoleExistsAsync(SD.CoreTeam).GetAwaiter().GetResult())
            {
                await _roleManager.CreateAsync(new IdentityRole(SD.CoreTeam));
                await _roleManager.CreateAsync(new IdentityRole(SD.TechMember));
                await _roleManager.CreateAsync(new IdentityRole(SD.RegMember));
                await _roleManager.CreateAsync(new IdentityRole(SD.SysAdmin));
            }

            await _userManager.CreateAsync(user, password);

            await _userManager.AddToRoleAsync(user, SD.RegMember);

            return IdentityResult.Success;
        }
    }
}

using Microsoft.AspNetCore.Identity;
using MSClubInsights.Domain.Entities.Identity;
using MSClubInsights.Shared.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSClubInsights.Domain.RepoInterfaces
{
    public interface iUserRepository
    {
        Task<AppUser> GetUserByEmail(string email);

        Task<AppUser> GetUserById(string id);

        Task<AppUser> GetUserByUserName(string userName);

        Task<IdentityResult> CreateUser(AppUser user , string password);
        
        Task<bool> CheckPassword(AppUser user, string password);

        Task<IList<string>> GetUserRoles(AppUser user);
    }
}

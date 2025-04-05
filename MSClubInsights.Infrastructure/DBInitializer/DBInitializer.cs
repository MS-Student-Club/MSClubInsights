using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MSClubInsights.Domain.Entities;
using MSClubInsights.Domain.Entities.Identity;
using MSClubInsights.Infrastructure.DB;
using MSClubInsights.Shared.Utitlites;

namespace MSClubInsights.Infrastructure.DBInitializer
{
    public class DBInitializer : IDBInitializer
    {
        private readonly AppDbContext _db;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DBInitializer(AppDbContext db, RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            _db = db;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public void Initialize()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                    _db.Database.Migrate();
            }
            catch (Exception ex)
            {
                throw new Exception("Database migration failed", ex);
            }


            if (!_roleManager.RoleExistsAsync(SD.SysAdmin).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.SysAdmin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.TechMember)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.CoreTeam)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.RegMember)).GetAwaiter().GetResult();

                _db.cities.AddRange(new City { Name = "New York" },
                    new City {  Name = "Los Angeles" });


                _db.categories.AddRange(new Category { Name = "Machine Learning" },
                    new Category { Name = "Backend" });

                _db.tags.AddRange(new Tag { Name = "PyTorch" },
                    new Tag { Name = "C#" });

                _db.College.AddRange(
                    new College { Name = "Faculty of Computer Science"},
                    new College { Name = "Faculty of Engineering" }
                    );

                _db.Univeristy.AddRange(
                    new Univeristy { Name = "Suez Canal Univeristy"}
                    );

                _db.SaveChanges();


                _userManager.CreateAsync(new AppUser
                {
                    UserName = "admin@Microsoft.com",
                    Email = "admin@Microsoft.com",
                    FirstName = "Admin",
                    LastName = "Admin",
                    PhoneNumber = "1234567890",
                    CityId = 1,
                    EmailConfirmed = true,
                    DateOfBirth = DateOnly.Parse("1990-07-15"),
                    collegeId = 1,
                    univeristyId = 1,
                }, "Admin@123").GetAwaiter().GetResult();

                AppUser user = _db.users.FirstOrDefault(u => u.Email == "admin@Microsoft.com");
                _userManager.AddToRoleAsync(user, SD.SysAdmin).GetAwaiter().GetResult();



                return;
            }
        }
    }
}
        
    

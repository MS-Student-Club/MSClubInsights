using System.Text;
using System.Threading.RateLimiting;
using EntityFramework.Exceptions.SqlServer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MSClubInsights.Application.ServiceInterfaces;
using MSClubInsights.Application.Services;
using MSClubInsights.Domain.Entities.Identity;
using MSClubInsights.Domain.RepoInterfaces;
using MSClubInsights.Infrastructure.DB;
using MSClubInsights.Infrastructure.Persistence.Repositories;

namespace MSClubInsights.Infrastructure.DependancyInjection;

public static class ServiceContainer
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
    {
        string connectionString = "cs";
        
        //Adding DB Service.
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(config.GetConnectionString(connectionString));
        });
        
        services.AddDefaultIdentity<AppUser>(options =>
            {
                options.SignIn.RequireConfirmedEmail = true;
                options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                RequireExpirationTime = true,
                ValidIssuer = config["JWT:Issuer"],
                ValidAudience = config["JWT:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Key"]!))
            };
        });

        services.AddRateLimiter(rateLimiterOptions =>
        {
            rateLimiterOptions.AddFixedWindowLimiter("Auth", o =>
            {
                o.PermitLimit = 5;
                o.Window = TimeSpan.FromMinutes(1);
                o.QueueLimit = 1;
            });

            rateLimiterOptions.AddSlidingWindowLimiter("Public", o =>
            {
                o.PermitLimit = 100;
                o.Window = TimeSpan.FromMinutes(1);
                o.SegmentsPerWindow= 5;
                o.QueueLimit = 5;
            });

            rateLimiterOptions.AddTokenBucketLimiter("Modify", o =>
            {
                o.TokenLimit = 20;
                o.ReplenishmentPeriod = TimeSpan.FromSeconds(3);
                o.TokensPerPeriod = 1;
                o.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                o.QueueLimit = 5;
            });

            rateLimiterOptions.AddFixedWindowLimiter("Default", o =>
            {
                o.PermitLimit = 50;
                o.Window = TimeSpan.FromMinutes(1);
                o.QueueLimit = 1;
            });

            rateLimiterOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
        });

        services.AddScoped<ICityRepository , CityRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ITagRepository, TagRepository>();
        services.AddScoped<IArticleRepository, ArticleRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IArticleTagRepository, ArticleTagRepository>();
        services.AddScoped<ICommentRepository, CommentRepository>();
        services.AddScoped<ILikeRepository, LikeRepository>();
        services.AddScoped<IRatingRepository, RatingRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<ITagService, TagService>();
        services.AddScoped<IArticleService, ArticleService>();
        services.AddScoped<IArticleTagService, ArticleTagService>();
        services.AddScoped<ICommentService, CommentService>();
        services.AddScoped<IRatingService, RatingService>();
        services.AddScoped<ICityService, CityService>();
        services.AddScoped<ILikeService, LikeService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}
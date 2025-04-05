using System.Text;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MSClubInsights.Application.ServiceInterfaces;
using MSClubInsights.Application.Services;
using MSClubInsights.Domain.Entities.Identity;
using MSClubInsights.Domain.RepoInterfaces;
using MSClubInsights.Infrastructure.DB;
using MSClubInsights.Infrastructure.DBInitializer;
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
                options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = false;
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
            // Policy for Auth using IP-based fixed window limiting
            rateLimiterOptions.AddPolicy("Auth", httpContext => RateLimitPartition.GetFixedWindowLimiter(
                partitionKey: httpContext.Connection.RemoteIpAddress?.ToString(),
                factory: (partitionKey) => new FixedWindowRateLimiterOptions()
                {
                    PermitLimit = 5,
                    Window = TimeSpan.FromMinutes(1),
                    QueueLimit = 1
                }));

            // Policy for Public using IP-based sliding window limiting
            rateLimiterOptions.AddPolicy("Public", httpContext => RateLimitPartition.GetSlidingWindowLimiter(
                partitionKey: httpContext.Connection.RemoteIpAddress?.ToString(),
                factory: (partitionKey) => new SlidingWindowRateLimiterOptions()
                {
                    PermitLimit = 100,
                    Window = TimeSpan.FromMinutes(1),
                    SegmentsPerWindow = 5,
                    QueueLimit = 5
                }));

            // Policy for Modify using IP-based token bucket limiting
            rateLimiterOptions.AddPolicy("Modify", httpContext => RateLimitPartition.GetTokenBucketLimiter(
                partitionKey: httpContext.Connection.RemoteIpAddress?.ToString(),
                factory: (partitionKey) => new TokenBucketRateLimiterOptions()
                {
                    TokenLimit = 20,
                    ReplenishmentPeriod = TimeSpan.FromSeconds(3),
                    TokensPerPeriod = 1,
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                    QueueLimit = 5
                }));

            // Default fallback policy using IP-based fixed window limiting
            rateLimiterOptions.AddPolicy("Default", httpContext => RateLimitPartition.GetFixedWindowLimiter(
                partitionKey: httpContext.Connection.RemoteIpAddress?.ToString(),
                factory: (partitionKey) => new FixedWindowRateLimiterOptions()
                {
                    PermitLimit = 50,
                    Window = TimeSpan.FromMinutes(1),
                    QueueLimit = 1
                }));

            // Global rejection status code for rate-limited requests
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
        services.AddScoped<ICollegeRepository, CollegeRepository>();
        services.AddScoped<IUniveristyRepository, UniveristyRepository>();

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
        services.AddScoped<ICollegeService, CollegeService>();
        services.AddScoped<IUniveristyService, UniveristyService>();

        services.AddScoped<IDBInitializer, DBInitializer.DBInitializer>();


        return services;
  
    }
}
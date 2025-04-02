using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MSClubInsights.Domain.Entities.Identity;
using MSClubInsights.Infrastructure.DependancyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddInfrastructureServices(builder.Configuration);


//Adding CustomServices

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "MS Club Insights API ",
        Description = "A RESTful API to access insights and data related to Microsoft Student Club activities, events, and engagement.",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n" +
                       "Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\n" +
                       "Example: \"Bearer 12345abcdef\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Scheme = "Bearer",
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(o => {
    o.DocumentTitle = "MS Club Insights API";
    o.HeadContent = "<style>.swagger-ui .topbar { background-color: #5298DF; }</style>"; // Changes the header color
    o.InjectStylesheet("/swagger-custom.css"); // Inject custom CSS
    o.SwaggerEndpoint("/swagger/v1/swagger.json", "MS Club Insights API v1");
    o.RoutePrefix = string.Empty;
});


app.UseHttpsRedirection();
app.UseStaticFiles(); // Enables serving static files from wwwroot



using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<AppUser>>();
}

app.UseAuthentication();
app.UseAuthorization();
app.UseRateLimiter();
app.MapControllers();

app.Run();



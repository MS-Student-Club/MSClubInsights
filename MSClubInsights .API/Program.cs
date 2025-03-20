using Microsoft.AspNetCore.Identity;
using MSClubInsights.Domain.Entities.Identity;
using MSClubInsights.Infrastructure.DependancyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddInfrastructureServices(builder.Configuration);


//Adding CustomServices

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseSwaggerUI(o => o.SwaggerEndpoint("/openapi/v1.json", "MS Club Insights API v1"));


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<AppUser>>();
    SeedUser(userManager).Wait();
}
app.UseAuthorization();

app.MapControllers();

app.Run();

async Task SeedUser(UserManager<AppUser> userManager)
{
    if (await userManager.FindByEmailAsync("admin@example.com") == null)
    {
        var user = new AppUser { UserName = "admin", Email = "admin@example.com", EmailConfirmed = true };
        await userManager.CreateAsync(user, "Admin@123"); // Password
    }
}
